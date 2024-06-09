using HtmlAgilityPack;
using OneWay.Class;
using OneWay.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace OneWay
{
    public partial class MainWindow : Window
    {
        public int IdUser;
        DataBase db = new DataBase("OneWayDB.db");

        public MainWindow()
        {
            InitializeComponent();
            Tg_Btn.Visibility = Visibility.Collapsed;
            ItemPanel.Visibility = Visibility.Collapsed;

            AuthenticationPage authenticationPage = new AuthenticationPage();
            authenticationPage.AuthenticationCompleted += (sender, IdUser) =>
            {
                this.IdUser = IdUser;
                if (IdUser != 0)
                {
                    Tg_Btn.Visibility = Visibility.Visible;
                    ItemPanel.Visibility = Visibility.Visible;
                    Home HomePage = new Home(IdUser);
                    fContainer.Navigate(HomePage);
                }

            };

            fContainer.Navigate(authenticationPage);
            UpdateFuelPrice();
        }
        public async void UpdateFuelPrice()
        {
            DateTime currentDate = DateTime.Now.Date;
            DateTime lastUpdateDate = DateTime.ParseExact(db.GetLastDate(), "dd.MM.yyyy", null);
            if (lastUpdateDate != currentDate)
            {
                bool interner = await CheckInternetConnectionAsync();
                if (interner)
                {
                    List<string> prices = GetFuelPrices();
                    int[] idFuel = new int[] { 9, 4, 5, 6, 1, 2, 3 };
                    string date = DateTime.Now.ToString("dd.MM.yyyy");
                    for (int i = 0; i < prices.Count - 1; i++)
                    {
                        db.UpdatePrice(idFuel[i], -1, prices[i], date);
                    }
                }
            }
        }
        public async Task<bool> CheckInternetConnectionAsync()
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = await ping.SendPingAsync("www.google.com");

                return reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
        }
        public List<string> GetFuelPrices()
        {
            List<string> prices = new List<string>();
            string web = "https://azs.belorusneft.by/sitebeloil/ru/center/azs/center/fuelandService/price/";
            try
            {
                HtmlWeb webGet = new HtmlWeb();
                HtmlDocument document = webGet.Load(web);

                HtmlNode fuelNode = document.DocumentNode.SelectSingleNode("//div[@class='b-left__informer']");
                if (fuelNode != null)
                {
                    HtmlNodeCollection tdNodes = fuelNode.SelectNodes(".//td");
                    if (tdNodes != null)
                    {
                        foreach (HtmlNode tdNode in tdNodes)
                        {
                            prices.Add(tdNode.InnerText);
                        }
                    }
                }
                prices.Add(DateTime.Now.ToString("f"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }

            return prices;
        }
        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void btnHome_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnHome;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Главная";
            }
        }

        private void btnHome_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnDashboard_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnDashboard;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Статистика";
            }
        }

        private void btnDashboard_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }
        private void btnTrip_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnTrip;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Поездка";
            }
        }

        private void btnTrip_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }
        private void btnRoute_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnRoute;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Маршрут";
            }
        }

        private void btnRoute_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }
        private void btnHistory_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnHistory;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "История";
            }
        }

        private void btnHistory_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnFuel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnFuel;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Топливо";
            }
        }

        private void btnFuel_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnCar_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnCar;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Транспорт";
            }
        }

        private void btnCar_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnReport_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnReport;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Отчет";
            }
        }

        private void btnReport_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnHelp_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnHelp;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Справка";
            }
        }

        private void btnHelp_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Home HomePage = new Home(IdUser);
            fContainer.Navigate(HomePage);
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            Dashboard DashboardPage = new Dashboard(IdUser);
            fContainer.Navigate(DashboardPage);
        }
        private void btnTrip_Click(object sender, RoutedEventArgs e)
        {
            AddTripPage TripPage = new AddTripPage(IdUser);
            fContainer.Navigate(TripPage);
        }
        private void btnRoute_Click(object sender, RoutedEventArgs e)
        {
            AllRoutes RoutePage = new AllRoutes(IdUser);
            fContainer.Navigate(RoutePage);
        }
        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            History HistoryPage = new History(IdUser);
            fContainer.Navigate(HistoryPage);
        }
        private void btnCars_Click(object sender, RoutedEventArgs e)
        {
            AllCarsPage CarsPage = new AllCarsPage(IdUser);
            fContainer.Navigate(CarsPage);
        }

        private void btnFuel_Click(object sender, RoutedEventArgs e)
        {
            AllFuel FuelPage = new AllFuel(IdUser);
            fContainer.Navigate(FuelPage);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            Report ReportPage = new Report(IdUser);
            fContainer.Navigate(ReportPage);
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("Help.chm");
        }
    }
}
