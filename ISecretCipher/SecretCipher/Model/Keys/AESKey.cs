using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Utilities;

namespace SecretCipher.Model.Keys
{

   public class AESKey : IKey
    {
        #region Properties
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public KeySize Size { get; set; }
        /// <summary>
        /// Gets or sets the key values.
        /// </summary>
        /// <value>The key values.</value>
        public MyByte[] KeyValues { get; set; }
        /// <summary>
        /// Gets or sets the key string.
        /// </summary>
        /// <value>The key string.</value>
        public string KeyString { get; set; }
        /// <summary>
        /// Gets or sets the type of the key.
        /// </summary>
        /// <value>The type of the key.</value>
        public KeyType KeyType { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AESKey"/> class.
        /// </summary>
        /// <param name="p_keySize">Size of the p_key.</param>
        /// <param name="p_keyString">The p_key string.</param>
        public AESKey(KeySize p_keySize, string p_keyString, KeyType p_keyType)
        {
            int si = Toolbox.GetKeySize(p_keyString, p_keyType);

            this.KeyValues = Toolbox.ConvertToMyByte(p_keyString, p_keyType);
            //Toolbox.Transpose(this.KeyValues);
            this.KeyType = p_keyType;
            this.KeyString = p_keyString;
            this.Size = p_keySize;
        }
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Expands the key.
        /// </summary>
        /// <returns></returns>
        public List<MyByte[]> ExpandKey()
        {
            if (this.KeyValues == null)
            {
                throw new ArgumentNullException();
            }

            switch (this.Size)
            {
                case KeySize.x64Bits:
                    throw new NotSupportedException();
                case KeySize.x128Bits:
                    return Expand128Key();
                case KeySize.x192Bits:
                    return Expand192Key();
                case KeySize.x256Bits:
                    return Expand256Key();
                default:
                    throw new NotSupportedException();
            }
        }
       
        /// <summary>
        /// Expands x256 keys.
        /// </summary>
        /// <returns></returns>
        private List<MyByte[]> Expand256Key()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Expands x192 keys.
        /// </summary>
        /// <returns></returns>
        private List<MyByte[]> Expand192Key()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Expands x128 keys.
        /// </summary>
        /// <returns>44 Word</returns>
        private List<MyByte[]> Expand128Key()
        {
            List<MyByte[]> ExpandedKey = new List<MyByte[]>();
            int k = 0;
            for (int i = 0; i < 16; i+=4 )
            {
                ExpandedKey.Add(new MyByte[4]);
                ExpandedKey[k++] = this.GetSubKey(i, i + 3);
            }
            MyByte[] tempWord = new MyByte[4];
            for (int i = 4; i < 44; i++ )
            {
                tempWord = ExpandedKey[i - 1];
                if (i%4 == 0)
                {
                    tempWord = Toolbox.CircularRotate(tempWord, 0, 3);// 1 byte circular left rotate
                    tempWord = Toolbox.ApplySBox(tempWord);
                    tempWord[0] = new MyByte((byte)(tempWord[0].Value ^ GaloisField.Rcon((byte)((i / 4))))); // XOR with Round Constant
                }
                ExpandedKey.Add(new MyByte[4]);
                //ExpandedKey[i] = tempWord;// for initialization
                ExpandedKey[i][0] = new MyByte((byte)(tempWord[0].Value ^ ExpandedKey[i - 4][0].Value));
                ExpandedKey[i][1] = new MyByte((byte)(tempWord[1].Value ^ ExpandedKey[i - 4][1].Value));
                ExpandedKey[i][2] = new MyByte((byte)(tempWord[2].Value ^ ExpandedKey[i - 4][2].Value));
                ExpandedKey[i][3] = new MyByte((byte)(tempWord[3].Value ^ ExpandedKey[i - 4][3].Value));
            }

            return ExpandedKey;
        }

        /// <summary>
        /// Gets the sub key.
        /// </summary>
        /// <param name="p_start">The p_start.</param>
        /// <param name="p_end">The p_end.</param>
        /// <returns></returns>
        private MyByte[] GetSubKey(int p_start, int p_end)
        {

            if (p_start<0||p_start>p_end||p_end>=(int)this.Size)
                throw new NotImplementedException();
            MyByte[] result = new MyByte[p_end - p_start + 1];
            for (int i = p_start,k=0; i <= p_end ; i++)
            {
                result[k++] = this.KeyValues[i];
            }
            return result;
        }

        #endregion
        
        #region IKey Methods

        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <returns></returns>
        public ValidationResponse ValidateKey()
        {
            if (this.KeyType == null || this.KeyString == null)
            {
                throw new ArgumentNullException();
            }
            if ((int)this.Size < Toolbox.GetKeySize(this.KeyString, this.KeyType))
            {
                return ValidationResponse.TooLongKey;
            }
            else if ((int)this.Size > Toolbox.GetKeySize(this.KeyString, this.KeyType))
                return ValidationResponse.TooShortKey;
            else
                return ValidationResponse.Sufficient;
        }
        #endregion
    }
}
