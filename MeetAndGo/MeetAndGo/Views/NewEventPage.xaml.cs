using DurianCode.PlacesSearchBar;
using MeetAndGo.ViewModels;
using Xamarin.Forms;

namespace MeetAndGo.Views {
    public partial class NewEventPage : ContentPage {
        public NewEventPage() {
            InitializeComponent();
            InitializePlaceBar(startLocationsSearchBar);
            InitializePlaceBar(endLocationsSearchBar);
            InitializePlaceBar(intermediateLocationsSearchBar);
        }

        private void InitializePlaceBar( PlacesBar placesBar ) {
            placesBar.ApiKey = Constants.ConstantHelper.GoogleApiKey;
            placesBar.PlacesRetrieved += PlacesSearchBar_PlacesRetrieved;
        }

        private void PlacesSearchBar_PlacesRetrieved( object sender, AutoCompleteResult result ) {
            if( BindingContext is NewEventPageViewModel viewModel ) {
                if( viewModel.SearchBarListView.StartBarIsFocused ) {
                    viewModel.SearchBarListView.StartLocations = result.AutoCompletePlaces;
                } else if( viewModel.SearchBarListView.EndBarIsFocused ) {
                    viewModel.SearchBarListView.EndLocations = result.AutoCompletePlaces;
                } else if( viewModel.SearchBarListView.IntermediateBarIsFocused ) {
                    viewModel.SearchBarListView.IntermediateLocations = result.AutoCompletePlaces;
                }
            }
        }
    }
}
