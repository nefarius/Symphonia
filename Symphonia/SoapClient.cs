using System;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Symphonia.Properties;

namespace Symphonia
{
    public class SoapClient : IDisposable
    {
        private readonly WebClient _client = new WebClient();
        private readonly Uri _serverUrl;

        protected SoapClient()
        {
            _client.Encoding = Encoding.UTF8;
        }

        public SoapClient(Uri url) : this()
        {
            _serverUrl = url;
        }

        public SoapClient(string url) : this(new Uri(url))
        {
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public SoapActionResponse<T> InvokeSoapAction<T>(SoapAction action)
        {
            _client.Headers[HttpRequestHeader.ContentType] = "text/xml; charset=\"utf - 8\"";
            _client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
            _client.Headers[HttpRequestHeader.UserAgent] = Settings.Default.UserAgent;
            _client.Headers["SOAPAction"] = action.Action;

            var xdoc = XDocument.Parse(action.Body);
            var url = new Uri(_serverUrl, action.Path);

            return new SoapActionResponse<T>(_client.UploadString(url, xdoc.ToStringWithXmlDeclarationFlattened()));
        }
    }
}