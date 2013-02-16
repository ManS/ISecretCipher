using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Model.Keys
{
    public class Rc4Key : IKey
    {
        
        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>The sequence.</value>
        public List<char> Sequence { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnarKey"/> class.
        /// </summary>
        /// <param name="p_sequence">The p_sequence.</param>
        /// <param name="p_Pt">The p_ pt.</param>
        public Rc4Key(List<char> p_sequence)
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
           
            return ValidationResponse.Sufficient;
        }
    }
}
