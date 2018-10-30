using MeetAndGo.Models.Enums;
using MeetAndGo.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.SecureStorage;
using Plugin.SecureStorage.Abstractions;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml; 
using Prism.Plugin.Popups;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MeetAndGo {
    public partial class App : PrismApplication {
        private readonly ISecureStorage _secureStorage = CrossSecureStorage.Current;

        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App () : this ( null ) { }

        public App ( IPlatformInitializer initializer ) : base ( initializer ) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            AppCenter.Start("ios=93549d86-5a9f-4d31-be5a-54aa8806c531;" +
                              "android=80730428-d76e-4d62-b0ac-51f392bfeedd;",
                              typeof(Analytics), typeof(Crashes));

            //_secureStorage.SetValue("userStatus", UserStatus.User.ToString());

            if (_secureStorage.GetValue("userStatus") != null)
            {
                if (_secureStorage.GetValue("userStatus") == UserStatus.Member.ToString())
                {
                    await NavigationService.NavigateAsync("MenuMasterPage/NavigationPage/EventStatusMemberPage");
                    return;
                }

                if (_secureStorage.GetValue("userStatus") == UserStatus.Organizer.ToString())
                {
                    await NavigationService.NavigateAsync("MenuMasterPage/NavigationPage/EventStatusOrganizerPage");
                    return;
                }
                else
                {
                    await NavigationService.NavigateAsync("MenuMasterPage/NavigationPage/SearchPage");
                }
            }
            else
            {
                await NavigationService.NavigateAsync("MenuMasterPage/NavigationPage/SearchPage");
                return;
            }
        }

        protected override void OnResume () {
            _secureStorage.DeleteKey ( "tempStartLocation" );
            _secureStorage.DeleteKey ( "tempEndLocation" );

            base.OnResume ();
        }

        protected override void RegisterTypes( IContainerRegistry containerRegistry ) {
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<MenuMasterPage>();
            containerRegistry.RegisterForNavigation<EventListPage>();
            containerRegistry.RegisterForNavigation<EventDetailPage>();
            containerRegistry.RegisterForNavigation<EventStatusMemberPage>();
            containerRegistry.RegisterForNavigation<EventStatusOrganizerPage>();
            containerRegistry.RegisterForNavigation<SearchPage>();
            containerRegistry.RegisterForNavigation<EventMemberPage>();
            containerRegistry.RegisterForNavigation<MessagePopupPage>();
            containerRegistry.RegisterForNavigation<NewEventPage>();
            containerRegistry.RegisterForNavigation<ProfilePage>();
        }
    }
}
