using MeetAndGo.Controls.Models;
using MeetAndGo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MeetAndGo.Controls {
    public class UserMapControl : Map {
        private readonly List<Polyline> _directionPolylines = new List<Polyline>();
        private readonly List<Pin> _directionPins = new List<Pin>();
        private readonly List<Pin> _waypointPins = new List<Pin>();

        public static readonly BindableProperty DirectionsProperty = BindableProperty.Create(
           nameof(Directions),
           typeof(List<Direction>),
           typeof(UserMapControl),
           null,
           propertyChanged: DirectionsPropertyChanged);

        public static readonly BindableProperty WaypointsProperty = BindableProperty.Create(
           nameof(Waypoints),
           typeof(List<Waypoint>),
           typeof(UserMapControl),
           null,
           propertyChanged: WaypointsPropertyChanged);

        public static readonly BindableProperty MoveToCommandProperty = BindableProperty.Create(
            nameof(MoveToCommand),
            typeof(ICommand),
            typeof(UserMapControl),
            null,
            BindingMode.OneWayToSource,
            propertyChanged: MoveToCommandPropertyChanged);

        public List<Direction> Directions {
            get => (List<Direction>)GetValue(DirectionsProperty);
            set => SetValue(DirectionsProperty, value);
        }

        public List<Waypoint> Waypoints {
            get => (List<Waypoint>)GetValue(WaypointsProperty);
            set => SetValue(WaypointsProperty, value);
        }

        public ICommand MoveToCommand {
            get => (ICommand)GetValue(MoveToCommandProperty);
            set => SetValue(MoveToCommandProperty, value);
        }

        public UserMapControl() {
            MoveToCommand = new Command<object>(ExecuteMoveToCommand);
            Task.Run(() => Initialization());
        }

        private void Initialization() {
            MyLocationEnabled = true;
            UiSettings.MyLocationButtonEnabled = true;
            UiSettings.RotateGesturesEnabled = false;
            UiSettings.ZoomGesturesEnabled = true;
        }

        private static void MoveToCommandPropertyChanged( BindableObject bindable, object oldValue, object newValue ) { }

        private void ExecuteMoveToCommand( object o ) {
            if( o is Position position ) {
                MoveToRegion(new MapSpan(position, 0.05, 0.05));
            }
        }

        private static void DirectionsPropertyChanged( BindableObject bindable, object oldValue, object newValue ) {
            if( !( bindable is UserMapControl control ) ) {
                return;
            }

            // Remove previous direction polylines
            control._directionPolylines?.ForEach(x => control.Polylines.Remove(x));
            control._directionPolylines?.Clear();

            // Remove previous direction pins
            control._directionPins?.ForEach(x => control.Pins.Remove(x));
            control._directionPins?.Clear();

            if( newValue is List<Direction> directions ) {
                directions.ForEach(direction => {
                    var polyline = control.GetPolyline(direction.Positions);

                    control._directionPolylines?.Add(polyline);
                    control.Polylines.Add(polyline);

                    var pinEnd = new Pin {
                        Type = PinType.SearchResult,
                        Label = "Destination",
                        Position = direction.Positions.Last()
                    };
                    var pinStart = new Pin {
                        Type = PinType.SearchResult,
                        Label = "Start",
                        Position = direction.Positions.First()
                    };

                    control._directionPins.Add(pinEnd);
                    control._directionPins.Add(pinStart);
                    control.Pins.Add(pinEnd);
                    control.Pins.Add(pinStart);

                    control.MoveToRegion(MapSpan.FromCenterAndRadius(pinEnd.Position, Distance.FromKilometers(1)));
                });
            }
        }

        private static void WaypointsPropertyChanged( BindableObject bindable, object oldValue, object newValue ) {
            if( !( bindable is UserMapControl control ) ) {
                return;
            }

            control._waypointPins?.ForEach(x => control.Pins.Remove(x));
            control._waypointPins?.Clear();

            if( newValue is List<Waypoint> waypoints ) {
                waypoints.ForEach(waypoint => {
                    var pin = new Pin {
                        Label = "Waypoint",
                        Position = waypoint.Position
                    };

                    control._waypointPins.Add(pin);
                    control.Pins.Add(pin);
                });
            }
        }

        private Polyline GetPolyline( List<Position> direction ) {
            var polyline = new Polyline {
                StrokeWidth = 3,
                StrokeColor = Color.FromHex("#626262")
            };

            direction.ForEach(x => polyline.Positions.Add(x));
            return polyline;
        }

    }
}
