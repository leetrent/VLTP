using System;
using System.Xml;
using System.Text;
using VLTP.Constants;

namespace VLTP.Utils
{
    public class MaxGovResponse
    {
        public string MaxSessionIndex { get; }
        public string MaxEmail { get; }

        public MaxGovResponse(string encodedSamlResponse)
        {
            string logSnippet = "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][MaxGovResponse][Constructor] => ";

            string decodedSamlResponse = Encoding.UTF8.GetString(Convert.FromBase64String(encodedSamlResponse));
            //this.writeToLog(decodedSamlResponse);

            XmlDocument xmlDoc = new XmlDocument
            {
                XmlResolver = null,
                PreserveWhitespace = true
            };

            xmlDoc.LoadXml(decodedSamlResponse);
            XmlElement xmlDocElement = xmlDoc.DocumentElement;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // StatusCode
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            string statusCodeValue = extractStatusCodeValue(xmlDocElement);
            if (statusCodeValue.Equals(Saml2Constants.StatusCodeSuccessValue) == false)
            {
                StringBuilder sb = new StringBuilder("Saml2Response did not contain a successful status code value.");
                sb.Append(" Expected: \"");
                sb.Append(Saml2Constants.StatusCodeSuccessValue);
                sb.Append("\". Received: \"");
                sb.Append(statusCodeValue);
                sb.Append("\". Redirecting user to 'Authentication Failed' page.");

                throw new Exception(sb.ToString());
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // SessionIndex
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            this.MaxSessionIndex = extractSessionIndexValue(xmlDocElement);
            if ( String.IsNullOrEmpty(this.MaxSessionIndex) || String.IsNullOrWhiteSpace(this.MaxSessionIndex))
            {
                throw new Exception($"Saml2Response did not contain a '{Saml2Constants.AttributeNameForSessionIndex}' value.");
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // MaxEmail
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            this.MaxEmail = extractMaxEmailValue(xmlDocElement);
            if (String.IsNullOrEmpty(this.MaxEmail) || String.IsNullOrWhiteSpace(this.MaxEmail))
            {
                throw new Exception($"Saml2Response did not contain a '{Saml2Constants.AttributeNameValueForMaxEmail}' value.");
            }

            Console.WriteLine(logSnippet + $"|{this.MaxEmail}|{statusCodeValue}|{this.MaxSessionIndex}|");
        }

        private string extractStatusCodeValue(XmlElement xmlDocElement)
        {
            XmlNodeList xmlStatusNodeList = xmlDocElement.GetElementsByTagName(Saml2Constants.ElementNameForStatus);
            XmlNode xmlStatusNode = xmlStatusNodeList[0];
            XmlNode xmlStatusCodeNode = xmlStatusNode.FirstChild;
            XmlAttribute xmlStatusCodeValueAttribute = xmlStatusCodeNode.Attributes[Saml2Constants.AttributeNameForStatusCodeValue];
            return xmlStatusCodeValueAttribute.Value;
        }

        private string extractSessionIndexValue(XmlElement xmlDocElement)
        {
            XmlNodeList xmlAssertionNodeList = xmlDocElement.GetElementsByTagName(Saml2Constants.ElementNameForAssertion);
            XmlNode xmlAssertionNode = xmlAssertionNodeList[0];

            XmlElement xmlAuthnStatementElement = xmlAssertionNode[Saml2Constants.ElementNameForAuthnStatement];
            XmlAttribute xmlSessionIndexAttribute = xmlAuthnStatementElement.Attributes[Saml2Constants.AttributeNameForSessionIndex];

            return xmlSessionIndexAttribute.Value;
        }

        private string extractMaxEmailValue(XmlElement xmlDocElement)
        {
            XmlNodeList xmlAssertionNodeList = xmlDocElement.GetElementsByTagName(Saml2Constants.ElementNameForAssertion);
            XmlNode xmlAssertionNode = xmlAssertionNodeList[0];

            XmlElement xmlAttributeStatementElement = xmlAssertionNode[Saml2Constants.ElementNameForAttributeStatement];
            for (int ii = 0; ii < xmlAttributeStatementElement.ChildNodes.Count; ii++)
            {
                if (xmlAttributeStatementElement.ChildNodes[ii].Attributes[Saml2Constants.AttributeNameForName].Value.Equals(Saml2Constants.AttributeNameValueForMaxEmail))
                {
                    XmlNode xmlAttributeNode = xmlAttributeStatementElement.ChildNodes[ii];
                    XmlElement xmlAttributeValueElement = xmlAttributeNode[Saml2Constants.ElementNameForAttributeValue];
                    return xmlAttributeValueElement.InnerText;
                }
            }
            return null;
        }

        private void writeToLog(string decodedSamlResponse)
        {
            string logSnippet = "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "][MaxGovResponse][Constructor] => ";
            Console.WriteLine(logSnippet + "\nBEGIN decodedSamlResponse:\n");
            Console.WriteLine(decodedSamlResponse);
            Console.WriteLine(logSnippet + "\n:END decodedSamlResponse\n");
        }
    }
}
