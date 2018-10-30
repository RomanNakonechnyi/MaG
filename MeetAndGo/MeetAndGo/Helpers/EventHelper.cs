using MeetAndGo.Constants;
using MeetAndGo.Extensions;
using MeetAndGo.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeetAndGo.Helpers {
    public class EventHelper {
        public static List<string> TransportList => Enum.GetNames ( typeof ( Transport ) ).Select ( b => b.SplitCamelCase () ).ToList ();

        public static List<int> SeatsList {
            get {
                List<int> seats = new List<int> ();

                for ( int i = 1; i <= EventConstantHelper.MaximumAmountOfSeats; i++ ) {
                    seats.Add ( i );
                }

                return seats;
            }
        }
    }
}
