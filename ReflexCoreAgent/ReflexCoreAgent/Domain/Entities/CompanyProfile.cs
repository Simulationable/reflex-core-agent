namespace ReflexCoreAgent.Domain.Entities
{
    public class CompanyProfile
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? LogoBase64 { get; set; }
    }

}
