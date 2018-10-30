using System.ComponentModel;
using CoreGraphics;
using MeetAndGo.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer ( typeof ( SearchBar ), typeof ( PlacesSearchBarRenderer ) )]
namespace MeetAndGo.iOS.Renderers {
    public class PlacesSearchBarRenderer : SearchBarRenderer {
        protected override void OnElementPropertyChanged ( object sender, PropertyChangedEventArgs e ) {
            //Override needed, otherwise the original Xamarin code will force show the Cancel button on the right side of the entry field
            if ( e.PropertyName == SearchBar.TextProperty.PropertyName ) {
                Control.Text = Element.Text;
            }

            if ( e.PropertyName != SearchBar.CancelButtonColorProperty.PropertyName && e.PropertyName != SearchBar.TextProperty.PropertyName )
                base.OnElementPropertyChanged ( sender, e );
        }

        protected override void OnElementChanged ( ElementChangedEventArgs<SearchBar> args ) {
            base.OnElementChanged ( args );

            UISearchBar bar = Control;

            bar.ShowsCancelButton = false;
            bar.SearchBarStyle = UISearchBarStyle.Minimal;
            bar.SetImageforSearchBarIcon ( UIImage.FromBundle ( "location" ), UISearchBarIcon.Search, UIControlState.Normal );


            Foundation.NSString _searchField = new Foundation.NSString ( "searchField" );
            var textFieldInsideSearchBar = ( UITextField )bar.ValueForKey ( _searchField );
            textFieldInsideSearchBar.BorderStyle = UITextBorderStyle.None;
            textFieldInsideSearchBar.ClipsToBounds = true;
            textFieldInsideSearchBar.Layer.BorderWidth = 0.3f;
            textFieldInsideSearchBar.Layer.CornerRadius = 5f;
            textFieldInsideSearchBar.Layer.BorderColor = UIColor.LightGray.CGColor;
            textFieldInsideSearchBar.BackgroundColor = UIColor.White;
        }
    }
}