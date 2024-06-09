using System;
using System.Windows;
using System.Windows.Controls;

namespace OneWay.Controls
{
    /// <summary>
    /// Логика взаимодействия для TripItem.xaml
    /// </summary>
    public partial class TripItem : UserControl
    {
        public event EventHandler ReportButtonClicked;
        public event EventHandler InfoButtonClicked;
        public event EventHandler DeleteButtonClicked;
        public TripItem()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void TripItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (Point.ToString().Length == 0)
            {
                PointIcon.Visibility = Visibility.Collapsed;
            }
        }
        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            ReportButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            InfoButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteButtonClicked?.Invoke(this, EventArgs.Empty);
        }
        public string TripName
        {
            get { return (string)GetValue(TripNameProperty); }
            set { SetValue(TripNameProperty, value); }
        }

        public static readonly DependencyProperty TripNameProperty =
            DependencyProperty.Register("TripName", typeof(string), typeof(TripItem));

        public string CarModel
        {
            get { return (string)GetValue(CarModelProperty); }
            set { SetValue(CarModelProperty, value); }
        }

        public static readonly DependencyProperty CarModelProperty =
            DependencyProperty.Register("CarModel", typeof(string), typeof(TripItem));

        public string Distance
        {
            get { return (string)GetValue(DistanceProperty); }
            set { SetValue(DistanceProperty, value); }
        }

        public static readonly DependencyProperty DistanceProperty =
            DependencyProperty.Register("Distance", typeof(string), typeof(TripItem));


        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(string), typeof(TripItem));
        public string Car
        {
            get { return (string)GetValue(CarProperty); }
            set { SetValue(CarProperty, value); }
        }

        public static readonly DependencyProperty CarProperty =
            DependencyProperty.Register("Car", typeof(string), typeof(TripItem));

        public string Point
        {
            get { return (string)GetValue(PointProperty); }
            set { SetValue(PointProperty, value); }
        }

        public static readonly DependencyProperty PointProperty =
            DependencyProperty.Register("Point", typeof(string), typeof(TripItem));
        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(string), typeof(TripItem));
    }
}
