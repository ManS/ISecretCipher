using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISecretCipher.Utilities;
using ISecretCipher.Controller;
using SecretCipher.Model.Keys;

namespace ISecretCipher.Model
{
    interface ICipherSubject
    {
        void Register(IUserObserver p_userObserver);
        void UnRegister(IUserObserver p_userObserver);
        void EncryptASCII(string p_plainText, ISecretCipherAlgorithms p_selectedAlgorithm, IKey p_key);
        void DecryptASCII(string p_cipherText, ISecretCipherAlgorithms p_selectedAlgorithm, IKey p_key);
        void EncryptHex(string p_plainHex, ISecretCipherAlgorithms p_selectedAlgorithm, IKey p_key);
        void DecryptHex(string p_cipherHex, ISecretCipherAlgorithms p_selectedAlgorithm, IKey p_key);
        void EncryptFile(string p_filePath,string p_saveToPath, ISecretCipherAlgorithms p_selectedAlgorithm, IKey p_key);
        void DecryptFile(string p_filePath, string p_saveToPath, ISecretCipherAlgorithms p_selectedAlgorithm, IKey p_key);
    }
}
