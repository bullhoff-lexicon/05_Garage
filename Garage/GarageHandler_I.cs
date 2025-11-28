namespace Garage;


internal interface IGarageHandler {
	public Vehicle? AddVehicle(Vehicle? vehicle = null);
	public bool Remove(int i);
	public bool Remove(string id);
	public Vehicle? GetVehicle(string licencePlate);
	public int FindIndex(Func<Vehicle, bool> func);
	public void Print(Action<Vehicle> action);
	public void Print();
	public void Print(Vehicle v);
	public void PrintVehicleTypeCount(IGarage<Vehicle>? garage = null);

	public int Capacity { get; }
	public int Length { get; }
}
