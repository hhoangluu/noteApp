using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
//using Android.OS;

namespace HLNote
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Feedback : ContentPage
    {
        string bodyEmail = "";
        public Feedback()
        {
            InitializeComponent();

        }
        void EditorCompleted(object sender, EventArgs e)
        {
            var text = ((Editor)sender).Text; // sender is cast to an Editor to enable reading the `Text` property of the view.
            bodyEmail = text as string;
        }

        private async void SendOnClicked(object sender, EventArgs args)
        {
            List<string> recipients = new List<string>();

            recipients.Add("luuhoang231@gmail.com");
            await SendEmail("[Feedback HLNote Android]", bodyEmail, recipients);
        }
        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            //try
            //{
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            //}
            //catch (FeatureNotSupportedException fbsEx)
            //{
            //    // Email is not supported on this device
            //}
            //catch (Exception ex)
            //{
            //    // Some other exception occurred
            //}
        }
    }
}
