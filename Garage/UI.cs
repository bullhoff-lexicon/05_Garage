// using System.Collections;
using Garage.Helpers;

namespace Garage;


internal class UI : IUI {

	private readonly GarageHandler _garage;
	private readonly Dictionary<string, Action> _actionMap;

	public UI(int size = 50) {
		_garage = new GarageHandler(new Garage<Vehicle>(size));
		_actionMap = new Dictionary<string, Action>{
			{ "Populate garage", ()=> { PopulateGarage(); _garage.Print(); } },
			{ "Add vehicle", ()=> PromptVehicle() },
			{ "Remove vehicle", ()=> PromptRemoveVehicle() },
			{ "Search", ()=> PromptSearch() },
			{ "Search licence plate", ()=> PromptSearchLicencePlate() },
			{ "Print garage", ()=> _garage.Print() },
			{ "Print vehicle type count", ()=> _garage.PrintVehicleTypeCount() },
		};
	}

	public void Print() { _garage.Print(); }

	public void PrintGarageInfo() {
		Console.WriteLine($"Garage space: {_garage.Length} / {_garage.Capacity}");
	}

	public void ShowMainMenu() {
		while (true) {
			PrintGarageInfo();
			// Console.Write(Style.MoveCursorUp1);
			int ans = Util.PromptAction(_actionMap, "Return to main menu");
			if (ans == 0) break;
			var (_, func) = _actionMap.ElementAt(ans - 1);
			func();
		}
	}

	private static readonly Random rnd = new Random();
	private static string RandomLicencePlate(){
		return new string(Enumerable.Repeat("0123456789abcdefghijklmnopqrstuvwxyz", 6).Select(s => s[rnd.Next(s.Length)]).ToArray());
	}
	public void PopulateGarage() {
		_garage.AddVehicle(new Car("abc123", "black"));
		_garage.AddVehicle(new Car("abc123", "red"));
		_garage.AddVehicle(new Car("abc124", "red"));
		_garage.AddVehicle(new Car("abc125", "green", brand: "Range Rover"));
		_garage.AddVehicle(new Motorcycle("def123", "red"));
		_garage.AddVehicle(new Motorcycle("def124", "teal"));
		_garage.AddVehicle(new Motorcycle("def125", "black"));
		_garage.AddVehicle(new Airplane("ghi123", "black", engines: 1));
		_garage.AddVehicle(new Airplane("ghi124", "black", 8));
		_garage.AddVehicle(new Bus("jkl125", "black"));
		_garage.AddVehicle(new Bus("jkl126", "black", 4));
		_garage.AddVehicle(new Boat("mno125", "black"));

		_garage.AddVehicle(new Car(RandomLicencePlate(), "black"));
		_garage.AddVehicle(new Motorcycle(RandomLicencePlate(), "red"));
		_garage.AddVehicle(new Airplane(RandomLicencePlate(), "orange"));
		_garage.AddVehicle(new Bus(RandomLicencePlate(), "teal"));
		_garage.AddVehicle(new Boat(RandomLicencePlate(), "orange"));

	}

	public Vehicle? PromptSearchLicencePlate() {
		string licencePlate = Util.PromptString($"{Style.PromptText} LicencePlate:{Style.Reset} ").ToLower();
		if (licencePlate == "") return null;
		try {
			Vehicle vehicle = _garage.GetEnumerator().Where(v => v.LicencePlate?.ToLower() == licencePlate).Last();
			_garage.Print(vehicle);
			return vehicle;
		} catch {
			Console.WriteLine($"No vehicle with licence plate {licencePlate} found");
		}
		return null;
	}

	public void PromptSearch() {
		string vehicleType = Util.PromptString($"{Style.PromptText} Vehicle type ({Stuff.VehicleTypesCsv}):{Style.Reset} ", str => str == "" || Enum.IsDefined(typeof(VehicleTypes), str.ToLower()));
		IEnumerable<Vehicle> q = _garage.GetEnumerator();
		if (vehicleType != "") q = q.Where(v => vehicleType.ToLower() == v.GetType().ToString().Split('.').Last().ToLower());
		if (vehicleType == "") vehicleType = "Vehicle";
		var properties = Type.GetType(typeof(Vehicle).Namespace + "." + vehicleType.Capitalize())?.GetProperties();
		if (properties != null) {
			foreach (var property in properties.Where(p => p.Name != "Item")) {
				string val_ = Util.PromptString($"{property.Name}: ");
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
		foreach (var v in q) _garage.Print(v);

	}

	public void PromptRemoveVehicle() {
		_garage.Print();
		string licencePlace = Util.PromptString("Enter Licenceplate of vehicle to remove: ", str => _garage.GetVehicle(str) != null);
		_garage.Remove(licencePlace);
	}

	public void PromptVehicle() {
		string vehicleType = Util.PromptString($"{Style.PromptText} Vehicle type ({Stuff.VehicleTypesCsv}):{Style.Reset} ", str => Enum.IsDefined(typeof(VehicleTypes), str.ToLower()));
		string licencePlate = Util.PromptString($"{Style.PromptText} licencePlate:{Style.Reset} ", str => _garage.GetVehicle(str) == null);
		string color = Util.PromptString($"{Style.PromptText} color:{Style.Reset} ");
		Vehicle? vehicle = vehicleType.ToLower() switch {
			"car" => new Car(licencePlate, color),
			"motorcycle" => new Motorcycle(licencePlate, color),
			"airplane" => new Airplane(licencePlate, color),
			"bus" => new Bus(licencePlate, color),
			"boat" => new Boat(licencePlate, color),
			_ => null,
		};
		if (vehicle == null) return;
		var properties = Type.GetType(typeof(Vehicle).Namespace + "." + vehicleType.Capitalize())?.GetProperties();
		if (properties != null) {
			foreach (var property in properties.Where(p => p.CanWrite && p.Name != "Item")) {
				if (property.Name == "LicencePlate") continue;
				if (property.Name == "Color") continue;
				string val_ = Util.PromptString($"{Style.PromptText} {property.Name}:{Style.Reset} ");
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
		_garage.AddVehicle(vehicle);
	}

}
