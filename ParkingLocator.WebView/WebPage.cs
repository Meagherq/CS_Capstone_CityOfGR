using Xamarin.Forms;

namespace WorkingWithWebview
{
    public class WebPage : ContentPage
    {
        public WebPage()
        {
            var browser = new WebView();
            browser.Source = "http://2da9636c.ngrok.io";
            Content = browser;            
        }
    }
}

