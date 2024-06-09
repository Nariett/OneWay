using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace OneWay.Controls
{
    /// <summary>
    /// Логика взаимодействия для ShortCarItem.xaml
    /// </summary>
    public partial class ShortCarItem : UserControl
    {
        public ShortCarItem()
        {
            InitializeComponent();
        }
        public string CarName
        {
            get { return (string)GetValue(CarNameProperty); }
            set { SetValue(CarNameProperty, value); }
        }

        public static readonly DependencyProperty CarNameProperty =
            DependencyProperty.Register("CarName", typeof(string), typeof(ShortCarItem));

        public string Volume
        {
            get { return (string)GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }

        public static readonly DependencyProperty VolumeProperty =
            DependencyProperty.Register("Volume", typeof(string), typeof(ShortCarItem));

        public string Body
        {
            get { return (string)GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.Register("Body", typeof(string), typeof(ShortCarItem));

        public string Transmission
        {
            get { return (string)GetValue(TransmissionProperty); }
            set { SetValue(TransmissionProperty, value); }
        }

        public static readonly DependencyProperty TransmissionProperty =
            DependencyProperty.Register("Transmission", typeof(string), typeof(ShortCarItem));
        public string Seat
        {
            get { return (string)GetValue(SeatProperty); }
            set { SetValue(SeatProperty, value); }
        }

        public static readonly DependencyProperty SeatProperty =
            DependencyProperty.Register("Seat", typeof(string), typeof(ShortCarItem));

        
    }
}
