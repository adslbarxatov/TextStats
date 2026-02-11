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
			this.Text = RDGenerics.DefaultAssemblyVisibleName;

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

		// Определения для горячих клавиш

		/// <summary>
		/// Метод переопределяет обработку клавиатуры формой
		/// </summary>
		protected override bool ProcessCmdKey (ref Message msg, Keys keyData)
			{
			switch (keyData)
				{
				// Открытие файла
				case Keys.Control | Keys.O:
					SelectFileButton_Click (null, null);
					return true;

				// Получение из буфера обмена
				case Keys.Control | Keys.B:
					FromClipboardButton_Click (null, null);
					return true;

				// Сохранение статистики
				case Keys.Control | Keys.S:
					SaveStats_Click (null, null);
					return true;

				// Поиск
				case Keys.Control | Keys.F:
					SearchButton_Click (null, null);
					return true;

				// Перезапрос ручной статистики
				case Keys.Control | Keys.G:
					GetManualStats_Click (null, null);
					return true;

				// Остальные клавиши обрабатываются стандартной процедурой
				default:
					return base.ProcessCmdKey (ref msg, keyData);
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
			RDInterface.ShowAbout (false);
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

		// Копирование из буфера обмена
		private void FromClipboardButton_Click (object sender, EventArgs e)
			{
			ManualTextBox.Text = RDGenerics.GetFromClipboard ();
			GetManualStats_Click (null, null);
			}

		// Поиск отдельных символов или слов
		private void SearchButton_Click (object sender, EventArgs e)
			{
			// Контроль существования источника
			bool hasManualText = !string.IsNullOrWhiteSpace (ManualTextBox.Text);
			bool hasFile = !string.IsNullOrWhiteSpace (TextStatsMath.LastFilePath);
			if (!hasManualText && !hasFile)
				{
				RDInterface.LocalizedMessageBox (RDMessageFlags.Warning | RDMessageFlags.CenterText,
					"SearchSourceNotFound", 1000);
				return;
				}

			// Ввод текста
			string textForSearch = RDInterface.LocalizedMessageBox ("SearchRequest", true, 50);
			if (string.IsNullOrWhiteSpace (textForSearch))
				{
				RDInterface.LocalizedMessageBox (RDMessageFlags.Warning | RDMessageFlags.CenterText,
					"SearchSourceNotFound", 1000);
				return;
				}

			// Определение источника
			bool useFile = false;
			bool useManualText = false;
			if (hasFile && hasManualText)
				{
				switch (RDInterface.MessageBox (RDMessageFlags.Question | RDMessageFlags.CenterText,
					RDLocale.GetText ("SearchVariant"),
					RDLocale.GetText ("SearchVariantFile"), RDLocale.GetText ("SearchVariantText"),
					RDLocale.GetDefaultText (RDLDefaultTexts.Button_Cancel)))
					{
					case RDMessageButtons.ButtonOne:
						useFile = true;
						break;

					case RDMessageButtons.ButtonTwo:
						useManualText = true;
						break;
					}
				}

			else if (hasFile)
				{
				useFile = true;
				}
			else //if (hasManualText)
				{
				useManualText = true;
				}

			// Запуск
			string sourceText;
			if (useFile)
				sourceText = TextStatsMath.GetTextFromLastFile ();
			else if (useManualText)
				sourceText = ManualTextBox.Text;
			else
				return;

			// Результат
			string stats = TextStatsMath.SearchForText (sourceText, textForSearch);
			if (RDInterface.MessageBox (RDMessageFlags.Information, stats,
				RDLocale.GetDefaultText (RDLDefaultTexts.Button_Close),
				RDLocale.GetDefaultText (RDLDefaultTexts.Button_Save)) == RDMessageButtons.ButtonTwo)
				{
				if (SFDialog.ShowDialog () != DialogResult.OK)
					return;

				stats = StatsLabel.Text +
					string.Format (RDLocale.GetText ("SearchHeaderFmt"), textForSearch) +
					RDLocale.RNRN + stats;
				TextStatsMath.PutTextToFile (SFDialog.FileName, stats);
				}
			}
		}
	}
