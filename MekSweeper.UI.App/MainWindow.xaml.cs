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
    }
}
