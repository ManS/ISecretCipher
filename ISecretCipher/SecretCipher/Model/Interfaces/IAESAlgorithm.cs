using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Keys;
using SecretCipher.Utilities;

namespace SecretCipher.Model.Interfaces
{
    abstract public class IAESAlgorithm
    {

        #region Attributes
        protected int m_numOfRounds;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public AESKey Key { get; set; }

        /// <summary>
        /// Gets or sets the expanded key.
        /// </summary>
        /// <value>The expanded key.</value>
        protected List<MyByte[]> ExpandedKey { get; set; }
        /// <summary>
        /// Gets or sets the num of rounds.
        /// </summary>
        /// <value>The num of rounds.</value>
        public int NumOfRounds
        {
            get { return m_numOfRounds; }
            set
            {
                if (value != 10 && value != 12 && value != 14)
                    throw new ArgumentOutOfRangeException();
                m_numOfRounds = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Substitutes the bytes.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <returns></returns>
        protected abstract MyByte[] SubstituteBytes(MyByte[] p_inputBytes);
   
        /// <summary>
        /// Shifts the rows.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <returns></returns>
        protected abstract MyByte[] ShiftRows(MyByte[] p_inputBytes);

        /// <summary>
        /// Mixes the cols.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <returns></returns>
        protected abstract MyByte[] MixCols(MyByte[] p_inputBytes);

        /// <summary>
        /// Applies the AES.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <returns></returns>
        protected abstract MyByte[] ApplyAES(MyByte[] p_inputBytes);

        /// <summary>
        /// Applies the AES pipeline.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <returns></returns>
        protected abstract MyByte[] ApplyAESPipeline(MyByte[] p_inputBytes);

        /// <summary>
        /// Adds the round key.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <param name="p_roundKey">The p_round key.</param>
        /// <returns></returns>
        protected MyByte[] AddRoundKey(MyByte[] p_inputBytes, MyByte[] p_roundKey)
        {
            MyByte[] resultBuffer = new MyByte[p_inputBytes.Length];
            for (int i = 0; i < p_inputBytes.Length; i++ )
            {
                resultBuffer[i] = new MyByte((byte)(p_inputBytes[i].Value ^ p_roundKey[i].Value));
            }
            return resultBuffer;
        }

        /// <summary>
        /// Gets the round key.
        /// </summary>
        /// <param name="p_roundNum">The p_round num.</param>
        /// <returns></returns>
        protected MyByte[] GetRoundKey(int p_roundNum)
        {
            MyByte[] roundKey = new MyByte[16];
            for (int i = p_roundNum * 4,k=0,j=0; i < p_roundNum * 4 + 4; i++)
            {
                roundKey[k++] = new MyByte(this.ExpandedKey[i][j++].Value);
                roundKey[k++] = new MyByte(this.ExpandedKey[i][j++].Value);
                roundKey[k++] = new MyByte(this.ExpandedKey[i][j++].Value);
                roundKey[k++] = new MyByte(this.ExpandedKey[i][j].Value);
                j = 0;
            }
            return Toolbox.Transpose(roundKey);
        }
        #endregion

    }
}
