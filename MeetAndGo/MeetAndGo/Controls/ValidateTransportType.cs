using MeetAndGo.Models;
using MeetAndGo.Models.Enums;
using MeetAndGo.Resources;

namespace MeetAndGo.Controls {
    class ValidateTransportType : IValidatable<EventModel> {
        public string Message => ResourcesForValidation.InvalidTransportType;

        public bool IsValid ( EventModel newEvent ) {
            if ( newEvent?.Transport == null ) {
                return false;
            }

            return true;
        }
    }
}