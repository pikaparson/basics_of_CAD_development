using System;
using System.Collections.Generic;
using KompasAPI7;
using Logic;

namespace API_singly
{
    public class Builder
    {
        private Wrapper _wrapper;

        public Builder()
        {
            _wrapper = new Wrapper();
        }

        public void BuildChair(Parameters parameters, SeatTypes seatType, LegTypes legType)
        {
            _wrapper.OpenCad();

            IPart7 part = _wrapper.CreatePart();
            BuildSeat(part, parameters, seatType);
            BuildLegs(part, parameters, seatType, legType);
        }

        private void BuildSeat(IPart7 part, Parameters parameters, SeatTypes type)
        {
            ISketch sketch = _wrapper.CreateSketch(part, "Эскиз: сидушка");
            switch (type)
            {
                case SeatTypes.SquareSeat:
                    {
                        _wrapper.CreateRectangle(sketch, 0, 0, parameters.SeatWidth, parameters.SeatLength);
                        break;
                    }
                case SeatTypes.RoundSeat:
                    {
                        _wrapper.CreateCircle(sketch, 0, 0, parameters.SeatWidth);
                        break;
                    }
            }
            
            _wrapper.ExtrudeSketch(sketch, parameters.SeatThickness, "Сидушка", false);
        }

        private void BuildLegs(IPart7 part, Parameters parameters, SeatTypes seatType, LegTypes legType)
        {
            int legNumber = 0;
            legNumber++;
            ISketch legSketch = _wrapper.CreateSketch(part, "Эскиз: Ножка " + legNumber);
            if (seatType == SeatTypes.SquareSeat)
            {
                var coords = new List<Tuple<int, int>>
                {
                    new Tuple<int, int>(0, 0),
                    new Tuple<int, int>(parameters.SeatWidth - parameters.LegWidth, 0),
                    new Tuple<int, int>(0, parameters.SeatLength - parameters.LegWidth),
                    new Tuple<int, int>(parameters.SeatWidth - parameters.LegWidth, parameters.SeatLength - parameters.LegWidth)
                };
                foreach (var point in coords)
                {
                    switch (legType)
                    {
                        case LegTypes.SquareLeg:
                            {
                                _wrapper.CreateRectangle(legSketch, point.Item1, point.Item2, parameters.LegWidth, parameters.LegWidth);
                                break;
                            }
                        case LegTypes.RoundLeg:
                            {
                                _wrapper.CreateCircle(legSketch, point.Item1 + parameters.LegWidth / 2, point.Item2 + parameters.LegWidth / 2, parameters.LegWidth);
                                break;
                            }
                    }
                    _wrapper.ExtrudeSketch(legSketch, -parameters.LegLength, "Элемент выдавливания: Ножка " + legNumber, false);
                }
            }
            else if (seatType == SeatTypes.RoundSeat)
            {
                var coords = new List<Tuple<int, int>>
                {
                    new Tuple<int, int>(-parameters.SeatWidth / 2, -parameters.LegWidth / 2),
                    new Tuple<int, int>(-parameters.LegWidth / 2, -parameters.SeatLength / 2),
                    new Tuple<int, int>(parameters.SeatWidth / 2 - parameters.LegWidth, -parameters.LegWidth / 2),
                    new Tuple<int, int>(-parameters.LegWidth / 2 , parameters.SeatLength / 2 - parameters.LegWidth)
                };
                foreach (var point in coords)
                {
                    switch (legType)
                    {
                        case LegTypes.SquareLeg:
                            {
                                _wrapper.CreateRectangle(legSketch, point.Item1, point.Item2, parameters.LegWidth, parameters.LegWidth);
                                break;
                            }
                        case LegTypes.RoundLeg:
                            {
                                _wrapper.CreateCircle(legSketch, point.Item1 + parameters.LegWidth / 2, point.Item2 + parameters.LegWidth / 2, parameters.LegWidth);
                                break;
                            }
                    }
                    _wrapper.ExtrudeSketch(legSketch, -parameters.LegLength, "Элемент выдавливания: Ножка " + legNumber, false);
                }
            }
        }
    }
}