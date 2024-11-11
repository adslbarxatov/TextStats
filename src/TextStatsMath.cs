using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс описывает счётчик количества для отдельной буквы или символа
	/// </summary>
	public class TSLetter: IComparable<TSLetter>, IEquatable<TSLetter>
		{
		/// <summary>
		/// Образец буквы или символа, для которого сформирован счётчик, в нижнем регистре
		/// </summary>
		public string Letter
			{
			get
				{
				return letter;
				}
			}
		private string letter;

		/// <summary>
		/// Возвращает количество образца в верхнем регистре
		/// </summary>
		public uint UpperQuantity
			{
			get
				{
				return upperQuantity;
				}
			}
		private uint upperQuantity;

		/// <summary>
		/// Возвращает количество образца в нижнем регистре
		/// </summary>
		public uint LowerQuantity
			{
			get
				{
				return lowerQuantity;
				}
			}
		private uint lowerQuantity;

		/// <summary>
		/// Возвращает суммарное количество образца
		/// </summary>
		public uint TotalQuantity
			{
			get
				{
				return lowerQuantity + upperQuantity;
				}
			}

		/// <summary>
		/// Конструктор. Инициализирует экземпляр счётчика
		/// </summary>
		/// <param name="Letter">Образец буквы, для которого формируется счётчик;
		/// при указании автоматически переводится в нижний регистр</param>
		/// <param name="UQuantity">Количество образца в верхнем регистре</param>
		/// <param name="LQuantity">Количество образца в нижнем регистре</param>
		public TSLetter (string Letter, uint UQuantity, uint LQuantity)
			{
			if (string.IsNullOrWhiteSpace (Letter))
				throw new Exception ("TSLetter init failure: empty string passed as parameter");

			letter = Letter.ToLower ().Substring (0, 1);
			upperQuantity = UQuantity;
			lowerQuantity = LQuantity;
			}

		/// <summary>
		/// Конструктор. Инициализирует экземпляр счётчика
		/// </summary>
		/// <param name="Character">Образец символа, для которого формируется счётчик;
		/// при указании передаётся в конструктор без изменений</param>
		/// <param name="Quantity">Количество образца в счётчике</param>
		public TSLetter (char Character, uint Quantity)
			{
			letter = Character.ToString ();
			upperQuantity = Quantity;
			lowerQuantity = 0;
			}

		/// <summary>
		/// Метод возвращает результат сравнения данного экземпляра счётчика с указанным Sample
		/// </summary>
		/// <param name="Sample">Экземпляр счётчика для сравнения</param>
		public int CompareTo (TSLetter Sample)
			{
			if (TotalQuantity < Sample.TotalQuantity)
				return 1;

			if (TotalQuantity > Sample.TotalQuantity)
				return -1;

			return 0;
			}

		/// <summary>
		/// Метод возвращает true, если образец буквы данного экземпляра совпадает с образцом в Sample
		/// </summary>
		/// <param name="Sample">Экземпляр счётчика для сравнения</param>
		public bool Equals (TSLetter Sample)
			{
			return Letter == Sample.Letter;
			}
		}

	/// <summary>
	/// Класс описывает счётчик количества для строкового элемента
	/// </summary>
	public class TSString: IComparable<TSString>, IEquatable<TSString>
		{
		/// <summary>
		/// Образец строки, для которого сформирован счётчик, в нижнем регистре
		/// </summary>
		public string Sample
			{
			get
				{
				return sample;
				}
			}
		private string sample;

		/// <summary>
		/// Возвращает длину образца строки
		/// </summary>
		public uint Length
			{
			get
				{
				return (uint)sample.Length;
				}
			}

		/// <summary>
		/// Возвращает количество образца строки такой же длины
		/// </summary>
		public uint Quantity
			{
			get
				{
				return quantity;
				}
			}
		private uint quantity;

		/// <summary>
		/// Метод увеличивает количество образца на единицу
		/// </summary>
		public void Increase ()
			{
			quantity++;
			}

		/// <summary>
		/// Конструктор. Инициализирует экземпляр счётчика
		/// </summary>
		/// <param name="SSample">Образец строки, для которого формируется счётчик</param>
		/// <param name="SortByLength">Флаг указывает на необходимость сортировки экземпляров
		/// по длине образца, а не по его количеству</param>
		public TSString (string SSample, bool SortByLength)
			{
			if (string.IsNullOrWhiteSpace (SSample))
				throw new Exception ("TSString init failure: empty string passed as parameter");

			sample = SSample;
			quantity = 1;
			sortByLength = SortByLength;
			}

		/// <summary>
		/// Конструктор. Инициализирует экземпляр счётчика
		/// </summary>
		/// <param name="SLength">Длина образца строки, для которого формируется счётчик</param>
		/// <param name="SortByLength">Флаг указывает на необходимость сортировки экземпляров
		/// по длине образца, а не по его количеству</param>
		public TSString (uint SLength, bool SortByLength)
			{
			if (SLength < 1)
				throw new Exception ("TSString init failure: zero length passed as parameter");

			sample = "A".PadLeft ((int)SLength, 'A');
			quantity = 1;
			sortByLength = SortByLength;
			}

		/// <summary>
		/// Метод возвращает результат сравнения данного экземпляра счётчика с указанным Sample
		/// </summary>
		/// <param name="Sample">Экземпляр счётчика для сравнения</param>
		public int CompareTo (TSString Sample)
			{
			if (sortByLength)
				{
				if (Length < Sample.Length)
					return 1;

				if (Length > Sample.Length)
					return -1;
				}
			else
				{
				if (Quantity < Sample.Quantity)
					return 1;

				if (Quantity > Sample.Quantity)
					return -1;
				}

			return 0;
			}

		/// <summary>
		/// Метод возвращает true, если длина образца строки данного экземпляра совпадает с длиной в Sample
		/// </summary>
		/// <param name="Sample">Экземпляр счётчика для сравнения</param>
		public bool Equals (TSString Sample)
			{
			return Length == Sample.Length;
			}
		private bool sortByLength;
		}

	/// <summary>
	/// Класс описывает математику приложения
	/// </summary>
	public static class TextStatsMath
		{
		/// <summary>
		/// Метод получает читаемый текст из файла, если это возможно
		/// </summary>
		/// <param name="FilePath">Путь к текстовому файлу</param>
		/// <returns>Текстовое содержимое файла</returns>
#if ANDROID
		public static async Task<string> GetTextFromFile ()
#else
		public static string GetTextFromFile (string FilePath)
#endif
			{
			// Попытка открытия файла
#if ANDROID
			string text = await AndroidSupport.LoadFromFile (RDEncodings.UTF8);
#else
			List<byte> res = new List<byte> ();
			string text = "";

			try
				{
				res = new List<byte> (File.ReadAllBytes (FilePath));
				}
			catch
				{
				return text;
				}

			// Подбор кодировки
			for (int i = 0; i < 3; i++)
				{
				Encoding e;
				switch (i)
					{
					case 0:
					default:
						e = RDGenerics.GetEncoding (RDEncodings.Unicode16);
						break;

					case 1:
						e = RDGenerics.GetEncoding (RDEncodings.UTF8);
						break;

					case 2:
						e = RDGenerics.GetEncoding (RDEncodings.CP1251);
						break;
					}

				byte[] preamble = e.GetPreamble ();
				bool encodingFound = true;
				for (int j = 0; j < preamble.Length; j++)
					{
					if (preamble[j] != res[j])
						{
						encodingFound = false;
						break;
						}
					}
				if (!encodingFound)
					continue;

				// Успешно
				text = e.GetString (res.GetRange (preamble.Length, res.Count - preamble.Length).ToArray ());
#endif

				for (int j = 0; j < nonTextChars.Length; j++)
					{
					if (text.Contains (nonTextChars[j].ToString ()))
						{
						text = "";
						break;
						}
					}

#if !ANDROID
				if (!string.IsNullOrWhiteSpace (text))
					break;
				}
#endif

			return text;
			}
		private static char[] nonTextChars = new char[] {
			'\x01', '\x02', '\x03', '\x04', '\x05', '\x06', '\x07', '\x08',
			'\x0B', '\x0C', '\x0E', '\x0F', '\x10', '\x11', '\x12', '\x13',
			'\x14', '\x15', '\x16', '\x17', '\x18', '\x19', '\x1A', '\x1B',
			'\x1C', '\x1D', '\x1E', '\x1F',
			};

		/// <summary>
		/// Метод сохраняет статистику в файл
		/// </summary>
		/// <param name="FilePath">Путь к текстовому файлу</param>
		/// <param name="Text">Текст статистики</param>
#if ANDROID
		public static async Task<bool> PutTextToFile (string FileName, string Text)
#else
		public static void PutTextToFile (string FilePath, string Text)
#endif
			{
#if ANDROID
			return await AndroidSupport.SaveToFile (FileName, Text, RDEncodings.UTF8);
#else
			try
				{
				File.WriteAllText (FilePath, Text + RDLocale.RN,
					RDGenerics.GetEncoding (RDEncodings.Unicode16));
				}
			catch { }
#endif
			}

		/// <summary>
		/// Возвращает список групп статистики для текущего языка
		/// </summary>
		public static string[] StatisticsGroups
			{
			get
				{
				switch (RDLocale.CurrentLanguage)
					{
					case RDLanguages.ru_ru:
						return new string[] {
							"Все параметры",
							"Общее",
							"Буквы",
							"Цифры",
							"Остальные символы",
							"Слова",
							"Предложения в символах",
							"Предложения в словах",
							"Абзацы в символах",
							"Абзацы в словах",
							"Абзацы в предложениях",
							};

					case RDLanguages.en_us:
					default:
						return new string[] {
							"All parameters",
							"Generic",
							"Letters",
							"Digits",
							"Other characters",
							"Words",
							"Sententes in characters",
							"Sententes in words",
							"Paragraphs in characters",
							"Paragraphs in words",
							"Paragraphs in sentences",
							};
					}
				}
			}

		// Текстовые фрагменты разделов статистики
		private const int stGeneric = 0;
		private const int stLetters = 1;
		private const int stDigits = 2;
		private const int stChars = 3;
		private const int stWords = 4;
		private const int stSentencesChars = 5;
		private const int stSentencesWords = 6;
		private const int stParagraphsChars = 7;
		private const int stParagraphsWords = 8;
		private const int stParagraphsSentences = 9;

		private static string[][] captions = new string[][] {
			new string[] { "Длина текста, символов: ", "Text length, chars: " },
			new string[] { "Всего букв: ", "Total letters: " },
			new string[] { "Всего цифр: ", "Total digits: " },
			new string[] { "Всего символов: ", "Total characters: " },
			new string[] { " {0:S} бкв. – {1:S} слв.", " {0:S} lts. – {1:S} wds." },
			new string[] { " {0:S} смв. – {1:S} прдл.", " {0:S} chr. – {1:S} snt." },
			new string[] { " {0:S} слв. – {1:S} прдл.", " {0:S} wdr. – {1:S} snt." },
			new string[] { " {0:S} смв. – {1:S} абз.", " {0:S} chr. – {1:S} par." },
			new string[] { " {0:S} слв. – {1:S} абз.", " {0:S} wdr. – {1:S} par." },
			new string[] { " {0:S} прдл. – {1:S} абз.", " {0:S} snt. – {1:S} par." },
			};

		// Общие текстовые фрагменты
		private const int gnNotFound = 0;
		private const int gnMinimum = 1;
		private const int gnMaximum = 2;
		private const int gnAverage = 3;
		private const int gnWords = 4;
		private const int gnSentences = 5;
		private const int gnParagraphs = 6;

		private static string[][] generics = new string[][] {
			new string[] { "(не найдены)", "(not found)" },
			new string[] { "Минимальная длина: ", "Minimum length: " },
			new string[] { "Максимальная длина: ", "Maximum length: " },
			new string[] { "Средняя длина: ", "Average length: " },
			new string[] { "Количество слов: ", "Quantity of words: " },
			new string[] { "Количество предложений: ", "Quantity of sentences: " },
			new string[] { "Количество абзацев: ", "Quantity of paragraphs: " },
			};

		// Методы получают текстовые фрагменты применительно к текущему языку
		private static string GetCaption (int Section, int Row)
			{
			return captions[Section][RDLocale.LanguagesNames.Length * Row + (int)RDLocale.CurrentLanguage];
			}
		private static string GetCaption (int Generic)
			{
			return generics[Generic][(int)RDLocale.CurrentLanguage];
			}

		/// <summary>
		/// Метод собирает набор параметров статистики в полный отчёт
		/// </summary>
		/// <param name="StatsSet">Сформированный набор параметров статистики</param>
		/// <returns>Полная статистика</returns>
		public static string MakeFullStatistics (string[] StatsSet)
			{
			string fullStats = "";
			string[] groups = StatisticsGroups;

			if ((StatsSet == null) || (StatsSet.Length < groups.Length - 1))
				return null;

			for (int i = 0; i < groups.Length - 1; i++)
				{
				fullStats += groups[i + 1] + RDLocale.RNRN;
				fullStats += StatsSet[i];
				if (i < groups.Length - 2)
					fullStats += RDLocale.RNRN + RDLocale.RN;
				}

			return fullStats;
			}

		/// <summary>
		/// Метод формирует статистику для указанного текста
		/// </summary>
		/// <param name="Text">Текст для сбора статистики</param>
		/// <returns>Текстовая статистика</returns>
		public static string[] GetStatistics (string Text)
			{
			// Защита
			List<string> stats = new List<string> ();
			if (string.IsNullOrWhiteSpace (Text))
				return null;

			#region Общее

			// Общая длина
			stats.Add ("");
			stats[stGeneric] += GetCaption (stGeneric, 0) + Text.Length;

			#endregion

			#region Все символы (без отображения)

			List<char> chars = new List<char> ();
			List<uint> counts = new List<uint> ();
			for (int i = 0; i < Text.Length; i++)
				{
				int idx = chars.IndexOf (Text[i]);
				if (idx < 0)
					{
					chars.Add (Text[i]);
					counts.Add (1);
					}
				else
					{
					counts[idx]++;
					}
				}

			#endregion

			#region Буквы

			stats.Add ("");

			// 1. Извлечение только букв
			List<char> ltrsSmp = new List<char> ();
			List<uint> ltrsCnt = new List<uint> ();
			for (int i = chars.Count - 1; i >= 0; i--)
				{
				if (!supportedLetters.Contains (chars[i].ToString ()))
					continue;

				ltrsSmp.Add (chars[i]);
				chars.RemoveAt (i);
				ltrsCnt.Add (counts[i]);
				counts.RemoveAt (i);
				}

			ltrsSmp.Reverse ();
			/*List<char> ltrSmp2 = new List<char> (ltrsSmp1.ToArray ());
			ltrsSmp1.Clear ();*/

			ltrsCnt.Reverse ();
			/*List<uint> ltrCts = new List<uint> (ltrsCnt1.ToArray ());
			ltrsCnt1.Clear ();*/
			List<TSLetter> letters = new List<TSLetter> ();

			// 2. Группировка одинаковых букв и расчёт общих количеств
			for (int i = 0; i < ltrsSmp.Count; i++)
				{
				string c = ltrsSmp[i].ToString ();
				char u = c.ToUpper ()[0];
				char l = c.ToLower ()[0];

				TSLetter lt = new TSLetter (c, 0, 0);
				if (!letters.Contains (lt))
					{
					/*lettersSamples.Add (u);*/

					int idxu = ltrsSmp.IndexOf (u);
					int idxl = ltrsSmp.IndexOf (l);
					uint countu = (idxu < 0) ? 0 : ltrsCnt[idxu];
					uint countl = (idxl < 0) ? 0 : ltrsCnt[idxl];

					/*lettersCounts.Add (countu + countl);
					lettersCounts.Add (countu);
					lettersCounts.Add (countl);*/
					letters.Add (new TSLetter (c, countu, countl));
					}
				}
			ltrsCnt.Clear ();
			ltrsSmp.Clear ();

			// 3. Сортировка
			/*bool sorted;*/
			letters.Sort ();
			/*do
				{
				sorted = true;
				for (int i = 0; i < lettersSamples.Count - 1; i++)
					{
					// Сортировка по сумме больших и маленьких букв
					if (lettersCounts[3 * i] < lettersCounts[3 * (i + 1)])
						{
						char c = lettersSamples[i];
						lettersSamples[i] = lettersSamples[i + 1];
						lettersSamples[i + 1] = c;

						for (int j = 0; j < 3; j++)
							{
							uint v = lettersCounts[3 * i + j];
							lettersCounts[3 * i + j] = lettersCounts[3 * (i + 1) + j];
							lettersCounts[3 * (i + 1) + j] = v;
							}

						sorted = false;
						}
					}
				} while (!sorted);*/

			// 4. Вывод
			Encoding unicode = RDGenerics.GetEncoding (RDEncodings.Unicode16);
			uint totalLetters = 0;
			for (int i = 0; i < letters.Count; i++)
				{
				/*string c = letters[i].Letter;*/
				byte[] v = unicode.GetBytes (letters[i].Letter);
				uint code = (uint)v[0] | ((uint)v[1] << 8);

				stats[stLetters] += "[ " + letters[i].Letter.ToUpper () + letters[i].Letter +
					" | U+" + code.ToString ("X4") + " ]: " +
					letters[i].UpperQuantity.ToString () + " + " +
					letters[i].LowerQuantity.ToString () + " = " +
					letters[i].TotalQuantity.ToString () + RDLocale.RN;
				totalLetters += letters[i].TotalQuantity;
				}

			if (letters.Count < 1)
				{
				stats[stLetters] += GetCaption (gnNotFound);
				}
			else
				{
				stats[stLetters] += GetCaption (stLetters, 0) + totalLetters.ToString () +
					" (" + (100.0 * totalLetters / Text.Length).ToString (percentageFormat) + "%)";
				}

			#endregion

			#region Цифры

			stats.Add ("");

			// 1. Извлечение только цифр
			/*List<char> dgtsSmp = new List<char> ();
			List<uint> dgtsCnt = new List<uint> ();*/
			List<TSLetter> digits = new List<TSLetter> ();
			for (int i = chars.Count - 1; i >= 0; i--)
				{
				if (!supportedDigits.Contains (chars[i].ToString ()))
					continue;

				digits.Add (new TSLetter (chars[i], counts[i]));
				/*dgtsSmp.Add (chars[i]);*/
				chars.RemoveAt (i);
				/*dgtsCnt.Add (counts[i]);*/
				counts.RemoveAt (i);
				}
			/*dgtsSmp.Reverse ();
			dgtsCnt.Reverse ();*/

			// 2. Сортировка
			digits.Sort ();
			/*do
				{
				sorted = true;
				for (int i = 0; i < digitsCounts.Count - 1; i++)
					{
					// Сортировка по сумме больших и маленьких букв
					if (digitsCounts[i] < digitsCounts[i + 1])
						{
						char c = digitsSamples[i];
						digitsSamples[i] = digitsSamples[i + 1];
						digitsSamples[i + 1] = c;

						uint v = digitsCounts[i];
						digitsCounts[i] = digitsCounts[i + 1];
						digitsCounts[i + 1] = v;

						sorted = false;
						}
					}
				} while (!sorted);*/

			// 3. Вывод
			uint totalDigits = 0;
			for (int i = 0; i < digits.Count; i++)
				{
				stats[stDigits] += "[ " + digits[i].Letter + " ]: " + digits[i].TotalQuantity.ToString () + RDLocale.RN;
				totalDigits += digits[i].TotalQuantity;
				}

			if (digits.Count < 1)
				{
				stats[stDigits] += GetCaption (gnNotFound);
				}
			else
				{
				stats[stDigits] += GetCaption (stDigits, 0) + totalDigits.ToString () +
					" (" + (100.0 * totalDigits / Text.Length).ToString (percentageFormat) + "%)";
				}

			#endregion

			#region Остальные символы

			// 1. Перегонка и сортировка
			stats.Add ("");

			List<TSLetter> characters = new List<TSLetter> ();
			for (int i = 0; i < chars.Count; i++)
				characters.Add (new TSLetter (chars[i], counts[i]));
			chars.Clear ();
			counts.Clear ();

			/*do
				{
				sorted = true;
				for (int i = 0; i < counts.Count - 1; i++)
					{
					// Сортировка по сумме больших и маленьких букв
					if (counts[i] < counts[i + 1])
						{
						char c = chars[i];
						chars[i] = chars[i + 1];
						chars[i + 1] = c;

						uint v = counts[i];
						counts[i] = counts[i + 1];
						counts[i + 1] = v;

						sorted = false;
						}
					}
				} while (!sorted);*/
			characters.Sort ();

			// 2. Отображение
			uint totalChars = 0;
			for (int i = 0; i < characters.Count; i++)
				{
				string c;
				byte[] v = unicode.GetBytes (characters[i].Letter);
				uint code = (uint)v[0] | ((uint)v[1] << 8);

				switch (characters[i].Letter[0])
					{
					default:
						c = " " + characters[i].Letter + " ";
						break;

					case ' ':
						c = "spc";
						break;

					case '\x09':
						c = "tab";
						break;

					case '\x0D':
						c = "ret";
						break;

					case '\x0A':
						c = "eol";
						break;

					case '\xA0':
						c = "nbs";
						break;
					}

				stats[stChars] += "[" + c + " | U+" + code.ToString ("X4") + " ]: " +
					characters[i].TotalQuantity.ToString () + RDLocale.RN;
				totalChars += characters[i].TotalQuantity;
				}

			if (characters.Count < 1)
				{
				stats[stChars] += GetCaption (gnNotFound);
				}
			else
				{
				stats[stChars] += GetCaption (stChars, 0) + totalChars.ToString () +
					" (" + (100.0 * totalChars / Text.Length).ToString (percentageFormat) + "%)";
				}

			#endregion

			#region Слова

			// 1. Дробление на слова и предложения
			stats.Add ("");

			List<List<List<string>>> words2 = new List<List<List<string>>> ();
			words2.Add (new List<List<string>> ());
			words2[0].Add (new List<string> ());
			int prgr = 0, sntn = 0, wrd = 0;

			string word = "";
			List<char> sntSpl = new List<char> (sentenceSplitters2);
			List<char> parSpl = new List<char> (paragraphSplitters2);

			// !!! В текущем виде схема теряет предложения и слова, состоящие целиком из не-букв и не-цифр
			for (int i = 0; i < Text.Length + 1; i++)
				{
				string c = (i < Text.Length) ? Text[i].ToString () : "";
				if ((i < Text.Length) && (supportedLetters.Contains (c) || supportedSubletters.Contains (c) ||
					supportedDigits.Contains (c)))
					{
					word += c;
					}
				else
					{
					if (!string.IsNullOrWhiteSpace (word))
						{
						wrd++;
						words2[prgr][sntn].Add (word);
						word = "";
						}

					if (i < Text.Length)
						{
						if (sntSpl.Contains (c[0]) && (wrd > 0))
							{
							words2[prgr].Add (new List<string> ());
							sntn++;
							wrd = 0;
							}

						if (parSpl.Contains (c[0]) && (sntn > 0))
							{
							words2.Add (new List<List<string>> ());
							prgr++;
							sntn = wrd = 0;
							words2[prgr].Add (new List<string> ());
							}
						}
					}
				}

			// Очистка пустых полей
			for (prgr = words2.Count - 1; prgr >= 0; prgr--)
				{
				for (sntn = words2[prgr].Count - 1; sntn >= 0; sntn--)
					{
					if (words2[prgr][sntn].Count < 1)
						words2[prgr].RemoveAt (sntn);
					}

				if (words2[prgr].Count < 1)
					words2.RemoveAt (prgr);
				}

			// 2. Расчёт длин
			/*List<uint> wordsLengths = new List<uint> ();
			List<uint> wordsCounts = new List<uint> ();*/
			List<TSString> wordsCounts = new List<TSString> ();
			for (prgr = 0; prgr < words2.Count; prgr++)
				{
				for (sntn = 0; sntn < words2[prgr].Count; sntn++)
					{
					for (wrd = 0; wrd < words2[prgr][sntn].Count; wrd++)
						{
						TSString wd = new TSString (words2[prgr][sntn][wrd], false);
						int idx = wordsCounts.IndexOf (wd);

						if (idx < 0)
							wordsCounts.Add (wd);
						else
							wordsCounts[idx].Increase ();
						}
					/*{
					uint length = (uint)words[i][j].Length;
					int idx = wordsLengths.IndexOf (length);
					if (idx < 0)
						{
						wordsLengths.Add (length);
						wordsCounts.Add (1);
						}
					else
						{
						wordsCounts[idx]++;
						}
					}*/
					}
				}

			// 3. Сортировка
			wordsCounts.Sort ();
			/*do
				{
				sorted = true;
				for (int i = 0; i < wordsCounts.Count - 1; i++)
					{
					// Сортировка по сумме больших и маленьких букв
					if (wordsCounts[i] < wordsCounts[i + 1])
						{
						uint c = wordsCounts[i];
						wordsCounts[i] = wordsCounts[i + 1];
						wordsCounts[i + 1] = c;

						c = wordsLengths[i];
						wordsLengths[i] = wordsLengths[i + 1];
						wordsLengths[i + 1] = c;

						sorted = false;
						}
					}
				} while (!sorted);*/

			// 4. Отображение
			stats[stWords] += BuildMinMaxMid (wordsCounts, stWords, gnWords);

			#endregion

			#region Предложения в символах

			// 1. Дробление на предложения
			stats.Add ("");

			List<string> sentences = new List<string> (Text.Split (sentenceSplitters2,
				StringSplitOptions.RemoveEmptyEntries));

			// 2. Расчёт длин по числу букв
			/*List<uint> sentencesLengths = new List<uint> ();
			List<uint> sentencesCounts = new List<uint> ();*/
			List<TSString> sentencesCounts = new List<TSString> ();
			for (int i = 0; i < sentences.Count; i++)
				{
				uint length = (uint)sentences[i].Trim ().Length;
				if (length == 0)
					{
					// На случай пробелов в начале и конце предложения
					sentences.RemoveAt (i);
					i--;
					continue;
					}

				TSString sn = new TSString (length, true);
				int idx = sentencesCounts.IndexOf (sn);
				if (idx < 0)
					sentencesCounts.Add (sn);
				/*{
				sentencesLengths.Add (length);
				sentencesCounts.Add (1);
				}*/
				else
					sentencesCounts[idx].Increase ();
				/*{
				sentencesCounts[idx]++;
				}*/
				}

			// 3. Сортировка
			sentencesCounts.Sort ();
			/*do
				{
				sorted = true;
				for (int i = 0; i < sentencesLengths.Count - 1; i++)
					{
					// Сортировка по сумме больших и маленьких букв
					if (sentencesLengths[i] < sentencesLengths[i + 1])
						{
						uint c = sentencesCounts[i];
						sentencesCounts[i] = sentencesCounts[i + 1];
						sentencesCounts[i + 1] = c;

						c = sentencesLengths[i];
						sentencesLengths[i] = sentencesLengths[i + 1];
						sentencesLengths[i + 1] = c;

						sorted = false;
						}
					}
				} while (!sorted);*/

			// 4. Отображение
			stats[stSentencesChars] += BuildMinMaxMid (sentencesCounts, stSentencesChars, gnSentences);
			/*midLength = 0.0;
			maxLength = 0;
			minLength = 0;

			for (int i = 0; i < sentencesCounts.Count; i++)
				{
				stats[stSentencesChars] += string.Format (GetCaption (stSentencesChars, 0),
					sentencesCounts[i].Length.ToString ().PadLeft (3),
					sentencesCounts[i].Quantity.ToString ().PadLeft (3)) + RDLocale.RN;

				midLength += 1.0 * sentencesCounts[i].Length * sentencesCounts[i].Quantity;
				if (i == 0)
					{
					maxLength = minLength = sentencesCounts[i].Length;
					}
				else
					{
					if (maxLength < sentencesCounts[i].Length)
						maxLength = sentencesCounts[i].Length;
					if (minLength > sentencesCounts[i].Length)
						minLength = sentencesCounts[i].Length;
					}
				}

			midLength /= sentences.Count;
			stats[stSentencesChars] += GetCaption (gnSentences) + sentences.Count.ToString () + RDLocale.RN;
			stats[stSentencesChars] += GetCaption (gnMinimum) + minLength.ToString () + RDLocale.RN;
			stats[stSentencesChars] += GetCaption (gnMaximum) + maxLength.ToString () + RDLocale.RN;
			stats[stSentencesChars] += GetCaption (gnAverage) + midLength.ToString (numbersFormat);*/

			#endregion

			#region Предложения в словах

			// 1. Расчёт длин по числу слов
			/*sentencesLengths.Clear ();
			sentencesCounts.Clear ();*/
			sentencesCounts.Clear ();
			stats.Add ("");

			/*for (int i = 0; i < words2.Count; i++)
				{
				uint length = (uint)words2[i].Count;
				int idx = sentencesLengths.IndexOf (length);
				if (idx < 0)
					{
					sentencesLengths.Add (length);
					sentencesCounts.Add (1);
					}
				else
					{
					sentencesCounts[idx]++;
					}
				}*/
			for (prgr = 0; prgr < words2.Count; prgr++)
				{
				for (sntn = 0; sntn < words2[prgr].Count; sntn++)
					{
					TSString sn = new TSString ((uint)words2[prgr][sntn].Count, false);
					int idx = sentencesCounts.IndexOf (sn);
					if (idx < 0)
						sentencesCounts.Add (sn);
					else
						sentencesCounts[idx].Increase ();
					}
				}

			// 2. Сортировка
			sentencesCounts.Sort ();
			/*do
				{
				sorted = true;
				for (int i = 0; i < sentencesCounts.Count - 1; i++)
					{
					// Сортировка по сумме больших и маленьких букв
					if (sentencesCounts[i] < sentencesCounts[i + 1])
						{
						uint c = sentencesCounts[i];
						sentencesCounts[i] = sentencesCounts[i + 1];
						sentencesCounts[i + 1] = c;

						c = sentencesLengths[i];
						sentencesLengths[i] = sentencesLengths[i + 1];
						sentencesLengths[i + 1] = c;

						sorted = false;
						}
					}
				} while (!sorted);*/

			// 3. Отображение
			stats[stSentencesWords] += BuildMinMaxMid (sentencesCounts, stSentencesWords, gnSentences);

			/*midLength = 0.0;
			maxLength = 0;
			minLength = 0;

			for (int i = 0; i < sentencesCounts.Count; i++)
				{
				stats[stSentencesWords] += string.Format (GetCaption (stSentencesWords, 0),
					sentencesCounts[i].Length.ToString ().PadLeft (2),
					sentencesCounts[i].Quantity.ToString ().PadLeft (3)) + RDLocale.RN;

				midLength += 1.0 * sentencesCounts[i].Length * sentencesCounts[i].Quantity;
				if (i == 0)
					{
					maxLength = minLength = sentencesCounts[i].Length;
					}
				else
					{
					if (maxLength < sentencesCounts[i].Length)
						maxLength = sentencesCounts[i].Length;
					if (minLength > sentencesCounts[i].Length)
						minLength = sentencesCounts[i].Length;
					}
				}

			midLength /= sentences.Count;
			stats[stSentencesWords] += GetCaption (gnSentences) + sentences.Count.ToString () + RDLocale.RN;
			stats[stSentencesWords] += GetCaption (gnMinimum) + minLength.ToString () + RDLocale.RN;
			stats[stSentencesWords] += GetCaption (gnMaximum) + maxLength.ToString () + RDLocale.RN;
			stats[stSentencesWords] += GetCaption (gnAverage) + midLength.ToString (numbersFormat);*/

			#endregion

			#region Абзацы в символах

			// 1. Дробление на абзацы
			stats.Add ("");

			List<string> paragraphs = new List<string> (Text.Split (paragraphSplitters2,
				StringSplitOptions.RemoveEmptyEntries));

			// 2. Расчёт длин по числу букв
			List<TSString> paragraphsCounts = new List<TSString> ();
			for (int i = 0; i < paragraphs.Count; i++)
				{
				uint length = (uint)paragraphs[i].Trim ().Length;
				if (length == 0)
					{
					// На случай пробелов в начале и конце предложения
					paragraphs.RemoveAt (i);
					i--;
					continue;
					}

				TSString sn = new TSString (length, true);
				int idx = paragraphsCounts.IndexOf (sn);
				if (idx < 0)
					paragraphsCounts.Add (sn);
				else
					paragraphsCounts[idx].Increase ();
				}

			// 3. Сортировка
			paragraphsCounts.Sort ();

			// 4. Отображение
			stats[stParagraphsChars] += BuildMinMaxMid (paragraphsCounts, stParagraphsChars, gnParagraphs);

			/*midLength = 0.0;
			maxLength = 0;
			minLength = 0;

			for (int i = 0; i < paragraphsCounts.Count; i++)
				{
				stats[stParagraphsChars] += string.Format (GetCaption (stParagraphsChars, 0),
					paragraphsCounts[i].Length.ToString ().PadLeft (3),
					paragraphsCounts[i].Quantity.ToString ().PadLeft (3)) + RDLocale.RN;

				midLength += 1.0 * paragraphsCounts[i].Length * paragraphsCounts[i].Quantity;
				if (i == 0)
					{
					maxLength = minLength = paragraphsCounts[i].Length;
					}
				else
					{
					if (maxLength < paragraphsCounts[i].Length)
						maxLength = paragraphsCounts[i].Length;
					if (minLength > paragraphsCounts[i].Length)
						minLength = paragraphsCounts[i].Length;
					}
				}

			midLength /= paragraphs.Count;
			stats[stParagraphsChars] += GetCaption (gnParagraphs) + paragraphs.Count.ToString () + RDLocale.RN;
			stats[stParagraphsChars] += GetCaption (gnMinimum) + minLength.ToString () + RDLocale.RN;
			stats[stParagraphsChars] += GetCaption (gnMaximum) + maxLength.ToString () + RDLocale.RN;
			stats[stParagraphsChars] += GetCaption (gnAverage) + midLength.ToString (numbersFormat);*/

			#endregion

			#region Абзацы в словах

			// 1. Расчёт длин по числу слов
			paragraphsCounts.Clear ();
			stats.Add ("");

			for (prgr = 0; prgr < words2.Count; prgr++)
				{
				uint q = 0;
				for (sntn = 0; sntn < words2[prgr].Count; sntn++)
					q += (uint)words2[prgr][sntn].Count;

				TSString sn = new TSString (q, false);
				int idx = paragraphsCounts.IndexOf (sn);
				if (idx < 0)
					paragraphsCounts.Add (sn);
				else
					paragraphsCounts[idx].Increase ();
				}

			// 2. Сортировка
			paragraphsCounts.Sort ();

			// 3. Отображение
			stats[stParagraphsWords] += BuildMinMaxMid (paragraphsCounts, stParagraphsWords, gnParagraphs);

			/*midLength = 0.0;
			maxLength = 0;
			minLength = 0;

			for (int i = 0; i < paragraphsCounts.Count; i++)
				{
				stats[stParagraphsWords] += string.Format (GetCaption (stParagraphsWords, 0),
					paragraphsCounts[i].Length.ToString ().PadLeft (2),
					paragraphsCounts[i].Quantity.ToString ().PadLeft (3)) + RDLocale.RN;

				midLength += 1.0 * paragraphsCounts[i].Length * paragraphsCounts[i].Quantity;
				if (i == 0)
					{
					maxLength = minLength = paragraphsCounts[i].Length;
					}
				else
					{
					if (maxLength < paragraphsCounts[i].Length)
						maxLength = paragraphsCounts[i].Length;
					if (minLength > paragraphsCounts[i].Length)
						minLength = paragraphsCounts[i].Length;
					}
				}

			midLength /= paragraphs.Count;
			stats[stParagraphsWords] += GetCaption (gnParagraphs) + paragraphs.Count.ToString () + RDLocale.RN;
			stats[stParagraphsWords] += GetCaption (gnMinimum) + minLength.ToString () + RDLocale.RN;
			stats[stParagraphsWords] += GetCaption (gnMaximum) + maxLength.ToString () + RDLocale.RN;
			stats[stParagraphsWords] += GetCaption (gnAverage) + midLength.ToString (numbersFormat);*/

			#endregion

			#region Абзацы в предложениях

			// 1. Расчёт длин по числу слов
			paragraphsCounts.Clear ();
			stats.Add ("");

			for (prgr = 0; prgr < words2.Count; prgr++)
				{
				TSString sn = new TSString ((uint)words2[prgr].Count, false);
				int idx = paragraphsCounts.IndexOf (sn);
				if (idx < 0)
					paragraphsCounts.Add (sn);
				else
					paragraphsCounts[idx].Increase ();
				}

			// 2. Сортировка
			paragraphsCounts.Sort ();

			// 3. Отображение
			stats[stParagraphsSentences] += BuildMinMaxMid (paragraphsCounts, stParagraphsSentences, gnParagraphs);

			/*midLength = 0.0;
			maxLength = 0;
			minLength = 0;

			for (int i = 0; i < paragraphsCounts.Count; i++)
				{
				stats[stParagraphsSentences] += string.Format (GetCaption (stParagraphsSentences, 0),
					paragraphsCounts[i].Length.ToString ().PadLeft (2),
					paragraphsCounts[i].Quantity.ToString ().PadLeft (3)) + RDLocale.RN;

				midLength += 1.0 * paragraphsCounts[i].Length * paragraphsCounts[i].Quantity;
				if (i == 0)
					{
					maxLength = minLength = paragraphsCounts[i].Length;
					}
				else
					{
					if (maxLength < paragraphsCounts[i].Length)
						maxLength = paragraphsCounts[i].Length;
					if (minLength > paragraphsCounts[i].Length)
						minLength = paragraphsCounts[i].Length;
					}
				}

			midLength /= paragraphs.Count;
			stats[stParagraphsSentences] += GetCaption (gnParagraphs) + paragraphs.Count.ToString () + RDLocale.RN;
			stats[stParagraphsSentences] += GetCaption (gnMinimum) + minLength.ToString () + RDLocale.RN;
			stats[stParagraphsSentences] += GetCaption (gnMaximum) + maxLength.ToString () + RDLocale.RN;
			stats[stParagraphsSentences] += GetCaption (gnAverage) + midLength.ToString (numbersFormat);*/

			#endregion

			// Завершено
			return stats.ToArray ();
			}

		// Метод формирует таблицу и сводные строки для указанных массивов счётчиков
		private static string BuildMinMaxMid (List<TSString> Counts, int CaptionsIndex, int GenericIndex)
			{
			double midLength = 0.0;
			uint maxLength = 0;
			uint minLength = 0;
			uint total = 0;
			string res = "";

			for (int i = 0; i < Counts.Count; i++)
				{
				res += string.Format (GetCaption (CaptionsIndex, 0),
					Counts[i].Length.ToString ().PadLeft (3),
					Counts[i].Quantity.ToString ().PadLeft (4)) + RDLocale.RN;

				midLength += 1.0 * Counts[i].Length * Counts[i].Quantity;
				if (i == 0)
					{
					maxLength = minLength = Counts[i].Length;
					}
				else
					{
					if (maxLength < Counts[i].Length)
						maxLength = Counts[i].Length;
					if (minLength > Counts[i].Length)
						minLength = Counts[i].Length;
					}
				total += Counts[i].Quantity;
				}

			if (total != 0)
				midLength /= total;
			res += GetCaption (GenericIndex) + total.ToString () + RDLocale.RN;
			res += GetCaption (gnMinimum) + minLength.ToString () + RDLocale.RN;
			res += GetCaption (gnMaximum) + maxLength.ToString () + RDLocale.RN;
			res += GetCaption (gnAverage) + midLength.ToString (numbersFormat);

			return res;
			}

		private const string supportedLetters =
			"ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
			"АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
			"abcdefghijklmnopqrstuvwxyz" +
			"абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
		private const string supportedDigits = "0123456789";
		private const string supportedSubletters = "-'’";

		private static char[] sentenceSplitters2 = new char[] { '\n', '\r', '.', '?', '!', '…' };
		private static char[] paragraphSplitters2 = new char[] { '\n', '\r' };

		private const string percentageFormat = "0.0#";
		private const string numbersFormat = "0.0";
		}
	}
