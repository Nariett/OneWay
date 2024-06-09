using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using OneWay.Class;
using OneWay.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OneWay.Pages
{

    public partial class Dashboard : Page
    {
        public ObservableCollection<string> TripDates { get; set; }
        DataBase db = new DataBase("OneWayDB.db");
        List<Class.Trip> userTrip = new List<Class.Trip>();
        public static List<TripData> allTrip = new List<TripData> { };
        int IdUser;
        public Dashboard(int idUser)
        {
            InitializeComponent();
            IdUser = idUser;
            liveChart.Series.Clear();
            userTrip.Clear();
            allTrip.Clear();
            userTrip = db.GetAllTripByUserId(IdUser);
            if (userTrip.Count > 0)
            {
                liveChart.Visibility = Visibility.Visible;
                userTrip = userTrip.OrderBy(trip => DateTime.Parse(trip.StartTime)).ToList();
                for (int i = 0; i < userTrip.Count; i++)
                {
                    allTrip.Add(new TripData(i, userTrip[i].UsedMoney, Convert.ToDateTime(userTrip[i].StartTime)));
                }

                string name = db.GetFirstNameById(1);
                var seriesNew = new LineSeries
                {
                    Title = name,
                    Values = observablePoints(allTrip),
                    Visibility = System.Windows.Visibility.Visible
                };
                TripDates = new ObservableCollection<string>();
                DataContext = this;
                foreach (var x in userTrip)
                {
                    TripDates.Add(Convert.ToDateTime(x.StartTime).ToString("dd.MM.yyyy"));
                }
                TripDates.Add(allTrip[allTrip.Count - 1].Date.ToString("dd.MM.yyyy"));
                liveChart.Series = new SeriesCollection { seriesNew };
            }
            else
            {
                liveChart.Visibility = Visibility.Hidden;
            }
        }
        private ChartValues<ObservablePoint> observablePoints(List<TripData> list)
        {
            ChartValues<ObservablePoint> newPoint = new ChartValues<ObservablePoint>();
            foreach (var x in list)
            {
                ObservablePoint point = new ObservablePoint(x.X, x.Y);
                newPoint.Add(point);
            }
            return newPoint;
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            DateFrom.Text = string.Empty;
            DateBefore.Text = string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string dateOne = DateFrom.Text;
            string dateTwo = DateBefore.Text;
            string filterDate = "";
            DateTime date1, date2;
            if (!string.IsNullOrWhiteSpace(dateOne) || !string.IsNullOrWhiteSpace(dateTwo))
            {
                if (!DateTime.TryParseExact(dateOne, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date1) && !string.IsNullOrWhiteSpace(dateOne))
                {
                    CustomMessageBox.Show("Уведомление", $"Неправильный формат даты 1. Введите дату в формате dd.mm.yyyy.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                    return;
                }
                if (!DateTime.TryParseExact(dateTwo, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date2) && !string.IsNullOrWhiteSpace(dateTwo))
                {
                    CustomMessageBox.Show("Уведомление", $"Неправильный формат даты 2. Введите дату в формате dd.mm.yyyy.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                    return;
                }

                if (date1 != DateTime.MinValue && date2 != DateTime.MinValue)
                {
                    if (date1 < date2)
                    {
                        date2 = date2.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                        filterDate = $"StartTime BETWEEN '{date1.ToString("yyyy-MM-dd HH:mm:ss")}' AND '{date2.ToString("yyyy-MM-dd HH:mm:ss")}'";
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Дата 1 больше, чем дата 2. Введите корректные данные и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                        return;
                    }
                }
                else if (date1 != DateTime.MinValue)
                {
                    filterDate = $"StartTime >= '{date1.ToString("yyyy-MM-dd HH:mm:ss")}'";
                }
                else if (date2 != DateTime.MinValue)
                {
                    date2 = date2.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                    filterDate = $"StartTime <= '{date2.ToString("yyyy-MM-dd HH:mm:ss")}'";
                }
                userTrip = db.GetAllTripByUserIdAndDateRange(IdUser, "AND " + filterDate);
                userTrip = userTrip.OrderBy(trip => DateTime.Parse(trip.StartTime)).ToList();
                if (userTrip.Count > 0)
                {
                    liveChart.Series.Clear();
                    List<TripData> filteredTripData = new List<TripData>();
                    for (int i = 0; i < userTrip.Count; i++)
                    {
                        filteredTripData.Add(new TripData(i, userTrip[i].UsedMoney, Convert.ToDateTime(userTrip[i].StartTime)));
                    }
                    var seriesNew = new LineSeries
                    {
                        Title = db.GetFirstNameById(1),
                        Values = observablePoints(filteredTripData),
                        Visibility = Visibility.Visible
                    };
                    liveChart.Series.Add(seriesNew);

                    TripDates = new ObservableCollection<string>();
                    DataContext = this;
                    foreach (var x in userTrip)
                    {
                        TripDates.Add(Convert.ToDateTime(x.StartTime).ToString("dd.MM.yyyy"));
                    }
                    liveChart.Series = new SeriesCollection { seriesNew };
                    liveChart.AxisX[0].Labels = TripDates;
                }
                else
                {
                    CustomMessageBox.Show("Уведомление", $"Поездки за данный период не найдены.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                }
            }
            else
            {
                userTrip = db.GetAllTripByUserId(IdUser);
                userTrip = userTrip.OrderBy(trip => DateTime.Parse(trip.StartTime)).ToList();
                liveChart.Series.Clear();
                List<TripData> filteredTripData = new List<TripData>();
                for (int i = 0; i < userTrip.Count; i++)
                {
                    filteredTripData.Add(new TripData(i, userTrip[i].UsedMoney, Convert.ToDateTime(userTrip[i].StartTime)));
                }
                var seriesNew = new LineSeries
                {
                    Title = db.GetFirstNameById(1),
                    Values = observablePoints(filteredTripData),
                    Visibility = Visibility.Visible
                };
                liveChart.Series.Add(seriesNew);

                TripDates = new ObservableCollection<string>();
                DataContext = this;
                foreach (var x in userTrip)
                {
                    TripDates.Add(Convert.ToDateTime(x.StartTime).ToString("dd.MM.yyyy"));
                }
                liveChart.Series = new SeriesCollection { seriesNew };
                liveChart.AxisX[0].Labels = TripDates;
            }
        }

    }
    public class TripData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime Date { get; set; }

        public TripData(double x, double y, DateTime date)
        {
            X = x;
            Y = y;
            Date = date;
        }
    }
}
