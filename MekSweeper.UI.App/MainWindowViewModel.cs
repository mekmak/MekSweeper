﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MekSweeper.Extensions;
using MekSweeper.Game;
using MekSweeper.Logging;
using MekSweeper.Logging.Interfaces;

namespace MekSweeper.UI.App
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly string _traceId;
        private readonly DialogController _dialogController;
        private readonly BoardBuilder _boardBuilder;
        private readonly BoardSolver _boardSolver;
        private readonly IMekLogger _logger;

        public MainWindowViewModel()
        {
            _logger = new MekLogger("MekSweeper");
            _traceId = TraceId.New();

            try
            {
                var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "";
                Title = $"MekSweeper {version}";
            }
            catch
            {
                Title = "MekSweeper";
            }


            _logger.Info(_traceId, "AppLaunched");

            _boardBuilder = new BoardBuilder(_logger.SubLogger("BoardBuilder"));
            _boardSolver = new BoardSolver(_logger.SubLogger("BoardSolver"));
            _dialogController = new DialogController();

            _cells = new List<List<CellModel>>();

            NewGameCommand = new Command(OnNewGame);
            UncoverCellCommand = new Command(UncoverCell);
            FlagCellCommand = new Command(FlagCell);
            RevealKnownCommand = new Command(RevealKnown);

            DifficultyTiers = new List<DifficultyTier>
            {
                DifficultyTier.Easy,
                DifficultyTier.Medium,
                DifficultyTier.Hard,
                DifficultyTier.Custom
            };

            SelectedDifficulty = DifficultyTier.Hard;

            State = GameState.NotStarted;
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

        private Command _revealKnownCommand;

        public Command RevealKnownCommand
        {
            get => _revealKnownCommand;
            set => SetProperty(nameof(RevealKnownCommand), ref _revealKnownCommand, ref value);
        }

        #endregion

        #region Properties

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(nameof(Title), ref _title, ref value);
        }

        private GameState _gameState;
        public GameState State
        {
            get => _gameState;
            set => SetProperty(nameof(State), ref _gameState, ref value);
        }

        private DifficultyTier _selectedDifficulty;
        public DifficultyTier SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                SetProperty(nameof(SelectedDifficulty), ref _selectedDifficulty, ref value);
                OnDifficultyTierChanged();
            }
        }

        public List<DifficultyTier> DifficultyTiers { get; set; }

        private int _numberOfRows;
        public int NumberOfRows
        {
            get => _numberOfRows;
            set
            {
                SetProperty(nameof(NumberOfRows), ref _numberOfRows, ref value);
                OnNumberOfRowsChanged();
            }
        }

        private int _numberOfColumns;
        public int NumberOfColumns
        {
            get => _numberOfColumns;
            set
            {
                SetProperty(nameof(NumberOfColumns), ref _numberOfColumns, ref value);
                OnNumberOfColumnsChanged();
            }
        }

        private int _numberOfMines;
        public int NumberOfMines
        {
            get => _numberOfMines;
            set
            {
                SetProperty(nameof(NumberOfMines), ref _numberOfMines, ref value);
                OnNumberOfMinesChanged();
            }
        }

        private string _messageContent;
        public string MessageContent
        {
            get => _messageContent;
            set => SetProperty(nameof(MessageContent), ref _messageContent, ref value);
        }

        private BoardOptions _currentBoardOptions;
        private List<List<CellModel>> _cells;
        public List<List<CellModel>> Cells
        {
            get => _cells;
            set => SetProperty(nameof(Cells), ref _cells, ref value);
        }

        private bool _hasMove;
        public bool HasMove
        {
            get => _hasMove;
            set => SetProperty(nameof(HasMove), ref _hasMove, ref value);
        }

        #endregion

        private void OnDifficultyTierChanged()
        {
            switch (_selectedDifficulty)
            {
                case DifficultyTier.Easy:
                    _numberOfMines = 10;
                    _numberOfColumns = 10;
                    _numberOfRows = 10;
                    break;
                case DifficultyTier.Medium:
                    _numberOfMines = 40;
                    _numberOfColumns = 16;
                    _numberOfRows = 16;
                    break;
                case DifficultyTier.Hard:
                    _numberOfMines = 99;
                    _numberOfColumns = 30;
                    _numberOfRows = 16;
                    break;
                case DifficultyTier.Custom:
                    break;
                default:
                    _logger.Warn(_traceId, "OnDifficultyTierChanged.UnrecognizedDifficulty", ("tier", _selectedDifficulty));
                    break;
            }

            OnPropertyChanged(nameof(NumberOfMines));
            OnPropertyChanged(nameof(NumberOfColumns));
            OnPropertyChanged(nameof(NumberOfRows));
        }

        private void OnNumberOfMinesChanged()
        {
            SelectedDifficulty = DifficultyTier.Custom;
        }

        private void OnNumberOfColumnsChanged()
        {
            SelectedDifficulty = DifficultyTier.Custom;
        }

        private void OnNumberOfRowsChanged()
        {
            SelectedDifficulty = DifficultyTier.Custom;
        }

        private void OnNewGame()
        {
            _dialogController.AskYesNo("New game", "Are you sure you want to start a new game?", NewGame, () => { });
        }

        private void NewGame()
        {
            _logger.Info(_traceId, "NewGame");

            var options = new BoardOptions
            {
                // This is not a mistake
                ColumnCount = NumberOfRows,
                RowCount = NumberOfColumns,
                MineCount = NumberOfMines
            };

            List<List<CellModel>> cells;
            try
            {
                Board newBoard = _boardBuilder.Build(_traceId, options);

                cells = new List<List<CellModel>>(options.ColumnCount);
                for (int col = 0; col < options.ColumnCount; col++)
                {
                    cells.Add(new List<CellModel>(options.RowCount));
                    for (int row = 0; row < options.RowCount; row++)
                    {
                        Cell cell = newBoard.Cells[col, row];
                        cells[col].Add(CellModel.FromCell(col, row, cell));
                    }
                }
            }
            catch (Exception ex)
            {
                State = GameState.NotStarted;
                MessageContent = $"Game setup issue: {ex.ShortSummary()}";
                return;
            }

            Cells = cells;
            _currentBoardOptions = options;

            State = GameState.InProgress;
            UpdateGameState();
        }

        private void RevealKnown(object cellObj)
        {
            if (State != GameState.InProgress)
            {
                return;
            }

            var cell = cellObj as CellModel;
            if (cell == null)
            {
                _logger.Warn(_traceId, "RevealKnown.CannotCast", ("objectType", cellObj?.GetType().Name));
                return;
            }

            if (cell.FlagState != CellFlagState.Uncovered)
            {
                return;
            }

            List<CellModel> neighbors = _cells.GetNeighbors(cell.ColumnNumber, cell.RowNumber).ToList();
            List<CellModel> flagged = neighbors.Where(n => n.FlagState == CellFlagState.Flagged).ToList();
            List<CellModel> unrevealed = neighbors.Where(n => n.FlagState == CellFlagState.Blank).ToList();

            if (flagged.Count != cell.NeighboringMineCount)
            {
                return;
            }

            foreach (CellModel unrevealedCell in unrevealed)
            {
                if (UncoverCell(unrevealedCell))
                {
                    return;
                }
            }
        }

        private void FlagCell(object cellObj)
        {
            if (State != GameState.InProgress)
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
            if (State != GameState.InProgress)
            {
                return;
            }

            var cell = cellObj as CellModel;
            if (cell == null)
            {
                _logger.Warn(_traceId, "UncoverCell.CannotCast", ("objectType", cellObj?.GetType().Name));
                return;
            }

            UncoverCell(cell);
        }

        /// <summary>
        /// Returns true if the uncovering this cell ends the game, false otherwise
        /// </summary>
        private bool UncoverCell(CellModel cell)
        {
            _logger.Info(_traceId, "UncoverCell", cell.LogTags().ToArray());

            if (cell.FlagState != CellFlagState.Blank)
            {
                return false;
            }

            cell.FlagState = CellFlagState.Uncovered;

            if (cell.IsMine)
            {
                EndGame(GameState.Lost);
                return true;
            }

            if (cell.NeighboringMineCount == 0)
            {
                ChainUncoverZeroCells(cell);
            }

            return UpdateGameState();
        }

        private void ChainUncoverZeroCells(CellModel cell)
        {
            var cellsToUncover = new Queue<CellModel>();
            cellsToUncover.Enqueue(cell);

            while (cellsToUncover.Count > 0)
            {
                CellModel currentCell = cellsToUncover.Dequeue();
                List<CellModel> neighbors = _cells.GetNeighbors(currentCell.ColumnNumber, currentCell.RowNumber).ToList();
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

        private bool UpdateGameState()
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
                return true;
            }

            HasMove = _boardSolver.HasMove(_traceId, GetBoardCopy());
            MessageContent = $"Mines left: {totalMineCount - totalFlaggedCount}";
            return false;
        }

        private Board GetBoardCopy()
        {
            var cells = new Cell[_currentBoardOptions.ColumnCount, _currentBoardOptions.RowCount];
            for (int col = 0; col < Cells.Count; col++)
            {
                var rows = Cells[col];
                for (int row = 0; row < rows.Count; row++)
                {
                    CellModel cellModel = rows[row];
                    cells[col, row] = cellModel.ToCell();
                }
            }

            return new Board
            {
                Options = _currentBoardOptions,
                Cells = cells
            };
        }
        
        private void EndGame(GameState endState)
        {
            _logger.Info(_traceId, "EndGame", ("endState", endState));

            State = endState;
            HasMove = false;

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

        public enum GameState
        {
            NotStarted,
            InProgress,
            Lost,
            Won
        }

        public enum DifficultyTier
        {
            Easy,
            Medium,
            Hard,
            Custom
        }
    }
}
