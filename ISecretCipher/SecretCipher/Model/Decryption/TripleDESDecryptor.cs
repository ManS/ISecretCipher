using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Utilities;
using SecretCipher.Model.Keys;
using SecretCipher.Model.Encryption;

namespace SecretCipher.Model.Decryption
{
    
    public class TripleDESDecryptor : IASCIIDecryptor, IHexDecryptor
    {

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public TripleDESKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TripleDESDecryptor"/> class.
        /// </summary>
        /// <param name="p_Tkeys">The p_ tkeys.</param>
        public TripleDESDecryptor(TripleDESKey p_key)
        {
            this.Key = p_key;
        }

        /// <summary>
        /// Decrypts the message.
        /// </summary>
        /// <param name="p_cipherText">The p_cipher text.</param>
        /// <param name="p_Mode">The p_ mode.</param>
        /// <returns></returns>
        public string DecryptMessage(string p_cipherText)
        {
            string Cipher = p_cipherText;
            if (this.Key.Mode == TripleDESMode.ThreekeysMode)
            {
                for (int i = 0; i < 3; i++)
                {
                    DESDecryptor DE = new DESDecryptor(this.Key.TripleKeys[2 - i]);
                    Cipher = DE.DecryptMessage(Cipher);
                }
            }
            else // Decryption(K1) -- Encryption(k2) -- Decryption(K1)
            {
                DESDecryptor DE_phaseOne = new DESDecryptor(this.Key.TripleKeys[0]);
                Cipher = DE_phaseOne.DecryptMessage(Cipher);

                DESEncryptor EN = new DESEncryptor(this.Key.TripleKeys[1]);
                Cipher = EN.EncryptMessage(Cipher);

                DESDecryptor DE_phaseTwo = new DESDecryptor(this.Key.TripleKeys[0]);
                Cipher = DE_phaseTwo.DecryptMessage(Cipher);

            }
            return Cipher;
        }

        /// <summary>
        /// Decrypts the hex message.
        /// </summary>
        /// <param name="p_cipherText">The p_cipher text.</param>
        /// <param name="p_Mode">The p_ mode.</param>
        /// <returns></returns>
        public string DecryptHexMessage(string p_cipherText)
        {
            string Cipher = p_cipherText;
            if (this.Key.Mode == TripleDESMode.ThreekeysMode)
            {
                for (int i = 0; i < 3; i++)
                {
                    DESDecryptor DE = new DESDecryptor(this.Key.TripleKeys[2 - i]);
                    Cipher = DE.DecryptHexMessage(Cipher);
                }
            }
            else // Decryption(K1) -- Encryption(k2) -- Decryption(K1)
            {
                DESDecryptor DE_phaseOne = new DESDecryptor(this.Key.TripleKeys[0]);
                Cipher = DE_phaseOne.DecryptHexMessage(Cipher);

                DESEncryptor EN = new DESEncryptor(this.Key.TripleKeys[1]);
                Cipher = EN.EncryptHexMessage(Cipher);

                DESDecryptor DE_phaseTwo = new DESDecryptor(this.Key.TripleKeys[0]);
                Cipher = DE_phaseTwo.DecryptHexMessage(Cipher);
            }
            return Cipher;
        }
    }
}
