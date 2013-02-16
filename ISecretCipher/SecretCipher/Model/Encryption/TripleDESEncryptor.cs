using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Utilities;
using SecretCipher.Model.Keys;
using SecretCipher.Model.Encryption;
using SecretCipher.Model.Decryption;

namespace SecretCipher.Model.Encryption
{
   
    public class TripleDESEncryptor : IASCIIEncryptor, IHexEncryptor
    {

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public TripleDESKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TripleDESEncryptor"/> class.
        /// </summary>
        /// <param name="p_Tkeys">The p_ tkeys.</param>
        public TripleDESEncryptor(TripleDESKey p_key)
        {
            this.Key = p_key;
        }

        /// <summary>
        /// Encrypts the message.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        public string EncryptMessage(string p_plainText)
        {
            string Plain = p_plainText;
            if (this.Key.Mode == TripleDESMode.ThreekeysMode)
            {
                for (int i = 0; i < 3; i++)
                {
                    DESEncryptor EN = new DESEncryptor(this.Key.TripleKeys[i]);
                    Plain = EN.EncryptMessage(Plain);
                }
            }
            else // Encryption(k1) -- Decryption(K2) -- Encryption(k1)
            {
                DESEncryptor EN_phaseOne = new DESEncryptor(this.Key.TripleKeys[0]);
                Plain = EN_phaseOne.EncryptMessage(Plain);

                DESDecryptor DE = new DESDecryptor(this.Key.TripleKeys[1]);
                Plain = DE.DecryptMessage(Plain);

                DESEncryptor EN_phaseTwo = new DESEncryptor(this.Key.TripleKeys[0]);
                Plain = EN_phaseTwo.EncryptMessage(Plain);
            }
            return Plain;
        }

        /// <summary>
        /// Encrypts the hex message.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        public string EncryptHexMessage(string p_plainText)
        {
            string Plain = p_plainText;
            if (this.Key.Mode == TripleDESMode.ThreekeysMode)
            {
                for (int i = 0; i < 3; i++)
                {
                    DESEncryptor EN = new DESEncryptor(this.Key.TripleKeys[i]);
                    Plain = EN.EncryptHexMessage(Plain);
                }
            }
            else
            {
                DESEncryptor EN_phaseOne = new DESEncryptor(this.Key.TripleKeys[0]);
                Plain = EN_phaseOne.EncryptHexMessage(Plain);

                DESDecryptor DE = new DESDecryptor(this.Key.TripleKeys[1]);
                Plain = DE.DecryptHexMessage(Plain);

                DESEncryptor EN_phaseTwo = new DESEncryptor(this.Key.TripleKeys[0]);
                Plain = EN_phaseTwo.EncryptHexMessage(Plain);

            }
            return Plain;
        }

        


    }
}
