using Xamarin.Forms;

namespace WorkingWithWebview
{
    public class WebPage : ContentPage
    {
        public WebPage()
        {
            var browser = new WebView();
            browser.Source = "https://3a5a5a5a.ngrok.io";
            Content = browser;            
        }
    }
}

