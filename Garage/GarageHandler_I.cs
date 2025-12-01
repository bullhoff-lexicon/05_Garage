namespace Garage;


internal interface IGarageHandler {
	public Garage<Vehicle> CurrentGarage { get; }
	public int GarageSelected { get; set; }
	public IEnumerable<Vehicle> GetEnumerator();
	public int FindIndex(Func<Vehicle, bool> func);
	public void AddGarage(int size = 20);
	public Vehicle? AddVehicle(Vehicle? vehicle = null);
	public bool Remove(int i);
	public bool Remove(string id);
	public Vehicle? GetVehicle(string licencePlate);
}
