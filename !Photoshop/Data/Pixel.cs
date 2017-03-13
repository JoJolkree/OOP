using System;

namespace MyPhotoshop
{
    public struct Pixel
    {
        private double _red;
        public double Red
        {
            get { return _red; }
            set
            {
                _red = CheckIsCorrect(value);
            }
        }

        private double _green;
        public double Green
        {
            get { return _green; }
            set
            {
                _green = CheckIsCorrect(value);
            }
        }

        private double _blue;
        public double Blue
        {
            get
            {
                return _blue;
            }
            set
            {
                _blue = CheckIsCorrect(value);
            }
        }

        private static double CheckIsCorrect(double value)
        {
            if (value < 0 || value > 1) throw new ArgumentException();
            return value;
        }

        public Pixel(double r, double g, double b)
        {
            _red = _green = _blue = 0;
            Red = r;
            Green = g;
            Blue = b;
        }

        public static double Trim(double value)
        {
            if (value < 0) return 0;
            if (value > 1) return 1;
            return value;
        }

        public static Pixel operator *(Pixel p, double c)
        {
            return new Pixel(Trim(p.Red * c), Trim(p.Green * c), Trim(p.Blue * c));
        }

        public static Pixel operator *(double c, Pixel p)
        {
            return p * c;
        }
    }
}