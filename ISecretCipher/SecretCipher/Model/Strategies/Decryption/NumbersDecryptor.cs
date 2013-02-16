using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;

namespace SecretCipher.Model.Strategies.Decryption
{
    public class NumbersDecryptor
    {
        /// <summary>
        /// Gets or sets the decryption strategy.
        /// </summary>
        /// <value>The decryption strategy.</value>
        public INumbersDecryptor DecryptionStrategy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumbersDecryptor"/> class.
        /// </summary>
        /// <param name="p_decryptor">The p_decryptor.</param>
        public NumbersDecryptor(INumbersDecryptor p_decryptor)
        {
            this.DecryptionStrategy = p_decryptor;
        }

        /// <summary>
        /// Decrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal DecryptNumber(decimal p_number)
        {
            return this.DecryptionStrategy.DecryptNumber(p_number);
        }
    }
}
