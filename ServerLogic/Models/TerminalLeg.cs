using AirportSimCore.Interfaces;
using AirportSimCore.Models;
using Microsoft.Extensions.Logging;
using ServerDal.Repositories;
using System;

namespace ServerLogic.Models
{
    public class TerminalLeg : IEntity
    {
        public int Id { get; set; }
        private int _legNumber;
        public int LegNumber { get { return _legNumber; } }

        private bool _isOperating;
        public bool IsOperating { get { return _isOperating; } }

        private int _duration;
        public int Duration { get { return _duration; } }
        private Flight? _currentFlight;

        private List<TerminalLeg>? _nextLegsDeparting;
        private List<TerminalLeg>? _nextLegsArriving;
        internal event Action<TerminalLeg>? OnEmptiedLeg;

        private readonly ILogger<TerminalLeg> _logger;
        private readonly IFlightLogCreator _flightLogCreator;
        private readonly ITerminalLegHub _hub;

        public TerminalLeg(int legNumber, int duration, IFlightLogCreator flightLogCreator, ITerminalLegHub myHub, ILogger<TerminalLeg> logger)
        {
            _logger = logger;
            _flightLogCreator = flightLogCreator;
            _hub = myHub;
            _duration = duration;
            _legNumber = legNumber;
            _currentFlight = null;
            _nextLegsDeparting = new List<TerminalLeg>();
            _nextLegsArriving = new List<TerminalLeg>();
            _isOperating = false;
        }
        public Flight CurrentFlight
        {
            get
            {
                return _currentFlight!;
            }
            internal set
            {
                lock (this)
                {
                    CheckValid(value);
                    _currentFlight = value;
                    OnEntering(_currentFlight);
                    _ = HandleFlight(value);
                }
            }
        }

        private void CheckValid(Flight value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("flight cannot be null !!!");
            }
            if (_currentFlight != null)
            {
                throw new InvalidOperationException($"flight Transfer to occupied Leg (no. {_legNumber})  !!!");
            }
        }

        private Task HandleFlight(Flight value)
        {
            return Task.Run(() =>
            {
                DoTerminalStuff();
                SearchForNextLeg(value);
            });
        }

        private void SearchForNextLeg(Flight value)
        {
            var nextLegs = GetNextLegs(value);
            if (nextLegs.Count == 0)
            {
                TransferFlight();
            }
            foreach (var leg in nextLegs)
            {
                lock (leg)
                {
                    if (leg.CurrentFlight == null && _currentFlight != null)
                    {
                        TransferFlight(leg);
                        break;
                    }
                }
            }
        }
        internal void AddLegToArriving(TerminalLeg leg)
        {
            _nextLegsArriving!.Add(leg);
            leg.OnEmptiedLeg += MoveToNextLeg;
        }

        internal void AddLegToDeparting(TerminalLeg leg)
        {
            _nextLegsDeparting!.Add(leg);
            leg.OnEmptiedLeg += MoveToNextLeg;
        }

        private void DoTerminalStuff()
        {
            lock (this)
            {
                _isOperating = true;
            }
            Thread.Sleep(1000 * _duration);
            lock (this)
            {
                _isOperating = false;
            }
            _hub.SendPendingStatus(_legNumber);
        }

        private void MoveToNextLeg(TerminalLeg freedUpLeg)
        {
            lock (freedUpLeg)
            {
                if (_currentFlight != null && !_isOperating && freedUpLeg.CurrentFlight == null && IsRightLeg(freedUpLeg))
                {
                    TransferFlight(freedUpLeg);
                }
            }
        }

        private bool IsRightLeg(TerminalLeg freedUpLeg) => FlightAndLegDeparting(freedUpLeg) || FlightAndLegArriving(freedUpLeg);
        private bool FlightAndLegDeparting(TerminalLeg freedUpLeg) => _currentFlight!.IsDeparting && _nextLegsDeparting!.Contains(freedUpLeg);
        private bool FlightAndLegArriving(TerminalLeg freedUpLeg) => !_currentFlight!.IsDeparting && _nextLegsArriving!.Contains(freedUpLeg);

        private void TransferFlight(TerminalLeg? freedUpLeg)
        {
            if (_currentFlight != null)
            {
                OnLeaving(_currentFlight);
                freedUpLeg!.CurrentFlight = CurrentFlight;
                _currentFlight = null;
                Task.Run(() => OnEmptiedLeg!(this));
            }
        }

        private void TransferFlight()
        {
            if (_currentFlight != null)
            {
                OnLeaving(_currentFlight);
                _currentFlight = null;
                Task.Run(() => OnEmptiedLeg!(this));
            }
        }

        private void OnLeaving(Flight currentFlight)
        {
            _flightLogCreator.CreateLogWhenExiting(_legNumber, currentFlight!.Id);
            _logger.LogInformation($"Flight number {currentFlight!.FlightNumber} left leg {_legNumber} at {DateTime.Now}");
            _hub.SendLeavingMessege(_legNumber, currentFlight);
        }
        private void OnEntering(Flight currentFlight)
        {
            _flightLogCreator.CreateLogWhenEntering(_legNumber, currentFlight!.Id);
            _logger.LogInformation($"Flight number {currentFlight!.FlightNumber} entered leg {_legNumber} at {DateTime.Now}");
            _hub.SendEnteringMessege(_legNumber, currentFlight);
        }

        private List<TerminalLeg> GetNextLegs(Flight flight) => flight!.IsDeparting ? _nextLegsDeparting! : _nextLegsArriving!;
    }
}