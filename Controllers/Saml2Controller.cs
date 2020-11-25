using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VLTP.Services;
using VLTP.Utils;

namespace VLTP.Controllers
{
    public class Saml2Controller : Controller
    {
        private readonly IConfiguration _config;
        private readonly IOracleService _oracleService;

        public Saml2Controller(IConfiguration configuration, IOracleService oracleSvc)
        {
            _config = configuration;
            _oracleService = oracleSvc;
        }


        //public async Task<IActionResult> AssertionConsumerService()
       public IActionResult AssertionConsumerService()
        {
            string logSnippet = "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][Saml2Controller][AssertionConsumerService] => ";

            string encodedSamlResponse = null;
            if (Request.Form != null)
            {
                encodedSamlResponse = Request.Form["SamlResponse"];
            }
            MaxGovResponse maxGovResponse = new MaxGovResponse(encodedSamlResponse);
            Console.WriteLine(logSnippet + $"(maxGovResponse.MaxSessionIndex): '{maxGovResponse.MaxSessionIndex}'");
            Console.WriteLine(logSnippet + $"(maxGovResponse.MaxEmail).......: '{maxGovResponse.MaxEmail}'");

            ViewData["sessionIndex"] = $"{maxGovResponse.MaxSessionIndex}";
            ViewData["maxEmailValue"] = $"{maxGovResponse.MaxEmail}";

            _oracleService.InsertRow(maxGovResponse.MaxSessionIndex, maxGovResponse.MaxEmail);
            string vltpUrl = _config["VLTP_URL"];
            return Redirect(string.Format(vltpUrl, maxGovResponse.MaxSessionIndex));

            //return View();
        }
    }
}