using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;
using SecretCipher.Utilities;
using System.IO;

namespace SecretCipher.Model.Encryption
{
    public class AESEncryptor : IAESAlgorithm, IASCIIEncryptor, IFileEncryptor, IHexEncryptor, INumbersEncryptor
    {
        
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AESEncryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public AESEncryptor(AESKey p_key)
        {
            this.Key = p_key;
            this.ExpandedKey = this.Key.ExpandKey();
            switch (this.Key.Size)
            {
                case KeySize.x64Bits:
                    throw new NotSupportedException();
                case KeySize.x128Bits:
                    {
                        this.NumOfRounds = 10;
                        break;
                    }
                case KeySize.x192Bits:
                    {
                        this.NumOfRounds = 12;
                        break;
                    }
                case KeySize.x256Bits:
                    {
                        this.NumOfRounds = 14;
                        break;
                    }
                default:
                    break;
            }
        }
        #endregion

        #region IAESAlgorithm Methods

        /// <summary>
        /// Substitutes the bytes.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <returns></returns>
        protected override MyByte[] SubstituteBytes(MyByte[] p_inputBytes)
        {
            Toolbox.GenerateSBox();

            for (int i = 0; i < p_inputBytes.Length; i++)
            {
                p_inputBytes[i] = Toolbox.SBox[p_inputBytes[i].SecondWord, p_inputBytes[i].FirstWord];
            }
            return p_inputBytes;
        }

        /// <summary>
        /// Shifts the rows.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <returns></returns>
        protected override MyByte[] ShiftRows(MyByte[] p_inputBytes)
        {
            for (int i = 4, shiftVal = 1; i < p_inputBytes.Length; shiftVal++, i+=4 )
            {
                for (int j = 0; j < shiftVal; j++)
                {
                    MyByte temp = p_inputBytes[i];// beginning
                    for (int k = 0; k < 3; k++ )
                    {
                        p_inputBytes[i + k] = p_inputBytes[i + k + 1];
                    }
                    p_inputBytes[i + 3] = temp;// end
                }
            }

            return p_inputBytes;
        }

        /// <summary>
        /// Mixes the cols.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <returns></returns>
        protected override MyByte[] MixCols(MyByte[] p_inputBytes)
        {
            MyByte[] result = new MyByte[16];
            for (int i = 0; i < 4; i++)
            {
                MyByte[] currentCol = new MyByte[4];
                byte[] gfArray = new byte[4];
                int j = 0;
                for (; j < 4; j++)
                {
                    currentCol[j] = p_inputBytes[j * 4 + i];
                    gfArray[j] = GaloisField.Mul(currentCol[j].Value, 2);
                }
                j = 0;

                result[j++ * 4 + i] = new MyByte((byte)(gfArray[0] ^ currentCol[3].Value ^ currentCol[2].Value ^ gfArray[1] ^ currentCol[1].Value));
                result[j++ * 4 + i] = new MyByte((byte)(gfArray[1] ^ currentCol[0].Value ^ currentCol[3].Value ^ gfArray[2] ^ currentCol[2].Value));
                result[j++ * 4 + i] = new MyByte((byte)(gfArray[2] ^ currentCol[1].Value ^ currentCol[0].Value ^ gfArray[3] ^ currentCol[3].Value));
                result[j++ * 4 + i] = new MyByte((byte)(gfArray[3] ^ currentCol[2].Value ^ currentCol[1].Value ^ gfArray[0] ^ currentCol[0].Value));
            }
            return result;
        }

        /// <summary>
        /// Applies the AES.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <returns></returns>
        protected override MyByte[] ApplyAES(MyByte[] p_inputBytes)
        {
            MyByte[] encryptedData = new MyByte[p_inputBytes.Length];
            for (int i = 0; i < p_inputBytes.Length; i+=16)
            {
                MyByte[] subArray = new MyByte[16];
                
                Array.Copy(p_inputBytes, i, subArray, 0, 16);
                subArray = Toolbox.Transpose(subArray);
                subArray = ApplyAESPipeline(subArray);
                Array.Copy(subArray, 0, encryptedData, i, 16);
            }
            return encryptedData;
        }

