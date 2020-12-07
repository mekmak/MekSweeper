using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MekSweeper.UI.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }

        private MainWindowViewModel ViewModel => (MainWindowViewModel) DataContext;
        
        private void OnRightClick(object sender, MouseButtonEventArgs e)
        {
            var button = (Button) sender;
            ViewModel.FlagCellCommand.Execute(button.DataContext);
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            ViewModel.UncoverCellCommand.Execute(button.DataContext);
        }

        private void UIElement_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState != MouseButtonState.Released)
            {
                return;
            }

            var button = (Button) sender;
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    ViewModel.UncoverCellCommand.Execute(button.DataContext);
                    break;
                case MouseButton.Right:
                    ViewModel.FlagCellCommand.Execute(button.DataContext);
                    break;
                case MouseButton.Middle:
                    ViewModel.RevealKnownCommand.Execute(button.DataContext);
                    break;
            }
        }
    }
}
