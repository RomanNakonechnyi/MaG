using Newtonsoft.Json;
using Prism.Mvvm;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace MeetAndGo.Controls.Models {
    public class Location : BindableBase {
        private string _name;
        private double _latitude;
        private double _longitude;

        [JsonProperty ( "start_point" )]
        public string Name {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        [JsonProperty ( "latitude" )]
        public double Latitude {
            get { return _latitude; }
            set { SetProperty ( ref _latitude, value ); }
        }

        [JsonProperty ( "longitude" )]
        public double Longitude {
            get { return _longitude; }
            set { SetProperty ( ref _longitude, value ); }
        }
        public static async Task<string> GetLocationName(double latitude,double longitude) {
            Position position = new Position(latitude, longitude);
            if (position != null) {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                if (locator != null) {
                    var addresses = await locator.GetAddressesForPositionAsync(position);
                    var address = addresses?.FirstOrDefault();
                    if (address != null) {
                        return string.Format("{0}, {1}", address.Thoroughfare, address.FeatureName);
                    }
                }
            }
            return string.Empty;
        }
    }
}