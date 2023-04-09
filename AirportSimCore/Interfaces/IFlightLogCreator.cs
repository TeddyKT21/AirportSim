namespace AirportSimCore.Interfaces
{
    public interface IFlightLogCreator
    {
        public void CreateLogWhenEntering(int legNumber, int flightId);
        public void CreateLogWhenExiting(int legNumber, int flightId);
    }
}
