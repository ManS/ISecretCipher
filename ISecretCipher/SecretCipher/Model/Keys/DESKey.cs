using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Utilities;

namespace SecretCipher.Model.Keys
{
    public class DESKey : IKey
    {
        /// <summary>
        /// Gets or sets the keyword.
        /// </summary>
        /// <value>The keyword.</value>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public Dictionary<int, byte[]> Keys { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public KeySize Size { get; set; }

        /// <summary>
        /// Gets or sets the type of the key.
        /// </summary>
        /// <value>The type of the key.</value>
        public KeyType KeyType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DESKey"/> class.
        /// </summary>
        /// <param name="p_keyword">The p_keyword.</param>
        public DESKey(string p_keyword, KeyType p_keytype)
        {
            this.Size = KeySize.x64Bits;
            if (Toolbox.GetKeySize(p_keyword, p_keytype) == (int)this.Size)
            {
                this.Keyword = p_keyword;
                this.KeyType = p_keytype;
                this.Keys = new Dictionary<int, byte[]>();
                this.GenerateKeys(p_keytype);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DESKey"/> class.
        /// </summary>
        /// <param name="p_keyword">The p_keyword.</param>
        public DESKey(string p_keyword)
        {
            this.Size = KeySize.x64Bits;
            if (Toolbox.GetKeySize(p_keyword, KeyType.ASCII) == (int)this.Size)
            {
                this.Keyword = p_keyword;
                this.KeyType = KeyType.ASCII;
                this.Keys = new Dictionary<int, byte[]>();
                this.GenerateKeys(Utilities.KeyType.ASCII);
            }
        }

        /// <summary>
        /// Generates the keys.
        /// </summary>
        private void GenerateKeys(KeyType p_keytype)
        {
            string binary = "";
            if (p_keytype == Utilities.KeyType.Hex)
            {
                binary = Toolbox.HexToBinary(this.Keyword);
            }

            if (p_keytype == Utilities.KeyType.ASCII)
            {
                string hex = Toolbox.ConvertToHex(this.Keyword);
                binary = Toolbox.HexToBinary(hex);
            }

            byte[] binarykey = Toolbox.TextToByteArrayBinary(binary);
            byte[] PC1key = DESUtilities.PC_1(binarykey);
            byte[] iteration = PC1key;
            int size = (PC1key.Length / 2);
            byte[] iterationR = new byte[size];
            byte[] iterationL = new byte[size];
            for (int i = 0; i < size; i++)
            {
                iterationL[i] = PC1key[i];
                iterationR[i] = PC1key[i + (PC1key.Length / 2)];
            }

            for (int i = 0; i < 16; i++)
            {

                if (HiddenData.RotateNumber[i] == 1)
                {
                    iterationL = Toolbox.TextToByteArrayBinary(DESUtilities.LCS_1(Toolbox.BinaryBytesArrayToString(iterationL)));
                    iterationR = Toolbox.TextToByteArrayBinary(DESUtilities.LCS_1(Toolbox.BinaryBytesArrayToString(iterationR)));
                    iteration = DESUtilities.Concat(iterationL, iterationR);
                    Keys.Add(i, DESUtilities.PC_2(iteration));

                }
                else
                {
                    iterationL = Toolbox.TextToByteArrayBinary(DESUtilities.LCS_2(Toolbox.BinaryBytesArrayToString(iterationL)));
                    iterationR = Toolbox.TextToByteArrayBinary(DESUtilities.LCS_2(Toolbox.BinaryBytesArrayToString(iterationR)));
                    iteration = DESUtilities.Concat(iterationL, iterationR);
                    Keys.Add(i, DESUtilities.PC_2(iteration));

                }
            }
        }

        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <returns></returns>
        public ValidationResponse ValidateKey()
        {
            this.Size = KeySize.x64Bits;
            int actualSize = Toolbox.GetKeySize(this.Keyword, this.KeyType);
            if (actualSize == (int)this.Size)
                return ValidationResponse.Sufficient;
            else if (actualSize < (int)this.Size)
                return ValidationResponse.TooShortKey;
            else
                return ValidationResponse.TooLongKey;
        }
    }
}
