using VehicleFunctionApp.Models;
using Xunit;
using System.Collections.Generic;

namespace VehicleFunctionApp.Tests
{
    public class VehicleFunctionTests
    {
        private readonly Dictionary<string, Vehicle> _vehicles = new()
        {
            { "ABC123", new Vehicle { RegistrationNumber = "ABC123", Make = "Mercedes", Model = "GLC", Year = 2019 } },
            { "XYZ789", new Vehicle { RegistrationNumber = "XYZ789", Make = "Volvo", Model = "XC40", Year = 2020 } }
        };

        [Fact]
        public void ShouldReturnVehicleForValidPlate()
        {
            var exists = _vehicles.TryGetValue("ABC123", out var vehicle);

            Assert.True(exists);
            Assert.NotNull(vehicle);
            Assert.Equal("Toyota", vehicle.Make);
        }

        [Fact]
        public void ShouldReturnNullForInvalidPlate()
        {
            var exists = _vehicles.TryGetValue("ZZZ999", out var vehicle);

            Assert.False(exists);
            Assert.Null(vehicle);
        }

        [Fact]
        public void VehicleShouldBeFromRecentYear()
        {
            var vehicle = _vehicles["XYZ789"];
            Assert.InRange(vehicle.Year, 2010, 2030);
        }
    }
}
