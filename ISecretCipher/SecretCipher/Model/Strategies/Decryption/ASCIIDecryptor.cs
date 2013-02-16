using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;

namespace SecretCipher.Model.Strategies.Decryption
{
    public class ASCIIDecryptor
    {
        /// <summary>
        /// Gets or sets the decryption strategy.
        /// </summary>
        /// <value>The decryption strategy.</value>
        public IASCIIDecryptor DecryptionStrategy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASCIIDecryptor"/> class.
        /// </summary>
        /// <param name="p_strategy">The p_strategy.</param>
        public ASCIIDecryptor(IASCIIDecryptor p_strategy)
        {
            this.DecryptionStrategy = p_strategy;
        }

        /// <summary>
        /// Decrypts the message.
        /// </summary>
        /// <param name="p_cipherText">The p_cipher text.</param>
        /// <returns></returns>
        public string DecryptMessage(string p_cipherText)
        {
            return this.DecryptionStrategy.DecryptMessage(p_cipherText);
        }
    }
}
