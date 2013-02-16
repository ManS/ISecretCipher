using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Model.Keys
{
    public enum KeySize
    {
        x64Bits = 64,
        x128Bits = 128,
        x192Bits = 192,
        x256Bits = 256
    }
    
    public enum ValidationResponse
    {
        TooShortKey,
        TooLongKey,
        Sufficient,
        WrongFormat,
        InvalidKey
    }
    
    public interface IKey
    {
        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <returns></returns>
        ValidationResponse ValidateKey();
    }
}
