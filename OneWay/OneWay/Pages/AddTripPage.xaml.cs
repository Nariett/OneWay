using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using OneWay.Controls;
using OneWay.Class;
using System.Data;
using System.Linq;


namespace OneWay.Pages
{
    /// <summary>
    /// Логика взаимодействия для Trip.xaml
    /// </summary>
    public partial class AddTripPage : Page
    {
        public int IdUser;
        DataBase db = new DataBase("OneWayDB.db");
        public List<(int id, string octaneNumber)> octaneNumber;
        List<Car> cars = new List<Car> { };
        List<Route> routes = new List<Route> { };
        List<Points> points = new List<Points> { };
        public static List<Tuple<string, string>> AllCity = new List<Tuple<string, string>>();
        public List<string> number = new List<string>();
        private Trip selectTrip = null;
        public AddTripPage(int IdUser)
        {
            InitializeComponent();
            this.IdUser = IdUser;
            routes = db.GetAllRoutes(IdUser);
            points = db.GetAllPoints(IdUser);
            comboBoxStartPoint.Items.Clear();
            foreach (var item in points)
            {
                comboBoxStartPoint.Items.Add(item.Name);
                comboBoxLastPoint.Items.Add(item.Name);
            }
        }
        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxCar.Items.Clear();
            cars = db.GetAllCars(IdUser);
            foreach (Car car in cars)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = $"{car.Brand} {car.Model} {car.Equipment}";
                if (db.CheckFavoriteCar(IdUser, car.IdCar) == 1)
                {
                    item.Content += " ⭐";
                    item.Foreground = new SolidColorBrush(Colors.Gold);
                }
                comboBoxCar.Items.Add(item);
            }
        }
        private void AddPoint(string point)
        {
            Points.Children.Clear();
            foreach (var item in point.Split('#'))
            {

                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                TextBlock textCount = new TextBlock()
                {
                    Text = "Точка остановки",
                    FontSize = 20,
                    Foreground = (Brush)FindResource("TextSecundaryColor"),
                    TextAlignment = TextAlignment.Left,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                Grid.SetColumn(textCount, 0);
                TextBox newTextBox = new TextBox()
                {
                    Height = 26.6,
                    Tag = "Город",
                    Text = item.ToString(),
                    FontSize = 20,
                    Foreground = (Brush)FindResource("TextSecundaryColor"),
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0, 0, 0, 1),
                    Margin = new Thickness(5, 0, 5, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    IsReadOnly = true,
                };
                Grid.SetColumn(newTextBox, 1);
                grid.Children.Add(textCount);
                grid.Children.Add(newTextBox);
                Points.Children.Add(grid);
            }
        }
        void CreateTripItem(List<Route> values)
        {
            number.Clear();
            TripInfo.Children.Clear();
            int count = 1;
            foreach (var value in values)
            {
                string name = value.IdRoute.ToString();
                number.Add(name);
                Border border = new Border()
                {
                    Margin = new Thickness(10),
                    Background = Brushes.Transparent,
                    BorderBrush = (Brush)FindResource("TextSecundaryColor"),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(5),
                };
                border.MouseLeftButtonDown += CreateTripClick;
                Grid grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                TextBlock textBlock1 = new TextBlock()
                {
                    Text = $"Маршрут {count}",
                    FontSize = 25,
                    Foreground = (Brush)FindResource("TextSecundaryColor"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10)
                };
                if(db.CheckFavoriteRoute(IdUser, value.IdRoute) == 1)
                {
                    textBlock1.Foreground = new SolidColorBrush(Colors.Gold);
                }
                Grid.SetRow(textBlock1, 0);
                TimeSpan date = TimeSpan.FromMinutes(Convert.ToDouble(value.TravelTime));
                if (date.Days >= 1)
                {
                    if (date.Hours == 0)
                    {
                        TextBlock textBlock2 = new TextBlock()
                        {
                            Text = $"{date.Days} д {date.Minutes} мин",
                            FontSize = 20,
                            Foreground = (Brush)FindResource("TextSecundaryColor"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(10, 0, 10, 0)
                        };
                        Grid.SetRow(textBlock2, 1);
                        grid.Children.Add(textBlock2);
                    }
                    else
                    {
                        TextBlock textBlock2 = new TextBlock()
                        {
                            Text = $"{date.Days} д {date.Hours} ч {date.Minutes} мин",
                            FontSize = 20,
                            Foreground = (Brush)FindResource("TextSecundaryColor"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(10, 0, 10, 0)
                        };
                        Grid.SetRow(textBlock2, 1);
                        grid.Children.Add(textBlock2);
                    }
                }
                else if (date.Hours != 0)
                {
                    TextBlock textBlock2 = new TextBlock()
                    {
                        Text = $"{date.Hours} ч {date.Minutes} мин",
                        FontSize = 20,
                        Foreground = (Brush)FindResource("TextSecundaryColor"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(10, 0, 10, 0)
                    };
                    Grid.SetRow(textBlock2, 1);
                    grid.Children.Add(textBlock2);
                }
                else
                {
                    TextBlock textBlock2 = new TextBlock()
                    {
                        Text = $"{date.Minutes} мин",
                        FontSize = 20,
                        Foreground = (Brush)FindResource("TextSecundaryColor"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(10, 0, 10, 0)
                    };
                    Grid.SetRow(textBlock2, 1);
                    grid.Children.Add(textBlock2);
                }

                TextBlock textBlock3 = new TextBlock()
                {
                    Text = $"{value.Distance} км",
                    FontSize = 20,
                    Foreground = (Brush)FindResource("TextSecundaryColor"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10, 5, 10, 10)
                };
                Grid.SetRow(textBlock3, 2);

                grid.Children.Add(textBlock1);
                grid.Children.Add(textBlock3);
                border.Child = grid;
                TripInfo.Children.Add(border);
                count++;
            }
        }
        private void CreateRouteButton_Click(object sender, RoutedEventArgs e)
        {
            if(selectTrip != null ) // проверка выбрана ли поездка
            {
                if (comboBoxCar.SelectedIndex != -1 )
                {
                    Car selectedCar = null;
                    string selectItemCar = (comboBoxCar.SelectedItem as ComboBoxItem)?.Content.ToString();
                    if (selectItemCar.Contains(" ⭐"))
                    {
                        selectItemCar = selectItemCar.Substring(0, selectItemCar.Length - 2);
                    }
                    foreach (var item in cars)
                    {

                        if ($"{item.Brand} {item.Model} {item.Equipment}" == selectItemCar)
                        {
                            selectedCar = item;
                            break;
                        }
                    }
                    if (selectedCar != null) // выбран ли автомобиль
                    {
                        if (string.IsNullOrWhiteSpace(People.Text))
                        {
                            CustomMessageBox.Show("Уведомление", $"Введите число пассажиров.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                            return;
                        }
                        int numberOfPeople;
                        if (!int.TryParse(People.Text, out numberOfPeople))
                        {
                            CustomMessageBox.Show("Уведомление", $"Введите корректное число пассажиров.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                            return;
                        }
                        if (numberOfPeople > selectedCar.Seats)
                        {
                            CustomMessageBox.Show("Уведомление", $"Число пассажиров больше, чем число мест в машине.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                            return;
                        }
                        selectTrip.IdCar = selectedCar.IdCar;
                        selectTrip.People = numberOfPeople;

                        double distance = db.GetDistance(selectTrip.IdRoute);
                        double usedFuel;
                        double usedMoney;
                        if (selectedCar.Fuel == "Электричество")
                        {
                            usedFuel = Math.Round(distance * selectedCar.FuelConsumption, 2);
                            int fuelId = octaneNumber[FuelOctane.SelectedIndex].id;
                            usedMoney = Math.Round(usedFuel * db.GetFuelLastPrice(fuelId), 2);
                            TextFuelUse.Text = "Затрачено кВт/ч";
                        }
                        else if (selectedCar.Fuel == "Бензин")
                        {
                            usedFuel = Math.Round((distance / 100) * selectedCar.FuelConsumption, 2);
                            usedMoney = Math.Round(usedFuel * db.GetFuelLastPrice(selectedCar.FuelOctane), 2);
                            TextFuelUse.Text = "Затрачено топлива, л";
                        }
                        else if (selectedCar.Fuel == "Газ")
                        {
                            usedFuel = Math.Round((distance / 100) * selectedCar.FuelConsumption, 2);
                            usedMoney = Math.Round(usedFuel * db.GetFuelLastPrice(9), 2);
                            TextFuelUse.Text = "Затрачено топлива, л";
                        }
                        else
                        {
                            if (FuelOctane.SelectedIndex != -1)
                            {
                                usedFuel = Math.Round((distance / 100) * selectedCar.FuelConsumption, 2);
                                int fuelId = octaneNumber[FuelOctane.SelectedIndex].id;
                                usedMoney = Math.Round(usedFuel * db.GetFuelLastPrice(fuelId), 2);
                                TextFuelUse.Text = "Затрачено топлива, л";
                            }
                            else
                            {
                                CustomMessageBox.Show("Уведомление", $"Выберите тип топлива", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                                return;
                            }
                        }
                        selectTrip.UsedFuel = usedFuel;
                        selectTrip.UsedMoney = usedMoney;
                        selectTrip.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int minutes = Convert.ToInt32(db.GetTime(selectTrip.IdRoute));
                        DateTime finishTime = DateTime.Now.AddMinutes(minutes);
                        selectTrip.FinishTime = finishTime.ToString("yyyy-MM-dd HH:mm:ss");
                        FuelUse.Text = selectTrip.UsedFuel.ToString();
                        FuelUse.BorderBrush = Brushes.Green;
                        FuelUse.BorderThickness = new Thickness(0, 0, 0, 2); 
                        Price.BorderBrush = Brushes.Green;
                        Price.BorderThickness = new Thickness(0, 0, 0, 2);
                        Price.Text = selectTrip.UsedMoney.ToString();
                        var message = CustomMessageBox.Show("Уведомление", $"Сохранить поездку на автомобиле {selectItemCar} по машруту {comboBoxStartPoint.SelectedItem} - {comboBoxLastPoint.SelectedItem}", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Information);
                        if(message == MessageBoxResult.Yes)
                        {
                            if (db.InsertTrip(selectTrip)) // добавление поездки в бд
                            {
                                CustomMessageBox.Show("Уведомление", $"Поездка сохранена в систему.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Выберите автомобиль из списка.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    }
                }
                else
                {
                    CustomMessageBox.Show("Уведомление", $"Выберите автомобиль из списка.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                }
            }
            else
            {
                CustomMessageBox.Show("Уведомление", $"Выберите маршрут.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
            }
        }
        private void CreateTripClick(object sender, MouseButtonEventArgs e)
        {
            FuelUse.Text = "";
            Price.Text = "";
            FuelUse.BorderBrush = Brushes.Gray;
            FuelUse.BorderThickness = new Thickness(0);
            Price.BorderBrush = Brushes.Gray;
            Price.BorderThickness = new Thickness(0);
            foreach (Border border in TripInfo.Children)
            {
                border.BorderBrush = Brushes.Gray;
            }
            Border clickedBorder = sender as Border;
            if (clickedBorder != null)
            {
                clickedBorder.BorderBrush = Brushes.Green;
                Grid grid = clickedBorder.Child as Grid;
                if (grid != null && grid.Children.Count == 3)
                {
                    TextBlock textBlock1 = grid.Children[0] as TextBlock;
                    TextBlock textBlock2 = grid.Children[1] as TextBlock;
                    TextBlock textBlock3 = grid.Children[2] as TextBlock;

                    if (textBlock1 != null && textBlock2 != null && textBlock3 != null)
                    {
                        string routeName = textBlock2.Text;
                        string distance = textBlock3.Text;
                        string time = textBlock1.Text;
                        var result = CustomMessageBox.Show("Уведомление", $"Вы выбрали маршрут:\nНазвание: {routeName}\nДистанция: {distance}\nВремя: {time}\nЖелаете просмотреть информацию о машруте?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Information);
                        if (result == MessageBoxResult.Yes)
                        {
                            Points.Children.Clear();
                            Trip newTrip = new Trip();
                            newTrip.IdUser = IdUser;
                            int id = Convert.ToInt32(routeName.Split(' ')[1].Trim()) - 1;
                            newTrip.IdRoute = Convert.ToInt32(number[id]);
                            string points = "";
                            foreach (var item in routes)
                            {
                                if (newTrip.IdRoute == item.IdRoute)
                                {
                                    points = item.AdditionPoint;
                                }
                            }
                            if (points != "") // проверка на наличие точек остановки 
                            {
                                AddPoint(points);
                            }
                            if (comboBoxCar.SelectedIndex != -1)
                            {
                                Car selectedCar = null;
                                string selectItemCar = (comboBoxCar.SelectedItem as ComboBoxItem)?.Content.ToString();
                                if (selectItemCar.Contains(" ⭐"))
                                {
                                    selectItemCar = selectItemCar.Substring(0, selectItemCar.Length - 2);
                                }
                                foreach (var item in cars)
                                {
                                    
                                    if ($"{item.Brand} {item.Model} {item.Equipment}" == selectItemCar)
                                    {
                                        selectedCar = item;
                                        break;
                                    }
                                }
                                if (selectedCar != null)
                                {
                                    selectTrip = newTrip;
                                    selectTrip.IdCar = selectedCar.IdCar;
                                    double distanceTwo = db.GetDistance(newTrip.IdRoute);
                                    double usedFuel;
                                    double usedMoney;


                                    if (selectedCar.Fuel == "Электричество")
                                    {
                                        usedFuel = Math.Round(distanceTwo * selectedCar.FuelConsumption, 2);
                                        int fuelId = octaneNumber[FuelOctane.SelectedIndex].id;
                                        usedMoney = Math.Round(usedFuel * db.GetFuelLastPrice(fuelId), 2);
                                        TextFuelUse.Text = "Затрачено кВт/ч";
                                    } 
                                    else if(selectedCar.Fuel == "Бензин")
                                    {
                                        usedFuel = Math.Round((distanceTwo / 100) * selectedCar.FuelConsumption, 2);
                                        usedMoney = Math.Round(usedFuel * db.GetFuelLastPrice(selectedCar.FuelOctane), 2);
                                        TextFuelUse.Text = "Затрачено топлива, л";
                                    }
                                    else if (selectedCar.Fuel == "Газ")
                                    {
                                        usedFuel = Math.Round((distanceTwo / 100) * selectedCar.FuelConsumption, 2);
                                        usedMoney = Math.Round(usedFuel * db.GetFuelLastPrice(9), 2);
                                        TextFuelUse.Text = "Затрачено топлива, л";
                                    }
                                    else
                                    {
                                        if (FuelOctane.SelectedIndex != -1)
                                        {
                                            usedFuel = Math.Round((distanceTwo / 100) * selectedCar.FuelConsumption, 2);
                                            int fuelId = octaneNumber[FuelOctane.SelectedIndex].id;
                                            usedMoney = Math.Round(usedFuel * db.GetFuelLastPrice(fuelId), 2);
                                            TextFuelUse.Text = "Затрачено топлива, л";
                                        }
                                        else
                                        {
                                            CustomMessageBox.Show("Уведомление", $"Выберите тип топлива", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                                            return;
                                        }
                                    }

                                    FuelUse.Text = usedFuel.ToString();
                                    FuelUse.BorderBrush = Brushes.Green;
                                    FuelUse.BorderThickness = new Thickness(0, 0, 0, 2);
                                    Price.BorderBrush = Brushes.Green;
                                    Price.BorderThickness = new Thickness(0, 0, 0, 2);
                                    Price.Text = usedMoney.ToString();
                                }
                                else
                                {
                                    CustomMessageBox.Show("Уведомление", $"Для отображения информации выберите автомобиль из списка и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show("Уведомление", $"Для отображения информации выберите автомобиль из списка и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                                clickedBorder.BorderBrush = Brushes.Gray;
                            }
                        }
                        else
                        {
                            clickedBorder.BorderBrush = Brushes.Gray;
                        }
                    }
                }
            }
        }

       

        private void People_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                return;
            }

            string currentText = People.Text.Insert(People.SelectionStart, e.Text);
            if (!int.TryParse(currentText, out int seat))
            {
                e.Handled = true;
                return;
            }

            if (seat <= 0)
            {
                CustomMessageBox.Show("Уведомление", $"Число пассажиров должно быть больше 0", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                e.Handled = true;
            }
            else if (seat > 10)
            {
                CustomMessageBox.Show("Уведомление", $"Число пассажиров должно быть меньше или равно 10", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                e.Handled = true;
            }
        }

        private void comboBoxCar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FuelOctane.Items.Clear();
            Car car = cars[comboBoxCar.SelectedIndex];
            if (car.Fuel == "Электричество")
            {
                FuelOctane.Visibility = Visibility.Visible;
                octaneNumber = db.GetOctaneNumber("Электричество");
            }
            else if (car.Fuel == "Дизельное топливо")
            {
                FuelOctane.Visibility = Visibility.Visible;
                octaneNumber = db.GetOctaneNumber("Дизельное топливо");
            }
            else
            {
                FuelOctane.Visibility = Visibility.Collapsed;
                return;
            }

            foreach (var item in octaneNumber)
            {
                FuelOctane.Items.Add(item.octaneNumber);
            }
            FuelOctane.SelectedIndex = 0;
        }
        private void comboBoxStartPoint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxStartPoint.SelectedIndex != -1 && comboBoxLastPoint.SelectedIndex != -1)
            {
                selectTrip = null;
                string startPoint = this.comboBoxStartPoint.SelectedItem?.ToString();
                string lastPoint = this.comboBoxLastPoint.SelectedItem?.ToString();
                List<Route> selectRoute = db.SelectRoute($"{startPoint} - {lastPoint}", IdUser);
                if (selectRoute.Count > 0)
                {
                    CreateTripItem(selectRoute);
                }
                else
                {
                    CustomMessageBox.Show("Уведомление", $"Маршрут не найден.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    number.Clear();
                    TripInfo.Children.Clear();
                }
            }


            if(comboBoxLastPoint.SelectedIndex == -1)
            {
                List<Route> allRoute = db.GetAllRoutes(IdUser);
                List<Route> selectRoute = new List<Route>();
                List<Points> FinishPoints = new List<Points>();
                string startPoint = this.comboBoxStartPoint.SelectedItem?.ToString();
                int index = db.GetCityIdByName(IdUser, startPoint);
                foreach (var item in allRoute)
                {
                    if(item.PointOne == index)
                    {
                        selectRoute.Add(item);
                    }
                }
                comboBoxLastPoint.Items.Clear();
                foreach(var route in selectRoute)
                {
                    int q = (int)route.PointTwo;
                    foreach(var item in points)
                    {
                        if(item.IdPoint == q)
                        {
                            FinishPoints.Add(item);
                            break;
                        }
                    }
                }
                List<Points> filterPoint = FinishPoints.GroupBy(p => p.Name)
                                          .Select(g => g.First())
                                          .ToList();

                foreach (var item in filterPoint)
                {
                    comboBoxLastPoint.Items.Add(item.Name);
                }
            }
            else
            {
                comboBoxLastPoint.Items.Clear(); 
            }
        }

        private void comboBoxLastPointInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboBoxStartPoint.SelectedIndex != -1 && comboBoxLastPoint.SelectedIndex != -1)
            {
                selectTrip = null;
                string startPoint = this.comboBoxStartPoint.SelectedItem?.ToString();
                string lastPoint = this.comboBoxLastPoint.SelectedItem?.ToString();
                List<Route> selectRoute = db.SelectRoute($"{startPoint} - {lastPoint}",IdUser);
                if(selectRoute.Count > 0)
                {
                    CreateTripItem(selectRoute);
                }
                else
                {
                    CustomMessageBox.Show("Уведомление", $"Маршрут не найден.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    number.Clear();
                    TripInfo.Children.Clear();
                }
            }
        }

    }
}
