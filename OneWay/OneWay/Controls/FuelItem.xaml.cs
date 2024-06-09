using LiveCharts;
using LiveCharts.Wpf;
using OneWay.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OneWay.Controls
{
    public partial class FuelItem : UserControl
    {
        public event EventHandler GridClicked;
        public FuelItem()
        {
            InitializeComponent();
        }
        private void GridClick(object sender, RoutedEventArgs e)
        {
            GridClicked?.Invoke(this, EventArgs.Empty);
        }
        public ObservableCollection<string> TripDates { get; set; }

        public static readonly DependencyProperty OctanNumberProperty =
            DependencyProperty.Register("OctanNumber", typeof(string), typeof(FuelItem));

        public string OctanNumber
        {
            get { return (string)GetValue(OctanNumberProperty); }
            set { SetValue(OctanNumberProperty, value); }
        }
        public static readonly DependencyProperty FuelTypeProperty =
            DependencyProperty.Register("FuelType", typeof(string), typeof(FuelItem));

        public string FuelType
        {
            get { return (string)GetValue(FuelTypeProperty); }
            set { SetValue(FuelTypeProperty, value); }
        }

        public static readonly DependencyProperty PriceRubProperty =
            DependencyProperty.Register("PriceRub", typeof(string), typeof(FuelItem));

        public string PriceRub
        {
            get { return (string)GetValue(PriceRubProperty); }
            set { SetValue(PriceRubProperty, value); }
        }

        public static readonly DependencyProperty IdUserProperty =
            DependencyProperty.Register("IdUser", typeof(string), typeof(FuelItem));

        public string IdUser
        {
            get { return (string)GetValue(IdUserProperty); }
            set { SetValue(IdUserProperty, value); }
        }

        private void this_Loaded(object sender, RoutedEventArgs e)
        {
            DataBase db = new DataBase("OneWayDB.db");

            List<(double price, DateTime Date)> fuelPriceHistory = db.GetFuelPriceHistory(db.GetFuelId(OctanNumber),Convert.ToInt32(IdUser));

            var seriesNew = new LineSeries
            {
                Title = "Цена",
                Values = new ChartValues<double>(fuelPriceHistory.Select(item => item.price)),
                StrokeThickness = 2,
                Fill = Brushes.Transparent,
                LineSmoothness = 0
            };

            TripDates = new ObservableCollection<string>();

            foreach (var priceEntry in fuelPriceHistory)
            {
                TripDates.Add(priceEntry.Date.ToString("dd.MM.yyyy"));
            }

            TripDates.Add(fuelPriceHistory.Last().Date.ToString("dd.MM.yyyy"));

            DataContext = this;

            liveChart.Series = new SeriesCollection { seriesNew };
        }
    }
}
