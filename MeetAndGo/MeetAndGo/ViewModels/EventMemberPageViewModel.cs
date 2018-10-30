using Acr.UserDialogs;
using MeetAndGo.Helpers;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MeetAndGo.Models;
using System.Collections.ObjectModel;
using MeetAndGo.Views;
using MeetAndGo.Models.Enums;
using MeetAndGo.Services;
using Plugin.SecureStorage.Abstractions;
using Plugin.SecureStorage;

namespace MeetAndGo.ViewModels {
    public class EventMemberPageViewModel : ViewModelBase {
        private UserModel _currentUser;
        private EventModel _event;
        private ApiService _apiService;
        private readonly ISecureStorage _secureStorage;

        bool _wasExecuted = false;

        public EventModel Event {
            get {
                return GetEvent();
            }
            set { SetProperty(ref _event, value); }
        }

        public UserModel CurrentUser {
            get {
                return _currentUser;
            }
            set { SetProperty ( ref _currentUser, value ); }
        }

        public ICommand FinishCommand => new DelegateCommand(async () => await ExecuteFinishCommand());

        public EventMemberPageViewModel( INavigationService navigationService )
            : base(navigationService) {
            _secureStorage = CrossSecureStorage.Current;
            _apiService = new ApiService ();
        }


        private async Task ExecuteFinishCommand() {
            UpdateStatus(Event.Members);
            _wasExecuted = false; 
            Event.State = EventState.CarriedOut;

            await _apiService.ChangeEventAsync ( Event );
            await _apiService.LeaveEventAsync ( Event );

            await NavigationService.NavigateAsync("MeetAndGo:///MenuMasterPage/NavigationPage/SearchPage");
        }

        public async override void OnNavigatingTo( INavigationParameters parameters ) {
            base.OnNavigatingTo(parameters);

            Console.WriteLine ( _secureStorage.GetValue ( "userId" ) );
            CurrentUser = ( await _apiService.GetUserAsync ( Int32.Parse ( _secureStorage.GetValue ( "userId" ) ) ) ).Data as UserModel;

            if ( parameters != null ) {
                if( parameters.ContainsKey(nameof(VoteModel)) ) {
                    var vote = (VoteModel)parameters[ nameof(VoteModel) ];
                    vote.Candidate.RatingEvent -= RatingUpdated;
                    UpdateRating(Event.Members, vote);
                    ChangePosibilityToRate(Event.Members, vote.Candidate);
                }
                if( parameters.ContainsKey(nameof(EventModel)) ) {
                    Event = (EventModel)parameters[ nameof(EventModel) ];
                }
            }
        }

        #region HelpingFunctions
        private void ChangePosibilityToRate( ObservableCollection<UserModel> userModels, UserModel user ) {
            foreach( var u in userModels ) {
                if( u.FullName == user.FullName && u.CanRate == user.CanRate ) {
                    u.CanRate = false;
                }
            }
        }

        private void NotificateStarSelected( UserModel user ) {
            if( user.TempRate > 0 ) {
                user.RatingEvent += RatingUpdated;
            }
        }

        private void UpdateStatus( ObservableCollection<UserModel> members ) {
            foreach( var member in members ) {
                member.Status = UserStatus.User;
                member.TempRate = 0;
            }
        }

        private void UpdateRating( ObservableCollection<UserModel> members, VoteModel vote ) {
            foreach( var mem in members ) {
                if( mem.FullName == vote.Candidate.FullName ) {
                    mem.Votes.Add(vote);
                    mem.Rating = (int)mem.Votes.Average(s => s.Rating);
                }
            }
        }

        private void RatingUpdated( object sender, EventArgs args ) {
            var candidate = (UserModel)sender;
            var vote = new VoteModel {
                Voter = CurrentUser,
                Candidate = candidate,
                Rating = candidate.TempRate
            };
            var navParam = new NavigationParameters
            {
                {nameof(VoteModel), vote }
            };

            NavigationService.NavigateAsync(nameof(MessagePopupPage), navParam);
        }

        private EventModel GetEvent() {
            if( _event != null && _event.Members != null ) {
                if( !_wasExecuted ) {
                    foreach( var member in _event.Members ) {
                        member.RatingAction = NotificateStarSelected;
                        if( member.Status == CurrentUser.Status ) {
                            member.CanRate = false;
                        } else
                            member.CanRate = true;
                    }
                    _wasExecuted = true;
                }
            }
            return _event;
        }
        #endregion 
    }
}
