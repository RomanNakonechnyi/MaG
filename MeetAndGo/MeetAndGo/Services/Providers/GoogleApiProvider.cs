using MeetAndGo.Contracts;
using MeetAndGo.Controls.Models;
using MeetAndGo.Helpers;
using MeetAndGo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace MeetAndGo.Services {
    class GoogleApiProvider
    {
        private readonly GoogleApiService _googleApiService;

        public GoogleApiProvider() {
            _googleApiService = new GoogleApiService();
        }

        #region Public methods

        public async Task<IResponseData<Direction>> GetDirectionAsync ( Plugin.Geolocator.Abstractions.Position start, Plugin.Geolocator.Abstractions.Position destination, List<Plugin.Geolocator.Abstractions.Position> waypoints = null ) {
            var pointA = $"{start.Latitude.DoubleToString ()},{start.Longitude.DoubleToString ()}";
            var pointB = $"{destination.Latitude.DoubleToString ()},{destination.Longitude.DoubleToString ()}";
            List<string> points = new List<string> ();

            IResponseData<GoogleDirectionModels> result;

            if ( waypoints != null && waypoints.Count != 0 ) {
                waypoints.ForEach ( waypoint => points.Add ( $"{waypoint.Latitude.DoubleToString ()},{waypoint.Longitude.DoubleToString ()}" ) );

                result = await _googleApiService.GetDirectionAsync ( pointA, pointB, points );
            } else {
                result = await _googleApiService.GetDirectionAsync ( pointA, pointB );
            }

            if ( result.IsSuccess ) {
                if ( result.Data.routes.Count == 0 ) {
                    // TODO: as resource
                    return new ResponseData<Direction> ( null, false, "No routs found" );
                }

                var polylinePoints = DecodePolyline ( result.Data.routes[0].overview_polyline.points );
                var direction = new Direction ( polylinePoints );
                return new ResponseData<Direction> ( direction, true );
            }

            return new ResponseData<Direction> ( null, false, result.ErrorMessage );
        }


        #endregion

        #region Private methods

        /// <summary>
        /// This method will decode google polyline points
        /// </summary>
        /// <param name="encodedPoints"></param>
        /// <returns></returns>
        private List<Position> DecodePolyline(string encodedPoints) {
            if (string.IsNullOrWhiteSpace(encodedPoints)) {
                return null;
            }

            int index = 0;
            var polylineChars = encodedPoints.ToCharArray();
            var poly = new List<Position>();
            int currentLat = 0;
            int currentLng = 0;

            while (index < polylineChars.Length) {
                // calculate next latitude
                int sum = 0;
                int shifter = 0;

                int next5Bits;
                do {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length) {
                    break;
                }

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                // calculate next longitude
                sum = 0;
                shifter = 0;

                do {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5Bits >= 32) {
                    break;
                }

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                var mLatLng = new Position(Convert.ToDouble(currentLat) / 100000.0, Convert.ToDouble(currentLng) / 100000.0);
                poly.Add(mLatLng);
            }

            return poly;
        }

        #endregion

    }
}
