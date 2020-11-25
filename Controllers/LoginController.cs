using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using VLTP.Constants;
//using VLTP.Saml2Utils;
using VLTP.Utils;

namespace VLTP.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration cfg)
        {
            _config = cfg;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin()
        {
            string logSnippet = new StringBuilder("[")
                                .Append(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"))
                                .Append("][LoginController][ExternalLogin][HttpPost] => ")
                                .ToString();

            string assertionConsumerServiceUrl = $"https://{Request.Host}{Request.PathBase}/{MaxGovConstants.ASSERTION_CONSUMER_SERVICE_URI}";
            Console.WriteLine(logSnippet + $"(assertionConsumerServiceUrl): '{assertionConsumerServiceUrl}'");

            // MaxGovConfig
            MaxGovConfig maxGovConfig = new MaxGovConfig(_config);

            // MaxGovRequest 
            MaxGovRequest maxGovRequest = new MaxGovRequest(maxGovConfig, assertionConsumerServiceUrl);

            Console.WriteLine(logSnippet + "BEGIN (MaxGovRequest) =>");
            Console.WriteLine(maxGovRequest.Saml2Request.OuterXml);
            Console.WriteLine(logSnippet + "<= END (MaxGovRequest)");

            StringBuilder result = new StringBuilder();
            result.AppendFormat("{0}=", "SAMLRequest");
            string xmlValue = maxGovRequest.Saml2Request.OuterXml;
            string encoded = xmlValue.DeflateEncode();
            string urlEncoded = encoded.UrlEncode();
            string upperCaseUrlEncoded = urlEncoded.UpperCaseUrlEncode();
            result.Append(upperCaseUrlEncoded);

            return Redirect($"{maxGovConfig.Destination}?{result}");
        }
    }
}