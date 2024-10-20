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
				ResultsBox.Text = RDLocale.GetText ("NonTextFile");
				return;
				}

			ResultsBox.Text = string.Format (RDLocale.GetText ("StatsForTextFile"),
				Path.GetFileName (OFDialog.FileName)) + RDLocale.RNRN +
				TextStatsMath.GetStatistics (text);
			}

		// Вызов статистики для введённого текста
		private void GetManualStats_Click (object sender, EventArgs e)
			{
			ResultsBox.Text = RDLocale.GetText ("StatsForManualText") + RDLocale.RNRN +
				TextStatsMath.GetStatistics (ManualTextBox.Text);
			}

		// Сохранение статистики
		private void SaveStats_Click (object sender, EventArgs e)
			{
			SFDialog.ShowDialog ();
			}

		private void SFDialog_FileOk (object sender, CancelEventArgs e)
			{
			TextStatsMath.PutTextToFile (SFDialog.FileName, ResultsBox.Text);
			}
		}
	}
