using SpaceChemSolver.Simulation;

namespace SpaceChemSolver.Parse
{
    public static class InstructionMapper
    {
        private static Map<string, Instruction> instructions;
        private static Map<string, Direction> directions;

        static InstructionMapper()
        {
            instructions = new Map<string, Instruction>();
            directions = new Map<string, Direction>();
            instructions.Add("in-alpha", Instruction.InAlpha);
            instructions.Add("out-alpha", Instruction.OutAlpha);
            instructions.Add("in-beta", Instruction.InBeta);
            instructions.Add("out-beta", Instruction.OutBeta);
            instructions.Add("grab", Instruction.Grab);
            instructions.Add("drop", Instruction.Drop);
            instructions.Add("grab-drop", Instruction.GrabDrop);
            instructions.Add("sync", Instruction.Sync);
            instructions.Add("add-bond", Instruction.AddBond);
            instructions.Add("remove-bond", Instruction.RemoveBond);
            instructions.Add("rotate-clockwise", Instruction.RotateClockwise);
            instructions.Add("rotate-counter-clockwise", Instruction.RotateCounterClockwise);
            instructions.Add("empty", Instruction.Empty);
            directions.Add("up", Direction.Up);
            directions.Add("down", Direction.Down);
            directions.Add("right", Direction.Right);
            directions.Add("left", Direction.Left);
            directions.Add("continue", Direction.Continue);
        }

        public static Instruction GetInstruction(string instruction)
        {
            return instructions.Forward[instruction];
        }

        public static string GetInstruction(Instruction instruction)
        {
            return instructions.Reverse[instruction];
        }

        public static Direction GetDirection(string movement)
        {
            return directions.Forward[movement];
        }

        public static string GetMovement(Direction movement)
        {
            return directions.Reverse[movement];
        }
    }
}
