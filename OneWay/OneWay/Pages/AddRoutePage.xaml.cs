using MahApps.Metro.IconPacks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using OneWay.Controls;
using OneWay.Class;
using System.Net.NetworkInformation;

namespace OneWay.Pages
{
    /// <summary>
    /// Логика взаимодействия для Route.xaml
    /// </summary>
    public partial class AddRoutePage : Page
    {
        public static string selectToken;
        public static string x = null;
        public static string y = null;
        public int fill = 0;
        int IdUser;
        DataBase db = new DataBase("OneWayDB.db");
        Route selectRoute = new Route();
        List<string> pointCity = new List<string>();
        List<string> additionalPoints = new List<string>();
        List<(double distance, int time)> info = new List<(double distance, int time)>();
        public static List<Tuple<string, string>> AllCity = new List<Tuple<string, string>>();
        public AddRoutePage(int idUser)
        {
            InitializeComponent();
            Fill.SelectedIndex = 0;
            IdUser = idUser;
        }
        private async void StartPoint_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Fill.SelectedIndex != 0)
            {
                comboBoxStartPoint.Items.Clear();
                comboBoxStartPoint.SelectedIndex = -1;
                if (StartPoint.Text.Length != 0)
                {
                    List<Tuple<string, string>> route = await GetCityAsync(StartPoint.Text);
                    AllCity.AddRange(route);
                    foreach (var item in route)
                    {
                        comboBoxStartPoint.Items.Add(item.Item1);
                    }
                }
            }
        }
        private async void FinishPoint_LostFocus(object sender, RoutedEventArgs e)
        {
            if(Fill.SelectedIndex != 0)
            {
                comboBoxLastPointInfo.Items.Clear();
                comboBoxLastPointInfo.SelectedIndex = -1;
                if (FinishPoint.Text.Length != 0)
                {
                    List<Tuple<string, string>> route = await GetCityAsync(FinishPoint.Text);
                    AllCity.AddRange(route);
                    foreach (var item in route)
                    {
                        comboBoxLastPointInfo.Items.Add(item.Item1);
                    }
                }
            }
        }
        public async Task<List<Tuple<string, string>>> GetCityAsync(string city)
        {
            string url = $"https://geocode-maps.yandex.ru/1.x/?apikey=810e6f73-e4e6-4d63-850a-3a87c1c662ef&geocode={city}&format=json";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        // Получаем содержимое ответа в виде строки
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        JObject jsonObject = JObject.Parse(jsonResponse);
                        List<Tuple<string, string>> posKeys = GetPosKeys(jsonObject);
                        if (posKeys.Count == 0)
                        {
                            CustomMessageBox.Show("Уведомление", $"Введенный город не найден. Повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                            return new List<Tuple<string, string>>();
                        }
                        return posKeys;
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Ошибка: {response.StatusCode}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show("Уведомление", $"Ошибка: {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                }
            }
            return new List<Tuple<string, string>>();
        }
        public static List<Tuple<string, string>> GetPosKeys(JToken token)
        {
            List<string> posKeys = new List<string>();
            List<Tuple<string, string>> data = new List<Tuple<string, string>>();

            if (token.Type == JTokenType.Object)
            {
                foreach (JProperty property in token.Children<JProperty>())
                {
                    JToken currentObject = property.Parent;
                    if (property.Name == "text" && (property.Parent?.SelectToken("kind")?.Value<string>() == "locality" || property.Parent?.SelectToken("kind")?.Value<string>() == "province" || property.Parent?.SelectToken("kind")?.Value<string>() == "house"))
                    {
                        x = property.Value.ToString();
                        selectToken = GetParentPathUntil(currentObject, "GeoObject");
                    }

                    if (property.Name == "pos")
                    {
                        string objectName = GetParentPathUntil(currentObject, "GeoObject");
                        if (selectToken == objectName)
                        {
                            y = ReverseString(property.Value.ToString());
                        }
                    }

                    data.AddRange(GetPosKeys(property.Value));

                    if (!string.IsNullOrEmpty(x) && !string.IsNullOrEmpty(y))
                    {
                        data.Add(Tuple.Create(x, y));
                        x = null;
                        y = null;
                    }
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                foreach (JToken arrayItem in token.Children())
                {
                    data.AddRange(GetPosKeys(arrayItem));
                }
            }
            return data;
        }
        private static string GetParentPathUntil(JToken token, string stopAt)
        {
            var path = token.Path;
            var index = path.LastIndexOf(stopAt, StringComparison.Ordinal);
            return index >= 0 ? path.Substring(0, index + stopAt.Length) : path;
        }
        public static string ReverseString(string input)
        {
            string[] parts = input.Split(' ');
            return $"{parts[1]} {parts[0]}";
        }

        private void CreatePointButton_Click(object sender, RoutedEventArgs e)
        {
            AddPoint();
        }

        private void AddPoint()
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(155) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });

            TextBlock textCount = new TextBlock()
            {
                Text = "Новая точка",
                Width = 130,
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
                FontSize = 20,
                Foreground = (Brush)FindResource("TextSecundaryColor"),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0, 0, 0, 1),
                Margin = new Thickness(5, 0, 5, 0),
                VerticalAlignment = VerticalAlignment.Center,
            };
            if(fill == 1)
            {
                newTextBox.LostFocus += Point_LostFocus;
            }
            Grid.SetColumn(newTextBox, 1);
            if(fill == 1 )
            {
                ComboBox comboBox = new ComboBox()
                {
                    Height = 30,
                    FontSize = 18,
                    Margin = new Thickness(5, 0, 10, 0),
                };
                Grid.SetColumn(comboBox, 2);
                grid.Children.Add(comboBox);
            }
            Button deleteButton = new Button();
            deleteButton.Width = 42;
            deleteButton.Height = 34;
            deleteButton.Margin = new Thickness(10);
            deleteButton.BorderThickness = new Thickness(0);
            deleteButton.Click += DeleteButton_Click;

            ControlTemplate buttonTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));

            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.BackgroundProperty, Brushes.Red);
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));

            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenterFactory.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);

            borderFactory.AppendChild(contentPresenterFactory);

            FrameworkElementFactory iconFactory = new FrameworkElementFactory(typeof(PackIconUnicons));
            iconFactory.SetValue(PackIconUnicons.KindProperty, PackIconUniconsKind.Times);
            iconFactory.SetValue(Control.ForegroundProperty, (Brush)FindResource("TextSecundaryColor"));
            iconFactory.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
            iconFactory.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);

            gridFactory.AppendChild(borderFactory);
            gridFactory.AppendChild(iconFactory);

            buttonTemplate.VisualTree = gridFactory;
            deleteButton.Template = buttonTemplate;
            deleteButton.Click += DeleteButton_Click;
            Grid.SetColumn(deleteButton, 3);

            grid.Children.Add(textCount);
            grid.Children.Add(newTextBox);
            
            grid.Children.Add(deleteButton);

            Points.Children.Add(grid);
        }

        private async void Point_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                Grid parentPanel = textBox.Parent as Grid;
                if (parentPanel != null)
                {
                    int index = parentPanel.Children.IndexOf(textBox); 
                    if (index < parentPanel.Children.Count - 1) 
                    {
                        ComboBox comboBox = parentPanel.Children[0] as ComboBox;
                        if (comboBox != null)
                        {
                            comboBox.Items.Clear();
                            if (textBox.Text.Length != 0)
                            {
                                List<Tuple<string, string>> route = await GetCityAsync(textBox.Text);
                                AllCity.AddRange(route);
                                foreach (var item in route)
                                {
                                    comboBox.Items.Add(item.Item1);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                Grid parentPanel = button.Parent as Grid;
                if (parentPanel != null)
                {
                    Points.Children.Remove(parentPanel);
                }
            }
        }

        private async void CreateRouteButton_Click(object sender, RoutedEventArgs e)
        {
            if(Fill.SelectedIndex == 1) // проверка типа заполения автоматический
            {
                additionalPoints = new List<string>();
                if (comboBoxStartPoint.SelectedIndex != -1 || comboBoxLastPointInfo.SelectedIndex != -1)
                {
                    string startPoint = this.comboBoxStartPoint.SelectedItem?.ToString();
                    string lastPoint = this.comboBoxLastPointInfo.SelectedItem?.ToString();
                    string points = "";
                    additionalPoints.Add(startPoint);
                    foreach (var item in Points.Children) // цикл для получения всех дополниетельных точек
                    {
                        if (item is Grid)
                        {
                            Grid block = item as Grid;
                            foreach (var element in block.Children)
                            {
                                if (element is ComboBox)
                                {
                                    ComboBox comboBox = element as ComboBox;
                                    if (comboBox.SelectedItem != null)
                                    {
                                        additionalPoints.Add(comboBox.SelectedItem.ToString());
                                        pointCity.Add(comboBox.SelectedItem.ToString().Split(' ')[comboBox.SelectedItem.ToString().Split(' ').Length -1]);
                                    }
                                    else
                                    {
                                        CustomMessageBox.Show("Уведомление", $"Заполните все поля или удалите ненужные и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    additionalPoints.Add(lastPoint);
                    if (!string.IsNullOrEmpty(startPoint) && !string.IsNullOrEmpty(lastPoint))
                    {
                        foreach (var item in additionalPoints)
                        {
                            if (!string.IsNullOrEmpty(item.ToString()))
                            {
                                points += $"point={AllCity.FirstOrDefault(x => x.Item1 == item)?.Item2.Replace(' ', ',')}&"; // создание строки с координатами для api
                            }
                            else
                            {
                                CustomMessageBox.Show("Уведомление", $"Заполните все поля или удалите ненужные и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                                return;
                            }
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Заполните все поля или удалите ненужные и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                        return;
                    }
                    var client = new HttpClient();
                    HttpResponseMessage request;
                    if (points.Split(',').Length == 3) // проверка на кол-во точек в маршруте
                    {
                        request = await client.GetAsync($"https://graphhopper.com/api/1/route?{points}profile=car&calc_points=false&algorithm=alternative_route&key=7bd83501-9f94-4196-bc57-9af92a81344d");
                    }
                    else
                    {
                        request = await client.GetAsync($"https://graphhopper.com/api/1/route?{points}profile=car&calc_points=false&key=7bd83501-9f94-4196-bc57-9af92a81344d");
                    }
                    var responseContent = await request.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseContent);
                    var paths = jsonResponse["paths"];
                    info = new List<(double distance, int time)>();
                    if(paths != null)
                    {
                        foreach (var path in paths)
                        {
                            var distanceInKm = Math.Round((double)path["distance"] / 1000, 2);
                            var timeInMinutes = (int)path["time"] / 60000;
                            info.Add((distanceInKm, timeInMinutes));
                        }
                        CreateTripItem(info);
                        CustomMessageBox.Show("Уведомление", $"Выберите маршрут в правой колонке для просмотра информации о выбранном маршруте.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Данный машрут не найден. Выберите другие города и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);          }
                }
                else
                {
                    CustomMessageBox.Show("Уведомление", $"Заполните все поля и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                }
            }
            else // ручно ввод данных
            {   

                if (StartPoint.Text != null || FinishPoint.Text != null || Distance.Text != null)
                {
                    additionalPoints = new List<string>();
                    string startPoint = StartPoint.Text;
                    string lastPoint = FinishPoint.Text;
                    Class.Route newRoute = new Class.Route();
                    db.InsertPoint(startPoint, "", IdUser);
                    db.InsertPoint(lastPoint, "", IdUser);
                    newRoute.Name = $"{startPoint} - {lastPoint}";
                    newRoute.PointOne = db.GetCityIdByName(IdUser,startPoint);
                    newRoute.PointTwo = db.GetCityIdByName(IdUser,lastPoint);
                    newRoute.AdditionPoint = "";
                    foreach (var item in Points.Children)
                    {
                        if (item is Grid)
                        {
                            Grid block = item as Grid;
                            foreach (var element in block.Children)
                            {
                                if (element is TextBox)
                                {
                                    TextBox TextBox = element as TextBox;
                                    if (TextBox.Text != null)
                                    {
                                        additionalPoints.Add(TextBox.Text.ToString());
                                    }
                                    else
                                    {
                                        CustomMessageBox.Show("Уведомление", $"Заполните все поля или удалите ненужные и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    newRoute.AdditionPoint = string.Join("#", pointCity);
                    double distance = 0;
                    if (!double.TryParse(Distance.Text, out distance) || distance <= 0)
                    {
                        CustomMessageBox.Show( "Предупреждение", "Пожалуйста, введите корректное расстояние.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                        return;
                    }
                    newRoute.Distance = distance;
                    newRoute.TravelTime = Math.Round(distance / 100 * 60,0).ToString();
                    newRoute.IdUser = IdUser;
                    if(db.InsertRoute(newRoute))
                    {
                        CustomMessageBox.Show("Уведомление", $"Маршрут {newRoute.Name} добавлен в систему.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Маршрут {newRoute.Name} не добавлен в систему.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Warning);
                    }

                }
                else
                {
                    CustomMessageBox.Show("Уведомление", $"Заполните все поля и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                }
            }
        }
        public void CreateTripItem(List<(double distance, int time)> values)
        {
            TripInfo.Children.Clear();
            int count = 1;
            foreach (var value in values)
            {
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
                Grid.SetRow(textBlock1, 0);
                TimeSpan date = TimeSpan.FromMinutes(value.time);
                if(date.Days >= 1)
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
                else if(date.Hours != 0)
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
                    Text = $"{value.distance} км",
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
        private void CreateTripClick(object sender, MouseButtonEventArgs e)
        {
            bool flagOne = false;
            bool flagTwo = false;
            Border clickedBorder = sender as Border;
            if (clickedBorder != null)
            {
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
                        var result = CustomMessageBox.Show("Уведомление", $"Вы выбрали маршрут:\nНазвание: {routeName}\nДистанция: {distance}\nВремя: {time}\nЖелаете сохранить данный маршрут в системе?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Information);
                        if (result == MessageBoxResult.Yes)
                        {
                            if (comboBoxStartPoint.SelectedIndex != -1 || comboBoxLastPointInfo.SelectedIndex != -1)
                            {
                                string startPoint = this.comboBoxStartPoint.SelectedItem?.ToString();
                                string lastPoint = this.comboBoxLastPointInfo.SelectedItem?.ToString();
                                if (startPoint.Contains("улица"))
                                {
                                    string[] arr = this.comboBoxStartPoint.SelectedItem?.ToString().Split(',');
                                    startPoint = arr[arr.Length - 2].Trim() + " " + arr[arr.Length - 1].Trim();
                                    flagOne = true;
                                }
                                else
                                {
                                    startPoint = this.comboBoxStartPoint.SelectedItem?.ToString().Split(' ')[comboBoxStartPoint.SelectedItem.ToString().Split(' ').Length - 1];
                                }
                                if (lastPoint.Contains("улица"))
                                {
                                    string[] arr = this.comboBoxLastPointInfo.SelectedItem?.ToString().Split(',');
                                    lastPoint = arr[arr.Length - 2].Trim() + " " + arr[arr.Length - 1].Trim();
                                    flagTwo = true;
                                }
                                else
                                {
                                    lastPoint = this.comboBoxLastPointInfo.SelectedItem?.ToString().Split(' ')[comboBoxLastPointInfo.SelectedItem.ToString().Split(' ').Length - 1];
                                }
                                string addPoint;
                                if (pointCity != null)
                                {
                                    addPoint = string.Join("#", pointCity);
                                }
                                else
                                {
                                    addPoint = null;
                                }
                                string coordinatesOne = "";
                                string coordinatesTwo = "";
                                foreach (var item in AllCity)
                                {
                                    if(flagOne)
                                    {
                                        string[] arr = item.Item1.ToString().Split(',');
                                        string path = arr[arr.Length - 2].Trim() + " " + arr[arr.Length - 1].Trim();
                                        if (path.Trim() == startPoint.Trim())
                                        {
                                            coordinatesOne = item.Item2.Replace(' ', ',');
                                        }
                                    }
                                    else
                                    {
                                        string path = item.Item1.Split(',')[item.Item1.Split(',').Length - 1];
                                        if (path.Trim() == startPoint.Trim())
                                        {
                                            coordinatesOne = item.Item2.Replace(' ', ',');
                                        }
                                    }
                                    if (flagTwo)
                                    {
                                        string[] arr = item.Item1.ToString().Split(',');
                                        string path = arr[arr.Length - 2].Trim() + " " + arr[arr.Length - 1].Trim();
                                        if (path.Trim() == lastPoint.Trim())
                                        {
                                            coordinatesTwo = item.Item2.Replace(' ', ',');
                                        }
                                    }
                                    else
                                    {
                                        string path = item.Item1.Split(',')[item.Item1.Split(',').Length - 1];
                                        if (path.Trim() == lastPoint.Trim())
                                        {
                                            coordinatesTwo = item.Item2.Replace(' ', ',');
                                        }
                                    }
                                }
                                db.InsertPoint(startPoint, coordinatesOne, IdUser);
                                db.InsertPoint(lastPoint, coordinatesTwo, IdUser);
                                Class.Route route = new Class.Route();
                                route.Name = $"{startPoint} - {lastPoint}";
                                route.PointOne = db.GetCityIdByName(IdUser, startPoint);
                                route.PointTwo = db.GetCityIdByName(IdUser, lastPoint);
                                route.AdditionPoint = addPoint;
                                route.Distance = Convert.ToDouble(distance.Split(' ')[0]);
                                int timeInt = info[Convert.ToInt32(routeName.Split(' ')[1]) -1].time;
                                route.TravelTime = timeInt.ToString();
                                route.IdUser = IdUser;
                                selectRoute = route;
                                if (db.InsertRoute(route))
                                {
                                    CustomMessageBox.Show("Уведомление", $"Машрут {route.Name} сохранен в систему.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                                }
                                else
                                {
                                    CustomMessageBox.Show("Уведомление", $"Машрут {route.Name} не был сохранен в систему, так как уже есть в базе данных.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                                }
                            }
                        }
                    }
                }
            }
        }

        private async void Fill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Points.Children.Clear();
            bool internet = await CheckInternetConnectionAsync();
            if (Fill.SelectedIndex == 1)
            {
                if(internet)
                {
                    comboBoxStartPoint.Visibility = Visibility.Visible;
                    comboBoxLastPointInfo.Visibility = Visibility.Visible;
                    Distance.Visibility = Visibility.Collapsed;
                    fill = 1;
                }
                else
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        CustomMessageBox.Show("Уведомление", $"Проверьте интернет соединение и повторите попытку позже.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    });
                    Fill.SelectedIndex = 0;
                }
            }
            else
            {
                comboBoxStartPoint.Visibility = Visibility.Collapsed;
                comboBoxLastPointInfo.Visibility = Visibility.Collapsed;
                Distance.Visibility = Visibility.Visible;
                fill = 0;

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
    }
}
