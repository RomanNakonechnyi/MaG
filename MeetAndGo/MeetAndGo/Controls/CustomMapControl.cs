using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using MeetAndGo.Controls.Models;

namespace MeetAndGo.Controls {
    public class CustomMapControl : Map {
        private readonly List<Xamarin.Forms.GoogleMaps.Polyline> _directionPolylines = new List<Xamarin.Forms.GoogleMaps.Polyline>();
        private readonly List<Xamarin.Forms.GoogleMaps.Polyline> userDirectionPolylines = new List<Xamarin.Forms.GoogleMaps.Polyline>();
        private readonly List<Pin> _directionPins = new List<Pin>();

        #region Public fields
        public List<Direction> Directions {
            get {
                return (List<Direction>)GetValue(DirectionsProperty);
            }
            set {
                SetValue(DirectionsProperty, value);
            }
        }
        public List<Direction> UserDirections {
            get {
                return (List<Direction>)GetValue(UserDirectionsProperty);
            }
            set {
                SetValue(UserDirectionsProperty, value);
            }
        }
        public ICommand MoveToCommand {
            get => (ICommand)GetValue(MoveToCommandProperty);
            set => SetValue(MoveToCommandProperty, value);
        }


        public static readonly BindableProperty DirectionsProperty = BindableProperty.Create(
            nameof(Directions),
            typeof(List<Direction>),
            typeof(CustomMapControl),
            null,
            propertyChanged: DirectionsPropertyChanged);

        public static readonly BindableProperty UserDirectionsProperty = BindableProperty.Create(
            nameof(UserDirections),
            typeof(List<Direction>),
            typeof(CustomMapControl),
            null,
            propertyChanged: UserDirectionsPropertyChanged);

        public static readonly BindableProperty MoveToCommandProperty = BindableProperty.Create(
            nameof(MoveToCommand),
            typeof(ICommand),
            typeof(CustomMapControl),
            null,
            BindingMode.OneWayToSource,
            propertyChanged: MoveToCommandPropertyChanged);
        #endregion


        public CustomMapControl() {
            MoveToCommand = new Command<object>(ExecuteMoveToCommand);
        }


        private static void DirectionsPropertyChanged( BindableObject bindable, object oldValue, object newValue ) {
            if( !( bindable is CustomMapControl control ) ) {
                return;
            }

            if( newValue is List<Direction> directions ) {
                directions.ForEach(direction => {
                    var polyline = control.GetPolyline(direction.Positions, "DB1A07");

                    control._directionPolylines?.Add(polyline);
                    control.Polylines.Add(polyline);
                    var startPin = new Pin {
                        Position = direction.Positions.First()
                    };
                    startPin.Label = "StartPin";
                    var endPin = new Pin {
                        Position = direction.Positions.Last()
                    };
                    endPin.Label = "EndPin";
                    control._directionPins.Add(startPin);
                    control._directionPins.Add(endPin);
                    control.Pins.Add(startPin);
                    control.Pins.Add(endPin);
                    control.ExecuteMoveToCommand(endPin.Position);

                });
            }
        }
        private static void UserDirectionsPropertyChanged( BindableObject bindable, object oldValue, object newValue ) {
            if( !( bindable is CustomMapControl control ) ) {
                return;
            }

            if( newValue is List<Direction> directions ) {
                directions.ForEach(direction => {
                    var polyline = control.GetPolyline(direction.Positions, "11583F");

                    control._directionPolylines?.Add(polyline);
                    control.Polylines.Add(polyline);
                    var startPin = new Pin {
                        Position = direction.Positions.First()
                    };
                    startPin.Label = "StartPin";
                    var endPin = new Pin {
                        Position = direction.Positions.Last()
                    };
                    endPin.Label = "EndPin";
                    control._directionPins.Add(startPin);
                    control._directionPins.Add(endPin);
                    control.Pins.Add(startPin);
                    control.Pins.Add(endPin);
                    control.ExecuteMoveToCommand(startPin.Position);

                });
            }
        }
        private static void MoveToCommandPropertyChanged( BindableObject bindable, object oldvalue, object newvalue ) {

        }

        private Xamarin.Forms.GoogleMaps.Polyline GetPolyline( List<Position> direction, string color ) {
            var polyline = new Xamarin.Forms.GoogleMaps.Polyline {
                StrokeWidth = 3,
                StrokeColor = Color.FromHex(color)
            };

            direction.ForEach(x => polyline.Positions.Add(x));
            return polyline;
        }
        private void ExecuteMoveToCommand( object o ) {
            if( o is Position position ) {
                MoveToRegion(new MapSpan(position, 0.01, 0.01));
            }

        }

    }
}
