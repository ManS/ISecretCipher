using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISecretCipher.Utilities
{
    public enum ISecretCipherAlgorithms
    {
        Caesar = 0,
        Monoalphabetic = 1,
        Vigenere = 2,
        PlayFair = 3,
        HillCipher = 4,
        RailFence = 5,
        Columnar = 6,
        DES = 7,
        AES = 8,
        RC4 = 9,
        TripleDES = 10,
        RSA = 11,
        DeffieHellman = 12,
        EllipticCurve = 13,
        MulInverse = 14
    }

    public enum InputType
    {
        ASCII,
        HEX,
        File
    }

    public enum Mode
    {
        Encryption,
        Decryption,
    }
}
