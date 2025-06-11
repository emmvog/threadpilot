using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using VehicleFunctionApp.Models;

namespace VehicleFunctionApp.Functions
{
    public class GetVehicleFunction
    {
        private static readonly Dictionary<string, Vehicle> Vehicles = new()
        {
            { "ABC123", new Vehicle { RegistrationNumber = "ABC123", Make = "Mercedes", Model = "GLC", Year = 2019 } },
            { "XYZ789", new Vehicle { RegistrationNumber = "XYZ789", Make = "Volvo", Model = "XC40", Year = 2020 } }
        };

        [Function("GetVehicle")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "vehicle/{registrationNumber}")] HttpRequestData req,
            string registrationNumber,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetVehicleFunction");
            logger.LogInformation($"Fetching vehicle for: {registrationNumber}");

            var response = req.CreateResponse();
            if (Vehicles.TryGetValue(registrationNumber.ToUpper(), out var vehicle))
            {
                response.StatusCode = HttpStatusCode.OK;
                response.WriteAsJsonAsync(vehicle).GetAwaiter();
            }
            else
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.WriteStringAsync("Vehicle not found").Wait();
            }

            return response;
        }
    }
}