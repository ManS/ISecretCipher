using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SecretCipher.Model.Interfaces
{
    public interface IFileDecryptor
    {
        /// <summary>
        /// Decrypts the file.
        /// </summary>
        /// <param name="p_encryptedFilePath">The encrypted file path.</param>
        /// <returns></returns>
        void  DecryptFile(string p_encryptedFilePath,string outPutFilePath);
    }
}
