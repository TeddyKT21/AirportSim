using AirportSimCore.Interfaces;
using AirportSimCore.Models;
using Microsoft.Extensions.Logging;
using ServerDal.Repositories;
using System;

namespace ServerLogic.Models
{
    public class TerminalLeg :IEntity
    {
        public int Id { get; set; }
        private int _legNumber;
        public int LegNumber {get { return _legNumber;}}

        private bool _isOperating;
        public bool IsOperating { get { return _isOperating;}}

        private int _duration;
        public int Duration { get { return _duration;}} 
        private Flight? _currentFlight;
        public Flight CurrentFlight 
        { 
            get
            {
                return _currentFlight!;
            }
            internal set
            {
               lock(this)
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException("flight cannot be null !!!");
                    }
                    if (_currentFlight != null)
                    {
                        throw new InvalidOperationException($"flight Transfer to occupied Leg (no. {_legNumber})  !!!");
                    }

                    _currentFlight = value;
                    OnEntering(_currentFlight);
                    Task.Run(() =>
                    {
                        DoTerminalStuff();
                        var nextLegs = getNextLegs(value);
                        if (nextLegs != null)
                        {
                            if (nextLegs.Count == 0)
                            {
                                TransferFlight();

                            }
                            foreach (var leg in nextLegs)
                            {
                                lock (leg)
                                {
                                    if (leg.CurrentFlight == null && _currentFlight!=null)
                                    {
                                        TransferFlight(leg);
                                        break;
                                    }
                                }
                            }
                        }
                    });
                }
            }
        }

        private ILogger<TerminalLeg> _logger;
        private IFlightLogCreator _flightLogCreator;
        private ITerminalLegHub _hub;

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

        private List<TerminalLeg>? _nextLegsDeparting;
        private List<TerminalLeg>? _nextLegsArriving;
        internal event Action<TerminalLeg>? _onEmptiedLeg;


        internal void AddLegToArriving(TerminalLeg leg)
        {
            _nextLegsArriving!.Add(leg);
            leg._onEmptiedLeg += MoveToNextTerminal;
        }

        internal void AddLegToDeparting(TerminalLeg leg)
        {
            _nextLegsDeparting!.Add(leg);
            leg._onEmptiedLeg += MoveToNextTerminal;
        }

        private void DoTerminalStuff() 
        {
            lock(this)
            {
                _isOperating = true;
            }
            Thread.Sleep(1000 * _duration);
            lock (this)
            {
                _isOperating = false;
                _hub.SendPendingStatus(_legNumber);
            }
        }

        private void MoveToNextTerminal(TerminalLeg freedUpLeg)
        {
            lock (freedUpLeg)
            {
                if (_currentFlight == null || _isOperating || freedUpLeg.CurrentFlight != null)
                {
                    return;
                }
                if (IsRightTerminal(freedUpLeg))
                {

                    TransferFlight(freedUpLeg);

                }
            }
        }

        private bool IsRightTerminal(TerminalLeg freedUpLeg) 
        { 
            if(_currentFlight.IsDeparting && _nextLegsDeparting.Contains(freedUpLeg))
            {
                return true;
            }
            if (!_currentFlight.IsDeparting && _nextLegsArriving.Contains(freedUpLeg))
            {
                return true;
            }
            return false;
        }

        private void TransferFlight(TerminalLeg? freedUpLeg = null)
        {
            if(_currentFlight == null)
            {
                return;
            }
           OnLeaving(_currentFlight);
            if (freedUpLeg != null)
            {
                freedUpLeg.CurrentFlight = CurrentFlight;
            }

            _currentFlight = null;
            Task.Run(() => _onEmptiedLeg!(this));
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

        private List<TerminalLeg> getNextLegs(Flight flight)
        {
            List<TerminalLeg>? nextLegs = null;
            if (flight!.IsDeparting)
            {
                nextLegs = _nextLegsDeparting!;
            }
            else
            {
                nextLegs = _nextLegsArriving!;
            }
            return nextLegs;
        }

    }
}