using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Model.Keys
{
    public enum TripleDESMode
    {
        ThreekeysMode,
        TwoKeysMode
    };

    public class TripleDESKey : IKey
    {

        /// <summary>
        /// Gets or sets the triple keys.
        /// </summary>
        /// <value>The triple keys.</value>
        public DESKey[] TripleKeys { get; set; }

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        public TripleDESMode Mode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TripleDESKey"/> class.
        /// </summary>
        /// <param name="p_keys">The p_keys.</param>
        /// <param name="p_mode">The p_mode.</param>
        public TripleDESKey(DESKey[] p_keys, TripleDESMode p_mode)
        {
            this.TripleKeys = p_keys;
            this.Mode = p_mode;
        }

        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <returns></returns>
        public ValidationResponse ValidateKey()
        {
            for (int i = 0; i < this.TripleKeys.Length; i++ )
            {
                if (this.TripleKeys[i].ValidateKey() == ValidationResponse.Sufficient)
                {
                    continue;
                }
                else
                    return ValidationResponse.InvalidKey;
            }
            return ValidationResponse.Sufficient;
        }
    }
}
