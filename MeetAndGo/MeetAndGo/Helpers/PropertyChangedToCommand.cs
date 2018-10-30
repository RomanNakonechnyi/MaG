using DurianCode.PlacesSearchBar;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace MeetAndGo.Helpers {
    class PropertyChangedToCommand : Behavior<PlacesBar> {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create (
                propertyName: "Command",
                returnType: typeof ( ICommand ),
                declaringType: typeof ( PropertyChangedToCommand ) );

        public ICommand Command {
            get { return ( ICommand )GetValue ( CommandProperty ); }
            set { SetValue ( CommandProperty, value ); }
        }

        protected override void OnAttachedTo ( PlacesBar bindable ) {
            base.OnAttachedTo ( bindable );
            bindable.PropertyChanged += PlacesBar_PropertyChanged;
            bindable.BindingContextChanged += PlacesBar_BindingContextChanged;
        }

        protected override void OnDetachingFrom ( PlacesBar bindable ) {
            base.OnDetachingFrom ( bindable );
            bindable.PropertyChanged -= PlacesBar_PropertyChanged;
            bindable.BindingContextChanged -= PlacesBar_BindingContextChanged;
        }

        private void PlacesBar_BindingContextChanged ( object sender, EventArgs eventArgs ) {
            var placesBar = sender as PlacesBar;
            BindingContext = placesBar?.BindingContext;
        }

        private void PlacesBar_PropertyChanged ( object sender, PropertyChangedEventArgs e ) {
            Command?.Execute ( null );
        }
    }
}
