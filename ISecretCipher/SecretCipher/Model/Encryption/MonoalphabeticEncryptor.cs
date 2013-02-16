using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;

namespace SecretCipher.Model.Encryption
{
    public class MonoalphabeticEncryptor : IASCIIEncryptor
    {

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public MonoalphabeticKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoalphabeticEncryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public MonoalphabeticEncryptor(MonoalphabeticKey p_key)
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
            string encryptedMessage = "";
            p_plainText = p_plainText.ToUpper();
            for (int i = 0; i < p_plainText.Length;i++ )
            {
                encryptedMessage += this.Key.KeyValues[((byte)p_plainText[i]) - 65];
            }
            return encryptedMessage;
        }
    }
}
