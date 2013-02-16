using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;

namespace SecretCipher.Model.Encryption
{
    public class CaesarEncryptor : IASCIIEncryptor, INumbersEncryptor
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public CaesarKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarEncryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public CaesarEncryptor(CaesarKey p_key)
        {
            this.Key = p_key;
        }

        /// <summary>
        /// Encrypts the message.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        public string EncryptMessage(string p_plainText)
        {
            return ApplyCaesar(p_plainText.ToLower(), 'a', 'z', (char)26);
        }

        /// <summary>
        /// Applies the caesar.
        /// </summary>
        /// <param name="p_cipherText">The p_cipher text.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns></returns>
        private string ApplyCaesar(string p_plainText, char min, char max, char divisor)
        {
            string encryptedMessage = "";
            for (int i = 0; i < p_plainText.Length; i++)
            {
                char encryptedChar = p_plainText[i];

                if (encryptedChar != ' ')
                {
                    encryptedChar += (char)this.Key.Key;
                    if (encryptedChar > max)
                    {
                        encryptedChar -= divisor;
                    }
                    if (encryptedChar < min)
                    {
                        encryptedChar += divisor;
                    }
                }
                encryptedMessage += encryptedChar.ToString();
            }

            return encryptedMessage;
        }

        /// <summary>
        /// Encrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal EncryptNumber(decimal p_number)
        {
            return decimal.Parse(ApplyCaesar(p_number.ToString(), '0', '9', (char)10));
        }
    }
}
