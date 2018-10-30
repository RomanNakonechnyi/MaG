using MeetAndGo.iOS.Renderers;
using MeetAndGo.iOS.Views;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer ( typeof ( ContentPage ), typeof ( SecondaryToolbarItemsRenderer ) )]
namespace MeetAndGo.iOS.Renderers {
    public class SecondaryToolbarItemsRenderer : PageRenderer {
        private List<ToolbarItem> _secondaryItems;
        private bool _isRendered;
        public UITableView table;

        protected override void OnElementChanged ( VisualElementChangedEventArgs e ) {
            base.OnElementChanged ( e );

            if ( e.NewElement is ContentPage page ) {
                _secondaryItems = page.ToolbarItems.Where ( i => i.Order == ToolbarItemOrder.Secondary ).ToList ();
                _secondaryItems.ForEach ( t => page.ToolbarItems.Remove ( t ) );
            }
        }

        public override void ViewWillAppear ( bool animated ) {
            var element = ( ContentPage )Element;

            if ( _secondaryItems != null && _secondaryItems.Count > 0 && !_isRendered ) {
                element.ToolbarItems.Add ( new ToolbarItem () {
                    Order = ToolbarItemOrder.Primary,
                    Icon = "icon_dots.png",
                    Priority = 1,
                    Command = new Command ( () => {
                        ToolClicked ();
                    } )
                } );
            }

            _isRendered = true;

            base.ViewWillAppear ( animated );
        }

        private void ToolClicked () {
            if ( table == null ) {
                var childRect = new RectangleF ( ( float )View.Bounds.Width - 250, 0, 250, _secondaryItems.Count () * 56 );
                table = new UITableView ( childRect ) {
                    Source = new TableSource ( _secondaryItems ),
                    ClipsToBounds = false
                };

                table.Layer.ShadowColor = UIColor.Black.CGColor;
                table.Layer.ShadowOpacity = 0.3f;
                table.Layer.ShadowRadius = 5.0f;
                table.Layer.ShadowOffset = new SizeF ( 5f, 5f );
                table.BackgroundColor = UIColor.White;

                Add ( table );

                return;
            }

            foreach ( var subview in View.Subviews ) {
                if ( subview == table ) {
                    table.RemoveFromSuperview ();

                    return;
                }
            }

            Add ( table );
        }
    }
}