namespace D3SK.NetCore.Domain.Models
{
    public class DomainOptions
    {
        public bool UseAuthentication { get; set; }

        public bool UseAuthorization { get; set; }

        public bool AllowUiTestAuthentication { get; set; }
    }
}
