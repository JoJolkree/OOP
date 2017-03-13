using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        public int Numerator { get; }
        public int Denominator { get; }
        public bool IsNan { get; }

        public Rational(int numerator, int denominator = 1)
        {
            if (denominator < 0)
            {
                numerator *= -1;
                denominator *= -1;
            }
            var gcd = Gcd(Math.Abs(numerator), Math.Abs(denominator));
            Numerator = numerator / gcd;
            Denominator = denominator / gcd;
            if (denominator == 0) IsNan = true;
        }

        private Rational(int numerator, int denomimator, bool isNan) : this(numerator, denomimator)
        {
            IsNan = isNan;
        }

        public static Rational operator +(Rational first, Rational second)
        {
            return new Rational(first.Numerator * second.Denominator + second.Numerator * first.Denominator,
                first.Denominator * second.Denominator);
        }

        public static Rational operator -(Rational first, Rational second)
        {
            return new Rational(first.Numerator * second.Denominator - second.Numerator * first.Denominator,
                first.Denominator * second.Denominator);
        }

        public static Rational operator *(Rational first, Rational second)
        {
            return new Rational(first.Numerator * second.Numerator, first.Denominator * second.Denominator);
        }

        public static Rational operator /(Rational first, Rational second)
        {
            return new Rational(first.Numerator * second.Denominator, first.Denominator * second.Numerator,
                first.IsNan || second.IsNan || first.Denominator * second.Numerator == 0);
        }

        public static implicit operator double(Rational num)
        {
            return (double) num.Numerator / num.Denominator;
        }

        public static explicit operator int(Rational num)
        {
            if (num.Denominator != 1) throw new ArgumentException();
            return num.Numerator;
        }

        public static implicit operator Rational(int num)
        {
            return new Rational(num, 1);
        }

        private static int Gcd(int a, int b)
        {
            while( b != 0 )
            {
                var remainder = a % b;
                a = b;
                b = remainder;
            }
            return a;
        }
    }
}
