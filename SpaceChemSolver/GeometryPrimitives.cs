using SpaceChemSolver.Simulation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SpaceChemSolver
{
    /// <summary>
    /// Represents integer x, y coordinates.
    /// A point is immutable.
    /// </summary>
    public class Point
    {
        public readonly int x;
        
        public readonly int y;

        [DebuggerStepThrough]
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Projects the point one step in the specified direction.
        /// </summary>
        /// <param name="direction">the direction to project</param>
        /// <returns>the projected point</returns>
        public Point Project(Direction direction)
        {
            if(Direction.Continue == direction)
            {
                throw new ArgumentException("cannot project a point using Direction.Continue");
            }

            if(Direction.Up == direction)
            {
                return new Point(x, y - 1);
            }
            else if(Direction.Down == direction)
            {
                return new Point(x, y + 1);
            }
            else if (Direction.Left == direction)
            {
                return new Point(x - 1, y);
            }
            else
            {
                return new Point(x + 1, y);
            }
        }

        /// <summary>
        /// Rotates the point 90 degrees.
        /// </summary>
        /// <param name="origin">the origin of rotation</param>
        /// <param name="clockwise">whether to rotate clockwise or counter-clockwise</param>
        /// <returns>the rotated point</returns>
        public Point Rotate(Point origin, bool clockwise)
        {
            if(clockwise)
            {
                return new Point(origin.x - origin.y + y, origin.x + origin.y - x);
            }
            else
            {
                return new Point(origin.x + origin.y - y, origin.y - origin.x + x);
            }
        }

        public double DistanceFrom(Point target)
        {
            int x1 = x;
            int y1 = y;
            int x2 = target.x;
            int y2 = target.y;
            return ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        /// <summary>
        /// Determines the direction from this point to the target.
        /// </summary>
        /// <param name="target">the target point</param>
        /// <returns>the direction to reach target</returns>
        /// <remarks>The points must be a distance of one away.</remarks>
        public Direction DirectionTo(Point target)
        {
            if(x != target.x)
            {
                if(x == target.x - 1)
                {
                    return Direction.Left;
                }
                else if(x + 1 == target.x)
                {
                    return Direction.Right;
                }
            }
            else if(y != target.y)
            {
                if(y == target.y + 1)
                {
                    return Direction.Up;
                }
                else if(y + 1 == target.y)
                {
                    return Direction.Down;
                }
            }

            throw new ArgumentException(
                string.Format("{0} is not a distance of 1 away from {1}", target, this));
        }

        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            Point other = obj as Point;
            if (other == null)
            {
                return false;
            }

            return x == other.x && y == other.y;
        }

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            int hash = 7;
            hash = 71 * hash + x;
            hash = 71 * hash + y;
            return hash;
        }

        [DebuggerStepThrough]
        public override string ToString()
        {
            return string.Format("({0},{1})", x, y);
        }
    }

    /// <summary>
    /// Represents a rectangle with integer parameters.
    /// A rectangle is immutable.
    /// </summary>
    public class Rectangle
    {
        /// <summary>
        /// The upper right hand x coordinate.
        /// </summary>
        public readonly int x;

        /// <summary>
        /// The upper right hand y coordinate.
        /// </summary>
        public readonly int y;

        /// <summary>
        /// The width of the rectangle.
        /// </summary>
        public readonly int width;
        
        /// <summary>
        /// The height of the rectangle.
        /// </summary>
        public readonly int height;

        public Rectangle(int x, int y, int width, int height)
        {
            if(width <= 0)
            {
                throw new ArgumentException(string.Format("negative width: {0}", width));
            }

            if(height <= 0)
            {
                throw new ArgumentException(string.Format("negative height: {0}", height));
            }

            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public bool Contains(Point point)
        {
            return (x <= point.x && x + width - 1 >= point.x) &&
               (y <= point.y && y + height - 1 >= point.y);
        }

        public bool ContainsAll(IEnumerable<Point> points)
        {
            return !points.Any(point => !Contains(point));
        }
    }
}
