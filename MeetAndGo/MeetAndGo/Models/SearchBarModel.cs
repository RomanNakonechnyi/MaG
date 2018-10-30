using DurianCode.PlacesSearchBar;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetAndGo.Models {
    public class SearchBarModel : BindableBase {
        private bool _startListIsVisible;
        private bool _endListIsVisible;
        private bool _intermediateListIsVisible;
        private bool _endBarIsFocused;
        private bool _startBarIsFocused;
        private bool _intermediateBarIsFocused;
        private bool _endLocationLoading;
        private bool _startLocationLoading;
        private List<AutoCompletePrediction> _intermediatelocations;
        private List<AutoCompletePrediction> _startLocations;
        private List<AutoCompletePrediction> _endLocations;
        private AutoCompletePrediction _startSelectedItem;
        private AutoCompletePrediction _endSelectedItem;
        private AutoCompletePrediction _intermediateSelectedItem;

        public bool StartListIsVisible {
            get => _startListIsVisible;
            set => SetProperty(ref _startListIsVisible, value);
        }

        public bool EndListIsVisible {
            get => _endListIsVisible;
            set => SetProperty(ref _endListIsVisible, value);
        }

        public bool IntermediateListIsVisible {
            get => _intermediateListIsVisible;
            set => SetProperty(ref _intermediateListIsVisible, value);
        }

        public bool StartBarIsFocused {
            get => _startBarIsFocused;
            set => SetProperty(ref _startBarIsFocused, value);
        }

        public bool EndBarIsFocused {
            get => _endBarIsFocused;
            set => SetProperty(ref _endBarIsFocused, value);
        }

        public bool IntermediateBarIsFocused {
            get => _intermediateBarIsFocused;
            set => SetProperty(ref _intermediateBarIsFocused, value);
        }

        public bool StartLocationLoading {
            get => _startLocationLoading;
            set => SetProperty(ref _startLocationLoading, value);
        }

        public bool EndLocationLoading {
            get => _endLocationLoading;
            set => SetProperty(ref _endLocationLoading, value);
        }

        public List<AutoCompletePrediction> IntermediateLocations {
            get => _intermediatelocations;
            set => SetProperty(ref _intermediatelocations, value);
        }

        public List<AutoCompletePrediction> StartLocations {
            get => _startLocations;
            set => SetProperty(ref _startLocations, value);
        }

        public List<AutoCompletePrediction> EndLocations {
            get => _endLocations;
            set => SetProperty(ref _endLocations, value);
        }

        public AutoCompletePrediction StartSelectedItem {
            get => _startSelectedItem;
            set => SetProperty(ref _startSelectedItem, value);
        }

        public AutoCompletePrediction EndSelectedItem {
            get => _endSelectedItem;
            set => SetProperty(ref _endSelectedItem, value);
        }

        public AutoCompletePrediction IntermediateSelectedItem {
            get => _intermediateSelectedItem;
            set => SetProperty(ref _intermediateSelectedItem, value);
        }
    }
}
