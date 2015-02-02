using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SpaceChemSolver.Simulation
{
    /// <summary>
    /// Represents an atom in an reactor.
    /// 
    /// Notes:
    /// * An atom may be bonded to other atoms in each direction (the graph of reachable 
    /// atoms is called the atom's molecule).
    /// * Modifying the position of an atom moves all the atoms in the molecule.
    /// </summary>
    public class Atom
    {
        /// <summary>
        /// The id of this atom.
        /// </summary>
        public readonly Guid id;

        /// <summary>
        /// The x position of the atom.
        /// </summary>
        private int x;

        /// <summary>
        /// The y position of the atom.
        /// </summary>
        private int y;

        /// <summary>
        /// The (x, y) position of the atom.
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
        /// The atom attributes (its name, maximum number of bonds etc.)
        /// </summary>
        public AtomAtrributes Attributes { get; private set; }

        /// <summary>
        /// The backing field for <see cref="StringRepresentation"/>
        /// </summary>
        private string stringRepresentation;

        /// <summary>
        /// A string representation of this atom and its bonds.
        /// 
        /// Example 1: if the atom is hydrogen, its string representation is "h"
        /// 
        /// Example 2: if the atom is hydrogen and it has a double bond with a nitrogen atom
        /// and a single bond with an oxygen atom, the string representation is "h-n2o1".
        /// </summary>
        private string StringRepresentation
        {
            get
            {
                if (stringRepresentation == null)
                {
                    var name = new StringBuilder();
                    name.Append(Attributes.name);

                    var bondsNames = new List<string>();
                    foreach (var bond in bonds.Values)
                    {
                        if (bond != null)
                        {
                            var bondName = string.Format("{1}{0}", bond.strength, bond.GetBondedAtom(this).Attributes.name);
                            bondsNames.Add(bondName);
                        }
                    }

                    if(bondsNames.Count != 0)
                    {
                        name.Append("-");
                    }

                    bondsNames.Sort();
                    foreach (var bondName in bondsNames)
                    {
                        name.Append(bondName);
                    }

                    stringRepresentation = name.ToString();
                }

                return stringRepresentation;
            }
        }

        /// <summary>
        /// The backing field for <see cref="AtomGraph"/>.
        /// </summary>
        private IEnumerable<Atom> atomGraph;

        /// <summary>
        /// The atoms reachable from the bonds of this atom (including this atom). 
        /// </summary>
        private IEnumerable<Atom> AtomGraph
        {
            get
            {
                if (atomGraph == null)
                {
                    // Perform a breadth first search to find all connected atoms.
                    atomGraph = Algorithms.BFS(this, (atom) =>
                    {
                        var neighbors = new List<Atom>();
                        foreach (var bond in atom.bonds.Values)
                        {
                            if (bond != null)
                            {
                                neighbors.Add(bond.GetBondedAtom(atom));
                            }
                        }
                        return neighbors;
                    });
                }
                return atomGraph;
            }
        }

        /// <summary>
        /// The molecule this atom is part of.
        /// </summary>
        public Molecule Molecule
        {
            get
            {
                return new Molecule(AtomGraph);
            }
        }

        /// <summary>
        /// The atoms this atom is bonded to in each direction.
        /// </summary>
        private Dictionary<Direction, Bond> bonds;   

        public Atom(Parse.Atom atom) : this(atom.x, atom.y, atom.name) { }

        public Atom(int x, int y, string atomName) :
            this(x, y, AtomAttributeFactory.Generate(atomName)) { }

        private Atom(int x, int y, AtomAtrributes attributes)
        {
            this.id = Guid.NewGuid();
            this.x = x;
            this.y = y;
            this.Attributes = attributes;
            this.bonds = new Dictionary<Direction, Bond>();

            foreach (Direction direction in (Direction[])Enum.GetValues(typeof(Direction)))
            {
                bonds[direction] = null;
            }
        }

        private Atom(int x, int y, AtomAtrributes attributes, Dictionary<Direction, Bond> bonds) :
            this(x, y, attributes)
        {
            this.bonds = bonds;
        }

        internal void Step(Direction direction)
        {
            foreach(var atom in AtomGraph)
            {
                atom.Position = atom.Position.Project(direction);
            }
        }

        private void SetBond(Direction direction, Bond bond)
        {
            // Force refresh of the string representation and atom graph.
            stringRepresentation = null;
            atomGraph = null;
            // Set the bond.
            bonds[direction] = bond;
        } 

        public void AddBond(Atom other)
        {
            AddBond(other, Position.DirectionTo(other.Position));
        }

        public void AddBond(Atom other, Direction direction)
        {
            var currentBond = bonds[direction];
            Bond newBond = null;

            if(currentBond == null)
            {
                newBond = new Bond(this, other);
            }
            else
            {
                newBond = currentBond.Strengthen();
            }

            SetBond(direction, newBond);
            other.SetBond(direction.GetOppositeDirection(), newBond);
        }

        public void RemoveBond(Atom other)
        {
            RemoveBond(other, Position.DirectionTo(other.Position));
        }

        public void RemoveBond(Atom other, Direction direction)
        {
            var weakenedBond = bonds[direction].Weaken();
            SetBond(direction, weakenedBond);
            SetBond(direction.GetOppositeDirection(), weakenedBond);
        }

        public override bool Equals(object obj)
        {
            Atom other = obj as Atom;
            if(other == null)
            {
                return false;
            }

            return id.Equals(other.id);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public override string ToString()
        {
            return StringRepresentation;
        }
    }
}
