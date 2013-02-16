using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Model.Interfaces
{
    public interface IASCIIEncryptor
    {
        /// <summary>
        /// Encrypts the message.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        string EncryptMessage(string p_plainText);
    }
}
