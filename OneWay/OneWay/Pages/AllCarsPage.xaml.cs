using OneWay.Class;
using OneWay.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OneWay.Pages
{
    public partial class AllCarsPage : Page
    {
        public int IdUser;
        DataBase db = new DataBase("OneWayDB.db");
        public AllCarsPage(int IdUser)
        {
            InitializeComponent();
            this.IdUser = IdUser;
        }

        private void AcceptFilter_Click(object sender, RoutedEventArgs e)
        {
            List<string> filter = new List<string>();
            if (CarBrand.Text != "")
            {
                filter.Add($"Brand = '{CarBrand.Text}'");
            }
            if (CarModel.Text != "")
            {
                filter.Add($"Model = '{CarModel.Text}'");
            }
            if (SeatFrom.Text != "" && SeatTo.Text != "")
            {
                filter.Add($"Seats BETWEEN {SeatFrom.Text} AND {SeatTo.Text}");
            }
            else
            {
                if(SeatFrom.Text != "")
                {
                    filter.Add($"Seats >= {SeatFrom.Text}");
                }
                if(SeatTo.Text != "")
                {
                    filter.Add($"Seats <= {SeatTo.Text}");
                }
            }
            List<string> fuel = new List<string>();
            if (Gasoline.IsChecked == true)
            {
                fuel.Add("Бензин");
            }
            if (Diesel.IsChecked == true)
            {
                fuel.Add("Дизельное топливо");
            }
            if (Electro.IsChecked == true)
            {
                fuel.Add("Электричество");
            }
            if (Gas.IsChecked == true)
            {
                fuel.Add("Газ");
            }
            string fuelString = "Fuel = ";
            if (fuel.Count != 0)
            {
                if (fuel.Count >= 2)
                {
                    fuelString += string.Join(" OR Fuel = ", fuel.Select(f => $"'{f}'"));
                }
                else
                {
                    fuelString += $"'{fuel[0]}'";
                }
            }

            List<string> gearBox = new List<string>();
            if (Auto.IsChecked == true)
            {
                gearBox.Add("АКПП");
            }
            if (Mech.IsChecked == true)
            {
                gearBox.Add("МКПП");
            }
            string gearBoxString = "GearBox = ";
            if (gearBox.Count != 0)
            {
                if (gearBox.Count >= 2)
                {
                    gearBoxString += string.Join(" OR GearBox = ", gearBox.Select(f => $"'{f}'"));
                }
                else
                {
                    gearBoxString += $"'{gearBox[0]}'";
                }
            }

            List<string> Body = new List<string>();
            if (Sedan.IsChecked == true)
            {
                Body.Add("Седан");
            }
            if (Jeep.IsChecked == true)
            {
                Body.Add("Внедорожник");
            }
            if (Universal.IsChecked == true)
            {
                Body.Add("Униварсал");
            }
            if (Hatchback.IsChecked == true)
            {
                Body.Add("Хетчбек");
            }
            if (Liftback.IsChecked == true)
            {
                Body.Add("Лифтбек");
            }
            if (Minivan.IsChecked == true)
            {
                Body.Add("Минивэн");
            }
            if (Coupe.IsChecked == true)
            {
                Body.Add("Купе");
            }
            if (Pickup.IsChecked == true)
            {
                Body.Add("Купе");
            }
            string bodyString = "Body = ";
            if (Body.Count != 0)
            {
                if (Body.Count >= 2)
                {
                    bodyString += string.Join(" OR Body = ", Body.Select(f => $"'{f}'"));
                }
                else
                {
                    bodyString += $"'{Body[0]}'";
                }
            }

            List<string> allFilters = new List<string>();
            if (filter.Count > 0)
            {
                allFilters.AddRange(filter);
            }
            if (!string.IsNullOrEmpty(fuelString) && fuelString.Length > 7)
            {
                allFilters.Add($"({fuelString})");
            }
            if (!string.IsNullOrEmpty(gearBoxString) && gearBoxString.Length > 10)
            {
                allFilters.Add($"({gearBoxString})");
            }
            if (!string.IsNullOrEmpty(bodyString) && bodyString.Length > 7)
            {
                allFilters.Add($"({bodyString})");
            }

            string resultString = string.Join(" AND ", allFilters);
            List<Car> allCar = db.GetFilterCars(resultString, IdUser);
            if(allCar.Count == 0)
            {
                CarPanel.Children.Clear();
                CustomMessageBox.Show("Уведомление", "Автомобиль с такими характеристикми не найден.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
            }
            else
            {
                CarPanel.Children.Clear();
                AddCarInPanel(allCar);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var child in FilterPanel.Children)
            {
                if (child is CheckBox checkBox)
                {
                    checkBox.IsChecked = false;
                }
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
                        if (childTwo is CheckBox checkBoxTwo)
                        {
                            checkBoxTwo.IsChecked = false;
                        }
                    }
                }
            }
        }

        private void DateFrom_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void DateBefore_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                return;
            }
        }

        private void SeatTo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void SeatFrom_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Car> allCar = db.GetAllCars(IdUser);
            AddCarInPanel(allCar);
        }
        private void AddCarInPanel(List<Car> cars)
        {
            CarPanel.Children.Clear();
            foreach (var item in cars)
            {
                CarItem newCar = new CarItem();
                newCar.Id = item.IdCar.ToString();
                newCar.CarName = item.Brand;
                newCar.Model = item.Model.ToString();
                newCar.Transmission = item.GearBox;
                newCar.Seat = item.Seats.ToString();
                newCar.Body = item.Body;
                newCar.FavoriteCar = db.CheckFavoriteCar(IdUser, item.IdCar).ToString();

                if (item.Volume.ToString() == "0")
                {
                    newCar.Volume = $"{item.EngineType}";
                    newCar.FuelConsumption = $"{item.FuelConsumption} кВт*ч / км";
                }
                else
                {
                    newCar.Volume = $"{Math.Round(item.Volume.Value / 1000, 2)} / {item.Fuel}";
                    newCar.FuelConsumption = $"{item.FuelConsumption} л / 100 км";
                }
                newCar.FavoriteButtonClicked += FavoriteButton_Click;
                newCar.DeleteButtonClicked += DeleteButton_Click;
                newCar.GridClicked += GridButton_Click;
                CarPanel.Children.Add(newCar);
                
            }
        }
