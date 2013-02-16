using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;

namespace SecretCipher.Model.Strategies.Encryption
{
    public class HexEncryptor
    {

        /// <summary>
        /// Gets or sets the encryption strategy.
        /// </summary>
        /// <value>The encryption strategy.</value>
        public IHexEncryptor EncryptionStrategy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexEncryptor"/> class.
        /// </summary>
        /// <param name="p_strategy">The p_strategy.</param>
        public HexEncryptor(IHexEncryptor p_strategy)
        {
            this.EncryptionStrategy = p_strategy;
        }

        /// <summary>
        /// Encrypts the hex message.
        /// </summary>
        /// <param name="p_hexMessage">The p_hex message.</param>
        /// <returns></returns>
        public string EncryptHexMessage(string p_hexMessage)
        {
            return this.EncryptionStrategy.EncryptHexMessage(p_hexMessage);
        }
    }
}
