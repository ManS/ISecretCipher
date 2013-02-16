using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Utilities;

namespace SecretCipher.Model
{
    public class EllpiticUser
    {
        #region Properties

        /// <summary>
        /// Gets or sets the curve.
        /// </summary>
        /// <value>The curve.</value>
        public EllpiticCurve Curve { get; private set; }

        /// <summary>
        /// Gets or sets the private N.
        /// </summary>
        /// <value>The private N.</value>
        public int PrivateN { get; private set; }

        /// <summary>
        /// Gets or sets the G.
        /// </summary>
        /// <value>The G.</value>
        public EllpiticPoint G { get; private set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public EllpiticPoint PublicKey { get; private set; }

        /// <summary>
        /// Gets or sets the K.
        /// </summary>
        /// <value>The K.</value>
        public int K { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EllpiticUser"/> class.
        /// </summary>
        /// <param name="p_curve">The p_curve.</param>
        /// <param name="p_n">The P_N.</param>
        /// <param name="p_privateN">The p_private N.</param>
        /// <param name="p_g">The P_G.</param>
        public EllpiticUser(EllpiticCurve p_curve, int p_privateN, EllpiticPoint p_g, int p_k)
        {
            this.Curve = p_curve;
            this.G = p_g;
            this.PrivateN = p_privateN;
            this.K = p_k;
            this.CalculatePublicKey();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the public key.
        /// </summary>
        private void CalculatePublicKey()
        {
            this.PublicKey = this.Curve.Mul(this.G, this.PrivateN);
        }

        /// <summary>
        /// Calculates the shared key.
        /// </summary>
        public EllpiticPoint CalculateSharedKey(EllpiticPoint p_publicKey)
        {
            return this.Curve.Mul(p_publicKey, this.PrivateN);
        }

        /// <summary>
        /// Encrypts the specified p_plain data.
        /// </summary>
        /// <param name="p_plainData">The p_plain data.</param>
        /// <param name="p_PublicKeyB">The p_ public key B.</param>
        /// <returns></returns>
        public List<EllpiticPoint> Encrypt(EllpiticPoint p_plainData, EllpiticPoint p_PublicKeyB)
        {
            List<EllpiticPoint> encryptedData = new List<EllpiticPoint>(2);
            encryptedData.Add(this.Curve.Mul(this.G, this.K));
            EllpiticPoint gam3 = (this.Curve.Mul(p_PublicKeyB, K));
            encryptedData.Add(this.Curve.Add(p_plainData, gam3));

            return encryptedData;
        }

        /// <summary>
        /// Decrypts the specified p_cipher point1.
        /// </summary>
        /// <param name="p_cipherPoint1">The p_cipher point1.</param>
        /// <param name="p_cipherPoint1">The p_cipher point1.</param>
        /// <param name="?">The ?.</param>
        /// <returns></returns>
        public EllpiticPoint Decrypt(EllpiticPoint p_cipherPoint1, EllpiticPoint p_cipherPoint2)
        {
            return this.Curve.Sub(p_cipherPoint2, (this.Curve.Mul(p_cipherPoint1, this.PrivateN)));
        }

        #endregion
    }
}
