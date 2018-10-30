using Acr.UserDialogs;
using MeetAndGo.Constants;
using MeetAndGo.Controls.Models;
using MeetAndGo.Conventors;
using MeetAndGo.Helpers;
using MeetAndGo.Models.Enums;
using MeetAndGo.Services;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MeetAndGo.Models {
    public class EventModel : BindableBase {
        private GeoLocationService _geoLocationService;
        private UserModel _author;
        private int _id;
        private string _name;
        private ObservableCollection<UserModel> _members = new ObservableCollection<UserModel> ();
        private int _maxSeats;
        private decimal _totalPrice;
        private string _currencyCode;
        private DateTimeOffset _startTime;
        private DateTime _createdTime = _eventDate.Date.Add ( _eventTime != null ? _eventTime : _eventDate.TimeOfDay ); // + offset
        private decimal _expectedRating;
        private string _details;
        private List<CommentModel> _comments;
        private double _distance;
        private Location _startLocation;
        private string _startAddress;
        private string _endAddress;
        private Location _endLocation;
        private Transport _transport;
        private EventState _state;
        private List<Location> _direction = new List<Location> ();
        private List<Location> _endPoints = new List<Location> ();
        private static DateTime _eventDate;
        private static TimeSpan _eventTime;
        public static string _startUserAddress;

        [JsonProperty ( "pk" )]
        public int Id {
            get {
                return _id;
            }
            set {
                SetProperty ( ref _id, value );
            }
        }

        [JsonProperty ( "name" )]
        public string Name {
            get {
                return _name;
            }
            set {
                SetProperty ( ref _name, value );
            }
        }

        [JsonProperty ( "state" )]
        public EventState State {
            get {
                return _state;
            }
            set {
                SetProperty ( ref _state, value );
            }
        }

        [JsonProperty ( "members" )]
        public ObservableCollection<UserModel> Members {
            get {
                return _members;
            }
            set {
                SetProperty ( ref _members, value );
            }
        }

        [JsonProperty ( "seats" )]
        public int MaxSeats {
            get {
                return _maxSeats;
            }
            set {
                SetProperty ( ref _maxSeats, value );
            }
        }

        [JsonProperty ( "total_price" )]
        public decimal TotalPrice {
            get {
                return _totalPrice;
            }
            set {
                SetProperty ( ref _totalPrice, value );
            }
        }
        
        public string CurrencyCode {
            get {
                return _currencyCode;
            }
            set {
                SetProperty ( ref _currencyCode, value );
            }
        }

        [JsonProperty ( "start_time" )]
        public DateTime CreatedTime {
            get {
                //if ( Members.Count == 0)
                    //return _eventDate.Date.Add ( _eventTime != null ? _eventTime : _eventDate.TimeOfDay );
                return DateTime.Now.AddHours(2);
            }
            set { 
                Console.WriteLine(value);
                SetProperty ( ref _createdTime, value ); 
            }
        }

        [JsonIgnore]
        public decimal ExpectedRating {
            get {
                return _expectedRating;
            }
            set {
                SetProperty ( ref _expectedRating, value );
            }
        }

        [JsonProperty ( "details" )]
        public string Details {
            get {
                return _details;
            }
            set {
                SetProperty ( ref _details, value );
            }
        }

        [JsonIgnore]
        public List<Location> EndPoints {
            get { return _endPoints; }
            set { SetProperty ( ref _endPoints, value ); }
        }

        [JsonProperty ( "points" )]
        public List<Location> Direction {
            get {
                return _direction;
            }
            set {
                SetProperty ( ref _direction, value );
            }
        }

        [JsonIgnore]
        public List<CommentModel> Comments {
            get {
                return _comments;
            }
            set {
                SetProperty ( ref _comments, value );
            }
        }

        [JsonIgnore]
        public DateTimeOffset StartTime {
            get {
                return _startTime;
            }
            set {
                SetProperty ( ref _startTime, value );
            }
        }

        [JsonIgnore]
        public DateTime EventDate {
            get {
                return _eventDate;
            }
            set {
                SetProperty ( ref _eventDate, value );
            }
        }

        public TimeSpan EventTime {
            get {
                return _eventTime;
            }
            set {
                SetProperty ( ref _eventTime, value );
            }
        }

        public double Distance {
            get {
                return _distance;
            }
            set {
                SetProperty ( ref _distance, value );
            }
        }

        [JsonIgnore]
        public Location StartLocation {
            get {
                return Direction?.FirstOrDefault ();
            }
        }

        [JsonIgnore]
        public Location EndLocation {
            get {
                return Direction?.LastOrDefault ();
            }
        }

        public EventModel () {
            _geoLocationService = new GeoLocationService ();
        }

        [JsonIgnore]
        public string CurrentNumberOfMembers {
            get {
                if ( Members != null ) {
                    return String.Format ( "{0}/{1}", Members.Count, MaxSeats );
                } else {
                    return String.Format ( "0/{0}", MaxSeats );
                }
            }
        }

        [JsonIgnore]
        public string Price {
            get {
                if ( Members != null ) {
                    return String.Format ( "{0} {1}", ( double )( TotalPrice / ( Members.Count + 1 ) ), CurrencyCode );
                } else {
                    return "";
                }
            }
        }
        [JsonIgnore]
        public UserModel Organiser {
            get {
                return Members.SingleOrDefault ( user => user.Status == UserStatus.Organizer );
            }
        }

        [JsonIgnore]
        public string Time {
            get {
                return CreatedTime.ToString ( "hh:mm tt" );
            }
        }

        [JsonIgnore]
        public string RemainingTime {
            get {
                return "1";
            }
        }

        [JsonIgnore]
        public string StartAddress {
            get {
                return _startAddress;
                // return "";
            }
            set {
                SetProperty ( ref _startAddress, value );
            }
        }

        [JsonIgnore]
        public string EndAddress {
            get {
                return _endAddress;
            }
            set {
                SetProperty ( ref _endAddress, value );
            }
        }

        [JsonIgnore]
        public Transport Transport {
            get {
                return _transport;
            }
            set {
                SetProperty ( ref _transport, value );
            }
        }

        public async Task<double> GetDistance () {
            var startPosition = new Position ( Direction.FirstOrDefault ().Latitude, Direction.FirstOrDefault ().Longitude );
            var endPosition = new Position ( Direction.LastOrDefault ().Latitude, Direction.LastOrDefault ().Longitude );
            return GeoLocationService.GetDistance ( startPosition, endPosition, Xamarin.Forms.GoogleMaps.DistanceType.Kilometers );
        }

        [JsonIgnore]
        public string StartUserAddress {
            get {
                return _startUserAddress;
            }
            set {
                SetProperty ( ref _startUserAddress, value );
            }
        }

        public async void SetProperties () {
            Distance = await GetDistance ();
            StartAddress = Direction.FirstOrDefault ().Name;
            EndAddress = Direction.LastOrDefault ().Name;

        }

    }
}
