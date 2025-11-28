// using System.Collections;
using Garage.Helpers;

namespace Garage;


enum VehicleTypes {
	car, motorcycle, airplane, bus, boat,
};

internal struct Stuff {
	public static string VehicleTypesCsv = string.Join(",", Enum.GetValues(typeof(VehicleTypes)).Cast<VehicleTypes>().Select(v => v.ToString()));
}

internal class Main {
	static List<IUI> garages = new List<IUI>();
	public void Run() {

		Dictionary<string, Action> actionMap;
		actionMap = new Dictionary<string, Action>{
			{ "Create garage, populate and print", ()=> {
					var garage = PromptNew(20);
					garage?.PopulateGarage();
					garage?.Print();
					garage?.ShowMainMenu();
				}
			},
			{ "Create garage", ()=> PromptNew()?.ShowMainMenu() },
			{ "Select garage", ()=> PromptSelect() },
		};

		while (true) {
			int ans = Util.PromptAction(actionMap);
			if (ans == 0) break;
			var (_, func) = actionMap.ElementAt(ans - 1);
			func();
		}

	}

	static IUI? PromptNew(int? ans = null) {
		ans ??= Util.PromptValidInt($"{Style.PromptText} Enter garage size: {Style.Reset}", -1, 200, new Dictionary<char, int> { { 'q', -1 }, {' ', 0}, });
		if (ans == -1) return null;
		garages.Add(new UI((int)ans));
		return garages.Last();
	}
	static IUI? PromptSelect(int? ans = null) {
		for (int i = 0; i < garages.Count; i++) { Console.Write($"{i + 1}. "); garages[i].PrintGarageInfo(); }

		Console.WriteLine($"0. Exit to main menu");
		ans ??= Util.PromptValidInt($"{Style.PromptText} Pick garage number: {Style.Reset}", 0, garages.Count, new Dictionary<char, int> { { 'q', 0 }, });
		if (ans == 0) return null;
		ans--;
		var garage = garages[(int)ans];
		garage.ShowMainMenu();
		return garage;
	}

}

