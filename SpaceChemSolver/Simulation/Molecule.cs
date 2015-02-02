using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SpaceChemSolver.Simulation
{
    /// <summary>
    /// A molecule is collection of bonded atoms. Two molecules are equal if they contain
    /// the same number of atoms with the same bond structure (orientation does not matter).
    /// </summary>
    public class Molecule
    {
        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(Atoms != null);
            Contract.Invariant(Contract.ForAll(Atoms, atom => atom != null));
        }

        public IEnumerable<Atom> Atoms { get; private set; }

        public IEnumerable<Point> AtomCoordinates
        {
            get
            {
                return Atoms.Select(_atom => _atom.Position);
            }
        }

        private List<string> SortedAtomStrings
        {
            get
            {
                List<string> atoms = Atoms.Select(atom => atom.ToString()).ToList();
                atoms.Sort();
                return atoms;
            }
        }

        [DebuggerStepThrough]
        public Molecule(IEnumerable<Atom> atoms)
        {
            Atoms = atoms;
        }

        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            Molecule other = obj as Molecule;
            if (other == null)
            {
                return false;
            }

            return Algorithms.ListEquals<string>(SortedAtomStrings, other.SortedAtomStrings);
        }

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            int hash = 7;
            foreach(string atom in SortedAtomStrings)
            {
                hash = 17 * atom.GetHashCode();
            }
            return hash;
        }

        [DebuggerStepThrough]
        public override string ToString()
        {
            var temp = SortedAtomStrings.Select(x => string.Format("[{0}]", x));
            return string.Join("", temp);
        }
    }
}
