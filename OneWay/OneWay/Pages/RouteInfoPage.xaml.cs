using OneWay.Class;
using OneWay.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OneWay.Pages
{
    /// <summary>
    /// Логика взаимодействия для RouteInfoPage.xaml
    /// </summary>
    public partial class RouteInfoPage : Page
    {
        int IdUser;
        int IdRoute;
        DataBase db = new DataBase("OneWayDB.db");
        public (string, string, double, string, string) route;
        public RouteInfoPage(int iduser, int idRoute)
        {
            InitializeComponent();
            this.IdUser = iduser;
            this.IdRoute = idRoute;
            if(db.CheckFavoriteRoute(IdUser, IdRoute) == 1)
            {
                this.FavoriteButton.Content = "Удалить из избранных";
            }
            else
            {
                this.FavoriteButton.Content = "Добавить в избранные";

            }
            route = db.GetRouteInfoById(iduser);
            StartPoint.Text = route.Item1;
            FinishPoint.Text = route.Item2;
            Distance.Text = route.Item3.ToString() + " км";
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
            string formattedPoints = "";
            if (!string.IsNullOrEmpty(route.Item5))
            {
                AdditionalPoint.Visibility = Visibility.Visible;
                AdditionalPointText.Visibility = Visibility.Visible;
                string[] point = route.Item5.Split('#');
                formattedPoints += string.Join("\n", point);
                AdditionalPoint.Text = formattedPoints;
            }
            else
            {
                AdditionalPoint.Visibility = Visibility.Collapsed;
                AdditionalPointText.Visibility = Visibility.Collapsed;
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomMessageBox.Show("Уведомление", $"Желаете удалить данный маршрут и удалить все совершенные поездки по данному маршруту.", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                if (db.DeleteRoute(IdRoute))
                {
                    CustomMessageBox.Show("Уведомление", $"Маршрут был успешно удален.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    AllCarsPage carPage = new AllCarsPage(IdUser);
                    Window parentWindow = Window.GetWindow(this);
                    if (parentWindow != null && parentWindow is MainWindow mainWindow && mainWindow.fContainer != null)
                    {
                        mainWindow.fContainer.Content = carPage;
                    }
                }
                else
                {
                    CustomMessageBox.Show("Уведомление", $"Маршрут не удален.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                }
            }
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (db.CheckFavoriteRoute(IdUser,IdRoute) == 0) // проверка наличия маршрута в избранном
            {
                var result = CustomMessageBox.Show("Уведомление", $"Желаете добавить маршрут в избранные маршруты?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (db.AddFavoriteRoute(IdUser, IdRoute))
                    {
                        this.FavoriteButton.Content = "Удалить из избранных";
                        CustomMessageBox.Show("Уведомление", $"Маршрут успешно добавлен в избранные маршруты.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Маршрут не был добавлен в избранные маршруты.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                var result = CustomMessageBox.Show("Уведомление", $"Желаете удалить маршрут из избранных машрутов?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (db.DeleteFavoriteRoute(IdUser, IdRoute))
                    {
                        this.FavoriteButton.Content = "Добавить в избранные";
                        CustomMessageBox.Show("Уведомление", $"Машрут успешно удален из избранных маршрутов.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Маршрут не удален из избранных маршрутов.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
