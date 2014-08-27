using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Azmyth
{
    public struct Vector
    {
        public static readonly Vector Origin = new Vector(0, 0, 0);
        public static readonly Vector xAxis = new Vector(1, 0, 0);
        public static readonly Vector yAxis = new Vector(0, 1, 0);
        public static readonly Vector zAxis = new Vector(0, 0, 1);

        public static readonly Vector MinValue = new Vector(int.MinValue, int.MinValue, int.MinValue);
        public static readonly Vector MaxValue = new Vector(int.MaxValue, int.MaxValue, int.MaxValue);

        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        private Vector(Vector v) : this(v.X, v.Y, v.Z) { }

        public Vector(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        public int Z
        {
            get { return _z; }
        }

        public static Vector operator +(Vector v1)
        {
            return new Vector(+v1.X, +v1.Y, +v1.Z);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v1.X, v1.Z + v2.Z);
        }

        public static Vector operator +(Vector v1, int s1)
        {
            return new Vector(v1.X + s1, v1.Y + s1, v1.Z + s1);
        }

        public static Vector operator -(Vector v1)
        {
            return new Vector(-v1.X, -v1.Y, -v1.Z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v1.X, v1.Z - v2.Z);
        }

        public static Vector operator -(Vector v1, int s1)
        {
            return new Vector(v1.X - s1, v1.Y - s1, v1.Z - s1);
        }

        public static Vector operator /(Vector v1, int s1)
        {
            return new Vector(v1.X / s1, v1.Y / s1, v1.Z / s1);
        }

        public static Vector operator /(int s1, Vector v1)
        {
            return v1 / s1;
        }

        public static Vector operator *(Vector v1, Vector v2)
        {
            return new Vector
            (
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X
            );
        }

        public static Vector operator *(Vector v1, int s1)
        {
            return new Vector(v1.X * s1, v1.Y * s1, v1.Z * s1);
        }

        public static Vector operator *(int s1, Vector v1)
        {
            return v1 * s1;
        }

        public static bool operator <(Vector v1, Vector v2)
        {
            return v1.Magnitude < v2.Magnitude;
        }

        public static bool operator <=(Vector v1, Vector v2)
        {
            return v1.Magnitude <= v2.Magnitude;
        }

        public static bool operator >(Vector v1, Vector v2)
        {
            return v1.Magnitude > v2.Magnitude;
        }

        public static bool operator >=(Vector v1, Vector v2)
        {
            return v1.Magnitude >= v2.Magnitude;
        }
        public static bool operator ==(Vector v1, Vector v2)
        {
            return ((v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Z == v2.Z));
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return !(v1 == v2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector)
            {
                return (this == (Vector)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ((_x + _y + _z) % Int32.MaxValue);
        }

        public override string ToString()
        {
            return "{" + _x + "," + _y + "," + _z + "}";
        }

        public static bool TryParse(string input, out Vector vector)
        {
            try
            {
                vector = Parse(input);

                return true;
            }
            catch
            {
                vector = Origin;

                return false;
            }
        }

        public static Vector Parse(string input)
        {
            input = input.Trim();

            Regex vectorRegex = new Regex(@"[{(\[\|]\s*[0-9]+\s*[.:;,\-]\s*[0-9]+\s*[.:;,\-]\s*[0-9]+\s*[})\]\|]");

            if (vectorRegex.IsMatch(input))
            {
                input = input.Substring(1, input.Length - 2);

                input = input.Trim();

                string[] values = input.Split(new char[] { ';', ':', '.', ',', '-'});

                if(values.Length > 3)
                {
                    throw new ArgumentException(); 
                }

                int x, y, z;

                if (int.TryParse(values[0], out x) && int.TryParse(values[1], out y) && int.TryParse(values[2], out z))
                {
                    return new Vector(x, y, z);
                }

                throw new InvalidCastException();
            }

            throw new ArgumentOutOfRangeException();

        }

        public int Sum()
        {
            return _x + _y + _z;
        }

        public Vector Pow(int power)
        {
            return new Vector
            (
                (int)System.Math.Pow((double)_x, (double)power),
                (int)System.Math.Pow((double)_y, (double)power),
                (int)System.Math.Pow((double)_z, (double)power)
            );
        }

        public Vector Sqrt()
        {
            return new Vector
            (
                (int)System.Math.Sqrt((double)_x),
                (int)System.Math.Sqrt((double)_y),
                (int)System.Math.Sqrt((double)_z)
            );
        }

        public double SumSqrs()
        {
            return System.Math.Sqrt((double)_x) + System.Math.Sqrt((double)_y) + System.Math.Sqrt((double)_z);
        }

        public double Magnitude
        {
            get { return System.Math.Sqrt((double)SumSqrs()); }
        }
    }
}
