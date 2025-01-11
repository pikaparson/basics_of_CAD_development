using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using API_singly;
using Logic;

using TextBox = System.Windows.Forms.TextBox;

namespace orsapr
{
    /// <summary>
    /// Класс для работы с главным окном плагина
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Параметры табурета
        /// </summary>
        private Parameters _parameters;

        /// <summary>
        /// Класс для построения табурета
        /// </summary>
        private StringBuilder _errorMessages;

        /// <summary>
        /// Инициализация компонентов главного окна
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            _parameters = new Parameters();
            _errorMessages = new StringBuilder();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedIndex = 0;
            label11.Text = string.Empty;
            textBox1.BackColor = SystemColors.Window;
        }

        /// <summary>
        /// Обработчик события изменения текста в поле для ввода длины и диаметра сиденья
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TextBox1_OnChanged(object sender, EventArgs e)
        {
            if (_parameters.SeatType == SeatTypes.SquareSeat)
            {
                ValidateAndSetValue(textBox1, "Длина сиденья");
            }
            else
            {
                ValidateAndSetValue(textBox1, "Диаметр сиденья");
            }
        }

        /// <summary>
        /// Обработчик события изменения текста в поле для ввода ширины сиденья
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TextBox2_OnChanged(object sender, EventArgs e)
        {
            ValidateAndSetValue(textBox2, "Ширина сиденья");
        }

        /// <summary>
        /// Обработчик события изменения текста в поле для ввода толщины сиденья
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TextBox3_OnChanged(object sender, EventArgs e)
        {
            ValidateAndSetValue(textBox3, "Толщина сиденья");
        }

        /// <summary>
        /// Обработчик события изменения текста в поле для ввода высоты ножек
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TextBox4_OnChanged(object sender, EventArgs e)
        {
            ValidateAndSetValue(textBox4, "Высота ножек");
        }

        /// <summary>
        /// Обработчик события изменения текста в поле для ввода ширины и длины ножек и их диаметра
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TextBox5_OnChanged(object sender, EventArgs e)
        {
            if (_parameters.SeatType == SeatTypes.SquareSeat)
            {
                ValidateAndSetValue(textBox5, "Ширина и длина ножек");
            }
            else
            {
                ValidateAndSetValue(textBox5, "Диаметр ножек");
            }
        }

        /// <summary>
        /// Метод для валидации и установки значения текстового поля
        /// </summary>
        /// <param name="textBox">Текстовое поле для валидации</param>
        /// <param name="minValue">Минимальное значение</param>
        /// <param name="maxValue">Максимальное значение</param>
        /// <param name="textBoxName">Имя текстового поля</param>
        private void ValidateAndSetValue(TextBox textBox, string textBoxName)
        {
            ClearError(textBoxName);

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                SetError(textBoxName, $"{textBoxName}: поле не должно быть пустым.");
                return;
            }

            if (!int.TryParse(textBox.Text, out int value))
            {
                SetError(textBoxName, $"{textBoxName}: введено некорректное значение.");
                return;
            }

            ClearError(textBoxName);

            switch (textBoxName)
            {
                case "Длина сиденья":
                    _parameters.SeatLength = value;
                    break;
                case "Диаметр сиденья":
                    _parameters.SeatLength = value;
                    _parameters.SeatWidth = value;
                    break;
                case "Ширина сиденья":
                    _parameters.SeatWidth = value;
                    break;
                case "Толщина сиденья":
                    _parameters.SeatThickness = value;
                    AdjustMinValuesBasedOnThickness(value);
                    break;
                case "Высота ножек":
                    _parameters.LegLength = value;
                    AdjustMinValuesBasedOnLegLength(value);
                    break;
                case "Ширина и длина ножек":
                    _parameters.LegWidth = value;
                    break;
                case "Диаметр ножек":
                    _parameters.LegWidth = value;
                    break;
            }

            if (textBoxName == "Толщина сиденья" || textBoxName == "Высота ножек")
            {
                ValidateDependentParameters();
                return;
            }

            if (_parameters.IsWrongValue(value, textBoxName, out Tuple<int, int> minMax))
            {
                SetError(textBoxName, $"{textBoxName}: введены значения, " +
                    $"не входящие в границы (от {minMax.Item1} до {minMax.Item2}).");
            }
        }

        /// <summary>
        /// Метод для корректировки минимального значения высоты ножек на основе толщины сиденья
        /// </summary>
        /// <param name="value">Толщина сиденья.</param>
        private void AdjustMinValuesBasedOnThickness(int value)
        {
            _parameters.AdjustMinValuesBasedOnThickness(value, out Tuple<int, int> newMinMax);

            label9.Text = $"от {newMinMax.Item1} до {newMinMax.Item2} мм";
        }

        /// <summary>
        /// Метод для корректировки минимального значения толщины сиденья на основе высоты ножек
        /// </summary>
        /// <param name="value">Высота ножек.</param>
        private void AdjustMinValuesBasedOnLegLength(int value)
        {
            _parameters.AdjustMinValuesBasedOnLegLength(value, out Tuple<int, int> newMinMax);
            label8.Text = $"от {newMinMax.Item1} до {newMinMax.Item2} мм";
        }

        /// <summary>
        /// Метод для указания ошибки в текстовом поле
        /// </summary>
        /// <param name="textBoxName">Имя текстового поля</param>
        /// <param name="message">Сообщение об ошибке</param>
        private void SetError(string textBoxName, string message)
        {
            if (!_errorMessages.ToString().Contains(message))
            {
                _errorMessages.AppendLine(message);
                UpdateErrorLabel();
            }
            SetColors(textBoxName);
        }

        /// <summary>
        /// Метод для очистки ошибок в текстовом поле
        /// </summary>
        /// <param name="textBoxName">Имя текстового поля.</param>
        private void ClearError(string textBoxName)
        {
            string errorMessage;
            if (textBoxName == "Зависимые параметры")
            {
                errorMessage = $"Сумма";
            }
            else
            {
                errorMessage = $"{textBoxName}: ";
            }

            string[] lines = _errorMessages.ToString().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            _errorMessages.Clear();

            foreach (string line in lines)
            {
                if (!line.StartsWith(errorMessage))
                {
                    _errorMessages.AppendLine(line);
                }
            }
            UpdateErrorLabel();

            if (_errorMessages.Length == 0)
            {
                label11.Text = string.Empty;
            }

            ResetColor(textBoxName);
        }

        /// <summary>
        /// Метод для проверки зависимых параметров
        /// </summary>
        private void ValidateDependentParameters()
        {
            ResetColor("Толщина сиденья");
            ResetColor("Высота ножек");

            if (_parameters.SeatThickness > 0 && _parameters.LegLength > 0)
            {
                if (!_parameters.CheckDependentParametersValue())
                {
                    SetError("Зависимые параметры", $"Сумма толщины сиденья и длины ножки должна быть не менее {Parameters.dependentParametersSumm}.");
                    SetColors("Зависимые параметры");
                }
                else
                {
                    ClearError("Зависимые параметры");
                }
            }
        }

        /// <summary>
        /// Метод для обновления ошибок в поле с предупреждениями
        /// </summary>
        private void UpdateErrorLabel()
        {
            label11.Text = _errorMessages.ToString();
        }

        /// <summary>
        /// Метод для установки цвета текстового поля с некорректным значением
        /// </summary>
        /// <param name="textBoxName">Имя текстового поля</param>
        private void SetColors(string textBoxName)
        {
             //TODO: duplication
            switch (textBoxName)
            {
                case "Длина сиденья":
                case "Диаметр сиденья":
                    textBox1.BackColor = Color.LightCoral;
                    break;
                case "Ширина сиденья":
                    textBox2.BackColor = Color.LightCoral;
                    break;
                case "Толщина сиденья":
                    textBox3.BackColor = Color.LightCoral;
                    break;
                case "Высота ножек":
                    textBox4.BackColor = Color.LightCoral;
                    break;
                case "Ширина и длина ножек":
                case "Диаметр ножек":
                    textBox5.BackColor = Color.LightCoral;
                    break;
                case "Зависимые параметры":
                    textBox3.BackColor = Color.LightCoral;
                    textBox4.BackColor = Color.LightCoral;
                    break;
            }
        }

        /// <summary>
        /// Метод для сброса цвета текстового поля на стандартный
        /// </summary>
        /// <param name="textBoxName">Имя текстового поля</param>
        private void ResetColor(string textBoxName)
        {
             //TODO: duplication
            switch (textBoxName)
            {
                case "Длина сиденья":
                case "Диаметр сиденья":
                    textBox1.BackColor = SystemColors.Window;
                    break;
                case "Ширина сиденья":
                    textBox2.BackColor = SystemColors.Window;
                    break;
                case "Толщина сиденья":
                    textBox3.BackColor = SystemColors.Window;
                    break;
                case "Высота ножек":
                    textBox4.BackColor = SystemColors.Window;
                    break;
                case "Ширина и длина ножек":
                case "Диаметр ножек":
                    textBox5.BackColor = SystemColors.Window;
                    break;
                case "Зависимые параметры":
                    textBox3.BackColor = SystemColors.Window;
                    textBox4.BackColor = SystemColors.Window;
                    break;
            }
        }

        /// <summary>
        /// Метод для обработки ввода только цифр
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void TextBox_OnlyDigitKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Build_Button_Click(object sender, EventArgs e)
        {
            _errorMessages.Clear();
            switch(_parameters.SeatType)
            {
                case SeatTypes.SquareSeat:
                {
                    ValidateAndSetValue(textBox1, "Длина сиденья");
                    ValidateAndSetValue(textBox2, "Ширина сиденья");
                    break;
                }
                case SeatTypes.RoundSeat:
                {
                    ValidateAndSetValue(textBox1, "Диаметр сиденья");
                    break;
                }
                default:
                {
                    break;
                }
            }

            ValidateAndSetValue(textBox3, "Толщина сиденья");

            switch(_parameters.LegsType)
            {
                case LegTypes.SquareLeg:
                {
                    ValidateAndSetValue(textBox5, "Ширина и длина ножек");
                    break;
                }
                case LegTypes.RoundLeg:
                {
                    ValidateAndSetValue(textBox5, "Диаметр ножек");
                    break;
                }
                default:
                {
                    break;
                }
            }

            ValidateAndSetValue(textBox4, "Высота ножек");

            if (_errorMessages.Length == 0)
            {
                Builder builder = new Builder();
                builder.Build(_parameters);
            }
            else
            {
                UpdateErrorLabel();
            }
        }

        /// <summary>
        /// Обработчик события изменения выбора типа сиденья
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void SeatTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    _parameters.SeatType = SeatTypes.SquareSeat;
                    ClearError("Длина сиденья");
                    ClearError("Ширина сиденья");
                    ClearError("Толщина сиденья");
                    ClearError("Диаметр сиденья");
                    textBox1.BackColor = SystemColors.Window;
                    textBox2.BackColor = SystemColors.Window;
                    textBox3.BackColor = SystemColors.Window;
                    if (textBox1.Text != "")
                    {
                        ValidateAndSetValue(textBox1, "Длина сиденья");
                    }
                    if (textBox3.Text != "")
                    {
                        ValidateAndSetValue(textBox3, "Толщина сиденья");
                    }

                    groupBox2.Size = new Size(417, 110);
                    label1.Text = "Длина";
                    label5.Visible = true;
                    label7.Visible = true;
                    textBox2.Visible = true;
                    textBox2.Enabled = true;

                    label3.Location = new Point(58, 85);
                    textBox3.Location = new Point(169, 82);
                    label8.Location = new Point(258, 85);
                    groupBox3.Location = new Point(7, 129);
                    groupBox1.Location = new Point(7, 228);
                    button1.Location = new Point(166, 318);
                    this.MaximumSize = new Size(452, 390);
                    this.MinimumSize = new Size(452, 390);
                    break;

                case 1:
                    _parameters.SeatType = SeatTypes.RoundSeat;
                    ClearError("Длина сиденья");
                    ClearError("Ширина сиденья");
                    ClearError("Толщина сиденья");
                    ClearError("Диаметр сиденья");
                    textBox1.BackColor = SystemColors.Window;
                    textBox2.BackColor = SystemColors.Window;
                    textBox3.BackColor = SystemColors.Window;
                    if (textBox1.Text != "")
                    {
                        ValidateAndSetValue(textBox1, "Диаметр сиденья");
                    }
                    if (textBox3.Text != "")
                    {
                        ValidateAndSetValue(textBox3, "Толщина сиденья");
                    }

                    if (_parameters.LegsType == LegTypes.SquareLeg && textBox5.BackColor == Color.LightCoral)
                    {
                        ValidateAndSetValue(textBox5, "Ширина и длина ножек");
                    }
                    else if (_parameters.SeatType == SeatTypes.RoundSeat && textBox5.BackColor == Color.LightCoral)
                    {
                        ValidateAndSetValue(textBox1, "Диаметр сиденья");
                    }
                    if (textBox4.BackColor == Color.LightCoral)
                    {
                        ValidateAndSetValue(textBox4, "Высота ножек");
                    }

                    _parameters.SeatWidth = 0;
                    textBox2.Visible = false;
                    textBox2.Enabled = false;
                    textBox2.Text = "";
                    ClearError("Ширина сиденья");

                    label1.Text = "Диаметр";
                    label5.Visible = false;
                    label7.Visible = false;

                    label3.Location = new Point(58, 64);
                    textBox3.Location = new Point(169, 61);
                    label8.Location = new Point(258, 64);
                    groupBox3.Location = new Point(7, 105);
                    groupBox1.Location = new Point(7, 204);
                    button1.Location = new Point(166, 293);
                    this.MaximumSize = new Size(452, 365);
                    this.MinimumSize = new Size(452, 365);
                    break;
            }
        }

        /// <summary>
        /// Обработчик события изменения выбора типа ножек
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void LegsTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    _parameters.LegsType = LegTypes.SquareLeg;
                    _errorMessages.Clear();
                    textBox4.BackColor = SystemColors.Window;
                    textBox5.BackColor = SystemColors.Window;
                    label2.Text = "Длина и ширина";

                    if (textBox4.Text != "")
                    {
                        ValidateAndSetValue(textBox4, "Высота ножек");
                    }
                    if (textBox5.Text != "")
                    {
                        ValidateAndSetValue(textBox5, "Ширина и длина ножек");
                    }
                    switch(_parameters.SeatType)
                    {
                        case SeatTypes.SquareSeat:
                        {
                            if (textBox1.BackColor == Color.LightCoral)
                            {
                                ValidateAndSetValue(textBox1, "Длина сиденья");
                            }
                            if (textBox2.BackColor == Color.LightCoral)
                            {
                                ValidateAndSetValue(textBox2, "Ширина сиденья");
                            }
                            if (textBox3.BackColor == Color.LightCoral)
                            {
                                ValidateAndSetValue(textBox3, "Толщина сиденья");
                            }
                            break;
                        }
                        case SeatTypes.RoundSeat:
                        {
                            ValidateAndSetValue(textBox1, "Диаметр сиденья");
                            ValidateAndSetValue(textBox3, "Толщина сиденья");
                            break;
                        }
                    }
                    break;

                case 1:
                    _parameters.LegsType = LegTypes.RoundLeg;
                    _errorMessages.Clear();
                    textBox4.BackColor = SystemColors.Window;
                    textBox5.BackColor = SystemColors.Window;
                    label2.Text = "Диаметр";
                    if (textBox4.Text != "")
                    {
                        ValidateAndSetValue(textBox4, "Высота ножек");
                    }
                    if (textBox5.Text != "")
                    {
                        ValidateAndSetValue(textBox5, "Диаметр ножек");
                    }

                    switch(_parameters.SeatType)
                    {
                        case SeatTypes.SquareSeat:
                        {
                            if (textBox1.BackColor == Color.LightCoral)
                            {
                                ValidateAndSetValue(textBox1, "Длина сиденья");
                            }
                            if (textBox2.BackColor == Color.LightCoral)
                            {
                                ValidateAndSetValue(textBox2, "Ширина сиденья");
                            }
                            if (textBox3.BackColor == Color.LightCoral)
                            {
                                ValidateAndSetValue(textBox3, "Толщина сиденья");
                            }
                            break;
                        }
                        case SeatTypes.RoundSeat:
                        {
                            ValidateAndSetValue(textBox1, "Диаметр сиденья");
                            ValidateAndSetValue(textBox3, "Толщина сиденья");
                            break;
                        }
                    }
                    break;
            }
        }
    }
}