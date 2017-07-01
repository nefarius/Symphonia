/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */

using System.IO;
using System.Xml.Serialization;

namespace Symphonia.DLNA.SOAP.Synology.ContentDirectory.Browse
{
    [XmlRoot(ElementName = "BrowseResponse", Namespace = "urn:schemas-upnp-org:service:ContentDirectory:1")]
    public class BrowseResponse
    {
        [XmlElement(ElementName = "Result", Namespace = "")]
        public string Result { get; set; }

        [XmlElement(ElementName = "NumberReturned", Namespace = "")]
        public int NumberReturned { get; set; }

        [XmlElement(ElementName = "TotalMatches", Namespace = "")]
        public int TotalMatches { get; set; }

        [XmlElement(ElementName = "UpdateID", Namespace = "")]
        public string UpdateID { get; set; }

        public DIDLLite DeserializedResult
        {
            get
            {
                var xs = new XmlSerializer(typeof(DIDLLite));
                using (var rdr = new StringReader(Result))
                {
                    return (DIDLLite) xs.Deserialize(rdr);
                }
            }
        }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Body
    {
        [XmlElement(ElementName = "BrowseResponse", Namespace = "urn:schemas-upnp-org:service:ContentDirectory:1")]
        public BrowseResponse BrowseResponse { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }
    }
}