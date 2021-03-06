﻿using System;
using System.Linq;
using MekSweeper.Extensions;
using MekSweeper.Logging.Interfaces;

namespace MekSweeper.Game
{
    public class BoardBuilder
    {
        private readonly IMekLogger _logger;
        public BoardBuilder(IMekLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Board Build(string traceId, BoardOptions options)
        {
            if (options == null)
            {
                _logger.Warn(traceId, "Build.InvalidOptions.NullOptions");
                throw new ArgumentNullException(nameof(options));
            }

            if (options.RowCount <= 0)
            {
                _logger.Warn(traceId, "Build.InvalidOptions.RowCount", ("rowCount", options.RowCount));
                throw new ArgumentException("Row count must be > 0");
            }

            if (options.ColumnCount <= 0)
            {
                _logger.Warn(traceId, "Build.InvalidOptions.ColumnCount", ("columnCount", options.ColumnCount));
                throw new ArgumentException("Column count must be > 0");
            }

            if (options.MineCount <= 0)
            {
                _logger.Warn(traceId, "Build.InvalidOptions.MineCount", ("mineCount", options.MineCount));
                throw new ArgumentException("Mine count must be > 0");
            }

            if (options.MineCount > options.RowCount * options.ColumnCount)
            {
                _logger.Warn(traceId, "Build.InvalidOptions.TooManyMines", ("mineCount", options.MineCount), ("rowCount", options.RowCount), ("colCount", options.ColumnCount));
                throw new ArgumentException("Cannot have more mines than cells");
            }

            _logger.Info(traceId, "Build", ("rowCount", options.RowCount), ("columnCount", options.ColumnCount), ("mineCount", options.MineCount));

            var cells = new Cell[options.ColumnCount, options.RowCount];
            var mines = SetMines(options);

            for (int x = 0; x < options.ColumnCount; x++)
            {
                for (int y = 0; y < options.RowCount; y++)
                {
                    if (mines[x, y])
                    {
                        cells[x, y] = new MineCell
                        {
                            State = CellState.Blank
                        };
                    }
                    else
                    {
                        int neighborMineCount = GetNeighboringMineCount(x, y, mines);
                        cells[x, y] = new EmptyCell
                        {
                            State = CellState.Blank,
                            NeighboringMineCount = neighborMineCount
                        };
                    }
                }
            }

            return new Board
            {
                Cells = cells,
                Options = options
            };
        }

        private int GetNeighboringMineCount(int col, int row, bool[,] mines)
        {
            return mines.GetNeighbors(col, row).Count(m => m);
        }

        private bool[,] SetMines(BoardOptions options)
        {
            var mines = new bool[options.ColumnCount, options.RowCount];

            int minesPlaced = 0;
            while (minesPlaced < options.MineCount)
            {
                (int col, int row) = GetRandomCell(options);
                if (mines[col, row])
                {
                    continue;
                }

                mines[col, row] = true;
                minesPlaced++;
            }

            return mines;
        }

        private static readonly Random Random = new Random();
        private (int, int) GetRandomCell(BoardOptions options)
        {
            return (Random.Next(0, options.ColumnCount), Random.Next(0, options.RowCount));
        }
    }
}
