using MeetAndGo.Models;
using MeetAndGo.Resources;

namespace MeetAndGo.Controls {
    class ValidateCreatedTime : IValidatable<EventModel> {
        public string Message => ResourcesForValidation.InvalidCreatedTime;

        public bool IsValid ( EventModel newEvent ) {
            if ( newEvent?.CreatedTime == null ) {
                return false;
            }

            return true;
        }
    }
}