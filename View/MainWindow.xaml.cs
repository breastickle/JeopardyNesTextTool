using System.Windows;
using System.Windows.Controls;
using JeopardyNesTextTool.ViewModel;

namespace JeopardyNesTextTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new ApplicationViewModel();
            DataContext = viewModel;
        }

        private void OnTextBoxChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is ApplicationViewModel viewModel)
            {
                viewModel.NotifyTextChanged();
            }
        }
    }
}
