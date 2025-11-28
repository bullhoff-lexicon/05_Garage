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
	public void Capacity_Capacity5_ShouldBe5() {
		var garage = new Garage<Vehicle>(5);
		Assert.Equal(5, garage.Capacity);
	}

	[Fact]
	public void Constructor_5AsArg1_ShouldNotBeNull() {
		var garage = new Garage<Vehicle>(5);
		Assert.False(garage == null);
	}

	[Theory]
	[InlineData("c", 2)]
	[InlineData("e", -1)]
	public void FindIndex_abcd(string find, int expected) {
		var garage = new Garage<Vehicle>(5);
		garage.Add(new Car("a"));
		garage.Add(new Car("b"));
		garage.Add(new Car("c"));
		garage.Add(new Car("d"));
		int index = garage.FindIndex(vehicle => vehicle.LicencePlate == find);
		Assert.True(index == expected);
	}

	[Fact]
	public void Add_Car_True() {
		var garage = new Garage<Vehicle>(5);
		bool res = garage.Add(new Car("a"));
		Assert.True(res == true);
	}

	[Fact]
	public void Remove_Index0_True() {
		var garage = new Garage<Vehicle>(5);
		garage.Add(new Car("a"));
		bool res = garage.Remove(0);
		Assert.True(res == true);
	}

	[Theory]
	[InlineData(5, 0, false)]
	[InlineData(5, 4, false)]
	[InlineData(5, 5, true)]
	public void IsFull(int capacity, int length, bool expected) {
		var garage = new Garage<Vehicle>(capacity);
		while (length-- > 0) garage.Add(new Car("a"));
		Assert.Equal(expected, garage.IsFull);
	}


}


