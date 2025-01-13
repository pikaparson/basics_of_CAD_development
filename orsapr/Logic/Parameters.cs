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
        private readonly Dictionary<StoolParameters, Tuple<int, int>> _minMaxValues = new Dictionary<StoolParameters, Tuple<int, int>>
        {
            {StoolParameters.SeatLength, new Tuple<int, int>(300, 400) },
            {StoolParameters.SeatDiameter, new Tuple<int, int>(300, 400) },
            {StoolParameters.SeatWidth, new Tuple<int, int>(300, 600) },
            {StoolParameters.SeatThickness, new Tuple<int, int>(20, 35) },
            {StoolParameters.LegsHeight, new Tuple<int, int>(300, 400) },
            {StoolParameters.LegsWightAndLength, new Tuple<int, int>(25, 35) },
            {StoolParameters.LegsDiameter, new Tuple<int, int>(25, 35) },
            };

        /// <summary>
        /// Словарь зависимых параметров табурета.
        /// </summary>
        private readonly Dictionary<StoolParameters, StoolParameters> DependentParameters = new Dictionary<StoolParameters, StoolParameters>
        {
            {StoolParameters.LegsHeight, StoolParameters.SeatThickness},
            {StoolParameters.SeatThickness, StoolParameters.LegsHeight},
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
        public bool IsWrongValue(int value, StoolParameters valueName, out Tuple<int, int> minMax)
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
        public Tuple<int, int> AdjustMinValues(int value, StoolParameters parameter)
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