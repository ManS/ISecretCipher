using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISecretCipher.Controller
{
    public interface IUserObserver
    {
        void OnMessage(string p_Msg);
        void OnMessageEncrypted(string p_encryptedMsg);
        void OnMessageDecrypted(string p_decryptedMsg);
    }
}
