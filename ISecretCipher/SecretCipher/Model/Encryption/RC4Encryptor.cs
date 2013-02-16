using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;

namespace SecretCipher.Model.Encryption
{
    public class RC4Encryptor : IASCIIEncryptor, IHexEncryptor ,INumbersEncryptor
    {
        /// <summary>
        /// Encrypts the message.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        public string EncryptMessage(string p_plainText)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Encrypts the hex message.
        /// </summary>
        /// <param name="p_hexMessage">The p_hex message.</param>
        /// <returns></returns>
        public string EncryptHexMessage(string p_hexMessage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Encrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal EncryptNumber(decimal p_number)
        {
            throw new NotImplementedException();
        }
    }
}
