using Microsoft.Maui.Controls;
using System.ComponentModel;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace RD_AAOW
	{
	/// <summary>
	/// Класс описывает функционал приложения
	/// </summary>
	public partial class App: Application
		{
		#region Общие переменные и константы

		// Прочие параметры
		private RDAppStartupFlags flags;
		private string[] currentStats;
		private string fullStats;

		private List<string> searchVariants = [];

		// Цветовая схема
		private readonly Color solutionMasterBackColor = Color.FromArgb ("#e0f0ff");
		private readonly Color solutionFieldBackColor = Color.FromArgb ("#f0f8ff");
		private readonly Color aboutMasterBackColor = Color.FromArgb ("#F0FFF0");
		private readonly Color aboutFieldBackColor = Color.FromArgb ("#D0FFD0");

		#endregion

		#region Переменные страниц

		private ContentPage solutionPage, aboutPage;

		private Label aboutFontSizeField, statsLabel;
		private List<Label> resultFields = [];

		private Button languageButton;

		private Editor manualTextBox;

		private StackLayout resultField;

		#endregion

		#region Запуск и настройка

		/// <summary>
		/// Конструктор. Точка входа приложения
		/// </summary>
		public App ()
			{
			// Инициализация
			InitializeComponent ();
			}

		// Замена определению MainPage = new MasterPage ()
		protected override Window CreateWindow (IActivationState activationState)
			{
			return new Window (AppShell ());
			}

		// Инициализация разметки страниц
		private Page AppShell()
			{
			Page mainPage = new MasterPage ();
			flags = RDGenerics.GetAppStartupFlags (RDAppStartupFlags.DisableXPUN |
				RDAppStartupFlags.CanReadFiles | RDAppStartupFlags.CanWriteFiles);

			// Общая конструкция страниц приложения
			solutionPage = RDInterface.ApplyPageSettings (new SolutionPage (),
				RDLocale.GetText ("SolutionPage"), solutionMasterBackColor);
			aboutPage = RDInterface.ApplyPageSettings (new AboutPage (),
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_AppAbout),
				aboutMasterBackColor);

			RDInterface.SetMasterPage (mainPage, solutionPage, solutionMasterBackColor);

			#region Основная страница

			// Поле ввода текста
			RDInterface.ApplyLabelSettings (solutionPage, "EnterTextLabel",
				RDLocale.GetText ("EnterTextLabel"), RDLabelTypes.HeaderLeft);

			manualTextBox = RDInterface.ApplyEditorSettings (solutionPage, "ManualTextBox",
				solutionFieldBackColor, Keyboard.Text, 10000, "", null, false);

			RDInterface.ApplyButtonSettings (solutionPage, "GetStatsButton",
				RDLocale.GetText ("GetManualStats"), solutionFieldBackColor, GetStatsButton_Clicked, false);

			// Загрузка из файла
			RDInterface.ApplyLabelSettings (solutionPage, "LoadFileLabel",
				RDLocale.GetText ("LoadFileLabel"), RDLabelTypes.HeaderLeft);

			RDInterface.ApplyButtonSettings (solutionPage, "ClipboardButton",
				RDLocale.GetText ("ClipboardButton"), solutionFieldBackColor, ClipboardButton_Clicked, false);
			Button lfb = RDInterface.ApplyButtonSettings (solutionPage, "LoadFileButton",
				RDLocale.GetText ("FileButton"), solutionFieldBackColor, LoadFile_Clicked, false);
			Label lft = RDInterface.ApplyLabelSettings (solutionPage, "LoadFileTip",
				RDLocale.GetDefaultText (RDLDefaultTexts.Message_NotificationPermission),
				RDLabelTypes.ErrorTip);

			lfb.IsVisible = !flags.HasFlag (RDAppStartupFlags.CanReadFiles);
			lft.IsVisible = flags.HasFlag (RDAppStartupFlags.CanReadFiles);

			// Раздел результатов
			statsLabel = RDInterface.ApplyLabelSettings (solutionPage, "StatsLabel",
				RDLocale.GetText ("StatsLabel"), RDLabelTypes.HeaderLeft);

			resultField = (StackLayout)solutionPage.FindByName ("ResultField");
			resultField.Add (new Label ());

			string[] captions = TextStatsMath.StatisticsGroups;
			for (int i = 0; i < captions.Length - 1; i++)
				{
				Label l = new Label ();
				l.FontAttributes = FontAttributes.Bold | FontAttributes.Italic;
				l.FontSize = RDInterface.MasterFontSize * 1.1;
				l.HorizontalOptions = LayoutOptions.Start;
				l.HorizontalTextAlignment = TextAlignment.Start;
				l.Margin = new Thickness (3);
				l.Text = captions[i + 1];
				l.TextColor = RDInterface.GetInterfaceColor (RDInterfaceColors.AndroidTextColor);
				resultField.Add (l);

				resultFields.Add (new Label ());
				resultFields[i].FontAttributes = FontAttributes.None;
				resultFields[i].FontFamily = RDGenerics.MonospaceFont;
				resultFields[i].FontSize = RDInterface.MasterFontSize * 1.05;
				resultFields[i].HorizontalOptions = LayoutOptions.Start;
				resultFields[i].HorizontalTextAlignment = TextAlignment.Start;
				resultFields[i].Margin = new Thickness (6);
				resultFields[i].Text = "";
				resultFields[i].TextColor = RDInterface.GetInterfaceColor (RDInterfaceColors.AndroidTextColor);
				resultField.Add (resultFields[i]);

				resultField.Add (new Label ());
				}
			resultField.IsVisible = false;

			// Вызов меню и сохранение
			RDInterface.ApplyButtonSettings (solutionPage, "MenuButton",
				RDDefaultButtons.Menu, solutionFieldBackColor, AboutButton_Clicked);
			RDInterface.ApplyButtonSettings (solutionPage, "SearchButton",
				RDDefaultButtons.Find, solutionFieldBackColor, SearchButton_Clicked);
			Button ssb = RDInterface.ApplyButtonSettings (solutionPage, "SaveStatsButton",
				RDLocale.GetText ("SaveStatsButton"), solutionFieldBackColor, SaveFile_Clicked, false);
			Label sst = RDInterface.ApplyLabelSettings (solutionPage, "SaveStatsTip",
				RDLocale.GetDefaultText (RDLDefaultTexts.Message_NotificationPermission),
				RDLabelTypes.ErrorTip);

			ssb.IsVisible = !flags.HasFlag (RDAppStartupFlags.CanWriteFiles);
			sst.IsVisible = flags.HasFlag (RDAppStartupFlags.CanWriteFiles);

			#endregion

			#region Страница "О программе"

			RDInterface.ApplyLabelSettings (aboutPage, "AboutLabel",
				RDGenerics.AppAboutLabelText, RDLabelTypes.AppAbout);

			RDInterface.ApplyButtonSettings (aboutPage, "ManualsButton",
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_ReferenceMaterials),
				aboutFieldBackColor, ReferenceButton_Click, false);
			RDInterface.ApplyButtonSettings (aboutPage, "HelpButton",
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_HelpSupport),
				aboutFieldBackColor, HelpButton_Click, false);
			RDInterface.ApplyLabelSettings (aboutPage, "GenericSettingsLabel",
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_GenericSettings),
				RDLabelTypes.HeaderLeft);

			RDInterface.ApplyLabelSettings (aboutPage, "RestartTipLabel",
				RDLocale.GetDefaultText (RDLDefaultTexts.Message_RestartRequired),
				RDLabelTypes.TipCenter);

			RDInterface.ApplyLabelSettings (aboutPage, "LanguageLabel",
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_InterfaceLanguage),
				RDLabelTypes.DefaultLeft);
			languageButton = RDInterface.ApplyButtonSettings (aboutPage, "LanguageSelector",
				RDLocale.LanguagesNames[(int)RDLocale.CurrentLanguage],
				aboutFieldBackColor, SelectLanguage_Clicked, false);

			RDInterface.ApplyLabelSettings (aboutPage, "FontSizeLabel",
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_InterfaceFontSize),
				RDLabelTypes.DefaultLeft);
			RDInterface.ApplyButtonSettings (aboutPage, "FontSizeInc",
				RDDefaultButtons.Increase, aboutFieldBackColor, FontSizeButton_Clicked);
			RDInterface.ApplyButtonSettings (aboutPage, "FontSizeDec",
				RDDefaultButtons.Decrease, aboutFieldBackColor, FontSizeButton_Clicked);
			aboutFontSizeField = RDInterface.ApplyLabelSettings (aboutPage, "FontSizeField",
				" ", RDLabelTypes.DefaultCenter);

			RDInterface.ApplyLabelSettings (aboutPage, "HelpHeaderLabel",
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_AppAbout),
				RDLabelTypes.HeaderLeft);
			Label htl = RDInterface.ApplyLabelSettings (aboutPage, "HelpTextLabel",
				RDGenerics.GetAppHelpText (), RDLabelTypes.SmallLeft);
			htl.TextType = TextType.Html;

			FontSizeButton_Clicked (null, null);

			#endregion

			// Отображение подсказок первого старта
			ShowStartupTips ();
			return mainPage;
			}

		// Метод отображает подсказки при первом запуске
		private async void ShowStartupTips ()
			{
			// Контроль XPUN
			if (!flags.HasFlag (RDAppStartupFlags.DisableXPUN))
				await RDInterface.XPUNLoop ();

			// Требование принятия Политики
			await RDInterface.PolicyLoop ();
			}

		/// <summary>
		/// Сохранение настроек программы
		/// </summary>
		protected override void OnSleep ()
			{
			}

		#endregion

		#region О приложении

		// Выбор языка приложения
		private async void SelectLanguage_Clicked (object sender, EventArgs e)
			{
			languageButton.Text = await RDInterface.CallLanguageSelector ();
			}

		// Вызов справочных материалов
		private async void ReferenceButton_Click (object sender, EventArgs e)
			{
			await RDInterface.CallHelpMaterials (RDHelpMaterials.ReferenceMaterials);
			}

		private async void HelpButton_Click (object sender, EventArgs e)
			{
			await RDInterface.CallHelpMaterials (RDHelpMaterials.HelpAndSupport);
			}

		// Изменение размера шрифта интерфейса
		private void FontSizeButton_Clicked (object sender, EventArgs e)
			{
			if (sender != null)
				{
				Button b = (Button)sender;
				if (RDInterface.IsNameDefault (b.Text, RDDefaultButtons.Increase))
					RDInterface.MasterFontSize += 0.5;
				else if (RDInterface.IsNameDefault (b.Text, RDDefaultButtons.Decrease))
					RDInterface.MasterFontSize -= 0.5;
				}

			aboutFontSizeField.Text = RDInterface.MasterFontSize.ToString ("F1");
			aboutFontSizeField.FontSize = RDInterface.MasterFontSize;
			}

		#endregion

		#region Рабочая зона

		// Метод открывает страницу О программе
		private void AboutButton_Clicked (object sender, EventArgs e)
			{
			RDInterface.SetCurrentPage (aboutPage, aboutMasterBackColor);
			}

		// Метод извлекает текст из буфера обмена
		private async void ClipboardButton_Clicked (object sender, EventArgs e)
			{
			RDGenerics.HideKeyboard (manualTextBox);
			manualTextBox.Text = await RDGenerics.GetFromClipboard ();

			FillFields ();
			}

		// Метод загружает текст из файла
		private async void LoadFile_Clicked (object sender, EventArgs e)
			{
			string text = await TextStatsMath.GetTextFromFile ();
			if (string.IsNullOrWhiteSpace (text))
				{
				ClearFields (false);
				RDInterface.ShowBalloon (RDLocale.GetText ("NonTextFile"), true);
				return;
				}

			// Запрос
			currentStats = TextStatsMath.GetStatistics (text);
			if (currentStats == null)
				{
				ClearFields (true);
				return;
				}

			// Отображение
			statsLabel.Text = RDLocale.GetText ("StatsForTextFile");
			LoadFields ();
			}

		// Методы обработки отдельных секций статистики
		private void FillFields ()
			{
			// Запрос
			currentStats = TextStatsMath.GetStatistics (manualTextBox.Text);
			if (currentStats == null)
				{
				ClearFields (true);
				return;
				}

			// Отображение
			statsLabel.Text = RDLocale.GetText ("StatsForManualText");
			LoadFields ();
			}

		private void ClearFields (bool Message)
			{
			for (int i = 0; i < resultFields.Count; i++)
				resultFields[i].Text = "";

			resultField.IsVisible = false;
			if (Message)
				RDInterface.ShowBalloon (RDLocale.GetText ("TextIsEmpty"), true);
			}

		private void LoadFields ()
			{
			fullStats = TextStatsMath.MakeFullStatistics (currentStats);
			for (int i = 0; i < resultFields.Count; i++)
				resultFields[i].Text = currentStats[i];
			resultField.IsVisible = true;
			}

		// Метод получается статистику по введённому тексту
		private void GetStatsButton_Clicked (object sender, EventArgs e)
			{
			RDGenerics.HideKeyboard (manualTextBox);
			FillFields ();
			}

		// Метод сохраняет статистику в файл
		private async void SaveFile_Clicked (object sender, EventArgs e)
			{
			await TextStatsMath.PutTextToFile ("Stats.txt", fullStats);
			}

		// Поиск отдельных символов или слов
		private async void SearchButton_Clicked (object sender, EventArgs e)
			{
			// Контроль существования источника
			bool hasManualText = !string.IsNullOrWhiteSpace (manualTextBox.Text);
			bool hasFile = !string.IsNullOrWhiteSpace (TextStatsMath.LastFilePath);
			if (!hasManualText && !hasFile)
				{
				RDInterface.ShowBalloon (RDLocale.GetText ("SearchSourceNotFound"), true);
				return;
				}

			// Ввод текста
			string textForSearch = await RDInterface.ShowInput (ProgramDescription.AssemblyVisibleName,
				RDLocale.GetText ("SearchRequest"), RDLocale.GetDefaultText (RDLDefaultTexts.Button_OK),
				RDLocale.GetDefaultText (RDLDefaultTexts.Button_Cancel), 50, Keyboard.Default);
			if (string.IsNullOrWhiteSpace (textForSearch))
				{
				RDInterface.ShowBalloon (RDLocale.GetText ("SearchSourceNotFound"), true);
				return;
				}

			// Определение источника
			bool useFile = false;
			bool useManualText = false;
			if (hasFile && hasManualText)
				{
				if (searchVariants.Count < 1)
					{
					searchVariants.Add (RDLocale.GetText ("SearchVariantFile"));
					searchVariants.Add (RDLocale.GetText ("SearchVariantText"));
					}

				int res = await RDInterface.ShowList (RDLocale.GetText ("SearchVariant"),
					RDLocale.GetDefaultText (RDLDefaultTexts.Button_Cancel), searchVariants);

				switch (res)
					{
					case 0:
						useFile = true;
						break;

					case 1:
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
				sourceText = manualTextBox.Text;
			else
				return;

			// Результат
			string stats = TextStatsMath.SearchForText (sourceText, textForSearch);
			if (!await RDInterface.ShowMessage (stats,
				RDLocale.GetDefaultText (RDLDefaultTexts.Button_Close),
				RDLocale.GetDefaultText (RDLDefaultTexts.Button_Save)))
				{
				stats = statsLabel.Text +
					string.Format (RDLocale.GetText ("SearchHeaderFmt"), textForSearch) +
					RDLocale.RNRN + stats;
				await TextStatsMath.PutTextToFile ("SearchStats.txt", stats);
				}
			}

		#endregion
		}
	}
