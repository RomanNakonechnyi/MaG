using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MeetAndGo.Services
{
    public class GeoLocationService {
        public async Task<string> GetAddress(double x, double y) {
            var locator = CrossGeolocator.Current;
            var position = new Position(x, y);
            var addresses = await locator.GetAddressesForPositionAsync(position);
            var address = addresses.FirstOrDefault();
            if (address == null) {
                return String.Empty;
            } else {
                return String.Format("{0}, {1}", address.Thoroughfare, address.FeatureName);
            }

        }
        public string Address(double x, double y) {
            Task<string> task = Task.Run(async () => await GetAddress(x, y));
            task.Wait();
            return task.Result;
        }
        public static double GetDistance(Plugin.Geolocator.Abstractions.Position pos1, Plugin.Geolocator.Abstractions.Position pos2, Xamarin.Forms.GoogleMaps.DistanceType type) {
            double R = (type == Xamarin.Forms.GoogleMaps.DistanceType.Miles) ? 3960 : 6371;
            double dLat = ToRadian(pos2.Latitude - pos1.Latitude);
            double dLon = ToRadian(pos2.Longitude - pos1.Longitude);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadian(pos1.Latitude)) * Math.Cos(ToRadian(pos2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
            return d;
        }
        private static double ToRadian(double val) {
            return (Math.PI / 180) * val;
        }
    }
}

