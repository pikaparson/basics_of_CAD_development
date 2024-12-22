using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using API_singly;
using Logic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


using TextBox = System.Windows.Forms.TextBox;
 
namespace orsapr
{

    public partial class MainForm : Form
    {
        private Parameters _parameters;
        private StringBuilder _errorMessages;

        private int _minLegLength = 300;
        private int _minSeatThickness = 20;
        private LegTypes _legsType = LegTypes.SquareLeg;
        private SeatTypes _seatType = SeatTypes.SquareSeat;

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

        // Метод для проверки значения textBox1
        private void TextBox1_OnChanged(object sender, EventArgs e)
        {
            if (_seatType == SeatTypes.SquareSeat)
            {
                ValidateAndSetValue(textBox1, 300, 400, "Длина сиденья");
            }
            else
            {
                ValidateAndSetValue(textBox1, 300, 400, "Диаметр сиденья");
            }
        }

        // Метод для проверки значения textBox2
        private void TextBox2_OnChanged(object sender, EventArgs e)
        {
            ValidateAndSetValue(textBox2, 300, 600, "Ширина сиденья");
        }

        // Метод для проверки значения textBox3
        private void TextBox3_OnChanged(object sender, EventArgs e)
        {
            ValidateAndSetValue(textBox3, _minSeatThickness, 35, "Толщина сиденья");
        }

        // Метод для проверки значения textBox4
        private void TextBox4_OnChanged(object sender, EventArgs e)
        {
            ValidateAndSetValue(textBox4, _minLegLength, 400, "Высота ножек");
        }

        // Метод для проверки значения textBox5
        private void TextBox5_OnChanged(object sender, EventArgs e)
        {
            ValidateAndSetValue(textBox5, 25, 35, "Ширина и длина ножек");
        }

        private void ValidateAndSetValue(TextBox textBox, int minValue, int maxValue, string textBoxName)
        {
            ClearError(textBoxName);

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                // Добавляем ошибку только для текущего поля
                SetError(textBoxName, $"{textBoxName}: поле не должно быть пустым.");

                return;
            }

            if (!int.TryParse(textBox.Text, out int value))
            {
                // Добавляем ошибку о некорректном вводе
                SetError(textBoxName, $"{textBoxName}: введено некорректное значение.");
                return;
            }

            // Проверяем границы значений
            if (value < minValue || value > maxValue)
            {
                SetError(textBoxName, $"{textBoxName}: введены значения, не входящие в границы (от {minValue} до {maxValue}).");
                return;
            }

            // Если всё хорошо, устанавливаем значения в параметры
            ClearError(textBoxName); // Дублируем вызов для завершения удаления предыдущих ошибок

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
                ValidateDependentParameters();
        }

        private void AdjustMinValuesBasedOnThickness(int value)
        {
            if (330 - value < 300)
                _minLegLength = 300;
            else
                _minLegLength = 330 - value;

            label9.Text = $"от {_minLegLength} до 400 мм";
        }

        private void AdjustMinValuesBasedOnLegLength(int value)
        {
            if (330 - value < 20)
                _minSeatThickness = 20;
            else
                _minSeatThickness = 330 - value;

            label8.Text = $"от {_minSeatThickness} до 35 мм";
        }

        // Установить ошибку
        private void SetError(string textBoxName, string message)
        {
            // Проверка на наличие сообщения в строке ошибок
            if (!_errorMessages.ToString().Contains(message))
            {
                _errorMessages.AppendLine(message);
                UpdateErrorLabel();
            }
            SetColors(textBoxName);
        }

        // Очистить ошибку
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

            // Убираем ошибки, которые содержат textBoxName
            foreach (string line in lines)
            {
                if (!line.StartsWith(errorMessage))
                {
                    _errorMessages.AppendLine(line);
                }
            }
            UpdateErrorLabel();

            // Проверка на наличие ошибок
            if (_errorMessages.Length == 0)
            {
                label11.Text = string.Empty;
            }

