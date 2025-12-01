namespace Garage;


internal interface IUI {
	public int AddGarage();
	public int SelectGarage(List<Garage<Vehicle>> garages);
	public void Print(GarageHandler _handler);
	public void Print(GarageHandler _handler, Action<Vehicle> action);
	public void PrintVehicleTypeCount(GarageHandler _handler);
	public void Print(Vehicle v);
	public void PromptSearchLicencePlate(GarageHandler _handler);
	public void PromptSearch(GarageHandler _handler);
	public string PromptRemoveVehicle(GarageHandler _handler);
	public Vehicle? PromptVehicle(GarageHandler _handler);
}
