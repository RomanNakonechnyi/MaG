using MeetAndGo.Models;
using MeetAndGo.Resources;

namespace MeetAndGo.Controls {
    class ValidatePrice : IValidatable<EventModel> {
        public string Message => ResourcesForValidation.InvalidPrice;

        public bool IsValid ( EventModel newEvent ) {
            if ( newEvent?.TotalPrice == 0 ) {
                return false;
            }

            return true;
        }
    }
}
