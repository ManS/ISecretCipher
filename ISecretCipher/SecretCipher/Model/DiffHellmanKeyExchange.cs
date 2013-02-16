using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Model
{
    public class DiffHellmanKeyExchange
    {
        #region Properties
        /// <summary>
        /// Gets or sets the Q.
        /// </summary>
        /// <value>The Q.</value>
        public double Q { get; set; }
        /// <summary>
        /// Gets or sets the alpha.
        /// </summary>
        /// <value>The alpha.</value>
        public double Alpha { get; set; }
        /// <summary>
        /// Gets or sets the xa.
        /// </summary>
        /// <value>The xa.</value>
        public double Xa { get; set; }
        /// <summary>
        /// Gets or sets the xb.
        /// </summary>
        /// <value>The xb.</value>
        public double Xb { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DiffHellmanKeyExchange"/> class.
        /// </summary>
        /// <param name="p_Q">The p_ Q.</param>
        /// <param name="p_Alpha">The p_ alpha.</param>
        /// <param name="p_XA">The p_ XA.</param>
        /// <param name="p_XB">The p_ XB.</param>
        public DiffHellmanKeyExchange(double p_Q, double p_Alpha, double p_XA, double p_XB)
        {
            this.Alpha = p_Alpha;
            this.Q = p_Q;
            this.Xa = p_XA;
            this.Xb = p_XB;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Exchanges the keys.
        /// </summary>
        /// <returns></returns>
        public double[] ExchangeKeys()
        {
            double[] Y = new double[2];
            double[] K = new double[2];

            Y[0] = (Math.Pow(this.Alpha, this.Xa)) % this.Q;//Ya
            Y[1] = (Math.Pow(this.Alpha, this.Xb)) % this.Q;//Yb

            K[0] = (Math.Pow(Y[1], this.Xa)) % this.Q;
            K[1] = (Math.Pow(Y[0], this.Xb)) % this.Q;

            return K;
        }
        #endregion
    }
}
