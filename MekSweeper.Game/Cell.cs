namespace MekSweeper.Game
{
    public abstract class Cell { }

    public class MineCell : Cell { }

    public class EmptyCell : Cell
    {
        public int NeighboringMineCount { get; set; }
    }
}
