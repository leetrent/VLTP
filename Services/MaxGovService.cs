using Microsoft.Extensions.Configuration;

namespace VLTP.Services
{
    public class MaxGovService
    {
        private readonly IConfiguration _config;

        public string EntityId { get; }
        public string Destination { get; }
        public string ProtocolBinding { get; }

        public MaxGovService(IConfiguration cfg)
        {
            _config = cfg;

            this.EntityId = _config["Saml2:MaxGov:ServiceProviderConfiguration:EntityId"];
            this.Destination = _config["Saml2:MaxGov:IdentityProviderConfiguration:SingleSignOnService"];
            this.ProtocolBinding = _config["Saml2:MaxGov:IdentityProviderConfiguration:ProtocolBinding"];
        }
    }
}
