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
using orsapr.Wrapper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace orsapr
{
    public partial class MainForm : Form
    {
        private Parameters _parameters;
        private StringBuilder _errorMessages;

        public MainForm()
        {
            InitializeComponent();
            _parameters = new Parameters();
            _errorMessages = new StringBuilder();
        }

        // Метод для проверки значения textBox1
        private void TextBox1_OnChanged(object sender, EventArgs e)
        {
            ValidateAndSetValue(textBox1, 300, 400, "Длина сиденья");
        }

        // Метод для проверки значения textBox2
        private void TextBox2_OnChanged(object sender, EventArgs e)
        {
            ValidateAndSetValue(textBox2, 300, 600, "Ширина сиденья");
        }

        // Метод для проверки значения textBox3
        private void TextBox3_OnChanged(object sender, EventArgs e)
        {
            ValidateAndSetValue(textBox3, 20, 35, "Толщина сиденья");
        }

        // Метод для проверки значения textBox4
        private void TextBox4_OnChanged(object sender, EventArgs e)
        {
            ValidateAndSetValue(textBox4, 300, 400, "Высота ножек");
        }

        // Метод для проверки значения textBox5
        private void TextBox5_OnChanged(object sender, EventArgs e)
        {
            ValidateAndSetValue(textBox5, 25, 35, "Ширина и длина ножек");
        }

        // Метод для валидации значений
        private void ValidateAndSetValue(TextBox textBox, int minValue, int maxValue, string textBoxName)
        {
            ClearError(textBoxName);

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                // Добавляем ошибку о пустом поле
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
                case "Ширина сиденья":
                    _parameters.SeatWidth = value;
                    break;
                case "Толщина сиденья":
                    _parameters.SeatThickness = value;
                    break;
                case "Высота ножек":
                    _parameters.LegLength = value;
                    break;
                case "Ширина и длина ножек":
                    _parameters.LegWidth = value;
                    break;
            }

            if (textBoxName == "Толщина сиденья" || textBoxName == "Высота ножек") ValidateDependentParameters();
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
            ValidateAndSetValue(textBox1, 300, 400, "Длина сиденья");
            ValidateAndSetValue(textBox2, 300, 600, "Ширина сиденья");
            ValidateAndSetValue(textBox3, 20, 35, "Толщина сиденья");
            ValidateAndSetValue(textBox4, 300, 400, "Высота ножек");
            ValidateAndSetValue(textBox5, 25, 35, "Ширина и длина ножек");

            if (_errorMessages.Length == 0)
            {
                // Если ошибок нет, продолжаем основную логику
                Wrapper.Wrapper wrapper = new Wrapper.Wrapper();
                wrapper.Build(_parameters);
            }
            else
            {
                // Если есть ошибки, обновляем метку ошибок
                UpdateErrorLabel();
            }
        }
    }
}
