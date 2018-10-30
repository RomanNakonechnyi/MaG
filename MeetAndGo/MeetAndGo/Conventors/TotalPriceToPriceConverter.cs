using MeetAndGo.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MeetAndGo.Conventors {
    public class TotalPriceToPriceConverter : IValueConverter {
        public object Convert ( object value, Type targetType, object parameter, CultureInfo culture ) {
            if ( Equals ( value, null ) ) {
                return "";
            }

            EventModel Event = value as EventModel;
            return string.Format ( $"{ Event.TotalPrice / Event.Members.Count } {Event.CurrencyCode}" );
        }

        public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture ) {
            throw new NotSupportedException ();
        }
    }
}
