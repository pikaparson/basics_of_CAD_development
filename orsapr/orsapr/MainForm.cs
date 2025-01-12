using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using API;
using BusinessLogic;

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
        /// Словарь названий параметров.
        /// </summary>
        private Dictionary<ChairParameters, string> _parameterNames = new Dictionary<ChairParameters, string>
        {
            {ChairParameters.SeatLength, "Длина сиденья"},
            {ChairParameters.SeatDiameter, "Диаметр сиденья"},
            {ChairParameters.SeatWidth, "Ширина сиденья"},
            {ChairParameters.SeatThickness, "Толщина сиденья"},
            {ChairParameters.LegsHeight, "Высота ножек"},
            {ChairParameters.LegsDiameter, "Диаметр ножек"},
            {ChairParameters.LegsWightAndLength, "Ширина и длина ножек"},
            {ChairParameters.DependentParameters, "Зависимые параметры"},
        };

        /// <summary>
        /// Инициализация компонентов главного окна
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            _parameters = new Parameters();
            _errorMessages = new StringBuilder();
            SeatFormsСomboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SeatFormsСomboBox.SelectedIndex = 0;
            LegsParametersComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            LegsParametersComboBox.SelectedIndex = 0;
            label11.Text = string.Empty;
            SeatLengthTextBox.BackColor = SystemColors.Window;
        }

        /// <summary>
        /// Обработчик события изменения значения TextBox.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TextBoxChanged(object sender, EventArgs e)
        {
            Dictionary<TextBox, ChairParameters> handledTextBoxChairParameters;
            handledTextBoxChairParameters = new Dictionary<TextBox, ChairParameters>
                {
                    {SeatLengthTextBox, ChairParameters.SeatDiameter},
                    {SeatWidthTextBox, ChairParameters.SeatWidth},
                    {SeatThicknessTextBox, ChairParameters.SeatThickness},
                    {LegsHeightTextBox, ChairParameters.LegsHeight},
                    {LegsLengthAndWidthTextBox, ChairParameters.LegsDiameter},
                };

            if (_parameters.SeatType == SeatTypes.SquareSeat)
            {
                handledTextBoxChairParameters = new Dictionary<TextBox, ChairParameters>
                {
                    {SeatLengthTextBox, ChairParameters.SeatLength},
                    {SeatWidthTextBox, ChairParameters.SeatWidth},
                    {SeatThicknessTextBox, ChairParameters.SeatThickness},
                    {LegsHeightTextBox, ChairParameters.LegsHeight},
                    {LegsLengthAndWidthTextBox, ChairParameters.LegsWightAndLength},
                };
            }

            var changedTextBox = (TextBox)sender;
            var chairParameters = handledTextBoxChairParameters[changedTextBox];
            ValidateAndSetValue(changedTextBox, chairParameters);
        }

        /// <summary>
        /// Метод для валидации и установки значения текстового поля.
        /// </summary>
        /// <param name="textBox">Текстовое поле для валидации.</param>
        /// <param name="chairParameters">Параметр стула.</param>
        private void ValidateAndSetValue(TextBox textBox, ChairParameters chairParameters)
        {
            var textBoxName = _parameterNames[chairParameters];
            ClearError(textBoxName, textBox);

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                SetError(textBox, $"{textBoxName}: поле не должно быть пустым.");
                return;
            }

            if (!int.TryParse(textBox.Text, out int value))
            {
                SetError(textBox, $"{textBoxName}: введено некорректное значение.");
                return;
            }

            ClearError(textBoxName, textBox);

            switch (chairParameters)
            {
                case ChairParameters.SeatLength:
                    _parameters.SeatLength = value;
                    break;
                case ChairParameters.SeatDiameter:
                    _parameters.SeatLength = value;
                    _parameters.SeatWidth = value;
                    break;
                case ChairParameters.SeatWidth:
                    _parameters.SeatWidth = value;
                    break;
                case ChairParameters.SeatThickness:
                    _parameters.SeatThickness = value;
                    AdjustMinValues(value, chairParameters, label9);
                    break;
                case ChairParameters.LegsHeight:
                    _parameters.LegLength = value;
                    AdjustMinValues(value, chairParameters, label8);
                    break;
                case ChairParameters.LegsWightAndLength:
                    _parameters.LegWidth = value;
                    break;
                case ChairParameters.LegsDiameter:
                    _parameters.LegWidth = value;
                    break;
            }

            if (chairParameters == ChairParameters.SeatThickness
                || chairParameters == ChairParameters.LegsHeight)
            {
                ValidateDependentParameters();
                return;
            }

            if (_parameters.IsWrongValue(value, chairParameters, out Tuple<int, int> minMax))
            {
                SetError(textBox, $"{textBoxName}: введены значения, " +
                    $"не входящие в границы (от {minMax.Item1} до {minMax.Item2}).");
            }
        }

        /// <summary>
        /// Метод для корректировки минимального значения.
        /// </summary>
        /// <param name="value">Значение параметра.</param>
        /// <param name="parameter">Параметр.</param>
        /// <param name="label">Лейбл диапазона значений.</param>
        private void AdjustMinValues(int value, ChairParameters parameter, System.Windows.Forms.Label label)
        {
            var newMinMax = _parameters.AdjustMinValues(value, parameter);
            label.Text = $"от {newMinMax.Item1} до {newMinMax.Item2} мм";
        }

        /// <summary>
        /// Метод для указания ошибки в текстовом поле
        /// </summary>
        /// <param name="textBoxName">Имя текстового поля</param>
        /// <param name="message">Сообщение об ошибке</param>
        private void SetError(TextBox textBox, string message)
        {
            if (!_errorMessages.ToString().Contains(message))
            {
                _errorMessages.AppendLine(message);
                UpdateErrorLabel();
            }
            SetColors(textBox);
        }

        /// <summary>
        /// Метод для очистки ошибок в текстовом поле
        /// </summary>
        /// <param name="textBoxName">Имя текстового поля.</param>
        private void ClearError(string textBoxName, TextBox textBox)
        {
            string errorMessage;
            if (textBoxName == _parameterNames[ChairParameters.DependentParameters])
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

            ResetColor(textBox);
        }

        /// <summary>
        /// Метод для проверки зависимых параметров
        /// </summary>
        private void ValidateDependentParameters()
        {
            ResetColor(SeatThicknessTextBox);
            ResetColor(LegsHeightTextBox);

            if (_parameters.SeatThickness > 0 && _parameters.LegLength > 0)
            {
                if (!_parameters.CheckDependentParametersValue())
                {
                    SetError(LegsHeightTextBox, "Сумма толщины сиденья и длины ножки должна быть в диапазоне от" +
                        $" {Parameters.dependentParametersMinSum} до \n{Parameters.dependentParametersMaxSum}.");
                    SetColors(SeatThicknessTextBox);
                }
                else
                {
                    ClearError(_parameterNames[ChairParameters.DependentParameters], SeatThicknessTextBox);
                    ClearError(_parameterNames[ChairParameters.DependentParameters], LegsHeightTextBox);
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
        private void SetColors(TextBox textBox)
        {
            textBox.BackColor = Color.LightCoral;
            //TODO: duplication +
        }

        /// <summary>
        /// Метод для сброса цвета текстового поля на стандартный
        /// </summary>
        /// <param name="textBoxName">Имя текстового поля</param>
        private void ResetColor(TextBox textBox)
        {
            textBox.BackColor = SystemColors.Window;
            //TODO: duplication +
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
            switch (_parameters.SeatType)
            {
                case SeatTypes.SquareSeat:
                    {
                        ValidateAndSetValue(SeatLengthTextBox, ChairParameters.SeatLength);
                        ValidateAndSetValue(SeatWidthTextBox, ChairParameters.SeatWidth);
                        break;
                    }
                case SeatTypes.RoundSeat:
                    {
                        ValidateAndSetValue(SeatLengthTextBox, ChairParameters.SeatDiameter);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            ValidateAndSetValue(SeatThicknessTextBox, ChairParameters.SeatThickness);

            switch (_parameters.LegsType)
            {
                case LegTypes.SquareLeg:
                    {
                        ValidateAndSetValue(LegsLengthAndWidthTextBox, ChairParameters.LegsWightAndLength);
                        break;
                    }
                case LegTypes.RoundLeg:
                    {
                        ValidateAndSetValue(LegsLengthAndWidthTextBox, ChairParameters.LegsDiameter);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            ValidateAndSetValue(LegsHeightTextBox, ChairParameters.LegsHeight);

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
            switch (SeatFormsСomboBox.SelectedIndex)
            {
                case 0:
                    _parameters.SeatType = SeatTypes.SquareSeat;
                    ClearError(_parameterNames[ChairParameters.SeatLength], SeatLengthTextBox);
                    ClearError(_parameterNames[ChairParameters.SeatWidth], SeatWidthTextBox);
                    ClearError(_parameterNames[ChairParameters.SeatThickness], SeatThicknessTextBox);
                    SeatLengthTextBox.BackColor = SystemColors.Window;
                    SeatWidthTextBox.BackColor = SystemColors.Window;
                    SeatThicknessTextBox.BackColor = SystemColors.Window;
                    if (SeatLengthTextBox.Text != "")
                    {
                        ValidateAndSetValue(SeatLengthTextBox, ChairParameters.SeatLength);
                    }
                    if (SeatThicknessTextBox.Text != "")
                    {
                        ValidateAndSetValue(SeatThicknessTextBox, ChairParameters.SeatThickness);
                    }

                    SeatParametersGroupBox.Size = new Size(417, 110);
                    SeatLengthLabel.Text = "Длина";
                    SeatWidthLabel.Visible = true;
                    label7.Visible = true;
                    SeatWidthTextBox.Visible = true;
                    SeatWidthTextBox.Enabled = true;

                    SeatThicknessLabel.Location = new Point(58, 85);
                    SeatThicknessTextBox.Location = new Point(169, 82);
                    label8.Location = new Point(258, 85);
                    LegsParametersGroupBox.Location = new Point(7, 129);
                    WarningsGroupBox.Location = new Point(7, 228);
                    button1.Location = new Point(166, 318);
                    this.MaximumSize = new Size(452, 390);
                    this.MinimumSize = new Size(452, 390);
                    break;

                case 1:
                    _parameters.SeatType = SeatTypes.RoundSeat;
                    ClearError(_parameterNames[ChairParameters.SeatLength], SeatLengthTextBox);
                    ClearError(_parameterNames[ChairParameters.SeatWidth], SeatWidthTextBox);
                    ClearError(_parameterNames[ChairParameters.SeatThickness], SeatThicknessTextBox);
                    SeatLengthTextBox.BackColor = SystemColors.Window;
                    SeatWidthTextBox.BackColor = SystemColors.Window;
                    SeatThicknessTextBox.BackColor = SystemColors.Window;
                    if (SeatLengthTextBox.Text != "")
                    {
                        ValidateAndSetValue(SeatLengthTextBox, ChairParameters.SeatDiameter);
                    }
                    if (SeatThicknessTextBox.Text != "")
                    {
                        ValidateAndSetValue(SeatThicknessTextBox, ChairParameters.SeatThickness);
                    }

                    if (_parameters.LegsType == LegTypes.SquareLeg && LegsLengthAndWidthTextBox.BackColor == Color.LightCoral)
                    {
                        ValidateAndSetValue(LegsLengthAndWidthTextBox, ChairParameters.LegsWightAndLength);
                    }
                    else if (_parameters.SeatType == SeatTypes.RoundSeat && LegsLengthAndWidthTextBox.BackColor == Color.LightCoral)
                    {
                        ValidateAndSetValue(SeatLengthTextBox, ChairParameters.SeatDiameter);
                    }
                    if (LegsHeightTextBox.BackColor == Color.LightCoral)
                    {
                        ValidateAndSetValue(LegsHeightTextBox, ChairParameters.LegsHeight);
                    }

                    _parameters.SeatWidth = 0;
                    SeatWidthTextBox.Visible = false;
                    SeatWidthTextBox.Enabled = false;
                    SeatWidthTextBox.Text = "";
                    ClearError(_parameterNames[ChairParameters.SeatWidth], SeatWidthTextBox);

                    SeatLengthLabel.Text = "Диаметр";
                    SeatWidthLabel.Visible = false;
                    label7.Visible = false;

                    SeatThicknessLabel.Location = new Point(58, 64);
                    SeatThicknessTextBox.Location = new Point(169, 61);
                    label8.Location = new Point(258, 64);
                    LegsParametersGroupBox.Location = new Point(7, 105);
                    WarningsGroupBox.Location = new Point(7, 204);
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
            switch (LegsParametersComboBox.SelectedIndex)
            {
                case 0:
                    _parameters.LegsType = LegTypes.SquareLeg;
                    _errorMessages.Clear();
                    LegsHeightTextBox.BackColor = SystemColors.Window;
                    LegsLengthAndWidthTextBox.BackColor = SystemColors.Window;
                    LegsLengthLabel.Text = "Длина и ширина";

                    if (LegsHeightTextBox.Text != "")
                    {
                        ValidateAndSetValue(LegsHeightTextBox, ChairParameters.LegsHeight);
                    }
                    if (LegsLengthAndWidthTextBox.Text != "")
                    {
                        ValidateAndSetValue(LegsLengthAndWidthTextBox, ChairParameters.LegsWightAndLength);
                    }
                    switch (_parameters.SeatType)
                    {
                        case SeatTypes.SquareSeat:
                            {
                                if (SeatLengthTextBox.BackColor == Color.LightCoral)
                                {
                                    ValidateAndSetValue(SeatLengthTextBox, ChairParameters.SeatLength);
                                }
                                if (SeatWidthTextBox.BackColor == Color.LightCoral)
                                {
                                    ValidateAndSetValue(SeatWidthTextBox, ChairParameters.SeatWidth);
                                }
                                if (SeatThicknessTextBox.BackColor == Color.LightCoral)
                                {
                                    ValidateAndSetValue(SeatThicknessTextBox, ChairParameters.SeatThickness);
                                }
                                break;
                            }
                        case SeatTypes.RoundSeat:
                            {
                                ValidateAndSetValue(SeatLengthTextBox, ChairParameters.SeatDiameter);
                                ValidateAndSetValue(SeatThicknessTextBox, ChairParameters.SeatThickness);
                                break;
                            }
                    }
                    break;

                case 1:
                    _parameters.LegsType = LegTypes.RoundLeg;
                    _errorMessages.Clear();
                    LegsHeightTextBox.BackColor = SystemColors.Window;
                    LegsLengthAndWidthTextBox.BackColor = SystemColors.Window;
                    LegsLengthLabel.Text = "Диаметр";
                    if (LegsHeightTextBox.Text != "")
                    {
                        ValidateAndSetValue(LegsHeightTextBox, ChairParameters.LegsHeight);
                    }
                    if (LegsLengthAndWidthTextBox.Text != "")
                    {
                        ValidateAndSetValue(LegsLengthAndWidthTextBox, ChairParameters.LegsDiameter);
                    }

                    switch (_parameters.SeatType)
                    {
                        case SeatTypes.SquareSeat:
                            {
                                if (SeatLengthTextBox.BackColor == Color.LightCoral)
                                {
                                    ValidateAndSetValue(SeatLengthTextBox, ChairParameters.SeatLength);
                                }
                                if (SeatWidthTextBox.BackColor == Color.LightCoral)
                                {
                                    ValidateAndSetValue(SeatWidthTextBox, ChairParameters.SeatWidth);
                                }
                                if (SeatThicknessTextBox.BackColor == Color.LightCoral)
                                {
                                    ValidateAndSetValue(SeatThicknessTextBox, ChairParameters.SeatThickness);
                                }
                                break;
                            }
                        case SeatTypes.RoundSeat:
                            {
                                ValidateAndSetValue(SeatLengthTextBox, ChairParameters.SeatDiameter);
                                ValidateAndSetValue(SeatThicknessTextBox, ChairParameters.SeatThickness);
                                break;
                            }
                    }
                    break;
                }
            }
        }
    }