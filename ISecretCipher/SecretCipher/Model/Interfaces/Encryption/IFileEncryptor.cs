using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SecretCipher.Model.Interfaces
{

    public interface IFileEncryptor
    {
        /// <summary>
        /// Encrypts the file.
        /// </summary>
        /// <param name="p_filePath">The p_file path.</param>
        /// <returns></returns>
        void EncryptFile(string p_filePath,string outputFilePath);
    }
}
