using CefSharp;
using CefSharp.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Windows.Input;

namespace AeroSurf
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TabItemViewModel> Tabs { get; set; }

        private TabItemViewModel _selectedTab;
        private string _address;
        private string _title;

        public MainViewModel()
        {
            Tabs = new ObservableCollection<TabItemViewModel>();

            AddNewTab("https://www.google.com", "Google");
        }

        public TabItemViewModel SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (_selectedTab != value)
                {
                    if (_selectedTab != null) _selectedTab.IsSelected = false;
                    _selectedTab = value;
                    if (_selectedTab != null)
                    {
                        _selectedTab.IsSelected = true;
                        Address = _selectedTab.Url;
                    }
                    OnPropertyChanged(nameof(SelectedTab));
                }
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
                if (SelectedTab != null) SelectedTab.Url = value;
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
                if (SelectedTab != null) SelectedTab.Url = value;
            }
        }

        public ICommand AddTabCommand => new RelayCommand<object>(o => AddNewTab("https://www.google.com", "Nowa Karta"));

        public ICommand RemoveTabCommand => new RelayCommand<TabItemViewModel>(tab =>
        {
            if (Tabs.Contains(tab))
            {
                Tabs.Remove(tab);
                if (Tabs.Count > 0 && SelectedTab == null)
                    SelectedTab = Tabs.Last();
                else if (Tabs.Count == 0)
                    AddNewTab("https://www.google.com", "Nowa Karta");
            }
        });
        public ICommand GoBackCommand => new RelayCommand<ChromiumWebBrowser>(b => b.Back(), b => b != null && b.CanGoBack);
        public ICommand GoForwardCommand => new RelayCommand<ChromiumWebBrowser>(b => b.Forward(), b => b != null && b.CanGoForward);
        public ICommand ReloadCommand => new RelayCommand<ChromiumWebBrowser>(b => b.Reload());

        public ICommand SearchCommand => new RelayCommand<ChromiumWebBrowser>(browser =>
        {
            if (string.IsNullOrWhiteSpace(Address)) return;
            string target = Address.Contains(".") ? (Address.StartsWith("http") ? Address : "https://" + Address) : $"https://www.google.com/search?q={Address}";
            browser.Load(target);
        });

        private void AddNewTab(string url, string title)
        {
            var newTab = new TabItemViewModel { Url = url, Title = title };
            newTab.CloseCommand = RemoveTabCommand;
            Tabs.Add(newTab);
            SelectedTab = newTab;
        }

        public void UpdateAddress(string url)
        {
            _address = url;
            if (SelectedTab != null) SelectedTab.Url = url;
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