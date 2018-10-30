using MeetAndGo.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MeetAndGo.Conventors {
    public class RemainingTimeConverter : IValueConverter {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
            if( Equals(value, null) ) {
                return "";

            }
            EventModel Event = value as EventModel;

            var days = Math.Abs(Event.CreatedTime.Day - DateTime.Now.Day);
            var hours = Math.Abs(Event.CreatedTime.Hour - DateTime.Now.Hour);
            var minutes = Math.Abs(Event.CreatedTime.Minute - DateTime.Now.Minute);

            return string.Format(( days != 0 ? $"{days}d " : "" ) + ( hours != 0 ? $"{hours}h " : "" ) + ( minutes != 0 ? $"{minutes}min" : "" ) + " left");
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
            throw new NotSupportedException();
        }
    }
}

