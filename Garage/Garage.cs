using System.Collections;
// using System.Linq;
using Garage.Helpers;

namespace Garage;


internal interface IGarageHandler { }
internal class GarageHandler : IGarageHandler {
	private readonly IGarage<Vehicle> _garage;

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

	public bool Remove(int i) {
		_garage.Remove(i);
		return true;
	}
	public bool Remove(string id) {
		int i = _garage.FindIndex(vehicle => vehicle != null && vehicle.LicencePlate == id);
		if (i < 0) return false;
		_garage.Remove(i);
		return true;
	}

	public Vehicle? GetVehicle(string licencePlate) {
		foreach (var v in _garage) {
			if (v.LicencePlate == licencePlate) return v;
		}
		return null;
	}

	public int FindIndex(Func<Vehicle, bool> func) {
		int index = _garage.FindIndex(func);
		return index;
	}

	public void Print(Action<Vehicle> action) {
		foreach (var v in _garage) {
			action?.Invoke(v);
		}
	}
	public void Print() {
		foreach (var v in _garage) {
			PrintVehicle(v);
		}
	}
	public void PrintVehicle(Vehicle v) {
		// Console.Write($" {v.GetVehicleType(),-10}  {v.LicencePlate,-6}  {v.Color,-6}  {Style.Key} Wheels {Style.Reset}{Style.Value} {v.Wheels} {Style.Reset} ");
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



internal interface IGarage<T> {
	IEnumerator<T> GetEnumerator();
	public int FindIndex(Func<T, bool> func);
	public bool Add(T item);
	public bool Remove(int i);
	public int Length { get; }
	public int Capacity { get; }
	public bool IsFull { get; }
}
internal class Garage<T> : IEnumerable<T>, IGarage<T> where T : Vehicle {
	protected T[] _list;
	public T this[int index] => _list[index];
	public bool IsFull => Capacity <= Length;
	public int Capacity => _list.Length;
	public int Length {
		get {
			int length = 0;
			foreach (T item in this.Where(item => item != null)) length++;
			return length;
		}
	}

	public Garage(int capacity) {
		_list = new T[capacity];
	}

	public int FindIndex(Func<T, bool> func) {
		int index = Array.FindIndex(_list, (item) => {
			if (item == null) return false;
			return func(item);
		});
		return index;
	}

	public virtual bool Add(T item) {
		int index = Array.FindIndex(_list, (v) => v == null);
		if (index == -1) return false;
		_list[index] = item;
		return true;
	}

	public virtual bool Remove(int i) {
		_list[i] = default(T)!;
		return true;
	}



	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	public IEnumerator<T> GetEnumerator() {
		foreach (T item in _list) {
			if (item == null) continue;
			yield return item;
		}
	}

}

