using OneWay.Class;
using OneWay.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OneWay.Pages
{
    /// <summary>
    /// Логика взаимодействия для CarInfoPage.xaml
    /// </summary>
    public partial class CarInfoPage : Page
    {
        DataBase db = new DataBase("OneWayDB.db");
        Car selectCar = new Car();
        public int IdCar;
        public int IdUser;
        public CarInfoPage(int IdUser,int IdCar)
        {
            InitializeComponent();
            this.IdCar = IdCar;
            this.IdUser = IdUser;
            selectCar = db.GetCar(IdCar);
            СompletionFill(selectCar);
            if(db.CheckFavoriteCar(IdUser, IdCar) == 1)
            {
                this.FavoriteButton.Content = "Удалить из избранных авто";
            }
            else
            {
                this.FavoriteButton.Content = "Добавить в избранные авто";

            }
        }
        void СompletionFill(Car selectCar)
        {
            this.CarBrand.Text = selectCar.Brand;
            this.CarModel.Text = selectCar.Model;
            this.CarGeneration.Text = selectCar.Generation;
            this.CarEquipment.Text = selectCar.Equipment;
            this.Year.Text = selectCar.Year;
            this.Body.Text = selectCar.Body;
            this.Drive.Text = selectCar.Drive;
            this.GearBox.Text = selectCar.GearBox;
            this.Seats.Text = selectCar.Seats.ToString();
            this.Doors.Text = selectCar.Doors.ToString();
            this.EngineType.Text = selectCar.EngineType;
            this.MaxSpeed.Text = selectCar.MaxSpeed.ToString();
            this.Conditioner.Text = selectCar.Conditioner;
            if(selectCar.EngineType == "Электрический")
            {
                this.Volume.Visibility = Visibility.Collapsed;
                this.TextVolume.Visibility = Visibility.Collapsed;
                this.Fuel.Text = selectCar.Fuel;

                this.TextFuelConsumption.Visibility = Visibility.Collapsed;
                this.FuelConsumption.Visibility = Visibility.Collapsed;

                this.TextOctaneNumber.Visibility = Visibility.Collapsed;
                this.OctaneNumber.Visibility = Visibility.Collapsed;


                this.BatteryCapacity.Text = selectCar.BatteryCapacity.ToString();
                this.TextBlockBatteryCapacity.Visibility = Visibility.Visible;

                this.BatteryCapacity.Visibility = Visibility.Visible;
                this.RangePerCharge.Text = selectCar.RangePerCharge.ToString();

                this.TextBlockRangePerCharge.Visibility = Visibility.Visible;
                this.BatteryCapacity.Visibility = Visibility.Visible;
            }
            else if(selectCar.EngineType == "Гибрид")
            {
                this.Volume.Text = selectCar.Volume.ToString();
                this.Volume.Visibility = Visibility.Visible;
                this.TextVolume.Visibility = Visibility.Visible;

                this.Fuel.Text = selectCar.Fuel;
                this.TextFuelConsumption.Visibility = Visibility.Collapsed;
                this.FuelConsumption.Visibility = Visibility.Collapsed;

                if(selectCar.FuelOctane != 0)
                {
                    this.OctaneNumber.Text = db.GetFuel(Convert.ToInt32(selectCar.FuelOctane));
                    this.OctaneNumber.Visibility = Visibility.Visible;
                    this.TextOctaneNumber.Visibility = Visibility.Visible;
                }
                else
                {
                    this.OctaneNumber.Visibility = Visibility.Collapsed;
                    this.TextOctaneNumber.Visibility = Visibility.Collapsed;
                }

                if(selectCar.BatteryCapacity != null)
                {
                    this.TextBlockBatteryCapacity.Visibility = Visibility.Visible;
                    this.BatteryCapacity.Visibility = Visibility.Visible;
                }
                else
                {
                    this.TextBlockBatteryCapacity.Visibility = Visibility.Collapsed;
                    this.BatteryCapacity.Visibility = Visibility.Collapsed;
                }
                if(selectCar.RangePerCharge != null)
                {
                    this.TextBlockRangePerCharge.Visibility = Visibility.Visible;
                    this.RangePerCharge.Visibility = Visibility.Visible;
                }
                else
                {
                    this.TextBlockRangePerCharge.Visibility = Visibility.Collapsed;
                    this.RangePerCharge.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                this.Volume.Text = selectCar.Volume.ToString(); 
                this.Volume.Visibility = Visibility.Visible;
                this.TextVolume.Visibility = Visibility.Visible;

                this.FuelConsumption.Text = selectCar.FuelConsumption.ToString();
                this.TextFuelConsumption.Visibility = Visibility.Visible;
                this.FuelConsumption.Visibility = Visibility.Visible;

                this.Fuel.Text = selectCar.Fuel;

                if (selectCar.FuelOctane != 0)
                {
                    this.OctaneNumber.Text = db.GetFuel(Convert.ToInt32(selectCar.FuelOctane));
                    this.OctaneNumber.Visibility = Visibility.Visible;
                    this.TextOctaneNumber.Visibility = Visibility.Visible;
                }
                else
                {
                    this.OctaneNumber.Visibility = Visibility.Collapsed;
                    this.TextOctaneNumber.Visibility = Visibility.Collapsed;
                }
                this.TextBlockBatteryCapacity.Visibility = Visibility.Collapsed;
                this.BatteryCapacity.Visibility = Visibility.Collapsed;
                this.TextBlockRangePerCharge.Visibility = Visibility.Collapsed;
                this.BatteryCapacity.Visibility = Visibility.Collapsed;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomMessageBox.Show("Уведомление", $"Желаете удалить данный автомобиль и поездки совершенные на нем?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (db.DeleteCar(IdCar))
                {
                    CustomMessageBox.Show("Уведомление", $"Автомобиль успешно удален.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    AllCarsPage carPage = new AllCarsPage(IdUser);
                    Window parentWindow = Window.GetWindow(this);
                    if (parentWindow != null && parentWindow is MainWindow mainWindow && mainWindow.fContainer != null)
                    {
                        mainWindow.fContainer.Content = carPage;
                    }
                }
                else
                {
                    CustomMessageBox.Show("Уведомление", $"Автомобиль не удален удален.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                }
            }
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (db.CheckFavoriteCar(IdUser,IdCar) == 0) // проверка добавлени ли автмобиль в избранное. если нет то
            {
                var result = CustomMessageBox.Show("Уведомление", $"Желаете добавить автомобиль в избранные автомобили?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (db.AddFavoriteCar(IdUser, IdCar))
                    {
                        this.FavoriteButton.Content = "Удалить из избранных авто";
                        CustomMessageBox.Show("Уведомление", $"Автомобиль успешно добавлен в избранные автомобили.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Автомобиль не был добавлен в избранные автомобили.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                    }
                }
            }
            else //если есть то удалить
            {
                var result = CustomMessageBox.Show("Уведомление", $"Желаете удалить автомобиль из избранных автомобилей?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (db.DeleteFavoriteCar(IdUser, IdCar))
                    {
                        this.FavoriteButton.Content = "Добавить в избранные авто";
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
}
