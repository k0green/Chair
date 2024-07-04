namespace Chair.BLL.Dto.Base
{
    public class Place
    {
        public Position Position { get; set; }
        public string Address { get; set; }
    }

    public class Position
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}