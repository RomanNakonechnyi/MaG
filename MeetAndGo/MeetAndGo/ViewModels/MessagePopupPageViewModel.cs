using Acr.UserDialogs;
using MeetAndGo.Controls;
using MeetAndGo.Helpers;
using MeetAndGo.Models;
using MeetAndGo.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MeetAndGo.ViewModels
{
    public class MessagePopupPageViewModel : ViewModelBase
    {
        public ICommand SendCommand => new Command(async () => await ExecuteSendCommand());

        private VoteModel _vote;

        public VoteModel Vote
        {
            get { return _vote; }
            set { SetProperty(ref _vote, value); }
        }

        public MessagePopupPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Vote = new VoteModel();
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey(nameof(VoteModel)))
                {
                    Vote = (VoteModel)parameters[nameof(VoteModel)];
                }

            }
        }

        public async Task ExecuteSendCommand()
        {
            try
            {
                var navParam = new NavigationParameters
                {
                    { nameof(VoteModel), Vote }
                };

                await NavigationService.GoBackAsync(navParam);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        internal async Task ClosePopupPage()
        {
            await NavigationService.GoBackAsync();

        }
    }
}
