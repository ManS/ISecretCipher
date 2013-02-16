using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;
using SecretCipher.Utilities;
using System.Numerics;

namespace SecretCipher.Model.Decryption
{
    public class RSADecryptor : INumbersDecryptor
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public RSAKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RSADecryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public RSADecryptor(RSAKey p_key)
        {
            this.Key = p_key;
        }

        /// <summary>
        /// Decrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal DecryptNumber(decimal p_number)
        {
            BigInteger PT = 0;
            int n = this.Key.P * this.Key.Q;
            int alphaN = (this.Key.P - 1) * (this.Key.Q - 1);
            double d = Toolbox.GetInverseMod(alphaN, this.Key.E);
            BigInteger x = new BigInteger(p_number);
            BigInteger temp = Toolbox.power(x, d);
            PT = temp % n;
            return (decimal)(PT);
        }
    }
}
