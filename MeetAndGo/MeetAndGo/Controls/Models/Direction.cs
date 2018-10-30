using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace MeetAndGo.Controls.Models
{
    public class Direction
    {
        public List<Position> Positions { get; set; }

        public Direction(List<Position> positions) {
            Positions = positions;
        }
    }
}
