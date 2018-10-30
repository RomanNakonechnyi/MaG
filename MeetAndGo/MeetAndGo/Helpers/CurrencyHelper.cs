using MeetAndGo.Services;
using MeetAndGo.UserControls;
using Plugin.Geolocator.Abstractions;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MeetAndGo.Helpers {
    class CurrencyHelper {
        public static string GetCurrencyCodeFromCountry () {
            ReverseGeocodingService reverseGeocoding = new ReverseGeocodingService ();

            var userPosition = CurrentLocation.CurrentPosition;

            var currency = "";

            if ( userPosition != null ) {
                var geographicInfo = reverseGeocoding?.GetGeographicInfo ( userPosition.Latitude, userPosition.Longitude, true );
                var address = geographicInfo?.results?.LastOrDefault ()?.address_components?.FirstOrDefault ();
                var regions = CultureInfo.GetCultures ( CultureTypes.SpecificCultures ).Select ( x => new RegionInfo ( x.LCID ) );

                if ( address == null || regions == null ) {
                    return currency;
                }

                currency = regions?.FirstOrDefault ( region => region.EnglishName.Contains ( address?.long_name ) )?.CurrencySymbol;

                if ( currency == null ) {
                    currency = "";
                }
            }

            return currency;
        }
    }
}
