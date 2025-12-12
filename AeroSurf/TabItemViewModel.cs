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

        public ICommand MediaPlayPauseCommand => new RelayCommand<object>(async o =>
        {
            if (BrowserInstance != null)
            {
                await BrowserInstance.EvaluateScriptAsync(@"
                    var media = document.querySelector('video, audio');
                    if (media) {
                        if (media.paused) media.play();
                        else media.pause();
                    }
                ");
            }
        });

        public ICommand MediaNextCommand => new RelayCommand<object>(async o =>
        {
            if (BrowserInstance != null)
            {
                await BrowserInstance.EvaluateScriptAsync(@"
                    // Próba kliknięcia przycisku 'Next' (YouTube specyficzne)
                    var ytNext = document.querySelector('.ytp-next-button');
                    if (ytNext) {
                        ytNext.click();
                    } else {
                        // Fallback: przewiń o 10 sekund
                        var media = document.querySelector('video, audio');
                        if (media) media.currentTime += 10; 
                    }
                ");
            }
        });
        public ICommand MediaPrevCommand => new RelayCommand<object>(async o =>
        {
            if (BrowserInstance != null)
            {
                await BrowserInstance.EvaluateScriptAsync(@"
                    var ytPrev = document.querySelector('.ytp-prev-button');
                    if (ytPrev) {
                        ytPrev.click();
                    } else {
                        var media = document.querySelector('video, audio');
                        if (media) media.currentTime -= 10;
                    }
                ");
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