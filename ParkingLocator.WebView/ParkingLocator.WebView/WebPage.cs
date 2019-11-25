using Xamarin.Forms;

namespace WorkingWithWebview
{
    public class WebPage : ContentPage
    {
        public WebPage()
        {
            var browser = new WebView();
            browser.Source = "http://43b74cc6.ngrok.io/";
            Content = browser;            
        }
    }
}