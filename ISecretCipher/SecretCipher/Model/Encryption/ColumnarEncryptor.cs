using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;
using SecretCipher.Utilities;

namespace SecretCipher.Model.Encryption
{
    public class ColumnarEncryptor : IASCIIEncryptor, INumbersEncryptor
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public ColumnarKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnarEncryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public ColumnarEncryptor(ColumnarKey p_key)
        {
            Key = p_key;
        }


        /// <summary>
        /// Encryption function.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        string Encrypt(string p_plainText)
        {
            List<StringBuilder> shuffledStrings = new List<StringBuilder>();
            StringBuilder cipheredText = new StringBuilder();
            List<column> newShuffledStrings = new List<column>();

            int pos = 0;
            int n = 1;
            shuffledStrings.Add(new StringBuilder());
            for (int i = 0; i < p_plainText.Length; i++)
            {
                if (p_plainText[i] != ' ')
                {
                    shuffledStrings[pos].Append(p_plainText[i]);
                    n++;
                    if (n == Key.Sequence.Count + 1)
                    {
                        n = 1;
                        pos++;
                        shuffledStrings.Add(new StringBuilder());
                    }
                }
            }

            for (int i = 0; i < Key.Sequence.Count; i++)
            {
                StringBuilder s = new StringBuilder();
                for (int j = 0; j < shuffledStrings.Count; j++)
                {
                    try
                    {
                        s.Append(shuffledStrings[j][i]);
                    }
                    catch
                    {

                    }
                }

                newShuffledStrings.Add(new column(s, Key.Sequence[i]));
            }

            for (int i = 0; i < Key.Sequence.Count; i++)
            {
                for (int j = 0; j < newShuffledStrings.Count; j++)
                {
                    if (newShuffledStrings[j].key == i + 1)
                    {
                        cipheredText.Append(newShuffledStrings[j].text);
                        cipheredText.Append("\n");
                        break;

                    }
                }
            }

            return cipheredText.ToString();
        }

        /// <summary>
        /// Encrypts the message.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        public string EncryptMessage(string p_plainText)
        {
            return Encrypt(p_plainText);
        }

        /// <summary>
        /// Encrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal EncryptNumber(decimal p_number)
        {
            string p_plainText = p_number.ToString();
            return decimal.Parse(Encrypt(p_plainText));
        }
    }
}
