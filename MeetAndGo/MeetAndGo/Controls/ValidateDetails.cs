using MeetAndGo.Models;
using MeetAndGo.Resources;

namespace MeetAndGo.Controls {
    class ValidateDetails : IValidatable<EventModel> {
        public string Message => ResourcesForValidation.InvalidDetails;

        public bool IsValid ( EventModel newEvent ) {
            if ( newEvent?.Details == null ) {
                return false;
            }

            return true;
        }
    }
}