
namespace Garage.Helpers {
	public static class Util {


		private static char getChar() {
			Console.Write($"{Style.PromptChar} ");
			char chr = Console.ReadKey().KeyChar;
			Console.WriteLine($" {Style.Reset}");
			return chr;
		}
		private static string getString() {
			// Console.Write(Style.SavePos + " ");
			string str = Console.ReadLine() ?? string.Empty;
			// // fix tmux issue where unreseted bg leaks to all empty cols on next row
			// Console.WriteLine(Style.RestorePos + Style.MoveCursorUp(1) + Style.PromptChar + " " + str + " " + Style.Reset);
			return str;
		}


		public static int PromptValidInt(string prompt = "", int min = int.MinValue, int max = int.MaxValue, Dictionary<char, int>? charMap = null) {
			while (true) {
				Console.Write(prompt);
				if (min >= 0 && max < 10) {
					char chr = getChar();
					if (charMap != null && charMap.ContainsKey(chr)) return charMap[chr];
					if (int.TryParse(chr.ToString(), out int result) && result >= min && result <= max) return result;
				} else {
					string str = Console.ReadLine() ?? string.Empty;
					if (charMap != null && str.Length<=1) {
						char chr = str.Length==0? ' ' : str[0];
						if(charMap.ContainsKey(chr)) return charMap[chr];
					}
					if (int.TryParse(str, out int result) && result >= min && result <= max) {
						return result;
					}
				}
				Console.Write(Messages.TryAgainMessage);
			}
		}
		// public static int PromptValidInt(string[] prompt, int min = int.MinValue, int max = int.MaxValue, Dictionary<char, int>? charMap = null) {
		// 	int result;
		// 	while (true) {
		// 		for (int i = 0; i < prompt.Length; i++) Console.WriteLine(prompt[i]);
		// 		if (min >= 0 && max < 10) {
		// 			char chr = getChar();
		// 			if (charMap != null && charMap.ContainsKey(chr)) return charMap[chr];
		// 			if (int.TryParse(chr.ToString(), out result) && result >= min && result <= max) return result;
		// 		} else if (int.TryParse(Console.ReadLine(), out result) && result >= min && result <= max) {
		// 			return result;
		// 		}
		// 		Console.Write(Messages.TryAgainMessage);
		// 	}
		// }

		public static int PromptInt(string prompt = "") {
			int result;
			while (true) {
				Console.Write(prompt);
				if (int.TryParse(Console.ReadLine(), out result)) return result;
			}
		}



		public static string PromptString(string prompt = "") {
			Console.Write(prompt);
			return getString();
			// return Console.ReadLine() ?? string.Empty;
		}
		// public static string PromptString(string[] prompt) {
		// 	for (int i = 0; i < prompt.Length; i++) Console.WriteLine(prompt[i]);
		// 	return Console.ReadLine() ?? string.Empty;
		// }

		public static string PromptString(string prompt, Func<string, bool> func) {
			while (true) {
				Console.Write(prompt);
				string str = getString();
				if (func(str)) return str;
				Console.Write(Messages.TryAgainMessage);
			}
		}



		public static char PromptChar(string prompt = "") {
			Console.Write(prompt);
			char chr = getChar();
			return char.ToLower(chr);
		}
		// public static char PromptChar(string[] prompt) {
		// 	for (int i = 0; i < prompt.Length; i++) Console.WriteLine(prompt[i]);
		// 	char chr = getChar();
		// 	return char.ToLower(chr);
		// }


		public static int PromptAction(Dictionary<string, Action> actionMap, string exitString = "Exit") {
			// Console.WriteLine();
			int i = 0;
			foreach (var (key, value) in actionMap) {
				Console.WriteLine($"{i + 1}. {key}");
				i++;
			}
			Console.WriteLine($"0. {exitString}");
			int ans = Util.PromptValidInt($"", 0, actionMap.Count, new Dictionary<char, int> { { 'q', 0 }, });
			return ans;
		}




		public static void PrintList<T>(IEnumerable<T> theArr) {
			int i = 0;
			foreach (T value in theArr) {
				Console.WriteLine($" {Style.RgbBg(0, 0, 0)}{Style.RgbFg(0, 185, 0)}{i}. {Style.Reset}{value}");
				i++;
			}
			// System.Collections.Generic.List`1[System.String]   --> List[String]
			// System.Collections.Generic.Queue`1[System.String]  --> Queue[String]
			// System.Collections.Generic.Stack`1[System.String]  --> Stack[String]
			Console.Write($" Type:{Style.Invert} {System.Text.RegularExpressions.Regex.Replace(theArr.GetType().ToString(), @"(Collections.Generic.|`1|System.)", "")} {Style.Reset}");
			if (theArr is IReadOnlyCollection<T> readOnlyColl) Console.Write($" Count:{Style.Invert} {readOnlyColl.Count} {Style.Reset}");
			if (theArr is List<T> list) Console.Write($" Capacity:{Style.Invert} {list.Capacity} {Style.Reset}");
			Console.WriteLine();
		}



	}
}
