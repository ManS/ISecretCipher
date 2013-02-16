using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Model.Interfaces
{
    public interface INumbersEncryptor
    {
        decimal EncryptNumber(decimal p_number);
    }
}
