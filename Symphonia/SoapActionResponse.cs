using System.IO;
using System.Xml.Serialization;

namespace Symphonia
{
    public class SoapActionResponse<T>
    {
        public string Response { get; }

        public SoapActionResponse(string response)
        {
            Response = response;
        }

        public T Deserialize()
        {
            var xs = new XmlSerializer(typeof(T));
            using (var rdr = new StringReader(Response))
            {
                return (T)xs.Deserialize(rdr);
            }
        }
    }

    public interface ISoapActionResponseDeserializer<out T>
    {
        T Deserialize();
    }
}
