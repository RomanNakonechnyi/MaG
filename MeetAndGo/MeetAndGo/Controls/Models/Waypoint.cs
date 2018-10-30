using System;
using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps;

namespace MeetAndGo.Controls.Models
{
    public class Waypoint
    {
        public Position Position { get; set; }

        public Waypoint ( Position positions ) {
            Position = positions;
        }
    }
}
