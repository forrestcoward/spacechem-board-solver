using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceChemSolver.Simulation;

namespace SpaceChemSolver.Tests
{
    [TestClass]
    public class SimulationTests
    {
        [TestMethod]
        public void SimpleSimulationTest()
        {
            var reactor = Reactor.Parse("reactors\\simple1.json");
            var state = new GameState(reactor);

            for (int step = 1; step <= 5; step++)
            {
                state.Step();
            }

            var alpha = state.Alpha;
            Assert.IsTrue(alpha.IsProjectedOutOfBounds);
            Assert.IsTrue(!alpha.HasAtom);
            Assert.AreEqual<Point>(new Point(0, 5), alpha.Position);

            state.Step();
        }

        [TestMethod]
        public void SimpleAlphaLoop()
        {
            var reactor = Reactor.Parse("reactors\\simple-alpha-loop.json");
            var state = new GameState(reactor);
            state.Step(30);
        }
    }
}
