using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;
using SecretCipher.Utilities;
using System.Numerics;

namespace SecretCipher.Model.Encryption
{
    public class RSAEncryptor : INumbersEncryptor
    {

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public RSAKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RSAEncryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public RSAEncryptor(RSAKey p_key)
        {
            this.Key = p_key;
        }

        /// <summary>
        /// Encrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal EncryptNumber(decimal p_number)
        {
            BigInteger CT = 0;
            int n = this.Key.P * this.Key.Q;
            int alphaN = (this.Key.P - 1) * (this.Key.Q - 1);
            BigInteger x = new BigInteger(p_number);
            BigInteger temp = Toolbox.power(x, this.Key.E);
            CT = temp % n;
            return (decimal)(CT);
        }

    }
}
