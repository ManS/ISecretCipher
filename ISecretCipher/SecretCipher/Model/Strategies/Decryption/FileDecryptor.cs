using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using System.IO;

namespace SecretCipher.Model.Strategies.Decryption
{
    public class FileDecryptor
    {
        /// <summary>
        /// Gets or sets the decryption strategy.
        /// </summary>
        /// <value>The decryption strategy.</value>
        public IFileDecryptor DecryptionStrategy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDecryptor"/> class.
        /// </summary>
        /// <param name="p_strategy">The p_strategy.</param>
        public FileDecryptor(IFileDecryptor p_strategy)
        {
            this.DecryptionStrategy = p_strategy;
        }

        /// <summary>
        /// Decrypts the file.
        /// </summary>
        /// <param name="p_encryptedFilePath">The p_encrypted file path.</param>
        /// <returns></returns>
        void DecryptFile(string p_encryptedFilePath,string outputFileStream)
        {
             this.DecryptionStrategy.DecryptFile(p_encryptedFilePath,outputFileStream);
        }
    }
}
