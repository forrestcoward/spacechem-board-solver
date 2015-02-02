using System.Diagnostics.Contracts;

namespace SpaceChemSolver.Simulation
{
    public class StartInstruction
    {
        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(x >= 0);
            Contract.Invariant(y >= 0);
            Contract.Invariant(direction != Direction.Continue);
        }

        public readonly int x;
        public readonly int y;
        public readonly Direction direction;

        public StartInstruction(Parse.Start start) : this(start.x, start.y, start.direction) { }

        private StartInstruction(int x, int y, Direction direction)
        {
            this.x = x;
            this.y = y;
            this.direction = direction;
        }
    }
}
