using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Model.Interfaces
{
    public interface IASCIIDecryptor
    {
        /// <summary>
        /// Decrypts the message.
        /// </summary>
        /// <param name="p_cipherText">The p_cipher text.</param>
        /// <returns></returns>
        string DecryptMessage(string p_cipherText);
    }
}
