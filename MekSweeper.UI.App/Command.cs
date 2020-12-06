using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MekSweeper.UI.App
{
    public class Command : ICommand
    {
        private readonly Action<object> _execute;
        
        public Command(Action execute) : this(_ => execute()) { }
        public Command(Action<object> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        
        public void Execute(object parameter) { _execute(parameter); }
    }
}
