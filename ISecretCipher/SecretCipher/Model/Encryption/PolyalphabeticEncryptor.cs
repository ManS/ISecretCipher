using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Utilities;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;

namespace SecretCipher.Model.Encryption
{
    public class PolyalphabeticEncryptor : IASCIIEncryptor
    {
        #region Attributes
        private PolyalphabeticKey m_key;
        private string m_PT;
        #endregion

        #region Properties
        public PolyalphabeticKey Key
        {
            get { return m_key; }
            set { m_key = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PolyalphabeticEncryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public PolyalphabeticEncryptor(PolyalphabeticKey p_key)
        {
            this.Key = p_key; UpperKey();
        }    
        #endregion
        public string EncryptMessage(string PlainText)
        {
            PlainText = PlainText.ToUpper();
            if (PlainText.Length > this.Key.Key.Length)
            {
                int sub = PlainText.Length - m_key.Key.Length;

                if (this.Key.KeyType ==  PolyKeyType.Repeat) // Repeating Key
                {
                    float dup = sub / m_key.Key.Length;
                    if ((sub % m_key.Key.Length) != 0)
                        dup++;
                    Duplicate((int)dup);
                }
                else // AutoKey
                {
                    int size = PlainText.Length;
                    char[] d_Key = new char[size];
                    for (int i = 0; i < m_key.Key.Length; i++)
                        d_Key[i] = m_key.Key[i];
                    for (int i = 0; i < sub; i++)
                        d_Key[i + m_key.Key.Length] = PlainText[i];
                    m_key.Key = new char[size];
                    m_key.Key = d_Key;
                }
            }

            byte[] p_DecryptedData = Toolbox.TextToByteArray(PlainText);
            byte[] Cipher = new byte[p_DecryptedData.Length];
            for (int i = 0; i < p_DecryptedData.Length; i++)
            {
                int cur = GetCorresponding((char)p_DecryptedData[i], m_key.Key[i]);
                cur += 65;
                Cipher[i] = (byte)cur;
            }
            return Toolbox.ByteArrayToText(Cipher);
        }
        private int GetGridIndex(char Let)
        {
            return (((int)Let) - 65);
        }
        private int GetCorresponding(char TP, char Key)
        {
            return ((GetGridIndex(TP)) + (GetGridIndex(Key))) % 26;
        }
        private void UpperKey()
        {
            for (int i = 0; i < m_key.Key.Length; i++)
            {
                if ((int)m_key.Key[i] <= 90 && (int)m_key.Key[i] >= 65)
                    continue;
                else
                    m_key.Key[i] = (char)((int)m_key.Key[i] - 32);
            }
        }
        private void Duplicate(int Count)
        {
            int _Size = m_key.Key.Length * (Count + 1);
            char[] d_Key = new char[_Size];
            for (int i = 0; i < _Size; i++)
            {
                d_Key[i] = m_key.Key[i % m_key.Key.Length];
            }
            m_key.Key = new char[_Size];
            m_key.Key = d_Key;
        }
    }
}
