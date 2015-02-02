using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SpaceChemSolver.Simulation;
using System.Collections.Generic;

namespace SpaceChemSolver.Parse
{
    /*
     * The object model for parsing json reactor files.
     * 
     * At first I used http://json2csharp.com/ and then I modified from there.
     */

    public class Start
    {
        public int x;
        public int y;
        [JsonConverter(typeof(DirectionConverter))]
        public Direction direction;
    }

    public class Starts
    {
        public Start alpha;
        public Start beta;
    }

    public class Tile
    {
        public int x;
        public int y;
        [JsonConverter(typeof(InstructionConverter))]
        public Instruction instruction;
        [JsonConverter(typeof(DirectionConverter))]
        public Direction direction;
        [JsonConverter(typeof(StringEnumConverter))]
        public WaldoType waldo;

        public Tile()
        {
            instruction = Instruction.Empty;
            direction = Direction.Continue;
        }
    }

    public class Atom
    {
        public int x;
        public int y;
        public string name;
    }

    public class Bond
    {
        public int x1;
        public int y1;
        public int x2;
        public int y2;
        public int strength;

        public bool IsValid
        {
            get
            {
                return (new Point(x1, y1).DistanceFrom(new Point(x2, y2)) == 1);
            }
        }
    }

    public class Input : IMolecule
    {
        public double probability;
        public List<Atom> atoms;
        public List<Bond> bonds;

        public IEnumerable<Atom> Atoms
        {
            get
            {
                return atoms;
            }
        }

        public IEnumerable<Bond> Bonds
        {
            get
            {
                return bonds;
            }
        }
    }

    public class Inputs
    {
        public List<Input> alpha;
        public List<Input> beta;

        public Inputs()
        {
            alpha = new List<Input>();
            beta = new List<Input>();
        }
    }

    public class Output : IMolecule
    {
        public int target;
        public List<Atom> atoms;
        public List<Bond> bonds;

        public IEnumerable<Atom> Atoms
        {
            get
            {
                return atoms;
            }
        }

        public IEnumerable<Bond> Bonds
        {
            get
            {
                return bonds;
            }
        }
    }

    public class Outputs
    {
        public Output alpha;
        public Output beta;

        /// <summary>
        /// True if there is expected alpha-zone output.
        /// </summary>
        public bool IsAlphaOutput
        {
            get
            {
                return alpha != null;
            }
        }

        /// <summary>
        /// True if there is expected beta-zone output.
        /// </summary>
        public bool IsBetaOutput
        {
            get
            {
                return beta != null;
            }
        }
    }

    public class Reactor
    {
        public int width;
        public int height;
        public Starts starts;
        public List<Tile> tiles;
        public Inputs inputs;
        public Outputs outputs;
    }

    public class ReactorRoot
    {
        public Reactor reactor;
    }
    
    public interface IMolecule
    {
        IEnumerable<Parse.Atom> Atoms { get; }
        IEnumerable<Parse.Bond> Bonds { get; }
    }
}
