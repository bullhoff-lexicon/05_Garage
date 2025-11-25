// using System.Collections;
using Garage.Helpers;

namespace Garage;


internal interface IUI { }
internal class UI : IUI {
	internal static int PromptAction(Dictionary<string, Action> actionMap) {
		Console.WriteLine();
		int i = 0;
		foreach (var (key, value) in actionMap) {
			Console.WriteLine($"{i + 1}. {key}");
			i++;
		}
		Console.WriteLine("0. Exit");
		int ans = Util.PromptValidInt($"", 0, actionMap.Count, new Dictionary<char, int> { { 'q', 0 }, });
		return ans;
	}
	internal static void PrintVehicleTypeCount(Garage<Vehicle> garage) {
		Dictionary<string, int> dict = new Dictionary<string, int>();
		foreach (var v in garage) {
			var type = v.GetType().ToString().Split('.').Last();
			if (!dict.ContainsKey(type)) dict[type] = 1;
			else dict[type]++;
		}
		foreach (var (key, value) in dict) {
			Console.WriteLine($"{value} {key}");
		}
	}
	internal static void PrintGarage(Garage<Vehicle> garage_) {
		foreach (var v in garage_) {
			PrintVehicle(v);
		}
	}
	internal static void PrintVehicle(Vehicle v) {
		Console.Write($"! {v.GetVehicleType(),-10}  {v.LicencePlate,-6}  {v.Color,-6}  {Style.Key} Wheels {Style.Reset}{Style.Value} {v.Wheels} {Style.Reset} ");
		foreach (var property in v.GetType().GetProperties().Where(p => p.Name != "LicencePlate" && p.Name != "Color" && p.Name != "Wheels")) {
			if (property.Name == "Item") continue;
			var propertyValue = property.GetValue(v);
			if (propertyValue == null) continue;
			Console.Write($" {Style.Key} {property.Name} {Style.Reset}{Style.Value} {propertyValue} {Style.Reset} ");
		}
		Console.WriteLine();
	}
}
