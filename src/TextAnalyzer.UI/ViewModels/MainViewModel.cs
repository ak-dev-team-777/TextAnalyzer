using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TextAnalyzer.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _FilePathContent = string.Empty;
        public string FilePathContent
        {
            get
            {
                if (string.IsNullOrEmpty(_FilePathContent))
                {
                    return "File not loaded yet, please load file";
                }
                return $"Loaded file path: {_FilePathContent}";
            }
            set
            {
                if (_FilePathContent == value)
                    return;

                _FilePathContent = value;
                OnPropertyChanged();
            }
        }

        private int _ProgressBarValue;
        public int ProgressBarValue
        {
            get => _ProgressBarValue;
            set
            {
                if (_ProgressBarValue == value)
                    return;

                _ProgressBarValue = value;
                OnPropertyChanged();
            }
        }

        private IDictionary<string, int> _ExtractedWords = new Dictionary<string, int>();
        public IDictionary<string, int> ExtractedWords
        {
            get => _ExtractedWords;
            set
            {
                _ExtractedWords = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
