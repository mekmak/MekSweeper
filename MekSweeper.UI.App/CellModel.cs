using System;
using System.Collections.Generic;
using MekSweeper.Game;

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
        public int ColumnNumber { get; set; }
        public int RowNumber { get; set; }
        public bool IsMine { get; set; }
        public int NeighboringMineCount { get; set; }

        private CellFlagState _flagState;
        public CellFlagState FlagState
        {
            get => _flagState;
            set => SetProperty(nameof(FlagState), ref _flagState, ref value, nameof(InProgressDisplay), nameof(EndGameDisplay));
        }

        public string InProgressDisplay
        {
            get
            {
                switch (FlagState)
                {
                    case CellFlagState.Blank:
                        return "";
                    case CellFlagState.Flagged:
                        return "M";
                    case CellFlagState.Uncovered when NeighboringMineCount != 0:
                        return NeighboringMineCount.ToString();
                    case CellFlagState.Uncovered when NeighboringMineCount == 0:
                        return "";
                    default:
                        return "?";
                }
            }
        }

        public string EndGameDisplay
        {
            get
            {
                switch (FlagState)
                {
                    case CellFlagState.Flagged when IsMine:
                        return "M";
                    case CellFlagState.Flagged when !IsMine:
                        return $"{NeighboringMineCount}!";
                    case CellFlagState.Uncovered when IsMine:
                        return "X!";
                    case CellFlagState.Blank when !IsMine:
                    case CellFlagState.Uncovered when !IsMine:
                        return NeighboringMineCount == 0 ? "" : NeighboringMineCount.ToString();
                    case CellFlagState.Blank when IsMine:
                        return "X";
                    default:
                        return "?";
                }
            }
        }

        public static CellModel FromCell(int col, int row, Cell cell)
        {
            switch (cell)
            {
                case MineCell:
                {
                    return new CellModel
                    {
                        FlagState = TranslateState(cell.State),
                        IsMine = true,
                        NeighboringMineCount = 0,
                        ColumnNumber = col,
                        RowNumber = row
                    };
                }

                case EmptyCell empty:
                {
                    return new CellModel
                    {
                        FlagState = TranslateState(cell.State),
                        IsMine = false,
                        NeighboringMineCount = empty.NeighboringMineCount,
                        ColumnNumber = col,
                        RowNumber = row
                    };
                }
            }

            throw new InvalidOperationException($"Unrecognized cell type '{cell.GetType().Name}'");
        }

        private static CellFlagState TranslateState(CellState cellState)
        {
            switch (cellState)
            {
                case CellState.Uncovered:
                    return CellFlagState.Uncovered;
                case CellState.Blank:
                    return CellFlagState.Blank;
                case CellState.Flagged:
                    return CellFlagState.Flagged;
                default:
                    throw new InvalidOperationException($"Unrecognized cell state '{cellState}'");
            }
        }

        public Cell ToCell()
        {
            if (IsMine)
            {
                return new MineCell
                {
                    State = TranslateState(FlagState)
                };
            }

            return new EmptyCell
            {
                State = TranslateState(FlagState),
                NeighboringMineCount = NeighboringMineCount
            };
        }

        private CellState TranslateState(CellFlagState cellState)
        {
            switch (cellState)
            {
                case CellFlagState.Uncovered:
                    return CellState.Uncovered;
                case CellFlagState.Blank:
                    return CellState.Blank;
                case CellFlagState.Flagged:
                    return CellState.Flagged;
                default:
                    throw new InvalidOperationException($"Unrecognized cell flag state '{cellState}'");
            }
        }

        public IEnumerable<(string, object)> LogTags()
        {
            yield return ("colNum", ColumnNumber);
            yield return ("rowNum", RowNumber);
            yield return ("isMine", IsMine);
            yield return ("neighborMineCount", NeighboringMineCount);
            yield return ("flagState", _flagState);
            yield return ("display", InProgressDisplay);
        }

        public override string ToString()
        {
            return $"({ColumnNumber},{RowNumber}) {(IsMine ? "X" : NeighboringMineCount.ToString())} {FlagState}";
        }


    }
}
