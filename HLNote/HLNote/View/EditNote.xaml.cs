using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
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
	public partial class EditNote : ContentPage
	{
        public event EventHandler<Note> NoteAdd;
        public event EventHandler<Note> NoteEdit;
        private GenreService _genreService = new GenreService();
        private ColorService _colorService = new ColorService();
        private QuestionService _questionService = new QuestionService();
        private int _noteId;
        private Note _note;

        bool flag = true;
        int j = 0;
        int i = -1;

        public EditNote(Note note=null)
		{
			InitializeComponent ();
            if (note != null)
            {
                _note = note;
                _noteId = note.NoteId;
                displayNote(_note);
            }
            else
            {
                _note = new Note();
                _noteId = 0;
                _note.Content = new NoteContent { Images = new List<NoteImage>() };
            }
            _genreService.GetData();
            _colorService.GetData();
            _questionService.GetData();
            ListColor.ItemsSource = _colorService.Colors;
            genre.ItemsSource = _genreService.Genres;
            if (_note.NoteColor!=null)
            this.BackgroundColor = Xamarin.Forms.Color.FromHex(_note.NoteColor.ColorName);
        }
        void displayNote(Note note)
        {
            if (note.NoteGenre != null)
                genre.Title = note.NoteGenre.GenreName;
            else genre.Title = "None";
            titleEditor.Text = note.Title;
            contentEditor.Text = note.Content.Content;
            if(note.Content.Images!=null)
            {
                foreach (var image in note.Content.Images)
                {
                    ReadImage(image.FilePath);
                }
            }
            
        }

        private void SaveBtn_Clicked(object sender, EventArgs e)
        {
            if(String.IsNullOrWhiteSpace(titleEditor.Text)&&String.IsNullOrWhiteSpace(contentEditor.Text))
            {
                DisplayAlert("Ops", "Title or Content of Note must be filled!","OK");
                return;
            }
            if (_noteId == 0)
            {
                _note.Title = titleEditor.Text;
                _note.Content.Content = contentEditor.Text;
                if(_note.NoteColor==null)
                _note.NoteColor = _colorService.Colors[0];
                _note.LastModify = DateTime.Now.ToString("dd-MM-yyyy HH:mm tt");
                NoteAdd?.Invoke(this, _note);
            }
            else
            {
                _note.Title = titleEditor.Text;
                _note.Content.Content = contentEditor.Text;
                _note.LastModify = DateTime.Now.ToString("dd-MM-yyyy HH:mm tt");
                NoteEdit?.Invoke(this, _note);
            }
        }

        private async void AddPassword(object sender, EventArgs e)
        {
            var page = new CreatePasswordPage(_note.NotePassword);
            page.AddSuccess += async (source, password) =>
            {
                _note.NotePassword = password;
                await Navigation.PopAsync();
            };
            page.TurnOff += async (source, password) =>
            {
                _note.NotePassword = password;
                await Navigation.PopAsync();
            };
            await Navigation.PushAsync(page);
        }
        void ReadImage(string filePath)// đọc ảnh
        {
            Image img = new Image();
            img.Source = ImageSource.FromFile(filePath);
            imgGrid.Children.Add(img);

            if (j % 3 == 0)
            {
                i++;
            }
            Grid.SetRow(img, i);
            Grid.SetColumn(img, j - 3 * i);

            if (j % 3 == 0 && j != 0)
            {
                imgGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star)
                });

            }

            if (j < 3)
            {
                imgGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
                j++;
            }
            ZoomImageNote(filePath, img);
        }
        void ZoomImageNote(string filePath, Image image)//
        {
             var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                ZoomImage.IsVisible = true;
                ZoomImageNote(filePath, image);
            };
            image.GestureRecognizers.Add(tapGestureRecognizer);
            ZoomImage.Source = filePath;

        }
        private void TapZoomImage(object sender, EventArgs e)//click nho anh
        {
            {
                ZoomImage.IsVisible = false;
            }
        }

        private async void BtnAddImage_Clicked(object sender, EventArgs e)
        {
            try
            {
                FileData file = new FileData();
                file = await CrossFilePicker.Current.PickFile();
                await DisplayAlert("Noitice", "Successfully inserted images!", "OK");
                if (file != null)
                {
                    ReadImage(file.FilePath);
                    if (_note.Content.Images != null)
                        _note.Content.AddImage(new NoteImage { FilePath = file.FilePath,
                                                                FileName = file.FileName});
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error at", ex.Message, "OK");
            }
        }

        private void BtnColor_Clicked(object sender, EventArgs e)
        {
            if (flag == false)
            {
                ListColor.IsVisible = true;
                flag = true;
            }
            else
            {
                ListColor.IsVisible = false;
                flag = false;
            }
        }

        private void LbColorName_Clicked(object sender, EventArgs e)

        {
            ListColor.IsVisible = false;
            flag = false;
        }
        private async void OnSwiped(object sender, SwipedEventArgs e)// delete Image
        {
            var leftSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            leftSwipeGesture.Swiped += OnSwiped;
            var rightSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
            rightSwipeGesture.Swiped += OnSwiped;
            if (!await DisplayAlert("Warning", "Are you sure you want to delete this image?", "Yes", "No"))
                return;
            Image image = sender as Image;
            var filePath = (FileImageSource)image.Source;
            var noteImage = _note.Content.Images.First(c => c.FilePath == filePath.File);
            _note.Content.RemoveImage(noteImage);
            ZoomImage.IsVisible = false;
            j = 0;
            i = -1;
            imgGrid.Children.Clear();
            imgGrid.RowDefinitions.Clear();
            imgGrid.ColumnDefinitions.Clear();
            if (_note.Content.Images != null)
            {
                foreach (var nImage in _note.Content.Images)
                {
                    ReadImage(nImage.FilePath);
                }
            }
        }

        private void ChooseColor(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var color = e.SelectedItem as Color;
            _note.NoteColor = color;
               this.BackgroundColor = Xamarin.Forms.Color.FromHex(color.ColorName);
            ListColor.SelectedItem = null;
            ListColor.IsVisible = false;
            
        }

        private void OnSelectGenre(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var selectedItem = picker.SelectedItem as Genre;
            _note.NoteGenre = selectedItem;
        }
    }
}