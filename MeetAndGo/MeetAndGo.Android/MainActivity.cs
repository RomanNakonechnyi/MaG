using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Prism;
using Prism.Ioc;
using MeetAndGo.Views;
using Xamarin.Forms;
using ImageCircle.Forms.Plugin.Droid;

namespace MeetAndGo.Droid {
    [Activity(Label = "MeetAndGo", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        protected override void OnCreate( Bundle bundle ) {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            UserDialogs.Init(this);
            Xamarin.FormsGoogleMaps.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            ImageCircleRenderer.Init();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0));
        }

        public override void OnRequestPermissionsResult( int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults ) {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override void OnBackPressed() {
            if( Xamarin.Forms.Application.Current.MainPage.Navigation.GetType() == typeof(NewEventPage) ) {
                Device.BeginInvokeOnMainThread(async () => {
                    var result = await UserDialogs.Instance.ConfirmAsync("Your changes will not be saved. Are you sure you want to exit?", "Caution!", "OK", "Cancel");
                    if( result )
                        base.OnBackPressed();
                });
            } else { base.OnBackPressed(); }
        }
        
        public class AndroidInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry container)
            {
                // Register any platform specific implementations
            }
        }

    }
    

}

