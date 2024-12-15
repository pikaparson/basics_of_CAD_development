using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using KompasAPI7;
using Logic;

namespace Logic
{
    public class Builder
    {
        private Wrapper _wrapper;

        public Builder()
        {
            _wrapper = new Wrapper();
        }

        public void BuildChair(Parameters parameters)
        {
            _wrapper.OpenCad();

            IPart7 part = _wrapper.CreatePart();
            BuildSeat(part, parameters);
            BuildLegs(part, parameters);
        }

        private void BuildSeat(IPart7 part, Parameters parameters)
        {
            ISketch sketch = _wrapper.CreateSketch(part, "Эскиз: сидушка");
            _wrapper.CreateRectangle(sketch, 0, 0, parameters.SeatWidth, parameters.SeatLength);
            _wrapper.ExtrudeSketch(sketch, parameters.SeatThickness, "Сидушка", false);
        }

        private void BuildLegs(IPart7 part, Parameters parameters)
        {
            var coords = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(0, 0),
                new Tuple<int, int>(parameters.SeatWidth - parameters.LegWidth, 0),
                new Tuple<int, int>(0, parameters.SeatLength - parameters.LegWidth),
                new Tuple<int, int>(parameters.SeatWidth - parameters.LegWidth, parameters.SeatLength - parameters.LegWidth)
            };
            
            int legNumber = 0;
            foreach (var point in coords)
            {
                legNumber++;
                ISketch legSketch = _wrapper.CreateSketch(part, "Эскиз: Ножка " + legNumber);
                _wrapper.CreateRectangle(legSketch, point.Item1, point.Item2, parameters.LegWidth, parameters.LegWidth);
                _wrapper.ExtrudeSketch(legSketch, -parameters.LegLength, "Элемент выдавливания: Ножка " + legNumber, false);
            }
        }
    }
}