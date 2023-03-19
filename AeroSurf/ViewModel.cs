using CefSharp;
using CefSharp.Wpf;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AeroSurf
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Model model;
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            this.model = new Model();

            // Attach event handlers to implement lazy loading
            model.Browser.FrameLoadEnd += OnFrameLoadEnd;
            model.Browser.LoadingStateChanged += OnLoadingStateChanged;
        }
        public ChromiumWebBrowser Browser
        {
            get { return model.Browser; }
        }
        public string SearchText
        {
            get { return model.SearchText; }
            set
            {
                model.SearchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        public ICommand PreviousCommand
        {
            get { return new RelayCommand(() => model.GoBack(), () => model.Browser != null && model.Browser.CanGoBack); }
        }
        public ICommand NextCommand
        {
            get { return new RelayCommand(() => model.GoForward(), () => model.Browser != null && model.Browser.CanGoForward); }
        }
        public ICommand RefreshCommand
        {
            get { return new RelayCommand(() => model.Refresh()); }
        }
        public ICommand StopCommand
        {
            get { return new RelayCommand(() => model.Stop()); }
        }
        public ICommand HomeCommand
        {
            get { return new RelayCommand(() => model.NavigateToHome()); }
        }
        public ICommand SearchCommand
        {
            get { return new RelayCommand(() => model.Search(SearchText)); }
        }

        private async Task RemoveUnnecessaryElements()
        {
            // Remove all elements with the class "advertisement"
            await model.Browser.EvaluateScriptAsync(@"
                var ads = document.getElementsByClassName('advertisement');

                for(var i = 0; i < ads.length; i++) 
                {
                    ads[i].remove();
                }
            ");
            // Remove all elements with the class "ad"
            await model.Browser.EvaluateScriptAsync(@"
                var ads = document.getElementsByClassName('ad');

                for(var i = 0; i < ads.length; i++) 
                {
                    ads[i].remove();
                }
            ");
            // Remove all elements with the class "advert"
            await model.Browser.EvaluateScriptAsync(@"
                var ads = document.getElementsByClassName('advert');

                for(var i = 0; i < ads.length; i++) 
                {
                    ads[i].remove();
                }
            ");
            // Remove all elements with the class "social-media"
            await model.Browser.EvaluateScriptAsync(@"
                var socialMedia = document.getElementsByClassName('social-media');

                for(var i = 0; i < socialMedia.length; i++)
                {
                    socialMedia[i].remove();
                }
            ");
            // Add more code to remove other unnecessary elements
        }
        private async void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            // Wait for the document to finish loading
            await Task.Delay(1000);
            // Remove unnecessary elements
            await RemoveUnnecessaryElements();
            // Inject JavaScript to implement lazy loading
            await model.Browser.EvaluateScriptAsync(@"
                window.addEventListener('scroll', function() 
                {
                    var images = document.querySelectorAll('img[data-src]');

                    for (var i = 0; i < images.length; i++) 
                    {
                        var image = images[i];
                        var rect = image.getBoundingClientRect();

                        if (rect.top >= 0 && rect.bottom <= window.innerHeight) 
                        {
                            image.src = image.dataset.src;
                            image.removeAttribute('data-src');
                        }
                    }
                });
            ");
        }
        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            // Hide images until they are loaded
            if (!e.IsLoading)
            {
                model.Browser.ExecuteScriptAsync(@"
                    var images = document.querySelectorAll('img[src]:not([src=""])');

                    for (var i = 0; i < images.length; i++) 
                    {
                        var image = images[i];

                        if (!image.complete) 
                        {
                            image.style.visibility = 'hidden';
                            image.addEventListener('load', function() 
                            {
                                image.style.visibility = 'visible';
                            });
                        }
                    }
                ");
            }
        }
        private class RelayCommand : ICommand
        {
            private readonly Action execute;
            private readonly Func<bool> canExecute;

            public RelayCommand(Action execute, Func<bool> canExecute = null)
            {
                this.execute = execute;
                this.canExecute = canExecute;
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return canExecute == null || canExecute();
            }

            public void Execute(object parameter)
            {
                execute();
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
