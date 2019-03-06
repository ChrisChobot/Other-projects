using System.Collections.Generic;
using System.Windows;

namespace SimpleCrawler
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AnalyseButtonClick(object sender, RoutedEventArgs e)
        {
            ErrorManager.Instance.SetErrorText(string.Empty);
            KeywordsGrid.Items.Clear();

            List<KeywordGridRow> gridRows = Website.AnalyseUrl(SiteName.Text);

            if (gridRows != null)
            {
                for (int i = 0; i < gridRows.Count; i++)
                {
                    KeywordsGrid.Items.Add(gridRows[i]);
                }
            }                       
        }
    }
}
