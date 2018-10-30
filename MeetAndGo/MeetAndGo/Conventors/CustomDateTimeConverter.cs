using Newtonsoft.Json.Converters;

namespace MeetAndGo.Conventors {
    public class CustomDateTimeConverter : IsoDateTimeConverter {
        public CustomDateTimeConverter () {
            base.DateTimeFormat = "yyyy-MM-ddTH:mm:ssZ";
        }
    }
}
