using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Utilities
{
    public static class PrimeChecker
    {
    
        private const int BufferSize = 64 * 1024; // 64K * sizeof(int) == 256 KB

        private static int[] primes;
        /// <summary>
        /// Gets or sets the max prime.
        /// </summary>
        /// <value>The max prime.</value>
        public static int MaxPrime { get; private set; }

        /// <summary>
        /// Determines whether the specified value is prime.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is prime; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPrime(int value)
        {
            if (value <= MaxPrime)
            {
                return Array.BinarySearch(primes, value) >= 0;
            }
            else
            {
                return IsPrime(value, primes.Length) && IsLargerPrime(value);
            }
        }

        /// <summary>
        /// Initializes the <see cref="PrimeChecker"/> class.
        /// </summary>
        static PrimeChecker()
        {
            primes = new int[BufferSize];
            primes[0] = 2;
            for (int i = 1, x = 3; i < primes.Length; x += 2)
            {
                if (IsPrime(x, i))
                    primes[i++] = x;
            }
            MaxPrime = primes[primes.Length - 1];
        }

        /// <summary>
        /// Determines whether the specified value is prime.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="primesLength">Length of the primes.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is prime; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPrime(int value, int primesLength)
        {
            for (int i = 0; i < primesLength; ++i)
            {
                if (value % primes[i] == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether [is larger prime] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if [is larger prime] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsLargerPrime(int value)
        {
            int max = (int)Math.Sqrt(value);
            for (int i = MaxPrime + 2; i <= max; i += 2)
            {
                if (value % i == 0)
                    return false;
            }
            return true;
        }
    }
}
