// using System.Collections;
// using System.Linq;
// using Garage.Helpers;

namespace Garage;


internal interface IVehicle { }
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

internal class Car : Vehicle {
	public string? Brand { get; set; }
	public bool? AmphibiousExploringVehicle { get { return (Brand?.ToLower() == "range rover") ? true : null; } }
	public Car(string licencePlate, string color = "", int wheels = 4, string? brand = null, bool amphibiousExploringVehicle = false) : base(licencePlate, color, wheels) {
		Brand = brand;
	}
}
internal class Motorcycle : Vehicle {
	public string FuelType { get; set; } = "Gasoline";
	public Motorcycle(string licencePlate, string color = "", int wheels = 2) : base(licencePlate, color, wheels) { }
}
internal class Airplane : Vehicle {
	public int Engines { get; set; } = 2;
	public Airplane(string licencePlate, string color = "", int wheels = 6, int engines = 2) : base(licencePlate, color, wheels) { Engines = engines; }
}
internal class Bus : Vehicle {
	public int Seats { get; set; } = 20;
	public Bus(string licencePlate, string color = "", int wheels = 6) : base(licencePlate, color, wheels) { }
}
internal class Boat : Vehicle {
	public int Length { get; set; } = 50;
	public Boat(string licencePlate, string color = "", int wheels = 0) : base(licencePlate, color, wheels) { }
}

