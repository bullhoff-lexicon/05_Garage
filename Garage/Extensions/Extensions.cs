// using Garage.Helpers;
// using System.Collections;
// using System.Linq;

namespace Garage;


public static class StringExtensions {

	public static string Capitalize(this string input) {
		if (input == null || input.Length == 0) return "";
		return input[0].ToString().ToUpper() + input.Substring(1);
	}

}

internal static class Extensions {

	public static string GetClassName<T>(this T inst) {
		if (inst == null) return typeof(T).ToString().Split('.').Last();
		return inst.GetType().ToString().Split('.').Last();
	}

	public static bool IsPropEqual<T>(this T inst, string key, bool val) {
		bool? x = inst?.GetType().GetProperty(key)?.GetValue(inst, null) as bool?;
		if (x == null) x = false;
		return x == val;
	}
	public static bool IsPropEqual<T>(this T inst, string key, int val) {
		int? x = inst?.GetType().GetProperty(key)?.GetValue(inst, null) as int?;
		return x == val;
	}
	public static bool IsPropEqual<T>(this T inst, string key, string val) {
		string? x = inst?.GetType().GetProperty(key)?.GetValue(inst, null) as string;
		return x == val;
	}

}


