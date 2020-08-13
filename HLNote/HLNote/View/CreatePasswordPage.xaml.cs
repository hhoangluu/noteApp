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
	public partial class CreatePasswordPage : ContentPage
	{
        public event EventHandler<Password> AddSuccess;
        public event EventHandler<Password> TurnOff;
        private Password _password=null;
        private QuestionService _questionService = new QuestionService();
		public CreatePasswordPage (Password password=null)
		{
			InitializeComponent ();
            if (password != null)
                _password = password;
        }
        protected override void OnAppearing()
        {
            switcher.IsToggled = _password!=null;
            _questionService.GetData();
            questions.ItemsSource = _questionService.Questions;
            base.OnAppearing();
        }

        private void OnSelect(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var selectedItem = picker.SelectedItem as Question;
            _password.Question = selectedItem;
            //DisplayAlert("Noitice", selectedItem.QuestionStr, "OK");
        }

        private void OnComfirm(object sender, EventArgs e)
        {
            if (_password.Question != null)
            {
                if (String.IsNullOrEmpty(inputPassword.Text) || String.IsNullOrEmpty(rePassword.Text))
                    DisplayAlert("Noitice", "Password and Retype password must not be empty!", "Ok");
                else if (String.Compare(inputPassword.Text, rePassword.Text) == 0)
                {
                    DisplayAlert("Noitice", "Create Password successful!", "Ok");
                    _password.PasswordStr = inputPassword.Text;
                    _password.Answer = answer.Text;
                    AddSuccess?.Invoke(this, _password);
                }
                else
                {
                    DisplayAlert("Noitice", "Reinput Password does not match!", "Ok");
                }
            }
            else
            {
                DisplayAlert("Noitice", "Please select Secret Questions!", "OK");
            }
        }

        private void InputPasswod_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(inputPassword.Text))
                DisplayAlert("Noitice", "Password must be filled!", "Ok");
        }

        private void RePasswod_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(rePassword.Text))
                DisplayAlert("Noitice", "Retype Password must be filled!", "Ok");
        }

        private async void Onchange(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (switcher.IsToggled)
            {
                _password = new Password();
                passwordForm.IsVisible = true;
            }
            else
            {
                if (await DisplayAlert("Warning", $"Are you sure you want to delete password?", "Yes", "No")&&_password!=null)
                {
                    _password = null;
                    passwordForm.IsVisible = false;
                    TurnOff?.Invoke(this,_password);
                }
                else switcher.IsToggled = true;
            }
        }
    }
}