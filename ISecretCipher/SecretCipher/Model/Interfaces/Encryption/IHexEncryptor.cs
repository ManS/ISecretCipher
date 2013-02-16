using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Model.Interfaces
{
    public interface IHexEncryptor
    {
        /// <summary>
        /// Encrypts the hex message.
        /// </summary>
        /// <param name="p_hexMessage">The p_hex message.</param>
        /// <returns></returns>
        string EncryptHexMessage(string p_hexMessage);
    }
}
