using CefSharp.DevTools.CSS;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace AeroSurf
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TabItemViewModel> Tabs { get; set; }

        private TabItemViewModel _selectedTab;
        private TabItemViewModel _mediaTab;
        private string _address;
        private string _title;

        public MainViewModel()
        {
            Tabs = new ObservableCollection<TabItemViewModel>();
            AddNewTab("https://www.google.com", "New Tab");
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
                        _address = _selectedTab.Url;
                        _title = "AeroSurf - " + _selectedTab.Title;
                        OnPropertyChanged(nameof(Address));
                        OnPropertyChanged(nameof(Title));
                        CheckIfMediaTab(_selectedTab);
                    }
                    OnPropertyChanged(nameof(SelectedTab));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public TabItemViewModel MediaTab
        {
            get => _mediaTab;
            set
            {
                _mediaTab = value;
                OnPropertyChanged(nameof(MediaTab));
                OnPropertyChanged(nameof(HasMedia));
            }
        }

        public bool HasMedia => MediaTab != null;

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));

                if (SelectedTab != null)
                {
                    CheckIfMediaTab(SelectedTab);
                }
            }
        }


        public ICommand AddTabCommand => new RelayCommand<object>(o => AddNewTab("https://www.google.com", "New Tab"));

        public ICommand RemoveTabCommand => new RelayCommand<TabItemViewModel>(tab =>
        {
            if (Tabs.Contains(tab))
            {
                if (tab == MediaTab) MediaTab = null;

                tab.Dispose();
                Tabs.Remove(tab);
                if (Tabs.Count == 0) AddNewTab("https://www.google.com", "New Tab");
                if (SelectedTab == null || !Tabs.Contains(SelectedTab)) SelectedTab = Tabs.LastOrDefault();
            }
        });

        public ICommand SearchCommand => new RelayCommand<object>(o =>
        {
            SelectedTab?.LoadUrlCommand.Execute(Address);
        });

        public ICommand GoBackCommand => new RelayCommand<object>(o => SelectedTab?.GoBackCommand.Execute(null), o => SelectedTab != null && SelectedTab.GoBackCommand.CanExecute(null));
        public ICommand GoForwardCommand => new RelayCommand<object>(o => SelectedTab?.GoForwardCommand.Execute(null), o => SelectedTab != null && SelectedTab.GoForwardCommand.CanExecute(null));
        public ICommand ReloadCommand => new RelayCommand<object>(o => SelectedTab?.ReloadCommand.Execute(null));
        public ICommand GlobalPlayPauseCommand => new RelayCommand<object>(o => MediaTab?.MediaPlayPauseCommand.Execute(null));
        public ICommand GlobalNextCommand => new RelayCommand<object>(o => MediaTab?.MediaNextCommand.Execute(null));
        public ICommand GlobalPrevCommand => new RelayCommand<object>(o => MediaTab?.MediaPrevCommand.Execute(null));

        public void AddNewTab(string url, string title)
        {
            var newTab = new TabItemViewModel { Url = url, Title = title };
            newTab.CloseCommand = RemoveTabCommand;
            Tabs.Add(newTab);
            SelectedTab = newTab;
            CheckIfMediaTab(newTab);
        }

        public void UpdateInfoFromBrowser(string url, string title)
        {
            if (SelectedTab != null)
            {
                if (!string.IsNullOrEmpty(url) && SelectedTab.Url != url)
                {
                    SelectedTab.Url = url;
                    if (SelectedTab.IsSelected)
                    {
                        _address = url;
                        OnPropertyChanged(nameof(Address));
                    }
                }

                if (!string.IsNullOrEmpty(title))
                {
                    SelectedTab.Title = title;
                    if (SelectedTab.IsSelected)
                    {
                        Title = "AeroSurf - " + title;
                    }
                }

                CheckIfMediaTab(SelectedTab);
            }
        }

        private void CheckIfMediaTab(TabItemViewModel tab)
        {
            if (tab == null) return;

            string t = (tab.Title ?? "").ToLower();
            string u = (tab.Url ?? "").ToLower();

            if (t.Contains("youtube") || u.Contains("youtube") ||
                t.Contains("spotify") || u.Contains("spotify") ||
                t.Contains("netflix") || u.Contains("netflix") ||
                t.Contains("twitch") || u.Contains("twitch") ||
                t.Contains("soundcloud"))
            {
                if (MediaTab != tab)
                {
                    MediaTab = tab;
                }
            }
        }

        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
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