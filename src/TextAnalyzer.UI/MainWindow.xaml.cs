using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TextAnalyzer.Core.Services.WordExtractor;
using TextAnalyzer.UI.ViewModels;

namespace TextAnalyzer.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IWordExtractorService WordExtractorService { get; }
        private string _FilePath = string.Empty;
        private CancellationTokenSource _CancellationTokenSource;
        private readonly MainViewModel _MainViewModel;

        public MainWindow(IWordExtractorService wordExtractorService)
        {
            InitializeComponent();

            WordExtractorService = wordExtractorService;
            WordExtractorService.ProgressChanged += WordExtractorService_OnProgressChanged;

            _MainViewModel = new MainViewModel();
            FilePathLabel.DataContext = _MainViewModel;
            ProgressBar.DataContext = _MainViewModel;
            WordsGrid.DataContext = _MainViewModel;
        }

        private void WordExtractorService_OnProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            _MainViewModel.ProgressBarValue = e.ProgressPercentage;
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            _FilePath = openFileDialog.FileName;
            _MainViewModel.FilePathContent = _FilePath;
            StartExtractWordsProcess.IsEnabled = true;
        }

        private async void StartExtractWordsProcess_Click(object sender, RoutedEventArgs e)
        {
            UpdateButtonStatus(true);

            _MainViewModel.ExtractedWords = new Dictionary<string, int>();
            _CancellationTokenSource = new CancellationTokenSource();
            _MainViewModel.ExtractedWords = await Task.Run(() => WordExtractorService.ExtractCountWords(_FilePath, _CancellationTokenSource.Token));

            UpdateButtonStatus(false);
        }

        private void UpdateButtonStatus(bool started)
        {
            StartExtractWordsProcess.IsEnabled = !started;
            CancelExtractWordsProcess.IsEnabled = started;
        }

        private void CancelExtractWordsProcess_Click(object sender, RoutedEventArgs e)
        {
            _CancellationTokenSource?.Cancel();
        }
    }
}
