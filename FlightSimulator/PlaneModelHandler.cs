using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator
{
    internal class PlaneModelHandler
    {
        private readonly string[]? _planeModels;
        private readonly int _minPassengerCount;
        public PlaneModelHandler? Next { get;  set; }

        public PlaneModelHandler(string[]? planeModels, int capacity = 0)
        {
            _planeModels = planeModels;
            _minPassengerCount = capacity;
        }

        public string GetPlaneModel(int PassengerCount)
        {
            if(PassengerCount >= _minPassengerCount)
            {
                Random random = new();
                return _planeModels![random.Next(_planeModels.Length)];
            }
            return Next!.GetPlaneModel(PassengerCount);
        }
    }
}
