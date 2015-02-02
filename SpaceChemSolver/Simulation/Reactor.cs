using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using SpaceChemSolver.Parse;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceChemSolver.Simulation
{
    public enum WaldoType
    {
        Alpha,
        Beta
    }

    public enum Zone
    {
        InputAlpha,
        OutputAlpha,
        InputBeta,
        OutputBeta,
        Empty
    }

    public enum Instruction
    {
        InAlpha,
        OutAlpha,
        InBeta,
        OutBeta,
        Grab,
        Drop,
        GrabDrop,
        Sync,
        AddBond,
        RemoveBond,
        RotateClockwise,
        RotateCounterClockwise,
        Empty
    }

    public enum Direction
    {
        Up,
        Down,
        Right,
        Left,
        Continue
    }

    public class Input
    {
        /// <summary>
        /// The molecular blueprint for generating this input.
        /// </summary>
        private MoleculeBlueprint blueprint;

        /// <summary>
        /// The probability this input is generated.
        /// </summary>
        public double Probability { get; private set; }

        public Input(Parse.Input input)
        {
            blueprint = new MoleculeBlueprint(input);
            Probability = input.probability;
        }

        /// <summary>
        /// Generates a new input molecule and returns one of its atoms.
        /// </summary>
        /// <returns>an atom in the created molecule</returns>
        public Atom Generate()
        {
            return blueprint.Generate();
        }
    }

    public class Output
    {
        public int Target { get; private set; }

        public Atom ExpectedAtom { get; private set; }

        public Output(Parse.Output output)
        {
            Target = output.target;
            ExpectedAtom = MoleculeBlueprint.Generate(output);
        }     
    }

    public class InputFactory
    {
        private List<Input> inputs;

        public InputFactory(IEnumerable<Parse.Input> _inputs)
        {
            inputs = new List<Input>();

            foreach(Parse.Input _input in _inputs)
            {
                this.inputs.Add(new Input(_input));
            }
        }

        public Atom Generate()
        {
            // TODO: Consider probabilities when generating inputs.
            return inputs[0].Generate();
        }
    }

    public class Reactor
    {
        private Tile[,] tiles;
        
        public int Width { get; private set; }

        public int Height { get; private set; }

        public Rectangle Bound
        {
            get
            {
                return new Rectangle(0, 0, Width, Height);
            }
        }

        public StartInstruction AlphaStart { get; private set; }

        public StartInstruction BetaStart { get; private set; }

        public InputFactory AlphaInput { get; private set; }

        public InputFactory BetaInput { get; private set; }

        public Output AlphaOutput { get; private set; }

        public Output BetaOutput { get; private set; }

        public InputFactory GetInputFactory(WaldoType type)
        {
            return type == WaldoType.Alpha ? 
                AlphaInput : BetaInput;
        }

        // Define the input and outzone for molecules.
        public static Rectangle inZoneAlpha;
        public static Rectangle outZoneAlpha;
        public static Rectangle inZoneBeta;
        public static Rectangle outZoneBeta;

        private static Zone ClassifyAsZone(Point point)
        {
            if (inZoneAlpha.Contains(point)) return Zone.InputAlpha;
            if (inZoneBeta.Contains(point)) return Zone.InputBeta;
            if (outZoneAlpha.Contains(point)) return Zone.OutputAlpha;
            if (outZoneBeta.Contains(point)) return Zone.OutputBeta;
            return Zone.Empty;
        }

        public TileAttributes GetTileAttributes(int x, int y)
        {
            return GetTileAttributes(new Point(x, y));
        }

        public TileAttributes GetTileAttributes(Point point)
        {
            if(!Bound.Contains(point))
            {
                throw new ArgumentException(
                    string.Format("No tile exists at position {0}", point));
            }

            return tiles[point.x, point.y].Attributes;
        }

        public static Reactor Parse(string jsonFile)
        {
            Parse.Reactor reactor = ReactorParser.Parse(jsonFile);
            return new Reactor(reactor);
        }

        private Reactor(Parse.Reactor reactor)
        {
            int width = reactor.width;
            int height = reactor.height;

            Width = width;
            Height = height;

            inZoneAlpha = new Rectangle(0, 0, 4, 4);
            outZoneAlpha = new Rectangle(6, 0, 4, 4);
            inZoneBeta = new Rectangle(0, 4, 4, 4);
            outZoneBeta = new Rectangle(6, 4, 4, 4);

            tiles = new Tile[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var zone = ClassifyAsZone(new Point(i, j));
                    tiles[i, j] = new Tile(i, j, zone);
                }
            }

            foreach(Parse.Tile tile in reactor.tiles)
            {
                tiles[tile.x, tile.y].Set(tile);
            }

            AlphaStart = new StartInstruction(reactor.starts.alpha);
            BetaStart = new StartInstruction(reactor.starts.beta);

            AlphaInput = new InputFactory(reactor.inputs.alpha);
            BetaInput = new InputFactory(reactor.inputs.beta);

            if (reactor.outputs.IsAlphaOutput)
            {
                AlphaOutput = new Output(reactor.outputs.alpha);
            }

            if (reactor.outputs.IsBetaOutput)
            {
                BetaOutput = new Output(reactor.outputs.beta);
            }
        }
    }
}
