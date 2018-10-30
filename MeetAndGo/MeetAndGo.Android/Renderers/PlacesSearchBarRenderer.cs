using Android.Content;
using Android.Widget;
using MeetAndGo.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchBar), typeof(PlacesSearchBarRenderer))]
namespace MeetAndGo.Droid.Renderers {
    class PlacesSearchBarRenderer : SearchBarRenderer {
        public PlacesSearchBarRenderer( Context context ) : base(context) {
        }

        protected override void OnElementChanged( ElementChangedEventArgs<SearchBar> e ) {
            base.OnElementChanged(e);
            if( Control != null ) {
                var searchView = Control;
                searchView.SetBackgroundColor(global::Android.Graphics.Color.White);
                searchView.SetIconifiedByDefault(false);
                int searchIconId = Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                var icon = (ImageView)searchView.FindViewById(searchIconId);
                icon.LayoutParameters = new LinearLayout.LayoutParams(0, 0);
            }
        }
    }
}