            // Устанавливаем цвет текстового поля на стандартный
            ResetColor(textBoxName);
        }

        // Метод для проверки зависимых параметров
        private void ValidateDependentParameters()
        {
            // Сбрасываем цвет полей ввода для обновления
            ResetColor("Толщина сиденья");
            ResetColor("Высота ножек");

            if (_parameters.SeatThickness > 0 && _parameters.LegLength > 0) // Убедитесь, что значения установлены
            {
                if (!_parameters.CheckDependentParametersValue())
                {
                    SetError("Зависимые параметры", "Сумма толщины сиденья и длины ножки должна быть не менее 330.");
                    SetColors("Зависимые параметры");
                }
                else
                {
                    ClearError("Зависимые параметры");
                }
            }
        }

        // Обновить строку ошибок
        private void UpdateErrorLabel()
        {
            label11.Text = _errorMessages.ToString();
        }

        // Метод, который устанавливает цвет текстового поля
        private void SetColors(string textBoxName)
        {
            switch (textBoxName)
            {
                case "Длина сиденья":
                    textBox1.BackColor = Color.LightCoral;
                    break;
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
                    textBox5.BackColor = Color.LightCoral;
                    break;
                case "Диаметр ножек":
                    textBox5.BackColor = Color.LightCoral;
                    break;
                case "Зависимые параметры":
                    textBox3.BackColor = Color.LightCoral;
                    textBox4.BackColor = Color.LightCoral;
                    break;
            }
        }
        // Метод, который сбрасывает цвет текстового поля на стандартный
        private void ResetColor(string textBoxName)
        {
            switch (textBoxName)
            {
                case "Длина сиденья":
                    textBox1.BackColor = SystemColors.Window;
                    break;
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
                    textBox5.BackColor = SystemColors.Window;
                    break;
                case "Диаметр ножек":
                    textBox5.BackColor = SystemColors.Window;
                    break;
                case "Зависимые параметры":
                    textBox3.BackColor = SystemColors.Window;
                    textBox4.BackColor = SystemColors.Window;
                    break;
            }
        }

        // Метод для возможности ввода только цифр
        private void TextBox_OnlyDigitKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сначала очищаем все предыдущие сообщения об ошибках
            _errorMessages.Clear();

            // Проводим валидацию каждого текстового поля
            if (_seatType == SeatTypes.SquareSeat)
            {
                ValidateAndSetValue(textBox1, 300, 400, "Длина сиденья");
                ValidateAndSetValue(textBox2, 300, 600, "Ширина сиденья");
            }
            else if (_seatType == SeatTypes.RoundSeat)
            {
                ValidateAndSetValue(textBox1, 300, 400, "Диаметр сиденья");
            }
            ValidateAndSetValue(textBox3, 20, 35, "Толщина сиденья");
            if (_legsType == LegTypes.SquareLeg)
            {
                ValidateAndSetValue(textBox5, 25, 35, "Ширина и длина ножек");
            }
            else if (_legsType == LegTypes.RoundLeg)
            {
                ValidateAndSetValue(textBox5, 25, 35, "Диаметр ножек");
            }
            ValidateAndSetValue(textBox4, 300, 400, "Высота ножек");

            if (_errorMessages.Length == 0)
            {
                // Если ошибок нет, продолжаем основную логику
                Builder builder = new Builder();
                builder.BuildChair(_parameters, _seatType, _legsType);
            }
            else
            {
                // Если есть ошибки, обновляем метку ошибок
                UpdateErrorLabel();
            }
        }

        private void SeatTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0: // Первый элемент (Длина)
                    {
                        _seatType = SeatTypes.SquareSeat;
                        ClearError("Длина сиденья");
                        ClearError("Ширина сиденья");
                        ClearError("Толщина сиденья");
                        ClearError("Диаметр сиденья");
                        textBox1.BackColor = SystemColors.Window;
                        textBox2.BackColor = SystemColors.Window;
                        textBox3.BackColor = SystemColors.Window;
                        if (textBox1.Text != "")
                        {
                            ValidateAndSetValue(textBox1, 300, 400, "Длина сиденья");
                        }
                        if (textBox3.Text != "")
                        {
                            ValidateAndSetValue(textBox3, 20, 35, "Толщина сиденья");
                        }

                        // Параметры для прямоугольного сиденья
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
                    }
                case 1: // Второй элемент (Диаметр)
                    {
                        _seatType = SeatTypes.RoundSeat;
                        ClearError("Длина сиденья");
                        ClearError("Ширина сиденья");
                        ClearError("Толщина сиденья");
                        ClearError("Диаметр сиденья");
                        textBox1.BackColor = SystemColors.Window;
                        textBox2.BackColor = SystemColors.Window;
                        textBox3.BackColor = SystemColors.Window;
                        if (textBox1.Text != "")
                        {
                            ValidateAndSetValue(textBox1, 300, 400, "Диаметр сиденья");
                        }
                        if (textBox3.Text != "")
                        {
                            ValidateAndSetValue(textBox3, 20, 35, "Толщина сиденья");
                        }

                        if (_legsType == LegTypes.SquareLeg && textBox5.BackColor == Color.LightCoral)
                        {
                            ValidateAndSetValue(textBox5, 25, 35, "Ширина и длина ножек");
                        }
                        else if (_seatType == SeatTypes.RoundSeat && textBox5.BackColor == Color.LightCoral)
                        {
                            ValidateAndSetValue(textBox1, 300, 400, "Диаметр сиденья");
                        }
                        if (textBox4.BackColor == Color.LightCoral)
                        {
                            ValidateAndSetValue(textBox4, 300, 400, "Высота ножек");
                        }

                        _parameters.SeatWidth = 0; // Обязательно сбрасываем значение ширины
                        textBox2.Visible = false;
                        textBox2.Enabled = false;
                        textBox2.Text = ""; // Очищаем текст, если поле отключено
                        ClearError("Ширина сиденья");

                        // Параметры для круглого сиденья
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
        }
        private void LegsTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    {
                        _legsType = LegTypes.SquareLeg;
                        _errorMessages.Clear();
                        textBox4.BackColor = SystemColors.Window;
                        textBox5.BackColor = SystemColors.Window;
                        label2.Text = "Длина и ширина";

                        if (textBox4.Text != "")
                        {
                            ValidateAndSetValue(textBox5, 300, 400, "Высота ножек");
                        }
                        if (textBox5.Text != "")
                        {
                            ValidateAndSetValue(textBox5, 25, 35, "Ширина и длина ножек");
                        }

                        if (_seatType == SeatTypes.SquareSeat)
                        {
                            if (textBox1.BackColor == Color.LightCoral)
                            {
                                ValidateAndSetValue(textBox1, 300, 400, "Длина сиденья");
                            }
                            if (textBox2.BackColor == Color.LightCoral)
                            {
                                ValidateAndSetValue(textBox2, 300, 600, "Ширина сиденья");
                            }
                            if (textBox3.BackColor == Color.LightCoral)
                            {
                                ValidateAndSetValue(textBox3, 20, 35, "Толщина сиденья");
                            }
                        }
                        else if (_seatType == SeatTypes.RoundSeat)
                        {
                            ValidateAndSetValue(textBox1, 300, 400, "Диаметр сиденья");
                            ValidateAndSetValue(textBox3, 20, 35, "Толщина сиденья");
                        }
                        break;
                    }
                case 1:
                    {
                        _legsType = LegTypes.RoundLeg;
                        _errorMessages.Clear();
                        textBox4.BackColor = SystemColors.Window;
                        textBox5.BackColor = SystemColors.Window;
                        label2.Text = "Диаметр";
                        if (textBox4.Text != "")
                        {
                            ValidateAndSetValue(textBox5, 300, 400, "Высота ножек");
                        }
                        if (textBox5.Text != "")
                        {
                            ValidateAndSetValue(textBox5, 25, 35, "Диаметр ножек");
                        }

                        if (_seatType == SeatTypes.SquareSeat)
                        {
                            if (textBox1.BackColor == Color.LightCoral)
                            {
                                ValidateAndSetValue(textBox1, 300, 400, "Длина сиденья");
                            }
                            if (textBox2.BackColor == Color.LightCoral)
                            {
                                ValidateAndSetValue(textBox2, 300, 600, "Ширина сиденья");
                            }
                            if (textBox3.BackColor == Color.LightCoral)
                            {
                                ValidateAndSetValue(textBox3, 20, 35, "Толщина сиденья");
                            }
                        }
                        else if (_seatType == SeatTypes.RoundSeat)
                        {
                            ValidateAndSetValue(textBox1, 300, 400, "Диаметр сиденья");
                            ValidateAndSetValue(textBox3, 20, 35, "Толщина сиденья");
                        }
                        break;
                    }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
