using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;

namespace BeeSweeper.View.Controls
{
    public class SettingsControl : BaseControl
    {
        private readonly Fonts _fonts = new Fonts();
        private readonly NumericUpDown _difficultyTb;
        private readonly NumericUpDown _heightTb;
        private readonly NumericUpDown _widthTb;
        private readonly ComboBox _levelsList;

        public SettingsControl()
        {
            BackColor = Color.White;
            Size = new Size(382, 275);
            var nameLabel = new Label
            {
                Size = new Size(Size.Width, 50),
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(_fonts.Font.FontFamily, 30f, FontStyle.Bold),
                Text = "Settings",
                ForeColor = Palette.Colors.TextColor,
                BackColor = Palette.Colors.InterfaceBackgroundColor
            };

            var panel = new Panel
            {
                Size = new Size(358, 135),
                Location = new Point(12, 86),
                BackColor = Palette.Colors.InterfaceBackgroundColor,
                BorderStyle = BorderStyle.FixedSingle
            };

            _levelsList = new ComboBox
            {
                Location = new Point(24, 76),
                Size = new Size(75, 35),
                FormattingEnabled = false,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Popup,
                Font = new Font(_fonts.ButtonFont.FontFamily, 10)
            };

            var widthLabel = new Label
            {
                AutoSize = true,
                Font = new Font(_fonts.ButtonFont.FontFamily, 10),
                Location = new Point(7, 38),
                Size = new Size(62, 23),
                BackColor = Palette.Colors.InterfaceBackgroundColor,
                Text = "Width:"
            };

            _widthTb = new NumericUpDown
            {
                Font = new Font(_fonts.ButtonFont.FontFamily, 10),
                Location = new Point(60, 35),
                Size = new Size(47, 27),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Palette.Colors.InterfaceBackgroundColor,
                Minimum = 5,
                Name = "Width"
            };

            var heightLabel = new Label
            {
                AutoSize = true,
                Font = new Font(_fonts.ButtonFont.FontFamily, 10),
                Location = new Point(7, 84),
                Size = new Size(70, 23),
                Text = "Height:"
            };

            _heightTb = new NumericUpDown
            {
                Font = new Font(_fonts.ButtonFont.FontFamily, 10),
                Location = new Point(60, 82),
                Size = new Size(47, 27),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Palette.Colors.InterfaceBackgroundColor,
                Minimum = 5,
                Name = "Height"
            };

            var difficultyLabel = new Label
            {
                AutoSize = true,
                Font = new Font(_fonts.ButtonFont.FontFamily, 10),
                Location = new Point(143, 38),
                Size = new Size(91, 25),
                Text = "Difficulty:"
            };

            _difficultyTb = new NumericUpDown
            {
                Font = new Font(_fonts.ButtonFont.FontFamily, 10),
                Location = new Point(210, 35),
                Size = new Size(47, 27),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Palette.Colors.InterfaceBackgroundColor,
                Minimum = 5,
                Maximum = 95,
                Name = "Percent"
            };

            var percentLabel = new Label
            {
                Font = new Font(_fonts.ButtonFont.FontFamily, 10),
                Location = new Point(260, 39),
                Size = new Size(19, 18),
                Text = "%"
            };

            var applyButton = new Button
            {
                Font = new Font(_fonts.ButtonFont.FontFamily, 10, FontStyle.Bold),
                Location = new Point(55, 225),
                Size = new Size(133, 40),
                Text = "Apply",
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = true
            };

            var cancelButton = new Button
            {
                Font = new Font(_fonts.ButtonFont.FontFamily, 10),
                Location = new Point(209, 225),
                Size = new Size(133, 40),
                Text = "Cancel",
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = true
            };

            foreach (var level in Levels.LevelsByName)
                _levelsList.Items.Add(level.Key);

            panel.Controls.Add(percentLabel);
            panel.Controls.Add(widthLabel);
            panel.Controls.Add(_widthTb);
            panel.Controls.Add(heightLabel);
            panel.Controls.Add(_heightTb);
            panel.Controls.Add(difficultyLabel);
            panel.Controls.Add(_difficultyTb);

            Controls.Add(nameLabel);
            Controls.Add(_levelsList);
            Controls.Add(panel);
            Controls.Add(applyButton);
            Controls.Add(cancelButton);

            _levelsList.SelectedIndexChanged += OnLevelsListSelectedIndexChange;
            _widthTb.ValueChanged += (sender, args) => { OnLevelParameterChange(_widthTb); };
            _heightTb.ValueChanged += (sender, args) => { OnLevelParameterChange(_heightTb); };
            _difficultyTb.ValueChanged += (sender, args) => { OnLevelParameterChange(_difficultyTb); };
            applyButton.Click += OnApplyButtonClick;
            cancelButton.Click += OnCancelButtonClick;

            _levelsList.Text = Levels.SelectedLevel.Name;
        }

        public static event Action ApplyButtonClick;
        public static event Action CancelButtonClick;

        private void OnLevelsListSelectedIndexChange(object sender, EventArgs eventArgs)
        {
            var level = Levels.LevelsByName[_levelsList.Text];
            if (_levelsList.Text == "Custom")
                SetValuesFromCustomLevel();
            else
                SetValuesFromLevel(level);
        }

        private void OnLevelParameterChange(NumericUpDown sender)
        {
            if (!sender.Focused)
                return;
            _levelsList.Text = "Custom";
            Levels.CustomLevelParametersByName[sender.Name] = (int) sender.Value;
        }

        private void SetValuesFromLevel(Level level)
        {
            _widthTb.Text = level.Size.Width.ToString();
            _heightTb.Text = level.Size.Height.ToString();
            _difficultyTb.Text = level.Percent.ToString();
        }

        private void SetValuesFromCustomLevel()
        {
            SetValuesFromLevel(new Level("Custom", new Size(Levels.CustomLevelParametersByName["Width"],
                Levels.CustomLevelParametersByName["Height"]), Levels.CustomLevelParametersByName["Percent"]));
        }

        private Level GetLevelFromFields()
        {
            var width = _widthTb.Value;
            var height = _heightTb.Value;
            var percent = _difficultyTb.Value;
            return new Level("Custom", new Size((int) width, (int) height), (int) percent);
        }

        private void OnApplyButtonClick(object sender, EventArgs eventArgs)
        {
            Levels.SelectedLevel = GetLevelFromFields();
            ApplyButtonClick?.Invoke();
        }

        private void OnCancelButtonClick(object sender, EventArgs eventArgs)
        {
            CancelButtonClick?.Invoke();
        }
    }
}