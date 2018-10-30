using MeetAndGo.Controls.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetAndGo.Models {
    public class ReverseGeocodingModels {
        public string status { get; set; }
        public List<Results> results { get; set; }
    }

    public class Results {
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public List<string> types { get; set; }
        public List<Address_component> address_components { get; set; }
    }

    public class Geometry {
        public string location_type { get; set; }
        public Location location { get; set; }
    }

    public class Address_component {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

}