        /// <summary>
        /// Applies the AES pipeline.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <returns></returns>
        protected override MyByte[] ApplyAESPipeline(MyByte[] p_inputBytes)
        {
            //[0] Add round key
            MyByte[] encryptedBytes = new MyByte[16];
            MyByte[] roundKey = this.GetRoundKey(0);
            encryptedBytes = this.AddRoundKey(p_inputBytes,roundKey );
       
            int i = 1;
            for (; i < this.NumOfRounds; i++ )
            {
                //Substitute bytes
                encryptedBytes = this.SubstituteBytes(encryptedBytes);
                
                //Shift Rows
                encryptedBytes = this.ShiftRows(encryptedBytes);

                //Mix cols
                encryptedBytes = this.MixCols(encryptedBytes);

                //Add Round Key
                encryptedBytes = this.AddRoundKey(encryptedBytes, this.GetRoundKey(i));

            }
            // final round

            //Substitute bytes
            encryptedBytes = this.SubstituteBytes(encryptedBytes);

            //Shift Rows
            encryptedBytes = this.ShiftRows(encryptedBytes);

            //Add Round Key
            encryptedBytes = this.AddRoundKey(encryptedBytes, this.GetRoundKey(i));
            
                   
            return encryptedBytes;
        }
        
        #endregion

        #region IFileEncyrptor, IASCIIEncyrptor, INumbersEncryptor and IHexEncryptor Methods

        /// <summary>
        /// Encrypts the hex message.
        /// </summary>
        /// <param name="p_hexMessage">The p_hex message.</param>
        /// <returns></returns>
        public string EncryptHexMessage(string p_hexMessage)
        {
            int rem = p_hexMessage.Length % 32;
            if (rem != 0)
            {
                int numberOfHexsToBeAdded = 32 - rem;
                for (int i = 0; i < numberOfHexsToBeAdded; i++)
                    p_hexMessage += "A";
            }
            MyByte[] PlainData = Toolbox.ConvertToMyByte(p_hexMessage, KeyType.Hex);
            MyByte[] res = this.ApplyAES(PlainData);

           // byte[] byteArray = new byte[0];
            //int numberOFDecryptions = PlainData.Length / 16;
            //string result = "";
            //for (int i = 0; i < numberOFDecryptions; i+=16)
            //{
            //    MyByte[] moiByte = new MyByte[16];
            //    Array.Copy(PlainData, i, moiByte, 0,16);

            //    MyByte[] res = ApplyAES(moiByte);
            //    //result += Toolbox.ByteArrayToHexString(Toolbox.ConvertMyByteToByteArray());
             
            //}
            //return res;
            return Toolbox.MyByteToHex(res);
        }

        /// <summary>
        /// Encrypts the file.
        /// </summary>
        /// <param name="p_filePath">The p_file path.</param>
        /// <returns></returns>
        public  void EncryptFile(string p_filePath,string outputFilePath)
        {

            FileStream p_encryptedFilePath = new FileStream(p_filePath, FileMode.Open, FileAccess.Read);
            byte[] buffer;

            try
            {
                int length = (int)p_encryptedFilePath.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = p_encryptedFilePath.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                p_encryptedFilePath.Close();
            }

            //padding
            int rem = buffer.Length % 16;

            if (rem != 0)
            {
                int numberOfBytesToBePadded = 16 - rem;
                byte[] newArr = new byte[numberOfBytesToBePadded];
                for (int i = 0; i < numberOfBytesToBePadded; i++)
                {
                    newArr[i] = (byte)'X';
                }
                buffer = (byte[])buffer.Concat(newArr).ToArray();

            }
            //Decrypting each 16
            byte[] byteArray = new byte[0];
            int numberOFDecryptions = buffer.Length / 16;
            for (int i = 0; i < numberOFDecryptions; i++)
            {
                MyByte[] moiByte = new MyByte[16];
                for (int j = 0; j < 16; j++)
                {

                    moiByte[j] = new MyByte(buffer[i * 16 + j]);
                }
            byteArray=byteArray.Concat(    Toolbox.ConvertMyByteToByteArray(  (ApplyAES(moiByte)).ToArray())).ToArray();


            }
            FileStream outputFileStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            outputFileStream.Write(byteArray, 0, byteArray.Length);
            outputFileStream.Close();
        }

        /// <summary>
        /// Encrypts the message.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        public string EncryptMessage(string p_plainText)
        {
            //this.Key.ExpandKey();
            while (p_plainText.Length%16 != 0)//padding
            {
                p_plainText += "x";
            }
            return Toolbox.ConvertMyByteToString(this.ApplyAES(Toolbox.ConvertToMyByte(p_plainText, KeyType.ASCII)));
        }

        /// <summary>
        /// Encrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal EncryptNumber(decimal p_number)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
