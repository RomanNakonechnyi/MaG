using MeetAndGo.Models.Enums;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace MeetAndGo.Models {
    public class UserModel : BindableBase {
        private int _id;
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _dateOfBirth;
        private string _phoneNumber;
        private UserStatus _status;
        private decimal _rating;
        private string _languageCode;
        private string _compressedPhoto;
        private string _highQualityPhoto;
        private Position _currentLocation;
        private List<VoteModel> _votes;
        private int _tempRate;

        [JsonIgnore]
        public Action<UserModel> RatingAction;
        public event EventHandler RatingEvent;
        private bool _canRate;
        [JsonIgnore]
        public int TempRate {
            get { return _tempRate; }
            set {

                SetProperty(ref _tempRate, value);
                RatingAction?.Invoke(this);
                RatingEvent?.Invoke(this, null);
            }
        }
        [JsonIgnore]
        public bool CanRate {
            get { return _canRate; }
            set { SetProperty(ref _canRate, value); }
        }

        [JsonProperty ( "pk" )]
        public int Id {
            get { return _id; }
            set { SetProperty ( ref _id, value ); }
        }

        [JsonProperty ( "first_name" )]
        public string FirstName {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        [JsonProperty ( "last_name" )]
        public string LastName {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        [JsonProperty ( "email" )]
        public string Email {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        [JsonProperty ( "date_of_birth" )]
        public DateTime DateOfBirth {
            get {
                return _dateOfBirth;
            }
            set {
                SetProperty ( ref _dateOfBirth, value );
            }
        }

        [JsonProperty ( "phone_number" )]
        public string PhoneNumber {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }

        [JsonProperty ( "status" )]
        public UserStatus Status {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public string FullName {
            get { return string.Format($"{FirstName} {LastName}"); }
        }

        [JsonProperty ( "rating" )]
        public decimal Rating {
            get {
                return _rating;
            }
            set {
                SetProperty ( ref _rating, value );
            }
        }

        [JsonProperty ( "language_code" )]
        public string LanguageCode {
            get {
                return _languageCode;
            }
            set {
                SetProperty ( ref _languageCode, value );
            }
        }
        
        public string CompressedPhoto {
            get {
                return _compressedPhoto;
            }
            set {
                SetProperty ( ref _compressedPhoto, value );
            }
        }
        
        public string HighQualityPhoto {
            get {
                return _highQualityPhoto;
            }
            set {
                SetProperty ( ref _highQualityPhoto, value );
            }
        }

        [JsonIgnore]
        public Position CurrentLocation {
            get {
                return _currentLocation;
            }
            set {
                SetProperty ( ref _currentLocation, value );
            }
        }
        [JsonIgnore]
        public int Age {
            get { return GetAge(DateOfBirth); }
        }

        private int GetAge( DateTime dateOfBirth ) {
            return (int)( DateTime.Today - dateOfBirth ).TotalDays / 365;
        }
        [JsonIgnore]
        public List<VoteModel> Votes {
            get { return _votes; }
            set { SetProperty(ref _votes, value); }

        }
    }
}
