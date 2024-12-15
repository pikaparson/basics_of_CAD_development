namespace Logic
{
    public class Parameters
    {
        // Закрытые поля для хранения значений
        private int _seatLength;
        private int _seatWidth;
        private int _seatThickness;
        private int _legLength;
        private int _legWidth;
        private int _legHeight;

        // Конструктор по умолчанию
        public Parameters()
        {
            // Инициализация полей значениями по умолчанию
            _seatLength = 0;
            _seatWidth = 0;
            _seatThickness = 0;
            _legLength = 0;
            _legWidth = 0;
            _legHeight = 0;
        }

        // Метод для проверки зависимости параметров
        public bool CheckDependentParametersValue()
        {
            // Проверяем, что сумма толщины сиденья и длины ножки не меньше 330
            return (_seatThickness + _legLength) >= 330;
        }

        // Свойства для доступа к переменным
        public int SeatLength
        {
            get { return _seatLength; }
            set { _seatLength = value; }
        }

        public int SeatWidth
        {
            get { return _seatWidth; }
            set { _seatWidth = value; }
        }

        public int SeatThickness
        {
            get { return _seatThickness; }
            set { _seatThickness = value; }
        }

        public int LegLength
        {
            get { return _legLength; }
            set { _legLength = value; }
        }

        public int LegWidth
        {
            get { return _legWidth; }
            set { _legWidth = value; }
        }

        public int LegHeight
        {
            get { return _legHeight; }
            set { _legHeight = value; }
        }
    }


}
