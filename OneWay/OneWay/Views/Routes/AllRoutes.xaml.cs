using OneWay.Class;
using OneWay.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace OneWay.Pages
{
    public partial class AllRoutes : Page
    {
        public int IdUser;
        DataBase db = new DataBase("OneWayDB.db");
        List<Class.Route> allRoute = new List<Class.Route>();

        public AllRoutes(int Iduser)
        {
            InitializeComponent();
            this.IdUser = Iduser;
            allRoute = db.GetAllRoutes(this.IdUser);
            CreateItem();
        }
        private void CreateItem()
        {
            RoutePanel.Children.Clear();
            foreach (var item in allRoute)
            {
                var info = db.GetRouteInfoById(item.IdRoute);
                string startPoint = info.Item1;
                string finishPoint = info.Item2;
                double distance = info.Item3;
                string travelTime = info.Item4;
                string points = info.Item5;
                RouteItem route = new RouteItem();
                route.Id = item.IdRoute.ToString();
                route.TripName = $"{startPoint} - {finishPoint}";
                route.Distance = distance + " км";
                TimeSpan date = TimeSpan.FromMinutes(Convert.ToDouble(travelTime));
                if (date.Days >= 1)
                {
                    if (date.Hours == 0)
                    {
                        route.Time = $"{date.Days} д {date.Minutes} мин";
                    }
                    else
                    {
                        route.Time = $"{date.Days} д {date.Hours} ч {date.Minutes} мин";
                    }
                }
                else
                {
                    if (date.Hours != 0)
                    {
                        route.Time = $"{date.Hours} ч {date.Minutes} мин";
                    }
                    else
                    {
                        route.Time = $"{date.Minutes} мин";
                    }
                }
                string formattedPoints = "";
                if (!string.IsNullOrEmpty(points))
                {
                    string[] point = points.Split('#');
                    formattedPoints += string.Join("\n", point);
                }
                route.Point = formattedPoints;
                route.FavoriteRoute = db.CheckFavoriteRoute(IdUser, item.IdRoute).ToString();
                route.InfoButtonClicked += Route_InfoButtonClicked;
                route.DeleteButtonClicked += Route_DeleteButtonClicked;
                route.FavoriteIconClicked += Route_FavoriteButtonClicked;
                RoutePanel.Children.Add(route);
            }
        }
        private void Route_FavoriteButtonClicked(object sender, EventArgs e)
        {
            if (sender is RouteItem routeItem)
            {
                if (routeItem.FavoriteRoute == "0")
                {
                    var result = CustomMessageBox.Show("Уведомление", $"Желаете добавить маршрут в избранные маршруты?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (db.AddFavoriteRoute(IdUser, Convert.ToInt32(routeItem.Id)))
                        {
                            routeItem.FavoriteRoute = "1";
                            CustomMessageBox.Show("Уведомление", $"Маршрут успешно добавлен в избранные маршруты.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                        }
                        else
                        {
                            CustomMessageBox.Show("Уведомление", $"Маршрут не был добавлен в избранные автомобили.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    var result = CustomMessageBox.Show("Уведомление", $"Желаете удалить маршрут из избранных маршрутов?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (db.DeleteFavoriteRoute(IdUser, Convert.ToInt32(routeItem.Id)))
                        {
                            routeItem.FavoriteRoute = "0";
                            CustomMessageBox.Show("Уведомление", $"Маршрут успешно удален из избранных маршрутов.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                        }
                        else
                        {
                            CustomMessageBox.Show("Уведомление", $"Маршрут не удален из избранных маршрутов.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                        }
                    }
                }

            }
        }
        private void Route_InfoButtonClicked(object sender, EventArgs e)
        {
            if (sender is RouteItem routeItem)
            {
                var messageBoxResult = CustomMessageBox.Show("Уведомление", $"Желаете посмотреть информаци о маршруте?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Information);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    RouteInfoPage routePage = new RouteInfoPage(IdUser, Convert.ToInt32(routeItem.Id));
                    Window parentWindow = Window.GetWindow(this);
                    if (parentWindow != null && parentWindow is MainWindow mainWindow && mainWindow.fContainer != null)
                    {
                        mainWindow.fContainer.Content = routePage;
                    }
                }
            }
        }

        private void Route_DeleteButtonClicked(object sender, EventArgs e)
        {
            var quequestion = CustomMessageBox.Show("Уведомление", $"Желаете удалить данный маршрут и удалить все совершенные поездки по данному маршруту.", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Information);
            if (quequestion == MessageBoxResult.Yes)
            {
                if (sender is RouteItem routeItem)
                {
                    int id = Convert.ToInt32(routeItem.Id);
                    if (db.DeleteRoute(id))
                    {
                        CustomMessageBox.Show("Уведомление", $"Данный маршрут был удален.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                        allRoute = db.GetAllRoutes(this.IdUser);
                        CreateItem();
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Данная поездка не была удалена из истории.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                    }
                }
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            StartPoint.Text = "";
            FinishPoint.Text = "";
        }

        private void AddRouteButton_Click(object sender, RoutedEventArgs e)
        {
            AddRoutePage addCarPage = new AddRoutePage(IdUser);
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null && parentWindow is MainWindow mainWindow && mainWindow.fContainer != null)
            {
                mainWindow.fContainer.Content = addCarPage;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(StartPoint.Text) | !string.IsNullOrEmpty(FinishPoint.Text))
            { 
                int startId = db.GetCityIdByName(IdUser, StartPoint.Text);
                int finishId = db.GetCityIdByName(IdUser, FinishPoint.Text);
                if(startId == -1 & !string.IsNullOrWhiteSpace(StartPoint.Text))
                {
                    CustomMessageBox.Show("Уведомление", "Точка отправления не найдена. Проверьте название и повторите попытку", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                    return;
                }
                if (finishId == -1 & !string.IsNullOrWhiteSpace(FinishPoint.Text))
                {
                    CustomMessageBox.Show("Уведомление", "Точка прибытия не найдена. Проверьте название и повторите попытку", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                    return;
                }
                allRoute = db.SelectRoutesByPoints(IdUser, startId, finishId);
                CreateItem();
            }
            else
            {
                CustomMessageBox.Show("Уведомление", "Отображены все маршруты.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                allRoute = db.GetAllRoutes(this.IdUser);
                CreateItem();
            }
        }
    }
}
