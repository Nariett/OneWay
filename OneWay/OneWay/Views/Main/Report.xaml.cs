using System.Windows.Controls;
using System.Windows;
using OneWay.Class;
using System.Collections.Generic;
using OneWay.Controls;
using System;
using Microsoft.Win32;
using Xceed.Words.NET;
using Xceed.Document.NET;
using System.Globalization;
using System.Text;
using System.Linq;
using System.Data;
using System.Windows.Media;
namespace OneWay.Pages
{
    public partial class Report : Page
    {
        int IdUser;
        int IdTrip;
        string startData;
        string finishData;
        DataBase db = new DataBase("OneWayDB.db");
        List<Trip> allTrip = new List<Trip>();
        List<Trip> filterTrip = new List<Trip>();
        Trip selectTrip = new Trip();
        List<Trip> tripList = new List<Trip>();
        public Report(int IdUser, int IdTrip)
        {
            InitializeComponent();
            this.IdUser = IdUser;
            this.IdTrip = IdTrip;
            Fill.SelectedIndex = 0;
            allTrip = db.GetAllTripByUserId(IdUser);
            foreach (Trip trip in allTrip)
            {
                var info = db.GetRouteInfoById(trip.IdRoute);
                Car car = db.GetCar(trip.IdCar);
                string startPoint = info.Item1;
                string finishPoint = info.Item2;
                double distance = info.Item3;
                string text = $"{startPoint} - {finishPoint} {Convert.ToDateTime(trip.StartTime).ToString("dd.MM.yyyy")}";
                text += $"\n{car.Brand} {car.Model}";
                text += $"\n{distance} км";
                comboBoxTrip.Items.Add(text);
            }
            comboBoxTrip.Visibility = Visibility.Visible;
            for(int i = 0; i < allTrip.Count; i++)
            {
                if (allTrip[i].IdTrip == IdTrip)
                {
                    comboBoxTrip.SelectedIndex = i;
                    break;
                }
            }
        }
        public Report(int IdUser)
        {
            InitializeComponent();
            this.IdUser = IdUser;
            Fill.SelectedIndex = 1;
            allTrip = db.GetAllTripByUserId(IdUser);
            foreach(Trip trip in allTrip)
            {
                var info = db.GetRouteInfoById(trip.IdRoute);
                Car car = db.GetCar(trip.IdCar);
                string startPoint = info.Item1;
                string finishPoint = info.Item2;
                double distance = info.Item3;
                string text = $"{startPoint} - {finishPoint} {Convert.ToDateTime(trip.StartTime).ToString("dd.MM.yyyy")}";
                text += $"\n{car.Brand} {car.Model}";
                text += $"\n{distance} км";
                comboBoxTrip.Items.Add(text);
            }
        }
        private void ClearGridFieldTextBoxes()
        {
            ClearTextBoxes(GridField);
        }

