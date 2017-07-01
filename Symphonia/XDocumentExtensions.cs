using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Symphonia
{
    public static class XDocumentExtensions
    {
        public static string ToStringWithXmlDeclarationFlattened(this XDocument doc)
        {
            var builder = new StringBuilder();
            var writer = new StringWriter(builder);
            doc.Save(writer, SaveOptions.DisableFormatting);
            writer.Flush();
            return builder.ToString();
        }
    }
}
