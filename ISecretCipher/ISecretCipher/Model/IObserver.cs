using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISecretCipher.Controller
{
    interface IUserObserver
    {
        void OnError(string p_errorMsg);

    }
}
