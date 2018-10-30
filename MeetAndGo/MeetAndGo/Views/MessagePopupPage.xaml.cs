using MeetAndGo.ViewModels;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeetAndGo.Views
{
    public partial class MessagePopupPage : PopupPage
    {
        public MessagePopupPage()
        {
            InitializeComponent();
        }

        private async Task TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            entry.Unfocus();
            if (BindingContext is MessagePopupPageViewModel model)
            {
                if (!entry.IsFocused)
                {
                    await Task.Delay(50);
                    await model.ExecuteSendCommand();
                }
            }
        }

        private async Task PopupPage_BackgroundClicked(object sender, System.EventArgs e)
        {
            entry.Unfocus();
            if (BindingContext is MessagePopupPageViewModel model)
            {
                await Task.Delay(50);
                await model.ClosePopupPage();
            }
        }
    }
}
