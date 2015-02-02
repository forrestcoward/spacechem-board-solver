using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceChemSolver.Simulation
{
    /// <summary>
    /// Represents a waldo in a reactor. 
    /// A waldo moves along its projected path and executes instructions along that 
    /// path. The waldo may carry an atom (which may be bonded to other atoms).
    /// </summary>
    public class Waldo
    {
        /// <summary>
        /// The x position of the waldo.
        /// </summary>
        private int x;

        /// <summary>
        /// The y position of the waldo.
        /// </summary>
        private int y;

        /// <summary>
        /// The grid the waldo is confined to.
        /// </summary>
        private Rectangle bound;

        /// <summary>
        /// The type of waldo (alpha or beta).
        /// </summary>
        public WaldoType Type { get; private set; }

        /// <summary>
        /// The (x, y) position of the waldo.
        /// </summary>
        public Point Position
        {
            get
            {
                return new Point(x, y);
            }
            private set
            {
                x = value.x;
                y = value.y;
            }
        }

        /// <summary>
        /// The (x, y) positions of the molecule this atom is holding.
        /// </summary>
        public IEnumerable<Point> AtomPositions
        {
            get
            {
                if (Atom == null)
                {
                    return new List<Point>();
                }
                else
                {
                    return Atom.Molecule.AtomCoordinates;
                }
            }
        }

        /// <summary>
        /// True if the waldo is projected out of bounds after next step.
        /// </summary>
        public bool IsProjectedOutOfBounds
        {
            get
            {
                var projectedPosition = Position.Project(CurrentDirection);
                return !bound.Contains(projectedPosition);
            }
        }

        /// <summary>
        /// The position of the waldo after a single step.
        /// </summary>
        private Point ProjectedPosition
        {
            get
            {
                return IsProjectedOutOfBounds ? Position : Position.Project(CurrentDirection);
            }
        }

        /// <summary>
        /// Backing field for <see cref="CurrentDirection"/>.
        /// </summary>
        private Direction currentDirection;

        /// <summary>
        /// Gets or sets the current direction of the waldo.
        /// </summary>
        /// <remarks>
        /// Direction.Continue does not change the current direction.
        /// </remarks>
        public Direction CurrentDirection
        {
            get
            {
                return currentDirection;
            }
            set
            {
                if(Direction.Continue != value)
                {
                    currentDirection = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the currently held atom, or null if no molecule is held.
        /// </summary>
        public Atom Atom { get; set; }

        /// <summary>
        /// True if this Waldo is holding an atom.
        /// </summary>
        public bool HasAtom
        {
            get
            {
                return Atom != null;
            }
        }

        public bool InLegalState
        {
            get
            {
                return bound.ContainsAll(AtomPositions);
            }
        }
       
        /// <summary>
        /// Creates a new waldo with the specified (x,y) coordinate and starting direction.
        /// </summary>
        /// <param name="start">the start information for the waldo</param>
        /// <param name="bound">the rectangular grid the waldo is confined to</param>
        public Waldo(StartInstruction start, Rectangle bound, WaldoType type)
        {
            this.x = start.x;
            this.y = start.y;
            this.bound = bound;
            this.Type = type;
            this.CurrentDirection = start.direction;
            this.Atom = null;
        }

        /// <summary>
        /// Moves the Waldo and the atoms its holding a single step. 
        /// </summary>
        /// <remarks>
        /// The Waldo and its molecule will not move if the Waldo is projected out of bounds.
        /// </remarks>
        public void Step()
        {
            bool wasProjectedOutOfBounds = IsProjectedOutOfBounds;
            Position = ProjectedPosition;
            if(!wasProjectedOutOfBounds && HasAtom)
            {
                Atom.Step(CurrentDirection);
            }
        }
    }
}
