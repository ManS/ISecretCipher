using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;
using SecretCipher.Utilities;

namespace SecretCipher.Model.Decryption
{
   public class HillCipherDecryptor : IASCIIDecryptor
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public HillCipherKey Key { get; set; }
      

        /// <summary>
        /// Initializes a new instance of the <see cref="HillCipherDecryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public HillCipherDecryptor(HillCipherKey p_key)
        {
            this.Key = p_key;
        }
        private int GetTheMultiplicativeInverseEcludsAlgorithm(int number, int baseNumber)
        {
            int Q = 0, A1 = 1, A2 = 0, A3 = baseNumber, B1 = 0, B2 = 1, B3 = number;
            int newQ = 0, newA1 = 1, newA2 = 0, newA3 = baseNumber, newB1 = 0, newB2 = 1, newB3 = number;
            while (B3 != 1 && B3 != 0)
            {
                newQ = A3 / B3;
                newA1 = B1;
                newA2 = B2;
                newA3 = B3;
                newB1 = A1 - (newQ * B1);
                newB2 = A2 - (newQ * B2);
                newB3 = A3 - (newQ * B3);
                //------------------------------

                Q = newQ;
                A1 = newA1;
                A2 = newA2;
                A3 = newA3;
                B1 = newB1;
                B2 = newB2;
                B3 = newB3;
            }
            if (B3 == 0)
            {
              
                return -1;
            }
            else if (B3 == 1)
                return (int)GetTheModOfOneElement(B2, baseNumber);
            return -1;
        }
        private double[,] GetTheInverse(double[,] matrix, int mod)
        {
            //1)get the determinant.
           // double result = Math.Round(Matrix.Det(matrix));
            //double detMod = GetTheModOfOneElement(result, mod);
            double[,] CoMatrix = CalculateCoFactor(matrix);
            double[,] CoMatrixDet = GetTheMod(CoMatrix, mod);
            double[,] transposeCoMatrixDet = Matrix.Transpose(CoMatrixDet);
         //   int multiplicativeInverse = GetTheMultiplicativeInverseEcludsAlgorithm((int)detMod, mod);

           
            double[,] scalarMultiply = Matrix.ScalarMultiply(Key.multiplicativeInverse, transposeCoMatrixDet);
            scalarMultiply = GetTheMod(scalarMultiply, mod);
            //2)get the cofactor
            return scalarMultiply;

        }
        private double GetTheModOfOneElement(double number, int mod)
        {
            if (number < 0)
            {
                double temp = number + (((int)(number * -1) / mod) * mod);
                while (temp < 0)
                {
                    temp += mod;
                }
                number = temp % mod;
            }
            else
                number = number % mod;
            return number;

        }
        private double[,] GetTheMod(double[,] matrix, int mod)
        {
            int length = matrix.GetLength(0);
            int length2 = matrix.GetLength(1);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length2; j++)
                {
                    if (matrix[i, j] < 0)
                    {
                        double temp = matrix[i, j] + (((int)(matrix[i, j] * -1) / mod) * mod);
                        while (temp < 0)
                        {
                            temp += mod;
                        }
                        matrix[i, j] = temp % mod;
                    }
                    else
                        matrix[i, j] = matrix[i, j] % mod;
                }
            }
            return matrix;
        }

        private double[,] CalculateCoFactor(double[,] matrix)
        {
            int length = matrix.GetLength(0);
            double[,] newMatrix = new double[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    newMatrix[i, j] = GetSubMatrixByRemovingIJ(matrix, i, j);
                }
            }
            return newMatrix;
        }
        private double GetSubMatrixByRemovingIJ(double[,] matrix, int ii, int jj)
        {
            int length = matrix.GetLength(0);
            double[,] newMatrix = new double[length - 1, length - 1];
            int rowIndex = 0, columnIndex = 0;
            for (int i = 0; i < length; i++)
            {
                columnIndex = 0;
                for (int j = 0; j < length; j++)
                {
                    if (ii != i && jj != j)
                    {
                        newMatrix[rowIndex, columnIndex] = matrix[i, j];
                        columnIndex++;

                    }
                }
                if (columnIndex != 0)
                    rowIndex++;
            }
            return  Matrix.Det(newMatrix)*GetSign(ii,jj);
        }

       private int  GetSign(int i,int j)
       {
           if (i % 2 == 0)
           {
               if (j % 2 == 0)
                   return 1;
               else
                   return -1;
           }
           else
           {
               if (j % 2 == 0)
                   return -1;
               else
                   return +1;
           }
       }
        private void FillPlainText(ref byte[] plainText, double[,] subPlainText,int count)
        {
            int length = subPlainText.GetLength(0);
            int indexOfPlainText = count * Key.KeyMatrix.GetLength(0);
            int index = plainText.Length;

            for (int i = 0; i <  subPlainText.GetLength(0); i++,indexOfPlainText++)
            {
                plainText[indexOfPlainText] = (byte)(subPlainText[i, 0] + 65);
            }
        }
        private string ByteToString(byte[] arr)
        {
            string s = "";
            for (int i = 0; i < arr.Length; i++)
                s += (char)arr[i];
            return s;

        }
       private byte[]StringToByteArray(string text)
       {
           byte[] textInByes = new byte[text.Length];
           for (int i = 0; i < text.Length; i++)
           {
               textInByes[i] =(byte) text[i];
           }
           return textInByes;

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
       /// Decrypts the message.
       /// </summary>
       /// <param name="p_cipherText">The p_cipher text.</param>
       /// <returns></returns>
       public string DecryptMessage(string p_cipherText)
       {
           int rem = p_cipherText.Length % Key.KeyMatrix.GetLength(0);
           if (rem != 0)
           {
               int numberOFCharactersNeedToBePadded = Key.KeyMatrix.GetLength(0) - (rem);
               for (int i = 0; i < numberOFCharactersNeedToBePadded; i++)
                   p_cipherText += "X";
           }
           
           byte[] p_encryptedData = StringToByteArray(p_cipherText);
           int numberOfCharacters = Key.KeyMatrix.GetLength(0);
           byte[] plainText = new byte[p_cipherText.Length];

           double[,] keyInverse = GetTheInverse(ByteToDoubleArray(Key.KeyMatrix) ,26);

           int count = 0;
           for (int i = 0; i < p_cipherText.Length; i += numberOfCharacters)
           {
               double[,] subCipherText = new double[numberOfCharacters, 1];
               for (int k = i; k < i + numberOfCharacters; k++)
               {
                   subCipherText[k - i, 0] = p_encryptedData[k] - 65;

               }
               double[,] subPlainText = GetTheMod((Matrix.Multiply(keyInverse, subCipherText)), 26);
               FillPlainText(ref plainText, subPlainText,count);
               count++;
           }

           return ByteToString(plainText);
       }
    }
}
