using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Utilities
{
    public class EllpiticPoint 
    {
        #region Properties
        public int X { get; set; }
        public int Y { get; set; }
        public int P { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EllpiticPoint"/> class.
        /// </summary>
        public EllpiticPoint()
        {
            this.X = 0;
            this.Y = 0;
            this.P = 10;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EllpiticPoint"/> class.
        /// </summary>
        /// <param name="p_P">The p_ P.</param>
        public EllpiticPoint(int p_P)
        {
            this.Y = 0;
            this.X = 0;
            this.P = p_P;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EllpiticPoint"/> class.
        /// </summary>
        /// <param name="p_X">The p_ A.</param>
        /// <param name="p_X">The p_ B.</param>
        /// <param name="p_P">The p_ P.</param>
        public EllpiticPoint(int p_X, int p_Y, int p_P)
        {
            this.X = p_X;
            this.Y = p_Y;
            this.P = p_P;
        }
        #endregion

        #region Overridden Functions
   
        /// <summary>
        /// Equals es the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this == (EllpiticPoint)obj;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.X ^ this.Y ^ this.P;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "{"+this.X.ToString()+","+ this.Y.ToString() +"}";
        }
       
        #endregion

        #region Operators Overloading

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="p_point1">The p_point1.</param>
        /// <param name="p_point2">The p_point2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(EllpiticPoint p_point1, EllpiticPoint p_point2)
        {
            return (p_point1.X == p_point2.X && p_point1.Y == p_point2.Y);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="p_point1">The p_point1.</param>
        /// <param name="p_point2">The p_point2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(EllpiticPoint p_point1, EllpiticPoint p_point2)
        {
            return !(p_point2 == p_point1);
        }

        /// <summary>
        /// Implements the operator %.
        /// </summary>
        /// <param name="p_point1">The p_point1.</param>
        /// <param name="Base">The base.</param>
        /// <returns>The result of the operator.</returns>
        public static EllpiticPoint operator %(EllpiticPoint p_point1, int Base)
        {
            EllpiticPoint result = new EllpiticPoint();
            result.P = Base;
            result.X = p_point1.X > 0 ? p_point1.X % Base : -1 * p_point1.X % Base;
            result.Y = p_point1.Y > 0 ? p_point1.Y % Base : -1 * p_point1.Y % Base;
            return result;
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="p_point1">The p_point1.</param>
        /// <param name="p_point2">The p_point2.</param>
        /// <returns>The result of the operator.</returns>
        public static EllpiticPoint operator -(EllpiticPoint p_point1, EllpiticPoint p_point2)
        {
            if (p_point2.P != p_point1.P)
            {
                throw new ArgumentException("Base must be equal!");
            }
            return p_point1 + (-p_point2);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="p_point1">The p_point1.</param>
        /// <returns>The result of the operator.</returns>
        public static EllpiticPoint operator -(EllpiticPoint p_point)
        {
            return new EllpiticPoint(p_point.X, Toolbox.CalculateMod(p_point.Y,p_point.P), p_point.P);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="p_point1">The p_point1.</param>
        /// <param name="p_point2">The p_point2.</param>
        /// <returns>The result of the operator.</returns>
        public static EllpiticPoint operator +(EllpiticPoint p_point1, EllpiticPoint p_point2)
        {

            throw new NotImplementedException();
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="p_point1">The p_point1.</param>
        /// <param name="p_point2">The p_point2.</param>
        /// <returns>The result of the operator.</returns>
        public static EllpiticPoint operator *(EllpiticPoint p_point1, EllpiticPoint p_point2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="p_point">The p_point.</param>
        /// <param name="num">The num.</param>
        /// <returns>The result of the operator.</returns>
        public static EllpiticPoint operator *(EllpiticPoint p_point, int num)
        {
            return (num * p_point);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <param name="p_point">The p_point.</param>
        /// <returns>The result of the operator.</returns>
        public static EllpiticPoint operator *(int num, EllpiticPoint p_point)
        {
            throw new NotImplementedException();

        }
        #endregion
    }
    
    public class EllpiticCurve
    {
        #region Properties
   
        /// <summary>
        /// Gets or sets the A.
        /// </summary>
        /// <value>The A.</value>
        public int A { get; set; }
       
        /// <summary>
        /// Gets or sets the B.
        /// </summary>
        /// <value>The B.</value>
        public int B { get; set; }
       
        /// <summary>
        /// Gets or sets the P.
        /// </summary>
        /// <value>The P.</value>
        public int P { get; set; }
     
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EllpiticCurve"/> class.
        /// </summary>
        /// <param name="p_A">The p_ A.</param>
        /// <param name="p_B">The p_ B.</param>
        /// <param name="p_P">The p_ P.</param>
        public EllpiticCurve(int p_A, int p_B, int p_P)
        {
            this.A = p_A;
            this.B = p_B;
            this.P = p_P;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Muls the specified p_point1.
        /// </summary>
        /// <param name="p_point1">The p_point1.</param>
        /// <param name="p_point2">The p_point2.</param>
        /// <returns></returns>
        public EllpiticPoint Mul(EllpiticPoint p_point1, int num)
        {
            if (num == 1)
            {
                return p_point1;
            }
            EllpiticPoint result = p_point1;
            for (int i = 0; i < num-1; i++ )
            {
                result = this.Add(result, p_point1);
            }

            return result;
        }

        /// <summary>
        /// Adds the specified p_point1.
        /// </summary>
        /// <param name="p_point1">The p_point1.</param>
        /// <param name="p_point2">The p_point2.</param>
        /// <returns></returns>
        public EllpiticPoint Add(EllpiticPoint p_point1, EllpiticPoint p_point2)
        {
            int Denominator;
            int Numerator;
            int lambda;
            if (p_point1 != p_point2)
            {
                Numerator = p_point2.Y - p_point1.Y;
                Denominator = p_point2.X - p_point1.X;
                if (Denominator < 0)
                {
                    Numerator *= -1;
                    Denominator *= -1;
                }
                if (Toolbox.CalculateMod(Numerator,Denominator)== 0)
                {
                    lambda = Toolbox.CalculateMod(Numerator / Denominator, this.P);
                }
                else
                {
                    Denominator = Toolbox.GetInverseMod(this.P, Denominator);
                    lambda = Toolbox.CalculateMod(Denominator * Numerator,this.P);
                }
            }
            else
            {
                Numerator = (int)(3 * Math.Pow(p_point1.X, 2) + this.A);
                Denominator = 2 * p_point1.Y;
                {
	                if (Denominator < 0)
	                {
	                    Numerator *= -1;
	                    Denominator *= -1;
	                }
                    if (Toolbox.CalculateMod(Numerator, Denominator) == 0)
                    {
                        lambda = Toolbox.CalculateMod(Numerator / Denominator, this.P);
                    }
                    else
                    {
                        Denominator = Toolbox.GetInverseMod(this.P, Denominator);
	                    lambda = Toolbox.CalculateMod(Denominator * Numerator, this.P);
	                }
                }
            }
            int X = Toolbox.CalculateMod(((int)Math.Pow(lambda,2)) - p_point1.X - p_point2.X ,this.P);
            int Y = Toolbox.CalculateMod(lambda * (p_point1.X - X) - p_point1.Y, this.P);
            return new EllpiticPoint(X, Y, this.P);
        }

        /// <summary>
        /// Subs the specified p_point1.
        /// </summary>
        /// <param name="p_point1">The p_point1.</param>
        /// <param name="p_point2">The p_point2.</param>
        /// <returns></returns>
        public EllpiticPoint Sub(EllpiticPoint p_point1, EllpiticPoint p_point2)
        {
            return this.Add(p_point1, -p_point2);
        }
        
        #endregion
    }
}