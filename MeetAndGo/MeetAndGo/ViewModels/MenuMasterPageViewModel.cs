using Acr.UserDialogs;
using MeetAndGo.Contracts;
using MeetAndGo.Helpers;
using MeetAndGo.Models;
using MeetAndGo.Models.Enums;
using MeetAndGo.Services;
using Plugin.SecureStorage;
using Plugin.SecureStorage.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetAndGo.ViewModels {
    public class MenuMasterPageViewModel : ViewModelBase {
        private readonly ISecureStorage _secureStorage;
        private readonly ApiService _apiService;

        private UserModel _user;
        private bool _eventStatusIsEnabled;

        public UserModel User {
            get => _user;
            set => SetProperty ( ref _user, value );
        }

        public bool EventStatusIsEnabled {
            get => _eventStatusIsEnabled;
            set => SetProperty ( ref _eventStatusIsEnabled, value );
        }

        public ICommand NavigateCommand => new DelegateCommand<string> ( NavigateOnPage );
        public ICommand ProfileCommand => new DelegateCommand<NavigationParameters> ( NavigateOnProfile );

        public MenuMasterPageViewModel ( INavigationService navigationService )
            : base ( navigationService ) {
            _secureStorage = CrossSecureStorage.Current;
            _apiService = new ApiService ();

            EventStatusIsEnabled = true;
        }
        public async override void OnNavigatingTo ( INavigationParameters parameters ) {
            var response = ( await _apiService.SignInAsync () ).Data as SignInModel;

            User = response?.User;

            _secureStorage.SetValue ( "userId", User.Id.ToString() );

            if ( User == null ) {

                return;
            }

            _secureStorage.SetValue ( "token", response?.Key );

            if ( User?.CompressedPhoto == null ) {
                User.HighQualityPhoto = "user";
                User.CompressedPhoto = "user";
            }

            if ( User?.Status == UserStatus.User ) {
                EventStatusIsEnabled = false;
            }

            base.OnNavigatingTo ( parameters );
        }

        public async Task CheckUserStatus () {
            User.Status = ( await _apiService.GetUserStatusAsync ( User ) ).Data;
        }

        private async void NavigateOnProfile ( NavigationParameters navigationParameters ) {
            var navigationParams = new NavigationParameters ();
            navigationParams.Add ( "User", User );

            await NavigationService.NavigateAsync ( "NavigationPage/ProfilePage", navigationParams );
        }

        private void NavigateOnPage ( string page ) {
            if ( page == null ) {
                switch ( User.Status ) {
                    case UserStatus.Member:
                        NavigationService.NavigateAsync ( "NavigationPage/EventStatusMemberPage" );
                        break;
                    case UserStatus.Organizer:
                        NavigationService.NavigateAsync ( "NavigationPage/EventStatusOrganizerPage" );
                        break;
                }

                return;
            }

            NavigationService.NavigateAsync ( page );
        }
    }
}
