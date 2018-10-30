using System;
using System.Collections.Generic;
using System.Text;

namespace MeetAndGo.Models.Enums {
    [Flags]
    public enum UserStatus {
        User,
        Member,
        Organizer
    };
}

