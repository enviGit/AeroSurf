using System;
using System.IO;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;

namespace AeroSurf
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var settings = new CefSettings();
            settings.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AeroSurfCache");

            settings.CefCommandLineArgs.Add("enable-gpu");
            settings.CefCommandLineArgs.Add("renderer-process-limit", "10");

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
        }
    }
}