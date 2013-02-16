using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;
using SecretCipher.Utilities;
namespace SecretCipher.Model.Encryption
{
    public class HillCipherEncryptor : IASCIIEncryptor
    {

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public HillCipherKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HillCipherEncryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public HillCipherEncryptor(HillCipherKey p_key)
        {
            this.Key = p_key;




        }
        private byte[] StringToByteArray(string text)
        {
            byte[] textInByes = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                textInByes[i] = (byte)text[i];
            }
            return textInByes;

        }
        
        private void FillCipherText(double[,] subCipherText,ref byte[]cipherText,int count)
        {
            int length = subCipherText.GetLength(0);

            int index = cipherText.Length;
            int indexOfCipher = count * Key.KeyMatrix.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                cipherText[indexOfCipher]= (byte)(subCipherText[i,0] + 65);
                index++;
                indexOfCipher++;
            }
        }



        private double[,] GetTheMod(double[,] Matrix, int modNumber)
        {
            int numberOfRows = Matrix.GetLength(0);
            int numberOfcolumns = Matrix.GetLength(1);
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfcolumns; j++)
                {
                    Matrix[i, j] = Matrix[i, j] % modNumber;
                }
            }
            return Matrix;
        }
        private string ByteToString(byte[] arr)
        {
            string s = "";
            for (int i = 0; i < arr.Length; i++)
                s += (char)arr[i];
            return s;

        }

        private double[,] ByteToDoubleArray(byte[,] byteArray)
        {

            int rows = byteArray.GetLength(0);
            int columns = byteArray.GetLength(1);
            double[,] doubleArray = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    doubleArray[i, j] = byteArray[i, j];
                }
            }
            return doubleArray;
        }


        /// <summary>
        /// Encrypts the message.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        public string EncryptMessage(string p_plainText)
        {
            int rem=p_plainText.Length % Key.KeyMatrix.GetLength(0);
            if (rem != 0)
            {
                int numberOFCharactersNeedToBePadded = Key.KeyMatrix.GetLength(0) - (rem);
                for (int i = 0; i < numberOFCharactersNeedToBePadded; i++)
                    p_plainText += "X";
            }
            byte[] p_plainData = StringToByteArray(p_plainText);
            byte[] cipherText = new byte[p_plainData.Length];

            int numberOfCharacters = Key.KeyMatrix.GetLength(0);
            int count = 0;
            for (int i = 0; i < p_plainData.Length; i += numberOfCharacters)
            {
                double[,] subPlainText = new double[numberOfCharacters, 1];
                double[,] subCipherText;
                for (int k = i; k < i + numberOfCharacters; k++)
                {
                    subPlainText[k - i, 0] = p_plainData[k] - 65;
                }
                double[,] result = Matrix.Multiply(ByteToDoubleArray(Key.KeyMatrix), subPlainText);
                subCipherText = GetTheMod(result, 26);
                FillCipherText(subCipherText,ref cipherText,count);
                count++;



            }
            return ByteToString(cipherText);
        }
    }
}
