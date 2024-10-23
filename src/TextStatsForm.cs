using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс описывает главную форму приложения
	/// </summary>
	public partial class TextStatsForm: Form
		{
		// Переменные
		private string[] currentStats;
		private string fullStats;

		/// <summary>
		/// Конструктор. Запускает главную форму
		/// </summary>
		public TextStatsForm ()
			{
			// Инициализация
			InitializeComponent ();
			this.Text = ProgramDescription.AssemblyTitle;
			RDGenerics.LoadWindowDimensions (this);

			LanguageCombo.Items.AddRange (RDLocale.LanguagesNames);
			try
				{
				LanguageCombo.SelectedIndex = (int)RDLocale.CurrentLanguage;
				}
			catch
				{
				LanguageCombo.SelectedIndex = 0;
				}

			}

		// Локализация формы
		private void LanguageCombo_SelectedIndexChanged (object sender, EventArgs e)
			{
			// Сохранение языка
			RDLocale.CurrentLanguage = (RDLanguages)LanguageCombo.SelectedIndex;

			// Локализация
			RDLocale.SetControlsText (this);
			BExit.Text = RDLocale.GetDefaultText (RDLDefaultTexts.Button_Exit);
			AboutButton.Text = RDLocale.GetDefaultText (RDLDefaultTexts.Control_AppAbout);

			OFDialog.Title = RDLocale.GetText ("OFTitle");
			SFDialog.Title = RDLocale.GetText ("SFTitle");
			OFDialog.Filter = SFDialog.Filter = RDLocale.GetText ("OFFilter");

			StatsSection.Items.Clear ();
			StatsSection.Items.AddRange (TextStatsMath.StatisticsGroups);
			StatsSection.SelectedIndex = 0;
			}

		// Запрос справки
		private void AboutButton_Clicked (object sender, EventArgs e)
			{
			RDGenerics.ShowAbout (false);
			}

		// Закрытие окна
		private void BExit_Click (object sender, EventArgs e)
			{
			this.Close ();
			}

		private void TextStatsForm_FormClosing (object sender, FormClosingEventArgs e)
			{
			RDGenerics.SaveWindowDimensions (this);
			}

		// Выбор файла
		private void SelectFileButton_Click (object sender, EventArgs e)
			{
			OFDialog.ShowDialog ();
			}

		private void OFDialog_FileOk (object sender, CancelEventArgs e)
			{
			// Получение текста
			string text = TextStatsMath.GetTextFromFile (OFDialog.FileName);
			if (string.IsNullOrWhiteSpace (text))
				{
				StatsSection.Enabled = false;
				ResultsBox.Text = RDLocale.GetText ("NonTextFile");
				return;
				}

			// Отображение
			StatsSection.Enabled = true;
			StatsLabel.Text = string.Format (RDLocale.GetText ("StatsForTextFile"),
				Path.GetFileName (OFDialog.FileName));
			currentStats = TextStatsMath.GetStatistics (text);
			fullStats = TextStatsMath.MakeFullStatistics (currentStats);

			StatsSection_SelectedIndexChanged (null, null);
			}

		// Вызов статистики для введённого текста
		private void GetManualStats_Click (object sender, EventArgs e)
			{
			// Запрос
			currentStats = TextStatsMath.GetStatistics (ManualTextBox.Text);
			if (currentStats == null)
				{
				ResultsBox.Text = RDLocale.GetText ("TextIsEmpty");
				StatsSection.Enabled = false;
				return;
				}

			// Отображение
			fullStats = TextStatsMath.MakeFullStatistics (currentStats);
			StatsSection.Enabled = true;
			StatsLabel.Text = RDLocale.GetText ("StatsForManualText");

			StatsSection_SelectedIndexChanged (null, null);
			}

		// Сохранение статистики
		private void SaveStats_Click (object sender, EventArgs e)
			{
			SFDialog.ShowDialog ();
			}

		private void SFDialog_FileOk (object sender, CancelEventArgs e)
			{
			TextStatsMath.PutTextToFile (SFDialog.FileName, StatsLabel.Text + RDLocale.RNRN + fullStats);
			}

		// Выбор раздела статистики
		private void StatsSection_SelectedIndexChanged (object sender, EventArgs e)
			{
			// Защита
			if (currentStats == null)
				return;

			// Полная статистика
			if (StatsSection.SelectedIndex == 0)
				{
				ResultsBox.Text = fullStats;
				ResultsBox.Focus ();
				ResultsBox.DeselectAll ();
				return;
				}

			// Только выбранная секция
			ResultsBox.Text = currentStats[StatsSection.SelectedIndex - 1];
			ResultsBox.Focus ();
			ResultsBox.DeselectAll ();
			}
		}
	}
