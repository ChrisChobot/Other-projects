using System;
using System.Windows;
using System.Windows.Controls;

namespace SimpleCrawler
{
    public sealed class ErrorManager
    {
        private TextBlock _errorBox;
        private static readonly Lazy<ErrorManager> _lazyInstance = new Lazy<ErrorManager>(() => new ErrorManager());

        public static ErrorManager Instance
        {
            get
            {
                return _lazyInstance.Value;
            }
        }
        
        ErrorManager()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            _errorBox = mainWindow.ErrorBox;
        }

        public void SetErrorText(string text)
        {
            _errorBox.Text = text;
        }
    }
}
