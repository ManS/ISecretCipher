using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISecretCipher.Model;
using SecretCipher.Model.Keys;
using SecretCipher.Model.Strategies.Encryption;
using SecretCipher.Model.Encryption;
using SecretCipher.Model;
using SecretCipher.Model.Strategies.Decryption;
using SecretCipher.Model.Decryption;
using SecretCipher.Model.Interfaces;

namespace ISecretCipher.Controller
{
    public class UserController : ICipherSubject
    {
        /// <summary>
        /// Gets or sets the observers.
        /// </summary>
        /// <value>The observers.</value>
        private List<IUserObserver> Observers { get; set; }
        public UserController()
        {
            Observers = new List<IUserObserver>();
        }
        /// <summary>
        /// Registers the specified p_user observer.
        /// </summary>
        /// <param name="p_userObserver">The p_user observer.</param>
        public void Register(IUserObserver p_userObserver)
        {
            Observers.Add(p_userObserver);
        }
        /// <summary>
        /// Uns the register.
        /// </summary>
        /// <param name="p_userObserver">The p_user observer.</param>
        public void UnRegister(IUserObserver p_userObserver)
        {
            Observers.Remove(p_userObserver);
        }

        #region ICipherSubject Methods

        /// <summary>
        /// Encrypts the ASCII.
        /// </summary>
        /// <param name="p_plainText">The p_plain text.</param>
        /// <param name="p_selectedAlgorithm">The p_selected algorithm.</param>
        /// <param name="p_key">The p_key.</param>
        public void EncryptASCII(string p_plainText, Utilities.ISecretCipherAlgorithms p_selectedAlgorithm, SecretCipher.Model.Keys.IKey p_key)
        {

            switch (p_key.ValidateKey())
            {
                case ValidationResponse.TooShortKey:
                    this.SendMessageToObservers("Too Short Key");
                    return;
                case ValidationResponse.TooLongKey:
                    this.SendMessageToObservers("Too Long Key");
                    return;
                case ValidationResponse.Sufficient:
                    break;
                case ValidationResponse.WrongFormat:
                    this.SendMessageToObservers("Key Wrong Format");
                    return;
                case ValidationResponse.InvalidKey:
                    this.SendMessageToObservers("Invalid Key");
                    return;
                default:
                    break;
            }
            ASCIIEncryptor MyEncryptor = null;

            switch (p_selectedAlgorithm)
            {
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.Caesar:
                    {
                        MyEncryptor = new ASCIIEncryptor(new CaesarEncryptor((CaesarKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.Monoalphabetic:
                    {
                        MyEncryptor = new ASCIIEncryptor(new MonoalphabeticEncryptor((MonoalphabeticKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.Vigenere:
                    {
                        MyEncryptor = new ASCIIEncryptor(new PolyalphabeticEncryptor((PolyalphabeticKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.PlayFair:
                    {
                        MyEncryptor = new ASCIIEncryptor(new PlayFairEncryptor((PlayFairKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.HillCipher:
                    {
                        MyEncryptor = new ASCIIEncryptor(new HillCipherEncryptor((HillCipherKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.RailFence:
                    {
                        MyEncryptor = new ASCIIEncryptor(new RailFenceEncryptorDecryptor((RailFenceKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.Columnar:
                    {
                        MyEncryptor = new ASCIIEncryptor(new ColumnarEncryptor((ColumnarKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.DES:
                    {
                        MyEncryptor = new ASCIIEncryptor(new DESEncryptor((DESKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.AES:
                    {
                        MyEncryptor = new ASCIIEncryptor(new AESEncryptor((AESKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.RC4:
                    {
                        MyEncryptor = new ASCIIEncryptor(new Rc4EncryptorDecryptor((Rc4Key)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.TripleDES:
                    {
                        MyEncryptor = new ASCIIEncryptor(new TripleDESEncryptor((TripleDESKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.RSA:
                    {
                        this.SendMessageToObservers("RSA Doesn't support ASCII Encryption");
                        return;
                    }
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.DeffieHellman:
                    {
                        this.SendMessageToObservers("RSA Doesn't support ASCII Encryption");
                        return;
                    }
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.EllipticCurve:
                    {
                        this.SendMessageToObservers("RSA Doesn't support ASCII Encryption");
                        return;
                    }
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.MulInverse:
                    {
                        this.SendMessageToObservers("RSA Doesn't support ASCII Encryption");
                        return;
                    }
                default:
                    break;
            }
            if (MyEncryptor != null)
            {
                string cipherText = MyEncryptor.EncryptMessage(p_plainText);
                foreach (IUserObserver Observer in Observers)
                {
                    Observer.OnMessageEncrypted(cipherText);
                }
            }
        }

        /// <summary>
        /// Sends the message to observers.
        /// </summary>
        /// <param name="p_msg">The P_MSG.</param>
        private void SendMessageToObservers(string p_msg)
        {
            foreach (IUserObserver observer in Observers)
            {
                observer.OnMessage(p_msg);
            }
        }

        /// <summary>
        /// Decrypts the ASCII.
        /// </summary>
        /// <param name="p_cipherText">The p_cipher text.</param>
        /// <param name="p_selectedAlgorithm">The p_selected algorithm.</param>
        /// <param name="p_key">The p_key.</param>
        public void DecryptASCII(string p_cipherText, Utilities.ISecretCipherAlgorithms p_selectedAlgorithm, SecretCipher.Model.Keys.IKey p_key)
        {

            switch (p_key.ValidateKey())
            {
                case ValidationResponse.TooShortKey:
                    this.SendMessageToObservers("Too Short Key");
                    return;
                case ValidationResponse.TooLongKey:
                    this.SendMessageToObservers("Too Long Key");
                    return;
                case ValidationResponse.Sufficient:
                    break;
                case ValidationResponse.WrongFormat:
                    this.SendMessageToObservers("Key Wrong Format");
                    return;
                case ValidationResponse.InvalidKey:
                    this.SendMessageToObservers("Invalid Key");
                    return;
                default:
                    break;
            }

            ASCIIDecryptor MyDecryptor = null;

            switch (p_selectedAlgorithm)
            {
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.Caesar:
                    {
                        MyDecryptor = new ASCIIDecryptor(new CaesarDecryptor((CaesarKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.Monoalphabetic:
                    {
                        MyDecryptor = new ASCIIDecryptor(new MonoalphabeticDecryptor((MonoalphabeticKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.Vigenere:
                    {
                        MyDecryptor = new ASCIIDecryptor(new PolyalphabeticDecryptor((PolyalphabeticKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.PlayFair:
                    {
                        MyDecryptor = new ASCIIDecryptor(new PlayFairDecryptor((PlayFairKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.HillCipher:
                    {
                        MyDecryptor = new ASCIIDecryptor(new HillCipherDecryptor((HillCipherKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.RailFence:
                    {
                        MyDecryptor = new ASCIIDecryptor(new RailFenceEncryptorDecryptor((RailFenceKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.Columnar:
                    {
                        MyDecryptor = new ASCIIDecryptor(new ColumnarDecryptor((ColumnarKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.DES:
                    {
                        MyDecryptor = new ASCIIDecryptor(new DESDecryptor((DESKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.AES:
                    {
                        MyDecryptor = new ASCIIDecryptor(new AESDecryptor((AESKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.RC4:
                    {
                        MyDecryptor = new ASCIIDecryptor(new Rc4EncryptorDecryptor((Rc4Key)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.TripleDES:
                    {
                        MyDecryptor = new ASCIIDecryptor(new TripleDESDecryptor((TripleDESKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.RSA:
                    this.SendMessageToObservers("RSA Doesn't support ASCII Decryption");
                    return;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.DeffieHellman:
                    this.SendMessageToObservers("DeffieHellman Doesn't support ASCII Decryption");
                    return;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.EllipticCurve:
                    this.SendMessageToObservers("EllipticCurve Doesn't support ASCII Decryption");
                    return;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.MulInverse:
                    this.SendMessageToObservers("MulInverse Doesn't support ASCII Decryption");
                    return;
                default:
                    return;
            }

            if (MyDecryptor != null)
            {
                string decryptedMsg =  MyDecryptor.DecryptMessage(p_cipherText);
                foreach (IUserObserver Observer in Observers)
                {
                    Observer.OnMessageDecrypted(decryptedMsg);
                }
            }
        }

        /// <summary>
        /// Encrypts the hex.
        /// </summary>
        /// <param name="p_plainHex">The p_plain hex.</param>
        /// <param name="p_selectedAlgorithm">The p_selected algorithm.</param>
        /// <param name="p_key">The p_key.</param>
        public void EncryptHex(string p_plainHex, Utilities.ISecretCipherAlgorithms p_selectedAlgorithm, SecretCipher.Model.Keys.IKey p_key)
        {
            HexEncryptor myEncryptor = null;
            switch (p_selectedAlgorithm)
            {

                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.DES:
                    {
                        myEncryptor = new HexEncryptor(new DESEncryptor((DESKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.AES:
                    {
                        myEncryptor = new HexEncryptor(new AESEncryptor((AESKey)p_key));
                    }
                    break;

                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.TripleDES:
                    {
                        myEncryptor = new HexEncryptor(new TripleDESEncryptor((TripleDESKey)p_key));
                    }
                    break;
                default:
                  this.SendMessageToObservers("This Algorithm Doesn't support Hex Encryption");
                    return;

            }

            if (myEncryptor != null)
            {
                string decryptedMsg = myEncryptor.EncryptHexMessage(p_plainHex);
                foreach (IUserObserver Observer in Observers)
                {
                    Observer.OnMessageDecrypted(decryptedMsg);
                }
            }
        }

        /// <summary>
        /// Decrypts the hex.
        /// </summary>
        /// <param name="p_cipherHex">The p_cipher hex.</param>
        /// <param name="p_selectedAlgorithm">The p_selected algorithm.</param>
        /// <param name="p_key">The p_key.</param>
        public void DecryptHex(string p_cipherHex, Utilities.ISecretCipherAlgorithms p_selectedAlgorithm, SecretCipher.Model.Keys.IKey p_key)
        {
            HexDecryptor myDecryptor = null;
            switch (p_selectedAlgorithm)
            {

                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.DES:
                    {
                        myDecryptor = new HexDecryptor(new DESDecryptor((DESKey)p_key));
                    }
                    break;
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.AES:
                    {
                        myDecryptor = new HexDecryptor(new AESDecryptor((AESKey)p_key));
                    }
                    break;

                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.TripleDES:
                    {
                        myDecryptor = new HexDecryptor(new TripleDESDecryptor((TripleDESKey)p_key));
                    }
                    break;
                default:
                    this.SendMessageToObservers("This Algorithm Doesn't support Hex Decryption");
                    return;

            }

            if (myDecryptor != null)
            {
                string decryptedMsg = myDecryptor.DecryptHexMessage(p_cipherHex);
                foreach (IUserObserver Observer in Observers)
                {
                    Observer.OnMessageDecrypted(decryptedMsg);
                }
            }
        }

        /// <summary>
        /// Encrypts the file.
        /// </summary>
        /// <param name="p_filePath">The p_file path.</param>
        /// <param name="p_saveToPath">The p_save to path.</param>
        /// <param name="p_selectedAlgorithm">The p_selected algorithm.</param>
        /// <param name="p_key">The p_key.</param>
        public void EncryptFile(string p_filePath, string p_saveToPath, Utilities.ISecretCipherAlgorithms p_selectedAlgorithm, SecretCipher.Model.Keys.IKey p_key)
        {
            FileEncryptor myEncryptor = null;
            switch (p_selectedAlgorithm)
            {
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.AES:
                    {
                        myEncryptor = new FileEncryptor(new AESEncryptor((AESKey)p_key));
                    }
                    break;
                default:
                    this.SendMessageToObservers("This Algorithm Doesn't support file encryption!");
                    break;
            }
            if (myEncryptor != null)
            {
                myEncryptor.EncryptFile(p_filePath, p_saveToPath);
                foreach (IUserObserver Observer in Observers)
                {
                    Observer.OnMessage("File Encrypted!");
                }
            }
        }

        /// <summary>
        /// Decrypts the file.
        /// </summary>
        /// <param name="p_cipherHex">The p_cipher hex.</param>
        /// <param name="p_saveToPath">The p_save to path.</param>
        /// <param name="p_selectedAlgorithm">The p_selected algorithm.</param>
        /// <param name="p_key">The p_key.</param>
        public void DecryptFile(string p_filePath, string p_saveToPath, Utilities.ISecretCipherAlgorithms p_selectedAlgorithm, SecretCipher.Model.Keys.IKey p_key)
        {
            FileDecryptor myDecryptor = null;
            switch (p_selectedAlgorithm)
            {
                case global::ISecretCipher.Utilities.ISecretCipherAlgorithms.AES:
                    {
                        myDecryptor = new FileDecryptor(new AESDecryptor((AESKey)p_key));
                    }
                    break;
                default:
                    this.SendMessageToObservers("This Algorithm Doesn't support file decryption!");
                    break;
            }
            if (myDecryptor != null)
            {
                myDecryptor.DecryptionStrategy.DecryptFile(p_filePath, p_saveToPath);
                foreach (IUserObserver Observer in Observers)
                {
                    Observer.OnMessage("File Decrypted!");
                }
            }
        }
        
        #endregion
    }
}
