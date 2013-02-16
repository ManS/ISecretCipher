using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Utilities;


namespace secu
{
    class PolyalphabeticCipherDecryptor
    {
        #region Attributes
        private char[] m_key;
        #endregion

        #region Properties
        public char[] Key
        {
            get { return m_key; }
            set { m_key = value; }
        }
        #endregion

        #region Constructors
        public PolyalphabeticCipherDecryptor(char[] p_key) { this.Key = p_key; UpperKey(); }    
        #endregion

        #region Methods
        public byte[] Decrypt(string CipherText)
        {
            #region Check Key
            if (CipherText.Length > m_key.Length)
            {
                int sub = CipherText.Length - m_key.Length;
                float dup = sub / m_key.Length;
                if ((sub % m_key.Length) != 0)
                    dup++;
                Duplicate((int)dup);
            }
            #endregion
            byte[] p_encryptedData = Toolbox.TextToByteArray(CipherText.ToUpper());
            byte[] PlainText = new byte[p_encryptedData.Length];
            for (int i = 0; i < p_encryptedData.Length; i++)
            {
                PlainText[i] = (byte)((int)p_encryptedData[i] - GetGridIndex(m_key[i]));
            }
            return PlainText;
        }
        private void UpperKey()
        {
            for (int i = 0; i < m_key.Length; i++)
            {
                if ((int)m_key[i] <= 90 && (int)m_key[i] >= 65)
                    continue;
                else
                    m_key[i] = (char)((int)m_key[i] - 32);
            }
        }
        private void Duplicate(int Count)
        {
            int _Size = m_key.Length * (Count + 1);
            char[] d_Key = new char[_Size];
            for (int i = 0; i < _Size; i++)
            {
                d_Key[i] = m_key[i % m_key.Length];
            }
            m_key = new char[_Size];
            m_key = d_Key;
        }
        private int GetGridIndex(char Let)
        {
            return (((int)Let) - 65);
        }
        #endregion
    }
}
