using MeetAndGo.Controls;
using MeetAndGo.Models;
using MeetAndGo.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using MeetAndGo.Controls.Models;

namespace MeetAndGo.Helpers {
    class FakeDataHelper {
        public static UserModel User { get; } = new UserModel() {
            FirstName = "Elon",
            LastName = "Mask",
            Email = "elon_mask@gmail.com",
            CompressedPhoto = "user.png",
            Rating = 5,
            //CurrentLocation = GeolocationService.GetCurrentPosition().Result,
            Status = UserStatus.Organizer,
            LanguageCode = CultureInfo.CurrentCulture.ToString(),
            Votes = new List<VoteModel>()
        };

        public static ObservableCollection<UserModel> UserList { get; } = new ObservableCollection<UserModel>() {
            User,
            new UserModel(){FirstName="Petro", LastName="Rohachiv", Email="petro_rohachiv@mail.ru", CompressedPhoto="user.png", Rating=4,Status = UserStatus.Member,Votes=new List<VoteModel>()},
            new UserModel(){FirstName="Anastasia", Email="anastasiia999@gmail.com", CompressedPhoto="user.png", Rating=5,Status = UserStatus.Member,Votes=new List<VoteModel>()},
            new UserModel(){FirstName="Sofia",Email="lesia.222@gmail.com",CompressedPhoto="user.png",Rating=5,Status = UserStatus.Organizer,Votes=new List<VoteModel>()},
        };

        public static Location StartLocation = new Location() {
            Latitude = 49.837017,
            Longitude = 24.003027,
            Name = "вулиця Городоцька, 110"
        };
        public static Location EndLocation = new Location() {
            Name = "вулиця Словацького, 3",
            Latitude = 49.83826,
            Longitude = 24.02324
        };

        public static EventModel Event = new EventModel() {
            TotalPrice = 200,
            StartTime = new DateTimeOffset(new DateTime(2018, 8, 17, 13, 0, 0)),
            Members = new ObservableCollection<UserModel>(UserList),
            Direction = new List<Location>() { StartLocation, EndLocation },
            MaxSeats = 5
        };
        //public static EventModel Event2 = new EventModel () {
        //    StartLocation = new Location () { Name = "вулиця Городоцька, 105", Latitude = 49.83834, Longitude = 24.00805 },
        //    EndLocation = new Location () { Name = "вулиця Словацького, 3", Latitude = 49.83826, Longitude = 24.02324 },
        //    TotalPrice = 200,
        //    Distance = 10,
        //    StartTime = new DateTimeOffset ( new DateTime ( 2018, 8, 17, 13, 0, 0 ) ),
        //    MaxSeats = 6
        //};
    }
}
