using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;

namespace SecretCipher.Model.Strategies.Encryption
{
    public class NumbersEncryptor
    {
        /// <summary>
        /// Gets or sets the encryption strategy.
        /// </summary>
        /// <value>The encryption strategy.</value>
        public INumbersEncryptor EncryptionStrategy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumbersEncryptor"/> class.
        /// </summary>
        /// <param name="p_strategy">The p_strategy.</param>
        public NumbersEncryptor(INumbersEncryptor p_strategy)
        {
            this.EncryptionStrategy = p_strategy;
        }

        /// <summary>
        /// Encrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal EncryptNumber(decimal p_number)
        {
            return this.EncryptionStrategy.EncryptNumber(p_number);
        }
    }
}
