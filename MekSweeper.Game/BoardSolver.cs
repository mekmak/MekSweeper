using System;
using System.Collections.Generic;
using System.Linq;
using MekSweeper.Extensions;
using MekSweeper.Logging.Interfaces;

namespace MekSweeper.Game
{
    public class BoardSolver
    {
        private readonly IMekLogger _logger;

        public BoardSolver(IMekLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Returns whether or not the player can make a move that guarantees uncovering a cell that is not mined
        /// </summary>
        public bool HasMove(string traceId, Board board)
        {
            if (board?.Cells == null)
            {
                throw new InvalidOperationException("Boards cells cannot be null");
            }

            for (int col = 0; col < board.Cells.GetLength(0); col++)
            {
                for (int row = 0; row < board.Cells.GetLength(1); row++)
                {
                    if (HasFlagCountMove(col, row, board))
                    {
                        _logger.Info(traceId, "HasMove.FlagCount.Found", ("col", col), ("row", row));
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns whether or not the player can make a move based off of simple neighboring mine count and number of flagged cells
        /// </summary>
        private bool HasFlagCountMove(int col, int row, Board board)
        {
            Cell cell = board.Cells[col, row];
            if (!(cell is EmptyCell emptyCell))
            {
                return false;
            }

            if (cell.State != CellState.Uncovered || emptyCell.NeighboringMineCount == 0)
            {
                return false;
            }

            List<Cell> neighbors = board.Cells.GetNeighbors(col, row).ToList();
            List<Cell> blankNeighbors = neighbors.Where(n => n.State == CellState.Blank).ToList();

            if (!blankNeighbors.Any())
            {
                // Everything around this cell is uncovered or flagged -- no moves here
                return false;
            }

            List<Cell> flaggedNeighbors = neighbors.Where(n => n.State == CellState.Flagged).ToList();
            int missingMines = emptyCell.NeighboringMineCount - flaggedNeighbors.Count;

            if (missingMines == 0)
            {
                // We've found all the mines
                return true;
            }

            if (missingMines == blankNeighbors.Count)
            {
                // The only blank cells have to be mines
                return true;
            }

            // No move based on immediate flag count
            return false;
        }
    }
}