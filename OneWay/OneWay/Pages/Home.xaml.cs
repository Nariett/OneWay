using LiveCharts;
using OneWay.Class;
using OneWay.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;

namespace OneWay.Pages
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _timeOfDay;
        public string TimeOfDay
        {
            get => _timeOfDay;
            set
            {
                _timeOfDay = value;
                OnPropertyChanged(nameof(TimeOfDay));
            }
        }

        private int _trip;
        public int Trip
        {
            get => _trip;
            set
            {
                _trip = value;
                OnPropertyChanged(nameof(Trip));
                TripDescription = GetPluralString(Convert.ToInt32(Trip), "поездка за месяц", "поездки за месяц", "поездок за месяц");
            }
        }

        private string _tripDescription;
        public string TripDescription
        {
            get => _tripDescription;
            set
            {
                _tripDescription = value;
                OnPropertyChanged(nameof(TripDescription));
            }
        }

        private int _favoriteCar;
        public int FavoriteCar
        {
            get => _favoriteCar;
            set
            {
                _favoriteCar = value;
                OnPropertyChanged(nameof(FavoriteCar));
                FavoriteCarDescription = GetPluralString(Convert.ToInt32(FavoriteCar), "избранное авто", "избранных авто", "избранных авто");
            }
        }

        private string _favoriteCarDescription;
        public string FavoriteCarDescription
        {
            get => _favoriteCarDescription;
            set
            {
                _favoriteCarDescription = value;
                OnPropertyChanged(nameof(FavoriteCarDescription));
            }
        }

        private int _distanceMonth;
        public int DistanceMonth
        {
            get => _distanceMonth;
            set
            {
                _distanceMonth = value;
                OnPropertyChanged(nameof(DistanceMonth));
                DistanceMonthDescription = GetPluralString(Convert.ToInt32(DistanceMonth), "преодален за месяц", "преодалено за месяц", "преодалено за месяц");
            }
        }

        private string _distanceMonthDescription;
        public string DistanceMonthDescription
        {
            get => _distanceMonthDescription;
            set
            {
                _distanceMonthDescription = value;
                OnPropertyChanged(nameof(DistanceMonthDescription));
            }
        }

        private int _money;
        public int Money
        {
            get => _money;
            set
            {
                _money = value;
                OnPropertyChanged(nameof(Money));
                MoneyDescription = GetPluralString(Convert.ToInt32(Money), "потрачен за месяц", "потрачено за месяц", "потрачено за месяц");

            }
        }

        private string _moneyDescription;
        public string MoneyDescription
        {
            get => _moneyDescription;
            set
            {
                _moneyDescription = value;
                OnPropertyChanged(nameof(MoneyDescription));
            }
        }


        public HomeViewModel()
        {
            UpdateAll();
        }

        public void UpdateAll()
        {
            TimeOfDay = GetTimeOfDay(DateTime.Now);
            TripDescription = GetPluralString(Convert.ToInt32(Trip), "поездка за месяц", "поездки за месяц", "поездкок за месяц");
            FavoriteCarDescription = GetPluralString(Convert.ToInt32(FavoriteCar), "избранное авто", "избранных авто", "избранных авто");
            DistanceMonthDescription = GetPluralString(Convert.ToInt32(DistanceMonth), "преодален за месяц", "преодалено за месяц", "преодалено за месяц");
            MoneyDescription = GetPluralString(Convert.ToInt32(Money), "потрачен за месяц", "потрачено за месяц", "потрачено за месяц");
        }

        private string GetTimeOfDay(DateTime date)
        {
            int hour = date.Hour;
            if (hour >= 0 && hour < 6)
            {
                return "Доброй ночи!";
            }
            else if (hour >= 6 && hour < 12)
            {
                return "Доброго утра!";
            }
            else if (hour >= 12 && hour < 18)
            {
                return "Доброго дня!";
            }
            else
            {
                return "Доброго вечера!";
            }
        }
        private string GetPluralString(int count, string singular, string plural, string pluralMany)
        {
            if (count < 0)
                return "";

            int lastDigit = count % 10;
            int lastTwoDigits = count % 100;

            if (lastTwoDigits >= 11 && lastTwoDigits <= 14)
            {
                return pluralMany;
            }

            switch (lastDigit)
            {
                case 1:
                    return singular;
                case 2:
                case 3:
                case 4:
                    return plural;
                default:
                    return pluralMany;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class Home : Page
    {
        public SeriesCollection Data { get; set; }
        DataBase db = new DataBase("OneWayDB.db");
        List<Car> allCars = new List<Car>();
        List<Trip> allTrip = new List<Trip>();
        public int IdUser;
        public string UserName;

        public Home(int IdUser)
        {
            InitializeComponent();
            this.IdUser = IdUser;
            HomeViewModel ViewModel = new HomeViewModel();
            DataContext = ViewModel;
            StartTimer();
            allCars = db.GetAllCars(IdUser);
            CreateItemsCar(allCars);
            this.name.Text = db.GetFirstNameById(IdUser);
            allTrip = db.GetAllTripByUserId(IdUser);
            CreateItemsTrip(allTrip);

            var data = db.GetCountAndSpendMoney(IdUser);
            trip.Text = data.Item1.ToString();
            ((HomeViewModel)DataContext).Trip = data.Item1;

            money.Text = data.Item2.ToString() + " руб";
            ((HomeViewModel)DataContext).Money = Convert.ToInt32(data.Item2);

            var count = db.GetFavoriteCarCount(IdUser);
            favoriteCar.Text = count.ToString();
            ((HomeViewModel)DataContext).FavoriteCar = count;

            var info = db.GetDistanceByMonth(IdUser);
            distanceMonth.Text = info.ToString() + " км";
            ((HomeViewModel)DataContext).DistanceMonth = Convert.ToInt32(info);

        }
        private void CreateItemsCar(List<Car> cars)
        {
            foreach (var item in cars)
            {
                ShortCarItem car = new ShortCarItem();
                car.CarName = $"{item.Brand} {item.Model}";
                car.Seat = item.Seats.ToString();
                if (item.Volume.ToString() == "0")
                {
                    car.Volume = $"{item.EngineType}";
                }
                else
                {
                    car.Volume = $"{Math.Round(item.Volume.Value / 1000, 2)} / {item.Fuel}";
                }
                car.Body = item.Body;
                car.Transmission = item.GearBox;
                //car.MouseLeftButtonDown += CarItemClick;
                MyCar.Children.Add(car);
            }
        }
        private void CreateItemsTrip(List<Class.Trip> trips)
        {
            foreach (var item in trips)
            {
                try
                {
                    var info = db.GetRouteInfoById(item.IdRoute);
                    string startPoint = info.Item1;
                    string finishPoint = info.Item2;
                    double distance = info.Item3;
                    string travelTime = info.Item4;
                    string points = info.Item5;
                    ShortTripItem trip = new ShortTripItem();
                    trip.Trip = startPoint + " - " + finishPoint;

                    TimeSpan date = TimeSpan.FromMinutes(Convert.ToDouble(travelTime));
                    if (date.Days >= 1)
                    {
                        if (date.Hours == 0)
                        {
                            trip.Time = $"{date.Days} д {date.Minutes} мин";
                        }
                        else
                        {
                            trip.Time = $"{date.Days} д {date.Hours} ч {date.Minutes} мин";
                        }
                    }
                    else
                    {
                        if (date.Hours != 0)
                        {
                            trip.Time = $"{date.Hours} ч {date.Minutes} мин";
                        }
                        else
                        {
                            trip.Time = $"{date.Minutes} мин";
                        }
                    }
                    trip.Distanse = distance + " км";
                    Car car = db.GetCar(item.IdCar);
                    if (car.Brand != null)
                    {
                        trip.Car = car.Brand;
                    }
                    else
                    {
                        trip.Car = "неизвестный";
                    }
                    MyTrip.Children.Add(trip);
                }
                catch
                {
                   
                }
            }
        }

        public void StartTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            string timeNow = DateTime.Now.ToString("HH:mm:ss");
            clock.Text = timeNow;
        }
    }
}
