using CefSharp;
using CefSharp.Wpf;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace AeroSurf
{
    public partial class MainWindow : Window
    {
        private ChromiumWebBrowser browser;

        public MainWindow()
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
                // Initialize CefSharp with custom settings
                Cef.Initialize(settings);
            }

            InitializeComponent();

            // Initialize ChromiumWebBrowser instance
            browser = new ChromiumWebBrowser("https://www.google.com");

            // Add the control to the form
            Content = browser;

            // Attach event handlers to implement lazy loading
            browser.FrameLoadEnd += OnFrameLoadEnd;
            browser.LoadingStateChanged += OnLoadingStateChanged;
        }
        private async void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            // Wait for the document to finish loading
            await Task.Delay(1000);

            // Inject JavaScript to implement lazy loading
            await browser.EvaluateScriptAsync(@"
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
                browser.ExecuteScriptAsync(@"
                    var images = document.querySelectorAll('img[src]:not([src=""])');
                    for (var i = 0; i < images.length; i++) {
                        var image = images[i];
                        if (!image.complete) {
                            image.style.visibility = 'hidden';
                            image.addEventListener('load', function() {
                                image.style.visibility = 'visible';
                            });
                        }
                    }
                ");
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Dispose of ChromiumWebBrowser instance
            browser.Dispose();

            // Shut down CefSharp
            Cef.Shutdown();
        }
    }
}
