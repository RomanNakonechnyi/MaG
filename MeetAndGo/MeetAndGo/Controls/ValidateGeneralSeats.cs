using MeetAndGo.Models;
using MeetAndGo.Resources;

namespace MeetAndGo.Controls {
    class ValidateGeneralSeats : IValidatable<EventModel> {
        public string Message => ResourcesForValidation.InvalidGeneralSeats;

        public bool IsValid ( EventModel newEvent ) {
            if ( newEvent?.MaxSeats == 0 ) {
                return false;
            }

            return true;
        }
    }
}
