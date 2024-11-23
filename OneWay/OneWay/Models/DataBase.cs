using OneWay.Controls;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows;

namespace OneWay.Class
{
    internal class DataBase
    {
        private string connectionString;
        public DataBase(string databasePath)
        {
            connectionString = $"Data Source={databasePath};";
        }

        /* пользователи */
        public string GetFirstNameById(int userId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT FirstName FROM User WHERE IdUser = @IdUser";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", userId);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            return result.ToString();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return null;
            }
        }

        public bool InsertUser(User user)
        {
            string insertQuery = "INSERT INTO User (FirstName, Login, Password, RegistrationDate) " +
                                 "VALUES (@FirstName, @Login, @Password, @RegistrationDate)";

            string selectQuery = "SELECT COUNT(*) FROM User WHERE Login = @Login AND Password = @Password";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@Login", user.Login);
                        selectCommand.Parameters.AddWithValue("@Password", user.Password);
                        object result = selectCommand.ExecuteScalar();
                        int userCount = result != null ? Convert.ToInt32(result) : 0;
                        if (userCount > 0)
                        {
                            return false;
                        }
                    }
                    using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                        insertCommand.Parameters.AddWithValue("@Login", user.Login);
                        insertCommand.Parameters.AddWithValue("@Password", user.Password);
                        insertCommand.Parameters.AddWithValue("@RegistrationDate", user.RegistrationDate);

                        int rowsAffected = insertCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false;
            }
        }

        public int CheckUser(string login, string password)
        {
            string selectQuery = "SELECT IdUser FROM User WHERE Login = @Login AND Password = @Password";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);

                        object result = command.ExecuteScalar();
                        if (result != null) // проверка результата
                        {
                            int idUser = Convert.ToInt32(result);
                            return idUser;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }
            catch (Exception ex) // обработка исключений
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return -1;
            }
        }

        /* автомобили */
        public bool InsertCar(Car car)
        {
            string query = @"INSERT INTO Car (IdUser, Brand, Model, Generation, Equipment, Year, Body, Drive, GearBox, Seats, Doors, MaxSpeed, EngineType, Volume, Fuel, FuelOctan, FuelConsumption, Conditioner, RangePerCharge, BatteryCapacity) 
                     VALUES (@IdUser, @Brand, @Model, @Generation, @Equipment, @Year, @Body, @Drive, @GearBox, @Seats, @Doors, @MaxSpeed, @EngineType, @Volume, @Fuel, @FuelOctan, @FuelConsumption, @Conditioner, @RangePerCharge, @BatteryCapacity)";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", car.IdUser);
                        command.Parameters.AddWithValue("@Brand", car.Brand);
                        command.Parameters.AddWithValue("@Model", car.Model);
                        command.Parameters.AddWithValue("@Generation", car.Generation.Split('\r')[0]);
                        command.Parameters.AddWithValue("@Equipment", car.Equipment);
                        command.Parameters.AddWithValue("@Year", car.Year);
                        command.Parameters.AddWithValue("@Body", car.Body);
                        command.Parameters.AddWithValue("@Drive", car.Drive);
                        command.Parameters.AddWithValue("@GearBox", car.GearBox);
                        command.Parameters.AddWithValue("@Seats", car.Seats);
                        command.Parameters.AddWithValue("@Doors", car.Doors);
                        command.Parameters.AddWithValue("@MaxSpeed", car.MaxSpeed);
                        command.Parameters.AddWithValue("@EngineType", car.EngineType);
                        command.Parameters.AddWithValue("@Volume", car.Volume);
                        command.Parameters.AddWithValue("@Fuel", car.Fuel);
                        if (car.FuelOctane.HasValue)
                        {
                            command.Parameters.AddWithValue("@FuelOctan", car.FuelOctane);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@FuelOctan", DBNull.Value);
                        }

                        command.Parameters.AddWithValue("@Conditioner", car.Conditioner);
                        command.Parameters.AddWithValue("@FuelConsumption", car.FuelConsumption);
                        if (car.RangePerCharge.HasValue)
                        {
                            command.Parameters.AddWithValue("@RangePerCharge", car.RangePerCharge);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RangePerCharge", DBNull.Value);
                        }
                        if (car.BatteryCapacity.HasValue)
                        {
                            command.Parameters.AddWithValue("@BatteryCapacity", car.BatteryCapacity);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@BatteryCapacity", DBNull.Value);
                        };
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false;
            }
        }


        public bool DeleteCar(int carId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string deleteFavoriteCarQuery = @"DELETE FROM FavoriteCar WHERE IdCar = @IdCar";
                    using (SQLiteCommand deleteFavoriteCarCommand = new SQLiteCommand(deleteFavoriteCarQuery, connection))
                    {
                        deleteFavoriteCarCommand.Parameters.AddWithValue("@IdCar", carId);
                        deleteFavoriteCarCommand.ExecuteNonQuery();
                    }

                    string deleteTripsQuery = @"DELETE FROM Trips WHERE IdCar = @IdCar";
                    using (SQLiteCommand deleteTripsCommand = new SQLiteCommand(deleteTripsQuery, connection))
                    {
                        deleteTripsCommand.Parameters.AddWithValue("@IdCar", carId);
                        deleteTripsCommand.ExecuteNonQuery();
                    }

                    string deleteCarQuery = @"DELETE FROM Car WHERE IdCar = @IdCar";
                    using (SQLiteCommand deleteCarCommand = new SQLiteCommand(deleteCarQuery, connection))
                    {
                        deleteCarCommand.Parameters.AddWithValue("@IdCar", carId);
                        int rowsAffected = deleteCarCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return true; 
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false; 
            }
        }


        public List<Car> GetAllCars(int userId)
        {
            List<Car> allCars = new List<Car>();

            string favoriteCarsQuery = @"SELECT c.* FROM Car c INNER JOIN FavoriteCar fc ON c.IdCar = fc.IdCar WHERE fc.IdUser = @IdUser;";
            string otherCarsQuery = @"SELECT * FROM Car WHERE IdUser = @IdUser AND IdCar NOT IN (SELECT IdCar FROM FavoriteCar WHERE IdUser = @IdUser);";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(favoriteCarsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", userId);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Car newCar = ReadCarFromReader(reader);
                                allCars.Add(newCar);
                            }
                        }
                    }

                    using (SQLiteCommand command = new SQLiteCommand(otherCarsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", userId);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Car newCar = ReadCarFromReader(reader);
                                allCars.Add(newCar);
                            }
                        }
                    }
                }
                return allCars;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new List<Car>();
            }
        }


        public List<Car> GetFilterCars(string filter, int IdUser)
        {
            List<Car> allCars = new List<Car>();
            string otherCarsQuery = $"SELECT * FROM Car WHERE IdUser = @IdUser ";
            if (!string.IsNullOrEmpty(filter))
            {
                otherCarsQuery += $" AND {filter}";
            }
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(otherCarsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", IdUser);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                allCars.Add(ReadCarFromReader(reader));
                            }
                        }
                    }
                }
                return allCars;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new List<Car>();
            }
        }


        public Car GetCar(int IdCar)
        {
            Car car = new Car();
            string favoriteCarsQuery = $"SELECT * FROM Car WHERE IdCar = @IdCar";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(favoriteCarsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@IdCar", IdCar);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                car = ReadCarFromReader(reader);
                            }
                        }
                    }
                }
                return car;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new Car();
            }
        }
        

        private Car ReadCarFromReader(SQLiteDataReader reader)
        {
            return new Car
            {
                IdCar = reader.GetInt32(0),
                IdUser = reader.GetInt32(1),
                Brand = reader.GetString(2),
                Model = reader.GetString(3),
                Generation = reader.GetString(4),
                Equipment = reader.GetString(5),
                Year = reader.GetString(6),
                Body = reader.GetString(7),
                Drive = reader.GetString(8),
                GearBox = reader.GetString(9),
                Seats = reader.GetInt32(10),
                Doors = reader.GetInt32(11),
                MaxSpeed = reader.GetInt32(12),
                EngineType = reader.GetString(13),
                Volume = reader.IsDBNull(14) ? 0 : reader.GetDouble(14),
                Fuel = reader.GetString(15),
                FuelOctane = reader.IsDBNull(16) ? 0 : reader.GetInt32(16),
                Conditioner = reader.GetString(17),
                FuelConsumption = reader.GetDouble(18),
                RangePerCharge = reader.IsDBNull(19) ? 0 : reader.GetDouble(19),
                BatteryCapacity = reader.IsDBNull(20) ? 0 : reader.GetDouble(20)
            };
        }


        /* любимые автомобили*/
        public int CheckFavoriteCar(int userId, int carId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM FavoriteCar WHERE IdUser = @IdUser AND IdCar = @IdCar";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", userId);
                        command.Parameters.AddWithValue("@IdCar", carId);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0 ? 1 : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return -1; 
            }
        }


        public bool AddFavoriteCar(int userId, int carId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO FavoriteCar (IdUser, IdCar) VALUES (@IdUser, @IdCar)";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", userId);
                        command.Parameters.AddWithValue("@IdCar", carId);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false;
            }
        }


        public bool DeleteFavoriteCar(int userId, int carId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM FavoriteCar WHERE IdUser = @IdUser AND IdCar = @IdCar";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", userId);
                        command.Parameters.AddWithValue("@IdCar", carId);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false;
            }
        }


        /// Топливо
        public List<Fuel> GetFuelTypes(int userId)
        {
            List<Fuel> fuelInfoList = new List<Fuel>();
           /* string query = @"
                SELECT f.FuelType, f.OctaneNumber, fph.Price
                FROM Fuel f
                JOIN (SELECT IdFuel, MAX(DateChanged) AS MaxDate FROM FuelPriceHistory GROUP BY IdFuel) 
                AS latest_date ON f.IdFuel = latest_date.IdFuel
                JOIN FuelPriceHistory fph ON f.IdFuel = fph.IdFuel AND fph.DateChanged = latest_date.MaxDate
                WHERE fph.IdUser = @UserId OR IdUser = -1";*/
           string query = @"
                SELECT f.FuelType, f.OctaneNumber, fph.Price FROM Fuel AS f JOIN (
                SELECT IdFuel, Price
                FROM FuelPriceHistory
                WHERE IdHistory IN (SELECT MAX(IdHistory) FROM FuelPriceHistory WHERE IdUser = @UserId OR IdUser = -1
                GROUP BY IdFuel)) AS fph ON f.IdFuel = fph.IdFuel
                ORDER BY f.IdFuel;";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Fuel fuelInfo = new Fuel
                                {
                                    FuelType = reader.GetString(0),
                                    OctaneNumber = reader.GetString(1),
                                    Price = Math.Round(reader.GetDouble(2), 2)
                                };
                                fuelInfoList.Add(fuelInfo);
                            }
                        }
                    }
                }

                return fuelInfoList;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new List<Fuel>();
            }
        }
        public int GetFuelId(string name)
        {
            string query = @"
                SELECT IdFuel FROM Fuel WHERE OctaneNumber = @OctaneNumber";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OctaneNumber", name);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                return reader.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return 0;
            }
            return 0;
        }
        public string GetFuelName(int IdFuel)
        {
            string query = @"
                SELECT OctaneNumber FROM Fuel WHERE IdFuel = @IdFuel";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdFuel", IdFuel);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                return reader.GetString(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return null;
            }
            return null;
        }

        public List<(int id, string octaneNumber)> GetOctaneNumber(string fuel)
        {
            List<(int id, string octaneNumber)> octaneNumber = new List<(int id, string octaneNumber)>();
            string query = $"SELECT  IdFuel ,OctaneNumber FROM Fuel WHERE FuelType = '{fuel}'";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                octaneNumber.Add((reader.GetInt32(0), reader.GetString(1)));
                            }
                        }
                    }
                }

                return octaneNumber;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new List<(int id, string octaneNumber)>();
            }
        }


        public List<Fuel> GetFilterFuelTypes(List<string> selectedFuelTypes)
        {
            List<Fuel> fuelInfoList = new List<Fuel>();

            string query = @"SELECT f.FuelType, f.OctaneNumber, fph.Price
                            FROM Fuel f
                            JOIN (  SELECT IdFuel, MAX(DateChanged) AS MaxDate
                                    FROM FuelPriceHistory
                                    GROUP BY IdFuel) 
                            AS latest_date ON f.IdFuel = latest_date.IdFuel
                            JOIN FuelPriceHistory fph ON f.IdFuel = fph.IdFuel AND fph.DateChanged = latest_date.MaxDate";

            if (selectedFuelTypes.Count > 0)
            {
                query += " WHERE f.FuelType IN (";
                query += string.Join(",", selectedFuelTypes.Select(fuelType => $"'{fuelType}'"));
                query += ")";
            }
            query += ";";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Fuel fuelInfo = new Fuel
                                {
                                    FuelType = reader.GetString(0),
                                    OctaneNumber = reader.GetString(1),
                                    Price = Math.Round(reader.GetDouble(2), 2)
                                };
                                fuelInfoList.Add(fuelInfo);
                            }
                        }
                    }
                }
                return fuelInfoList;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new List<Fuel>();
            }
        }


        public string GetLastDate()
        {
            string query = "SELECT DateChanged FROM FuelPriceHistory WHERE IdUser = -1 ORDER BY IdHistory DESC LIMIT 1";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string dateChanged = reader.GetString(0);
                                DateTime parsedDate;
                                if (DateTime.TryParse(dateChanged, out parsedDate))
                                {
                                    return parsedDate.ToString("dd.MM.yyyy");//////////
                                }
                                else
                                {
                                    throw new Exception("Ошибка при преобразовании даты");
                                }
                            }
                            else
                            {
                                throw new Exception("Запись с указанным IdUser не найдена");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return null;
            }
        }


        public List<(double price, DateTime date)> GetFuelPriceHistory(int IdFuel, int IdUser)
        {
            List<(double price, DateTime date)> priceHistory = new List<(double price, DateTime date)>();
            string query = @"SELECT Price, DateChanged
                FROM FuelPriceHistory
                WHERE IdFuel = @IdFuel AND (IdUser = -1 OR IdUser = @IdUser) ORDER BY IdHistory DESC LIMIT 4";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", IdUser);
                        command.Parameters.AddWithValue("@IdFuel", IdFuel);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                double price = reader.GetDouble(0);
                                DateTime date = Convert.ToDateTime(reader.GetString(1));
                                priceHistory.Add((price, date));
                            }
                        }
                    }
                }
                priceHistory.Reverse();
                //priceHistory.Sort((x, y) => x.date.CompareTo(y.date));
                return priceHistory;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new List<(double price, DateTime date)>();
            }
        }


        public string GetFuel(int IdFuel)
        {
            string favoriteCarsQuery = $"SELECT OctaneNumber FROM Fuel WHERE IdFuel = @IdFuel";
            string name = "";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(favoriteCarsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@IdFuel", IdFuel);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                name  =  reader.GetString(0);
                            }
                        }
                    }
                }
                return name;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return null;
            }
        }


        public bool UpdatePrice(int idFuel, int idUser, string price, string data) 
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    if(idUser == -1)
                    {
                        string checkQuery = "SELECT COUNT(*) FROM FuelPriceHistory WHERE IdFuel = @IdFuel AND DateChanged = @DateChanged";
                        using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
                        {
                            checkCommand.Parameters.AddWithValue("@IdFuel", idFuel);
                            checkCommand.Parameters.AddWithValue("@DateChanged", data);
                            int existingRecords = Convert.ToInt32(checkCommand.ExecuteScalar());
                            if (existingRecords > 0)
                            {
                                return false;
                            }
                        }
                    }
                    string insertQuery = "INSERT INTO FuelPriceHistory (IdFuel, IdUser, Price, DateChanged) VALUES (@IdFuel, @IdUser, @Price, @DateChanged)";
                    using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@IdFuel", idFuel);
                        command.Parameters.AddWithValue("@IdUser", idUser);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@DateChanged", data);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false;
            }
        }


        // Города
        public bool InsertPoint(string name, string coordinates,int idUser)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT COUNT(*) FROM Points WHERE Name = @Name AND IdUser = @IdUser";
                    using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@Name", name.Split(',')[name.Split(',').Length - 1]);
                        selectCommand.Parameters.AddWithValue("@IdUser", idUser);
                        int existingCount = Convert.ToInt32(selectCommand.ExecuteScalar());
                        if (existingCount > 0)
                        {
                            return false;
                        }
                    }
                    string insertQuery = @"INSERT INTO Points (IdUser, Name, Coordinates) VALUES (@IdUser, @Name, @Coordinates)";
                    using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", idUser);
                        command.Parameters.AddWithValue("@Name", name.Split(',')[name.Split(',').Length - 1]);
                        command.Parameters.AddWithValue("@Coordinates", coordinates);
                        command.ExecuteNonQuery();
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false;
            }
        }


        public List<Points> GetAllPoints(int userId)
        {
            List<Points> allPoints = new List<Points>();

            string query = @"SELECT IdPoint, Name, Coordinates FROM Points WHERE IdUser = @IdUser;";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", userId);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Points newPoint = new Points
                                {
                                    IdPoint = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Coordinates = !reader.IsDBNull(2) ? reader.GetString(2) : null
                                };
                                allPoints.Add(newPoint);
                            }
                        }
                    }
                }

                return allPoints;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new List<Points>();
            }
        }


        public int GetCityIdByName(int IdUser, string cityName)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT IdPoint FROM Points WHERE Name = @Name AND IdUser = @IdUser";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", cityName);
                        command.Parameters.AddWithValue("@IdUser", IdUser);
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int cityId))
                        {
                            return cityId;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return -1;
            }
        }


        //маршруты
        public List<Route> GetAllRoutes(int userId)
        {
            List<Route> allRoutes = new List<Route>();

            string query = @"SELECT r.IdRoute, r.Name, r.PointOne, r.PointTwo, r.AdditionPoint, r.Distance, r.TravelTime 
                            FROM Routes r
                            LEFT JOIN FavoriteRoute f ON r.IdRoute = f.IdRoute
                            WHERE r.IdUser = @IdUser
                            ORDER BY f.IdFavoriteRoute DESC;";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", userId);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Route newRoute = new Route
                                {
                                    IdRoute = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    PointOne = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                                    PointTwo = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                    AdditionPoint = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Distance = reader.GetDouble(5),
                                    TravelTime = reader.GetString(6)
                                };
                                allRoutes.Add(newRoute);
                            }
                        }
                    }
                }

                return allRoutes;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new List<Route>();
            }
        }


        public bool InsertRoute(Route route)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT COUNT(*) FROM Routes WHERE IdUser = @IdUser AND PointOne = @PointOne AND PointTwo = @PointTwo AND Distance = @Distance";
                    using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@IdUser", route.IdUser);
                        selectCommand.Parameters.AddWithValue("@PointOne", route.PointOne);
                        selectCommand.Parameters.AddWithValue("@PointTwo", route.PointTwo);
                        selectCommand.Parameters.AddWithValue("@Distance", route.Distance);
                        int existingCount = Convert.ToInt32(selectCommand.ExecuteScalar());
                        if (existingCount > 0)
                        {
                            return false;
                        }
                    }

                    string insertQuery = @"INSERT INTO Routes (IdUser, Name, PointOne, PointTwo, AdditionPoint, Distance, TravelTime) 
                                   VALUES (@IdUser, @Name, @PointOne, @PointTwo, @AdditionPoint, @Distance, @TravelTime)";
                    using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", route.IdUser);
                        command.Parameters.AddWithValue("@Name", route.Name);
                        command.Parameters.AddWithValue("@PointOne", route.PointOne);
                        command.Parameters.AddWithValue("@PointTwo", route.PointTwo);
                        command.Parameters.AddWithValue("@AdditionPoint", route.AdditionPoint);
                        command.Parameters.AddWithValue("@Distance", route.Distance);
                        command.Parameters.AddWithValue("@TravelTime", route.TravelTime);
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false;
            }
        }


        public bool DeleteRoute(int id)
        {
            DeleteTripByIdRoute(id);
            string query = "DELETE FROM Routes WHERE IdRoute = @IdRoute;";
            bool success = false;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdRoute", id);
                        int rowsAffected = command.ExecuteNonQuery();
                        success = rowsAffected > 0;
                    }
                }

                return success;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false;
            }
        }


        public List<Route> SelectRoute(string name, int userId)
        {
            List<Route> allRoutes = new List<Route>();

            string query = $"SELECT IdRoute, Name, PointOne, PointTwo, AdditionPoint, Distance,TravelTime FROM Routes WHERE NAME = @Name AND IdUser = @IdUser;";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@IdUser", userId);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Route newRoute = new Route
                                {
                                    IdRoute = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    PointOne = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                                    PointTwo = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                    AdditionPoint = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Distance = reader.GetDouble(5),
                                    TravelTime = reader.GetString(6)
                                };
                                allRoutes.Add(newRoute);
                            }
                        }
                    }
                }

                return allRoutes;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new List<Route>();
            }
        }


        public (string, string, double, string, string) GetRouteInfoById(int routeId)
        {
            string startPoint = "";
            string finishPoint = "";
            double distance = 0;
            string travelTime = "";
            string points = "";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Points.Name AS DeparturePoint, Points2.Name AS ArrivalPoint, Routes.Distance, Routes.TravelTime, Routes.AdditionPoint " +
                                    "FROM Routes " +
                                    "JOIN Points ON Routes.PointOne = Points.IdPoint " +
                                    "JOIN Points AS Points2 ON Routes.PointTwo = Points2.IdPoint " +
                                    "WHERE Routes.IdRoute = @RouteId";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RouteId", routeId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                startPoint = reader["DeparturePoint"].ToString();
                                finishPoint = reader["ArrivalPoint"].ToString();
                                distance = Convert.ToDouble(reader["Distance"]);
                                travelTime = reader["TravelTime"].ToString();
                                points = reader["AdditionPoint"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
            }
            return (startPoint, finishPoint, distance, travelTime, points);
        }


        //любимые маршруты
        public int CheckFavoriteRoute(int userId, int routeId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM FavoriteRoute WHERE IdUser = @IdUser AND IdRoute = @IdRoute";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", userId);
                        command.Parameters.AddWithValue("@IdRoute", routeId);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0 ? 1 : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return -1;
            }
        }


        public bool AddFavoriteRoute(int userId, int routeId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO FavoriteRoute (IdUser, IdRoute) VALUES (@IdUser, @IdRoute)";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", userId);
                        command.Parameters.AddWithValue("@IdRoute", routeId);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false;
            }
        }


        public bool DeleteFavoriteRoute(int userId, int routeId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM FavoriteRoute WHERE IdUser = @IdUser AND IdRoute = @IdRoute";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", userId);
                        command.Parameters.AddWithValue("@IdRoute", routeId);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false;
            }
        }


        public List<Route> SelectRoutesByPoints(int userId, int pointOne, int pointTwo)
        {
            List<Route> selectedRoutes = new List<Route>();
            ///
            string query = $"SELECT IdRoute, Name, PointOne, PointTwo, AdditionPoint, Distance, TravelTime FROM Routes WHERE IdUser = @IdUser";
            if (pointOne != -1)
            {
                query += " AND PointOne = @PointOne";
            }
            if (pointTwo != -1)
            {
                query += " AND PointTwo = @PointTwo";
            }
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PointOne", pointOne);
                        command.Parameters.AddWithValue("@PointTwo", pointTwo);
                        command.Parameters.AddWithValue("@IdUser", userId);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Route newRoute = new Route
                                {
                                    IdRoute = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    PointOne = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                                    PointTwo = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                    AdditionPoint = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Distance = reader.GetDouble(5),
                                    TravelTime = reader.GetString(6)
                                };
                                selectedRoutes.Add(newRoute);
                            }
                        }
                    }
                }
                return selectedRoutes;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new List<Route>();
            }
        }


        //поездки
        public bool InsertTrip(Trip trip)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = @"INSERT INTO Trips (IdUser, IdRoute, IdCar, StartTime, FinishTime, UsedFuel, UsedMoney, People) 
                                       VALUES (@IdUser, @IdRoute, @IdCar, @StartTime, @FinishTime, @UsedFuel, @UsedMoney, @People)";
                    using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection)) // добавление параметров запроса
                    {
                        command.Parameters.AddWithValue("@IdUser", trip.IdUser);
                        command.Parameters.AddWithValue("@IdRoute", trip.IdRoute);
                        command.Parameters.AddWithValue("@IdCar", trip.IdCar);
                        command.Parameters.AddWithValue("@StartTime", trip.StartTime);
                        command.Parameters.AddWithValue("@FinishTime", trip.FinishTime);
                        command.Parameters.AddWithValue("@UsedFuel", trip.UsedFuel);
                        command.Parameters.AddWithValue("@UsedMoney", trip.UsedMoney);
                        command.Parameters.AddWithValue("@People", trip.People);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false; 
            }
        }


        public List<Trip> GetAllTrips()
        {
            List<Trip> trips = new List<Trip>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM Trips";
                    using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Trip trip = new Trip
                                {
                                    IdTrip = Convert.ToInt32(reader["IdTrip"]),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    IdRoute = Convert.ToInt32(reader["IdRoute"]),
                                    IdCar = Convert.ToInt32(reader["IdCar"]),
                                    StartTime = Convert.ToString(reader["StartTime"]),
                                    FinishTime = Convert.ToString(reader["FinishTime"]),
                                    UsedFuel = Convert.ToDouble(reader["UsedFuel"]),
                                    UsedMoney = Convert.ToDouble(reader["UsedMoney"]),
                                    People = Convert.ToInt32(reader["People"])
                                };
                                trips.Add(trip);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
            }
            return trips;
        }


        public bool DeleteTripByIdRoute(int id)
        {
            string query = "DELETE FROM Trips WHERE IdRoute = @IdRoute;";
            bool success = false;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdRoute", id);
                        int rowsAffected = command.ExecuteNonQuery();
                        success = rowsAffected > 0;
                    }
                }

                return success;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false;
            }
        }


        public double GetDistance(int routeId)
        {
            double distance = 0;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT Distance FROM Routes WHERE IdRoute = @IdRoute";
                    using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@IdRoute", routeId);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            distance = Convert.ToDouble(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
            }
            return distance;
        }


        public double GetFuelLastPrice(int? fuelId)
        {
            double lastPrice = 0;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT Price FROM FuelPriceHistory WHERE IdFuel = @IdFuel ORDER BY DateChanged DESC LIMIT 1";
                    using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@IdFuel", fuelId);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            lastPrice = Convert.ToDouble(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
            }
            return lastPrice;
        }


        public string GetTime(int routeId)
        {
            string travelTime = "";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT TravelTime FROM Routes WHERE IdRoute = @IdRoute";
                    using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@IdRoute", routeId);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            travelTime = Convert.ToString(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
            }
            return travelTime;
        }


        //поездки
        public List<Trip> GetAllTripByUserId(int userId)
        {
            List<Trip> trips = new List<Trip>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Trips WHERE IdUser = @UserId ORDER BY IdTrip DESC";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Trip trip = new Trip
                                {
                                    IdTrip = Convert.ToInt32(reader["IdTrip"]),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    IdRoute = Convert.ToInt32(reader["IdRoute"]),
                                    IdCar = Convert.ToInt32(reader["IdCar"]),
                                    StartTime = reader["StartTime"].ToString(),
                                    FinishTime = reader["FinishTime"].ToString(),
                                    UsedFuel = Convert.ToDouble(reader["UsedFuel"]),
                                    UsedMoney = Convert.ToDouble(reader["UsedMoney"]),
                                    People = Convert.ToInt32(reader["People"])
                                };

                                trips.Add(trip);
                            }
                        }
                    }
                }
                return trips;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new List<Trip>();
            }
        }


        public bool DeleteTrip(int id)
        {
            bool success = false;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Trips WHERE IdTrip = @Id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            success = true;
                        }
                    }
                }

                return success;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return false;
            }
        }


        public List<Trip> GetAllTripByUserIdAndDateRange(int userId, string filter)
        {
            List<Trip> trips = new List<Trip>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM Trips WHERE IdUser = @UserId {filter}";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Trip trip = new Trip
                                {
                                    IdTrip = Convert.ToInt32(reader["IdTrip"]),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    IdRoute = Convert.ToInt32(reader["IdRoute"]),
                                    IdCar = Convert.ToInt32(reader["IdCar"]),
                                    StartTime = reader["StartTime"].ToString(),
                                    FinishTime = reader["FinishTime"].ToString(),
                                    UsedFuel = Convert.ToDouble(reader["UsedFuel"]),
                                    UsedMoney = Convert.ToDouble(reader["UsedMoney"]),
                                    People = Convert.ToInt32(reader["People"])
                                };

                                trips.Add(trip);
                            }
                        }
                    }
                }
                return trips;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new List<Trip>();
            }
        }

        public List<Trip> GetHistory(string query)
        {
            List<Trip> trips = new List<Trip>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Trip trip = new Trip
                                {
                                    IdTrip = Convert.ToInt32(reader["IdTrip"]),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    IdRoute = Convert.ToInt32(reader["IdRoute"]),
                                    IdCar = Convert.ToInt32(reader["IdCar"]),
                                    StartTime = reader["StartTime"].ToString(),
                                    FinishTime = reader["FinishTime"].ToString(),
                                    UsedFuel = Convert.ToDouble(reader["UsedFuel"]),
                                    UsedMoney = Convert.ToDouble(reader["UsedMoney"]),
                                    People = Convert.ToInt32(reader["People"])
                                };

                                trips.Add(trip);
                            }
                        }
                    }
                }
                return trips;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new List<Trip>();
            }
        }

        //статистика
        public Trip GetTripById(int idTrip)
        {
            Trip trip = null;
            try
            {

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Trips WHERE IdTrip = @IdTrip";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdTrip", idTrip);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                trip = new Trip
                                {
                                    IdTrip = Convert.ToInt32(reader["IdTrip"]),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    IdRoute = Convert.ToInt32(reader["IdRoute"]),
                                    IdCar = Convert.ToInt32(reader["IdCar"]),
                                    StartTime = reader["StartTime"].ToString(),
                                    FinishTime = reader["FinishTime"].ToString(),
                                    UsedFuel = Convert.ToDouble(reader["UsedFuel"]),
                                    UsedMoney = Convert.ToDouble(reader["UsedMoney"]),
                                    People = Convert.ToInt32(reader["People"])
                                };
                            }
                        }
                    }
                }
                return trip;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return new Trip();
            }

        }
        
        public (int, double) GetCountAndSpendMoney(int userId)
        {
            int totalTrips = 0;
            double totalMoneySpent = 0.0;
            string query = @"SELECT
            COUNT(IdTrip) AS TotalTrips,
            SUM(UsedMoney) AS TotalMoney
            FROM 
                Trips
            WHERE 
                IdUser = @UserId
                AND StartTime >= DATE('now', '-1 month');";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (!(reader["TotalTrips"] is DBNull))
                                    totalTrips = Convert.ToInt32(reader["TotalTrips"]);

                                if (!(reader["TotalMoney"] is DBNull))
                                    totalMoneySpent = Convert.ToDouble(reader["TotalMoney"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
            }
            return (totalTrips, totalMoneySpent);
        }


        public int GetFavoriteCarCount(int userId)
        {
            int count = 0;
            string query = @"SELECT 
                Count(IdUser) as Count 
                FROM FavoriteCar
                WHERE IdUser = @UserId";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                count = Convert.ToInt32(reader["Count"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
            }
            return count;
        }

        public double GetDistanceByMonth(int userId)
        {
            double totalDistance = 0.0;
            string query = @"
                SELECT
                    SUM(R.Distance) AS Distance
                FROM
                    Trips AS T
                JOIN
                    Routes AS R ON T.IdRoute = R.IdRoute
                WHERE
                    T.IdUser = @UserId
                AND T.StartTime >= DATE('now', '-1 month');";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (!(reader["Distance"] is DBNull))
                                    totalDistance = Convert.ToDouble(reader["Distance"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Уведомление", $"Произошла ошибка {ex.Message}.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
            }

            return totalDistance;
        }
    }
}
