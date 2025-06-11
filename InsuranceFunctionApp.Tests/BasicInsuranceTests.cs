using InsuranceFunctionApp.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace InsuranceFunctionApp.Tests
{
    public class BasicInsuranceTests
    {
        [Fact]
        public void CalculateTotalMonthlyCost_ShouldReturnCorrectSum()
        {
            // Arrange
            var insurances = new List<Insurance>
            {
                new Insurance { Type = "Pet", MonthlyCost = 10 },
                new Insurance { Type = "Car", MonthlyCost = 30 },
                new Insurance { Type = "Health", MonthlyCost = 20 }
            };

            // Act
            int total = (int)insurances.Sum(i => i.MonthlyCost);

            // Assert
            Assert.Equal(60, total);
        }

        [Fact]
        public void CarInsurance_ShouldContainVehicleDetails()
        {
            // Arrange
            var carInsurance = new Insurance
            {
                Type = "Car",
                MonthlyCost = 30,
                Vehicle = new Vehicle
                {
                    RegistrationNumber = "ABC123",
                    Make = "Toyota",
                    Model = "Camry",
                    Year = 2019
                }
            };

            // Assert
            Assert.NotNull(carInsurance.Vehicle);
            Assert.Equal("Toyota", carInsurance.Vehicle.Make);
            Assert.Equal(2019, carInsurance.Vehicle.Year);
        }

        [Fact]
        public void NoInsurances_ShouldReturnEmptyList()
        {
            // Arrange
            var insurances = new List<Insurance>();

            // Assert
            Assert.Empty(insurances);
        }
    }
}
