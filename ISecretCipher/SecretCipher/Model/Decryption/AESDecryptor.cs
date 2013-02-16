using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Utilities;
using System.IO;
using SecretCipher.Model.Keys;

namespace SecretCipher.Model.Decryption
{
    public class AESDecryptor : IAESAlgorithm, IFileDecryptor,IASCIIDecryptor,INumbersDecryptor,IHexDecryptor
    {

        #region Constrcutors
        /// <summary>
        /// Initializes a new instance of the <see cref="AESDecryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public AESDecryptor(AESKey p_key)
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
            Toolbox.GenerateISBox();
            for (int i = 0; i < p_inputBytes.Length; i++)
            {
                p_inputBytes[i] = Toolbox.ISBox[p_inputBytes[i].SecondWord, p_inputBytes[i].FirstWord];
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
            for (int i = 4, shiftVal = 1; i < p_inputBytes.Length; shiftVal++, i += 4)
            {
                for (int j = 0; j < shiftVal; j++)
                {
                    MyByte temp = p_inputBytes[i + 3];// end
                    for (int k = 2; k >= 0; k--)
                    {
                        p_inputBytes[i + k + 1] = p_inputBytes[i + k];
                    }
                    p_inputBytes[i] = temp;// beginning
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
                int j = 0;
                for (; j < 4; j++)
                {
                    currentCol[j] = p_inputBytes[j * 4 + i];
                }
                j = 0;
                result[j++ * 4 + i] = new MyByte((byte)(GaloisField.Mul(currentCol[0].Value, 14) ^ GaloisField.Mul(currentCol[3].Value, 9) ^ GaloisField.Mul(currentCol[2].Value, 13) ^ GaloisField.Mul(currentCol[1].Value, 11)));
                result[j++ * 4 + i] = new MyByte((byte)(GaloisField.Mul(currentCol[1].Value, 14) ^ GaloisField.Mul(currentCol[0].Value, 9) ^ GaloisField.Mul(currentCol[3].Value, 13) ^ GaloisField.Mul(currentCol[2].Value, 11)));
                result[j++ * 4 + i] = new MyByte((byte)(GaloisField.Mul(currentCol[2].Value, 14) ^ GaloisField.Mul(currentCol[1].Value, 9) ^ GaloisField.Mul(currentCol[0].Value, 13) ^ GaloisField.Mul(currentCol[3].Value, 11)));
                result[j++ * 4 + i] = new MyByte((byte)(GaloisField.Mul(currentCol[3].Value, 14) ^ GaloisField.Mul(currentCol[2].Value, 9) ^ GaloisField.Mul(currentCol[1].Value, 13) ^ GaloisField.Mul(currentCol[0].Value, 11)));

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
            MyByte[] decryptedData = new MyByte[p_inputBytes.Length];
            for (int i = 0; i < p_inputBytes.Length; i += 16)
            {
                MyByte[] subArray = new MyByte[16];
                Array.Copy(p_inputBytes, i, subArray, 0, 16);
                subArray = ApplyAESPipeline(subArray);
                subArray = Toolbox.Transpose(subArray);
                Array.Copy(subArray, 0, decryptedData, i, 16);
            }
            return decryptedData;
        }

        /// <summary>
        /// Applies the AES pipeline.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <returns></returns>
        protected override MyByte[] ApplyAESPipeline(MyByte[] p_inputBytes)
        {

            MyByte[] decryptedBytes = new MyByte[16];
            
            //[0] Add round key
            decryptedBytes = this.AddRoundKey(p_inputBytes, this.GetRoundKey(this.NumOfRounds));
            int i = this.NumOfRounds - 1;
            for (; i > 0; i--)
            {
                //Inverse Shift Rows
                decryptedBytes = this.ShiftRows(decryptedBytes);

                //Inverse Substitute bytes
                decryptedBytes = this.SubstituteBytes(decryptedBytes);
                
                //Add Round Key
                decryptedBytes = this.AddRoundKey(decryptedBytes, this.GetRoundKey(i));

                //Mix cols
                decryptedBytes = this.MixCols(decryptedBytes);

            }

            //final round

            //Inverse Shift Rows
            decryptedBytes = this.ShiftRows(decryptedBytes);

            //Inverse Substitute bytes
            decryptedBytes = this.SubstituteBytes(decryptedBytes);

            //Add Round Key
            decryptedBytes = this.AddRoundKey(decryptedBytes, this.GetRoundKey(i));

            return decryptedBytes;
        }
        #endregion

        #region IFileDecryptor, IASCIIDecryptor, INumbersDecryptor and IHexDecryptor Methods

        /// <summary>
        /// Decrypts the file.
        /// </summary>
        /// <param name="p_encryptedFilePath">The encrypted file path.</param>
        /// <returns></returns>
        public void DecryptFile(string p_filePath, string outputFilePath)
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
                byteArray = byteArray.Concat(Toolbox.ConvertMyByteToByteArray((ApplyAES(moiByte)).ToArray())).ToArray();


            }

            FileStream outputFileStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            outputFileStream.Write(byteArray, 0, byteArray.Length);
            outputFileStream.Close();

        }

        /// <summary>
        /// Decrypts the message.
        /// </summary>
        /// <param name="p_cipherText">The p_cipher text.</param>
        /// <returns></returns>
        public string DecryptMessage(string p_cipherText)
        {
            //this.Key.ExpandKey();
            while (p_cipherText.Length % 16 != 0)//padding
            {
                p_cipherText += "X";
            }
            return Toolbox.ConvertMyByteToString(this.ApplyAES(Toolbox.ConvertToMyByte(p_cipherText, KeyType.ASCII)));
        }

        /// <summary>
        /// Decrypts the nubmer.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal DecryptNumber(decimal p_number)
        {

            throw new NotImplementedException();
        }

        /// <summary>
        /// Decrypts the hex message.
        /// </summary>
        /// <param name="p_cipherHex">The p_cipher hex.</param>
        /// <returns></returns>
        public string DecryptHexMessage(string p_cipherHex)
        {
            int rem = p_cipherHex.Length % 32;
            if (rem != 0)
            {
                int numberOfHexsToBeAdded = 32 - rem;
                for (int i = 0; i < numberOfHexsToBeAdded; i++)
                    p_cipherHex += "A";
            }
            MyByte[] PlainData = Toolbox.ConvertToMyByte(p_cipherHex, KeyType.Hex);

            return Toolbox.MyByteToHex(this.ApplyAES(PlainData));
        }
        
        #endregion
    }
}
