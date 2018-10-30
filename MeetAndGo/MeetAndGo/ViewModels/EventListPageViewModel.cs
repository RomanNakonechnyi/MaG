using Acr.UserDialogs;
using MeetAndGo.Constants;
using MeetAndGo.Helpers;
using MeetAndGo.Models;
using MeetAndGo.Services;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MeetAndGo.Controls.Models;
using Plugin.SecureStorage.Abstractions;
using Plugin.SecureStorage;
using Newtonsoft.Json;
using MeetAndGo.Views;

namespace MeetAndGo.ViewModels {
    public class EventListPageViewModel : ViewModelBase {
        private readonly ISecureStorage _secureStorage;

        private List<EventModel> _eventList;
        private EventModel _selectedEvent;
        private Location _startlocation;
        private Location _endLocation;


        public Location StartLocation {
            get {
                return _startlocation;
            }
            set {
                SetProperty ( ref _startlocation, value );
            }
        }

        public Location EndLocation {
            get {
                return _endLocation;
            }
            set {
                SetProperty ( ref _endLocation, value );
            }
        }

        public List<EventModel> EventList {
            get {
                return _eventList;
            }
            set {
                SetProperty ( ref _eventList, value );
            }
        }
        public EventModel SelectedEvent {
            get {
                return _selectedEvent;
            }
            set {
                SetProperty ( ref _selectedEvent, value );
                if ( _selectedEvent != null ) {
                    GoToEventDetailPage ();
                }
            }
        }

        public ICommand BackCommand { private set; get; }
        public ICommand CreateCommand { private set; get; }

        public EventListPageViewModel ( INavigationService navigationService )
             : base ( navigationService ) {
            _secureStorage = CrossSecureStorage.Current;

            BackCommand = new Command ( async () => await BackButton () );
            CreateCommand = new Command ( async () => await CreateButton () );
        }


        public async Task BackButton () {
            await NavigationService.GoBackAsync ();
        }
        public async Task CreateButton()
        {
            _secureStorage.SetValue("StartLocation", _secureStorage.GetValue("tempStartLocation"));
            _secureStorage.SetValue("EndLocation", _secureStorage.GetValue("tempEndLocation"));

            await NavigationService.NavigateAsync("NewEventPage");
        }

        public async void GoToEventDetailPage () {
            var navigationParams = new NavigationParameters ();
            navigationParams.Add ( ConstantHelper.ParameterName, SelectedEvent );
            navigationParams.Add ( ConstantHelper.ParameterStartAddress, StartLocation );
            navigationParams.Add ( ConstantHelper.ParameterEndAddress, EndLocation );
            await NavigationService.NavigateAsync ( "EventDetailPage", navigationParams );
        }
        public async override void OnNavigatedFrom ( INavigationParameters parameters ) {
            SelectedEvent = null;
        }

        public override void OnNavigatingTo ( INavigationParameters parameters ) {
            StartLocation = JsonConvert.DeserializeObject ( _secureStorage.GetValue ( "tempStartLocation" ), typeof ( Location ) ) as Location;
            EndLocation = JsonConvert.DeserializeObject ( _secureStorage.GetValue ( "tempEndLocation" ), typeof ( Location ) ) as Location;
        }

        public override async void OnNavigatedTo ( INavigationParameters parameters ) {
            ApiService apiService = new ApiService ();
            
            using ( UserDialogs.Instance.Loading () ) {
                EventList = ( await apiService.GetEventListAsync ( StartLocation, EndLocation ) ).Data as List<EventModel>;

                for ( int i = 0; i < EventList?.Count; i++ ) {
                    EventList[i]?.SetProperties ();
                }
            }

            if( EventList?.Count == 0 ) {
                await UserDialogs.Instance.AlertAsync ( "No events were found." );
            }
        }
    }
}
