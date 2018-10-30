using MeetAndGo.Contracts;
using MeetAndGo.Controls.Models;
using MeetAndGo.Services;
using Plugin.Geolocator.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeetAndGo.Helpers {
    public class EventStatusMapHelper {
        private GoogleApiProvider _googleApiProvider;

        public EventStatusMapHelper () {
            _googleApiProvider = new GoogleApiProvider ();
        }

        public async Task<IResponseData<Direction>> GetLocations ( Location eventStart, Location eventEnd, Location userStart, Location userEnd, List<Location> eventWaypoints = null ) {
            Position startUserPosition = null;
            var startEventPosition = new Position ( eventStart.Latitude, eventStart.Longitude );
            var endEventPosition = new Position ( eventEnd.Latitude, eventEnd.Longitude );
            Position endUserPosition = null;

            var eventWaypointsPositions = new List<Position> ();

            if ( eventWaypoints != null && eventWaypoints.Count!=0 ) {
                eventWaypoints.ForEach ( waypoint => eventWaypointsPositions.Add ( new Position ( waypoint.Latitude, waypoint.Longitude ) ) );
            }

            List<Position> waypoints = new List<Position> () { startEventPosition, endEventPosition };
            waypoints.AddRange ( eventWaypointsPositions );

            if ( userStart == null || userEnd == null ) {
                return await GetDirectionBetweenTwoPositions ( startEventPosition, endEventPosition, null );
            }

            startUserPosition = new Position ( userStart.Latitude, userStart.Longitude );
            endUserPosition = new Position ( userEnd.Latitude, userEnd.Longitude );

            return await GetDirectionBetweenTwoPositions ( startEventPosition, endEventPosition, eventWaypointsPositions );
        }

        private async Task<IResponseData<Direction>> GetDirectionBetweenTwoPositions ( object startLocation, object endLocation, List<Position> waypoints ) {
            if ( ( startLocation is Plugin.Geolocator.Abstractions.Position start ) && ( endLocation is Plugin.Geolocator.Abstractions.Position end ) ) {
                return await _googleApiProvider.GetDirectionAsync ( start, end, waypoints );
            };

            return null;
        }
    }
}
