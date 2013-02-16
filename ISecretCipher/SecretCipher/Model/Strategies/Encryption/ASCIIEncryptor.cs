using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;

namespace SecretCipher.Model.Strategies.Encryption
{
    public class ASCIIEncryptor
    {
        /// <summary>
        /// Gets or sets the encryption strategy.
        /// </summary>
        /// <value>The encryption strategy.</value>
        public IASCIIEncryptor EncryptionStrategy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASCIIEncryptor"/> class.
        /// </summary>
        /// <param name="p_strategy">The p_strategy.</param>
        public ASCIIEncryptor(IASCIIEncryptor p_strategy)
        {
            this.EncryptionStrategy = p_strategy;
        }

        /// <summary>
        /// Encrypts the message.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        public string EncryptMessage(string p_plainText)
        {
            string msg = this.EncryptionStrategy.EncryptMessage(p_plainText);
            return msg;
        }
    }
}
