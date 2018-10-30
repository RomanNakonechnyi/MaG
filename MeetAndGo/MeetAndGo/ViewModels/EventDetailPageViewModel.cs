using Acr.UserDialogs;
using MeetAndGo.Constants;
using MeetAndGo.Controls;
using MeetAndGo.Controls.Models;
using MeetAndGo.Helpers;
using MeetAndGo.Models;
using MeetAndGo.Models.Enums;
using MeetAndGo.Services;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using Plugin.SecureStorage.Abstractions;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MeetAndGo.ViewModels {
    public class EventDetailPageViewModel : ViewModelBase
	{
        GoogleApiProvider _googleApiProvider = new GoogleApiProvider();
        private readonly ISecureStorage _secureStorage;
        private readonly ApiService _apiService;

        private EventModel _currentEvent;
        private Location _startLocation;
        private Location _endLocation;
        private string _startAddress;
        private string _endAddress;
        private bool _isAssignedToEvent;
        private List<Direction> _directions;
        private string _distance;
        private List<Direction> _userDirections;
        private ICommand _moveToCommand;

        public EventModel CurrentEvent {
            get {
                return _currentEvent;
            }
            set {
                SetProperty(ref _currentEvent, value);
            }

        }
        public Location StartLocation {
            get {
                return _startLocation;
            }
            set {
                SetProperty(ref _startLocation, value);
            }
        }
        public Location EndLocation {
            get {
                return _endLocation;
            }
            set {
                SetProperty(ref _endLocation, value);
            }
        }
        public string StartAddress {
            get {
                return _startAddress;
            }
            set {
                SetProperty(ref _startAddress, value);
            }
        }
        public string EndAddress {
            get {
                return _endAddress;
            }
            set {
                SetProperty(ref _endAddress, value);
            }
        }
        public bool IsAssignedToEvent {
            get {
                return _isAssignedToEvent;
            }
            set {
                SetProperty(ref _isAssignedToEvent, value);
            }
        }
        public string Distance {
            get {
                return _distance;
            }
            set {
                SetProperty(ref _distance, value);
            }
        }
        public ICommand BackCommand { private set; get; }
        public ICommand JoinCommand { private set; get; }
        public List<Direction> Directions {
            get => _directions;
            set => SetProperty(ref _directions, value);
        }
        public List<Direction> UserDirections {
            get => _userDirections;
            set => SetProperty(ref _userDirections, value);
        }

        public ICommand MoveToCommand {
            get => _moveToCommand;
            set => SetProperty(ref _moveToCommand, value);
        }
        public ICommand SelectedLocationCommand { get; set; }
        public EventDetailPageViewModel(INavigationService navigationService)
            : base(navigationService) {
            _secureStorage = CrossSecureStorage.Current;
            _apiService = new ApiService ();

            BackCommand = new Command(async () => await BackButton());
            JoinCommand = new Command(async () => await JoinButton());
        }

        public override void OnNavigatedFrom( INavigationParameters parameters ) {

        }

        public override async void OnNavigatingTo( INavigationParameters parameters ) {
            CurrentEvent = (EventModel)parameters[ConstantHelper.ParameterName];
            StartLocation = CurrentEvent.StartLocation;
            EndLocation = CurrentEvent.EndLocation;
            IsAssignedToEvent = false;
            await GetAddresses();
            await GetLocations();
        }
        
        private async Task GetDirectionBetweenTwoPositions(object startLocation, object endLocation, List<Plugin.Geolocator.Abstractions.Position> waypoints) {
            if ((startLocation is Plugin.Geolocator.Abstractions.Position start) && (endLocation is Plugin.Geolocator.Abstractions.Position end)) {
                var result = await _googleApiProvider.GetDirectionAsync(start, end, waypoints);
                if (result.IsSuccess) {
                    if (Directions == null) {
                        Directions = new List<Direction>() { result.Data };
                        return;
                    }
                    Directions.Add(result.Data);

                } 
            }
        }

        private async Task GetLocations() {
            var startUserLocation = (JsonConvert.DeserializeObject( _secureStorage.GetValue ( "tempStartLocation" ), typeof(Location)) as Location);
            var startUserPosition = new Plugin.Geolocator.Abstractions.Position ( startUserLocation.Latitude, startUserLocation.Longitude );
            var startEventPosition = new Plugin.Geolocator.Abstractions.Position(CurrentEvent.StartLocation.Latitude, CurrentEvent.StartLocation.Longitude);
            var endEventPosition = new Plugin.Geolocator.Abstractions.Position(CurrentEvent.EndLocation.Latitude, CurrentEvent.EndLocation.Longitude);
            var endUserLocation= ( JsonConvert.DeserializeObject ( _secureStorage.GetValue ( "tempEndLocation" ), typeof ( Location ) ) as Location );
            var endUserPosition = new Plugin.Geolocator.Abstractions.Position ( endUserLocation.Latitude, endUserLocation.Longitude );

            var Waypoints = new List<Waypoint>() { new Waypoint ( new Position ( CurrentEvent.StartLocation.Latitude, CurrentEvent.StartLocation.Longitude ) ),
                                                new Waypoint ( new Position ( CurrentEvent.EndLocation.Latitude, CurrentEvent.EndLocation.Longitude ) )};
            List<Plugin.Geolocator.Abstractions.Position> waypoints = new List<Plugin.Geolocator.Abstractions.Position>() { startEventPosition, endEventPosition };
            await GetDirectionBetweenTwoPositions(startUserPosition, endUserPosition,waypoints);
        }

        public async Task GetAddresses() {
            StartAddress = await Location.GetLocationName(CurrentEvent.Direction[0].Latitude, CurrentEvent.Direction[0].Longitude);
            EndAddress = await Location.GetLocationName(CurrentEvent.Direction[1].Latitude, CurrentEvent.Direction[1].Longitude);
        }
        public async Task BackButton() {
            var navigationParams = new NavigationParameters();
            navigationParams.Add(ConstantHelper.ParameterUserStartAddress, StartLocation);
            navigationParams.Add(ConstantHelper.ParameterUserEndAddress, EndLocation);
            await NavigationService.GoBackAsync(navigationParams);
        }
        public async Task JoinButton() {
            _secureStorage.SetValue ( "StartLocation", _secureStorage.GetValue("tempStartLocation") );
            _secureStorage.SetValue ( "EndLocation", _secureStorage.GetValue ( "tempEndLocation" ) );

            var result = await _apiService.JoinEventAsync ( CurrentEvent );

            _secureStorage.SetValue ( "eventId", result.Data.Id.ToString() );
            _secureStorage.SetValue ( "userStatus", UserStatus.Member.ToString() );

            await NavigationService.NavigateAsync("../../../EventStatusMemberPage");
        }
    }
}
