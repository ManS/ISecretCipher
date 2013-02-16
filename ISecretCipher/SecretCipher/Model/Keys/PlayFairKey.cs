using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Model.Keys
{
    public class PlayFairKey : IKey
    {
        /// <summary>
        /// Gets or sets the keyword.
        /// </summary>
        /// <value>The keyword.</value>
        public string  Keyword { get; set; }

        /// <summary>
        /// Gets or sets the key matrix.
        /// </summary>
        /// <value>The key matrix.</value>
        public byte[,] KeyMatrix { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayFairKey"/> class.
        /// </summary>
        /// <param name="p_keyword">The p_keyword.</param>
        public PlayFairKey(string p_keyword)
        {
            this.Keyword = p_keyword.ToUpper();
            KeyMatrix = new byte[5, 5];
            this.GenerateMatrix();
        }

        /// <summary>
        /// Generates the matrix.
        /// </summary>
        private void GenerateMatrix()
        {
            bool[] AlphabitIsTaken = new bool[26];
            AlphabitIsTaken[74 - 65] = true;//J:D
            int index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (index < Keyword.Length)
                    {
                        if ((AlphabitIsTaken[Keyword[index] - 65] == false))
                        {
                            KeyMatrix[i, j] = (byte)Keyword[index];
                            AlphabitIsTaken[Keyword[index] - 65] = true;
                            index++;
                        }
                        else
                        {
                            index++;
                            if (j > 0)
                                j--;
                            else
                            {
                                i--;
                                j = 3;
                            }

                        }
                    }
                    else
                    {
                        for (int k = 0; k < 26; k++)
                        {
                            if (AlphabitIsTaken[k] == false)
                            {
                                KeyMatrix[i, j] = (byte)(k + 65);
                                AlphabitIsTaken[k] = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <returns></returns>
        public ValidationResponse ValidateKey()
        {

            return ValidationResponse.Sufficient;
        }
    }
}
