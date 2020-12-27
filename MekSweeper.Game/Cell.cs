using System.Diagnostics;

namespace MekSweeper.Game
{
    public enum CellState
    {
        Blank,
        Uncovered,
        Flagged
    }

    public abstract class Cell
    {
        public CellState State { get; set; }
    }

    public class MineCell : Cell { }

    [DebuggerDisplay("{NeighboringMineCount} {State}")]
    public class EmptyCell : Cell
    {
        public int NeighboringMineCount { get; set; }
    }
}