        private void ClearTextBoxes(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is TextBox textBox && !string.IsNullOrEmpty(textBox.Name))
                {
                    textBox.Text = string.Empty;
                }
                else if (child is Panel panel)
                {
                    ClearTextBoxes(panel);
                }
                else if (child is ContentControl contentControl && contentControl.Content is DependencyObject content)
                {
                    ClearTextBoxes(content);
                }
            }
        }
        private void Fill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Fill.SelectedIndex == 0)
            {
                selectTrip = null;
                ClearGridFieldTextBoxes();
                TripText.Visibility = Visibility.Visible;
                comboBoxTrip.Visibility = Visibility.Visible;
                StackData.Visibility = Visibility.Collapsed;
                TextDate.Visibility = Visibility.Collapsed;
                CountText.Visibility = Visibility.Collapsed;
                Count.Visibility = Visibility.Collapsed;
                CountDistanceText.Visibility = Visibility.Collapsed;
                CountDistance.Visibility = Visibility.Collapsed;
                AveragePeopleText.Visibility = Visibility.Collapsed;
                AveragePeople.Visibility = Visibility.Collapsed;
                CreateButton.Visibility = Visibility.Collapsed;
                PopularCarText.Visibility = Visibility.Collapsed;
                PopularCar.Visibility = Visibility.Collapsed;
                PopularRouteText.Visibility = Visibility.Collapsed;
                PopularRoute.Visibility = Visibility.Collapsed;
                CountUseGasolineText.Visibility = Visibility.Collapsed;
                CountUseGasoline.Visibility = Visibility.Collapsed;
                CountUseDieselText.Visibility = Visibility.Collapsed;
                CountUseDiesel.Visibility = Visibility.Collapsed;
                CountUseElectText.Visibility = Visibility.Collapsed;
                CountUseElect.Visibility = Visibility.Collapsed;
                CountUseGasText.Visibility = Visibility.Collapsed;
                CountUseGas.Visibility = Visibility.Collapsed;
                TotalSpentMoneyText.Visibility = Visibility.Collapsed;
                TotalSpentMoney.Visibility = Visibility.Collapsed;
                ShowAllElements();
            }
            if (Fill.SelectedIndex == 1)
            {
                filterTrip = new List<Trip>();
                ClearGridFieldTextBoxes();
                TripText.Visibility = Visibility.Collapsed;
                comboBoxTrip.Visibility = Visibility.Collapsed;
                StackData.Visibility = Visibility.Visible;
                TextDate.Visibility = Visibility.Visible;
                CountText.Visibility = Visibility.Visible;
                Count.Visibility = Visibility.Visible;
                CountDistanceText.Visibility = Visibility.Visible;
                CountDistance.Visibility = Visibility.Visible;
                AveragePeopleText.Visibility = Visibility.Visible;
                AveragePeople.Visibility = Visibility.Visible;
                CreateButton.Visibility = Visibility.Visible;
                PopularCarText.Visibility = Visibility.Visible;
                PopularCar.Visibility = Visibility.Visible;
                PopularRouteText.Visibility = Visibility.Visible;
                PopularRoute.Visibility = Visibility.Visible;
                CountUseGasolineText.Visibility = Visibility.Visible;
                CountUseGasoline.Visibility = Visibility.Visible;
                CountUseDieselText.Visibility = Visibility.Visible;
                CountUseDiesel.Visibility = Visibility.Visible;
                CountUseElectText.Visibility = Visibility.Visible;
                CountUseElect.Visibility = Visibility.Visible;
                CountUseGasText.Visibility = Visibility.Visible;
                CountUseGas.Visibility = Visibility.Visible;
                TotalSpentMoneyText.Visibility = Visibility.Visible;
                TotalSpentMoney.Visibility = Visibility.Visible;
                HideAllElements();

            }
        }

        private void comboBoxTrip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboBoxTrip.SelectedIndex != -1) 
            {
                int index = comboBoxTrip.SelectedIndex;
                Trip trip = db.GetTripById(allTrip[index].IdTrip);
                selectTrip = trip;
                StartTime.Text = Convert.ToDateTime(trip.StartTime).ToString("HH:mm dd.MM.yyyy");
                FinishTime.Text = Convert.ToDateTime(trip.FinishTime).ToString("HH:mm dd.MM.yyyy");
                UsedFuel.Text = trip.UsedFuel.ToString();
                UsedMoney.Text = trip.UsedMoney.ToString();
                People.Text = trip.People.ToString();
                var route = db.GetRouteInfoById(trip.IdRoute);
                StartPoint.Text = route.Item1;
                FinishPoint.Text = route.Item2;
                Distance.Text = route.Item3.ToString();
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
                if (route.Item5 != "")
                {
                    Points.Text = route.Item5.Replace('#', '\n');
                }
                else
                {
                    Points.Visibility = Visibility.Collapsed;
                    TextPoints.Visibility = Visibility.Collapsed;
                }
                Car car = db.GetCar(trip.IdCar);
                if (car.Brand != "")
                {
                    Brand.Text = car.Brand;
                    Model.Text = car.Model;
                    Generation.Text = car.Generation;
                    Equipment.Text = car.Equipment;
                }
                else
                {
                    Brand.Text = "Неизвестно";
                    Model.Text = "Неизвестно";
                    Generation.Text = "Неизвестно";
                    Equipment.Text = "Неизвестно";
                }
                if (car.Fuel != "Электричество")
                {
                    TextUsedFuel.Text = "Использовано топлива, л";
                }
                else
                {
                    TextUsedFuel.Text = "Использовано электричества, кВт*ч";
                }
            }
        }
        private void HideAllElements()
        {
            DistanceText.Visibility = Visibility.Collapsed;
            Distance.Visibility = Visibility.Collapsed;
            StartPointText.Visibility = Visibility.Collapsed;
            StartPoint.Visibility = Visibility.Collapsed;
            FinishPointText.Visibility= Visibility.Collapsed;
            FinishPoint.Visibility= Visibility.Collapsed;
            TimeText.Visibility = Visibility.Collapsed;
            Time.Visibility = Visibility.Collapsed;
            StartTimeText.Visibility = Visibility.Collapsed;
            StartTime.Visibility = Visibility.Collapsed;
            FinishTimeText.Visibility = Visibility.Collapsed;
            FinishTime.Visibility = Visibility.Collapsed;;
            TextPoints.Visibility = Visibility.Collapsed;
            Points.Visibility = Visibility.Collapsed;
            TextUsedFuel.Visibility = Visibility.Collapsed;
            UsedFuel.Visibility = Visibility.Collapsed;
            UsedMoneyText.Visibility = Visibility.Collapsed;
            UsedMoney.Visibility = Visibility.Collapsed;
            PeopleText.Visibility = Visibility.Collapsed;
            People.Visibility = Visibility.Collapsed;
            BrandText.Visibility = Visibility.Collapsed;
            Brand.Visibility = Visibility.Collapsed;
            ModelText.Visibility = Visibility.Collapsed;
            Model.Visibility = Visibility.Collapsed;
            GenerationText.Visibility = Visibility.Collapsed;
            Generation.Visibility = Visibility.Collapsed;
            EquipmentText.Visibility = Visibility.Collapsed;
            Equipment.Visibility = Visibility.Collapsed; 
        }

        private void ShowAllElements()
        {
            DistanceText.Visibility = Visibility.Visible;
            Distance.Visibility = Visibility.Visible;
            StartPointText.Visibility = Visibility.Visible;
            StartPoint.Visibility = Visibility.Visible;
            FinishPointText.Visibility = Visibility.Visible;
            FinishPoint.Visibility = Visibility.Visible;
            TimeText.Visibility = Visibility.Visible;
            Time.Visibility = Visibility.Visible    ;
            StartTimeText.Visibility = Visibility.Visible;
            StartTime.Visibility = Visibility.Visible;
            FinishTimeText.Visibility = Visibility.Visible;
            FinishTime.Visibility = Visibility.Visible;
            StartTimeText.Visibility = Visibility.Visible;
            StartTime.Visibility = Visibility.Visible;
            TextPoints.Visibility = Visibility.Visible;
            Points.Visibility = Visibility.Visible;
            TextUsedFuel.Visibility = Visibility.Visible;
            UsedFuel.Visibility = Visibility.Visible;
            UsedMoneyText.Visibility = Visibility.Visible;
            UsedMoney.Visibility = Visibility.Visible;
            PeopleText.Visibility = Visibility.Visible;
            People.Visibility = Visibility.Visible;
            BrandText.Visibility = Visibility.Visible;
            Brand.Visibility = Visibility.Visible;
            ModelText.Visibility = Visibility.Visible;
            Model.Visibility = Visibility.Visible;
            GenerationText.Visibility = Visibility.Visible;
            Generation.Visibility = Visibility.Visible;
            EquipmentText.Visibility = Visibility.Visible;
            Equipment.Visibility = Visibility.Visible;
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder filterQuery = new StringBuilder($"SELECT * FROM Trips WHERE IdUser = {IdUser} ");
            startData = null;
            finishData = null;
            DateTime date1, date2;
            if (DateTime.TryParseExact(DateFrom.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date1))
            {
                filterQuery.Append($"AND StartTime >= '{date1.ToString("yyyy-MM-dd HH:mm:ss")}' ");
                startData = date1.ToString("dd.MM.yyyy");
            }
            else if(DateFrom.Text != "")
            {
                CustomMessageBox.Show("Уведомление", $"Введите корректную дату 1 в формате {DateTime.Now.ToString("dd.MM.yyyy")} и повторите попытку. Проверьте название и повторите попытку", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return;
            }

            if (DateTime.TryParseExact(DateBefore.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date2))
            {
                date2 = date2.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                filterQuery.Append($"AND StartTime <= '{date2.ToString("yyyy-MM-dd HH:mm:ss")}' ");
                finishData = date2.ToString("dd.MM.yyyy");
            }
            else if (DateBefore.Text != "")
            {
                CustomMessageBox.Show("Уведомление", $"Введите корректную дату 2 в формате {DateTime.Now.ToString("dd.MM.yyyy")} и повторите попытку. Проверьте название и повторите попытку", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return;
            }
            if (filterQuery.ToString().EndsWith(" AND "))
            {
                filterQuery.Remove(filterQuery.Length - 5, 5);
            }

            string combinedQuery = filterQuery.ToString();
            if (combinedQuery != "SELECT * FROM Trips WHERE ")
            {
                filterTrip = db.GetHistory(combinedQuery);
                if(filterTrip.Count > 0)
                {
                    Count.Text = filterTrip.Count.ToString();
                    double counteDistance = 0;
                    double money = 0;
                    int people = 0;
                    foreach(Trip item in filterTrip)
                    {
                        var route = db.GetRouteInfoById(item.IdRoute);
                        counteDistance += route.Item3;
                        people += item.People;
                        money += item.UsedMoney;
                    }
                    TotalSpentMoney.Text = money.ToString();
                    CountDistance.Text = counteDistance.ToString();
                    AveragePeople.Text = Math.Abs(people / filterTrip.Count).ToString();
                    double diesel = 0;
                    double gasoline = 0;
                    double electricity = 0;
                    double gas = 0;
                    foreach (Trip item in filterTrip)
                    {
                        Car selectCar = db.GetCar(item.IdCar);
                        if (selectCar.Fuel == "Бензин")
                        {
                            gasoline += item.UsedFuel;
                        }
                        else if (selectCar.Fuel == "Дизельное топливо")
                        {
                            diesel += item.UsedFuel;
                        }
                        else if (selectCar.Fuel == "Электричество")
                        {
                            electricity += item.UsedFuel;
                        }
                        else if (selectCar.Fuel == "Газ")
                        {
                            gas += item.UsedFuel;
                        }
                    }
                    if(gasoline > 0)
                    {
                        CountUseGasoline.Text = gasoline.ToString();
                        CountUseGasolineText.Visibility = Visibility.Visible;
                        CountUseGasoline.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        CountUseGasolineText.Visibility = Visibility.Collapsed;
                        CountUseGasoline.Visibility = Visibility.Collapsed;
                    }
                    if (diesel > 0)
                    {
                        CountUseDiesel.Text = diesel.ToString();
                        CountUseDieselText.Visibility = Visibility.Visible;
                        CountUseDiesel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        CountUseDieselText.Visibility = Visibility.Collapsed;
                        CountUseDiesel.Visibility = Visibility.Collapsed;
                    }
                    if (electricity > 0)
                    {
                        CountUseElect.Text = electricity.ToString();
                        CountUseElectText.Visibility = Visibility.Visible;
                        CountUseElect.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        CountUseElectText.Visibility = Visibility.Collapsed;
                        CountUseElect.Visibility = Visibility.Collapsed;
                    }
                    if (gas > 0)
                    {
                        CountUseGas.Text = gas.ToString();
                        CountUseGasText.Visibility = Visibility.Visible;
                        CountUseGas.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        CountUseGasText.Visibility = Visibility.Collapsed;
                        CountUseGas.Visibility = Visibility.Collapsed;
                    }

                    int popularRoute = filterTrip // расчет популярного маршрута
                        .GroupBy(trip => trip.IdRoute)
                        .OrderByDescending(group => group.Count())
                        .First()
                        .Key;
                    int popularCar = filterTrip // расчет популярного автомобиля
                        .GroupBy(trip => trip.IdCar)
                        .OrderByDescending(group => group.Count())
                        .First()
                        .Key;
                    int countRoute = 0;
                    int countCar = 0;
                    foreach(Trip item in filterTrip)
                    {
                        if(item.IdCar == popularCar)
                        {
                            countCar++;
                        }
                        if(item.IdRoute == popularRoute)
                        {
                            countRoute++;
                        }
                    }
                    if(countCar > 1 | (countRoute == 1 && filterTrip.Count == 1))
                    {
                        PopularCarText.Visibility = Visibility.Visible;
                        PopularCar.Visibility = Visibility.Visible;
                        Car car = db.GetCar(popularCar);
                        PopularCar.Text = $"{car.Brand} {car.Model} {car.Equipment}";
                    }
                    else
                    {
                        PopularCarText.Visibility = Visibility.Collapsed;
                        PopularCar.Visibility = Visibility.Collapsed;
                    }
                    if (countRoute > 1 | (countRoute == 1 && filterTrip.Count == 1))
                    {
                        PopularRouteText.Visibility = Visibility.Visible;
                        PopularRoute.Visibility = Visibility.Visible;
                        var select = db.GetRouteInfoById(popularRoute);
                        PopularRoute.Text = $"{select.Item1} - {select.Item2}";
                    }
                    else
                    {
                        PopularRouteText.Visibility = Visibility.Collapsed;
                        PopularRoute.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    CustomMessageBox.Show("Уведомление", "Поездки не найдены.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                }
            }
            else
            {
                CustomMessageBox.Show("Уведомление", "Отображен отчет по всем поездкам пользователя.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                filterTrip = db.GetAllTripByUserId(IdUser);
            }
        }
        private void CreateReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (Fill.SelectedIndex == 0) // отчет по поездке
            {
                if (selectTrip != null)
                {
                    Car car = db.GetCar(selectTrip.IdCar);
                    var result = CustomMessageBox.Show("Уведомление", $"Желаете создать отчет по выбранной поездке?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "Документ Word (*.docx)|*.docx";
                        var route = db.GetRouteInfoById(selectTrip.IdRoute);
                        StartPoint.Text = route.Item1;
                        FinishPoint.Text = route.Item2;
                        saveFileDialog.FileName = $"Отчет {route.Item1}-{route.Item2} на {Convert.ToDateTime(selectTrip.StartTime).ToString("dd.MM.yyyy")}.docx";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string fileName = saveFileDialog.FileName;
                            // Создание документа Word
                            try
                            {
                                using (DocX document = DocX.Create(fileName))
                                {
                                    document.InsertParagraph($"Отчет о поездке" + Environment.NewLine).FontSize(14d).Bold().Alignment = Alignment.center;
                                    Paragraph paragraph = document.InsertParagraph();
                                    paragraph.Append("Пользователь: ").FontSize(12d);
                                    paragraph.Append($"{db.GetFirstNameById(IdUser)};" + Environment.NewLine).FontSize(12d).Bold();

                                    Paragraph paragraphRoute = document.InsertParagraph();
                                    paragraphRoute.Append("Маршрут: ").FontSize(12d);
                                    paragraphRoute.Append($"{route.Item1}-{route.Item2};" + Environment.NewLine).FontSize(12d).Bold();

                                    Paragraph paragraphCar = document.InsertParagraph();
                                    paragraphCar.Append("Автомобиль: ").FontSize(12d);
                                    paragraphCar.Append($"{car.Brand} {car.Model} {car.Equipment}." + Environment.NewLine).FontSize(12d).Bold();

                                    int rowCount = route.Item5 != "" ? 8 : 7;
                                    int currentRow = 1;
                                    Table table = document.AddTable(rowCount, 2);

                                    table.Rows[0].Cells[0].Paragraphs.First().Append("Параметр").FontSize(12d);
                                    table.Rows[0].Cells[1].Paragraphs.First().Append("Значение").FontSize(12d);

                                    table.Rows[currentRow].Cells[0].Paragraphs.First().Append("Расстояние").FontSize(12d);
                                    table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{route.Item3} км").FontSize(12d);

                                    table.Rows[currentRow].Cells[0].Paragraphs.First().Append($"Время в пути").FontSize(12d);
                                    table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{Time.Text}").FontSize(12d);

                                    table.Rows[currentRow].Cells[0].Paragraphs.First().Append($"Время отправления").FontSize(12d);
                                    table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{StartTime.Text}").FontSize(12d);
                                
                                    table.Rows[currentRow].Cells[0].Paragraphs.First().Append($"Время прибытия").FontSize(12d);
                                    table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{FinishTime.Text}").FontSize(12d);
                                    if (route.Item5 != "")
                                    {
                                        table.Rows[currentRow].Cells[0].Paragraphs.First().Append($"Точки остановки").FontSize(12d);
                                        table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{route.Item5.Replace('#', '\n')}").FontSize(12d);
                                    }
                                    if (car.Fuel != "Электричество")
                                    {
                                        table.Rows[currentRow].Cells[0].Paragraphs.First().Append($"Общее кол-во потраченного топлива").FontSize(12d);
                                        table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{selectTrip.UsedFuel} литров").FontSize(12d);
                                    }
                                    else
                                    {
                                        table.Rows[currentRow].Cells[0].Paragraphs.First().Append($"Общее кол-во потраченного электричества").FontSize(12d);
                                        table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{selectTrip.UsedFuel} кВт * ч").FontSize(12d);
                                    }
                                    table.Rows[currentRow].Cells[0].Paragraphs.First().Append($"Затраты на поездку").FontSize(12d);
                                    table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{selectTrip.UsedMoney} рублей").FontSize(12d);
                                    document.InsertTable(table);

                                    document.InsertParagraph();
                                    document.InsertParagraph($"Дата формирования отчета {DateTime.Now.ToString("HH:mm dd.MM.yyyy")}").FontSize(12d).Alignment = Alignment.right;
                                
                                    document.Save();
                                }
                                CustomMessageBox.Show("Уведомление", $"Данный отчет сохранен.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                CustomMessageBox.Show("Ошибка", $"Произошла ошибка при сохранении отчета: {ex.Message}", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                            }
                        }
                    }
                }
                else
                {
                    CustomMessageBox.Show("Уведомление", $"Выберите поездку и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                }
            }
            if (Fill.SelectedIndex == 1) // отчет за выбранный период времени
            {
                if (filterTrip.Count > 0) // пользователь не совершил поездку
                {
                    MessageBoxResult result;
                    if(startData == null & finishData == null)
                    {
                        result = CustomMessageBox.Show("Уведомление", $"Желаете создать отчет за все поездки?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Question);
                    }
                    else
                    {
                        result = CustomMessageBox.Show("Уведомление", $"Желаете создать отчет выбранный период времени?", MessageBoxButton.YesNo, CustomMessageBox.MessageBoxImage.Question);
                    }
                    if (result == MessageBoxResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "Документ Word (*.docx)|*.docx";
                        saveFileDialog.FileName = $"Отчет на {DateTime.Now.ToString("dd.MM.yyyy")}.docx";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string fileName = saveFileDialog.FileName;
                            try
                            {

                                using (DocX document = DocX.Create(fileName))
                                {
                                    if (startData != null & finishData != null)
                                    {
                                        document.InsertParagraph($"Отчет о поездках за {startData} - {finishData}" + Environment.NewLine).FontSize(14d).Bold().Alignment = Alignment.center;
                                    }
                                    else if (startData != null)
                                    {
                                        document.InsertParagraph($"Отчет о поездках c {startData} - {DateTime.Now.ToString("dd.MM.yyyy")}" + Environment.NewLine).FontSize(14d).Bold().Alignment = Alignment.center;
                                    }
                                    else if (finishData != null)
                                    {
                                        document.InsertParagraph($"Отчет о поездках до {finishData}" + Environment.NewLine).FontSize(14d).Bold().Alignment = Alignment.center;
                                    }
                                    else
                                    {
                                        document.InsertParagraph($"Отчет о всех поездках" + Environment.NewLine).FontSize(14d).Bold().Alignment = Alignment.center;
                                    }
                                    Paragraph paragraph = document.InsertParagraph();
                                    paragraph.Append("Пользователь: ").FontSize(12d);
                                    paragraph.Append($"{db.GetFirstNameById(IdUser)}." + Environment.NewLine).FontSize(12d).Bold();

                                    int rowCount = 5; // базовые строки Количество поездок, Общее расстояние, Среднее количество пассажиров 
                                    if (PopularCar.Visibility == Visibility.Visible) rowCount++;
                                    if (PopularRoute.Visibility == Visibility.Visible) rowCount++;
                                    if (CountUseGasoline.Visibility == Visibility.Visible) rowCount++;
                                    if (CountUseDiesel.Visibility == Visibility.Visible) rowCount++;
                                    if (CountUseElect.Visibility == Visibility.Visible) rowCount++;
                                    if (CountUseGas.Visibility == Visibility.Visible) rowCount++;

                                    Table table = document.AddTable(rowCount, 2);

                                    table.Rows[0].Cells[0].Paragraphs.First().Append("Параметр").FontSize(12d);
                                    table.Rows[0].Cells[1].Paragraphs.First().Append("Значение").FontSize(12d);

                                    int currentRow = 1;

                                    table.Rows[currentRow].Cells[0].Paragraphs.First().Append("Количество поездок").FontSize(12d);
                                    table.Rows[currentRow++].Cells[1].Paragraphs.First().Append(Count.Text).FontSize(12d);

                                    table.Rows[currentRow].Cells[0].Paragraphs.First().Append("Общее расстояние").FontSize(12d);
                                    table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{CountDistance.Text} км").FontSize(12d);

                                    table.Rows[currentRow].Cells[0].Paragraphs.First().Append("Затраты на поездки:").FontSize(12d);
                                    table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{TotalSpentMoney.Text} рублей").FontSize(12d);

                                    table.Rows[currentRow].Cells[0].Paragraphs.First().Append("Среднее количество пассажиров").FontSize(12d);
                                    table.Rows[currentRow++].Cells[1].Paragraphs.First().Append(AveragePeople.Text).FontSize(12d);

                                    if (PopularCar.Visibility == Visibility.Visible)
                                    {
                                        table.Rows[currentRow].Cells[0].Paragraphs.First().Append("Часто используемый автомобиль").FontSize(12d);
                                        table.Rows[currentRow++].Cells[1].Paragraphs.First().Append(PopularCar.Text).FontSize(12d);
                                    }

                                    if (PopularRoute.Visibility == Visibility.Visible)
                                    {
                                        table.Rows[currentRow].Cells[0].Paragraphs.First().Append("Часто используемый маршрут").FontSize(12d);
                                        table.Rows[currentRow++].Cells[1].Paragraphs.First().Append(PopularRoute.Text).FontSize(12d);
                                    }

                                    if (CountUseGasoline.Visibility == Visibility.Visible)
                                    {
                                        table.Rows[currentRow].Cells[0].Paragraphs.First().Append("Затрачено бензина").FontSize(12d);
                                        table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{CountUseGasoline.Text} л").FontSize(12d);
                                    }

                                    if (CountUseDiesel.Visibility == Visibility.Visible)
                                    {
                                        table.Rows[currentRow].Cells[0].Paragraphs.First().Append("Затрачено дизельного топлива").FontSize(12d);
                                        table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{CountUseDiesel.Text} л").FontSize(12d);
                                    }

                                    if (CountUseElect.Visibility == Visibility.Visible)
                                    {
                                        table.Rows[currentRow].Cells[0].Paragraphs.First().Append("Затрачено электричества").FontSize(12d);
                                        table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{CountUseElect.Text} кВт * ч").FontSize(12d);
                                    }

                                    if (CountUseGas.Visibility == Visibility.Visible)
                                    {
                                        table.Rows[currentRow].Cells[0].Paragraphs.First().Append("Затрачено газа").FontSize(12d);
                                        table.Rows[currentRow++].Cells[1].Paragraphs.First().Append($"{CountUseGas.Text} л").FontSize(12d);
                                    }
                                    document.InsertTable(table);


                                    foreach (Trip item in filterTrip)
                                    {
                                        Car car = db.GetCar(item.IdCar);
                                        var route = db.GetRouteInfoById(item.IdRoute);
                                        StartPoint.Text = route.Item1;
                                        FinishPoint.Text = route.Item2;
                                        document.InsertParagraph().InsertPageBreakAfterSelf();

                                        Paragraph paragraphRoute = document.InsertParagraph();
                                        paragraphRoute.Append("Маршрут: ").FontSize(12d);
                                        paragraphRoute.Append($"{route.Item1}-{route.Item2};" + Environment.NewLine).FontSize(12d).Bold();

                                        Paragraph paragraphCar = document.InsertParagraph();
                                        paragraphCar.Append("Автомобиль: ").FontSize(12d);
                                        paragraphCar.Append($"{car.Brand} {car.Model} {car.Equipment}." + Environment.NewLine).FontSize(12d).Bold();

                                        int rowCountItem = route.Item5 != "" ? 8 : 7;
                                        int currentRowItem = 1;
                                        Table tableItem = document.AddTable(rowCountItem, 2);

                                        tableItem.Rows[0].Cells[0].Paragraphs.First().Append("Параметр").FontSize(12d);
                                        tableItem.Rows[0].Cells[1].Paragraphs.First().Append("Значение").FontSize(12d);

                                        tableItem.Rows[currentRowItem].Cells[0].Paragraphs.First().Append("Расстояние").FontSize(12d);
                                        tableItem.Rows[currentRowItem++].Cells[1].Paragraphs.First().Append($"{route.Item3} км").FontSize(12d);

                                        TimeSpan date = TimeSpan.FromMinutes(Convert.ToDouble(route.Item4));
                                        string time = "";
                                        if (date.Days >= 1)
                                        {
                                            if (date.Hours == 0)
                                            {
                                                time = $"{date.Days} д {date.Minutes} мин";
                                            }
                                            else
                                            {
                                                time = $"{date.Days} д {date.Hours} ч {date.Minutes} мин";
                                            }
                                        }
                                        else
                                        {
                                            if (date.Hours != 0)
                                            {
                                                time = $"{date.Hours} ч {date.Minutes} мин";
                                            }
                                            else
                                            {
                                                time = $"{date.Minutes} мин";
                                            }
                                        }

                                        tableItem.Rows[currentRowItem].Cells[0].Paragraphs.First().Append($"Время в пути").FontSize(12d);
                                        tableItem.Rows[currentRowItem++].Cells[1].Paragraphs.First().Append($"{time}").FontSize(12d);

                                        tableItem.Rows[currentRowItem].Cells[0].Paragraphs.First().Append($"Время отправления").FontSize(12d);
                                        tableItem.Rows[currentRowItem++].Cells[1].Paragraphs.First().Append($"{Convert.ToDateTime(item.StartTime).ToString("HH:mm dd.MM.yyyy")}").FontSize(12d);

                                        tableItem.Rows[currentRowItem].Cells[0].Paragraphs.First().Append($"Время прибытия").FontSize(12d);
                                        tableItem.Rows[currentRowItem++].Cells[1].Paragraphs.First().Append($"{Convert.ToDateTime(item.FinishTime).ToString("HH:mm dd.MM.yyyy")}").FontSize(12d);
                                        if (route.Item5 != "")
                                        {
                                            tableItem.Rows[currentRowItem].Cells[0].Paragraphs.First().Append($"Точки остановки").FontSize(12d);
                                            tableItem.Rows[currentRowItem++].Cells[1].Paragraphs.First().Append($"{route.Item5.Replace('#', '\n')}").FontSize(12d);
                                        }
                                        if (car.Fuel != "Электричество")
                                        {
                                            tableItem.Rows[currentRowItem].Cells[0].Paragraphs.First().Append($"Общее кол-во потраченного топлива").FontSize(12d);
                                            tableItem.Rows[currentRowItem++].Cells[1].Paragraphs.First().Append($"{item.UsedFuel} литров").FontSize(12d);
                                        }
                                        else
                                        {
                                            tableItem.Rows[currentRowItem].Cells[0].Paragraphs.First().Append($"Общее кол-во потраченного электричества").FontSize(12d);
                                            tableItem.Rows[currentRowItem++].Cells[1].Paragraphs.First().Append($"{item.UsedFuel} кВт * ч").FontSize(12d);
                                        }
                                        tableItem.Rows[currentRowItem].Cells[0].Paragraphs.First().Append($"Затраты на поездку").FontSize(12d);
                                        tableItem.Rows[currentRowItem++].Cells[1].Paragraphs.First().Append($"{item.UsedMoney} рублей").FontSize(12d);
                                        document.InsertTable(tableItem);
                                    }
                                    document.InsertParagraph($"Дата формирования отчета {DateTime.Now.ToString("HH:mm dd.MM.yyyy")}").FontSize(12d).Alignment = Alignment.right;
                                    document.Save();//сохранение word документа
                                }
                                CustomMessageBox.Show("Уведомление", $"Данный отчет сохранен.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                CustomMessageBox.Show("Ошибка", $"Произошла ошибка при сохранении отчета: {ex.Message}", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                            }
                        }
                    }
                }
                else
                {
                    CustomMessageBox.Show("Уведомление", $"Поездки не найдены, повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                }
            }
        }
    }
}
