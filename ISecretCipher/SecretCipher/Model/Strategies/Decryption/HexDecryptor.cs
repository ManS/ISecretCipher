using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;

namespace SecretCipher.Model.Strategies.Decryption
{
    public class HexDecryptor
    {

        /// <summary>
        /// Gets or sets the decryption strategy.
        /// </summary>
        /// <value>The decryption strategy.</value>
        public IHexDecryptor DecryptionStrategy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexDecryptor"/> class.
        /// </summary>
        /// <param name="p_strategy">The p_strategy.</param>
        public HexDecryptor(IHexDecryptor p_strategy)
        {
            this.DecryptionStrategy = p_strategy;

        }

        /// <summary>
        /// Decrypts the hex message.
        /// </summary>
        /// <param name="p_cipherHex">The p_cipher hex.</param>
        /// <returns></returns>
        public string DecryptHexMessage(string p_cipherHex)
        {
            return this.DecryptionStrategy.DecryptHexMessage(p_cipherHex);
        }
    }
}
