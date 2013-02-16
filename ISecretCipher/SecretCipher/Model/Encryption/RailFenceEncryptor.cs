using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;

namespace SecretCipher.Model.Encryption
{
    public class RailFenceEncryptorDecryptor : IASCIIEncryptor, INumbersEncryptor, IASCIIDecryptor, INumbersDecryptor
    {
        #region Properties
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public RailFenceKey Key { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RailFenceEncryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public RailFenceEncryptorDecryptor(RailFenceKey p_key)
        {
            this.Key = p_key;
        }
        #endregion

        /// <summary>
        /// Applies the Rail Fence.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        private string ApplyRailFenceOnString(string p_plainText)
        {

            StringBuilder cipheredText = new StringBuilder();

            List<StringBuilder> chuffledStrings = new List<StringBuilder>();
            int position = 0;
            for (int i = 0; i < this.Key.DepthLevel; i++)
            {
                chuffledStrings.Add(new StringBuilder());
            }
            for (int i = 0; i < p_plainText.Length; i++)
            {
                if (p_plainText[i] != ' ')
                {
                    chuffledStrings[position].Append(p_plainText[i]);
                    position++;
                    if (position == this.Key.DepthLevel)
                        position = 0;
                }
            }
            for (int i = 0; i < this.Key.DepthLevel; i++)
                cipheredText.Append(chuffledStrings[i]);

            return cipheredText.ToString();

        }

        #region Decryption Methods
        /// <summary>
        /// Decrypts the message.
        /// </summary>
        /// <param name="p_cipherText">The p_cipher text.</param>
        /// <returns></returns>
        public string DecryptMessage(string p_cipherText)
        {
            int[] strLen = new int[Key.DepthLevel];
            for (int i = 0; i < Key.DepthLevel; i++)
                strLen[i] = (int)(p_cipherText.Length / Key.DepthLevel);
            int reminder = (p_cipherText.Length % Key.DepthLevel);
            for (int i = 0; i < reminder; i++)
                strLen[i]++;

            string pT = "";
            int k = 0;
            for (int i = 0; i < p_cipherText.Length; i++)
            {
                if (k < p_cipherText.Length)
                {
                    pT += p_cipherText[i];
                    k++;
                    int index = i;
                    for (int j = 1; j < Key.DepthLevel; j++)
                    {
                        index += strLen[j - 1];
                        if (index < p_cipherText.Length && k < p_cipherText.Length)
                        {
                            pT += p_cipherText[index];
                            k++;
                        }
                        else
                        {
                            return pT;
                        }
                    }
                }
                else
                    return pT;
            }
            return this.ApplyRailFenceOnString(p_cipherText);
        }

        /// <summary>
        /// Decrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal DecryptNumber(decimal p_number)
        {
            string p_plainText = p_number.ToString();
            return decimal.Parse(ApplyRailFenceOnString(p_plainText));
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
            return ApplyRailFenceOnString(p_plainText);
        }
        /// <summary>
        /// Encrypts the number.
        /// </summary>
        /// <param name="p_number">The p_number.</param>
        /// <returns></returns>
        public decimal EncryptNumber(decimal p_number)
        {
            string p_plainText = p_number.ToString();
            return decimal.Parse(ApplyRailFenceOnString(p_plainText));
        }

        #endregion

    }
}