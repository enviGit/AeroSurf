using CefSharp;
using CefSharp.Wpf;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace AeroSurf
{
    public class Controller
    {
        private readonly Model model;
        private readonly View view;

        public Controller(Model model, View view)
        {
            this.model = model;
            this.view = view;
            this.view.Initialize += Initialize;
            this.view.Closing += Closing;
        }

        private void Initialize(object sender, RoutedEventArgs e)
        {
            // Check if Cef has been initialized already
            if (!Cef.IsInitialized)
            {
                var settings = new CefSettings();
                // Cache settings
                settings.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AeroSurfCache");
                // Hardware acceleration settings
                settings.CefCommandLineArgs.Add("enable-gpu", "1");
                settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1");
                settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling", "1");
                settings.CefCommandLineArgs.Add("use-gl", "swiftshader");
                settings.CefCommandLineArgs.Add("use-angle", "gl");
                // Enable gzip compression
                settings.CefCommandLineArgs.Add("disable-features", "NetworkService");
                settings.CefCommandLineArgs.Add("enable-features", "NetworkServiceInProcess");
                settings.CefCommandLineArgs.Add("enable-gzip", "1");
                // Initialize CefSharp with custom settings
                Cef.Initialize(settings);
            }

            // Initialize the model
            model.Initialize("https://www.google.com");

            // Set the content of the view to the browser control
            view.Content = model.Browser;

            // Attach event handlers to implement lazy loading
            model.Browser.FrameLoadEnd += OnFrameLoadEnd;
            model.Browser.LoadingStateChanged += OnLoadingStateChanged;
        }

        private void Closing(object sender, CancelEventArgs e)
        {
            // Dispose of ChromiumWebBrowser instance
            model.Browser.Dispose();

            // Shut down CefSharp
            Cef.Shutdown();
        }

        private async void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            // Wait for the document to finish loading
            await Task.Delay(1000);

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
    }
}
