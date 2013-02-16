using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Utilities;
using SecretCipher.Model.Keys;

namespace SecretCipher.Model.Encryption
{
    public class DESEncryptor : IASCIIEncryptor, IHexEncryptor
    {

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public DESKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DESEncryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public DESEncryptor(DESKey p_key)
        {
            this.Key = p_key;
        }

        /// <summary>
        /// Encrypts the message.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        public string EncryptMessage(string p_plainText)
        {
            string hex = Toolbox.ConvertToHex(p_plainText);
            string binary = Toolbox.HexToBinary(hex);
            return Toolbox.HexToASCII(Encrypt(binary));
        }

        /// <summary>
        /// Encrypts the hex message.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <returns></returns>
        public string EncryptHexMessage(string p_plainText)
        {
            string binary = Toolbox.HexToBinary(p_plainText);
            return Encrypt(binary);
        }

        /// <summary>
        /// Encrypts the specified p_plain text binary.
        /// </summary>
        /// <param name="p_plainTextBinary">The p_plain text binary.</param>
        /// <returns></returns>
        private string Encrypt(string p_plainTextBinary)
        {
            byte[] PT = Toolbox.TextToByteArrayBinary(p_plainTextBinary);
            byte[] pt_IP = DESUtilities.InitialPermutation(PT);

            byte[] itr = pt_IP;
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
                round = DESUtilities.RoundFunction(R, this.Key.Keys[i]);
                R = DESUtilities.XOR(L, round);
                L = _R;
            }
            byte[] final = DESUtilities.Concat(R, L);
            byte[] CT = DESUtilities.IPinv(final);

            string CTfinal = "";

            for (int i = 0; i < (CT.Length / 8); i++)
            {
                byte[] it = new byte[8];
                it = Toolbox.CopyAt(CT, i * 8, 8);
                CTfinal += Toolbox.BinaryToHex(Toolbox.BinaryBytesArrayToString(it));
            }
            return CTfinal;
        }

    }
}
