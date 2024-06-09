using System.Windows;
using System.Windows.Controls;

namespace OneWay.Controls
{
    /// <summary>
    /// Логика взаимодействия для ShortTripItem.xaml
    /// </summary>
    public partial class ShortTripItem : UserControl
    {
        public ShortTripItem()
        {
            InitializeComponent();
        }


        public string Trip
        {
            get { return (string)GetValue(TripProperty); }
            set { SetValue(TripProperty, value); }
        }
        
        public static readonly DependencyProperty TripProperty =
            DependencyProperty.Register("Trip", typeof(string), typeof(ShortTripItem));

        public string Car
        {
            get { return (string)GetValue(CarProperty); }
            set { SetValue(CarProperty, value); }
        }

        public static readonly DependencyProperty CarProperty =
            DependencyProperty.Register("Car", typeof(string), typeof(ShortTripItem));

        public string Distanse
        {
            get { return (string)GetValue(DistanseProperty); }
            set { SetValue(DistanseProperty, value); }
        }

        public static readonly DependencyProperty DistanseProperty =
            DependencyProperty.Register("Distanse", typeof(string), typeof(ShortTripItem));

        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(string), typeof(ShortTripItem));
    }
}
