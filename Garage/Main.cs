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
	UI _ui = new UI();
	GarageHandler _garageHandler = new GarageHandler();

	private readonly Dictionary<string, Action> _actionsMainMenu;
	private readonly Dictionary<string, Action> _actionsGarage;

	public Main() {
		_actionsMainMenu = new Dictionary<string, Action>{
			{
				"Create garage, populate and print", ()=> {
					AddGarage(50);
					SelectGarage(-1);
					PopulateGarage();
					_ui.Print(_garageHandler);
					ShowGarageMenu();
				}
			},
			{
				"Create garage", ()=> {
					AddGarage();
					SelectGarage(-1);
					_ui.Print(_garageHandler);
					ShowGarageMenu();
				}
			},
			{
				"Select garage", ()=> {
					SelectGarage();
					_ui.Print(_garageHandler);
					ShowGarageMenu();
				}
			},
		};
		_actionsGarage = new Dictionary<string, Action>{
			{ "Populate garage", ()=> { PopulateGarage(); _ui.Print(_garageHandler); } },
			{ "Add vehicle", ()=> AddVehicle() },
			{ "Remove vehicle", ()=> RemoveVehicle() },
			{ "Search", ()=> Search() },
			{ "Search licence plate", ()=> SearchLicencePlate() },
			{ "Print garage", ()=> _ui.Print(_garageHandler) },
			{ "Print vehicle type count", ()=> _ui.PrintVehicleTypeCount(_garageHandler) },
		};
	}

	public void Run() {
		while (true) {
			int ans = UI.PromptAction(_actionsMainMenu);
			if (ans == 0) break;
			var (_, func) = _actionsMainMenu.ElementAt(ans - 1);
			func();
		}
	}


	void AddGarage(int? ans = null) {
		ans ??= _ui.AddGarage(); // prompts "Enter garage size:" and returns int within limit
		if (ans == -1) return;
		_garageHandler.AddGarage((int)ans);
	}
	void SelectGarage(int? ans = null) {
		ans ??= _ui.SelectGarage(_garageHandler.garages); // print garages (List<Garage>) and prompts "Pick garage number:" and returns int within limit
		_garageHandler.GarageSelected = (int)ans;
	}

	void ShowGarageMenu() {
		while (true) {
			int ans = UI.PromptAction($"\nGarageSelected: {_garageHandler.GarageSelected}\n", _actionsGarage);
			if (ans == 0) break;
			var (_, func) = _actionsGarage.ElementAt(ans - 1);
			func();
		}
	}
	void RemoveVehicle() {
		_ui.PromptRemoveVehicle(_garageHandler);
	}
	void AddVehicle() {
		Vehicle? vehicle = _ui.PromptVehicle(_garageHandler);
		if (vehicle == null) return;
		_garageHandler.AddVehicle(vehicle);
	}

	void SearchLicencePlate() {
		_ui.PromptSearchLicencePlate(_garageHandler);
	}
	void Search() {
		_ui.PromptSearch(_garageHandler);
	}

	private static readonly Random rnd = new Random();
	private static string RandomLicencePlate() {
		return new string(Enumerable.Repeat("0123456789abcdefghijklmnopqrstuvwxyz", 6).Select(s => s[rnd.Next(s.Length)]).ToArray());
	}
	public void PopulateGarage() {
		var _garage = _garageHandler;
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

}



