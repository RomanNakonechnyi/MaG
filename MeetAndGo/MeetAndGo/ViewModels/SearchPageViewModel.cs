using Acr.UserDialogs;
using DurianCode.PlacesSearchBar;
using MeetAndGo.Constants;
using MeetAndGo.Controls.Models;
using MeetAndGo.Helpers;
using MeetAndGo.UserControls;
using Newtonsoft.Json;
using MeetAndGo.Models;
using Plugin.Geolocator;
using Plugin.SecureStorage;
using Plugin.SecureStorage.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.GoogleMaps;

namespace MeetAndGo.ViewModels {
    public class SearchPageViewModel : ViewModelBase {
        private readonly GeolocationService _geolocationService;
        private readonly ISecureStorage _secureStorage;

        private SearchBarModel _searchBarModel;
        private Location _startLocation = new Location();
        private Location _endLocation = new Location();
        private List<Direction> _directions;

        public SearchBarModel SearchBarModel {
            get { return _searchBarModel; }
            set { SetProperty(ref _searchBarModel, value); }
        }


        private ICommand _moveToCommand;

        public List<Direction> Directions {
            get => _directions;
            set => SetProperty(ref _directions, value);
        }

        public Location StartLocation {
            get => _startLocation;
            set => SetProperty(ref _startLocation, value);
        }

        public Location EndLocation {
            get => _endLocation;
            set => SetProperty(ref _endLocation, value);
        }

        public ICommand MoveToCommand {
            get => _moveToCommand;
            set => SetProperty(ref _moveToCommand, value);
        }

        public ICommand SearchCommand => new DelegateCommand(async () => await SearchButtonClicked());
        public DelegateCommand ReturnCommand => new DelegateCommand(async () => await DrawDirection());
        public DelegateCommand StartSelectedItemCommand => new DelegateCommand(async () => await ExecuteStartSelectedItemCommand());
        public DelegateCommand EndSelectedItemCommand => new DelegateCommand(async () => await ExecuteEndSelectedItemCommand());
        public DelegateCommand PropertyCommand => new DelegateCommand(ExecutePropertyCommand);

        public SearchPageViewModel( INavigationService navigationService, IPageDialogService pageDialogService )
            : base(navigationService) {
            _geolocationService = new GeolocationService();
            _secureStorage = CrossSecureStorage.Current;

            SearchBarModel = new SearchBarModel();
        }

        private void ExecutePropertyCommand() {
            SearchBarModel.StartListIsVisible = SearchBarModel.StartLocationLoading = SearchBarModel.StartBarIsFocused;
            SearchBarModel.EndListIsVisible = SearchBarModel.EndLocationLoading = SearchBarModel.EndBarIsFocused;
        }

        private async Task ExecuteEndSelectedItemCommand() {
            if( StartLocation == null ) {
                return;
            }

            var place = await Places.GetPlace(SearchBarModel.EndSelectedItem.Place_ID, ConstantHelper.GoogleApiKey);
            SetLocation(EndLocation, place);
            if( StartLocation?.Name == EndLocation?.Name ) {
                await UserDialogs.Instance.AlertAsync(ConstantHelper.MessageForIdenticalAddresses);
                return;
            }


            SearchBarModel.EndBarIsFocused = false;
            SearchBarModel.EndListIsVisible = false;

            await DrawDirection ();
        }

        private async Task ExecuteStartSelectedItemCommand () {
            var place = await Places.GetPlace ( SearchBarModel.StartSelectedItem.Place_ID, ConstantHelper.GoogleApiKey );
            SetLocation ( StartLocation, place );

            if ( EndLocation?.Name != null && EndLocation.Name != StartLocation.Name ) {
                await DrawDirection ();
            }

            SearchBarModel.StartBarIsFocused = false;
            SearchBarModel.StartListIsVisible = false;

            MoveToCommand?.Execute ( ( new Position ( StartLocation.Latitude, StartLocation.Longitude ) ) );
        }

        private async Task Initialization () {
            var position = await GeolocationService.GetCurrentPosition ();
            CurrentLocation.CurrentPosition = position;

            if ( position != null ) {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                if( locator != null ) {
                    var addresses = await locator.GetAddressesForPositionAsync(position);
                    var address = addresses?.FirstOrDefault();
                    if( address != null ) {
                        StartLocation = new Location {
                            Name = String.Format("{0}, {1}", address.Thoroughfare, address.Locality),
                            Longitude = address.Longitude,
                            Latitude = address.Latitude
                        };
                    }
                    EndLocation = new Location();
                }
                MoveToCommand?.Execute(new Position(position.Latitude, position.Longitude));
                return;
            }
            StartLocation = new Location();
            EndLocation = new Location();

        }

        private async Task DrawDirection() {
            if( String.IsNullOrWhiteSpace(StartLocation?.Name) || String.IsNullOrWhiteSpace(EndLocation?.Name) ) {
                await UserDialogs.Instance.AlertAsync(ConstantHelper.MessageForEmptyError, "Error", "OK");
                return;
            }

            var startPositon = new Plugin.Geolocator.Abstractions.Position(StartLocation.Latitude, StartLocation.Longitude);
            var endPosition = new Plugin.Geolocator.Abstractions.Position(EndLocation.Latitude, EndLocation.Longitude);
            Directions = await _geolocationService.GetDirectionBetweenTwoPositions(startPositon, endPosition);
            MoveToCommand?.Execute(endPosition);
        }

        private async Task SearchButtonClicked () {
            EventModel._startUserAddress = StartLocation.Name;
            NavigationParameters navigationParams = new NavigationParameters();
            if ( String.IsNullOrWhiteSpace ( StartLocation?.Name ) || String.IsNullOrWhiteSpace ( EndLocation?.Name ) ) {
                await UserDialogs.Instance.AlertAsync ( ConstantHelper.MessageForSearchButtonError, "Error", "OK" );
                return;
            }

            _secureStorage.SetValue ( "tempStartLocation", JsonConvert.SerializeObject ( StartLocation ) );
            _secureStorage.SetValue ( "tempEndLocation", JsonConvert.SerializeObject ( EndLocation ) );

            navigationParams.Add(ConstantHelper.ParameterUserStartAddress, StartLocation);
            navigationParams.Add(ConstantHelper.ParameterUserEndAddress, EndLocation);

            await NavigationService.NavigateAsync ( "EventListPage",navigationParams );
        }

        private void SetLocation( Location location, Place place ) {
            if( location == null || place == null ) {
                return;
            }
            location.Name = place.Name;
            location.Latitude = place.Latitude;
            location.Longitude = place.Longitude;
        }

        public override async void OnNavigatingTo( INavigationParameters parameters ) {
            base.OnNavigatingTo(parameters);
            await Initialization();
        }

        //internal void UpdateLocationsList(List<AutoCompletePrediction> locations)
        //{
        //    Locations = locations;
        //}
    }
}
