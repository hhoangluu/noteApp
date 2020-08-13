using Rg.Plugins.Popup.Services;
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
	public partial class HomePage : ContentPage
	{
        private NoteService _noteService = new NoteService();
        private bool _IsDataLoaded;
        private Genre _genre = null;
        public HomePage ()
		{
			InitializeComponent ();
            //listView.ItemsSource = _noteService.GetNotes();
        }
        public HomePage(Genre genre)
        {
            InitializeComponent();
            _genre = genre;
        }

        protected override void OnAppearing()
        {
            if (_IsDataLoaded)
            {
                return;
            }
            //_IsDataLoaded = true;
            //_noteService.Notes = _dbConnectorNote.LoadData();
            _noteService.GetData();
            if (_genre == null)
                listView.ItemsSource = _noteService.GetNotes();
            else
                listView.ItemsSource = _noteService.Notes.Where(c => c.GenreId == _genre.GenreId);
            base.OnAppearing();
        }
        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var note = menuItem.CommandParameter as Note;
            if(await DisplayAlert("Warning", $"Are you sure you want to delete { note.Title}?", "Yes", "No"))
            {
                _noteService.RemoveNote(note);
                //_dbConnector.RemoveNoteData(note);
                //_dbConnectorNote.RemoveData(note);
                await DisplayAlert("Noitice", "Deleted!", "OK");
                listView.ItemsSource = _noteService.GetNotes();
            }
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var note = e.SelectedItem as Note;
            var page = new EditNote(note);
            page.NoteEdit += (source, Note) =>
            {
                _noteService.ReplaceNote(Note);
                //_dbConnector.UpdateNoteData(Note);
                //_dbConnectorNote.UpdateData(Note);
                DisplayAlert("Noitice", "Updated!", "OK");
                Navigation.PopAsync();
            };
            if (note.NotePassword != null)
            {
                var passwordPage = new PasswordPage(note.NotePassword);
                passwordPage.LoginSuccess += async (source, Snote) =>
                {
                    await Navigation.PopAsync();    //pop password page
                    await Navigation.PushAsync(page);
                };
                passwordPage.LoginFail += async (source, Snote) =>
                {
                    await Navigation.PopAsync();    //pop password page
                    return;
                };
                await Navigation.PushAsync(passwordPage);   //push password page
            }
            else
            await Navigation.PushAsync(page);
            listView.SelectedItem = null;
        }
        private async void OnAdd(object sender, EventArgs e)
        {
            var page = new EditNote();
            page.NoteAdd += (source, Note) =>
            {
                _noteService.AddNote(Note);
                //_dbConnector.InsertNoteData(Note);
                //_dbConnectorNote.InsertData(Note);
                DisplayAlert("Noitice", "Added!", "OK");
                Navigation.PopAsync();
            };
            await Navigation.PushAsync(page);
        }
        private async void OnFeedBack(object sender, EventArgs e)
        {
            var page = new Feedback();
            await Navigation.PushAsync(page);
        }
        private async void OnPassword(object sender, EventArgs e)
        {
            var page = new PasswordPage();
            await Navigation.PushAsync(page);
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            listView.ItemsSource = _noteService.GetNotes(e.NewTextValue);
        }

        private void ListView_Refreshing(object sender, EventArgs e)
        {
            listView.ItemsSource = _noteService.GetNotes();
            listView.EndRefresh();
        }
    }
}