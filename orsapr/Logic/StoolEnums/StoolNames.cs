using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    /// <summary>
    /// Перечисление параметров стула.
    /// </summary>
    public enum StoolParameters
    {
        /// <summary>
        /// Длина сиденья.
        /// </summary>
        SeatLength,

        /// <summary>
        /// Диаметер сиденья.
        /// </summary>
        SeatDiameter,

        /// <summary>
        /// Ширина сиденья.
        /// </summary>
        SeatWidth,

        /// <summary>
        /// Толщина сиденья.
        /// </summary>
        SeatThickness,

        /// <summary>
        /// Высота ножки.
        /// </summary>
        LegsHeight,

        /// <summary>
        /// Ширина и длина ножки.
        /// </summary>
        LegsWightAndLength,

        /// <summary>
        /// Диаметер ножки.
        /// </summary>
        LegsDiameter,

        /// <summary>
        /// Зависимые параметры.
        /// </summary>
        DependentParameters
    }
}
