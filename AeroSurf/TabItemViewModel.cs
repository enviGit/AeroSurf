using CefSharp;
using CefSharp.Wpf;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace AeroSurf
{
    public class TabItemViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ChromiumWebBrowser BrowserInstance { get; set; }

        private string _title;
        private string _url;
        private bool _isSelected;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                OnPropertyChanged(nameof(Url));
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

        public ICommand CloseCommand { get; set; }

        public ICommand GoBackCommand => new RelayCommand<object>(o => BrowserInstance?.Back(), o => BrowserInstance != null && BrowserInstance.CanGoBack);
        public ICommand GoForwardCommand => new RelayCommand<object>(o => BrowserInstance?.Forward(), o => BrowserInstance != null && BrowserInstance.CanGoForward);
        public ICommand ReloadCommand => new RelayCommand<object>(o => BrowserInstance?.Reload());

        public ICommand LoadUrlCommand => new RelayCommand<string>(url =>
        {
            if (BrowserInstance != null && !string.IsNullOrWhiteSpace(url))
            {
                string target = url.Contains(".") && !url.Contains(" ")
                    ? (url.StartsWith("http") ? url : "https://" + url)
                    : $"https://www.google.com/search?q={url}";
                BrowserInstance.Load(target);
            }
        });
        public void Dispose()
        {
            if (BrowserInstance != null)
            {
                BrowserInstance.Dispose();
                BrowserInstance = null;
            }
        }

        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}