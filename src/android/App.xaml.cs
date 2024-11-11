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

		// Цветовая схема
		private readonly Color
			solutionMasterBackColor = Color.FromArgb ("#e0f0ff"),
			solutionFieldBackColor = Color.FromArgb ("#f0f8ff"),

			aboutMasterBackColor = Color.FromArgb ("#F0FFF0"),
			aboutFieldBackColor = Color.FromArgb ("#D0FFD0");

		#endregion

		#region Переменные страниц

		private ContentPage solutionPage, aboutPage;

		private Label aboutFontSizeField, statsLabel;
		private List<Label> resultFields = new List<Label> ();

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
			flags = AndroidSupport.GetAppStartupFlags (RDAppStartupFlags.DisableXPUN |
				RDAppStartupFlags.CanReadFiles | RDAppStartupFlags.CanWriteFiles);

			// Общая конструкция страниц приложения
			MainPage = new MasterPage ();

			solutionPage = AndroidSupport.ApplyPageSettings (new SolutionPage (), "SolutionPage",
				RDLocale.GetText ("SolutionPage"), solutionMasterBackColor);
			aboutPage = AndroidSupport.ApplyPageSettings (new AboutPage (), "AboutPage",
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_AppAbout),
				aboutMasterBackColor);

			AndroidSupport.SetMasterPage (MainPage, solutionPage, solutionMasterBackColor);

			#region Основная страница

			// Поле ввода текста
			AndroidSupport.ApplyLabelSettings (solutionPage, "EnterTextLabel",
				RDLocale.GetText ("EnterTextLabel"), RDLabelTypes.HeaderLeft);

			manualTextBox = AndroidSupport.ApplyEditorSettings (solutionPage, "ManualTextBox",
				solutionFieldBackColor, Keyboard.Text, 10000, "", null, false);

			AndroidSupport.ApplyButtonSettings (solutionPage, "GetStatsButton",
				RDLocale.GetText ("GetManualStats"), solutionFieldBackColor, GetStatsButton_Clicked, false);

			// Загрузка из файла
			AndroidSupport.ApplyLabelSettings (solutionPage, "LoadFileLabel",
				RDLocale.GetText ("LoadFileLabel"), RDLabelTypes.HeaderLeft);

			AndroidSupport.ApplyButtonSettings (solutionPage, "ClipboardButton",
				RDLocale.GetText ("ClipboardButton"), solutionFieldBackColor, ClipboardButton_Clicked, false);
			Button lfb = AndroidSupport.ApplyButtonSettings (solutionPage, "LoadFileButton",
				RDLocale.GetText ("FileButton"), solutionFieldBackColor, LoadFile_Clicked, false);
			Label lft = AndroidSupport.ApplyLabelSettings (solutionPage, "LoadFileTip",
				RDLocale.GetDefaultText (RDLDefaultTexts.Message_NotificationPermission),
				RDLabelTypes.ErrorTip);

			lfb.IsVisible = !flags.HasFlag (RDAppStartupFlags.CanReadFiles);
			lft.IsVisible = flags.HasFlag (RDAppStartupFlags.CanReadFiles);

			// Раздел результатов
			statsLabel = AndroidSupport.ApplyLabelSettings (solutionPage, "StatsLabel",
				RDLocale.GetText ("StatsLabel"), RDLabelTypes.HeaderLeft);

			resultField = (StackLayout)solutionPage.FindByName ("ResultField");
			resultField.Add (new Label ());

			string[] captions = TextStatsMath.StatisticsGroups;
			for (int i = 0; i < captions.Length - 1; i++)
				{
				Label l = new Label ();
				l.FontAttributes = FontAttributes.Bold | FontAttributes.Italic;
				l.FontSize = AndroidSupport.MasterFontSize * 1.1;
				l.HorizontalOptions = LayoutOptions.Start;
				l.HorizontalTextAlignment = TextAlignment.Start;
				l.Margin = new Thickness (3);
				l.Text = captions[i + 1];
				l.TextColor = AndroidSupport.MasterTextColor;
				resultField.Add (l);

				resultFields.Add (new Label ());
				resultFields[i].BackgroundColor = Color.FromRgba (0, 0, 0, 15);
				resultFields[i].FontAttributes = FontAttributes.None;
				resultFields[i].FontFamily = AndroidSupport.MonospaceFont;
				resultFields[i].FontSize = AndroidSupport.MasterFontSize * 1.05;
				resultFields[i].HorizontalOptions = LayoutOptions.Start;
				resultFields[i].HorizontalTextAlignment = TextAlignment.Start;
				resultFields[i].Margin = new Thickness (6);
				resultFields[i].Text = "";
				resultFields[i].TextColor = AndroidSupport.MasterTextColor;
				resultField.Add (resultFields[i]);

				resultField.Add (new Label ());
				}
			resultField.IsVisible = false;

			// Вызов меню и сохранение
			AndroidSupport.ApplyButtonSettings (solutionPage, "MenuButton",
				RDDefaultButtons.Menu, solutionFieldBackColor, AboutButton_Clicked);
			Button ssb = AndroidSupport.ApplyButtonSettings (solutionPage, "SaveStatsButton",
				RDLocale.GetText ("SaveStatsButton"), solutionFieldBackColor, SaveFile_Clicked, false);
			Label sst = AndroidSupport.ApplyLabelSettings (solutionPage, "SaveStatsTip",
				RDLocale.GetDefaultText (RDLDefaultTexts.Message_NotificationPermission),
				RDLabelTypes.ErrorTip);

			ssb.IsVisible = !flags.HasFlag (RDAppStartupFlags.CanWriteFiles);
			sst.IsVisible = flags.HasFlag (RDAppStartupFlags.CanWriteFiles);

			#endregion

			#region Страница "О программе"

			AndroidSupport.ApplyLabelSettings (aboutPage, "AboutLabel",
				RDGenerics.AppAboutLabelText, RDLabelTypes.AppAbout);

			AndroidSupport.ApplyButtonSettings (aboutPage, "ManualsButton",
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_ReferenceMaterials),
				aboutFieldBackColor, ReferenceButton_Click, false);
			AndroidSupport.ApplyButtonSettings (aboutPage, "HelpButton",
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_HelpSupport),
				aboutFieldBackColor, HelpButton_Click, false);
			AndroidSupport.ApplyLabelSettings (aboutPage, "GenericSettingsLabel",
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_GenericSettings),
				RDLabelTypes.HeaderLeft);

			AndroidSupport.ApplyLabelSettings (aboutPage, "RestartTipLabel",
				RDLocale.GetDefaultText (RDLDefaultTexts.Message_RestartRequired),
				RDLabelTypes.TipCenter);

			AndroidSupport.ApplyLabelSettings (aboutPage, "LanguageLabel",
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_InterfaceLanguage),
				RDLabelTypes.DefaultLeft);
			languageButton = AndroidSupport.ApplyButtonSettings (aboutPage, "LanguageSelector",
				RDLocale.LanguagesNames[(int)RDLocale.CurrentLanguage],
				aboutFieldBackColor, SelectLanguage_Clicked, false);

			AndroidSupport.ApplyLabelSettings (aboutPage, "FontSizeLabel",
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_InterfaceFontSize),
				RDLabelTypes.DefaultLeft);
			AndroidSupport.ApplyButtonSettings (aboutPage, "FontSizeInc",
				RDDefaultButtons.Increase, aboutFieldBackColor, FontSizeButton_Clicked);
			AndroidSupport.ApplyButtonSettings (aboutPage, "FontSizeDec",
				RDDefaultButtons.Decrease, aboutFieldBackColor, FontSizeButton_Clicked);
			aboutFontSizeField = AndroidSupport.ApplyLabelSettings (aboutPage, "FontSizeField",
				" ", RDLabelTypes.DefaultCenter);

			AndroidSupport.ApplyLabelSettings (aboutPage, "HelpHeaderLabel",
				RDLocale.GetDefaultText (RDLDefaultTexts.Control_AppAbout),
				RDLabelTypes.HeaderLeft);
			Label htl = AndroidSupport.ApplyLabelSettings (aboutPage, "HelpTextLabel",
				AndroidSupport.GetAppHelpText (), RDLabelTypes.SmallLeft);
			htl.TextType = TextType.Html;

			FontSizeButton_Clicked (null, null);

			#endregion

			// Отображение подсказок первого старта
			ShowStartupTips ();
			}

		// Метод отображает подсказки при первом запуске
		private async void ShowStartupTips ()
			{
			// Контроль XPUN
			if (!flags.HasFlag (RDAppStartupFlags.DisableXPUN))
				await AndroidSupport.XPUNLoop ();

			// Требование принятия Политики
			if (TipsState.HasFlag (TipTypes.PolicyTip))
				return;

			await AndroidSupport.PolicyLoop ();
			TipsState |= TipTypes.PolicyTip;
			}

		/// <summary>
		/// Сохранение настроек программы
		/// </summary>
		protected override void OnSleep ()
			{
			}

		/// <summary>
		/// Возвращает или задаёт состав флагов просмотра справочных сведений
		/// </summary>
		public static TipTypes TipsState
			{
			get
				{
				return (TipTypes)RDGenerics.GetSettings (tipsStatePar, 0);
				}
			set
				{
				RDGenerics.SetSettings (tipsStatePar, (uint)value);
				}
			}
		private const string tipsStatePar = "TipsState";

		/// <summary>
		/// Доступные типы уведомлений
		/// </summary>
		public enum TipTypes
			{
			/// <summary>
			/// Принятие Политики и первая подсказка
			/// </summary>
			PolicyTip = 0x0001,
			}

		#endregion

		#region О приложении

		// Выбор языка приложения
		private async void SelectLanguage_Clicked (object sender, EventArgs e)
			{
			languageButton.Text = await AndroidSupport.CallLanguageSelector ();
			}

		// Вызов справочных материалов
		private async void ReferenceButton_Click (object sender, EventArgs e)
			{
			await AndroidSupport.CallHelpMaterials (RDHelpMaterials.ReferenceMaterials);
			}

		private async void HelpButton_Click (object sender, EventArgs e)
			{
			await AndroidSupport.CallHelpMaterials (RDHelpMaterials.HelpAndSupport);
			}

		// Изменение размера шрифта интерфейса
		private void FontSizeButton_Clicked (object sender, EventArgs e)
			{
			if (sender != null)
				{
				Button b = (Button)sender;
				if (AndroidSupport.IsNameDefault (b.Text, RDDefaultButtons.Increase))
					AndroidSupport.MasterFontSize += 0.5;
				else if (AndroidSupport.IsNameDefault (b.Text, RDDefaultButtons.Decrease))
					AndroidSupport.MasterFontSize -= 0.5;
				}

			aboutFontSizeField.Text = AndroidSupport.MasterFontSize.ToString ("F1");
			aboutFontSizeField.FontSize = AndroidSupport.MasterFontSize;
			}

		#endregion

		#region Рабочая зона

		// Метод открывает страницу О программе
		private void AboutButton_Clicked (object sender, EventArgs e)
			{
			AndroidSupport.SetCurrentPage (aboutPage, aboutMasterBackColor);
			}

		// Метод извлекает текст из буфера обмена
		private async void ClipboardButton_Clicked (object sender, EventArgs e)
			{
			AndroidSupport.HideKeyboard (manualTextBox);
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
				AndroidSupport.ShowBalloon (RDLocale.GetText ("NonTextFile"), true);
				return;
				}

			// Запрос
			currentStats = TextStatsMath.GetStatistics (manualTextBox.Text);
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
				AndroidSupport.ShowBalloon (RDLocale.GetText ("TextIsEmpty"), true);
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
			AndroidSupport.HideKeyboard (manualTextBox);
			FillFields ();
			}

		// Метод сохраняет статистику в файл
		private async void SaveFile_Clicked (object sender, EventArgs e)
			{
			await TextStatsMath.PutTextToFile ("Stats.txt", fullStats);
			}

		#endregion
		}
	}
