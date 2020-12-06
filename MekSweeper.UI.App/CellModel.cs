using System.Collections.Generic;

namespace MekSweeper.UI.App
{
    public enum CellFlagState
    {
        Blank,
        Uncovered,
        Flagged
    }

    public class CellModel : ObservableObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsMine { get; set; }
        public int NeighboringMineCount { get; set; }

        private CellFlagState _flagState;

        public CellFlagState FlagState
        {
            get => _flagState;
            set => SetProperty(nameof(FlagState), ref _flagState, ref value, nameof(Display));
        }

        public string Display
        {
            get
            {
                switch (FlagState)
                {
                    case CellFlagState.Blank:
                        return "";
                    case CellFlagState.Flagged:
                        return "F";
                    case CellFlagState.Uncovered when NeighboringMineCount != 0:
                        return IsMine ? "X" : NeighboringMineCount.ToString();
                    case CellFlagState.Uncovered when NeighboringMineCount == 0:
                        return IsMine ? "X" : "";
                    default:
                        return "?";
                }
            }
        }

        public IEnumerable<(string, object)> LogTags()
        {
            yield return ("x", X);
            yield return ("y", Y);
            yield return ("isMine", IsMine);
            yield return ("neighborMineCount", NeighboringMineCount);
            yield return ("flagState", _flagState);
            yield return ("display", Display);
        }

        public override string ToString()
        {
            return $"({X},{Y}) {(IsMine ? "X" : NeighboringMineCount.ToString())} {FlagState}";
        }
    }
}
