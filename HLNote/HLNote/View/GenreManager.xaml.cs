using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HLNote
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GenreManager : ContentPage
	{
        private GenreService _genreService = new GenreService();
        private bool _IsDataLoaded;

        public GenreManager (ObservableCollection<Genre> genres=null)
		{
			InitializeComponent ();
            if(genres!=null)
            _genreService.Genres = genres;
		}
        protected override void OnAppearing()
        {
            //if (_IsDataLoaded)
            //{
            //    return;
            //}
            //_IsDataLoaded = true;
            //_genreService.GetData();
            genreList.ItemsSource = _genreService.Genres;
            base.OnAppearing();
        }

        private void OnAdd(object sender, EventArgs e)
        {
            var genre = new Genre { GenreName = newGenre.Text };
            _genreService.AddGenre(genre);
            DisplayAlert("Noitice", "Added!", "OK");
            newGenre.Text = null;
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            
            //Button button = sender as Button;
            //StackLayout listViewItem = button.Parent as StackLayout;
            //Entry entry = listViewItem.Children[0] as Entry;
            //string text = entry.Text;
            //var genre = _genreService.Genres.

            var genre = (sender as Button).BindingContext as Genre;
            if (await DisplayAlert("Warning", $"Are you sure you want to delete { genre.GenreName}?", "Yes", "No"))
            {
                _genreService.RemoveGenre(genre);
                await DisplayAlert("Noitice", "Deleted!", "OK");
                RefreshItems();
            }
            //newGenre.Text = null;
        }
        void RefreshItems()
        {
            genreList.ItemsSource = _genreService.Genres;
        }

        private void OnEdit(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)sender;
            StackLayout listViewItem = (StackLayout)entry.Parent;
            var button = listViewItem.Children[1] as Button;
            button.IsVisible = true;
        }

        private void OnEditAction(object sender, EventArgs e)
        {
            DisplayAlert("Noitice", "Tapped", "OK");
            var stackLayout = (StackLayout)sender;
            var button = stackLayout.Children[1] as Button;
            button.IsVisible = true;
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            Entry entry = (Entry)sender;
            StackLayout listViewItem = (StackLayout)entry.Parent;
            var button = listViewItem.Children[1] as Button;
            button.IsVisible = true;
            var updateButton = listViewItem.Children[2] as Button;
            updateButton.IsVisible = true;
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            Entry entry = (Entry)sender;
            StackLayout listViewItem = (StackLayout)entry.Parent;
            var button = listViewItem.Children[1] as Button;
            button.IsVisible = false;
            var updateButton = listViewItem.Children[2] as Button;
            updateButton.IsVisible = false;
        }

        private async void DeleteItem(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var genre = menuItem.CommandParameter as Genre;
            if (await DisplayAlert("Warning", $"Are you sure you want to delete { genre.GenreName}?", "Yes", "No"))
                _genreService.RemoveGenre(genre);
            await DisplayAlert("Noitice", "Deleted!", "OK");
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            var genre = (sender as Button).BindingContext as Genre;
            //if (await DisplayAlert("Warning", $"Are you sure you want to delete { genre.GenreName}?", "Yes", "No"))
                _genreService.ReplaceGenre(genre);
            DisplayAlert("Noitice", "Updated!", "OK");
            RefreshItems();
        }
    }
}