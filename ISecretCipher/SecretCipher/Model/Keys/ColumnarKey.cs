using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Model.Keys
{
    public class ColumnarKey : IKey
    {
        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>The sequence.</value>
        public List<int> Sequence { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnarKey"/> class.
        /// </summary>
        /// <param name="p_sequence">The p_sequence.</param>
        /// <param name="p_Pt">The p_ pt.</param>
        public ColumnarKey(List<int> p_sequence)
        {
            this.Sequence = p_sequence;
        }

        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <returns></returns>
        public ValidationResponse ValidateKey()
        {
            if (this.Sequence == null)
            {
                throw new ArgumentNullException();
            }
            if (this.Sequence.Count < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            int count;
            for (int i = 0; i < this.Sequence.Count; i++)
            {
                count = 0;
                for (int k = 0; k < this.Sequence.Count; k++)
                {
                    if (this.Sequence[k] == this.Sequence[i])
                        count++;
                    if (count > 1)
                        return ValidationResponse.WrongFormat;
                }
            }
            return ValidationResponse.Sufficient;
        }
    }
}
