using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Model.Keys
{
    public class MonoalphabeticKey : IKey
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public char[] KeyValues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoalphabeticKey"/> class.
        /// </summary>
        /// <param name="p_keyChars">The p_key chars.</param>
        public MonoalphabeticKey(char[] p_keyChars)
        {
            KeyValues = p_keyChars;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoalphabeticKey"/> class.
        /// Initialize with a random key.
        /// </summary>
        public MonoalphabeticKey()
        {
            this.GenerateRandomKey();
        }

        /// <summary>
        /// Generates the random key.
        /// </summary>
        private void GenerateRandomKey()
        {
            Dictionary<int, char> alpha = new Dictionary<int, char>();
            char currentChar = 'A';
            for (int i = 0; i < 26; i++ )
            {
                alpha.Add(i, currentChar++);
            }
            int randIndex;
            this.KeyValues = new char[26];
            Random random = new Random();
            for (int i = 0; i < 26; i ++ )
            {
                randIndex = random.Next(0, 25-i);
                this.KeyValues[i] = alpha[randIndex];
                alpha[randIndex] = alpha[25 - i];
            }
        }

        /// <summary>
        /// Gets the inverse.
        /// </summary>
        /// <returns></returns>
        public char[] GetInverse()
        {
            if (this.KeyValues == null)
            {
                throw new ArgumentNullException();
            }

            char[] inverseCihper = new char[26];
            for (byte i = 0; i < 26;i++ )
            {
                inverseCihper[((byte)this.KeyValues[i]) - 65] = (Char)('A' + (byte)i);
            }
            return inverseCihper;
        }

        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <returns></returns>
        public ValidationResponse ValidateKey()
        {
            if (this.KeyValues == null)
                throw new ArgumentNullException();
            for (int i = 0; i < 26; i++ )
            {
                if (KeyValues[i] < 'A' || KeyValues[i] > 'Z')
                    return  ValidationResponse.WrongFormat;
            }
            return   ValidationResponse.Sufficient;
        }
    }
}
