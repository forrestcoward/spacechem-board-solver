using System;
using System.Diagnostics.Contracts;

namespace SpaceChemSolver.Simulation
{
    /// <summary>
    /// Represents a bond between two atoms.
    /// A bond is immutable (though its atoms can be modified) - strengthening 
    /// or weakening a bond returns a new bond.
    /// </summary>
    public class Bond
    {
        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(strength >= 1);
            Contract.Invariant(first != null);
            Contract.Invariant(second != null);
            Contract.Invariant(first.Position.DistanceFrom(second.Position) == 1);
        }

        /// <summary>
        /// The strength of the bond (i.e. the number of bonds).
        /// </summary>
        public readonly int strength;

        /// <summary>
        /// The first atom of the bond.
        /// </summary>
        private readonly Atom first;

        /// <summary>
        /// The second atom of the bond.
        /// </summary>
        private readonly Atom second;

        public Bond(Atom first, Atom second) : this(first, second, 1) { }

        private Bond(Atom first, Atom second, int strength)
        {
            this.first = first;
            this.second = second;
            this.strength = strength;
        }

        public Bond Strengthen()
        {
            if (first.Attributes.maximumBonds <= strength + 1 &&
               second.Attributes.maximumBonds <= strength + 1)
            {
                return new Bond(first, second, strength + 1);
            }
            else
            {
                return this;
            }
        }

        public Bond Weaken()
        {
            if (strength <= 1)
            {
                return null;
            }
            else
            {
                return new Bond(first, second, strength - 1);
            }
        }

        public Atom GetBondedAtom(Atom atom)
        {
            if (first.Equals(atom))
            {
                return second;
            }
            else if (second.Equals(atom))
            {
                return first;
            }

            throw new ArgumentException(
                string.Format("bond does not contains element '{0}'", atom.id.ToString()));
        }
    }
}
