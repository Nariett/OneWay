using OneWay.Class;
using OneWay.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace OneWay.Pages
{
    public partial class History : Page
    {
        public int IdUser;
        DataBase db = new DataBase("OneWayDB.db");
        List<Trip> allTrip = new List<Trip>();
        List<Car> cars = new List<Car>();
        public History(int IdUser)
        {
            InitializeComponent();
            comboBoxCar.Items.Clear();
            cars = db.GetAllCars(IdUser);
            foreach (Car car in cars)
            {
                string carName = $"{car.Brand} {car.Model} {car.Equipment}";
                comboBoxCar.Items.Add(carName);
            }
            this.IdUser = IdUser;
            allTrip = db.GetAllTripByUserId(IdUser);
            CreateItem();
        }
        private void CreateItem()
        {
            HistoryPanel.Children.Clear();
            foreach (var item in allTrip)
            {
                try
                {
                    var info = db.GetRouteInfoById(item.IdRoute);
                    string startPoint = info.Item1;
                    string finishPoint = info.Item2;
                    double distance = info.Item3;
                    string travelTime = info.Item4;
                    string points = info.Item5;
                    TripItem trip = new TripItem();
                    trip.TripName = startPoint + " - " + finishPoint;
                    string formattedPoints = "";
                    if (!string.IsNullOrEmpty(points))
                    {
                        string[] point = points.Split('#');
                        formattedPoints += string.Join("\n", point);
                    }
                    trip.Point = formattedPoints;

                    TimeSpan date = TimeSpan.FromMinutes(Convert.ToDouble(travelTime));
                    if (date.Days >= 1)
                    {
                        if (date.Hours == 0)
                        {
                            trip.Time = $"{date.Days} д {date.Minutes} мин";
                        }
                        else
                        {
                            trip.Time = $"{date.Days} д {date.Hours} ч {date.Minutes} мин";
                        }
                    }
                    else
                    {
                        if (date.Hours != 0)
                        {
                            trip.Time = $"{date.Hours} ч {date.Minutes} мин";
                        }
                        else
                        {
                            trip.Time = $"{date.Minutes} мин";
                        }
                    }
                    trip.Distance = distance + " км";
                    trip.Id = item.IdTrip.ToString();
                    Car car = db.GetCar(item.IdCar);
                    if (car.Brand != null)
                    {
                        trip.Car = car.Brand;
                        trip.CarModel = car.Model;
                    }
                    else
                    {
                        trip.Car = "неизвестный";
                        trip.CarModel = "данный автомобиль был удален";
                    }
                    trip.ReportButtonClicked += TripItem_ReportButtonClicked;
                    trip.InfoButtonClicked += TripItem_InfoButtonClicked;
                    trip.DeleteButtonClicked += TripItem_DeleteButtonClicked;
                    HistoryPanel.Children.Add(trip);
                }
                catch
                {

                }
            }
        }
        private void AcceptFilter_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder filterQuery = new StringBuilder("SELECT * FROM Trips WHERE ");

            // автомобиль
            if (comboBoxCar.SelectedItem != null)
            {
                Car selectedCar = cars.FirstOrDefault(item => $"{item.Brand} {item.Model} {item.Equipment}" == comboBoxCar.SelectedItem.ToString());
                if (selectedCar != null)
                {
                    filterQuery.Append($"IdCar = {selectedCar.IdCar} AND ");
                }
            }

            // точки
            int startId = db.GetCityIdByName(IdUser, PointOne.Text);
            int finishId = db.GetCityIdByName(IdUser, PointTwo.Text);

            if (startId != -1)
            {
                filterQuery.Append($"IdRoute IN (SELECT IdRoute FROM Routes WHERE PointOne = {startId}) AND ");
            }
            else if (!string.IsNullOrWhiteSpace(PointOne.Text))
            {
                CustomMessageBox.Show("Уведомление", "Точка отправления не найдена. Проверьте название и повторите попытку", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return;
            }

            if (finishId != -1)
            {
                filterQuery.Append($"IdRoute IN (SELECT IdRoute FROM Routes WHERE PointTwo = {finishId}) AND ");
            }
            else if (!string.IsNullOrWhiteSpace(PointTwo.Text))
            {
                CustomMessageBox.Show("Уведомление", "Точка прибытия не найдена. Проверьте название и повторите попытку", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return;
            }

            // дата
            DateTime date1, date2;
            if (DateTime.TryParseExact(DateFrom.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date1))
            {
                filterQuery.Append($"StartTime >= '{date1.ToString("yyyy-MM-dd HH:mm:ss")}' AND ");
            }
            else if (DateFrom.Text != "")
            {
                CustomMessageBox.Show("Уведомление", $"Введите корректную дату 1 в формате {DateTime.Now.ToString("dd.MM.yyyy")} и повторите попытку. Проверьте название и повторите попытку", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return;
            }
            if (DateTime.TryParseExact(DateTo.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date2))
            {
                date2 = date2.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                filterQuery.Append($"StartTime <= '{date2.ToString("yyyy-MM-dd HH:mm:ss")}' AND ");
            }
            else if (DateTo.Text != "")
            {
                CustomMessageBox.Show("Уведомление", $"Введите корректную дату 2 в формате {DateTime.Now.ToString("dd.MM.yyyy")} и повторите попытку. Проверьте название и повторите попытку", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return;
            }

            // цена
            double priceOne, priceTwo;
            if (double.TryParse(PriceFrom.Text, out priceOne))
            {
                filterQuery.Append($"UsedMoney >= {priceOne.ToString("0.00", CultureInfo.InvariantCulture)} AND ");
            }

            if (double.TryParse(PriceTo.Text, out priceTwo))
            {
                filterQuery.Append($"UsedMoney <= {priceTwo.ToString("0.00", CultureInfo.InvariantCulture)} AND ");
            }

            if (filterQuery.ToString().EndsWith(" AND "))
            {
                filterQuery.Remove(filterQuery.Length - 5, 5);
            }

            string combinedQuery = filterQuery.ToString();
            if(combinedQuery != "SELECT * FROM Trips WHERE ")
            {
                allTrip = db.GetHistory(combinedQuery);
                CreateItem();
            }
            else
            {
                CustomMessageBox.Show("Уведомление", "Отображены все маршруты.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                allTrip = db.GetAllTripByUserId(IdUser);
                CreateItem();
            }
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            foreach (var child in FilterPanel.Children)
            {
                if (child is TextBox textBox && !string.IsNullOrEmpty(textBox.Name))
                {
                    textBox.Text = string.Empty;
                }
                if (child is StackPanel stackPanel)
                {
                    foreach (var childTwo in stackPanel.Children)
                    {
                        if (childTwo is TextBox textBoxTwo && !string.IsNullOrEmpty(textBoxTwo.Name))
                        {
                            textBoxTwo.Text = string.Empty;
                        }
                    }
                }
            }
            comboBoxCar.SelectedIndex = -1;
        }

        private void TripItem_ReportButtonClicked(object sender, EventArgs e)
        {
            if (sender is TripItem tripItem)
            {
                string tripName = tripItem.TripName;
                string distance = tripItem.Distance;
                string time = tripItem.Time;
                string car = tripItem.Car;
                string id = tripItem.Id;
                string point = tripItem.Point;
                Report reportPage = new Report(IdUser, Convert.ToInt32(tripItem.Id));
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null && parentWindow is MainWindow mainWindow && mainWindow.fContainer != null)
                {
                    mainWindow.fContainer.Content = reportPage;
                }
            }
        }

        private void TripItem_InfoButtonClicked(object sender, EventArgs e)
        {
            if (sender is TripItem tripItem)
            {
                var messageBoxResult = CustomMessageBox.Show("Уведомление", $"Желаете посмотреть информаци о поездке?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Information);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    TripInfoPage reportPage = new TripInfoPage(IdUser, Convert.ToInt32(tripItem.Id));
                    Window parentWindow = Window.GetWindow(this);
                    if (parentWindow != null && parentWindow is MainWindow mainWindow && mainWindow.fContainer != null)
                    {
                        mainWindow.fContainer.Content = reportPage;
                    }
                }
            }
        }

        private void TripItem_DeleteButtonClicked(object sender, EventArgs e)
        {
            var quequestion = CustomMessageBox.Show("Уведомление", $"Желаете удалить данную поездку?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Information);
            if (quequestion == MessageBoxResult.Yes)
            {
                if (sender is TripItem tripItem)
                {
                    int id = Convert.ToInt32(tripItem.Id);
                    if (db.DeleteTrip(id))
                    {
                        CustomMessageBox.Show("Уведомление", $"Данная поездка была удалена из истории.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                        allTrip = db.GetAllTripByUserId(IdUser);
                        CreateItem();
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Данная поездка не была удалена из истории.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                    }
                }
            }
        }

        private void PriceFrom_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                return;
            }
            string currentText = PriceFrom.Text;
            int selectionStart = PriceFrom.SelectionStart;
            string newText = currentText.Insert(selectionStart, e.Text);

            if (int.TryParse(newText, out int price))
            {
                if (price <= 0)
                {
                    CustomMessageBox.Show("Уведомление", $"Сумма затрат должна быть больше 0", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    e.Handled = true;
                }
            }
        }

        private void PriceTo_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                return;
            }
            string currentText = PriceTo.Text;
            int selectionStart = PriceTo.SelectionStart;
            string newText = currentText.Insert(selectionStart, e.Text);

            if (int.TryParse(newText, out int price))
            {
                if (price <= 0)
                {
                    CustomMessageBox.Show("Уведомление", $"Сумма затрат должна быть больше 0", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    e.Handled = true;
                }
            }
        }


    }
}
