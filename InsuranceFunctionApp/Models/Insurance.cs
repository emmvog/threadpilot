namespace InsuranceFunctionApp.Models
{
    public class Insurance
    {
        public string Type { get; set; }
        public decimal? MonthlyCost { get; set; }
        public string RegistrationNumber { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}