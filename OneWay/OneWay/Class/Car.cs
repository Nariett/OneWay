namespace OneWay.Class
{
    internal class Car
    {
        public int IdCar { get; set; }
        public int IdUser { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Generation { get; set; }
        public string Equipment { get; set; }
        public string Year { get; set; }
        public string Body { get; set; }
        public string Drive { get; set; }
        public string GearBox { get; set; }
        public int Seats { get; set; }
        public int Doors { get; set; }
        public int? MaxSpeed { get; set; }
        public string EngineType { get; set; }
        public double? Volume {  get; set; }
        public string Fuel { get; set; }
        public int? FuelOctane { get; set; }
        public string Conditioner { get; set; }
        public double FuelConsumption { get; set; }
        public double? RangePerCharge { get; set; }
        public double? BatteryCapacity { get; set; }
        public Car() 
        {
            Conditioner = "Нету";
        }
    }
}
