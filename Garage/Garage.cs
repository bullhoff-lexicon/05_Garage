using System.Collections;
// using System.Linq;
// using Garage.Helpers;

namespace Garage;


// internal interface IGarageHandler { }
// internal class GarageHandler : IGarageHandler {
// 	private readonly IGarage<Vehicle> _garage;
// 	public GarageHandler(IGarage<Vehicle> garage) { _garage = garage; }
// 	// public Vehicle? AddVehicle(Vehicle? vehicle = null) {
// 	// 	if (vehicle == null) return null;
// 	// 	int index = _garage.FindIndex(v => v.LicencePlate == vehicle.LicencePlate);
// 	// 	if (index == -1) {
// 	// 		bool res = _garage.Add(vehicle);
// 	// 		if (!res) Console.WriteLine($"Garage is full, couldnt add {vehicle.LicencePlate}");
// 	// 	} else {
// 	// 		Console.WriteLine($"{vehicle.LicencePlate} already exists");
// 	// 	}
// 	// 	return vehicle;
// 	// }
// }


internal interface IGarage<T> {
	IEnumerator<T> GetEnumerator();
	public int FindIndex(Func<T, bool> func);
	public bool Add(T item);
}
internal class Garage<T> : IEnumerable<T>, IGarage<T> where T : Vehicle {
	private T[] _list;
	public T this[int index] => _list[index];

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	public IEnumerator<T> GetEnumerator() {
		foreach (T item in _list) {
			if (item == null) continue;
			yield return item;
		}
	}

	public int Capacity => _list.Length;
	public int Length {
		get {
			int length = 0;
			foreach (T item in this) {
				if (item != null) length++;
			}
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

}

