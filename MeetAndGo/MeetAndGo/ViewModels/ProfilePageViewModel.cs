using Acr.UserDialogs;
using MeetAndGo.Constants;
using MeetAndGo.Helpers;
using MeetAndGo.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetAndGo.ViewModels {
    public class ProfilePageViewModel : ViewModelBase {
        private UserModel _userModel;
        public UserModel UserModel
        {
            get => _userModel;
            set => SetProperty(ref _userModel, value);
        }
        
        public ICommand EditCommand { get; private set; }

        public ProfilePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            EditCommand = new DelegateCommand(async () => await ExecuteEditCommand());
        }

        public override void OnNavigatingTo ( INavigationParameters parameters ) {
            UserModel = ( UserModel )parameters["User"];

            base.OnNavigatingTo ( parameters );
        }

        private async Task ExecuteEditCommand()
        {
            await UserDialogs.Instance.AlertAsync ( "Caution!", ConstantHelper.MessageForEditButton, "OK" );
        }
    }
}