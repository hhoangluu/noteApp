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
	public partial class PasswordPage : ContentPage
	{
        public event EventHandler<Password> LoginSuccess;
        public event EventHandler<Password> LoginFail;
        private Password _password;
        public PasswordPage (Password password=null)
		{
			InitializeComponent ();
            if (password!=null)
            _password = password;
            // note service
		}
       private void editor_TextChanged (object sender, EventArgs e)
        {

        }
        private async void btn_ForgorPasswordCLicked(object sender, EventArgs e)
        {
            var page = new ForgotPasswordPage(_password);
            page.ChangeSuccessful += async (source, password)=>{
                await Navigation.PopAsync();
            };
            page.ChangeFail += async (source, password) => {
                 await Navigation.PopAsync();
            };
            await Navigation.PushAsync(page);
        }

        private void OnLogin(object sender, EventArgs e)
        {
            if (String.Compare(input.Text, _password.PasswordStr) == 0)
            {
                DisplayAlert("Noitice", "Login successful!", "OK");
                LoginSuccess?.Invoke(this, _password);
            }
            else
            {
                DisplayAlert("Noitice", "Login fail!", "OK");
                LoginFail?.Invoke(this, _password);
            }

        }
    } 
}