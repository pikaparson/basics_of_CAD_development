using System;
using System.Collections.Generic;
using System.Reflection;

namespace BusinessLogic
{

    /// <summary>
    /// Класс, представляющий параметры конструкции стула.
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Словарь с минимальными и максимальными значениями параметров.
        /// </summary>
        private readonly Dictionary<ChairParameters, Tuple<int, int>> _minMaxValues = new Dictionary<ChairParameters, Tuple<int, int>>
        {
            {ChairParameters.SeatLength, new Tuple<int, int>(300, 400) },
            {ChairParameters.SeatDiameter, new Tuple<int, int>(300, 400) },
            {ChairParameters.SeatWidth, new Tuple<int, int>(300, 600) },
            {ChairParameters.SeatThickness, new Tuple<int, int>(20, 35) },
            {ChairParameters.LegsHeight, new Tuple<int, int>(300, 400) },
            {ChairParameters.LegsWightAndLength, new Tuple<int, int>(25, 35) },
            {ChairParameters.LegsDiameter, new Tuple<int, int>(25, 35) },
            };

        /// <summary>
        /// Словарь зависимых параметров табурета.
        /// </summary>
        private readonly Dictionary<ChairParameters, ChairParameters> DependentParameters = new Dictionary<ChairParameters, ChairParameters>
        {
            {ChairParameters.LegsHeight, ChairParameters.SeatThickness},
            {ChairParameters.SeatThickness, ChairParameters.LegsHeight},
        };

        /// <summary>
        /// Константа, обозначающая минимальную длину табурета.
        /// </summary>
        public const int dependentParametersMinSum = 330;

        /// <summary>
        /// Константа, обозначающая максимальную длину табурета.
        /// </summary>
        public const int dependentParametersMaxSum = 435;

        /// <summary>
        /// Свойство для доступа к длине сиденья.
        /// </summary>
        public int SeatLength { get; set; }

        /// <summary>
        /// Свойство для доступа к ширине сиденья.
        /// </summary>
        public int SeatWidth { get; set; }

        /// <summary>
        /// Свойство для доступа к толщине сиденья.
        /// </summary>
        public int SeatThickness { get; set; }

        /// <summary>
        /// Свойство для доступа к длине ножки.
        /// </summary>
        public int LegLength { get; set; }

        /// <summary>
        /// Свойство для доступа к ширине ножки.
        /// </summary>
        public int LegWidth { get; set; }

        /// <summary>
        /// Свойство для доступа к высоте ножки.
        /// </summary>
        public int LegHeight { get; set; }

        //TODO: в автосвойства+
        /// <summary>
        /// Тип ножек табурета.
        /// По умолчанию заданы квадратные ножки табурета.
        /// </summary>
        public LegTypes LegsType { get; set; }

        /// <summary>
        /// Тип сиденья табурета.
        /// По умолчанию задано прямоугольное/квадратное сиденье табурета.
        /// </summary>
        public SeatTypes SeatType { get; set; }

        /// <summary>
        /// Валидация значений свойств.
        /// </summary>
        /// <param name="value">Значение свойства.</param>
        /// <param name="valueName">Параметр табурета.</param>
        /// <param name="minMax">Границы значений свойств.</param>
        public bool IsWrongValue(int value, ChairParameters valueName, out Tuple<int, int> minMax)
        {
            var current = _minMaxValues[valueName];
            minMax = current;

            return !(current.Item1 <= value && value <= current.Item2);
        }

        /// <summary>
        /// Метод для проверки зависимости параметров.
        /// </summary>
        /// <returns>Возвращает true, если сумма толщины сиденья и длины 
        /// ножки не меньше 330; в противном случае - false.</returns>
        public bool CheckDependentParametersValue()
        {
            return (SeatThickness + LegLength) >= dependentParametersMinSum
                && (SeatThickness + LegLength) <= dependentParametersMaxSum;
        }

        /// <summary>
        /// Изменение границ значения зависимого параметра.
        /// </summary>
        /// <param name="value">Значение параметра.</param>
        /// <param name="parameter">Тип параметра.</param>
        /// <returns>Новый диапазон значений зависимого параметра.</returns>
        public Tuple<int, int> AdjustMinValues(int value, ChairParameters parameter)
        {
            var depentParameter = DependentParameters[parameter];
            var oldMin = _minMaxValues[depentParameter].Item1;
            var oldMax = _minMaxValues[depentParameter].Item2;

            if (dependentParametersMinSum - value < oldMin)
            {
                return new Tuple<int, int>(oldMin, oldMax);
            }

            return new Tuple<int, int>(dependentParametersMinSum - value, oldMax);
        }
    }

}