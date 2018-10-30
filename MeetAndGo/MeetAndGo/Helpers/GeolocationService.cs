using MeetAndGo.Controls.Models;
using MeetAndGo.Services;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MeetAndGo.Helpers {
    class GeolocationService {
        private readonly GoogleApiProvider _googleApiProvider;
        public GeolocationService () {
            _googleApiProvider = new GoogleApiProvider ();
        }
        public static async Task<Plugin.Geolocator.Abstractions.Position> GetCurrentPosition () {
            Plugin.Geolocator.Abstractions.Position position = null;
            try {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetLastKnownLocationAsync ();
                if ( position != null ) {
                    //got a cached position, so let's use it.
                    return position;
                }

                if ( !locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled ) {
                    //not available or enabled
                    return null;
                }

                position = await locator.GetPositionAsync ( TimeSpan.FromSeconds ( 20 ), null, true );

            } catch ( Exception ex ) {
                Debug.WriteLine ( "Unable to get location: " + ex );
            }

            if ( position == null )
                return null;

            var output = string.Format (
                "Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                position.Timestamp, position.Latitude, position.Longitude,
                position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed );

            Debug.WriteLine ( output );

            return position;
        }

        public static async Task<Plugin.Geolocator.Abstractions.Position> GetPosition ( string address ) {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 100;

            var possibleAddresses = await locator.GetPositionsForAddressAsync ( address );
            if ( possibleAddresses.FirstOrDefault () == null ) {
                return null;
            }
            return new Plugin.Geolocator.Abstractions.Position ( possibleAddresses.FirstOrDefault ().Latitude, possibleAddresses.FirstOrDefault ().Longitude );
        }

        public async Task<List<Direction>> GetDirectionBetweenTwoPositions ( object startLocation, object endLocation ) {
            List<Direction> directions = null;
            if ( ( startLocation is Plugin.Geolocator.Abstractions.Position start ) && ( endLocation is Plugin.Geolocator.Abstractions.Position end ) ) {
                var result = await _googleApiProvider.GetDirectionAsync ( start, end );

                if ( result.IsSuccess ) {
                    directions = new List<Direction> { result.Data };
                }
            }
            return directions;
        }
    }
}