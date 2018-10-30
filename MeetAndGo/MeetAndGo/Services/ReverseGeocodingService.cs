using MeetAndGo.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace MeetAndGo.Services {
    class ReverseGeocodingService
    {
        public ReverseGeocodingModels GetGeographicInfo ( double latitude, double longitude, bool usesSensor ) {
            String strUrl = String.Empty;

            strUrl = "http://maps.googleapis.com/maps/api/geocode/json?latlng=" + latitude + "," + longitude;
            strUrl += usesSensor ? "&sensor=true" : "&sensor=false";

            HttpWebRequest request = ( HttpWebRequest )WebRequest.Create ( strUrl );

            request.ServicePoint.Expect100Continue = false;

            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";

            WebResponse response = request.GetResponse ();

            string strResponse = String.Empty;

            using ( var sr = new StreamReader ( response.GetResponseStream () ) ) {
                strResponse = sr.ReadToEnd ();

                return JsonConvert.DeserializeObject<ReverseGeocodingModels> ( strResponse );
            }
        }
    }
}
