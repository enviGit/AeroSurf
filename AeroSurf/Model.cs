using CefSharp;
using CefSharp.Wpf;
using System;

namespace AeroSurf
{
    public class Model
    {
        public ChromiumWebBrowser Browser { get; private set; }
        public string SearchText { get; set; }

        public Model()
        {
            Browser = new ChromiumWebBrowser();
            NavigateToHome();
        }
        public void Dispose()
        {
            Browser.Dispose();
        }
        public void GoBack()
        {
            if (Browser.CanGoBack)
                Browser.Back();
        }
        public void GoForward()
        {
            if (Browser.CanGoForward)
                Browser.Forward();
        }
        public void Refresh()
        {
            Browser.Reload();
        }
        public void Stop()
        {
            if (Browser != null)
                Browser.Stop();
        }
        public void NavigateToHome()
        {
            Browser.Load("https://www.google.com");
        }
        public void Search(string searchText)
        {
            NavigateTo($"https://www.google.com/search?q={searchText}");
        }
        public void NavigateTo(string url)
        {
            Uri uri;

            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                Browser.Load(uri.ToString());
            else
                Browser.Load($"https://www.google.com/search?q={url}");
        }
    }
}
