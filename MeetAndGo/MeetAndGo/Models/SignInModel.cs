using Newtonsoft.Json;
using Prism.Mvvm;

namespace MeetAndGo.Models {
    public class SignInModel : BindableBase{
        private string _key;
        private UserModel _user;

        [JsonProperty ( "key" )]
        public string Key {
            get { return _key; }
            set { SetProperty ( ref _key, value ); }
        }

        [JsonProperty ( "user" )]
        public UserModel User {
            get { return _user; }
            set { SetProperty ( ref _user, value ); }
        }
    }
}
