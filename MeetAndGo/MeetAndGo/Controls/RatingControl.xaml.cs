using Acr.UserDialogs;
using MeetAndGo.Models;
using MeetAndGo.ViewModels;
using MeetAndGo.Views;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeetAndGo.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingControl : StackLayout
    {
        public static readonly BindableProperty RatingProperty =
           BindableProperty.Create("Rating",
                                   typeof(int),
                                   typeof(RatingControl), default(int),
                                   BindingMode.TwoWay,
                                   propertyChanged: (bindable, oldValue, newValue) =>
                                   {
                                       var ratingControl = (RatingControl)bindable;
                                       ratingControl.FireStars(ratingControl.Rating);
                                   });

        public ICommand StarSelected => new Command(ExecuteStarSelected);

        public int Rating
        {
            set { SetValue(RatingProperty, value); }
            get { return (int)GetValue(RatingProperty); }
        }
        List<Image> stars = new List<Image>();

        public RatingControl()
        {
            InitializeComponent();
            stars.Add(starSelectedOne);
            stars.Add(starSelectedTwo);
            stars.Add(starSelectedThree);
            stars.Add(starSelectedFour);
            stars.Add(starSelectedFive);
            FireStars(Rating);
        }

        private void ExecuteStarSelected(object obj)
        {
            if (obj != null)
            {
                Rating = Int32.Parse(obj.ToString());
            }

            FireStars(Rating);
        }

        public void FireStars(int rating)
        {
            foreach (var star in stars)
            {
                if (Int32.Parse(star.AutomationId) <= rating)
                {
                    star.IsVisible = true;
                }
                else
                {
                    star.IsVisible = false;
                }
            }
        }

    }
}