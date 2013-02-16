using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Keys;

namespace SecretCipher.Model.Keys
{
   public  enum PolyKeyType
    {
        Auto,
        Repeat
    }
    public class PolyalphabeticKey : IKey
    {

        /// <summary>
        /// Gets or sets the type of the key.
        /// </summary>
        /// <value>The type of the key.</value>
        public PolyKeyType KeyType { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public char []  Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolyalphabeticKey"/> class.
        /// </summary>
        /// <param name="p_keyType">Type of the p_key.</param>
        /// <param name="p_keyword">The p_keyword.</param>
        /// 

        public PolyalphabeticKey(PolyKeyType p_keyType, string p_key)
        {
            this.Key = p_key.ToCharArray();
            this.KeyType = p_keyType;
            //this.GenerateKey(p_keyType);
        }


        ///// <summary>
        ///// Generates the key.
        ///// </summary>
        ///// <param name="p_keytype">The p_keytype.</param>
        //private void GenerateKey(PolyKeyType p_keytype)
        //{
        //    switch (p_keytype)
        //    {
        //        case PolyKeyType.Auto:
        //            break;

        //        case PolyKeyType.Repeat:
        //            if (this.Keyword.Length > this.Key.Length)
        //                {
        //                    int sub = this.Keyword.Length -  this.Key.Length;
        //                    float dup = sub /  this.Key.Length;
        //                    if ((sub %  this.Key.Length) != 0)
        //                    dup++;
        //                    Duplicate((int)dup,this.Key);
        //                }
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private void Duplicate(int Count, char[] m_key)
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
        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <returns></returns>
        public ValidationResponse ValidateKey()
        {
            if (this.Key.GetLength(0)>0)
            return ValidationResponse.Sufficient;

            return ValidationResponse.InvalidKey;
        }
    }
}
