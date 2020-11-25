using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VLTP.Constants
{
    public class Saml2Constants
    {
        public const string Version = "2.0";

        // XML Elements
        public static readonly string ElementNameForStatus = "saml2p:Status";
        public static readonly string ElementNameForAssertion = "saml2:Assertion";
        public static readonly string ElementNameForAuthnStatement = "saml2:AuthnStatement";
        public static readonly string ElementNameForAttributeStatement = "saml2:AttributeStatement";
        public static readonly string ElementNameForAttributeValue = "saml2:AttributeValue";


        // XML Attributes
        public static readonly string AttributeNameForStatusCodeValue = "Value";
        public static readonly string AttributeNameForSessionIndex = "SessionIndex";
        public static readonly string AttributeNameForName = "Name";

        // XML Values
        public static readonly string StatusCodeSuccessValue = "urn:oasis:names:tc:SAML:2.0:status:Success";
        public static readonly string AttributeNameValueForMaxEmail = "maxEmail";
    }
}
