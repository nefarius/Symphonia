using System.IO;
using System.Reflection;
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
                st.Add("Filter", "*");

                var t = st.Render();
            }
        }
    }
}