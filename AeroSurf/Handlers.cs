using CefSharp;
using CefSharp.Handler;

namespace AeroSurf
{
    public class AdBlockRequestHandler : ResourceRequestHandler
    {
        private readonly string[] _blockedDomains = new[]
        {
            "doubleclick.net", "googleadservices.com", "googlesyndication.com",
            "moatads.com", "adnxs.com", "facebook.com/tr"
        };

        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            foreach (var domain in _blockedDomains)
            {
                if (request.Url.Contains(domain))
                {
                    return CefReturnValue.Cancel;
                }
            }
            return CefReturnValue.Continue;
        }
    }

    public class CustomRequestHandler : RequestHandler
    {
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return new AdBlockRequestHandler();
        }
    }
}