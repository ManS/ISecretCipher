using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Utilities;

namespace secu
{
    class PolyalphabeticCipherEncryptor
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
        public PolyalphabeticCipherEncryptor(char[] p_key) { this.Key = p_key; UpperKey(); }    
        #endregion

        #region Methods
        public byte[] Encrypt(string PlainText)
        {
            if (PlainText.Length > m_key.Length)
            {
                int sub = PlainText.Length - m_key.Length;
                float dup = sub / m_key.Length;
                if ((sub % m_key.Length) != 0)
                    dup++;
                Duplicate((int)dup);
                
            }
            byte[] p_DecryptedData = Toolbox.TextToByteArray(PlainText.ToUpper());
            byte[] Chiper = new byte[p_DecryptedData.Length];
            for (int i = 0; i < p_DecryptedData.Length; i++)
            {
                int cur = GetCorresponding((char)p_DecryptedData[i], m_key[i]);
                cur += 65;
                Chiper[i] = (byte)cur;
            }
            return Chiper;
        }
        private int GetGridIndex(char Let) 
        {
            return (((int)Let) - 65);
        }
        private int GetCorresponding(char TP , char Key)
        {
            return ((GetGridIndex(TP)) + (GetGridIndex(Key)) )% 26;
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
        #endregion

    }
}
