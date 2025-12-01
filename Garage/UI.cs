// using System.Collections;
using Garage.Helpers;

namespace Garage;


internal partial class UI : IUI {

	public int AddGarage() {
		return UI.PromptValidInt($"{Style.PromptText} Enter garage size: {Style.Reset}", -1, 200, new Dictionary<char, int> { { 'q', -1 }, { ' ', 0 }, });
	}

	public int SelectGarage(List<Garage<Vehicle>> garages) {
		for (int i = 0; i < garages.Count; i++) {
			var _garage = garages[i];
			Console.WriteLine($"{i + 1}. Garage space: {_garage.Length} / {_garage.Capacity}");
		}
		return UI.PromptValidInt($"{Style.PromptText} Pick garage number: {Style.Reset}", 0, garages.Count, new Dictionary<char, int> { { 'q', 0 }, }) - 1;
	}

	public void Print(Vehicle v) {
		Console.Write($" {v.GetType().Name,-10}  {v.LicencePlate,-6}  {v.Color,-6}  {Style.Key} Wheels {Style.Reset}{Style.Value} {v.Wheels} {Style.Reset} ");
		foreach (var property in v.GetType().GetProperties().Where(p => p.Name != "LicencePlate" && p.Name != "Color" && p.Name != "Wheels")) {
			if (property.Name == "Item") continue;
			var propertyValue = property.GetValue(v);
			if (propertyValue == null) continue;
			Console.Write($" {Style.Key} {property.Name} {Style.Reset}{Style.Value} {propertyValue} {Style.Reset} ");
		}
		Console.WriteLine();
	}

	public void Print(GarageHandler _handler) {
		foreach (var v in _handler.CurrentGarage) Print(v);
	}
	public void Print(GarageHandler _handler, Action<Vehicle> action) {
		foreach (var v in _handler.CurrentGarage) action?.Invoke(v);
	}

	public void PrintVehicleTypeCount(GarageHandler _handler) {
		Dictionary<string, int> dict = new Dictionary<string, int>();
		foreach (var v in _handler.CurrentGarage) {
			var type = v.GetType().Name;
			if (!dict.ContainsKey(type)) dict[type] = 1;
			else dict[type]++;
		}
		foreach (var (key, value) in dict) {
			Console.WriteLine($"{value} {key}");
		}
	}

	public void PromptSearchLicencePlate(GarageHandler _handler) {
		string licencePlate = UI.PromptString($"{Style.PromptText} LicencePlate:{Style.Reset} ").ToLower();
		if (licencePlate == "") return;
		try {
			Vehicle vehicle = _handler.GetEnumerator().Where(v => v.LicencePlate?.ToLower() == licencePlate).Last();
			Print(vehicle);
		} catch {
			Console.WriteLine($"No vehicle with licence plate {licencePlate} found");
		}
	}

	public void PromptSearch(GarageHandler _handler) {
		string vehicleType = UI.PromptString($"{Style.PromptText} Vehicle type ({Stuff.VehicleTypesCsv}):{Style.Reset} ", str => str == "" || Enum.IsDefined(typeof(VehicleTypes), str.ToLower()));
		IEnumerable<Vehicle> q = _handler.GetEnumerator();
		if (vehicleType != "") q = q.Where(v => vehicleType.ToLower() == v.GetType().ToString().Split('.').Last().ToLower());
		if (vehicleType == "") vehicleType = "Vehicle";
		var properties = Type.GetType(typeof(Vehicle).Namespace + "." + vehicleType.Capitalize())?.GetProperties();
		if (properties != null) {
			foreach (var property in properties.Where(p => p.Name != "Item")) {
				string val_ = UI.PromptString($"{property.Name}: ");
				if (val_ == "") continue;
				var type = property.PropertyType.ToString();
				if (type.Contains("Int32")) {
					if (Int32.TryParse(val_, out int val)) q = q.Where(v => val == v[property.Name] as int?);
				} else if (type.Contains("Boolean")) {
					if (Boolean.TryParse(val_, out bool val)) q = q.Where(v => v.IsPropEqual(property.Name, val));
				} else {
					q = q.Where(v => val_ == v[property.Name] as string);
				}
			}
		}
		Console.WriteLine("\nMatches:");
		foreach (var v in q) Print(v);

	}

	public string PromptRemoveVehicle(GarageHandler _handler) {
		foreach (var v in _handler.CurrentGarage) Print(v);
		string licencePlace = UI.PromptString("Enter Licenceplate of vehicle to remove: ", str => _handler.GetVehicle(str) != null);
		return licencePlace;
	}

	public Vehicle? PromptVehicle(GarageHandler _handler) {
		string vehicleType = UI.PromptString($"{Style.PromptText} Vehicle type ({Stuff.VehicleTypesCsv}):{Style.Reset} ", str => Enum.IsDefined(typeof(VehicleTypes), str.ToLower()));
		string licencePlate = UI.PromptString($"{Style.PromptText} licencePlate:{Style.Reset} ", str => _handler.GetVehicle(str) == null);
		string color = UI.PromptString($"{Style.PromptText} color:{Style.Reset} ");
		Vehicle? vehicle = vehicleType.ToLower() switch {
			"car" => new Car(licencePlate, color),
			"motorcycle" => new Motorcycle(licencePlate, color),
			"airplane" => new Airplane(licencePlate, color),
			"bus" => new Bus(licencePlate, color),
			"boat" => new Boat(licencePlate, color),
			_ => null,
		};
		if (vehicle == null) return vehicle;
		var properties = Type.GetType(typeof(Vehicle).Namespace + "." + vehicleType.Capitalize())?.GetProperties();
		if (properties != null) {
			foreach (var property in properties.Where(p => p.CanWrite && p.Name != "Item")) {
				if (property.Name == "LicencePlate") continue;
				if (property.Name == "Color") continue;
				string val_ = UI.PromptString($"{Style.PromptText} {property.Name}:{Style.Reset} ");
				if (val_ == "") continue;
				var type = property.PropertyType.ToString();
				if (type.Contains("Int32")) {
					if (Int32.TryParse(val_, out int val)) vehicle[property.Name] = val;
				} else if (type.Contains("Boolean")) {
					if (Boolean.TryParse(val_, out bool val)) vehicle[property.Name] = val;
				} else {
					vehicle[property.Name] = val_;
				}
			}
		}
		return vehicle;
	}

}

internal partial class UI {

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
				if (charMap != null && str.Length <= 1) {
					char chr = str.Length == 0 ? ' ' : str[0];
					if (charMap.ContainsKey(chr)) return charMap[chr];
				}
				if (int.TryParse(str, out int result) && result >= min && result <= max) {
					return result;
				}
			}
			Console.Write(Messages.TryAgainMessage);
		}
	}

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
	}

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


	public static int PromptAction(string str, Dictionary<string, Action> actionMap, string exitString = "Exit") {
		Console.Write(str);
		return PromptAction(actionMap, exitString);
	}
	public static int PromptAction(Dictionary<string, Action> actionMap, string exitString = "Exit") {
		int i = 0;
		foreach (var (key, value) in actionMap) {
			Console.WriteLine($"{i + 1}. {key}");
			i++;
		}
		Console.WriteLine($"0. {exitString}");
		int ans = UI.PromptValidInt($"", 0, actionMap.Count, new Dictionary<char, int> { { 'q', 0 }, });
		return ans;
	}

}


