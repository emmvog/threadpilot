using Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "vehicle/{registrationNumber}")] HttpRequestData req, string registrationNumber, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetVehicleFunction");

            var response = req.CreateResponse();

            try
            {
                logger.LogInformation($"Fetching vehicle for: {registrationNumber}");

                if (string.IsNullOrWhiteSpace(registrationNumber))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    await response.WriteAsJsonAsync(new { error = "Registration number is required." });
                    return response;
                }

                if (Vehicles.TryGetValue(registrationNumber.ToUpper(), out var vehicle))
                {
                    response.StatusCode = HttpStatusCode.OK;
                    await response.WriteAsJsonAsync(vehicle);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    await response.WriteAsJsonAsync(new { error = "Vehicle not found." });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error occurred in GetVehicle function.");
                response.StatusCode = HttpStatusCode.InternalServerError;
                await response.WriteAsJsonAsync(new { error = "Internal server error. Please try again later." });
            }

            return response;
        }
    }
}