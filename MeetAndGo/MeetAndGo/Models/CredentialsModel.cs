using Newtonsoft.Json;

namespace MeetAndGo.Models {
    public class CredentialsModel {
        [JsonProperty ( "phone_number" )]
        public string PhoneNumber { get; set; }

        [JsonProperty ( "password" )]
        public string Password { get; set; }

        public CredentialsModel(string number, string password ) {
            PhoneNumber = number;
            Password = password;
        }
    }
}
