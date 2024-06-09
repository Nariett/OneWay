using OneWay.Class;
using OneWay.Controls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace OneWay.Pages
{
    public partial class FuelPriceEditPage : Page
    {
        DataBase db = new DataBase("OneWayDB.db");
        int IdUser;
        int IdFuel;
        public FuelPriceEditPage(int idUser, int idFuel)
        {
            InitializeComponent();
            IdUser = idUser;
            IdFuel = idFuel;
            FuelInfo.Text = $"Изменение цены на {db.GetFuelName(IdFuel)}";
            if(IdFuel != 7 && IdFuel != 8)
            {
                FuelPriceText.Text = $"Цена за литр, руб";
            }
            else
            {
                FuelPriceText.Text = $"Цена за кВт*ч, руб";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FuelPrice.Text))
            {
                CustomMessageBox.Show("Уведомление", $"Пожалуйста, заполните все обязательные поля.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                return;
            }
            double fuelPrice = 0;
            if (!double.TryParse(FuelPrice.Text, out fuelPrice) || fuelPrice <= 0)
            {
                CustomMessageBox.Show("Предупреждение", $"Пожалуйста, введите корректное значение для цены топлива в формате (x,xx).", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                return;
            }
            string formattedFuelConsumption = fuelPrice.ToString("0.00", CultureInfo.InvariantCulture);
            string date = DateTime.Now.ToString("dd.MM.yyyy");
            if (db.UpdatePrice(IdFuel, IdUser, formattedFuelConsumption, date)) // добавление в цены в базу данных
            {
                CustomMessageBox.Show("Уведомление", $"Цена на топливо была обновлена.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
            }
            else
            {
                CustomMessageBox.Show("Уведомление", $"Цена на топливо не была обновлена.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
            }
        }
    }
}
