using CefSharp.Wpf;

namespace AeroSurf
{
    public class Model
    {
        public ChromiumWebBrowser Browser { get; private set; }

        public void Initialize(string url)
        {
            Browser = new ChromiumWebBrowser(url);
        }
    }
}
