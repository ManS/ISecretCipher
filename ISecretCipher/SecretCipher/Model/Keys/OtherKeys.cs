using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Utilities;
namespace SecretCipher.Model.Keys
{
    public class CaesarKey : IKey
    {
        /// <summary>
        /// Gets or sets the key string.
        /// </summary>
        /// <value>The key string.</value>
        public int Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarKey"/> class.
        /// </summary>
        /// <param name="p_key">The p_key string.</param>
        public CaesarKey(int p_key)
        {
            this.Key = p_key;
        }

        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <returns></returns>
        public ValidationResponse ValidateKey()
        {
            if (this.Key < 1 || this.Key > 25)
            {
                return ValidationResponse.WrongFormat;
            }
            return ValidationResponse.Sufficient;
        }
    }

    public class RailFenceKey : IKey
    {
        /// <summary>
        /// Gets or sets the depth level.
        /// </summary>
        /// <value>The depth level.</value>
        public int DepthLevel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RailFenceKey"/> class.
        /// </summary>
        /// <param name="p_depthLevel">The p_depth level.</param>
        public RailFenceKey(int p_depthLevel)
        {
            this.DepthLevel = p_depthLevel;
        }

        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <returns></returns>
        public ValidationResponse ValidateKey()
        {
           if (DepthLevel < 1)
           {
               return ValidationResponse.WrongFormat;
           }
           return ValidationResponse.Sufficient;
        }
    }

    public class HillCipherKey : IKey
    {
        /// <summary>
        /// Gets or sets the size of the block.
        /// </summary>
        /// <value>The size of the block.</value>
        public int BlockSize { get; set; }

        /// <summary>
        /// Gets or sets the key word.
        /// </summary>
        /// <value>The key word.</value>
        public string KeyWord { get; set; }

        /// <summary>
        /// Gets or sets the key matrix.
        /// </summary>
        /// <value>The key matrix.</value>
        public byte[,] KeyMatrix { get; set; }

        public int multiplicativeInverse { get; set; }
        public   double detMod { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HillCipherKey"/> class.
        /// </summary>
        /// <param name="p_blockSize">Size of the p_block.</param>
        public HillCipherKey(int p_blockSize, string p_keyWord)
        {
            this.KeyWord=p_keyWord;
			//this.KeyWord.ToLower();
            this.BlockSize = p_blockSize;
            this.GenerateKeyMatrix();
            double result = Math.Round(Matrix.Det(ByteToDoubleArray(KeyMatrix)));
            detMod = GetTheModOfOneElement(result, 26);
            multiplicativeInverse = GetTheMultiplicativeInverseEcludsAlgorithm((int)detMod, 26);

        }

        /// <summary>
        /// Generates the key matrix.
        /// </summary>
        private void GenerateKeyMatrix()
        {
            this.KeyMatrix = new byte[this.BlockSize , this.BlockSize];
            for (int i = 0, k =0; i < this.BlockSize; i++ )
            {
                for (int j = 0; j < this.BlockSize;k++, j++ )
                {
                    this.KeyMatrix[i, j] = (byte)(this.KeyWord[k]-65);
                }
            }
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

        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <returns></returns>
        public ValidationResponse ValidateKey()
        {
 
      
            if (this.BlockSize < 2 ||multiplicativeInverse==-1)
            {
                return ValidationResponse.InvalidKey;
            }

            return ValidationResponse.Sufficient;
        }
    }

    public class RSAKey : IKey
    {

        /// <summary>
        /// Gets or sets the P.
        /// </summary>
        /// <value>The P.</value>
        public int P { get; set; }
   
        /// <summary>
        /// Gets or sets the Q.
        /// </summary>
        /// <value>The Q.</value>
        public int Q { get; set; }
 
        /// <summary>
        /// Gets or sets the E.
        /// </summary>
        /// <value>The E.</value>
        public int E { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RSAKey"/> class.
        /// </summary>
        /// <param name="p_p">The P_P.</param>
        /// <param name="p_q">The P_Q.</param>
        /// <param name="p_e">The p_e.</param>
        public RSAKey(int p_p, int p_q, int p_e)
        {
            this.P = p_p;
            this.E = p_e;
            this.Q = p_q;
        }

        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <returns></returns>
        public ValidationResponse ValidateKey()
        {
            throw new NotImplementedException();
        }
    }
}
