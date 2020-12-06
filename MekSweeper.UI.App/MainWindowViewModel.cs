using System;
using System.Collections.Generic;
using System.Linq;
using MekSweeper.Extensions;
using MekSweeper.Game;
using MekSweeper.Logging;
using MekSweeper.Logging.Interfaces;

namespace MekSweeper.UI.App
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly string _traceId;
        private readonly BoardBuilder _boardBuilder;
        private readonly IMekLogger _logger;

        public MainWindowViewModel()
        {
            _logger = new MekLogger("MekSweeper");
            _traceId = TraceId.New();

            _logger.Info(_traceId, "AppLaunched");

            _boardBuilder = new BoardBuilder(_logger.SubLogger("BoardBuilder"));
            _cells = new List<List<CellModel>>();

            NewGameCommand = new Command(NewGame);
            UncoverCellCommand = new Command(UncoverCell);
            FlagCellCommand = new Command(FlagCell);

            _gameState = GameState.NotStarted;

            NewGame();
        }

        #region Commands

        private Command _newGameCommand;
        public Command NewGameCommand
        {
            get => _newGameCommand;
            set => SetProperty(nameof(NewGameCommand), ref _newGameCommand, ref value);
        }

        private Command _uncoverCellCommand;
        public Command UncoverCellCommand
        {
            get => _uncoverCellCommand;
            set => SetProperty(nameof(UncoverCellCommand), ref _uncoverCellCommand, ref value);
        }

        private Command _flagCellCommand;
        public Command FlagCellCommand
        {
            get => _flagCellCommand;
            set => SetProperty(nameof(FlagCellCommand), ref _flagCellCommand, ref value);
        }

        #endregion

        #region Properties

        private GameState _gameState;

        private string _messageContent;
        public string MessageContent
        {
            get => _messageContent;
            set => SetProperty(nameof(MessageContent), ref _messageContent, ref value);
        }

        private List<List<CellModel>> _cells;
        public List<List<CellModel>> Cells
        {
            get => _cells;
            set => SetProperty(nameof(Cells), ref _cells, ref value);
        }

        #endregion

        private void NewGame()
        {
            _logger.Info(_traceId, "NewGame");
            var options = new BoardOptions
            {
                RowCount = 10,
                ColumnCount = 10,
                MineCount = 10
            };

            Board newBoard = _boardBuilder.Build(_traceId, options);

            var cells = new List<List<CellModel>>(newBoard.ColumnCount);
            for (int x = 0; x < newBoard.ColumnCount; x++)
            {
                cells.Add(new List<CellModel>(newBoard.RowCount));
                for (int y = 0; y < newBoard.RowCount; y++)
                {
                    Cell cell = newBoard.GetCell(_traceId, x, y);
                    cells[x].Add(FromCell(x, y, cell));
                }
            }

            Cells = cells;

            _gameState = GameState.InProgress;
            UpdateGameState();
        }

        private CellModel FromCell(int x, int y, Cell cell)
        {
            switch (cell)
            {
                case MineCell:
                {
                    return new CellModel
                    {
                        FlagState = CellFlagState.Blank,
                        IsMine = true,
                        NeighboringMineCount = 0,
                        X = x,
                        Y = y
                    };
                }

                case EmptyCell empty:
                {
                    return new CellModel
                    {
                        FlagState = CellFlagState.Blank,
                        IsMine = false,
                        NeighboringMineCount = empty.NeighboringMineCount,
                        X = x,
                        Y = y
                    };
                }
            }

            _logger.Warn(_traceId, "FromCell.UnrecognizedCellType", ("cellType", cell.GetType().Name));
            throw new InvalidOperationException($"Unrecognized cell type '{cell.GetType().Name}'");
        }

        private void FlagCell(object cellObj)
        {
            if (_gameState != GameState.InProgress)
            {
                return;
            }

            var cell = cellObj as CellModel;
            if (cell == null)
            {
                _logger.Warn(_traceId, "FlagCell.CannotCast", ("objectType", cellObj?.GetType().Name));
                return;
            }

            _logger.Info(_traceId, "FlagCell", cell.LogTags().ToArray());

            if (cell.FlagState == CellFlagState.Uncovered)
            {
                return;
            }

            switch (cell.FlagState)
            {
                case CellFlagState.Blank:
                    cell.FlagState = CellFlagState.Flagged;
                    break;
                case CellFlagState.Flagged:
                    cell.FlagState = CellFlagState.Blank;
                    break;
                default:
                    _logger.Warn(_traceId, "FlagCell.UnknownFlagState", ("flagState", cell.FlagState));
                    break;
            }

            UpdateGameState();
        }

        private void UncoverCell(object cellObj)
        {
            if (_gameState != GameState.InProgress)
            {
                return;
            }

            var cell = cellObj as CellModel;
            if (cell == null)
            {
                _logger.Warn(_traceId, "UncoverCell.CannotCast", ("objectType", cellObj?.GetType().Name));
                return;
            }

            _logger.Info(_traceId, "UncoverCell", cell.LogTags().ToArray());

            if (cell.FlagState != CellFlagState.Blank)
            {
                return;
            }

            if (cell.IsMine)
            {
                EndGame(GameState.Lost);
                return;
            }

            cell.FlagState = CellFlagState.Uncovered;
            if (cell.NeighboringMineCount == 0)
            {
                ChainUncoverZeroCells(cell);
            }

            UpdateGameState();
        }

        private void ChainUncoverZeroCells(CellModel cell)
        {
            var cellsToUncover = new Queue<CellModel>();
            cellsToUncover.Enqueue(cell);

            while (cellsToUncover.Count > 0)
            {
                CellModel currentCell = cellsToUncover.Dequeue();
                List<CellModel> neighbors = _cells.GetNeighbors(currentCell.X, currentCell.Y).ToList();
                foreach (var neighbor in neighbors)
                {
                    if (neighbor.FlagState == CellFlagState.Uncovered)
                    {
                        continue;
                    }

                    neighbor.FlagState = CellFlagState.Uncovered;
                    if (!neighbor.IsMine && neighbor.NeighboringMineCount == 0)
                    {
                        cellsToUncover.Enqueue(neighbor);
                    }
                }
            }
        }

        private void UpdateGameState()
        {
            int totalCellCount = 0;
            int totalMineCount = 0;
            int totalUncoveredCount = 0;
            int totalFlaggedCount = 0;

            foreach (CellModel cell in _cells.SelectMany(c => c))
            {
                totalCellCount++;

                if (cell.IsMine)
                {
                    totalMineCount++;
                }

                if (cell.FlagState == CellFlagState.Uncovered)
                {
                    totalUncoveredCount++;
                }

                if (cell.FlagState == CellFlagState.Flagged)
                {
                    totalFlaggedCount++;
                }
            }

            bool allMinesFlagged = totalFlaggedCount == totalMineCount;
            bool allCellsUncovered = totalFlaggedCount + totalUncoveredCount == totalCellCount;
            if (allMinesFlagged && allCellsUncovered)
            {
                EndGame(GameState.Won);
                return;
            }

            MessageContent = $"Mines left: {totalMineCount - totalFlaggedCount}";
        }

        private void EndGame(GameState endState)
        {
            _logger.Info(_traceId, "EndGame", ("endState", endState));
            foreach (CellModel cell in _cells.SelectMany(c => c))
            {
                cell.FlagState = CellFlagState.Uncovered;
            }

            OnPropertyChanged(nameof(Cells));

            switch (endState)
            {
                case GameState.Lost:
                    MessageContent = "You lose!";
                    break;
                case GameState.Won:
                    MessageContent = "You win!";
                    break;
                default:
                    MessageContent = "Gave over! (Unknown state)";
                    _logger.Warn(_traceId, "EndGame.UnknownGameState", ("endState", endState));
                    break;
            }
        }

        private enum GameState
        {
            NotStarted,
            InProgress,
            Lost,
            Won
        }
    }
}
