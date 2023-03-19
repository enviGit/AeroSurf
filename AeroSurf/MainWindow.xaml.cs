using CefSharp;
using CefSharp.Wpf;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace AeroSurf
{
    public partial class MainWindow : Window
    {
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
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Dispose of ChromiumWebBrowser instance
            var browser = (ChromiumWebBrowser)this.Content;
            browser.Dispose();

            // Shut down CefSharp
            Cef.Shutdown();
        }
    }
}
