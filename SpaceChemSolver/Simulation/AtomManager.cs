using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SpaceChemSolver.Simulation
{
    public class AtomManager
    {
        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(atoms != null);
            Contract.Invariant(Contract.ForAll(atoms, atom => atom != null));
        }

        private List<Atom> atoms;

        /// <summary>
        /// Constructs a new atom manager with no atoms.
        /// </summary>
        public AtomManager()
        {
            atoms = new List<Atom>();
        }

        private IEnumerable<Molecule> Molecules
        {
            get
            {
                var molecules = new HashSet<Molecule>();
                foreach(Atom atom in atoms)
                {
                    molecules.Add(atom.Molecule);
                }
                return molecules;
            }
        }

        /// <summary>
        /// Adds the atoms of the molecule to this manager. If an atom already exists
        /// at the position of one of the molecule's atoms, then no atoms are added.
        /// </summary>
        /// <param name="molecule">the molecule</param>
        /// <returns>true if the molecule's atoms were added, false otherwise</returns>
        public bool Add(Atom atom)
        {
            Molecule molecule = atom.Molecule;

            foreach(Atom _atom in molecule.Atoms)
            {
                if (IsAtomAtPosition(_atom.Position))
                {
                    return false;
                }
            }

            foreach(Atom _atom in molecule.Atoms)
            {
                atoms.Add(_atom);
            }

            return true;
        }

        /// <summary>
        /// Gets the atom at the specified position.
        /// </summary>
        /// <param name="position">the position to query</param>
        /// <returns>the atom at the specified position, or null</returns>
        public Atom GetAtomAtPosition(Point position)
        {
            return atoms.FirstOrDefault(atom => atom.Position.Equals(position));
        }

        /// <summary>
        /// Checks if an atom occupies the specified position.
        /// </summary>
        /// <param name="position">the position</param>
        /// <returns>true if an atom occupies the specified position</returns>
        public bool IsAtomAtPosition(Point position)
        {
            return GetAtomAtPosition(position) != null;
        }

        public IEnumerable<Molecule> GetMoleculesInBounds(Rectangle bound)
        {
            var molecules = new List<Molecule>();

            return molecules;
        }

        public bool RemoveTargetOuput(Molecule target, Rectangle bound)
        {
            foreach(Molecule molecule in Molecules)
            {
                if(bound.ContainsAll(molecule.AtomCoordinates))
                {
                    if(molecule.Equals(target))
                    {
                        foreach(Atom atom in molecule.Atoms)
                        {
                            this.atoms.Remove(atom);
                        }

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
