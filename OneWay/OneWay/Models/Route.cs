namespace OneWay.Class
{
    internal class Route
    {
        public int IdRoute { get; set; }
        public int IdUser { get; set; }
        public string Name { get; set; }
        public int? PointOne { get; set; }
        public int? PointTwo { get; set; }
        public string AdditionPoint { get; set; }
        public double Distance { get; set; }
        public string TravelTime { get; set; }
        public Route() { }
    }
}
