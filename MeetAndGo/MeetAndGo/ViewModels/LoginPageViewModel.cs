using Acr.UserDialogs;
using MeetAndGo.Models;
using MeetAndGo.Services;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetAndGo.ViewModels {
    public class LoginPageViewModel : ViewModelBase {
        private readonly ApiService _apiService;

        private UserModel _user;
        private string _password;

        public UserModel User {
            get { return _user; }
            set { SetProperty ( ref _user, value ); }
        }
        public string Password {
            get { return _password; }
            set { SetProperty ( ref _password, value ); }
        }

        public ICommand SignInCommand => new DelegateCommand<UserModel> ( async user => await ExecuteSignInCommand () );

        public LoginPageViewModel ( INavigationService navigationService ) 
            : base ( navigationService ) {
            _apiService = new ApiService ();
        }

        private async Task<UserModel> ExecuteSignInCommand() {
            // Get user's JSON. 
            // ...

            return User;
        }
    }
}
