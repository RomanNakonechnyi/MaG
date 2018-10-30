using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MeetAndGo.Helpers {
    public class ItemSelectedToCommand : Behavior<ListView> {
        public static readonly BindableProperty SelectCommandProperty =
            BindableProperty.Create (
                propertyName: "SelectCommand",
                returnType: typeof ( ICommand ),
                declaringType: typeof ( ItemSelectedToCommand ) );

        public ICommand SelectCommand {
            get { return ( ICommand )GetValue ( SelectCommandProperty ); }
            set { SetValue ( SelectCommandProperty, value ); }
        }

        protected override void OnAttachedTo ( ListView bindable ) {
            base.OnAttachedTo ( bindable );
            bindable.ItemSelected += ListView_ItemSelected;
            bindable.BindingContextChanged += ListView_BindingContextChanged;
        }

        protected override void OnDetachingFrom ( ListView bindable ) {
            base.OnDetachingFrom ( bindable );
            bindable.ItemSelected -= ListView_ItemSelected;
            bindable.BindingContextChanged -= ListView_BindingContextChanged;
        }

        private void ListView_BindingContextChanged ( object sender, EventArgs eventArgs ) {
            var listView = sender as ListView;
            BindingContext = listView?.BindingContext;
        }

        private void ListView_ItemSelected ( object sender, SelectedItemChangedEventArgs e ) {
            SelectCommand.Execute ( null );
        }
    }
}