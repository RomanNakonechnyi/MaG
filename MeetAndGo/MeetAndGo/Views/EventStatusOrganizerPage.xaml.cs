using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeetAndGo.Views {
    [XamlCompilation ( XamlCompilationOptions.Compile )]
    public partial class EventStatusOrganizerPage : ContentPage {
        public static EventStatusOrganizerPage Instance { get; private set; }

        public EventStatusOrganizerPage () {
            InitializeComponent ();

            Instance = this;
        }
    }
}