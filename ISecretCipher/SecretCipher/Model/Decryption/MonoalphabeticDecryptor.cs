using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;

namespace SecretCipher.Model.Decryption
{
    public class MonoalphabeticDecryptor : IASCIIDecryptor
    {

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public MonoalphabeticKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoalphabeticDecryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public MonoalphabeticDecryptor(MonoalphabeticKey p_key)
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
            char[] cipher = this.Key.GetInverse();
            string decryptedMessage = "";
            p_cipherText = p_cipherText.ToUpper();
            for (int i = 0; i < p_cipherText.Length; i++)
            {
                decryptedMessage += cipher[((byte)p_cipherText[i]) - 65];
            }
            return decryptedMessage;
        }
    }
}
