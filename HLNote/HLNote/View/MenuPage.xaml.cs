using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HLNote
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : MasterDetailPage
	{
        private GenreService _genreService = new GenreService();
        public MenuPage ()
		{
			InitializeComponent ();
		}
        protected override void OnAppearing()
        {
            _genreService.GetData();
            listMenu.ItemsSource = _genreService.Genres;
            base.OnAppearing();
        }
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var genre = e.SelectedItem as Genre;
            var page = new HomePage(genre);
            listMenu.SelectedItem = null;
            Detail = new NavigationPage(page);
            IsPresented = false;
        }

        private void GoHome(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new HomePage());
            IsPresented = false;
        }

        private void GoGenre(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new GenreManager(_genreService.Genres));
            IsPresented = false;
        }

        private void GoFeedBack(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Feedback());
            IsPresented = false;
        }
    }
}