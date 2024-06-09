namespace OneWay.Class
{
    internal class Trip
    {
        public int IdTrip { get; set; }
        public int IdUser { get; set; }
        public int IdRoute { get; set; }
        public int IdCar { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public double UsedFuel { get; set; }
        public double UsedMoney { get; set; }
        public int People { get; set; }

        public Trip()
        {}
    }
}
