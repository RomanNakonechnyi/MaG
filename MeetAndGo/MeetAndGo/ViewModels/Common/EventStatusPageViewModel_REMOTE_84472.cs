using Acr.UserDialogs;
using MeetAndGo.Controls.Models;
using MeetAndGo.Helpers;
using MeetAndGo.Models;
using MeetAndGo.Models.Enums;
using MeetAndGo.Services;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using Plugin.SecureStorage.Abstractions;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeetAndGo.ViewModels {
    public class EventStatusPage : ViewModelBase {
        protected readonly ISecureStorage _secureStorage;
        protected readonly ApiService _apiService;

        private List<Direction> _directions;
        private List<Waypoint> _waypoints;

        private EventModel _event;
        private ObservableCollection<UserModel> _userList;

        public ObservableCollection<UserModel> UserList {
            get => _userList;
            set => SetProperty ( ref _userList, value );
        }

        public EventModel Event {
            get => _event;
            set => SetProperty ( ref _event, value );
        }

        public List<Direction> Directions {
            get => _directions;
            set => SetProperty ( ref _directions, value );
        }
        public List<Waypoint> Waypoints {
            get => _waypoints;
            set => SetProperty ( ref _waypoints, value );
        }

        public EventStatusPage ( INavigationService navigationService )
            : base ( navigationService ) {
            _apiService = new ApiService ();
            _secureStorage = CrossSecureStorage.Current;

            //Event.State = EventState.Activated;
        }

        public async override void OnNavigatingTo ( INavigationParameters parameters ) {
            base.OnNavigatingTo ( parameters );

            using ( UserDialogs.Instance.Loading () ) {
                if ( _secureStorage.GetValue ( "eventId" ) == null ) {
                    await NavigationService.NavigateAsync ( "../SearchPage" );

                    return;
                }

                var eventId = Int32.Parse ( _secureStorage.GetValue ( "eventId" ) );

                Event = ( await _apiService.GetEventDetailsAsync ( eventId ) ).Data as EventModel;

                if ( Event == null || Event.Members.Count == 0 ) {
                    await NavigationService.NavigateAsync ( "../SearchPage" );

                    return;
                }

                UserList = Event.Members;

                foreach ( var user in UserList ) {
                    if ( user.CompressedPhoto == null ) {
                        user.HighQualityPhoto = "user";
                        user.CompressedPhoto = "user";
                    }
                }

                //_secureStorage.SetValue ( "EventState", Event.State.ToString() );

                await MapConfiguration ();
            }
        }

        private async Task MapConfiguration () {
            EventStatusMapHelper helper = new EventStatusMapHelper ();

            var waypointsList = new List<Location> ();
            if ( Event.Direction.Count > 2 ) {
                var list = Event.Direction.Skip ( 1 ).Take ( Event.Direction.Count - 2 ).ToList ();
                list.ForEach ( el => waypointsList.Add ( el ) );
            }

            Location userStart = null;
            Location userEnd = null;

            if ( _secureStorage.HasKey ( "StartLocation" ) && _secureStorage.HasKey ( "EndLocation" ) ) {
                userStart = JsonConvert.DeserializeObject ( _secureStorage.GetValue ( "StartLocation" ), typeof ( Location ) ) as Location;
                userEnd = JsonConvert.DeserializeObject ( _secureStorage.GetValue ( "EndLocation" ), typeof ( Location ) ) as Location;
            }

            var result = await helper.GetLocations ( Event.Direction?.FirstOrDefault (),
                                                     Event.Direction?.LastOrDefault (),
                                                     userStart,
                                                     userEnd,
                                                     waypointsList.Count != 0 ? waypointsList : null );

            if ( result?.IsSuccess == true ) {
                if ( Directions == null ) {
                    Directions = new List<Direction> () { result.Data };
                    Waypoints = new List<Waypoint> () { new Waypoint ( new Xamarin.Forms.GoogleMaps.Position ( Event.StartLocation.Latitude, Event.StartLocation.Longitude ) ),
                                                    new Waypoint ( new Xamarin.Forms.GoogleMaps.Position ( Event.EndLocation.Latitude, Event.EndLocation.Longitude ) )};
                    return;
                }
            } else {
                Directions = null;
                Waypoints = null;
            }
        }
    }
}
