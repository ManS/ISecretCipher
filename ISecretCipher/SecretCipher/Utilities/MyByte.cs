using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SecretCipher.Utilities
{
    public enum KeyType
    {
        Hex,
        Binary,
        Integer,
        ASCII,
    }

    public class MyByte 
    {
        #region Attributes
        private byte m_firstWord;
        private byte m_secondWord;
        private BitArray m_myBits;
        private byte m_value;
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public byte Value
        {
            get { return m_value; }
            set {
                    m_value = value;
                    this.FirstWord = (byte)(m_value & 15);
                    this.SecondWord = (byte)(m_value & 240);
                    this.SecondWord >>= 4;
                    byte[] tempArr = { this.FirstWord, this.SecondWord };
                    this.m_myBits = new BitArray(tempArr);

                }
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public short Size { get; set; }

        /// <summary>
        /// Gets the first half.
        /// </summary>
        /// <value>The first half.</value>
        public byte FirstWord
        {
            get
            {
                return m_firstWord;
            }
            set
            {
                m_firstWord = value;
            }
        }
    
        /// <summary>
        /// Gets or sets my bits.
        /// </summary>
        /// <value>My bits.</value>
        public BitArray MyBits
        {
            get { return m_myBits; }
            set { m_myBits = value; }
        }

        /// <summary>
        /// Gets the second half.
        /// </summary>
        /// <value>The second half.</value>
        public byte SecondWord
        {
            get
            {
                return m_secondWord;
            }
            set
            {
                m_secondWord = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MyByte"/> class.
        /// </summary>
        public MyByte()
            : this(0)
        {
            //this.MyBits = new BitArray(8, false);
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyByte"/> class.
        /// </summary>
        /// <param name="p_keyString">The p_key string.</param>
        /// <param name="p_keyType">Type of the p_key.</param>
        public MyByte(string p_keyString, KeyType p_keyType)
        {
            if (Toolbox.GetKeySize(p_keyString,p_keyType) > 8)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.Size = 8;
            this.Value = Toolbox.ConvertToMyByte(p_keyString, p_keyType)[0].Value;
            this.FirstWord = (byte)(this.Value & 15);
            this.SecondWord = (byte)(this.Value & 240);
            this.SecondWord >>= 4;
            byte[] tempArr = { this.FirstWord, this.SecondWord };
            this.m_myBits = new BitArray(tempArr);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyByte"/> class.
        /// </summary>
        /// <param name="p_value">The p_value.</param>
        public MyByte(byte p_value)
        {
            this.Size = 8;
            this.Value = p_value;
            this.FirstWord = (byte)(p_value & 15);
            this.SecondWord = (byte)(p_value & 240);
            this.SecondWord >>= 4;
            byte[] tempArr = { this.FirstWord, this.SecondWord };
            this.m_myBits = new BitArray(tempArr);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Indexer<see cref="System.Boolean"/> with the specified i.
        /// </summary>
        /// <value></value>
        public bool this[int i]
        {
            get
            {
                if (i > 8 || i < 0)
                {
                    throw new IndexOutOfRangeException();
                }
                return this.MyBits[i];
            }
            set
            {
                if (i > 8 || i < 0)
                {
                    throw new IndexOutOfRangeException();
                }
                this.MyBits[i] = value;
            }
        }

        /// <summary>
        /// Overload the operator ^.(for the XOR operations)
        /// </summary>
        /// <param name="data1">The data1.</param>
        /// <param name="data2">The data2.</param>
        /// <returns>The result of the operator.</returns>
        public static MyByte operator ^(MyByte data1, MyByte data2)
        {
            if (data1 == null || data2 == null)
            {
                throw new ArgumentNullException();
            }

            if (data1.Size != data2.Size)
            {
                throw new ArgumentOutOfRangeException();
            }

            MyByte result = data1;
            for (int i = 0; i < data1.Size; i++)
            {
                result[i] ^= data2[i];
            }
            return result;
        }

        /// <summary>
        /// Overloads the operator |.
        /// </summary>
        /// <param name="data1">The data1.</param>
        /// <param name="data2">The data2.</param>
        /// <returns>The result of the operator.</returns>
        public static MyByte operator |(MyByte data1, MyByte data2)
        {
            if (data1 == null || data2 == null)
            {
                throw new ArgumentNullException();
            }
            if (data1.Size != data2.Size)
            {
                throw new ArgumentOutOfRangeException();
            }

            MyByte result = data1;
            for (int i = 0; i < result.Size; i++)
            {
                result[i] |= data2[i];
            }
            return result;
        }

        /// <summary>
        /// Overloads the operator -. (for
        /// </summary>
        /// <param name="data1">The data1.</param>
        /// <param name="data2">The data2.</param>
        /// <returns>The result of the operator.</returns>
        public static MyByte operator &(MyByte data1, MyByte data2)
        {
            if (data1 == null || data2 == null)
            {
                throw new ArgumentNullException();
            }
            if (data1.Size != data2.Size)
            {
                throw new ArgumentOutOfRangeException();
            }
            MyByte result = data1;
            for (int i = 0; i < result.Size; i++)
            {
                result[i] &= data2[i];
            }
            return result;
        }
        
        #endregion

    }
}
