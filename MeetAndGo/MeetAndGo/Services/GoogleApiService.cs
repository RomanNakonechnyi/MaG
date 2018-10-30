using MeetAndGo.Constants;
using MeetAndGo.Contracts;
using MeetAndGo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace MeetAndGo.Services {
    public class GoogleApiService : HttpBase {

        private readonly CancellationTokenSource _cancellationTokenSource;

        public GoogleApiService () {
            _cancellationTokenSource = new CancellationTokenSource ();
        }

        public async Task<IResponseData<GoogleDirectionModels>> GetDirectionAsync ( string start, string destination, List<string> waypoints = null ) {
            var refBuilder = new StringBuilder ();
            refBuilder.Append ( "https://maps.googleapis.com/maps/api/directions/json?origin=" );
            refBuilder.Append ( start );
            refBuilder.Append ( "&destination=" );
            refBuilder.Append ( destination );

            if ( waypoints != null ) {
                refBuilder.Append ( $"&waypoints={waypoints[0]}" );

                foreach ( var waypoint in waypoints ) {
                    refBuilder.Append ( $"|{waypoint}" );
                }
            }

            refBuilder.Append ( "&key=" );
            refBuilder.Append ( ConstantHelper.GoogleApiKey );
            var uri = new Uri ( refBuilder.ToString () );
            var jsonContent = await GetAsync ( uri );

            if ( jsonContent.IsSuccess ) {
                var googleDirection = JsonConvert.DeserializeObject<GoogleDirectionModels> ( jsonContent.Data );
                return new ResponseData<GoogleDirectionModels> ( googleDirection, true );
            }

            return new ResponseData<GoogleDirectionModels> ( null, false, jsonContent.ErrorMessage );
        }

        /// <summary>
        /// This method will decode google polyline points
        /// </summary>
        /// <param name="encodedPoints"></param>
        /// <returns></returns>
        private List<Position> DecodePolyline ( string encodedPoints ) {
            if ( string.IsNullOrWhiteSpace ( encodedPoints ) ) {
                return null;
            }

            int index = 0;
            var polylineChars = encodedPoints.ToCharArray ();
            var poly = new List<Position> ();
            int currentLat = 0;
            int currentLng = 0;

            while ( index < polylineChars.Length ) {
                // calculate next latitude
                int sum = 0;
                int shifter = 0;

                int next5Bits;
                do {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= ( next5Bits & 31 ) << shifter;
                    shifter += 5;
                }
                while ( next5Bits >= 32 && index < polylineChars.Length );

                if ( index >= polylineChars.Length ) {
                    break;
                }

                currentLat += ( sum & 1 ) == 1 ? ~( sum >> 1 ) : ( sum >> 1 );

                // calculate next longitude
                sum = 0;
                shifter = 0;

                do {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= ( next5Bits & 31 ) << shifter;
                    shifter += 5;
                }
                while ( next5Bits >= 32 && index < polylineChars.Length );

                if ( index >= polylineChars.Length && next5Bits >= 32 ) {
                    break;
                }

                currentLng += ( sum & 1 ) == 1 ? ~( sum >> 1 ) : ( sum >> 1 );

                var mLatLng = new Position ( Convert.ToDouble ( currentLat ) / 100000.0, Convert.ToDouble ( currentLng ) / 100000.0 );
                poly.Add ( mLatLng );
            }

            return poly;
        }

    }
}