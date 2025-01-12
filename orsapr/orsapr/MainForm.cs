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
        private Dictionary<StoolParameters, string> _parameterNames = new Dictionary<StoolParameters, string>
        {
            {StoolParameters.SeatLength, "Длина сиденья"},
            {StoolParameters.SeatDiameter, "Диаметр сиденья"},
            {StoolParameters.SeatWidth, "Ширина сиденья"},
            {StoolParameters.SeatThickness, "Толщина сиденья"},
            {StoolParameters.LegsHeight, "Высота ножек"},
            {StoolParameters.LegsDiameter, "Диаметр ножек"},
            {StoolParameters.LegsWightAndLength, "Ширина и длина ножек"},
            {StoolParameters.DependentParameters, "Зависимые параметры"},
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
            Dictionary<TextBox, StoolParameters> handledTextBoxStoolParameters;
            handledTextBoxStoolParameters = new Dictionary<TextBox, StoolParameters>
                {
                    {SeatLengthTextBox, StoolParameters.SeatDiameter},
                    {SeatWidthTextBox, StoolParameters.SeatWidth},
                    {SeatThicknessTextBox, StoolParameters.SeatThickness},
                    {LegsHeightTextBox, StoolParameters.LegsHeight},
                    {LegsLengthAndWidthTextBox, StoolParameters.LegsDiameter},
                };

            if (_parameters.SeatType == SeatTypes.SquareSeat)
            {
                handledTextBoxStoolParameters = new Dictionary<TextBox, StoolParameters>
                {
                    {SeatLengthTextBox, StoolParameters.SeatLength},
                    {SeatWidthTextBox, StoolParameters.SeatWidth},
                    {SeatThicknessTextBox, StoolParameters.SeatThickness},
                    {LegsHeightTextBox, StoolParameters.LegsHeight},
                    {LegsLengthAndWidthTextBox, StoolParameters.LegsWightAndLength},
                };
            }

            var changedTextBox = (TextBox)sender;
            var stoolParameters = handledTextBoxStoolParameters[changedTextBox];
            ValidateAndSetValue(changedTextBox, stoolParameters);
        }

        /// <summary>
        /// Метод для валидации и установки значения текстового поля.
        /// </summary>
        /// <param name="textBox">Текстовое поле для валидации.</param>
        /// <param name="StoolParameters">Параметр стула.</param>
        private void ValidateAndSetValue(TextBox textBox, StoolParameters StoolParameters)
        {
            var textBoxName = _parameterNames[StoolParameters];
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

            switch (StoolParameters)
            {
                case StoolParameters.SeatLength:
                    _parameters.SeatLength = value;
                    break;
                case StoolParameters.SeatDiameter:
                    _parameters.SeatLength = value;
                    _parameters.SeatWidth = value;
                    break;
                case StoolParameters.SeatWidth:
                    _parameters.SeatWidth = value;
                    break;
                case StoolParameters.SeatThickness:
                    _parameters.SeatThickness = value;
                    AdjustMinValues(value, StoolParameters, label9);
                    break;
                case StoolParameters.LegsHeight:
                    _parameters.LegLength = value;
                    AdjustMinValues(value, StoolParameters, label8);
                    break;
                case StoolParameters.LegsWightAndLength:
                    _parameters.LegWidth = value;
                    break;
                case StoolParameters.LegsDiameter:
                    _parameters.LegWidth = value;
                    break;
            }

            if (StoolParameters == StoolParameters.SeatThickness
                || StoolParameters == StoolParameters.LegsHeight)
            {
                ValidateDependentParameters();
                return;
            }

            if (_parameters.IsWrongValue(value, StoolParameters, out Tuple<int, int> minMax))
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
        private void AdjustMinValues(int value, StoolParameters parameter, System.Windows.Forms.Label label)
        {
            var newMinMax = _parameters.AdjustMinValues(value, parameter);
            label.Text = $"от {newMinMax.Item1} до {newMinMax.Item2} мм";
        }

        /// <summary>
        /// Метод для указания ошибки в текстовом поле
        /// </summary>
        /// <param name="textBox">Текстовое поле</param>
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
        /// <param name="textBox">Текстовое поле.</param>
        private void ClearError(string textBoxName, TextBox textBox)
        {
            string errorMessage;
            if (textBoxName == _parameterNames[StoolParameters.DependentParameters])
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
                    ClearError(_parameterNames[StoolParameters.DependentParameters], SeatThicknessTextBox);
                    ClearError(_parameterNames[StoolParameters.DependentParameters], LegsHeightTextBox);
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
        /// <param name="textBox">Текстовое поле</param>
        private void SetColors(TextBox textBox)
        {
            textBox.BackColor = Color.LightCoral;
            //TODO: duplication +
        }

        /// <summary>
        /// Метод для сброса цвета текстового поля на стандартный
        /// </summary>
        /// <param name="textBox">Текстовое поле</param>
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
                        ValidateAndSetValue(SeatLengthTextBox, StoolParameters.SeatLength);
                        ValidateAndSetValue(SeatWidthTextBox, StoolParameters.SeatWidth);
                        break;
                    }
                case SeatTypes.RoundSeat:
                    {
                        ValidateAndSetValue(SeatLengthTextBox, StoolParameters.SeatDiameter);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            ValidateAndSetValue(SeatThicknessTextBox, StoolParameters.SeatThickness);

            switch (_parameters.LegsType)
            {
                case LegTypes.SquareLeg:
                    {
                        ValidateAndSetValue(LegsLengthAndWidthTextBox, StoolParameters.LegsWightAndLength);
                        break;
                    }
                case LegTypes.RoundLeg:
                    {
                        ValidateAndSetValue(LegsLengthAndWidthTextBox, StoolParameters.LegsDiameter);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            ValidateAndSetValue(LegsHeightTextBox, StoolParameters.LegsHeight);

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
                    ClearError(_parameterNames[StoolParameters.SeatLength], SeatLengthTextBox);
                    ClearError(_parameterNames[StoolParameters.SeatWidth], SeatWidthTextBox);
                    ClearError(_parameterNames[StoolParameters.SeatThickness], SeatThicknessTextBox);
                    SeatLengthTextBox.BackColor = SystemColors.Window;
                    SeatWidthTextBox.BackColor = SystemColors.Window;
                    SeatThicknessTextBox.BackColor = SystemColors.Window;
                    if (SeatLengthTextBox.Text != "")
                    {
                        ValidateAndSetValue(SeatLengthTextBox, StoolParameters.SeatLength);
                    }
                    if (SeatThicknessTextBox.Text != "")
                    {
                        ValidateAndSetValue(SeatThicknessTextBox, StoolParameters.SeatThickness);
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
                    ClearError(_parameterNames[StoolParameters.SeatLength], SeatLengthTextBox);
                    ClearError(_parameterNames[StoolParameters.SeatWidth], SeatWidthTextBox);
                    ClearError(_parameterNames[StoolParameters.SeatThickness], SeatThicknessTextBox);
                    SeatLengthTextBox.BackColor = SystemColors.Window;
                    SeatWidthTextBox.BackColor = SystemColors.Window;
                    SeatThicknessTextBox.BackColor = SystemColors.Window;
                    if (SeatLengthTextBox.Text != "")
                    {
                        ValidateAndSetValue(SeatLengthTextBox, StoolParameters.SeatDiameter);
                    }
                    if (SeatThicknessTextBox.Text != "")
                    {
                        ValidateAndSetValue(SeatThicknessTextBox, StoolParameters.SeatThickness);
                    }

                    if (_parameters.LegsType == LegTypes.SquareLeg && LegsLengthAndWidthTextBox.BackColor == Color.LightCoral)
                    {
                        ValidateAndSetValue(LegsLengthAndWidthTextBox, StoolParameters.LegsWightAndLength);
                    }
                    else if (_parameters.SeatType == SeatTypes.RoundSeat && LegsLengthAndWidthTextBox.BackColor == Color.LightCoral)
                    {
                        ValidateAndSetValue(SeatLengthTextBox, StoolParameters.SeatDiameter);
                    }
                    if (LegsHeightTextBox.BackColor == Color.LightCoral)
                    {
                        ValidateAndSetValue(LegsHeightTextBox, StoolParameters.LegsHeight);
                    }

                    _parameters.SeatWidth = 0;
                    SeatWidthTextBox.Visible = false;
                    SeatWidthTextBox.Enabled = false;
                    SeatWidthTextBox.Text = "";
                    ClearError(_parameterNames[StoolParameters.SeatWidth], SeatWidthTextBox);

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
                        ValidateAndSetValue(LegsHeightTextBox, StoolParameters.LegsHeight);
                    }
                    if (LegsLengthAndWidthTextBox.Text != "")
                    {
                        ValidateAndSetValue(LegsLengthAndWidthTextBox, StoolParameters.LegsWightAndLength);
                    }
                    switch (_parameters.SeatType)
                    {
                        case SeatTypes.SquareSeat:
                            {
                                if (SeatLengthTextBox.BackColor == Color.LightCoral)
                                {
                                    ValidateAndSetValue(SeatLengthTextBox, StoolParameters.SeatLength);
                                }
                                if (SeatWidthTextBox.BackColor == Color.LightCoral)
                                {
                                    ValidateAndSetValue(SeatWidthTextBox, StoolParameters.SeatWidth);
                                }
                                if (SeatThicknessTextBox.BackColor == Color.LightCoral)
                                {
                                    ValidateAndSetValue(SeatThicknessTextBox, StoolParameters.SeatThickness);
                                }
                                break;
                            }
                        case SeatTypes.RoundSeat:
                            {
                                ValidateAndSetValue(SeatLengthTextBox, StoolParameters.SeatDiameter);
                                ValidateAndSetValue(SeatThicknessTextBox, StoolParameters.SeatThickness);
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
                        ValidateAndSetValue(LegsHeightTextBox, StoolParameters.LegsHeight);
                    }
                    if (LegsLengthAndWidthTextBox.Text != "")
                    {
                        ValidateAndSetValue(LegsLengthAndWidthTextBox, StoolParameters.LegsDiameter);
                    }

                    switch (_parameters.SeatType)
                    {
                        case SeatTypes.SquareSeat:
                            {
                                if (SeatLengthTextBox.BackColor == Color.LightCoral)
                                {
                                    ValidateAndSetValue(SeatLengthTextBox, StoolParameters.SeatLength);
                                }
                                if (SeatWidthTextBox.BackColor == Color.LightCoral)
                                {
                                    ValidateAndSetValue(SeatWidthTextBox, StoolParameters.SeatWidth);
                                }
                                if (SeatThicknessTextBox.BackColor == Color.LightCoral)
                                {
                                    ValidateAndSetValue(SeatThicknessTextBox, StoolParameters.SeatThickness);
                                }
                                break;
                            }
                        case SeatTypes.RoundSeat:
                            {
                                ValidateAndSetValue(SeatLengthTextBox, StoolParameters.SeatDiameter);
                                ValidateAndSetValue(SeatThicknessTextBox, StoolParameters.SeatThickness);
                                break;
                            }
                    }
                    break;
                }
            }
        }
    }