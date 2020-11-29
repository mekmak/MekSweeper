namespace MekSweeper.Game
{
    public abstract class Cell
    {
        public abstract bool IsMine { get; }
    }

    public class MineCell : Cell
    {
        public override bool IsMine => false;
    }

    public class EmptyCell : Cell
    {
        public override bool IsMine => false;

        public int NeighboringMineCount { get; init; }
    }
}
