
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SpaceChemSolver.Simulation
{
    public class MoleculeBlueprint
    {
        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(atoms != null);
            Contract.Invariant(Contract.ForAll(atoms, atom => atom != null));
            Contract.Invariant(bonds != null);
            Contract.Invariant(Contract.ForAll(bonds, bond => bond != null));
        }

        /// <summary>
        /// Describes the atoms in the molecule.
        /// </summary>
        IEnumerable<Parse.Atom> atoms;

        /// <summary>
        /// Describes the bonds in the molecule.
        /// </summary>
        IEnumerable<Parse.Bond> bonds;

        private MoleculeBlueprint(IEnumerable<Parse.Atom> atoms, IEnumerable<Parse.Bond> bonds)
        {
            this.atoms = atoms;
            this.bonds = bonds;
        }

        public MoleculeBlueprint(Parse.IMolecule blueprint) : this(blueprint.Atoms, blueprint.Bonds) { }

        private static Atom Generate(IEnumerable<Parse.Atom> atoms, IEnumerable<Parse.Bond> bonds)
        {
            Contract.Requires(atoms != null);
            Contract.Requires(Contract.ForAll(atoms, atom => atom != null));
            Contract.Requires(bonds != null);
            Contract.Requires(Contract.ForAll(bonds, bond => bond != null));
            Contract.Ensures(Contract.Result<Atom>() != null);

            var addedAtoms = new Dictionary<Point, Atom>();

            foreach (Parse.Atom atom in atoms)
            {
                Atom newAtom = new Atom(atom);
                addedAtoms.Add(newAtom.Position, newAtom);
            }

            foreach (Parse.Bond bond in bonds)
            {
                var a = new Point(bond.x1, bond.y1);
                var b = new Point(bond.x2, bond.y2);

                if (addedAtoms.ContainsKey(a) && addedAtoms.ContainsKey(b))
                {
                    addedAtoms[a].AddBond(addedAtoms[b]);
                }
            }

            return addedAtoms.Values.First();
        }

        /// <summary>
        /// Creates the molecule this blueprint represents and returns one of the atoms that composes it.
        /// </summary>
        /// <returns>an atom in the created molecule</returns>
        public Atom Generate()
        {
            return Generate(atoms, bonds);
        }

        /// <summary>
        /// Creates a molecule from the specified blueprint and returns one of the atoms that composes it.
        /// </summary>
        /// <param name="input">the parsed molecule blueprint</param>
        /// <returns>an atom in the created molecule</returns>
        public static Atom Generate(Parse.IMolecule blueprint)
        {
            Contract.Requires(blueprint != null);
            return Generate(blueprint.Atoms, blueprint.Bonds);
        }
    }
}
