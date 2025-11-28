using System.Text.RegularExpressions;

namespace Garage;


internal class Vehicle : IVehicle {

	public string? LicencePlate { get; set; } = null;
	public string? Color { get; set; } = null;
	public int? Wheels { get; set; } = null;

	public Vehicle(string licencePlate, string color, int wheels) {
		LicencePlate = licencePlate.ToLower();
		Color = color;
		Wheels = wheels;
	}

	public object this[string key] {
		get => this.GetType().GetProperty(key)?.GetValue(this, null)!;
		set => GetType().GetProperty(key)?.SetValue(this, value);
	}

}


