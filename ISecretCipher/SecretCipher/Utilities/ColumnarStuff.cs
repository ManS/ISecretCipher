using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Utilities
{
    struct column
    {
        public StringBuilder text;
        public int key;
        public column(StringBuilder t, int k)
        {
            text = t;
            key = k;
        }

    }

}