namespace ConsoleSimulator.Models
{
    //DTO = Data Transfer Object
    public class FlightDto
    {
        public int FlightNumber { get; set; }

        public bool IsDeparting { get; set; }

        public int NumberOfPassengers { get; set; }

        public string PlaneModel { get; set; }
        
        public string? AirLine { get; set; }

        public DateTime MadeContactAt { get; set; }

        private static readonly string[] _veryLargePlaneModels = { "Boing 747-800", "Boing 747-400", "Airbus A380-900", "Airbus A380-800" };
        private static readonly string[] _largePlaneModels = { "Boing 777-9", "Boing 777-300", "Airbus A350-900", "Airbus A350-1000", "Boing 787-8", "Boing 787-9", "Boing 787-10", "Airbus A340-200", "Airbus A340-300" };
        private static readonly string[] _medumPlaneModels = { "Boing 737-900", "Boing 737-800", "Airbus A320", "Airbus A321", "Airbus A330-200", "Airbus A330-300", "Airbus A318", "Airbus A330-900" };
        private static readonly string[] _smallPlaneModels = { "Airbus A220-100", "Airbus A220-300", "Boing 737-600", "Boing 737-700" };
        private static readonly string[] _airlines = { "Delta Air", "American Airlines", "Lufthansa", "Air France", "Southwest", "Emirates", "British Airways", "El Al", "EasyJet", "AirAsia" };
        public FlightDto()
        {
            Random random = new();
            FlightNumber = random.Next(1000000);
            random = new();
            IsDeparting = random.Next(0, 2) == 0;
            NumberOfPassengers = random.Next(90, 550);
            PlaneModel = GetPlaneModel();
            MadeContactAt = DateTime.Now;
            AirLine = _airlines[random.Next(_airlines.Length)];
        }

        private string GetPlaneModel()
        {
            Random random = new();
            string[] planeModels = { };
            if (NumberOfPassengers > 370)
            {
                planeModels = _veryLargePlaneModels;
            }
            else if (NumberOfPassengers > 220)
            {
                planeModels = _largePlaneModels;
            }
            else if (NumberOfPassengers > 130)
            {
                planeModels = _medumPlaneModels;
            }
            else
            {
                planeModels = _smallPlaneModels;
            }
            return planeModels[random.Next(planeModels.Length)];
        }

        public override string ToString()
        {
            string status = "Arriving";
            if (IsDeparting)
            {
                status = "Departing";
            }
            return $"flight number: {FlightNumber}, Plane model: {PlaneModel} ,passengers: {NumberOfPassengers}, status:{status}, registered at airport at:{MadeContactAt:G}";
        }
    }
}