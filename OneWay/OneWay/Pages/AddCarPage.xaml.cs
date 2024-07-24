using OneWay.Class;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;
using OneWay.Controls;
using System.Net.NetworkInformation;
using Microsoft.Win32.SafeHandles;

namespace OneWay.Pages
{
    public partial class AddCarPage : Page
    {
        public int IdUser;
        Car newCar = new Car();
        public string selectUrl;
        public string carNameUrl;
        public string carModelUrl;
        public string carEquipmentUrl;
        DataBase db = new DataBase("OneWayDB.db");
        public List<(int id, string octaneNumber)> octaneNumber;
        public List<Tuple<string, string>> carName;
        public List<Tuple<string, string>> carModel;
        public List<Tuple<string, string>> carGeneration;
        public List<Tuple<string, string>> carEquipment;
        public Dictionary<string, string> countryTranslations = new Dictionary<string, string>()
        {
            { "no", "Для мира" },
            { "russia", "Для России" },
            { "europe", "Для Европы" },
            { "japan", "Для Японии" },
            { "usa", "Для США" },
            { "uae", "Для ОАЭ" },
            { "south-east-asia", "Для Азии" },
            { "south-korea", "Для Южной Кореи" },
            { "china", "Для Китая" }
        };


        public AddCarPage(int IdUser)
        {
            InitializeComponent();
            this.IdUser = IdUser;
        }
        private async void Fill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Fill.SelectedIndex == 1) // автоматическое заполение полей
            {
                ClearField();
                newCar = new Car();
                bool isConnected = await CheckInternetConnectionAsync();
                if (isConnected)
                {
                    BlockField();
                    ComboBoxCarBrand.Visibility = Visibility.Visible;
                    CarBrand.Visibility = Visibility.Collapsed;
                    ComboBoxCarModel.Visibility = Visibility.Visible;
                    CarModel.Visibility = Visibility.Collapsed;
                    ComboBoxCarGeneration.Visibility = Visibility.Visible;
                    CarGeneration.Visibility = Visibility.Collapsed;
                    ComboBoxCarEquipment.Visibility = Visibility.Visible;
                    CarEquipment.Visibility = Visibility.Collapsed;
                    Button_Web.Visibility = Visibility.Visible;

                    carName = await GetCarData("https://www.drom.ru/catalog/", "//div[@data-ftid='component_cars-list']");
                    carName = carName.OrderBy(t => t.Item1).ToList();
                    if(carName.Count != 0)
                    {
                        await Dispatcher.InvokeAsync(() =>
                        {
                            foreach (Tuple<string, string> car in carName)
                            {
                                ComboBoxCarBrand.Items.Add(car.Item1);
                            }
                        });
                    }
                    else
                    {
                        Fill.SelectedIndex = 0;
                    }
                }
                else
                {
                    Fill.SelectedIndex = 0;
                    var error = CustomMessageBox.Show("Уведомление", $"Проверьте интернет - соединение и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                }

            }
            else // ручное заполнение полей
            {
                ClearField();
                UnBlockField();
                newCar = new Car();
                ComboBoxCarBrand.Visibility = Visibility.Collapsed;
                CarBrand.Visibility = Visibility.Visible;
                ComboBoxCarModel.Visibility = Visibility.Collapsed;
                CarModel.Visibility = Visibility.Visible;
                ComboBoxCarGeneration.Visibility = Visibility.Collapsed;
                CarGeneration.Visibility = Visibility.Visible;
                ComboBoxCarEquipment.Visibility = Visibility.Collapsed;
                CarEquipment.Visibility = Visibility.Visible;
                Button_Web.Visibility = Visibility.Collapsed;
            }
        }
        private void EngineType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EngineType.SelectedIndex == 0)
            {
                Volume.Visibility = Visibility.Visible;
                TextVolume.Visibility = Visibility.Visible;
                TextBlockBatteryCapacity.Visibility = Visibility.Collapsed;
                TextBlockRangePerCharge.Visibility = Visibility.Collapsed;
                BatteryCapacity.Visibility = Visibility.Collapsed;
                RangePerCharge.Visibility = Visibility.Collapsed;
                Petrol.Visibility = Visibility.Visible;
                Gaz.Visibility = Visibility.Visible;
                Diesel.Visibility = Visibility.Visible;
                Electro.Visibility = Visibility.Collapsed;

            }
            else if(EngineType.SelectedIndex == 1)
            {
                Volume.Visibility = Visibility.Collapsed;
                TextVolume.Visibility = Visibility.Collapsed;

                TextBlockBatteryCapacity.Visibility = Visibility.Visible;
                TextBlockRangePerCharge.Visibility = Visibility.Visible;

                BatteryCapacity.Visibility = Visibility.Visible;
                RangePerCharge.Visibility = Visibility.Visible;
                Fuel.SelectedIndex = 2;
                Petrol.Visibility = Visibility.Collapsed;
                Gaz.Visibility = Visibility.Collapsed;
                Diesel.Visibility = Visibility.Collapsed;
                Electro.Visibility = Visibility.Visible;

            }
            else if(EngineType.SelectedIndex == 2)
            {
                Volume.Visibility = Visibility.Visible;
                TextVolume.Visibility = Visibility.Visible;
                TextBlockBatteryCapacity.Visibility = Visibility.Visible;
                TextBlockRangePerCharge.Visibility = Visibility.Visible;
                BatteryCapacity.Visibility = Visibility.Visible;
                RangePerCharge.Visibility = Visibility.Visible;
                Petrol.Visibility = Visibility.Visible;
                Gaz.Visibility = Visibility.Visible;
                Diesel.Visibility = Visibility.Visible;
                Electro.Visibility = Visibility.Collapsed;
            }
            OctaneNumber.SelectedIndex = -1;
            Fuel.SelectedIndex = -1;
            //Volume.Text = "";
        }
        public void ClearComboBox(ComboBox startingComboBox)
        {
            bool foundStartingComboBox = false;

            List<ComboBox> allComboBoxes = new List<ComboBox>
            {
                ComboBoxCarBrand,
                ComboBoxCarModel,
                ComboBoxCarGeneration,
                ComboBoxCarEquipment
            };
            foreach (var comboBox in allComboBoxes)
            {
                if (foundStartingComboBox)
                {
                    comboBox.Items.Clear();
                }

                if (comboBox == startingComboBox)
                {
                    foundStartingComboBox = true;
                }
            }
        }

        private async void ComboBoxCarBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.ComboBoxCarBrand.SelectedIndex;
            if (index != -1)
            {
                ClearField();
                ClearComboBox(ComboBoxCarBrand);
                selectUrl = carName[index].Item2;
                carModel = await GetCarData(selectUrl, "//div[@data-ftid='component_cars-list']");
                if (carModel.Count != 0)
                {
                    carModel = carModel.OrderBy(t => t.Item1).ToList();
                    foreach (Tuple<string, string> car in carModel)
                    {
                        this.ComboBoxCarModel.Items.Add(car.Item1);
                    }
                }
                else
                {
                    Fill.SelectedIndex = 0;
                }
            }
        }


        private async void ComboBoxCarModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.ComboBoxCarModel.SelectedIndex;
            if (index != -1)
            {
                ClearField();
                ClearComboBox(ComboBoxCarModel);
                selectUrl = carModel[index].Item2;
                carGeneration = await GetCarGeneration(selectUrl, "//div[@class='css-10ib5jr']");
                if (carGeneration.Count != 0)
                {
                    carGeneration = carGeneration.OrderBy(t => t.Item1).ToList();
                    foreach (Tuple<string, string> car in carGeneration)
                    {
                        this.ComboBoxCarGeneration.Items.Add(car.Item1);
                    }
                }
                else
                {
                    Fill.SelectedIndex = 0;
                }
            }
        }
        private async void ComboBoxCarGeneration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.ComboBoxCarGeneration.SelectedIndex;
            if (index != -1)
            {
                ClearField();
                ClearComboBox(ComboBoxCarGeneration);
                string url = selectUrl + carGeneration[index].Item2;
                carEquipment = await GetCarEquipment(url, "//table[@class='b-table b-table_text-left']");
                carEquipment.RemoveAll(t => string.IsNullOrEmpty(t.Item1) | t.Item2.Contains("engine") | t.Item2.Contains("#"));
                if (carEquipment.Count == 0)
                {

                    CustomMessageBox.Show("Уведомление","Извините, произошла ошибка. Информация о данном автомобиле не найдена или автомобиль не был выпущен.", MessageBoxButton.OK,CustomMessageBox.MessageBoxImage.Information);
                }
                foreach (Tuple<string, string> car in carEquipment)
                {
                    this.ComboBoxCarEquipment.Items.Add(car.Item1);
                }
            }
        }
        private void ComboBoxCarEquipment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.ComboBoxCarEquipment.SelectedIndex;
            if (index != -1)
            {
                ClearField();
                string url = $"https://www.drom.ru/{carEquipment[index].Item2}";
                GetAllData(url, "//div[@class='b-media-cont b-media-cont_margin_b-size-m bm-catCompTableWrap']");
            }
        }
        public async Task<List<Tuple<string, string>>> GetCarData(string url, string xpath)
        {
            List<Tuple<string, string>> data = new List<Tuple<string, string>>();
            int retryCount = 0; 

            while (data.Count == 0 && retryCount < 3) 
            {
                try
                {
                    if (!(await CheckInternetConnectionAsync()))
                    {
                        break;
                    }
                    HtmlWeb webGet = new HtmlWeb();
                    HtmlDocument document = await webGet.LoadFromWebAsync(url);

                    HtmlNode parentDivNode = document.DocumentNode.SelectSingleNode(xpath);
                    if (parentDivNode != null)
                    {
                        HtmlNodeCollection anchorNodes = parentDivNode.SelectNodes(".//a");
                        if (anchorNodes != null)
                        {
                            foreach (HtmlNode anchorNode in anchorNodes)
                            {
                                string linkText = anchorNode.InnerText.Trim();
                                string href = anchorNode.GetAttributeValue("href", "");
                                if (!string.IsNullOrEmpty(href))
                                {
                                    data.Add(Tuple.Create(linkText, href));
                                }
                            }
                        }
                    }
                    if (data.Count > 0)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        CustomMessageBox.Show("Уведомление", $"Произошла ошибка:{ex.Message}", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    });
                }
                retryCount++;
            }
            if (data.Count == 0)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    CustomMessageBox.Show("Уведомление", $"Данные не удалось получить. Проверьте интернет соединение и повторите попытку позже.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                });
            }

            return data;
        }
        public async Task<List<Tuple<string, string>>> GetCarGeneration(string url, string xpath)
        {
            List<Tuple<string, string>> carDataList = new List<Tuple<string, string>>(); 
            int attempts = 0;

            while (carDataList.Count == 0 && attempts < 3)
            {
                try
                {
                    if (!(await CheckInternetConnectionAsync()))
                    {
                        break;
                    }
                    HtmlWeb webGet = new HtmlWeb();
                    HtmlDocument document = await webGet.LoadFromWebAsync(url);
                    HtmlNode parentDivNode = document.DocumentNode.SelectSingleNode(xpath);
                    if (parentDivNode != null)
                    {
                        HtmlNodeCollection countryDivs = parentDivNode.SelectNodes("div[@class = 'css-pyemnz e1ei9t6a4']");
                        foreach (HtmlNode countryDivNode in countryDivs)
                        {
                            HtmlNodeCollection countryDivTexts = countryDivNode.SelectNodes("div[@class = 'css-112idg0 e1ei9t6a3']");
                            string countryCode = countryDivTexts[0].GetAttributeValue("id", "");
                            string countryName = countryTranslations.ContainsKey(countryCode) ? countryTranslations[countryCode] : "";
                            HtmlNodeCollection carDivs = countryDivNode.SelectNodes("div[@class = 'css-kumxje e1naoij51']");
                            foreach (HtmlNode BlockWithCar in carDivs)
                            {
                                HtmlNodeCollection selectCar = BlockWithCar.SelectNodes("div[@class = 'css-0 e1ei9t6a0']");
                                foreach (HtmlNode car in selectCar)
                                {
                                    HtmlNodeCollection anchorNodes = car.SelectNodes(".//a");
                                    if (anchorNodes != null && anchorNodes.Count > 0)
                                    {
                                        string href = anchorNodes[0].GetAttributeValue("href", "");

                                        HtmlNode spanNode = car.SelectSingleNode(".//span[@class='css-1089mxj e1ei9t6a2']");
                                        string spanText = spanNode != null ? spanNode.InnerText.Trim() : "";

                                        HtmlNodeCollection divNodes = car.SelectNodes(".//div[@data-ftid='component_article_extended-info']/div[@class='css-1qzcj5h e1rlzkvp0'][2]");
                                        string divText = "";
                                        if (divNodes != null)
                                        {
                                            foreach (HtmlNode divNode in divNodes)
                                            {
                                                divText = divNode.InnerText.Trim();
                                            }
                                        }

                                        string resultText = $"{spanText}\n{divText}\n{countryName}"; 

                                        if (!string.IsNullOrEmpty(resultText))
                                        {
                                            carDataList.Add(Tuple.Create(resultText, href));
                                        }
                                    }
                                }

                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show("Уведомление", $"Произошла ошибка:{ex.Message}", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                }
                attempts++;
                if (carDataList.Count == 0)
                {
                    CustomMessageBox.Show("Уведомление", $"Данные не удалось получить. Проверьте интернет соединение и повторите попытку позже.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                }
            }
            return carDataList;
        }
        public async Task<List<Tuple<string, string>>> GetCarEquipment(string url, string xpath)
        {
            List<Tuple<string, string>> data = new List<Tuple<string, string>>();

            int attempts = 0;
            while (attempts < 3)
            {
                try
                {
                    if (!(await CheckInternetConnectionAsync()))
                    {
                        break;
                    }
                    HtmlWeb webGet = new HtmlWeb();
                    HtmlDocument document = await webGet.LoadFromWebAsync(url);

                    HtmlNode parentDivNode = document.DocumentNode.SelectSingleNode(xpath);
                    if (parentDivNode == null)
                    {
                        return data;
                    }

                    HtmlNodeCollection anchorNodes = parentDivNode.SelectNodes(".//a");
                    if (anchorNodes != null)
                    {
                        foreach (HtmlNode anchorNode in anchorNodes)
                        {
                            string linkText = anchorNode.InnerText.Trim();
                            string href = anchorNode.GetAttributeValue("href", "");
                            if (!string.IsNullOrEmpty(href))
                            {
                                data.Add(Tuple.Create(linkText, href));
                            }
                        }
                    }
                    break;
                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show("Уведомление", $"Произошла ошибка:{ex.Message}", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                }
                attempts++;
            }
            if (data.Count == 0)
            {
                CustomMessageBox.Show("Уведомление", $"Данные не удалось получить. Проверьте интернет соединение и повторите попытку позже.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
            }
            return data;
        }
        public void GetAllData(string url, string xpath)
        {
            bool hybrid = false;
            List<Tuple<string, string>> info = new List<Tuple<string, string>>();
            try
            {
                HtmlWeb webGet = new HtmlWeb();
                HtmlDocument document = webGet.Load(url);

                HtmlNode parentDivNode = document.DocumentNode.SelectSingleNode(xpath);
                if (parentDivNode == null)
                {
                    return;
                }
                HtmlNodeCollection anchorNodes = parentDivNode.SelectNodes(".//td");
                if (anchorNodes != null)
                {
                    bool skipFirst = true;
                    string key = "";
                    foreach (HtmlNode tdNode in anchorNodes)
                    {
                        string text = tdNode.InnerText.Trim();
                        string svg = tdNode.InnerHtml.Trim();
                        if (svg.Contains("svg") && text.Length == 0)
                        {
                            text = "Есть";
                        }
                        string value = svg;
                        if(value.Contains("&mdash;") && key == "Гибридный автомобиль")
                        {
                            hybrid = true;
                            text = "Нет";
                        }
                        if (!IsIgnoredText(text))
                        {
                            if (skipFirst)
                            {
                                key = text;
                                skipFirst = false;
                            }
                            else
                            {
                                info.Add(new Tuple<string, string>(key, text));
                                skipFirst = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
            var filteredItems = info.Where(x => x.Item1 == "Название комплектации" ||
                                                x.Item1 == "Период выпуска" ||
                                                x.Item1 == "Тип привода" ||
                                                x.Item1 == "Тип кузова" ||
                                                x.Item1 == "Тип трансмиссии" ||
                                                x.Item1 == "Максимальная скорость, км/ч" ||
                                                x.Item1 == "Объем двигателя, куб.см" ||
                                                x.Item1 == "Число дверей" ||
                                                x.Item1 == "Число мест" ||
                                                x.Item1 == "Используемое топливо" ||
                                                x.Item1 == "Кондиционер" ||
                                                x.Item1 == "Расход топлива в смешанном цикле, л/100 км" ||
                                                x.Item1 == "Запас хода на электротяге в км" ||
                                                x.Item1 == "Ёмкость батареи, кВт*ч"
                                                ).ToList();

            ClearField();
            newCar = new Car();
            newCar.Brand = ComboBoxCarBrand.SelectedItem.ToString();
            newCar.Model = ComboBoxCarModel.SelectedItem.ToString();
            newCar.Generation = ComboBoxCarGeneration.SelectedItem.ToString().Split('\r')[0];
            newCar.Equipment = ComboBoxCarEquipment.SelectedItem.ToString();
            foreach (var item in filteredItems)
            {
                switch (item.Item1)
                {
                    case "Период выпуска":
                        string patternNumber = @"\b\d{4}\b";
                        Regex regexNumber = new Regex(patternNumber);
                        MatchCollection matchesNumber = regexNumber.Matches(item.Item2);
                        if (matchesNumber.Count == 2)
                        {
                            Year.Text = $"{matchesNumber[0].Value} - {matchesNumber[1].Value}";
                            newCar.Year = $"{matchesNumber[0].Value} - {matchesNumber[1].Value}";
                        }
                        else
                        {
                            Year.Text = $"{matchesNumber[0].Value} - н.в.";
                            newCar.Year = $"{matchesNumber[0].Value} - н.в.";
                        }
                        break;
                    case "Тип привода":
                        SelectComboBoxItemByText(Drive, item.Item2.Split(' ')[0]);
                        newCar.Drive = item.Item2.Split(' ')[0];
                        break;
                    case "Тип кузова":
                        string body = "";
                        if (item.Item2 == "SUV")
                        {
                            body = "Внедорожник";
                        }
                        else if (item.Item2 == "Открытый кузов")
                        {
                            body = "Кабриолет";
                        }
                        else
                        {
                            body = item.Item2.Split(' ')[0];
                        }
                        SelectComboBoxItemByText(Body, body);
                        newCar.Body = body;
                        break;
                    case "Тип трансмиссии":
                        string gearBox = item.Item2.Split(' ')[0];
                        if (gearBox == "РКПП" | gearBox == "АКПП" | gearBox == "Вариатор" | gearBox == "Редуктор")
                        {
                            GearBox.SelectedIndex = 0;
                            gearBox = "АКПП";
                        }
                        else if (gearBox == "МКПП")
                        {
                            GearBox.SelectedIndex = 1;
                            gearBox = "МКПП";
                        }
                        newCar.GearBox = gearBox;
                        break;
                    case "Максимальная скорость, км/ч":
                        MaxSpeed.Text = item.Item2;
                        newCar.MaxSpeed = Convert.ToInt32(item.Item2);
                        break;
                    case "Объем двигателя, куб.см":
                        TextVolume.Visibility = Visibility.Visible;
                        Volume.Visibility = Visibility.Visible;
                        Volume.Text = item.Item2;
                        newCar.Volume = Convert.ToInt32(item.Item2);
                        break;
                    case "Число дверей":
                        Doors.Text = item.Item2;
                        newCar.Doors = Convert.ToInt32(item.Item2);
                        break;
                    case "Число мест":
                        Seats.Text = item.Item2;
                        newCar.Seats = Convert.ToInt32(item.Item2);
                        break;
                    case "Используемое топливо":
                        string fuelInfo = filteredItems.Where(x => x.Item1 == "Используемое топливо").Select(x => x.Item2).FirstOrDefault();
                        string fuel = fuelInfo.Split(' ')[0];
                        switch(fuel)
                        {
                            case "Бензин":
                                TextFuelConsumption.Text = "Расход топлива в смешанном цикле, л/100 км";
                                newCar.Fuel = "Бензин";
                                EngineType.SelectedIndex = 0;
                                newCar.EngineType = "ДВС";
                                Fuel.SelectedIndex = 0;
                                string[] fuelParts = item.Item2.Split(' ');
                                if (fuelParts.Length >= 2)
                                {
                                    string patternOctaneNumber = @"АИ-\d+";
                                    Regex regexOctaneNumber = new Regex(patternOctaneNumber);
                                    MatchCollection matches = regexOctaneNumber.Matches(item.Item2);
                                    if (matches.Count != 0)
                                    {
                                        int index = octaneNumber.FindIndex(x => x.octaneNumber == matches[0].ToString());
                                        if(index != -1)
                                        {
                                            OctaneNumber.SelectedIndex = index;
                                            newCar.FuelOctane = octaneNumber[index].id;
                                        }
                                        else
                                        {
                                            index = octaneNumber.FindIndex(x => x.octaneNumber == "АИ-95");
                                            OctaneNumber.SelectedIndex = index;
                                            newCar.FuelOctane = octaneNumber[index].id;
                                        }
                                    }
                                    else
                                    {
                                        int index = octaneNumber.FindIndex(x => x.octaneNumber == "АИ-95");
                                        OctaneNumber.SelectedIndex = index;
                                        newCar.FuelOctane = octaneNumber[index].id;
                                    }
                                }
                                // сделать от года выпуска
                                else
                                {
                                    int index = octaneNumber.FindIndex(x => x.octaneNumber == "АИ-95");
                                    OctaneNumber.SelectedIndex = index;
                                    newCar.FuelOctane = octaneNumber[index].id;
                                }
                                break;
                            case "Дизельное топливо":
                                TextFuelConsumption.Text = "Расход топлива в смешанном цикле, л/100 км";
                                EngineType.SelectedIndex = 0;
                                newCar.EngineType = "ДВС";
                                TextOctaneNumber.Visibility = Visibility.Collapsed;
                                OctaneNumber.Visibility = Visibility.Collapsed;
                                Fuel.SelectedIndex = 1;
                                OctaneNumber.SelectedIndex = -1;
                                newCar.Fuel = "Дизельное топливо";
                                newCar.FuelOctane = null;
                                break;
                            case "Электричество":
                                EngineType.SelectedIndex = 1;
                                newCar.EngineType = "Электрический";
                                TextOctaneNumber.Visibility = Visibility.Collapsed;
                                OctaneNumber.Visibility = Visibility.Collapsed;
                                Fuel.SelectedIndex = 3;
                                OctaneNumber.SelectedIndex = 0;
                                newCar.Fuel = "Газ";
                                newCar.FuelOctane = null;
                                TextVolume.Visibility = Visibility.Collapsed;
                                Volume.Visibility = Visibility.Collapsed;
                                if(BatteryCapacity.Text != "")
                                {
                                    TextFuelConsumption.Text = "Расход батареии, кВт*ч/км";
                                    double fuelConsumption = Math.Round(Convert.ToDouble(BatteryCapacity.Text.Replace('.', ',')) / Convert.ToDouble(RangePerCharge.Text), 2);
                                    FuelConsumption.Text = fuelConsumption.ToString();
                                    newCar.FuelConsumption = fuelConsumption;
                                }
                                else
                                {

                                    TextFuelConsumption.Text = "Расход батареии, кВт*ч/км";
                                    FuelConsumption.Text = "нет данных";
                                    newCar.FuelConsumption = -1;
                                }
                                break;
                            case "Природный":
                                TextFuelConsumption.Text = "Расход топлива в смешанном цикле, л/100 км";
                                newCar.EngineType = "Гибрид";
                                EngineType.SelectedIndex = 2;
                                newCar.Fuel = "Газ";
                                Fuel.SelectedIndex = 3;
                                newCar.FuelOctane = null;
                                OctaneNumber.SelectedIndex = 0;
                                OctaneNumber.SelectedIndex = 0;
                                TextOctaneNumber.Visibility = Visibility.Collapsed;
                                OctaneNumber.Visibility = Visibility.Collapsed;
                                break;
                            case "Газ/бензин":
                                TextFuelConsumption.Text = "Расход топлива в смешанном цикле, л/100 км";
                                newCar.EngineType = "Гибрид";
                                EngineType.SelectedIndex = 2;
                                newCar.Fuel = "Газ";
                                Fuel.SelectedIndex = 3;
                                newCar.FuelOctane = null;
                                OctaneNumber.SelectedIndex = 0;
                                OctaneNumber.SelectedIndex = 0;
                                TextOctaneNumber.Visibility = Visibility.Collapsed;
                                OctaneNumber.Visibility = Visibility.Collapsed;
                                break;
                        }
                        break;
                    case "Кондиционер":
                        if (item.Item2 == "Есть")
                        {
                            Conditioner.SelectedIndex = 1;
                            newCar.Conditioner = "Есть";
                        }
                        else
                        {
                            Conditioner.SelectedIndex = 0;
                            newCar.Conditioner = "Нет";
                        }
                        break;
                    case "Расход топлива в смешанном цикле, л/100 км":
                        FuelConsumption.Text = item.Item2;
                        newCar.FuelConsumption = Convert.ToDouble(item.Item2.ToString());
                        break;
                    case "Ёмкость батареи, кВт*ч": 
                        BatteryCapacity.Text = item.Item2;
                        newCar.BatteryCapacity = Convert.ToDouble(item.Item2.Replace('.', ','));
                        break;
                    case "Запас хода на электротяге в км":
                        TextBlockRangePerCharge.Visibility = Visibility.Visible;
                        RangePerCharge.Visibility = Visibility.Visible;
                        RangePerCharge.Text = item.Item2;
                        newCar.RangePerCharge = Convert.ToDouble(item.Item2);
                        break;
                }
            }
            if (newCar.MaxSpeed == null)
            {
                MaxSpeed.Text = 150.ToString();
                newCar.MaxSpeed = 150;
            }
            return;
        }
        private bool IsIgnoredText(string text)
        {
            string[] ignoredTexts = {
                "Основные параметры", "Размеры", "Размеры кузова", "Размеры салона",
                "Размерности ходовой части", "Вес и допустимые нагрузки", "Объёмы", "Дополнительно",
                "Двигатель, коробка передач и рулевое управление", "Двигатель", "Электротяга",
                "Расход топлива", "Коробка передач", "Рулевое управление", "Подвеска / Ходовая часть",
                "Подвеска", "Диски", "Шины", "Тормоза", "Экстерьер и внешнее оснащение", "Фары",
                "Боковые зеркала", "Двери", "Стёкла", "Крыша", "Аксессуары", "Внутреннее оснащение",
                "Руль и центральная панель", "Исполнение салона", "Сиденья", "Электропакет салона",
                "Доп. оборудование салона", "Активные и пассивные системы безопасности",
                "Подушки безопасности", "Детская безопасность", "Активная и пассивная безопасность",
                "Электронные системы безопасности и контроля движения", "Удобства", "Микроклимат салона",
                "Аудио системы", "Навигационное и видео оборудование", "Дополнительные удобства",
                "Сохранность автомобиля", "Комплект водителя","Окраска кузова","Прочее"
            };
            return ignoredTexts.Contains(text);
        }
        public void SelectComboBoxItemByText(ComboBox comboBox, string searchText)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content.ToString() == searchText)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void Fuel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Fuel.SelectedIndex == 2)
            {
                OctaneNumber.Items.Clear();
                TextOctaneNumber.Visibility = Visibility.Collapsed;
                OctaneNumber.Visibility = Visibility.Collapsed;
            }
            else if(Fuel.SelectedIndex == 3)
            {
                OctaneNumber.Items.Clear();
                TextOctaneNumber.Visibility = Visibility.Visible;
                OctaneNumber.Visibility = Visibility.Visible;
                octaneNumber = db.GetOctaneNumber("Газ");
                foreach (var item in octaneNumber)
                {
                    string octane = item.octaneNumber.ToString();
                    OctaneNumber.Items.Add(octane);
                }
            }
            else if (Fuel.SelectedIndex != -1)
            {
                OctaneNumber.Items.Clear();
                TextOctaneNumber.Visibility = Visibility.Visible;
                OctaneNumber.Visibility = Visibility.Visible;
                octaneNumber = db.GetOctaneNumber(((ComboBoxItem)Fuel.SelectedItem).Content.ToString());
                foreach (var item in octaneNumber)
                {
                    string octane = item.octaneNumber.ToString();
                    OctaneNumber.Items.Add(octane);
                }
            }
            OctaneNumber.SelectedIndex = -1;
        }

        private void Seats_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                return;
            }

            string currentText = Seats.Text.Insert(Seats.SelectionStart, e.Text); // Вставляем новый символ в текущий текст
            if (!int.TryParse(currentText, out int seat))
            {
                e.Handled = true;
                return;
            }

            if (seat <= 0)
            {
                var messageBoxResultTwo = CustomMessageBox.Show("Уведомление", $"Число мест должно быть больше 0", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                e.Handled = true;
            }
            else if (seat > 10)
            {
                CustomMessageBox.Show("Уведомление", $"Число мест должно быть меньше или равно 10", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                e.Handled = true;
            }
        }

        private void Doord_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string currentText = Doors.Text.Insert(Doors.SelectionStart, e.Text);
            if (!int.TryParse(currentText, out int seat))
            {
                e.Handled = true;
                return;
            }

            if (seat <= 0)
            {
                CustomMessageBox.Show("Уведомление", $"Число дверей должно быть больше 0", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                e.Handled = true;
            }
            else if (seat > 5)
            {
                CustomMessageBox.Show("Уведомление", $"Число дверей должно меньше или равно 5", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                e.Handled = true;
            }
        }
        private void Volume_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                return;
            }

            string currentText = Volume.Text.Insert(Volume.SelectionStart, e.Text); 
            if (!int.TryParse(currentText, out int volume))
            {
                e.Handled = true;
                return;
            }

            if (volume <= 0)
            {
                CustomMessageBox.Show("Уведомление", $"Объем двигателя должен быть больше 0", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                e.Handled = true;
            }
            else if (volume > 10000)
            {
                CustomMessageBox.Show("Уведомление", $"Объем двигателя должен быть меньше 10000", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                e.Handled = true;
            }
        }
        private void Year_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                return;
            }

            string currentText = Year.Text.Insert(Year.SelectionStart, e.Text);
            if (!int.TryParse(currentText, out int volume))
            {
                e.Handled = true;
                return;
            }

            if (volume > 2024)
            {
                CustomMessageBox.Show("Уведомление", $"Год должен быть меньше 2024", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                e.Handled = true;
            }
        }
        public void ClearField()
        {
            foreach (UIElement element in FilterGrid.Children)
            {
                if (element is TextBox textBox)
                {
                    textBox.Clear();
                }
                else if (element is ComboBox comboBox)
                {
                    if (comboBox.Name != "ComboBoxCarBrand" & comboBox.Name != "ComboBoxCarModel" & comboBox.Name != "ComboBoxCarGeneration" & comboBox.Name != "ComboBoxCarEquipment")
                    {
                        comboBox.SelectedIndex = -1;
                    }    
                }
            }
        }
        private void BlockField()
        {
            foreach (UIElement element in FilterGrid.Children)
            {
                if (element is TextBox textBox)
                {
                    textBox.IsReadOnly = true;
                }
                else if (element is ComboBox comboBox)
                {
                    if (comboBox.Name != "ComboBoxCarBrand" & comboBox.Name != "ComboBoxCarModel" & comboBox.Name != "ComboBoxCarGeneration" & comboBox.Name != "ComboBoxCarEquipment")
                    {
                        comboBox.IsEnabled = false;
                    }
                }
            }
        }
        private void UnBlockField()
        {
            foreach (UIElement element in FilterGrid.Children)
            {
                if (element is TextBox textBox)
                {
                    textBox.IsReadOnly = false;
                }
                else if (element is ComboBox comboBox)
                {
                    comboBox.IsEnabled = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Fill.SelectedIndex == 1)
            {
                if(CarCheck(newCar)) // проверка автомобиля на заполенность
                {
                    newCar.IdUser = IdUser;
                    if(db.InsertCar(newCar))
                    {
                        CustomMessageBox.Show("Уведомление", $"Автомобиль {newCar.Brand} {newCar.Model} был добавлен в систему.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Автомобиль {newCar.Brand} {newCar.Model} не был добавлен в систему.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                    }
                }
                else
                {
                    CustomMessageBox.Show("Уведомление", $"Недостаточно информации об автомобиле {newCar.Brand} {newCar.Model}. Выберите другой автомобиль.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(CarBrand.Text) || string.IsNullOrEmpty(CarModel.Text) || string.IsNullOrEmpty(CarGeneration.Text)
                || string.IsNullOrEmpty(CarEquipment.Text) || string.IsNullOrEmpty(Year.Text) || string.IsNullOrEmpty(Seats.Text)
                || string.IsNullOrEmpty(Doors.Text) || string.IsNullOrEmpty(FuelConsumption.Text))
                {
                    CustomMessageBox.Show("Уведомление", $"Пожалуйста, заполните все обязательные поля.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    return;
                }

                int seats, doors;
                double volume = 0 ,fuelConsumption, rangePerCharge = 0, batteryCapacity = 0;
                if (!int.TryParse(Seats.Text, out seats) || seats <= 0)
                {
                     CustomMessageBox.Show("Предупреждение", $"Пожалуйста, введите корректное значение для количества сидений.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    return;
                }

                if (!int.TryParse(Doors.Text, out doors) || doors <= 0)
                {
                    CustomMessageBox.Show("Предупреждение", $"Пожалуйста, введите корректное значение для количества дверей.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    return;
                }

                int? maxSpeed = null;
                if (MaxSpeed.Text != "")
                {
                    if (!int.TryParse(MaxSpeed.Text, out int parsedMaxSpeed) || parsedMaxSpeed <= 0)
                    {
                        CustomMessageBox.Show("Предупреждение", $"Пожалуйста, введите корректное значение для максимальной скорости.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                        return;
                    }
                    maxSpeed = parsedMaxSpeed;
                }

                if (Volume.Visibility == Visibility.Visible)
                {
                    if (!double.TryParse(Volume.Text, out volume) || volume <= 0)
                    {
                        CustomMessageBox.Show("Предупреждение", $"Пожалуйста, введите корректное значение для объема двигателя.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                        return;
                    }
                }
                if (!double.TryParse(FuelConsumption.Text, out fuelConsumption) || fuelConsumption <= 0)
                {
                    CustomMessageBox.Show("Предупреждение", $"Пожалуйста, введите корректное значение для расхода топлива.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    return;
                }
                if (RangePerCharge.Visibility == Visibility.Visible)
                {
                    if (!double.TryParse(RangePerCharge.Text, out rangePerCharge) || rangePerCharge <= 0)
                    {
                        CustomMessageBox.Show("Предупреждение", $"Пожалуйста, введите корректное значение для пробега на одном заряде.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                        return;
                    }
                }
                if (RangePerCharge.Visibility == Visibility.Visible)
                {
                    if (!double.TryParse(BatteryCapacity.Text, out batteryCapacity) || batteryCapacity <= 0)
                    {
                        CustomMessageBox.Show("Предупреждение", $"Пожалуйста, введите корректное значение для емкости батареи.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                        return;
                    }
                }
                if (Body.SelectedItem == null || Drive.SelectedItem == null || GearBox.SelectedItem == null
                    || EngineType.SelectedItem == null || Fuel.SelectedItem == null || Conditioner.SelectedItem == null)
                {
                    CustomMessageBox.Show("Предупреждение", $"Пожалуйста, выберите значение для всех параметров.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    return;
                }

                Car newCar = new Car()
                {
                    Brand = CarBrand.Text,
                    Model = CarModel.Text,
                    Generation = CarGeneration.Text,
                    Equipment = CarEquipment.Text,
                    Year = Year.Text,
                    Body = ((ComboBoxItem)Body.SelectedItem).Content.ToString(),
                    Drive = ((ComboBoxItem)Drive.SelectedItem).Content.ToString(),
                    GearBox = ((ComboBoxItem)GearBox.SelectedItem).Content.ToString(),
                    Seats = seats,
                    Doors = doors,
                    MaxSpeed = MaxSpeed.Text != "" ? maxSpeed : (int?)null,
                    EngineType = ((ComboBoxItem)EngineType.SelectedItem).Content.ToString(),
                    Volume = Volume.Visibility == Visibility.Visible ? volume : (double?)null,
                    Fuel = ((ComboBoxItem)Fuel.SelectedItem).Content.ToString(),
                    FuelOctane = OctaneNumber.Visibility == Visibility.Visible ? OctaneNumber.SelectedIndex : (int?)null,
                    Conditioner = ((ComboBoxItem)Conditioner.SelectedItem).Content.ToString(),
                    FuelConsumption = fuelConsumption,
                    RangePerCharge = RangePerCharge.Visibility == Visibility.Visible ? rangePerCharge : (double?)null,
                    BatteryCapacity = BatteryCapacity.Visibility == Visibility.Visible ? batteryCapacity : (double?)null
                };
                if (CarCheck(newCar))
                {
                    newCar.IdUser = IdUser;
                    if (db.InsertCar(newCar))
                    {
                        CustomMessageBox.Show("Уведомление", $"Автомобиль {newCar.Brand} {newCar.Model} был добавлен в систему.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                    }
                    else
                    {
                        CustomMessageBox.Show("Уведомление", $"Автомобиль {newCar.Brand} {newCar.Model} не был добавлен в систему.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                    }
                }
                else
                {
                    CustomMessageBox.Show("Уведомление", $"Недостаточно информации об автомобиле {newCar.Brand} {newCar.Model}. Выберите другой автомобиль.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                }
            }
        }
        private bool CarCheck(Car car)
        {
            if((car.Model != "" && car.Brand != "" && car.Generation != "" && car.Year != "" && car.Body != "" && car.Drive != "" && car.GearBox != "" && car.Seats != 0 && car.Doors != 0 && (car.EngineType == "ДВС" | car.EngineType == "Гибрид") && car.Fuel != "" && car.FuelConsumption > 0)
              |(car.Model != "" && car.Brand != "" && car.Generation != "" && car.Year != "" && car.Body != "" && car.Drive != "" && car.GearBox != "" && car.Seats != 0 && car.Doors != 0 && car.EngineType == "Электрический" && car.Fuel != "" && car.FuelConsumption > 0 && car.BatteryCapacity > 0 && car.RangePerCharge > 0))
            {
                return true;
            }
            return false;
        }
        private void Button_Web_Click(object sender, RoutedEventArgs e)
        {
            if (Fill.SelectedIndex == 1)
            {
                if (ComboBoxCarEquipment.SelectedIndex != -1)
                {
                    Process.Start($"https://www.drom.ru/{carEquipment[ComboBoxCarEquipment.SelectedIndex].Item2}");
                }
                else
                {
                    CustomMessageBox.Show("Уведомление", $"Выберите автмобиль и повторите попытку", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);

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
    }
}
