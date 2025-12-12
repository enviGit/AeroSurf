using CefSharp;
using CefSharp.Wpf;
using System.ComponentModel;
using System.Windows.Input;

namespace AeroSurf
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _address;
        private string _title;

        public MainViewModel()
        {
            Address = "https://www.google.com";
        }

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public ICommand GoBackCommand => new RelayCommand<ChromiumWebBrowser>(b => b.Back(), b => b != null && b.CanGoBack);
        public ICommand GoForwardCommand => new RelayCommand<ChromiumWebBrowser>(b => b.Forward(), b => b != null && b.CanGoForward);
        public ICommand ReloadCommand => new RelayCommand<ChromiumWebBrowser>(b => b.Reload());

        public ICommand SearchCommand => new RelayCommand<ChromiumWebBrowser>(browser =>
        {
            if (string.IsNullOrWhiteSpace(Address)) return;

            if (Address.Contains(".") && !Address.Contains(" "))
            {
                var target = Address.StartsWith("http") ? Address : "https://" + Address;
                browser.Load(target);
            }
            else
            {
                browser.Load($"https://www.google.com/search?q={Address}");
            }
        });

        public void UpdateAddress(string newUrl)
        {
            _address = newUrl;
            OnPropertyChanged(nameof(Address));
        }

        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly System.Action<T> _execute;
        private readonly System.Predicate<T> _canExecute;
        public RelayCommand(System.Action<T> execute, System.Predicate<T> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute((T)parameter);
        public void Execute(object parameter) => _execute((T)parameter);
        public event System.EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}