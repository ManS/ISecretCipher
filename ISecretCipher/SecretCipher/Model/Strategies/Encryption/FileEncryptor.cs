using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using System.IO;

namespace SecretCipher.Model.Strategies.Encryption
{
    public class FileEncryptor
    {
        /// <summary>
        /// Gets or sets the encryption strategy.
        /// </summary>
        /// <value>The encryption strategy.</value>
        public IFileEncryptor EncryptionStrategy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileEncryptor"/> class.
        /// </summary>
        /// <param name="p_strategy">The p_strategy.</param>
        public FileEncryptor(IFileEncryptor p_strategy)
        {
            this.EncryptionStrategy = p_strategy;
        }

        /// <summary>
        /// Encrypts the file.
        /// </summary>
        /// <param name="p_filePath">The p_file path.</param>
        /// <returns></returns>
        public void  EncryptFile(string p_filePath,string outputFilePath)
        {
             this.EncryptionStrategy.EncryptFile(p_filePath,outputFilePath);
        }
    }
}
