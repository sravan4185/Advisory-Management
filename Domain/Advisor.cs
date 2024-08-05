namespace AdvisorManagement.Domain
{
    public class Advisor
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string SIN { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public required string HealthStatus { get; set; }
    }

}
