using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Model.Interfaces
{
    public interface INumbersDecryptor
    {
        /// <summary>
        /// Decrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        decimal DecryptNumber(decimal p_number);
    }
}
