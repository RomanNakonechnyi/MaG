using MeetAndGo.Models.Enums;
using MeetAndGo.ViewModels;
using Xamarin.Forms;

namespace MeetAndGo.Views {
    public partial class MenuMasterPage : MasterDetailPage {
        public MenuMasterPage () {
            InitializeComponent ();

            this.IsPresentedChanged += MenuMasterPage_IsPresentedChanged;
        }

        private async void MenuMasterPage_IsPresentedChanged ( object sender, System.EventArgs e ) {
            var context = ( MenuMasterPageViewModel )BindingContext;

            await context.CheckUserStatus ();

            if ( context.User?.Status == UserStatus.User ) {
                context.EventStatusIsEnabled = false;
            }
            if ( context.User?.Status == UserStatus.Member || context.User?.Status == UserStatus.Organizer ) {
                context.EventStatusIsEnabled = true;
            }
        }
    }
}