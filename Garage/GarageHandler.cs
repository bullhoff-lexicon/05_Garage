namespace Garage;


internal class GarageHandler : IGarageHandler {
	public List<Garage<Vehicle>> garages = new List<Garage<Vehicle>>();
	public Garage<Vehicle> CurrentGarage { get { return garages[GarageSelected]; } }

	private int _garageSelected = -1;
	public int GarageSelected { get { return _garageSelected == -1 ? garages.Count - 1 : _garageSelected; } set { _garageSelected = value; } }

	public IEnumerable<Vehicle> GetEnumerator() {
		return (IEnumerable<Vehicle>)CurrentGarage;
	}


	public int Capacity => CurrentGarage.Capacity;
	public int Length => CurrentGarage.Length;
	public int FindIndex(Func<Vehicle, bool> func) { return CurrentGarage.FindIndex(func); }

	public void AddGarage(int size = 20) {
		garages.Add(new Garage<Vehicle>(size));
	}

	public Vehicle? AddVehicle(Vehicle? vehicle = null) {
		if (vehicle == null) return null;
		int index = CurrentGarage.FindIndex(v => v.LicencePlate == vehicle.LicencePlate);
		if (index == -1) {
			bool res = CurrentGarage.Add(vehicle);
			if (!res) Console.WriteLine($"Garage is full, couldnt add {vehicle.LicencePlate}");
		} else {
			Console.WriteLine($"{vehicle.LicencePlate} already exists");
		}
		return vehicle;
	}

	public bool Remove(int i) { return CurrentGarage.Remove(i); }
	public bool Remove(string id) {
		int i = CurrentGarage.FindIndex(vehicle => vehicle != null && vehicle.LicencePlate == id);
		if (i < 0) return false;
		return CurrentGarage.Remove(i);
	}

	public Vehicle? GetVehicle(string licencePlate) {
		foreach (var v in CurrentGarage) {
			if (v.LicencePlate == licencePlate) return v;
		}
		return null;
	}

}

