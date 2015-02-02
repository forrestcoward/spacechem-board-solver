using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceChemSolver.Simulation
{
    public static class Utility
    {
        /// <summary>
        /// Gets the direction from first atom to the second atom, or null
        /// if the two atoms are not a distance of one apart.
        /// </summary>
        /// <param name="first">the first atom</param>
        /// <param name="second">the second atom</param>
        /// <returns>the direction from first to second, or null if the atoms
        /// are not a distance of one apart</returns>
        public static Direction? GetBondDirectionFirstToSecond(Atom first, Atom second)
        {
            var p1 = first.Position;
            var p2 = second.Position;

            if (p1.x != p2.x)
            {
                if (p1.x == p2.x - 1)
                {
                    return Direction.Left;
                }
                else if (p1.x + 1 == p2.x)
                {
                    return Direction.Right;
                }
            }
            else if (p1.y != p2.y)
            {
                if (p1.y == p2.y + 1)
                {
                    return Direction.Up;
                }
                else if (p1.y + 1 == p2.y)
                {
                    return Direction.Down;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the opposite direction
        /// </summary>
        /// <param name="direction">the direction</param>
        /// <returns>the opposite direction</returns>
        public static Direction GetOppositeDirection(this Direction direction)
        {
            Contract.Requires(direction != Direction.Continue);

            if (direction == Direction.Up)
            {
                return Direction.Down;
            }
            else if (direction == Direction.Down)
            {
                return Direction.Up;
            }
            else if (direction == Direction.Right)
            {
                return Direction.Left;
            }
            else
            {
                return Direction.Right;
            }
        }
    }
}
