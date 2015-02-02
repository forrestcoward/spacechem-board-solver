using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceChemSolver.Simulation;

namespace SpaceChemSolver.Tests
{
    [TestClass]
    public class AtomTests
    {
        [TestMethod]
        public void ConnectedAtomsTest()
        {
            var atom1 = new Atom(2, 2, "hydrogen");
            var atom2 = new Atom(2, 3, "oxygen");
            var atom3 = new Atom(2, 1, "nitrogen");
            atom1.AddBond(atom2);
            atom1.AddBond(atom3);
            var m1 = atom1.Molecule;

            var atom4 = new Atom(2, 2, "hydrogen");
            var atom5 = new Atom(2, 1, "oxygen");
            var atom6 = new Atom(2, 3, "nitrogen");
            atom5.AddBond(atom4);
            atom4.AddBond(atom6);
            var m2 = atom4.Molecule;
            var m3 = atom5.Molecule;

            Assert.IsTrue(m1.Equals(m2));
            Assert.IsTrue(m1.Equals(m3));
            Assert.IsTrue(m2.Equals(m3));
        }
    }
}
