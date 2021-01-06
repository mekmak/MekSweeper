using System;
using System.Windows;

namespace MekSweeper.UI.App
{
    public class DialogController
    {
        public void AskYesNo(string caption, string question, Action onYes, Action onNo)
        {
            onYes ??= () => { };
            onNo ??= () => { };

            MessageBoxResult result = MessageBox.Show(question, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                case MessageBoxResult.OK:
                {
                    onYes();
                    break;
                }

                default:
                {
                    onNo();
                    break;
                }
            }
        }
    }
}
