using Prism.Mvvm;

namespace MeetAndGo.Models
{
    public class VoteModel : BindableBase
    {
        private UserModel _voter;
        private UserModel _candidate;
        private int _rating;
        private string _comment;

        public UserModel Voter
        {
            get { return _voter; }
            set { SetProperty(ref _voter, value); }
        }

        public UserModel Candidate
        {
            get { return _candidate; }
            set { SetProperty(ref _candidate, value); }
        }

        public int Rating
        {
            get { return _rating; }
            set { SetProperty(ref _rating, value); }
        }

        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }

    }
}
