using CefSharp;
using CefSharp.Wpf;
using System;
using System.IO;
using System.Windows;

namespace AeroSurf
{
    public class Controller : IDisposable
    {
        private readonly Model model;
        private readonly View view;

        public Controller(Model model, View view)
        {
            this.model = model;
            this.view = view;
            // Set the DataContext of the View to an instance of ViewModel
            view.DataContext = new ViewModel();
        }
        public void Dispose()
        {
            model.Dispose();
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
        }
    }
}
