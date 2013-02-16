using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;

namespace SecretCipher.Model.Decryption
{
    public class ColumnarDecryptor : IASCIIDecryptor, INumbersDecryptor
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public ColumnarKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnarDecryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public ColumnarDecryptor(ColumnarKey p_key)
        {
            this.Key = p_key;
        }

        /// <summary>
        /// Decrypts the message.
        /// </summary>
        /// <param name="p_cipherText">The p_cipher text.</param>
        /// <returns></returns>
        public string DecryptMessage(string p_cipherText)
        {
            string[] CipheredTexts = new string [Key.Sequence.Count];
            int[] numberOfRows = new int[Key.Sequence.Count];
            int reminder = p_cipherText.Length % Key.Sequence.Count;
          
            //min number of rows in any column
            for(int i=0;i<Key.Sequence.Count;i++)
                numberOfRows[i] = p_cipherText.Length / Key.Sequence.Count;

            //remaining letters
            for (int i = 0; i < reminder; i++)
                numberOfRows[Key.Sequence[i]-1] += 1;

            int x = 0;
            for (int i = 0; i < Key.Sequence.Count; i++)
            {
                for (int j = x; j < numberOfRows[i] + x; j++)
                    CipheredTexts[i] += p_cipherText[j];
                x += numberOfRows[i];
            }

            string plaintext = "";
            for (int i = 0; i < numberOfRows.Min(); i++)
            {
                for (int j = 0; j < Key.Sequence.Count; j++)
                {
                    plaintext += CipheredTexts[Key.Sequence[j] - 1][i];
                }
            }

            for (int i = 0; i < reminder; i++)
                plaintext += CipheredTexts[Key.Sequence[i] - 1 ][CipheredTexts[Key.Sequence[i] - 1].Length-1];

            return plaintext;
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
    }
}
