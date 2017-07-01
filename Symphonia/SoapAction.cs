using System.Text.RegularExpressions;

namespace Symphonia
{
    public class SoapAction
    {
        private SoapAction(string path, string action, string body)
        {
            Path = path;
            Action = action;
            Body = body;
        }

        public string Path { get; }

        public string Action { get; }

        public string Body { get; }

        public static SoapAction FromTemplate(string template)
        {
            var path = new Regex("POST (.*) HTTP").Match(template).Groups[1].Value;
            var action = new Regex("SOAPAction: (.*)").Match(template).Groups[1].Value;
            var body = new Regex("(<.*)", RegexOptions.Singleline).Match(template).Groups[1].Value;

            return new SoapAction(path, action, body);
        }
    }
}