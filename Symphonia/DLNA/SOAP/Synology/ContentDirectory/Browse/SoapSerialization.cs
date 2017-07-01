using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Symphonia.DLNA.SOAP.Synology.ContentDirectory.Browse
{
    public static class SoapSerialization
    {
        public static DIDLLite Deserialize(string response)
        {
            var xdoc = XDocument.Parse(response);
            var result = xdoc.Descendants(XName.Get("Result")).First().Value;

            var xs = new XmlSerializer(typeof(DIDLLite));
            using (var rdr = new StringReader(result))
            {
                return (DIDLLite) xs.Deserialize(rdr);
            }
        }
    }
}