using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;
using SecretCipher.Utilities;

namespace SecretCipher.Model.Decryption
{
    /// <summary>
    /// DES Decryption type ( ASCII | HEXA )
    /// </summary>
    public enum DESDecryptionType
    {
        /// <summary>
        /// ASCII CT
        /// </summary>
        ASCIITYPE,
        /// <summary>
        /// HEXA CT
        /// </summary>
        HEXATYPE
    };

    public class DESDecryptor : IASCIIDecryptor, IHexDecryptor
    {

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public DESKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DESDecryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public DESDecryptor(DESKey p_key)
        {
            this.Key = p_key;
        }

        /// <summary>
        /// Decrypts the message.
        /// </summary>
        /// <param name="p_cipherText">(ASCII)The p_cipher text.</param>
        /// <returns>ASCII PlainText</returns>
        public string DecryptMessage(string p_cipherText)
        {
            string hex = Toolbox.ConvertToHex(p_cipherText);
            string binary = Toolbox.HexToBinary(hex);
            return Toolbox.HexToASCII(Decrypt(binary));
        }

        /// <summary>
        /// Decrypts the message.
        /// </summary>
        /// <param name="p_cipherText">(HEX) The p_cipher text.</param>
        /// <returns>HEX PlainText </returns>
        public string DecryptHexMessage(string p_cipherText)
        {
            string binary = Toolbox.HexToBinary(p_cipherText);
            return Decrypt(binary);
        }

        /// <summary>
        /// Decrypts the message.
        /// </summary>
        /// <param name="p_cipherText">(46-bits)The p_cipherTextBinary string.</param>
        /// <returns>HEX PlainText </returns>
        private string Decrypt(string p_cipherTextBinary)
        {
            byte[] CT = Toolbox.TextToByteArrayBinary(p_cipherTextBinary);
            byte[] ct_IP = DESUtilities.InitialPermutation(CT);


            byte[] itr = ct_IP;
            byte[] round = new byte[32];

            int size = (itr.Length / 2);
            byte[] R = new byte[size];
            byte[] L = new byte[size]; ;
            for (int j = 0; j < size; j++)
            {
                L[j] = itr[j];
                R[j] = itr[j + size];
            }

            for (int i = 0; i < 16; i++)
            {
                byte[] _R = R;
                round = DESUtilities.RoundFunction(R, this.Key.Keys[15 - i]);
                R = DESUtilities.XOR(L, round);
                L = _R;
            }

            byte[] final = DESUtilities.Concat(R, L);
            byte[] PT = DESUtilities.IPinv(final);

            string PTfinal = "";

            for (int i = 0; i < (PT.Length / 8); i++)
            {
                byte[] it = new byte[8];
                it = Toolbox.CopyAt(PT, i * 8, 8);
                PTfinal += Toolbox.BinaryToHex(Toolbox.BinaryBytesArrayToString(it));
            }
            return PTfinal;
        }

    }
}
