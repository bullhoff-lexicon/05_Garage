using Xunit; // dotnet new install xunit.v3.templates

namespace Garage.Tests;


public class GarageTests {

	[Fact]
	public void Length_AddedTwoToCapacity5_ShouldBe2() {
		var garage = new Garage<Vehicle>(5);
		garage.Add(new Car("abc123"));
		garage.Add(new Car("def123"));
		Assert.Equal(2, garage.Length);
	}

	[Fact]
	public void Capacity_SetFromConstructor_ShouldBe5() {
		var garage = new Garage<Vehicle>(5);
		Assert.Equal(5, garage.Capacity);
	}

}

