
namespace Garage.Helpers {
	public static class Style {

		public static string Reset = "\x1b[0m";
		public static string Italic = "\x1b[3m";
		public static string Bold = "\x1b[1m";
		public static string Invert = "\x1b[7m";
		public static string Strike = "\x1b[9m";
		public static string RgbBg(int r, int g, int b) { return $"\x1b[48;2;{r};{g};{b}m"; }
		public static string RgbFg(int r, int g, int b) { return $"\x1b[38;2;{r};{g};{b}m"; }

		public static string PromptText = $"{Style.RgbBg(55, 55, 0)}{Style.RgbFg(210, 210, 210)}";
		public static string Grayed = $"{Style.RgbBg(55, 55, 0)}{Style.RgbFg(210, 210, 210)}";
		public static string Placeholder = $"{Style.Italic}{Style.RgbFg(155, 155, 155)}";
		public const string PromptChar = "\x1b[48;2;255;255;0m\x1b[38;2;0;0;0m";

		public static string Key = $"{Style.Italic}{Style.RgbFg(155, 155, 155)}{Style.RgbBg(65, 65, 65)}";
		public static string Value = $"";



		public static string I { get { return Italic; } }
		public static string B { get { return Bold; } }

		// public const string PromptInstruction = Italic + Bold;

		public static string ReportPos = "\x1b[6n";
		public static string SavePos = "\x1b[s";
		public static string RestorePos = "\x1b[u";
		public static string ClearToRight = "\x1b[K";
		public static string MoveCursorUp1 = "\x1b[1A";
		public static string MoveCursorLeft1 = "\x1b[1D";
		public static Func<int, string> MoveCursorUp = (int rows) => $"\x1b[{rows}A";
		public static Func<int, string> MoveCursorLeft = (int cols) => $"\x1b[{cols}D";
		public static Func<int, string> MoveCursorRight = (int cols) => $"\x1b[{cols}C";
		public static Func<int, string> MoveCursorDown = (int cols) => $"\x1b[{cols}B";


		public static string WrappingDisable = "\x1b[?7l";
		public static string WrappingEnable = "\x1b[?7h";
		public static string CursirHide = "\x1b[?25l";
		public static string CursirShow = "\x1b[?25h";
		public static string MoveCursorToEnd = "\x1b[9999999;1H";

	}
}
