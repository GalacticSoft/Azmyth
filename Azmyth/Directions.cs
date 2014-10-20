using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth
{
    public enum Directions
    {
        North = 0,
        Northeast = 1,
        East = 2,
        Southeast = 3,
        South = 4,
        Southwest = 5,
        West = 6,
        Northwest = 7,
        MaxCardinal = 8,
        Up = 8,
        Down = 9,
        Max = 10
    }

    public static class Direction
    {
        public static Vector North      { get { return new Vector( 0, -1,  0); } }
        public static Vector NorthEast  { get { return new Vector( 1, -1,  0); } }
        public static Vector East       { get { return new Vector( 1,  0,  0); } }
        public static Vector SouthEast  { get { return new Vector( 1,  1,  0); } }
        public static Vector South      { get { return new Vector( 0,  1,  0); } }
        public static Vector SouthWest  { get { return new Vector(-1,  1,  0); } }
        public static Vector West       { get { return new Vector(-1,  0,  0); } }
        public static Vector NorthWest  { get { return new Vector(-1,  1,  0); } }
        public static Vector Up         { get { return new Vector( 0,  0,  1); } }
        public static Vector Down       { get { return new Vector( 0,  0, -1); } }

        public static Vector GetDirectionVector(Directions direction)
        {
            Vector vector = new Vector(0, 0, 0);

            switch (direction)
            {
                case Directions.North:
                    vector = Direction.North;
                    break;
                case Directions.Northeast:
                    vector = Direction.NorthEast;
                    break;
                case Directions.East:
                    vector = Direction.East;
                    break;
                case Directions.Southeast:
                    vector = Direction.SouthEast;
                    break;
                case Directions.South:
                    vector = Direction.South;
                    break;
                case Directions.Southwest:
                    vector = Direction.SouthWest;
                    break;
                case Directions.West:
                    vector = Direction.West;
                    break;
                case Directions.Northwest:
                    vector = Direction.NorthWest;
                    break;
                case Directions.Up:
                    vector = Direction.Up;
                    break;
                case Directions.Down:
                    vector = Direction.Down;
                    break;
            }

            return vector;
        }
    }
}
