using Xamarin.Forms;

namespace WorkingWithWebview
{
	public class App : Application
	{
		public App ()
		{
            var page = new Page();

            page = new WebPage();
            MainPage = page;
		}
	}
}

