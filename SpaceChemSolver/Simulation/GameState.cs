using System;
using System.Collections.Generic;

namespace SpaceChemSolver.Simulation
{
    public class GameState
    {
        public Reactor Reactor { get; set; }

        /// <summary>
        /// The alpha waldo.
        /// </summary>
        public Waldo Alpha { get; private set; }

        /// <summary>
        /// The beta waldo.
        /// </summary>
        public Waldo Beta { get; private set; }

        /// <summary>
        /// True if the simulation has started, false otherwise.
        /// </summary>
        public bool HasStarted
        {
            get
            {
                return Cycles == 0;
            }
        }

        /// <summary>
        /// The number of steps that have occured during simulation.
        /// </summary>
        public int Cycles { get; private set; }

        public int AlphaTarget { get; private set; }

        public int BetaTarget { get; private set; }

        private AtomManager atomManager;

        public GameState(Reactor reactor)
        {
            Reactor = reactor;
            Reset();
        }

        public void Reset()
        {
            Cycles = 0;
            AlphaTarget = 0;
            BetaTarget = 0;
            Alpha = new Waldo(Reactor.AlphaStart, Reactor.Bound, WaldoType.Alpha);
            Beta = new Waldo(Reactor.BetaStart, Reactor.Bound, WaldoType.Beta);
            atomManager = new AtomManager();
        }

        public bool Step(int steps)
        {
            bool success = true;
            for(int i = 1; i <= steps; i++)
            {
                success &= Step();
            }
            return success;
        }

        public bool Step()
        {
            bool proceed;

            proceed = Step(Alpha);
            if (!proceed) return false;

            /*
            proceed = Step(Beta);
            if (!proceed) return false;
             */


            Cycles++;

            return true;
        }

        private bool Step(Waldo waldo)
        {
            // Move the waldo and its atom.
            waldo.Step();

            // Check for a collision and then process tile instructions.
            if(!IsCollision(waldo))
            {
                if(DoTile(waldo))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsCollision(Waldo waldo)
        {
            Waldo other = waldo.Type == WaldoType.Alpha ? Beta : Alpha;
            bool moleculeCollision = waldo.HasAtom && atomManager.IsAtomAtPosition(waldo.Position);
            bool waldoCollision = waldo.HasAtom && other.HasAtom && waldo.Position.Equals(other.Position);
            return moleculeCollision && waldoCollision;
        }
 
        private bool DoTile(Waldo waldo)
        {
            var type = waldo.Type;
            TileAttributes tile = Reactor.GetTileAttributes(waldo.Position);
            var commands = tile.GetCommands(type);
            Direction direction = commands.Item1;
            Instruction instruction = commands.Item2;

            // Set the direction for the next step.
            waldo.CurrentDirection = direction;

            // Process the instruction.
            switch (instruction)
            {
                case Instruction.InAlpha:
                    return InputMolecule(WaldoType.Alpha);
                case Instruction.InBeta:
                    return InputMolecule(WaldoType.Beta);
                case Instruction.Grab:
                    return GrabAtom(waldo);
                case Instruction.Drop:
                    return DropAtom(waldo);
                case Instruction.GrabDrop:
                    return GrabOrDropAtom(waldo);
                case Instruction.OutAlpha:
                    return RemoveOutput(WaldoType.Alpha);
                case Instruction.OutBeta:
                    return RemoveOutput(WaldoType.Beta);
                case Instruction.RotateClockwise:
                    return Rotate(waldo, true);
                case Instruction.RotateCounterClockwise:
                    return Rotate(waldo, false);
                case Instruction.RemoveBond:
                case Instruction.AddBond:
                case Instruction.Sync:
                case Instruction.Empty:
                default:
                    return true;
            }
        }

        private bool Rotate(Waldo waldo, bool clockwise)
        {
            return true;
        }

        private bool RemoveOutput(WaldoType type)
        {
            if(type == WaldoType.Alpha)
            {
                return atomManager.RemoveTargetOuput(Reactor.AlphaOutput.ExpectedAtom.Molecule, Reactor.outZoneAlpha);
            }
            else
            {
                return atomManager.RemoveTargetOuput(Reactor.BetaOutput.ExpectedAtom.Molecule, Reactor.outZoneBeta);
            }
        }

        private bool InputMolecule(WaldoType type)
        {
            InputFactory factory = Reactor.GetInputFactory(type);
            Atom atom = factory.Generate();
            return atomManager.Add(atom);
        }

        private bool GrabAtom(Waldo waldo)
        {
            if(!waldo.HasAtom)
            {
                Atom grab = atomManager.GetAtomAtPosition(waldo.Position);
                waldo.Atom = grab;
            }

            return true;
        }

        private bool GrabOrDropAtom(Waldo waldo)
        {
            if(waldo.HasAtom)
            {
                return GrabAtom(waldo);
            }
            else
            {
                return DropAtom(waldo);
            }
        }

        private bool DropAtom(Waldo waldo)
        {
            waldo.Atom = null;
            return true;
        }
    }
}
