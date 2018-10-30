using MeetAndGo.Models;
using MeetAndGo.Resources;

namespace MeetAndGo.Controls {
    class ValidateLocation : IValidatable<EventModel> {
        public string Message => ResourcesForValidation.InvalidLocation;

        public bool IsValid ( EventModel newEvent ) {
            if ( newEvent?.EndPoints == null ) {
                return false;
            }

            return true;
        }
    }
}
