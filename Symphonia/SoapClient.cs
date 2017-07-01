using System;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Symphonia.Properties;

namespace Symphonia
{
    public class SoapClient : WebClient
    {
        public SoapClient()
        {
            Encoding = Encoding.UTF8;
        }

        public string InvokeSoapAction(Uri address, string action, string body)
        {
            Headers[HttpRequestHeader.ContentType] = "text/xml; charset=\"utf - 8\"";
            Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
            Headers[HttpRequestHeader.UserAgent] = Settings.Default.UserAgent;
            Headers["SOAPAction"] = action;

            var xdoc = XDocument.Parse(body);

            return UploadString(address, xdoc.ToStringWithXmlDeclarationFlattened());
        }
    }
}