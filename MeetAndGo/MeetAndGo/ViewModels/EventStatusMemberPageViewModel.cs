using Acr.UserDialogs;
using MeetAndGo.Constants;
using MeetAndGo.Models.Enums;
using MeetAndGo.Resources;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetAndGo.ViewModels {
    public class EventStatusMemberPageViewModel : EventStatusPage {
        public ICommand QuitCommand => new DelegateCommand ( async () => await ExecuteQuitCommand () );

        public EventStatusMemberPageViewModel ( INavigationService navigationService )
            : base ( navigationService ) {
                
        }

        #region Command methods

        private async Task ExecuteQuitCommand () {
            var result = await UserDialogs.Instance.ConfirmAsync ( ResourcesForUserDialogs.MessageForQuitingEvent, 
                                                                   ResourcesForUserDialogs.AlertTitle,
                                                                   ResourcesForUserDialogs.OKButton,
                                                                   ResourcesForUserDialogs.CancelButton );

            if ( result ) {
                _secureStorage.DeleteKey ( "StartLocation" );
                _secureStorage.DeleteKey ( "EndLocation" );
                _secureStorage.DeleteKey ( "eventId" );

                _secureStorage.SetValue ( "userStatus", UserStatus.User.ToString () );

                await _apiService.LeaveEventAsync ( Event );
                await NavigationService.NavigateAsync ( "../SearchPage" );
            }
        }

        #endregion Command methods
    }
}