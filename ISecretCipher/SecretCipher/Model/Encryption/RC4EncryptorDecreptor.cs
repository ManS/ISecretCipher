using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;
using SecretCipher.Utilities;

namespace SecretCipher.Model.Encryption
{
    public class Rc4EncryptorDecryptor : IASCIIEncryptor, INumbersEncryptor, IASCIIDecryptor, INumbersDecryptor
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Rc4Key Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rc4EncryptorDecryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public Rc4EncryptorDecryptor(Rc4Key p_key)
        {
            this.Key = p_key;
        }

        /// <summary>
        /// Applies the Rc4.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        private string ApplyRc4(string p_plainText)
        {

            int[] S = new int[256];
            int[] T = new int[256];
           Toolbox.ShuffleKey (Key.Sequence,S, T);
            string CipheredText = "";
            int x = 0, y = 0;
            for (int i = 0; i < p_plainText.Length; i++)
            {
                x = (x + 1) % 256;
                y = (y + S[x]) % 256;
                int temp = S[x];
                S[y] = S[x];
                S[x] = temp;
                int t = (S[x] + S[y]) % 256;
                int CurrentKey = S[t];
                int currentstring = CurrentKey ^ Convert.ToByte(p_plainText[i]);

                CipheredText += Convert.ToChar(currentstring);
            }
            return CipheredText;

        }

        #region Decryption Methods
        /// <summary>
        /// Decrypts the message.
        /// </summary>
        /// <param name="p_cipherText">The p_cipher text.</param>
        /// <returns></returns>
        public string DecryptMessage(string p_cipherText)
        {
            return this.ApplyRc4(p_cipherText);
        }

        /// <summary>
        /// Decrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal DecryptNumber(decimal p_number)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Encryption Methods
        /// <summary>
        /// Encrypts the message.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        public string EncryptMessage(string p_plainText)
        {
            return ApplyRc4(p_plainText);
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

        #endregion

    }
}
