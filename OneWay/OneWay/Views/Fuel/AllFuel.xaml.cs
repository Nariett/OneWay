using OneWay.Class;
using OneWay.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace OneWay.Pages
{
    /// <summary>
    /// Логика взаимодействия для Fuel.xaml
    /// </summary>
    public partial class AllFuel : Page
    {
        public int IdUser;
        DataBase db = new DataBase("OneWayDB.db");
        public AllFuel(int idUser)
        {
            InitializeComponent();
            IdUser = idUser;
            info.Text = $"Данные взяты c сайта Белорусьнефть на {db.GetLastDate()}";
        }

        private void AcceptFilter_Click(object sender, RoutedEventArgs e)
        {
            List<string> selectedFuelTypes = new List<string>();
            if (Gasoline.IsChecked == true)
            {
                selectedFuelTypes.Add("Бензин");
            }
            if (Diesel.IsChecked == true)
            {
                selectedFuelTypes.Add("Дизельное топливо");
            }
            if (Electro.IsChecked == true)
            {
                selectedFuelTypes.Add("Электричество");
            }
            FuelPanel.Children.Clear();
            DataBase db = new DataBase("OneWayDB.db");
            List<Fuel> allFuel = db.GetFilterFuelTypes(selectedFuelTypes);
            foreach (var item in allFuel)
            {
                FuelItem fuelItem = new FuelItem();
                fuelItem.FuelType = item.FuelType;
                fuelItem.OctanNumber = item.OctaneNumber;
                fuelItem.PriceRub = item.Price.ToString() + " руб";
                FuelPanel.Children.Add(fuelItem);
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataBase db = new DataBase("OneWayDB.db");
            List<Fuel> allFuel = db.GetFuelTypes(IdUser);
            foreach(var item in  allFuel)
            {
                FuelItem fuelItem = new FuelItem();
                fuelItem.FuelType = item.FuelType;
                fuelItem.OctanNumber = item.OctaneNumber;
                fuelItem.PriceRub = item.Price.ToString() + " руб";
                fuelItem.IdUser = IdUser.ToString();
                fuelItem.GridClicked += Grid_Click;
                FuelPanel.Children.Add(fuelItem);
            }
        }
        public void Grid_Click(object sender, EventArgs e)
        {
            if (sender is FuelItem fuelItem)
            {
                FuelPriceEditPage fuelEditPage = new FuelPriceEditPage(IdUser, db.GetFuelId(fuelItem.OctanNumber));
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null && parentWindow is MainWindow mainWindow && mainWindow.fContainer != null)
                {
                    mainWindow.fContainer.Content = fuelEditPage;
                }
            }
        }
        private void TextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start($"https://azs.belorusneft.by/sitebeloil/ru/center/azs/center/fuelandService/price/");
        }
    }
}
