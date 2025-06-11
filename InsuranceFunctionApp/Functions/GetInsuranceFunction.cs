using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using InsuranceFunctionApp.Services;
using InsuranceFunctionApp.Models;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InsuranceFunctionApp.Functions
{
    public class GetInsuranceFunction
    {
        private readonly IVehicleClient _vehicleClient;
        private readonly ILogger _logger;

        public GetInsuranceFunction(IVehicleClient vehicleClient, ILoggerFactory loggerFactory)
        {
            _vehicleClient = vehicleClient;
            _logger = loggerFactory.CreateLogger<GetInsuranceFunction>();
        }

        [Function("GetInsurance")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "insurance/{personId}")] HttpRequestData req,
            string personId)
        {
            if (string.IsNullOrWhiteSpace(personId))
            {
                var badRes = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRes.WriteStringAsync("Person ID is required.");
                return badRes;
            }

            var insurances = new List<Insurance>();
            if (personId == "12345")
            {
                insurances.Add(new Insurance { Type = "Pet", MonthlyCost = 10 });
                insurances.Add(new Insurance { Type = "Car", MonthlyCost = 30 });
                var vehicle = await _vehicleClient.GetVehicleAsync("ABC123");
                insurances.Last().Vehicle = vehicle;
            }

            if (!insurances.Any())
            {
                var notFound = req.CreateResponse(HttpStatusCode.NotFound);
                await notFound.WriteStringAsync("No insurance found for person.");
                return notFound;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new InsuranceResponse
            {
                PersonId = personId,
                Insurances = insurances
            });

            return response;
        }
    }
}