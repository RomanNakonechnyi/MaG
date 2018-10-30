using MeetAndGo.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer ( typeof ( ListView ), typeof ( ListRenderer ) )]
namespace MeetAndGo.iOS.Renderers {
    public class ListRenderer : ListViewRenderer {
        protected override void OnElementChanged ( ElementChangedEventArgs<ListView> e ) {
            base.OnElementChanged ( e );

            if ( this.Control == null ) return;

            this.Control.TableFooterView = new UIView ();
        }
    }
}