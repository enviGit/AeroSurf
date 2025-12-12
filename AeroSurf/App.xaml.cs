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
            settings.LogSeverity = LogSeverity.Disable;
            settings.CefCommandLineArgs.Add("no-proxy-server");
            settings.CefCommandLineArgs.Add("enable-quic");
            settings.CefCommandLineArgs.Add("enable-gpu");
            settings.CefCommandLineArgs.Add("enable-gpu-rasterization");
            settings.CefCommandLineArgs.Add("disable-gpu-vsync"); 
            settings.CefCommandLineArgs.Add("renderer-process-limit", "10");
            settings.CefCommandLineArgs.Add("autoplay-policy", "no-user-gesture-required");
            settings.CefCommandLineArgs.Add("site-per-process");

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
        }
    }
}