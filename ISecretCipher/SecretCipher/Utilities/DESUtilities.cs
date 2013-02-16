using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Utilities
{
    public class DESUtilities
    {

        /// <summary>
        /// Initials the permutation.
        /// </summary>
        /// <param name="m_PT">The m_ PT.</param>
        /// <returns></returns>
        static public byte[] InitialPermutation(byte[] m_PT)
        {
            byte[] _ptip = new byte[m_PT.Length];
            for (int i = 0; i < m_PT.Length; i++)
                _ptip[i] = m_PT[HiddenData.IP[i] - 1];
            return _ptip;
        }
        static public byte[] PC_1(byte[] binarykey)
        {
            byte[] PC1key = new byte[56];
            for (int i = 0; i < 56; i++)
                PC1key[i] = binarykey[HiddenData.PermutationChoiceOne[i] - 1];
            return PC1key;
        }
        static public byte[] PC_2(byte[] binarykey)
        {
            byte[] PC2key = new byte[48];
            for (int i = 0; i < 48; i++)
                PC2key[i] = binarykey[HiddenData.PermutationChoiceTwo[i] - 1];
            return PC2key;
        }
        static public byte[] IPinv(byte[] m_)
        {
            byte[] _Pinv = new byte[64];
            for (int i = 0; i < 64; i++)
                _Pinv[i] = m_[HiddenData.IPInverse[i] - 1];
            return _Pinv;
        }
        /// <summary>
        /// Expansions the permutation.
        /// </summary>
        /// <param name="binarykey">The binarykey.</param>
        /// <returns></returns>
        static public byte[] ExpansionPermutation(byte[] binarykey)
        {
            byte[] Ekey = new byte[48];
            for (int i = 0; i < 48; i++)
                Ekey[i] = binarykey[HiddenData.KeyExpansion[i] - 1];
            return Ekey;
        }
        /// <summary>
        /// Permutations the specified binarykey.
        /// </summary>
        /// <param name="binarykey">The binarykey.</param>
        /// <returns></returns>
        static public byte[] Permutation(byte[] binarykey)
        {
            byte[] Per = new byte[32];
            for (int i = 0; i < 32; i++)
                Per[i] = binarykey[HiddenData.Permutation[i] - 1];
            return Per;
        }
        /// <summary>
        /// Concats the specified L.
        /// </summary>
        /// <param name="L">The L.</param>
        /// <param name="R">The R.</param>
        /// <returns></returns>
        static public byte[] Concat(byte[] L, byte[] R)
        {
            byte[] array = new byte[L.Length + R.Length];
            Array.Copy(L, 0, array, 0, L.Length);
            Array.Copy(R, 0, array, L.Length, R.Length);
            return array;
        }
        static public string LCS_1(string S)
        {
            string res = "";
            for (int i = 1; i < S.Length; i++)
                res += S[i];
            res += S[0];
            return res;
        }
        static public string LCS_2(string S)
        {
            string res = "";
            for (int i = 2; i < S.Length; i++)
                res += S[i];
            res += S[0];
            res += S[1];
            return res;
        }
        /// <summary>
        /// XORs the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="B">The B.</param>
        /// <returns></returns>
        static public byte[] XOR(byte[] A, byte[] B)
        {
            byte[] C = new byte[A.Length];
            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] == B[i])
                    C[i] = 0;
                else
                    C[i] = 1;
            }
            return C;
        }
        /// <summary>
        /// Rounds function.
        /// </summary>
        /// <param name="R">The R.</param>
        /// <param name="p_Key">The p_ key.</param>
        /// <returns></returns>
        static public byte[] RoundFunction(byte[] R, byte[] p_Key)
        {
            byte[] EP = ExpansionPermutation(R);
            byte[] EPxorKey = XOR(EP, p_Key);
            string S = "";
            for (int i = 0; i < (EPxorKey.Length / 6); i++)
            {
                byte[] it = new byte[6];
                it = Toolbox.CopyAt(EPxorKey, i * 6, 6);
                int Rownum = Toolbox.BinaryToInt(it[0].ToString() + it[5].ToString());
                int Colnum = Toolbox.BinaryToInt(it[1].ToString() + it[2].ToString() + it[3].ToString() + it[4].ToString());
                switch (i)
                {
                    case 0:
                        S += Toolbox.IntToBinary(HiddenData.SBox1[Rownum, Colnum]);
                        break;
                    case 1:
                        S += Toolbox.IntToBinary(HiddenData.SBox2[Rownum, Colnum]);
                        break;
                    case 2:
                        S += Toolbox.IntToBinary(HiddenData.SBox3[Rownum, Colnum]);
                        break;
                    case 3:
                        S += Toolbox.IntToBinary(HiddenData.SBox4[Rownum, Colnum]);
                        break;
                    case 4:
                        S += Toolbox.IntToBinary(HiddenData.SBox5[Rownum, Colnum]);
                        break;
                    case 5:
                        S += Toolbox.IntToBinary(HiddenData.SBox6[Rownum, Colnum]);
                        break;
                    case 6:
                        S += Toolbox.IntToBinary(HiddenData.SBox7[Rownum, Colnum]);
                        break;
                    case 7:
                        S += Toolbox.IntToBinary(HiddenData.SBox8[Rownum, Colnum]);
                        break;
                }
            }

            byte[] SP = Permutation(Toolbox.TextToByteArrayBinary(S));
            return SP;
        }
    }
}