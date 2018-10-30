using MeetAndGo.Models;
using MeetAndGo.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeetAndGo.Views {
    [XamlCompilation ( XamlCompilationOptions.Compile )]
    public partial class MemberViewCell : ViewCell {
        public EventStatusOrganizerPageViewModel ViewModel { get; private set; }

        public MemberViewCell () {
            InitializeComponent ();

            ViewModel = ( EventStatusOrganizerPageViewModel )EventStatusOrganizerPage.Instance.BindingContext;
        }

        private void Button_Clicked ( object sender, EventArgs e ) {
            var user = BindingContext as UserModel;
            ViewModel.DeleteUserCommand.Execute ( user );
        }
    }
}