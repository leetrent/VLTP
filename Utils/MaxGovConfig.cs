using Microsoft.Extensions.Configuration;

namespace VLTP.Utils
{
    public class MaxGovConfig
    {
        public string EntityId { get; }
        public string Destination { get; }
        public string ProtocolBinding { get; }

        public MaxGovConfig(IConfiguration config)
        {
            this.EntityId = config["Saml2:MaxGov:ServiceProviderConfiguration:EntityId"];
            this.Destination = config["Saml2:MaxGov:IdentityProviderConfiguration:SingleSignOnService"];
            this.ProtocolBinding = config["Saml2:MaxGov:IdentityProviderConfiguration:ProtocolBinding"];
        }
    }
}
