// using System.Collections;
using Garage.Helpers;

namespace Garage;


enum VehicleTypes {
	car, motorcycle, airplane, bus, boat,
};

internal struct Stuff {
	public static string VehicleTypesCsv = string.Join(",", Enum.GetValues(typeof(VehicleTypes)).Cast<VehicleTypes>().Select(v => v.ToString()));
}

internal class Main {
	static List<Manager> garages = new List<Manager>();
	public void Run() {
		int ans = Util.PromptValidInt($"{Style.PromptText} Enter garage size: {Style.Reset}", 0, 200, new Dictionary<char, int> { { 'q', 0 }, });
		if (ans == 0) return;
		garages.Add(new Manager(ans));
		garages.Last().ShowMainMenu();
	}
	// static Dictionary<string, Action> actionMap = new Dictionary<string, Action>{
	// 	{ "Add garage", ()=> { PromptNew();  } },
	// 	{ "Select garage", ()=> { PromptSelect();  } },
	// };
	// static void ShowMainMenu() {
	// 	while (true) {
	// 		int ans = UI.PromptAction(actionMap);
	// 		if (ans == 0) break;
	// 		var (_, func) = actionMap.ElementAt(ans - 1);
	// 		func();
	// 	}
	// }
	// static void PromptNew() {
	// 	int ans = Util.PromptValidInt($"{Style.PromptText} Enter garage size: {Style.Reset}", 0, 200, new Dictionary<char, int> { { 'q', 0 }, });
	// 	if (ans == 0) return;
	// 	garages.Add(new Manager(ans));
	// }
	// static void PromptSelect() {
	// 	int ans = Util.PromptValidInt($"{Style.PromptText} Pick action number: {Style.Reset}", 0, 200, new Dictionary<char, int> { { 'q', 0 }, });
	// 	if (ans == 0) return;
	// 	garages.Add(new Manager(ans));
	// }
}

internal class Manager {
	// private GarageHandler garage;
	private Garage<Vehicle> garage;
	private Dictionary<string, Action> actionMap;

	public Manager(int size = 50) {
		// garage = new GarageHandler(new Garage<Vehicle>(size));
		garage = new Garage<Vehicle>(size);
		actionMap = new Dictionary<string, Action>{
			{ "Populate garage", ()=> { PopulateGarage(); UI.PrintGarage(garage); } },
			{ "Add vehicle", ()=> PromptVehicle() },
			{ "Remove vehicle", ()=> PromptRemoveVehicle() },
			{ "Search", ()=> PromptSearch() },
			{ "Search Registration plate", ()=> PromptSearch() },
			{ "Print garage", ()=> UI.PrintGarage(garage) },
			{ "Print vehicle type count", ()=> UI.PrintVehicleTypeCount(garage) },
		};
	}

	public void ShowMainMenu() {
		while (true) {
			int ans = UI.PromptAction(actionMap);
			if (ans == 0) break;
			var (_, func) = actionMap.ElementAt(ans - 1);
			func();
		}
	}

	public void PopulateGarage() {
		AddVehicle(new Car("abc123", "black"));
		AddVehicle(new Car("abc123", "red"));
		AddVehicle(new Car("abc124", "red"));
		AddVehicle(new Car("abc125", "green", brand: "Range Rover"));
		AddVehicle(new Motorcycle("def123", "red"));
		AddVehicle(new Motorcycle("def124", "teal"));
		AddVehicle(new Motorcycle("def125", "black"));
		AddVehicle(new Airplane("ghi123", "black", engines: 1));
		AddVehicle(new Airplane("ghi124", "black", 8));
		AddVehicle(new Bus("jkl125", "black"));
		AddVehicle(new Bus("jkl126", "black", 4));
		AddVehicle(new Boat("mno125", "black"));
	}

	public Vehicle? AddVehicle(Vehicle? vehicle = null) {
		if (vehicle == null) return null;
		int index = garage.FindIndex(v => v.LicencePlate == vehicle.LicencePlate);
		if (index == -1) {
			bool res = garage.Add(vehicle);
			if (!res) Console.WriteLine($"Garage is full, couldnt add {vehicle.LicencePlate}");
		} else {
			Console.WriteLine($"{vehicle.LicencePlate} already exists");
		}
		return vehicle;
	}

	public void PromptSearch() {
		string vehicleType = Util.PromptString($"{Style.PromptText} Vehicle type ({Stuff.VehicleTypesCsv}):{Style.Reset} ", str => str == "" || Enum.IsDefined(typeof(VehicleTypes), str.ToLower()));
		IEnumerable<Vehicle> q = garage;
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
		foreach (var v in q) {
			UI.PrintVehicle(v);
		}
	}

	public void PromptRemoveVehicle() {
		foreach (var v in garage) {
			var type = v.GetType().ToString().Split('.').Last();
			Console.WriteLine($"{type,-10} {v.LicencePlate,-6}  {v.Color,-6}  {v.Wheels} ");
		}
		string licencePlace = Util.PromptString("Enter Licenceplate of vehicle to remove: ", str => garage.GetVehicle(str) != null);
		int index = garage.FindIndex(v => v.LicencePlate == licencePlace);
		garage.Remove(index);
	}

	public void PromptVehicle() {
		string vehicleType = Util.PromptString($"{Style.PromptText} Vehicle type ({Stuff.VehicleTypesCsv}):{Style.Reset} ", str => Enum.IsDefined(typeof(VehicleTypes), str.ToLower()));
		string licencePlate = Util.PromptString($"{Style.PromptText} licencePlate:{Style.Reset} ", str => garage.GetVehicle(str) == null);
		string color = Util.PromptString($"{Style.PromptText} color:{Style.Reset} ");
		Vehicle? vehicle = vehicleType.ToLower() switch {
			"car" => AddVehicle(new Car(licencePlate, color)),
			"motorcycle" => AddVehicle(new Motorcycle(licencePlate, color)),
			"airplane" => AddVehicle(new Airplane(licencePlate, color)),
			"bus" => AddVehicle(new Bus(licencePlate, color)),
			"boat" => AddVehicle(new Boat(licencePlate, color)),
			_ => null,
		};
		if (vehicle == null) return;
		var properties = Type.GetType(typeof(Vehicle).Namespace + "." + vehicleType.Capitalize())?.GetProperties();
		if (properties != null) {
			foreach (var property in properties.Where(p => p.Name != "Item")) {
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
	}
}


