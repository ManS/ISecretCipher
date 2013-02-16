using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;

namespace SecretCipher.Model.Decryption
{
    public class RC4Decryptor : IASCIIDecryptor, IHexDecryptor, INumbersDecryptor
    {

        /// <summary>
        /// Decrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal DecryptNumber(decimal p_number)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decrypts the hex message.
        /// </summary>
        /// <param name="p_cipherHex">The p_cipher hex.</param>
        /// <returns></returns>
        public string DecryptHexMessage(string p_cipherHex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decrypts the message.
        /// </summary>
        /// <param name="p_cipherText">The p_cipher text.</param>
        /// <returns></returns>
        public string DecryptMessage(string p_cipherText)
        {
            throw new NotImplementedException();
        }
    }
}
