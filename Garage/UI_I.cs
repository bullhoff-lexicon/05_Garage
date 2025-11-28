namespace Garage;


internal interface IUI {
	public void ShowMainMenu();
	public void PopulateGarage();
	public Vehicle? PromptSearchLicencePlate();
	public void PromptSearch();
	public void PromptRemoveVehicle();
	public void PromptVehicle();
	public void PrintGarageInfo();
	public void Print();
}
