using Acr.UserDialogs;
using DurianCode.PlacesSearchBar;
using MeetAndGo.Constants;
using MeetAndGo.Controls;
using MeetAndGo.Controls.Models;
using MeetAndGo.Helpers;
using MeetAndGo.Models;
using MeetAndGo.Models.Enums;
using MeetAndGo.Resources;
using MeetAndGo.Services;
using Plugin.SecureStorage;
using Plugin.SecureStorage.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetAndGo.ViewModels {
    public class NewEventPageViewModel : ViewModelBase {
        private readonly ISecureStorage _secureStorage;
        private readonly ApiService _apiService;

        private EventModel _event;
        private SearchBarModel _searchBarListView;
        private Location _intermediateLocation;
        private Location _startLocation;
        private Location _endLocation;

        public Location UtilityLocation { get; set; }

        public Location IntermediateLocation {
            get { return _intermediateLocation; }
            set { SetProperty ( ref _intermediateLocation, value ); }
        }

        public Location StartLocation {
            get { return _startLocation; }
            set { SetProperty ( ref _startLocation, value ); }
        }

        public Location EndLocation {
            get { return _endLocation; }
            set { SetProperty ( ref _endLocation, value ); }
        }

        public EventModel Event {
            get { return _event; }
            set { SetProperty ( ref _event, value ); }
        }

        public SearchBarModel SearchBarListView {
            get { return _searchBarListView; }
            set { SetProperty ( ref _searchBarListView, value ); }
        }

        public UserModel User { get; set; }
        public List<int> SeatsAmount => EventHelper.SeatsList;
        public List<string> Transports => EventHelper.TransportList;
        public DateTime MaxDate => DateTime.Now.AddDays ( EventConstantHelper.MaximumEventDayFromToday );

        public ICommand BackCommand => new DelegateCommand ( async () => await ExecuteBackCommand () );
        public ICommand CreateCommand => new DelegateCommand ( async () => await ExecuteCreateCommand () );
        public ICommand AddLocationCommand => new DelegateCommand ( () => ExecuteAddLocationCommand () );
        public ICommand StartSelectedItemCommand => new DelegateCommand ( async () => await ExecuteStartSelectedItemCommand () );
        public ICommand EndSelectedItemCommand => new DelegateCommand ( async () => await ExecuteEndSelectedItemCommand () );
        public ICommand IntermediateSelectedItemCommand => new DelegateCommand ( async () => await ExecuteIntermediateSelectedItemCommand () );
        public ICommand PropertyCommand => new DelegateCommand ( () => ExecutePropertyCommand () );

        public NewEventPageViewModel ( INavigationService navigationService )
                        : base ( navigationService ) {
            _apiService = new ApiService ();
            _secureStorage = CrossSecureStorage.Current;

            Event = new EventModel ();
            SearchBarListView = new SearchBarModel ();

            Event.CurrencyCode = CurrencyHelper.GetCurrencyCodeFromCountry ();
        }

        #region Command methods

        private async Task ExecuteBackCommand () {
            var result = await UserDialogs.Instance.ConfirmAsync ( ResourcesForUserDialogs.MessageForExitConfirmation,
                                                                   ResourcesForUserDialogs.AlertTitle,
                                                                   ResourcesForUserDialogs.OKButton,
                                                                   ResourcesForUserDialogs.CancelButton );

            if ( result == true ) {
                _secureStorage.DeleteKey ( "StartLocation" );
                _secureStorage.DeleteKey ( "EndLocation" );

                await NavigationService.GoBackAsync ();
            }
        }

        private async Task ExecuteCreateCommand () {
            NewEventValidation newEventValidation = new NewEventValidation ();

            if ( newEventValidation.IsValid ( Event ) ) {
                Event.Direction.Add ( EndLocation );
                Event.Direction.Insert ( 0, StartLocation );

                Event.EndPoints.Add ( StartLocation );
                Event.EndPoints.Add ( EndLocation );

                Event.Name = $"{StartLocation.Name} - {EndLocation.Name}";
                Event.State = EventState.Formation;

                //User.Status = UserStatus.Organizer;
                //Event.Author = ( await _apiService.GetUserAsync ( Int32.Parse ( _secureStorage.GetValue ( "userId" ) ) ) ).Data as UserModel;

                //_secureStorage.SetValue ( "userStatus", UserStatus.Organizer.ToString () );

                var createdEvent = ( await _apiService.CreateNewEventAsync ( Event ) ).Data as EventModel;
                await _apiService.AddDirectionPointsAsync ( createdEvent.Id, Event );
                createdEvent.CreatedTime = DateTime.Now;
                createdEvent.EventDate = DateTime.Now;
                await _apiService.JoinEventAsync ( createdEvent );

                _secureStorage.SetValue ( "eventId", createdEvent.Id.ToString () );

                await NavigationService.NavigateAsync ( "../../../EventStatusOrganizerPage" );
            } else {
                await UserDialogs.Instance.AlertAsync ( newEventValidation.Message,
                                                        ResourcesForUserDialogs.ErrorTitle,
                                                        ResourcesForUserDialogs.OKButton );
            }
        }

        private void ExecuteAddLocationCommand () {
            if ( IntermediateLocation == null ) {
                return;
            }

            if ( Event.Direction == null ) {
                Event.Direction = new List<Location> ();
            }

            if ( Event.Direction.Exists ( location => location.Name == IntermediateLocation.Name ) ) {
                UserDialogs.Instance.Toast ( ResourcesForUserDialogs.MessageForTheSameIntermediateLocation, null );

                return;
            }

            Event.Direction.Add ( UtilityLocation );

            UserDialogs.Instance.Toast ( ResourcesForUserDialogs.MessageForAddedIntemediateLocation, null );
        }

        private async Task ExecuteStartSelectedItemCommand () {
            var place = await GetPlaceAsync ( SearchBarListView?.StartSelectedItem?.Place_ID );

            StartLocation = new Location ();

            if ( place != null ) {
                SetLocation ( StartLocation, place );
            } else { return; }

            if ( EndLocation?.Name != null && StartLocation.Name == EndLocation.Name ) {
                await UserDialogs.Instance.AlertAsync ( ResourcesForUserDialogs.MessageForStartEndLocations,
                                                        ResourcesForUserDialogs.ErrorTitle,
                                                        ResourcesForUserDialogs.OKButton );
                return;
            }

            SearchBarListView.StartBarIsFocused = false;
            SearchBarListView.StartListIsVisible = false;
        }

        private async Task ExecuteEndSelectedItemCommand () {
            var place = await GetPlaceAsync ( SearchBarListView?.EndSelectedItem?.Place_ID );

            EndLocation = new Location ();

            if ( place != null ) {
                SetLocation ( EndLocation, place );
            } else { return; }

            if ( StartLocation?.Name != null && StartLocation.Name == EndLocation.Name ) {
                await UserDialogs.Instance.AlertAsync ( ResourcesForUserDialogs.MessageForStartEndLocations,
                                                        ResourcesForUserDialogs.ErrorTitle,
                                                        ResourcesForUserDialogs.OKButton );
                return;
            }

            SearchBarListView.EndBarIsFocused = false;
            SearchBarListView.EndListIsVisible = false;
        }

        private async Task ExecuteIntermediateSelectedItemCommand () {
            var place = await GetPlaceAsync ( SearchBarListView?.IntermediateSelectedItem?.Place_ID );

            UtilityLocation = new Location ();
            IntermediateLocation = new Location ();

            if ( place != null ) {
                SetLocation ( UtilityLocation, place );
                SetLocation ( IntermediateLocation, place );
            } else { return; }

            if ( EndLocation?.Name != null && EndLocation.Name == IntermediateLocation.Name ) {
                await UserDialogs.Instance.AlertAsync ( ResourcesForUserDialogs.MessageForEndIntermediateLocations,
                                                        ResourcesForUserDialogs.ErrorTitle,
                                                        ResourcesForUserDialogs.OKButton );
                return;
            }
            if ( StartLocation?.Name != null && StartLocation.Name == IntermediateLocation.Name ) {
                await UserDialogs.Instance.AlertAsync ( ResourcesForUserDialogs.MessageForStartIntermediateLocations,
                                                        ResourcesForUserDialogs.ErrorTitle,
                                                        ResourcesForUserDialogs.OKButton );
                return;
            }

            SearchBarListView.IntermediateBarIsFocused = false;
            SearchBarListView.IntermediateListIsVisible = false;
        }

        private void ExecutePropertyCommand () {
            SearchBarListView.StartListIsVisible = SearchBarListView.StartBarIsFocused;
            SearchBarListView.EndListIsVisible = SearchBarListView.EndBarIsFocused;
            SearchBarListView.IntermediateListIsVisible = SearchBarListView.IntermediateBarIsFocused;
        }

        #endregion Command methods

        #region Utility functions

        private void SetLocation ( Location location, Place place ) {
            location.Name = place.Name;
            location.Latitude = place.Latitude;
            location.Longitude = place.Longitude;
        }

        private async Task<Place> GetPlaceAsync ( string placeId ) {
            return await Places.GetPlace ( placeId, ConstantHelper.GoogleApiKey );
        }

        #endregion Utility functions
    }
}
