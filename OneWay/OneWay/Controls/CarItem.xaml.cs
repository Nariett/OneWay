using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OneWay.Controls
{
    /// <summary>
    /// Логика взаимодействия для CarItem.xaml
    /// </summary>
    public partial class CarItem : UserControl
    {
        public event EventHandler FavoriteButtonClicked;
        public event EventHandler DeleteButtonClicked;
        public event EventHandler GridClicked;
        public CarItem()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void FavoriteCar_Click(object sender, RoutedEventArgs e)
        {
            FavoriteButtonClicked?.Invoke(this, EventArgs.Empty);
        }
        private void DeleteButtom_Click(object sender, RoutedEventArgs e)
        {
            DeleteButtonClicked?.Invoke(this, EventArgs.Empty);
        }
        private void GridClick(object sender, RoutedEventArgs e)
        {
            GridClicked?.Invoke(this, EventArgs.Empty);
        }
        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(string), typeof(CarItem));

        public string CarName
        {
            get { return (string)GetValue(CarNameProperty); }
            set { SetValue(CarNameProperty, value); }
        }

        public static readonly DependencyProperty CarNameProperty =
            DependencyProperty.Register("CarName", typeof(string), typeof(CarItem));

        public string Model
        {
            get { return (string)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(string), typeof(CarItem));
        public string Volume
        {
            get { return (string)GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }

        public static readonly DependencyProperty VolumeProperty =
            DependencyProperty.Register("Volume", typeof(string), typeof(CarItem));

        public string Transmission
        {
            get { return (string)GetValue(TransmissionProperty); }
            set { SetValue(TransmissionProperty, value); }
        }

        public static readonly DependencyProperty TransmissionProperty =
            DependencyProperty.Register("Transmission", typeof(string), typeof(CarItem));

        public string Body
        {
            get { return (string)GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.Register("Body", typeof(string), typeof(CarItem));
        public string Seat
        {
            get { return (string)GetValue(SeatProperty); }
            set { SetValue(SeatProperty, value); }
        }

        public static readonly DependencyProperty SeatProperty =
            DependencyProperty.Register("Seat", typeof(string), typeof(CarItem));

        public string FuelConsumption
        {
            get { return (string)GetValue(FuelConsumptionProperty); }
            set { SetValue(FuelConsumptionProperty, value); }
        }

        public static readonly DependencyProperty FuelConsumptionProperty =
            DependencyProperty.Register("FuelConsumption", typeof(string), typeof(CarItem));
        public string FavoriteCar
        {
            get { return (string)GetValue(FavoriteCarProperty); }
            set { SetValue(FavoriteCarProperty, value); }
        }

        public static readonly DependencyProperty FavoriteCarProperty =
            DependencyProperty.Register("FavoriteCar", typeof(string), typeof(CarItem), new PropertyMetadata(OnFavoriteCarChanged));


        private static void OnFavoriteCarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            CarItem carItem = (CarItem)d;
            string newValue = (string)e.NewValue;

            if (newValue == "1")
            {
                carItem.FavoriteIcon.Foreground = new SolidColorBrush(Color.FromRgb(255, 215, 0));
            }
            else
            {
                carItem.FavoriteIcon.Foreground = new SolidColorBrush(Color.FromRgb(176, 183, 195));
            }
        }
    }
}
