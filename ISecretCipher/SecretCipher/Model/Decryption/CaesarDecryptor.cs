using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;

namespace SecretCipher.Model.Decryption
{
    public class CaesarDecryptor : IASCIIDecryptor, INumbersDecryptor
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public CaesarKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarDecryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public CaesarDecryptor(CaesarKey p_key)
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
            return ApplyCaesar(p_cipherText.ToLower(), 'a', 'z', (char)26);
        }

        /// <summary>
        /// Applies the caesar.
        /// </summary>
        /// <param name="p_cipherText">The p_cipher text.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns></returns>
        private string ApplyCaesar(string p_cipherText, char min, char max, char divisor)
        {
            string decryptedMessage = "";
            for (int i = 0; i < p_cipherText.Length; i++)
            {
                char decryptedChar = p_cipherText[i];
                if (decryptedChar != ' ')
                {
                    decryptedChar -= (char)this.Key.Key;
                    if (decryptedChar > max)
                    {
                        decryptedChar -= divisor;
                    }
                    if (decryptedChar < min)
                    {
                        decryptedChar += divisor;
                    }
                }
                decryptedMessage += decryptedChar.ToString();
            }
            return decryptedMessage;
        }

        /// <summary>
        /// Decrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal DecryptNumber(decimal p_number)
        {
            return decimal.Parse(ApplyCaesar(p_number.ToString(), '0', '9', (char)10));
        }
    }
}
