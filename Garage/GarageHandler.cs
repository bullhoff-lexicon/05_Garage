using Garage.Helpers;

namespace Garage;


internal class GarageHandler : IGarageHandler {

	private readonly IGarage<Vehicle> _garage;
	public int Capacity => _garage.Capacity;
	public int Length => _garage.Length;
	public int FindIndex(Func<Vehicle, bool> func) { return _garage.FindIndex(func); }

	public GarageHandler(int size = 50) { _garage = new Garage<Vehicle>(size); }
	public GarageHandler(IGarage<Vehicle> garage) { _garage = garage; }

	public IEnumerable<Vehicle> GetEnumerator() {
		return (IEnumerable<Vehicle>)_garage;
	}

	public Vehicle? AddVehicle(Vehicle? vehicle = null) {
		if (vehicle == null) return null;
		int index = _garage.FindIndex(v => v.LicencePlate == vehicle.LicencePlate);
		if (index == -1) {
			bool res = _garage.Add(vehicle);
			if (!res) Console.WriteLine($"Garage is full, couldnt add {vehicle.LicencePlate}");
		} else {
			Console.WriteLine($"{vehicle.LicencePlate} already exists");
		}
		return vehicle;
	}

	public bool Remove(int i) { return _garage.Remove(i); }
	public bool Remove(string id) {
		int i = _garage.FindIndex(vehicle => vehicle != null && vehicle.LicencePlate == id);
		if (i < 0) return false;
		return _garage.Remove(i);
	}

	public Vehicle? this[string licencePlate] => GetVehicle(licencePlate);
	public Vehicle? GetVehicle(string licencePlate) {
		foreach (var v in _garage) {
			if (v.LicencePlate == licencePlate) return v;
		}
		return null;
	}


	public void Print() {
		foreach (var v in _garage) Print(v);
	}
	public void Print(Action<Vehicle> action) {
		foreach (var v in _garage) action?.Invoke(v);
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

	public void PrintVehicleTypeCount(IGarage<Vehicle>? garage = null) {
		garage ??= _garage;
		Dictionary<string, int> dict = new Dictionary<string, int>();
		foreach (var v in garage) {
			// var type = v.GetType().ToString().Split('.').Last();
			var type = v.GetType().Name;
			if (!dict.ContainsKey(type)) dict[type] = 1;
			else dict[type]++;
		}
		foreach (var (key, value) in dict) {
			Console.WriteLine($"{value} {key}");
		}
	}

}
