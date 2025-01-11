using System;
using System.Collections.Generic;
using System.Reflection;

namespace Logic
{
    /// <summary>
    /// Класс, представляющий параметры конструкции стула
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Закрытое поле для хранения длины сиденья
        /// </summary>
        private int _seatLength;

        /// <summary>
        /// Закрытое поле для хранения ширины сиденья
        /// </summary>
        private int _seatWidth;

        /// <summary>
        /// Закрытое поле для хранения толщины сиденья
        /// </summary>
        private int _seatThickness;

        /// <summary>
        /// Закрытое поле для хранения длины ножки
        /// </summary>
        private int _legLength;

        /// <summary>
        /// Закрытое поле для хранения ширины ножки
        /// </summary>
        private int _legWidth;

        /// <summary>
        /// Закрытое поле для хранения высоты ножки
        /// </summary>
        private int _legHeight;
       
        /// <summary>
        /// Словарь с минимальными и максимальными значениями параметров
        /// </summary>
        private Dictionary<string, Tuple<int, int>> _minMaxValues;
        
        /// <summary>
        /// Константа, обозначающая минимальную длину табурета
        /// </summary>
        public const int dependentParametersSumm = 330;
        
        /// <summary>
        /// Конструктор по умолчанию, инициализирует параметры значениями
        /// по умолчанию
        /// </summary>
        public Parameters()
        {
            _seatLength = 0;
            _seatWidth = 0;
            _seatThickness = 0;
            _legLength = 0;
            _legWidth = 0;
            _legHeight = 0;

            _minMaxValues = new Dictionary<string, Tuple<int, int>>
            {
                //TODO: refactor
                {"Длина сиденья", new Tuple<int, int>(300, 400) },
                {"Диаметр сиденья", new Tuple<int, int>(300, 400) },
                {"Ширина сиденья", new Tuple<int, int>(300, 600) },
                {"Толщина сиденья", new Tuple<int, int>(20, 35) },
                {"Высота ножек", new Tuple<int, int>(300, 400) },
                {"Ширина и длина ножек", new Tuple<int, int>(25, 35) },
                {"Диаметр ножек", new Tuple<int, int>(25, 35) },
            };
        }


        /// <summary>
        /// Метод для проверки зависимости параметров
        /// </summary>
        /// <returns>Возвращает true, если сумма толщины сиденья и длины 
        /// ножки не меньше 330; в противном случае - false.</returns>
        public bool CheckDependentParametersValue()
        {
            return (_seatThickness + _legLength) >= 330;
        }

        /// <summary>
        /// Свойство для доступа к длине сиденья
        /// </summary>
        public int SeatLength
        {
            get { return _seatLength; }
            set { _seatLength = value; }
        }

        /// <summary>
        /// Свойство для доступа к ширине сиденья
        /// </summary>
        public int SeatWidth
        {
            get { return _seatWidth; }
            set { _seatWidth = value; }
        }

        /// <summary>
        /// Свойство для доступа к толщине сиденья
        /// </summary>
        public int SeatThickness
        {
            get { return _seatThickness; }
            set { _seatThickness = value; }
        }

        /// <summary>
        /// Свойство для доступа к длине ножки
        /// </summary>
        public int LegLength
        {
            get { return _legLength; }
            set { _legLength = value; }
        }

        /// <summary>
        /// Свойство для доступа к ширине ножки
        /// </summary>
        public int LegWidth
        {
            get { return _legWidth; }
            set { _legWidth = value; }
        }

        /// <summary>
        /// Свойство для доступа к высоте ножки
        /// </summary>
        public int LegHeight
        {
            get { return _legHeight; }
            set { _legHeight = value; }
        }

        //TODO: в автосвойства+
        /// <summary>
        /// Тип ножек табурета
        /// По умолчанию заданы квадратные ножки табурета.
        /// </summary>
        public LegTypes LegsType
        {
            get; set;
        }

        /// <summary>
        /// Тип сиденья табурета.
        /// По умолчанию задано прямоугольное/квадратное сиденье табурета.
        /// </summary>
        public SeatTypes SeatType
        {
            get; set;
        }

        /// <summary>
        /// Валидация значений свойств
        /// </summary>
        /// <param name="value">Значение свойства</param>
        /// <param name="valueName">Название свойства</param>
        /// <param name="minMax">Границы значений свойств</param>
        public bool IsWrongValue(int value, string valueName, out Tuple<int, int> minMax)
        {
            var current = _minMaxValues[valueName];
            minMax = current;

            return current.Item1 > value && value < current.Item2;
        }

        /// <summary>
        /// Изменение границ значения толщины сиденья
        /// </summary>
        /// <param name="value">Значение толщины сиденья</param>
        /// <param name="newMinMax">Новые границы значений свойств</param>
        public void AdjustMinValuesBasedOnThickness(int value, out Tuple<int, int> newMinMax)
        {
            var oldMax = _minMaxValues["Высота ножек"].Item2;
            if (330 - value < 300)
            {
                _minMaxValues["Высота ножек"] = new Tuple<int, int>(300, oldMax);
            }
            else
            {
                _minMaxValues["Высота ножек"] = new Tuple<int, int>(330 - value, oldMax);
            }
            newMinMax = _minMaxValues["Высота ножек"];
        }

        /// <summary>
        /// Изменение границ значения высоты ножек
        /// </summary>
        /// <param name="value">Значение высоты ножек</param>
        /// <param name="newMinMax">Новые границы значений свойств</param>
        public void AdjustMinValuesBasedOnLegLength(int value, out Tuple<int, int> newMinMax)
        {
            var oldMax = _minMaxValues["Толщина сиденья"].Item2;
            if (330 - value < 20)
            {
                _minMaxValues["Толщина сиденья"] = new Tuple<int, int>(20, oldMax);
            }
            else
            {
                _minMaxValues["Толщина сиденья"] = new Tuple<int, int>(330 - value, oldMax);
            }
            newMinMax = _minMaxValues["Толщина сиденья"];
        }
    }
}