/*        private void CarItemClick(object sender, MouseButtonEventArgs e)
        {
            CarItem clickedCar = sender as CarItem;
            CustomMessageBox.Show("Уведомление", $"Вы выбрали автомобиль: {clickedCar.CarName}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
        }*/
        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            AddCarPage addCarPage = new AddCarPage(IdUser);
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null && parentWindow is MainWindow mainWindow && mainWindow.fContainer != null)
            {
                mainWindow.fContainer.Content = addCarPage;
            }
        }
        public void FavoriteButton_Click(object sender, EventArgs e)
        {
            if (sender is CarItem carItem)
            {
                if (carItem.FavoriteCar == "0")
                {
                    var result = CustomMessageBox.Show("Уведомление", $"Желаете добавить автомобиль в избранные автомобили?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if(db.AddFavoriteCar(IdUser, Convert.ToInt32(carItem.Id)))
                        {
                            carItem.FavoriteCar = "1";
                            CustomMessageBox.Show("Уведомление", $"Автомобиль успешно добавлен в избранные автомобили.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                        }
                        else
                        {
                            CustomMessageBox.Show("Уведомление", $"Автомобиль не был добавлен в избранные автомобили.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    var result = CustomMessageBox.Show("Уведомление", $"Желаете удалить автомобиль из избранных автомобилей?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (db.DeleteFavoriteCar(IdUser, Convert.ToInt32(carItem.Id)))
                        {
                            carItem.FavoriteCar = "0";
                            CustomMessageBox.Show("Уведомление", $"Автомобиль успешно удален из избранных автомобилей.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                        }
                        else
                        {
                            CustomMessageBox.Show("Уведомление", $"Автомобиль не удален из избранных автомобилей.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
        public void DeleteButton_Click(object sender, EventArgs e)
        {
            if (sender is CarItem carItem)
            {
                var result = CustomMessageBox.Show("Уведомление", $"Желаете удалить данный автомобиль и поездки совершенные на нем?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (db.DeleteCar(Convert.ToInt32(carItem.Id)))
                    {
                        CustomMessageBox.Show("Уведомление", $"Автомобиль успешно удален.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                        List<Car> allCar = db.GetAllCars(IdUser);
                        AddCarInPanel(allCar);
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Автомобиль не удален удален.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                    }
                }
            }
        }
        public void GridButton_Click(object sender, EventArgs e)
        {
            if (sender is CarItem carItem)
            {
                CarInfoPage carInfoPage = new CarInfoPage(IdUser,Convert.ToInt32(carItem.Id));
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null && parentWindow is MainWindow mainWindow && mainWindow.fContainer != null)
                {
                    mainWindow.fContainer.Content = carInfoPage;
                }
            }
        }
    }
}
