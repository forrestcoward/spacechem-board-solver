using System;

namespace SpaceChemSolver.Simulation
{
    /// <summary>
    /// Represents an immutable view of a tile.
    /// </summary>
    public class TileAttributes
    {
        public readonly int x;
        public readonly int y;
        public readonly Zone zone;
        public readonly Direction directionAlpha;
        public readonly Direction directionBeta;
        public readonly Instruction instructionAlpha;
        public readonly Instruction instructionBeta;
        public readonly bool hasBonder;

        public TileAttributes(Tile tile)
        {
            x = tile.x;
            y = tile.y;
            zone = tile.zone;
            directionAlpha = tile.AlphaArrow;
            directionBeta = tile.BetaArrow;
            instructionAlpha = tile.AlphaCommand;
            instructionBeta = tile.BetaCommand;
            hasBonder = tile.HasBonder;
        }

        /// <summary>
        /// Gets the direction and instruction on the tile for the given waldo type.
        /// </summary>
        /// <param name="type">the waldo type (alpha or beta)</param>
        /// <returns>the direction and instruction commands</returns>
        public Tuple<Direction, Instruction> GetCommands(WaldoType type)
        {
            if (type == WaldoType.Alpha)
            {
                return new Tuple<Direction, Instruction>(directionAlpha, instructionAlpha);
            }
            else
            {
                return new Tuple<Direction, Instruction>(directionBeta, instructionBeta);
            }
        }
    }

    /// <summary>
    /// Represents a tile/square of a reactor.
    /// A tile sets the direction of each Waldo that passes over it and 
    /// optionally contains a different instruction for each waldo. 
    /// 
    /// The directions and instructions a tile contain are mutable.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// The x position of the tile.
        /// </summary>
        internal readonly int x;

        /// <summary>
        /// The y position of the tile.
        /// </summary>
        internal readonly int y;

        /// <summary>
        /// The zone this tile belongs to (alpha-out, beta-in etc.)
        /// </summary>
        internal readonly Zone zone;

        /// <summary>
        /// The direction to send the alpha waldo.
        /// </summary>
        internal Direction AlphaArrow { get; set; }

        /// <summary>
        /// The direction to send the beta waldo.
        /// </summary>
        internal Direction BetaArrow { get; set; }

        /// <summary>
        /// The instruction the alpha waldo should execute when passed over.
        /// </summary>
        internal Instruction AlphaCommand { get; set; }

        /// <summary>
        /// The instruction the beta waldo should execute when passed over.
        /// </summary>
        internal Instruction BetaCommand { get; set; }

        /// <summary>
        /// True if this tile contains a bonder, false otherwise.
        /// </summary>
        internal bool HasBonder { get; set; }

        /// <summary>
        /// Gets an immutable view of this tiles fields.
        /// </summary>
        public TileAttributes Attributes
        {
            get
            {
                return new TileAttributes(this);
            }
        }

        internal Tile(int x,
                    int y,
                    Zone zone = Zone.Empty,
                    Direction directionAlpha = Direction.Continue,
                    Direction directionBeta = Direction.Continue,
                    Instruction instructionAlpha = Instruction.Empty,
                    Instruction instructionBeta = Instruction.Empty,
                    bool hasBonder = false)
        {
            this.x = x;
            this.y = y;
            this.zone = zone;
            this.AlphaArrow = directionAlpha;
            this.BetaArrow = directionBeta;
            this.AlphaCommand = instructionAlpha;
            this.BetaCommand = instructionBeta;
            this.HasBonder = hasBonder;
        }

        internal void Set(Parse.Tile tile)
        {
            if(tile.waldo == WaldoType.Alpha)
            {
                AlphaArrow = tile.direction;
                AlphaCommand = tile.instruction;
            }
            else
            {
                BetaArrow = tile.direction;
                BetaCommand = tile.instruction;
            }
        }
    }
}
