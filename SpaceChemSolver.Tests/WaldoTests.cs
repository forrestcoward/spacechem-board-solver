using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceChemSolver.Simulation;
using System;

namespace SpaceChemSolver.Tests
{
    [TestClass]
    public class WaldoTests
    {
        [TestMethod]
        public void ReactorParseTest()
        {
            var reactor = Reactor.Parse("reactors\\simple1.json");

            Assert.AreEqual(10, reactor.Width);
            Assert.AreEqual(8, reactor.Height);
            Assert.AreEqual(reactor.AlphaStart.x, 5);
            Assert.AreEqual(reactor.AlphaStart.y, 5);
            Assert.AreEqual(reactor.AlphaStart.direction, Direction.Left);
            Assert.AreEqual(reactor.BetaStart.x, 5);
            Assert.AreEqual(reactor.BetaStart.y, 6);
            Assert.AreEqual(reactor.BetaStart.direction, Direction.Up);

            var tile = reactor.GetTileAttributes(0, 3);
            Assert.AreEqual(Instruction.Grab, tile.instructionBeta);

            tile = reactor.GetTileAttributes(6, 6);
            Assert.AreEqual(Instruction.Empty, tile.instructionAlpha);
            Assert.AreEqual(Instruction.Empty, tile.instructionBeta);
            Assert.AreEqual(Direction.Continue, tile.directionAlpha);
            Assert.AreEqual(Direction.Continue, tile.directionBeta);

            var atom1 = new Atom(2, 2, "hydrogen");
            var atom2 = new Atom(2, 3, "oxygen");
            atom1.AddBond(atom2);

            var input = reactor.AlphaInput.Generate();

            Assert.IsTrue(input.Molecule.Equals(atom1.Molecule));

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReactorIllegalTileQueryTest()
        {
            var reactor = Reactor.Parse("reactors\\simple1.json");
            reactor.GetTileAttributes(0, -1);
        }
    }
}
