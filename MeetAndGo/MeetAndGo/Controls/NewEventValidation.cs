using MeetAndGo.Models;
using MeetAndGo.Resources;
using System.Collections.Generic;
using System.Linq;

namespace MeetAndGo.Controls {
    class NewEventValidation : IValidatable<EventModel> {
        public List<IValidatable<EventModel>> ValidationRules;

        public string Message { get; set; }

        public NewEventValidation () {
            ValidationRules = new List<IValidatable<EventModel>> {
                new ValidateCreatedTime() as IValidatable<EventModel>,
                new ValidateGeneralSeats() as IValidatable<EventModel>,
                new ValidateLocation() as IValidatable<EventModel>,
                new ValidatePrice() as IValidatable<EventModel>,
                new ValidateTransportType() as IValidatable<EventModel>,
                new ValidateDetails() as IValidatable<EventModel>
            };
        }

        public bool IsValid ( EventModel newEvent ) {
            if ( ValidationRules.FindAll ( p => ( p.IsValid ( newEvent ) == false ) ).Count != 1 ) {
                Message = ResourcesForValidation.InvalidMultipleFields;
            } else {
                Message = ValidationRules.Find ( p => ( p.IsValid ( newEvent ) == false ) ).Message;
            }

            return !ValidationRules.Any ( p => ( p.IsValid ( newEvent ) == false ) );
        }
    }
}