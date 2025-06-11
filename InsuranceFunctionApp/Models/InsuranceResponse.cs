using System.Collections.Generic;
using System.Linq;

namespace InsuranceFunctionApp.Models
{
    public class InsuranceResponse
    {
        public string PersonId { get; set; }
        public List<Insurance> Insurances { get; set; }
        public double TotalMonthlyCost => Insurances.Sum(i => i.MonthlyCost);
    }
}