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
        /// Тип ножек табурета
        /// По умолчанию заданы квадратные ножки табурета.
        /// </summary>
        private LegTypes _legsType = LegTypes.SquareLeg;

        /// <summary>
        /// Тип сиденья табурета
        /// По умолчанию задано прямоугольное/квадратное сиденье табурета.
        /// </summary>
        private SeatTypes _seatType = SeatTypes.SquareSeat;

        /// <summary>
        /// Конструктор по умолчанию, инициализирует параметры значениями по умолчанию
        /// </summary>
        public Parameters()
        {
            _seatLength = 0;
            _seatWidth = 0;
            _seatThickness = 0;
            _legLength = 0;
            _legWidth = 0;
            _legHeight = 0;
        }

        /// <summary>
        /// Метод для проверки зависимости параметров
        /// </summary>
        /// <returns>Возвращает true, если сумма толщины сиденья и длины ножки не меньше 330; в противном случае - false.</returns>
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

        public LegTypes LegsType
        {
            get { return _legsType; }
            set { _legsType = value; }
        }

        public SeatTypes SeatType
        {
            get { return _seatType; }
            set { _seatType = value; }
        }
    }
}