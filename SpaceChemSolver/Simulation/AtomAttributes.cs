using System.Diagnostics.Contracts;

namespace SpaceChemSolver.Simulation
{
    /// <summary>
    /// Represents the attributes of an element (name, maximum bonds etc.)
    /// </summary>
    public class AtomAtrributes
    {
        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(maximumBonds >= 1);
            Contract.Invariant(!string.IsNullOrEmpty(name)); 
            Contract.Invariant(!string.IsNullOrEmpty(fullName));  
        }

        /// <summary>
        /// The short name of the atom (i.e. "H")
        /// </summary>
        public readonly string name;

        /// <summary>
        /// The full name of the atom (i.e. "Hydrogen")
        /// </summary>
        public readonly string fullName;

        /// <summary>
        /// The maximum number of bonds this atom can have.
        /// </summary>
        public readonly int maximumBonds;

        public AtomAtrributes(string name, string fullName, int maximumBonds)
        {
            this.name = name.ToLower();
            this.fullName = fullName.ToLower();
            this.maximumBonds = maximumBonds;
        }
    }
}
