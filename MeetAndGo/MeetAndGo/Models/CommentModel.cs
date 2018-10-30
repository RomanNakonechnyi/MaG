using System;
using System.Collections.Generic;
using System.Text;

namespace MeetAndGo.Models
{
    public class CommentModel
    {
        public UserModel Author { get; set; }
        public EventModel Event { get; set; }
        public DateTimeOffset CommentedIn { get; set; }
        public string Text { get; set; }

    }
}
