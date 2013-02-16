using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Utilities
{
    public class GaloisField : FiniteField
    {
        #region Properties
        /// <summary>
        /// Gets or sets the atable.
        /// </summary>
        /// <value>The atable.</value>
        static private byte[] atable { get; set; }
        
        /// <summary>
        /// Gets or sets the ltable.
        /// </summary>
        /// <value>The ltable.</value>
        static private byte[] ltable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [tables generated].
        /// </summary>
        /// <value><c>true</c> if [tables generated]; otherwise, <c>false</c>.</value>
        static private bool TablesGenerated { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Subs the specified p_value1.
        /// Addition and subtraction are performed by the exclusive or operation. 
        /// The two operations are the same; there is no difference between addition and subtraction.
        /// </summary>
        /// <param name="p_value1">The p_value1.</param>
        /// <param name="p_value2">The p_value2.</param>
        /// <returns></returns>
        static public byte Sub(byte p_value1, byte p_value2)
        {
            return (byte)(p_value1 ^ p_value2);
        }

        /// <summary>
        /// Adds the specified p_value1.
        /// Addition and subtraction are performed by the exclusive or operation. 
        /// The two operations are the same; there is no difference between addition and subtraction.
        /// </summary>
        /// <param name="p_value1">The p_value1.</param>
        /// <param name="p_value2">The p_value2.</param>
        /// <returns></returns>
        static public byte Add(byte p_value1, byte p_value2)
        {
            return (byte)(p_value1 ^ p_value2);
        }

        /// <summary>
        /// Muls the specified p_value1.
        /// </summary>
        /// <param name="p_value1">The p_value1.</param>
        /// <param name="p_valye2">The p_valye2.</param>
        /// <returns></returns>
        static public byte Mul(byte p_value1, byte p_valye2)
        {
            byte p = 0;
            byte hi_bit_set;

            for (int i = 0; i < 8; i++)
            {
                if ((p_valye2 & 1) == 1)
                    p ^= p_value1;
                hi_bit_set =(byte) (p_value1 & 0x80);
                p_value1 <<=1;

                if (hi_bit_set == 0x80)
                    p_value1 = (byte)(p_value1 ^ 0x11b);/* x^8 + x^4 + x^3 + x + 1 */
                
                p_valye2 >>= 1;

            }
            return p;
        }

        /// <summary>
        /// Rcons the specified p_value1.
        /// </summary>
        /// <param name="p_value1">The p_value1.</param>
        /// <returns></returns>
        static public byte Rcon(byte p_value1)
        {
            byte c = 1;
            if (p_value1 == 0)
                return 0;
            while (p_value1 != 1)
            {
                c = Mul(c, 2);
                p_value1--;
            }
            return c;
        }

        /// <summary>
        /// Generates the tables.
        /// </summary>
        static private void GenerateTables()
        {
            if (TablesGenerated)
            {
                return;
            }
            byte a = 1;
            byte d;
            atable = new byte[256];
            ltable = new byte[256];
            for (byte i = 0; i < 255; i++ )
            {
                atable[i] = a;
                /* Multiply by three */
                d = (byte)(a & 0x80);
                a <<= 1;
                if (d == 0x80)
                    a ^= 0x1b;
                a ^= atable[i];
                /* Set the log table value */
                ltable[atable[i]] = i;
            }
            atable[255] = atable[0];
            ltable[0] = 0;
            TablesGenerated = true;
        }

        /// <summary>
        /// Gets the mul inverse.
        /// </summary>
        /// <param name="p_value">The p_value.</param>
        /// <returns></returns>
        static public byte GetMulInverse(byte p_value)
        {
            if (!TablesGenerated)
            {
                GenerateTables();
            }
            if (p_value == 0)
                return 0;
            return atable[255 - ltable[p_value]];
        }
        #endregion
    }
}
