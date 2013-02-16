using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace SecretCipher.Utilities
{
    public enum StringType 
    {
        Hex,
        Binary,
        ASCII,
    }

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

    public class Toolbox
    {
        #region Static Properties
        /// <summary>
        /// Gets or sets the S box.
        /// </summary>
        /// <value>The S box.</value>
        static public MyByte[,] SBox { get; set; }

        /// <summary>
        /// Gets or sets the Inverse S box.
        /// </summary>
        /// <value>The IS box.</value>
        static public MyByte[,] ISBox { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [S box generated].
        /// </summary>
        /// <value><c>true</c> if [S box generated]; otherwise, <c>false</c>.</value>
        static public bool SBoxGenerated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [Inverse S box generated].
        /// </summary>
        /// <value><c>true</c> if [Inverse S box generated]; otherwise, <c>false</c>.</value>
        static public bool ISBoxGenerated { get; set; }
        #endregion

        #region Static Constructor

        /// <summary>
        /// Initializes the <see cref="Toolbox"/> class.
        /// </summary>
        static Toolbox()
        {
            GenerateSBox();
            GenerateISBox();
        }
        
        #endregion

        #region SBox Methods
        /// <summary>
        /// Generates the S box.
        /// </summary>
        public static void GenerateSBox()
        {
            SBox = new MyByte[16, 16];
            byte value = 0;
            for (int i = 0; i < 16; i++ )
            {
                for (int j = 0; j < 16; j++)
                {
                    byte sBoxvalue = CalSBoxValue(value++);
                    SBox[i, j] = new MyByte(sBoxvalue);
                }
            }
            SBoxGenerated = true;
        }

        /// <summary>
        /// Generates the Inverse S box.
        /// </summary>
        public static void GenerateISBox()
        {
            if (!SBoxGenerated)
                GenerateSBox();

            ISBox = new MyByte[16, 16];
            for (byte i = 0; i < 16; i++ )
            {
                for (byte j = 0; j < 16; j++ )
                {
                    MyByte temp1 = SBox[i, j];
                    byte col = temp1.FirstWord;
                    byte row = temp1.SecondWord;
                    MyByte temp2 = SBox[row, col];
                    row = temp2.SecondWord;
                    col = temp2.FirstWord;
                    ISBox[row, col] = temp1;
                }
            }

            ISBoxGenerated = true;
        }

        /// <summary>
        /// Cals the S box value.
        /// </summary>
        /// <param name="p_value">The p_value.</param>
        /// <returns></returns>
        public static byte CalSBoxValue(byte p_value)
        {
            byte s, x;
            s = x = GaloisField.GetMulInverse(p_value);
            for (byte i = 0; i < 4; i++)
            {
                /* One bit circular rotate to the left */
                s = (byte)((s << 1) | (s >> 7));
                /* xor with x */
                x ^= s;
            }
            x ^= 99; /* 0x63 */
            return x;
        }

        /// <summary>
        /// Applies the Sbox.
        /// </summary>
        /// <param name="tempWord">The temp word.</param>
        public static void ApplySBox(ref MyByte[] tempWord)
        {
            if (!SBoxGenerated)
                GenerateSBox();
            for (int i = 0; i < tempWord.Length; i++)
            {
                tempWord[i] = SBox[tempWord[i].SecondWord, tempWord[i].FirstWord];
            }
        }

        /// <summary>
        /// Applies the Sbox.
        /// </summary>
        /// <param name="tempWord">The temp word.</param>
        public static MyByte[] ApplySBox(MyByte[] tempWord)
        {
            MyByte[] result = new MyByte[tempWord.Length];
            if (!SBoxGenerated)
                GenerateSBox();
            for (int i = 0; i < tempWord.Length; i++)
            {
                result[i] = SBox[tempWord[i].SecondWord, tempWord[i].FirstWord];
            }

            return result;
        }

        #endregion

        #region Converters
        /// <summary>
        /// Strings to byte array.
        /// </summary>
        /// <param name="hex">The hex.</param>
        /// <returns></returns>
        public static byte[]    HexToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        /// <summary>
        /// Converts to my byte.
        /// </summary>
        /// <param name="p_value">The p_value.</param>
        /// <param name="p_type">The p_type.</param>
        /// <returns></returns>
        public static MyByte[]  ConvertToMyByte(string p_value, KeyType p_type)
        {
            MyByte[] resultBytes;
            switch (p_type)
            {
                case KeyType.Hex:
                    {
                        if (p_value.Length % 2 != 0)
                        {
                            p_value = "0" + p_value;
                        }
                        byte[] ByteArr = HexToByteArray(p_value);
                        resultBytes = new MyByte[ByteArr.Length];
                        for (int i = 0, k = ByteArr.Length-1; i < ByteArr.Length; i++, k--)
                        {
                            resultBytes[i] = new MyByte(ByteArr[i]);
                        }
                        
                        return resultBytes;
                    }
                case KeyType.Binary:
                    {
                        while (p_value.Length%8 != 0)
                        {
                            p_value = "0" + p_value;
                        }
                        resultBytes = new MyByte[p_value.Length/8];

                        for (int i =0, k = p_value.Length; i< resultBytes.Length; i++,k-=8)
                        {
                            int start = k - 8;
                            string ttx = p_value.Substring(start, 8);
                            resultBytes[i] = new MyByte(Convert.ToByte(ttx,2));
                        }

                        return resultBytes;
                    }
                case KeyType.ASCII:
                    {
                        resultBytes = new MyByte[p_value.Length];
                        
                        for (int i = 0, k= p_value.Length-1; i < p_value.Length; i++,k-- )
                        {
                            resultBytes[i] = new MyByte(Convert.ToByte(p_value[i]));
                        }
                        return resultBytes;
                    }
                default:
                    break;
            }
            return new MyByte[1];
        }
        /// <summary>
        /// Converts my byte to string.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <returns></returns>
        public static string    ConvertMyByteToString(MyByte[] p_inputBytes)
        {
            string resultString = "";
            for (int i = 0; i < p_inputBytes.Length;i++ )
            {
                resultString += Convert.ToChar(p_inputBytes[i].Value);
            }
            return resultString;
        }
        /// <summary>
        /// Converts my byte to byte array.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <returns></returns>
        public static byte[]    ConvertMyByteToByteArray(MyByte[] p_inputBytes)
        {
            byte[] resultBuffer = new byte[p_inputBytes.Length];

            for (int i = 0; i < p_inputBytes.Length; i++ )
            {
                resultBuffer[i] = p_inputBytes[i].Value;
            }

            return resultBuffer;
        }
        /// <summary>
        /// Texts to byte array.
        /// </summary>
        /// <param name="p_text">The p_text.</param>
        /// <returns></returns>
        static public byte[]    TextToByteArray(string p_text)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(p_text);
        }
        /// <summary>
        /// Bytes the array to text.
        /// </summary>
        /// <param name="p_byteArray">The p_byte array.</param>
        /// <returns></returns>
        static public string    ByteArrayToText(byte[] p_byteArray)
        {
            return System.Text.ASCIIEncoding.ASCII.GetString(p_byteArray);
        }
        /// <summary>
        /// Converts to hex.
        /// </summary>
        /// <param name="asciiString">The ASCII string.</param>
        /// <returns></returns>
        static public string    ConvertToHex(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt64(tmp.ToString()));
            }
            return hex;
        }
        /// <summary>
        /// Hexes to binary.
        /// </summary>
        /// <param name="hexvalue">The hexvalue.</param>
        /// <returns></returns>
        static public string    HexToBinary(string hexvalue)
        {
            string binaryval = "";
            binaryval = Convert.ToString(Convert.ToInt64(hexvalue, 16), 2);
            //if (binaryval.Length % 2 != 0)
            //    binaryval = '0' + binaryval;
            if (binaryval.Length < 64)
            {
                int dif = 64 - binaryval.Length;
                for (int i = 0; i < dif; i++)
                    binaryval = '0' + binaryval;
            }
            return binaryval;
        }
        /// <summary>
        /// Hexes to ASCII.
        /// </summary>
        /// <param name="hexString">The hex string.</param>
        /// <returns></returns>
        static public string    HexToASCII(string hexString)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexString.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }
        /// <summary>
        /// Binaries to hex.
        /// </summary>
        /// <param name="binary">The binary.</param>
        /// <returns></returns>
        public static string    BinaryToHex(string binary)
        {
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            // TODO: check all 1's or 0's... Will throw otherwise

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                // pad to length multiple of 8
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }
        /// <summary>
        /// Ints to binary.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        static public string    IntToBinary(byte number)
        {
            string _int = Convert.ToString(number, 2);
            if (_int.Length == 1) return _int = "000" + _int;
            else if (_int.Length == 2) return _int = "00" + _int;
            else if (_int.Length == 3) return _int = "0" + _int;
            else return _int;
        }
        /// <summary>
        /// Texts to byte array binary.
        /// </summary>
        /// <param name="p_text">The p_text.</param>
        /// <returns></returns>
        static public byte[]    TextToByteArrayBinary(string p_text)
        {
            byte[] Ascii = TextToByteArray(p_text);
            for (int i = 0; i < Ascii.Length; i++)
            {
                if (Ascii[i] == 48) Ascii[i] = 0;
                else Ascii[i] = 1;
            }
            return Ascii;
        }
        /// <summary>
        /// Binaries the bytes array to string.
        /// </summary>
        /// <param name="binary">The binary.</param>
        /// <returns></returns>
        static public string    BinaryBytesArrayToString(byte[] binary)
        {
            string binarystring = "";
            for (int i = 0; i < binary.Length; i++)
                binarystring += binary[i].ToString();
            return binarystring;
        }
        /// <summary>
        /// Binaries to int.
        /// </summary>
        /// <param name="binaryNumber">The binary number.</param>
        /// <returns></returns>
        static public int       BinaryToInt(string binaryNumber)
        {
            int multiplier = 1;
            int converted = 0;
            for (int i = binaryNumber.Length - 1; i >= 0; i--)
            {
                int t = System.Convert.ToInt16(binaryNumber[i].ToString());
                converted = converted + (t * multiplier);
                multiplier = multiplier * 2;
            }
            return converted;
        }
        /// <summary>
        /// Bytes the array to hex string.
        /// </summary>
        /// <param name="ba">The ba.</param>
        /// <returns></returns>
        public static string    ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        #endregion

        #region Helper Functions
        /// <summary>
        /// Strings to elliptic point.
        /// </summary>
        /// <param name="p_point">The p_point.</param>
        /// <param name="Base">The base.</param>
        /// <returns></returns>
        static public EllpiticPoint StringToEllipticPoint(string p_point, int Base)
        {
            string tempPoint = "";
            p_point = p_point.Trim();
            for (int i = 1; i < p_point.Length-1; i++ )
            {
                tempPoint += p_point[i].ToString();
            }
            string[] temparr = tempPoint.Split(',');
            return new EllpiticPoint(int.Parse(temparr[0]), int.Parse(temparr[1]), Base);
        }

        /// <summary>
        /// Gets the cipher elliptic.
        /// </summary>
        /// <param name="p_encrypted">The p_encrypted.</param>
        /// <param name="Base">The base.</param>
        /// <returns></returns>
        static public List<EllpiticPoint> GetCipherElliptic(string p_encrypted, int Base)
        {
            List<EllpiticPoint> CT = new List<EllpiticPoint>();
            string[] tempArr = p_encrypted.Split(';');
            CT.Add(StringToEllipticPoint(tempArr[0],Base));
            CT.Add(StringToEllipticPoint(tempArr[1], Base));
            return CT;
        }

        /// <summary>
        /// Gets the size of the key in bits.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        /// <param name="p_keyType">Type of the p_key.</param>
        /// <returns></returns>
        public static int GetKeySize(string p_key, KeyType p_keyType)
        {
            switch (p_keyType)
            {
                case KeyType.Hex:
                    {
                        while (p_key.Length % 2 != 0)
                            p_key = "0" + p_key;
                        return 4 * p_key.Length;
                    }
                case KeyType.Binary:
                    {
                        while (p_key.Length % 8 != 0)
                            p_key = "0" + p_key;
                        return p_key.Length;
                    }
                case KeyType.Integer:
                    return p_key.Length * 2;
                case KeyType.ASCII:
                    return p_key.Length * 8;
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the elements from the array.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <param name="arr">The arr.</param>
        /// <returns></returns>
        static public byte[] GetElementsFromTheArray(int startIndex, int length, ref byte[] arr)
        {
            byte[] newArr = new byte[length];
            int index = 0;
            for (int i = startIndex; i < startIndex + length; i++, index++)
                newArr[index] = arr[i];
            return newArr;

        }

        /// <summary>
        /// Circulars the rotate.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <param name="p_startIndex">Index of the p_start.</param>
        /// <param name="p_endIndex">Index of the p_end.</param>
        public static void CircularRotate(ref MyByte[] p_inputBytes,int p_startIndex, int p_endIndex)
        {
            if (p_startIndex > p_endIndex || p_startIndex < 0 || p_endIndex >= p_inputBytes.Length)
            {
                throw new IndexOutOfRangeException();
            }
            MyByte tempByte = p_inputBytes[p_startIndex];
            for (int i = p_startIndex; i < p_endIndex;i++ )
            {
                p_inputBytes[i] = p_inputBytes[i + 1];
            }
            p_inputBytes[p_endIndex] = tempByte;
        }

        /// <summary>
        /// Circulars the rotate.
        /// </summary>
        /// <param name="p_inputBytes">The p_input bytes.</param>
        /// <param name="p_startIndex">Index of the p_start.</param>
        /// <param name="p_endIndex">Index of the p_end.</param>
        public static MyByte[] CircularRotate(MyByte[] p_inputBytes, int p_startIndex, int p_endIndex)
        {
          
            if (p_startIndex > p_endIndex || p_startIndex < 0 || p_endIndex >= p_inputBytes.Length)
            {
                throw new IndexOutOfRangeException();
            }
            MyByte[] result = new MyByte[p_endIndex - p_startIndex + 1];
            result[p_endIndex - p_startIndex] = p_inputBytes[p_startIndex];
            //MyByte tempByte = p_inputBytes[p_startIndex];
            int k = 0;
            for (int i = p_startIndex; i < p_endIndex; i++)
            {
                result[k++] = p_inputBytes[i + 1];
            }

            return result;
        }

        /// <summary>
        /// Powers the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static BigInteger power(BigInteger x, double y)
        {
            BigInteger z = 1;
            for (int i = 0; i < y; i++)
                z *= x;
            return z;
        }

        /// <summary>
        /// Gets the inverse mod.
        /// </summary>
        /// <param name="Base">The base.</param>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static int GetInverseMod(int Base, int number)
        {
            if (number < 0)
            {
                int temp = number + (((int)(number * -1) / Base) * Base);
                while (temp < 0)
                {
                    temp += Base;
                }
                number = CalculateMod(temp,Base);
            }
            int A1, A2, A3, B1, B2, B3;
            int Q;
            A1 = 1;
            A2 = 0;
            A3 = Base;
            B1 = 0;
            B2 = 1;
            B3 = number;
            while (B3 != 1 && B3 != 0)
            {
                Q = (int)(A3 / B3);
                int newB1 = A1 - Q * B1;
                int newB2 = A2 - Q * B2;
                A1 = B1;
                A2 = B2;
                int newA3 = B3;
                B3 = CalculateMod(A3 , B3);
                A3 = newA3;
                B1 = newB1;
                B2 = newB2;

            }
            if (B3 == 1)
            {
                if (B2 < 0)
                {
                    int temp = B2 + (((int)(B2 * -1) / Base) * Base);
                    while (temp < 0)
                    {
                        temp += Base;
                    }
                    B2 = CalculateMod(temp, Base);
                }
                return B2;
            }
            return -1;
        } 
       
        /// <summary>
        /// Calculates the mod.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="Base">The base.</param>
        /// <returns></returns>
        public static int CalculateMod(int number, int Base)
        {
            return (number % Base + Base) % Base;
        }

        /// <summary>
        /// Shuffle the Key.
        /// </summary>
        /// <param name="S">The shuffled key index.</param>
        /// <param name="T">The Shufled key.</param>
        /// <returns></returns>
        public static  void ShuffleKey(List<char> key,int[] S, int[] T)
        {
         
            for (int i = 0; i < 256; i++)
            {
                S[i] = i;
                T[i] = key[i % key.Count];
            }

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + T[i]) % 256;
                int temp = S[i];
                S[j] = S[i];
                S[i] = temp;
            }
        }

        /// <summary>
        /// Copies at.
        /// </summary>
        /// <param name="P">The P.</param>
        /// <param name="index">The index.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        static public byte[] CopyAt(byte[] P, int index, int count)
        {
            byte[] res = new byte[count];
            for (int i = 0; i < count; i++)
                res[i] = P[i + index];
            return res;
        }
        #endregion


        public static MyByte[] Transpose(MyByte[] subArray)
        {
            int sqr = (int)Math.Sqrt(subArray.GetLength(0));
            if (sqr % 1 != 0)
                return subArray;
            MyByte[] tempArr = new MyByte[subArray.GetLength(0)];

            for (int i = 0; i < sqr; i++)
            {
                for (int k = 0; k < sqr; k++)
                {
                    tempArr[i * sqr + k] = subArray[k * sqr + i];
                }
            }
            return tempArr;
        }

        /// <summary>
        /// Mies the byte to hex.
        /// </summary>
        /// <param name="res">The res.</param>
        /// <returns></returns>
        public static string MyByteToHex(MyByte[] res)
        {
            return ByteArrayToHexString(ConvertMyByteToByteArray(res));
        }
    }
}
