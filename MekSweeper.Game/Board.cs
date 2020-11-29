using System;
using MekSweeper.Logging.Interfaces;

namespace MekSweeper.Game
{
    public class Board
    {
        private readonly IMekLogger _logger;
        private readonly Cell[,] _cells;

        public Board(IMekLogger logger, Cell[,] cells)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cells = cells ?? throw new ArgumentNullException(nameof(cells));
        }

        public int ColumnCount => _cells.GetLength(0);
        public int RowCount => _cells.GetLength(1);

        public Cell GetCell(string traceId, int col, int row)
        {
            if (col < 0 || col >= ColumnCount)
            {
                _logger.Warn(traceId, "GetCell.InvalidColumn", ("row", row), ("col", col));
                throw new ArgumentOutOfRangeException(nameof(col));
            }

            if (row < 0 || row >= RowCount)
            {
                _logger.Warn(traceId, "GetCell.InvalidRow", ("row", row), ("col", col));
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            return _cells[col, row];
        }
    }
}
