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
	public partial class ForgotPasswordPage : ContentPage
	{
        public event EventHandler<Password> ChangeSuccessful;
        public event EventHandler<Password> ChangeFail;
        private Password _password;
        private QuestionService _questionService = new QuestionService();
        public ForgotPasswordPage (Password password=null)
		{
			InitializeComponent ();
            if (password != null)
            {
                _password = password;
                _questionService.GetData();
                DisplayData();
            }
            
		}
        void DisplayData()
        {
            question.Text = _password.Question.QuestionStr;
        }
        private void answer_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(String.IsNullOrEmpty(newPassword.Text))
                DisplayAlert("Noitice", "Password must be filled!", "Ok");

        }
        private void password2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(rePassword.Text))
                DisplayAlert("Noitice", "Retype Password must be filled!", "Ok");
        }

        private void OnComfirm(object sender, EventArgs e)
        {
            if (String.Compare(answer.Text, _password.Answer) == 0)
            {
                if(String.IsNullOrEmpty(newPassword.Text)||String.IsNullOrEmpty(rePassword.Text))
                    DisplayAlert("Noitice", "Password and Retype password must not be empty!", "Ok");
                else if (String.Compare(newPassword.Text,rePassword.Text)==0)
                {
                    DisplayAlert("Noitice", "Password Changed successful!", "Ok");
                    _password.PasswordStr = newPassword.Text;
                    ChangeSuccessful?.Invoke(this, _password);
                }
                else
                {
                    DisplayAlert("Noitice", "Reinput Password does not match!", "Ok");
                }
            }
            else
            {
                DisplayAlert("Noitice", "Wrong answer!", "Ok");
                ChangeFail?.Invoke(this, _password);
            }
        }
    }
}