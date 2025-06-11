using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using InsuranceFunctionApp.Services;
using InsuranceFunctionApp.Models;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace InsuranceFunctionApp.Functions
{
    public class GetInsuranceFunction
    {
        private readonly IVehicleClient _vehicleClient;

        public GetInsuranceFunction(IVehicleClient vehicleClient)
        {
            _vehicleClient = vehicleClient;
        }

        [Function("GetInsurance")]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "insurance/{personId}")] HttpRequestData req,
            string personId,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetInsuranceFunction");
            var response = req.CreateResponse();

            try
            {
                logger.LogInformation($"Fetching insurance for person: {personId}");

                if (string.IsNullOrWhiteSpace(personId))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    await response.WriteAsJsonAsync(new { error = "Person ID is required." });
                    return response;
                }

                var insurances = new List<Insurance>();

                if (personId == "12345")
                {
                    insurances.Add(new Insurance { Type = "Pet", MonthlyCost = 10 });
                    insurances.Add(new Insurance { Type = "Car", MonthlyCost = 30 });

                    try
                    {
                        var vehicle = await _vehicleClient.GetVehicleAsync("ABC123");
                        insurances.Last().Vehicle = vehicle;
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "Vehicle service unavailable — skipping vehicle info.");
                        insurances.Last().Vehicle = null;
                    }
                }
                else if (personId == "67890")
                {
                    insurances.Add(new Insurance { Type = "Personal Health", MonthlyCost = 20 });
                }

                if (!insurances.Any())
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    await response.WriteAsJsonAsync(new { error = "No insurance found for person." });
                    return response;
                }

                var result = new InsuranceResponse
                {
                    PersonId = personId,
                    Insurances = insurances
                };

                response.StatusCode = HttpStatusCode.OK;
                await response.WriteAsJsonAsync(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception in GetInsurance function.");
                response.StatusCode = HttpStatusCode.InternalServerError;
                await response.WriteAsJsonAsync(new { error = "Internal server error." });
            }

            return response;
        }
    }
}