using OneWay.Class;
using OneWay.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OneWay.Pages
{
    public partial class TripInfoPage : Page
    {
        public int IdUser;
        public int IdTrip;
        DataBase db = new DataBase("OneWayDB.db");
        public TripInfoPage(int idUser, int idTrip)
        {
            InitializeComponent();
            IdUser = idUser;
            IdTrip = idTrip;
            Trip trip = db.GetTripById(idTrip);
            StartTime.Text = Convert.ToDateTime(trip.StartTime).ToString("HH:mm dd.MM.yyyy");
            FinishTime.Text = Convert.ToDateTime(trip.FinishTime).ToString("HH:mm dd.MM.yyyy");
            UsedFuel.Text = trip.UsedFuel.ToString();
            UsedMoney.Text = trip.UsedMoney.ToString();
            People.Text = trip.People.ToString();
            var route = db.GetRouteInfoById(trip.IdRoute);
            StartPoint.Text = route.Item1;
            FinishPoint.Text = route.Item2;
            Distance.Text = route.Item3.ToString();
            TimeSpan date = TimeSpan.FromMinutes(Convert.ToDouble(route.Item4));
            if (date.Days >= 1)
            {
                if (date.Hours == 0)
                {
                    Time.Text = $"{date.Days} д {date.Minutes} мин";
                }
                else
                {
                    Time.Text = $"{date.Days} д {date.Hours} ч {date.Minutes} мин";
                }
            }
            else
            {
                if (date.Hours != 0)
                {
                    Time.Text = $"{date.Hours} ч {date.Minutes} мин";
                }
                else
                {
                    Time.Text = $"{date.Minutes} мин";
                }
            }
            if (route.Item5 != "")
            {
                Points.Text = route.Item5.Replace('#', '\n');
            }
            else
            {
                Points.Visibility = Visibility.Collapsed;
                TextPoints.Visibility = Visibility.Collapsed;
            }
            Car car = db.GetCar(trip.IdCar);
            if (car.Brand != "")
            {
                Brand.Text = car.Brand;
                Model.Text = car.Model;
                Generation.Text = car.Generation;
                Equipment.Text = car.Equipment;
            }
            else
            {
                Brand.Text = "Неизвестно";
                Model.Text = "Неизвестно";
                Generation.Text = "Неизвестно";
                Equipment.Text = "Неизвестно";
            }
            if (car.Fuel != "Электричество")
            {
                TextUsedFuel.Text = "Использовано топлива, л";
            }
            else
            {
                TextUsedFuel.Text = "Использовано электричества, кВт*ч";
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var messageBoxResult = CustomMessageBox.Show("Уведомление", $"Желаете удалить данную поездку?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Information);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if(db.DeleteTrip(IdTrip))
                {
                    CustomMessageBox.Show("Уведомление", $"Поездка была удалена.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    History historyPage = new History(IdUser);
                    Window parentWindow = Window.GetWindow(this);
                    if (parentWindow != null && parentWindow is MainWindow mainWindow && mainWindow.fContainer != null)
                    {
                        mainWindow.fContainer.Content = historyPage;
                    }
                }
            }
        }
    }
}
