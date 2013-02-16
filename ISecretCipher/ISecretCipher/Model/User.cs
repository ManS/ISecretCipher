using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Strategies.Encryption;
using SecretCipher.Model.Strategies.Decryption;
using ISecretCipher.Controller;

namespace ISecretCipher.Model
{
    class User  : IUserSubject
    {
        public ASCIIEncryptor MyASCIIEncryptor { get; set; }
        public ASCIIDecryptor MyASCIIDecryptor { get; set; }
        public FileEncryptor MyFileEncryptor { get; set; }
        public FileDecryptor MyFileDecryptor { get; set; }
        public NumbersEncryptor MyNumbersEncryptor { get; set; }
        public NumbersDecryptor MyNumbersDecryptor { get; set; }
        public HexDecryptor MyHexDecryptor { get; set; }
        public HexEncryptor MyHexEncryptor { get; set; }



    }
}
