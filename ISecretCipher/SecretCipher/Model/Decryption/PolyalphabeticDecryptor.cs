using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Utilities;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;


namespace SecretCipher.Model.Encryption
{
    public class PolyalphabeticDecryptor : IASCIIDecryptor
    {
        #region Attributes
        private PolyalphabeticKey m_key;
        #endregion

        #region Properties
        public PolyalphabeticKey Key
        {
            get { return m_key; }
            set { m_key = value; }
        }
        #endregion

        #region Constructors
        public PolyalphabeticDecryptor(PolyalphabeticKey p_key)
        {
            this.Key = p_key; 
            UpperKey();
        }    
        #endregion

        private string DecryptAuto(string CipherText)
        {
            if (CipherText.Length > m_key.Key.Length)
            {
                string mykey = CharArrToString(m_key.Key);
                do
                {
                    string k = CharArrToString(GetFromArray(0, mykey.Length, CipherText));
                    string p = DecryptChararr(k, mykey.ToCharArray());
                    int lastupdate = p.Length - 1;
                    mykey += p[lastupdate];

                } while (CipherText.Length > mykey.Length);
                this.m_key.Key = mykey.ToCharArray();
                byte[] p_encryptedData = Toolbox.TextToByteArray(CipherText);
                byte[] PlainText = new byte[p_encryptedData.Length];
                for (int i = 0; i < p_encryptedData.Length; i++)
                {
                    int cur = (GetGridIndex((char)p_encryptedData[i]) - GetGridIndex(m_key.Key[i]));
                    if (cur < 0)
                        cur = 26 + cur;
                    cur += 65;
                    PlainText[i] = (byte)cur;
                }
                return Toolbox.ByteArrayToText(PlainText);
            }
            else
            {
                byte[] p_encryptedData = Toolbox.TextToByteArray(CipherText);
                byte[] PlainText = new byte[p_encryptedData.Length];
                for (int i = 0; i < p_encryptedData.Length; i++)
                {
                    int cur = (GetGridIndex((char)p_encryptedData[i]) - GetGridIndex(m_key.Key[i]));
                    if (cur < 0)
                        cur = 26 + cur;
                    cur += 65;
                    PlainText[i] = (byte)cur;
                }
                return Toolbox.ByteArrayToText(PlainText);
            }

        }
        private string DecryptRepating(string CipherText)
        {
            if (CipherText.Length > m_key.Key.Length)
            {
                int sub = CipherText.Length - m_key.Key.Length;
                float dup = sub / m_key.Key.Length;
                if ((sub % m_key.Key.Length) != 0)
                    dup++;
                Duplicate((int)dup);
            }
            byte[] p_encryptedData = Toolbox.TextToByteArray(CipherText);
            byte[] PlainText = new byte[p_encryptedData.Length];
            for (int i = 0; i < p_encryptedData.Length; i++)
            {
                int cur = (GetGridIndex((char)p_encryptedData[i]) - GetGridIndex(m_key.Key[i]));
                if (cur < 0)
                    cur = 26 + cur;
                cur += 65;
                PlainText[i] = (byte)cur;
            }
            return Toolbox.ByteArrayToText(PlainText);
        }
        public string DecryptMessage(string CipherText)
        {
            CipherText = CipherText.ToUpper();
            if (m_key.KeyType == PolyKeyType.Repeat) // Repeating
                return DecryptRepating(CipherText);
            else
                return DecryptAuto(CipherText);
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
        private int GetGridIndex(char Let)
        {
            return (((int)Let) - 65);
        }
        private string DecryptChararr(string CipherText, char[] mykey)
        {
            byte[] p_encryptedData = Toolbox.TextToByteArray(CipherText);
            byte[] PlainText = new byte[p_encryptedData.Length];
            for (int i = 0; i < p_encryptedData.Length; i++)
            {
                int cur = (GetGridIndex((char)p_encryptedData[i]) - GetGridIndex(mykey[i]));
                if (cur < 0)
                    cur = 26 + cur;
                cur += 65;
                PlainText[i] = (byte)cur;
            }
            return Toolbox.ByteArrayToText(PlainText);
        }
        private string CharArrToString(char[] arr)
        {
            string res = "";
            for (int i = 0; i < arr.Length; i++)
                res += arr[i];
            return res;
        }
        private char[] GetFromArray(int a, int b, string arr)
        {
            int size = (b - a);
            char[] ret = new char[size];
            for (int i = 0; i < size; i++)
                ret[i] = arr[a + i];
            return ret;
        }
    }
}
