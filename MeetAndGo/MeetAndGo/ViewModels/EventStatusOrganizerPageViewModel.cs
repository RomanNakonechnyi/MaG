using Acr.UserDialogs;
using MeetAndGo.Constants;
using MeetAndGo.Models;
using MeetAndGo.Models.Enums;
using MeetAndGo.Resources;
using MeetAndGo.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetAndGo.ViewModels {
    public class EventStatusOrganizerPageViewModel : EventStatusPage {
        public ICommand DeleteUserCommand => new DelegateCommand<UserModel> ( async user => await DeleteUser ( user ) );
        public ICommand StartEventCommand => new DelegateCommand ( async () => await StartEvent () );
        public ICommand EndEventCommand => new DelegateCommand ( async () => await CloseEvent () );

        public EventStatusOrganizerPageViewModel ( INavigationService navigationService )
            : base ( navigationService ) {

        }

        #region Command methods

        private async Task DeleteUser ( UserModel user ) {
            var alertMessage = $"Are you sure you want to delete {user.FirstName} {user.LastName}?";
            var result = await UserDialogs.Instance.ConfirmAsync ( alertMessage,
                                                                   ResourcesForUserDialogs.AlertTitle,
                                                                   ResourcesForUserDialogs.OKButton,
                                                                   ResourcesForUserDialogs.CancelButton );

            if ( result ) {
                UserList.Remove ( user );

                await _apiService.RemoveUserFromEventAsync ( user, Event );
            }
        }

        private async Task StartEvent () {
            Event.CreatedTime = DateTime.Now;
            Event.State = EventState.Activated;

            await _apiService.ChangeEventAsync ( Event );

            var navParams = new NavigationParameters ();
            navParams.Add ( nameof ( EventModel ), Event );

            await NavigationService.NavigateAsync ( "../EventMemberPage", navParams );
        }

        private async Task CloseEvent () {
            var result = await UserDialogs.Instance.ConfirmAsync ( ResourcesForUserDialogs.MessageForClosingEvent,
                                                                   ResourcesForUserDialogs.AlertTitle,
                                                                   ResourcesForUserDialogs.OKButton,
                                                                   ResourcesForUserDialogs.CancelButton );

            if ( result ) {
                Event.State = EventState.Canceled;

                await _apiService.ChangeEventAsync ( Event );

                _secureStorage.DeleteKey ( "StartLocation" );
                _secureStorage.DeleteKey ( "EndLocation" );
                _secureStorage.DeleteKey ( "eventId" );

                _secureStorage.SetValue ( "userStatus", UserStatus.User.ToString () );

                foreach ( var user in UserList ) {
                    await _apiService.RemoveUserFromEventAsync ( user, Event );
                }
                
                await NavigationService.NavigateAsync ( "../SearchPage" );
            }
        }

        #endregion Command methods
    }
}