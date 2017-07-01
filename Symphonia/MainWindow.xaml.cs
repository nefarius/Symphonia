using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Antlr4.StringTemplate;
using Symphonia.Properties;

namespace Symphonia
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Symphonia.DLNA.SOAP.Synology.ContentDirectory.Browse.txt";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var result = reader.ReadToEnd();

                var st = new Template(result, Settings.Default.TemplateStartCharacter, Settings.Default.TemplateStopCharacter);
                
                st.Add("ObjectID", 23);
                st.Add("BrowseFlag", "BrowseDirectChildren");
                st.Add("Filter", "*");
                st.Add("StartingIndex", 0);
                st.Add("RequestedCount", "1000");

                var t = st.Render();

                var sa = SoapAction.FromTemplate(t);

                var url = new Uri("http://192.168.2.180:50001/ContentDirectory/control");

                using (var sc = new SoapClient())
                {
                    var ret = sc.InvokeSoapAction(url, sa.Action, sa.Body);
                }
            }
        }
    }
}