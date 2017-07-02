using System.IO;
using System.Xml.Serialization;

namespace Symphonia
{
    public class SoapActionResponse<T>
    {
        public SoapActionResponse(string response)
        {
            Response = response;
        }

        public string Response { get; }

        public T DeserializedResponse
        {
            get
            {
                var xs = new XmlSerializer(typeof(T));
                using (var rdr = new StringReader(Response))
                {
                    return (T) xs.Deserialize(rdr);
                }
            }
        }
    }

    public interface ISoapActionResponseDeserializer<out T>
    {
        T Deserialize();
    }
}