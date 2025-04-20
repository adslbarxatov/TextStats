using RD_AAOW;
using System.Reflection;
using System.Resources;

// Управление общими сведениями о сборке
// ВИДИМЫЕ СТРОКИ
[assembly: AssemblyTitle (ProgramDescription.AssemblyDescription)]
[assembly: AssemblyCompany (RDGenerics.AssemblyCompany)]
// НЕВИДИМЫЕ СТРОКИ
[assembly: AssemblyDescription (ProgramDescription.AssemblyDescription)]
[assembly: AssemblyProduct (ProgramDescription.AssemblyTitle)]
[assembly: AssemblyCopyright (RDGenerics.AssemblyCopyright)]
[assembly: AssemblyVersion (ProgramDescription.AssemblyVersion)]

namespace RD_AAOW
	{
	/// <summary>
	/// Класс, содержащий сведения о программе
	/// </summary>
	public class ProgramDescription
		{
		/// <summary>
		/// Название программы
		/// </summary>
		public const string AssemblyTitle = AssemblyMainName + " v 1.4";

		/// <summary>
		/// Версия программы
		/// </summary>
		public const string AssemblyVersion = "1.4.0.0";

		/// <summary>
		/// Последнее обновление
		/// </summary>
		public const string AssemblyLastUpdate = "20.04.2025; 22:37";

		/// <summary>
		/// Пояснение к программе
		/// </summary>
		public const string AssemblyDescription = "Tool for obtaining stats on specified text";

		/// <summary>
		/// Основное название сборки
		/// </summary>
		public const string AssemblyMainName = "TextStats";

		/// <summary>
		/// Видимое название сборки
		/// </summary>
		public const string AssemblyVisibleName = "• " + AssemblyMainName + " •";

		/// <summary>
		/// Возвращает список менеджеров ресурсов для локализации приложения
		/// </summary>
		public readonly static ResourceManager[] AssemblyResources = [
#if ANDROID
			// Языковые ресурсы
			RD_AAOW.TextStats_ru_ru.ResourceManager,
			RD_AAOW.TextStats_en_us.ResourceManager,
#else
			TextStatsResources.ResourceManager,

			TextStats_ru_ru.ResourceManager,
			TextStats_en_us.ResourceManager,
#endif
			];

		/// <summary>
		/// Возвращает набор ссылок на видеоматериалы по языкам
		/// </summary>
		public readonly static string[] AssemblyVideoLinks = [];

		/// <summary>
		/// Возвращает набор поддерживаемых языков
		/// </summary>
		public readonly static RDLanguages[] AssemblyLanguages = [
			RDLanguages.ru_ru,
			RDLanguages.en_us,
			];

		/// <summary>
		/// Возвращает описание сопоставлений файлов для приложения
		/// </summary>
		public readonly static string[][] AssemblyAssociations = [];
		}
	}
