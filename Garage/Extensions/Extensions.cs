// using Garage.Helpers;
// using System.Collections;
// using System.Linq;

namespace Garage;


public static class StringExtensions {

	public static string Capitalize(this string input) {
		if (input == null || input.Length == 0) return "";
		return input[0].ToString().ToUpper() + input.Substring(1);
	}

}

internal static class Extensions {

	public static string GetClassName<T>(this T inst) {
		if (inst == null) return typeof(T).ToString().Split('.').Last();
		return inst.GetType().ToString().Split('.').Last();
	}

	public static bool IsPropEqual<T>(this T inst, string key, bool val) {
		bool? x = inst?.GetType().GetProperty(key)?.GetValue(inst, null) as bool?;
		if (x == null) x = false;
		return x == val;
	}
	public static bool IsPropEqual<T>(this T inst, string key, int val) {
		int? x = inst?.GetType().GetProperty(key)?.GetValue(inst, null) as int?;
		return x == val;
	}
	public static bool IsPropEqual<T>(this T inst, string key, string val) {
		string? x = inst?.GetType().GetProperty(key)?.GetValue(inst, null) as string;
		return x == val;
	}

}

// TODO: Move to GarageHandler
internal static class GarageExtensions {

	public static void AddVehicle(this Garage<Vehicle> garage, Vehicle? vehicle = null) {
		if (vehicle == null) return;
		if (vehicle.LicencePlate == null || vehicle.LicencePlate.Length < 3) return;
		vehicle.LicencePlate ??= "";
		var veh = garage.GetVehicle(vehicle.LicencePlate);
		if (veh == null) garage.Add(vehicle);
		else Console.WriteLine($"{vehicle.LicencePlate} already exists");
	}

	public static System.Reflection.PropertyInfo[]? GetProperties<T>(this T vehicle) {
		if (vehicle == null) return null;
		string vehicleType = vehicle.GetType().ToString().Split('.').Last();
		return (vehicleType.ToLower() switch {
			"car" => typeof(Car),
			"motorcycle" => typeof(Motorcycle),
			"airplane" => typeof(Airplane),
			"bus" => typeof(Bus),
			"boat" => typeof(Boat),
			_ => typeof(Vehicle),
		})?.GetProperties();
	}
	public static System.Reflection.PropertyInfo[]? GetProperties<T>(this Type vehicleClass) {
		string vehicleType = vehicleClass.ToString().Split('.').Last();
		return (vehicleType.ToLower() switch {
			"car" => typeof(Car),
			"motorcycle" => typeof(Motorcycle),
			"airplane" => typeof(Airplane),
			"bus" => typeof(Bus),
			"boat" => typeof(Boat),
			_ => typeof(Vehicle),
		})?.GetProperties();
	}

	public static Vehicle? GetVehicle(this IEnumerable<Vehicle> vehicles, string licencePlate) {
		foreach (var v in vehicles) {
			if (v.LicencePlate == licencePlate) return v;
		}
		return null;
	}
	public static string GetVehicleType<T>(this T vehicle) {
		if (vehicle == null) return typeof(T).ToString().Split('.').Last();
		return vehicle.GetType().ToString().Split('.').Last();
	}


}



