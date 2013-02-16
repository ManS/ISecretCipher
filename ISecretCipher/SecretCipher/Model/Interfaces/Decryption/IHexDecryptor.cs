using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Model.Interfaces
{
    public interface IHexDecryptor
    {
        /// <summary>
        /// Decrypts the hex message.
        /// </summary>
        /// <param name="p_cipherHex">The p_cipher hex.</param>
        /// <returns></returns>
        string DecryptHexMessage(string p_cipherHex);
    }
}
