using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OneWay.Controls
{
    /// <summary>
    /// Логика взаимодействия для RouteItem.xaml
    /// </summary>
    public partial class RouteItem : UserControl
    {
        public event EventHandler InfoButtonClicked;
        public event EventHandler DeleteButtonClicked;
        public event EventHandler FavoriteIconClicked;
        public RouteItem()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void Route_Loaded(object sender, RoutedEventArgs e)
        {
            if (Point.ToString().Length == 0)
            {
                PointIcon.Visibility = Visibility.Collapsed;
            }
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
            DependencyProperty.Register("TripName", typeof(string), typeof(RouteItem));

        public string Distance
        {
            get { return (string)GetValue(DistanceProperty); }
            set { SetValue(DistanceProperty, value); }
        }

        public static readonly DependencyProperty DistanceProperty =
            DependencyProperty.Register("Distance", typeof(string), typeof(RouteItem));


        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(string), typeof(RouteItem));
        public string Point
        {
            get { return (string)GetValue(PointProperty); }
            set { SetValue(PointProperty, value); }
        }

        public static readonly DependencyProperty PointProperty =
            DependencyProperty.Register("Point", typeof(string), typeof(RouteItem));
        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(string), typeof(RouteItem));

        public string FavoriteRoute
        {
            get { return (string)GetValue(FavoriteRouteProperty); }
            set { SetValue(FavoriteRouteProperty, value); }
        }

        public static readonly DependencyProperty FavoriteRouteProperty =
            DependencyProperty.Register("FavoriteRoute", typeof(string), typeof(RouteItem), new PropertyMetadata(OnFavoriteRouteChanged));

        private static void OnFavoriteRouteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            RouteItem carItem = (RouteItem)d;
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

        private void FavoriteIcon_Click(object sender, RoutedEventArgs e)
        {
            FavoriteIconClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
