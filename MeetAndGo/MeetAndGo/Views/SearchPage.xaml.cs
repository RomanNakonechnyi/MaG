using DurianCode.PlacesSearchBar;
using MeetAndGo.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeetAndGo.Views {
    public partial class SearchPage : ContentPage {
        public SearchPage() {
            InitializeComponent();
            InitializePlaceBar(StartLocationBar);
            InitializePlaceBar(EndLocationBar);
        }

        private void InitializePlaceBar( PlacesBar placesBar ) {
            placesBar.ApiKey = Constants.ConstantHelper.GoogleApiKey;
            placesBar.PlacesRetrieved += PlacesSearchBar_PlacesRetrieved;
        }

        private void PlacesSearchBar_PlacesRetrieved( object sender, AutoCompleteResult result ) {
            if( BindingContext is SearchPageViewModel viewModel ) {
                if( viewModel.SearchBarModel.StartBarIsFocused ) {
                    viewModel.SearchBarModel.StartLocations = result.AutoCompletePlaces;
                    viewModel.SearchBarModel.StartLocationLoading = false;
                } else if( viewModel.SearchBarModel.EndBarIsFocused ) {
                    viewModel.SearchBarModel.EndLocations = result.AutoCompletePlaces;
                    viewModel.SearchBarModel.EndLocationLoading = false;
                }
            }
        }
    }
}
