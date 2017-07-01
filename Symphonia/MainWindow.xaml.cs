using System.IO;
using System.Linq;
using System.Reflection;
using Antlr4.StringTemplate;
using LiteDB;
using Symphonia.DLNA.SOAP.Synology.ContentDirectory.Browse;
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
            const string resourceName = "Symphonia.DLNA.SOAP.Synology.ContentDirectory.Browse.txt";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var result = reader.ReadToEnd();

                using (var sc = new SoapClient("http://192.168.2.180:50001"))
                {
                    var index = 0;

                    do
                    {
                        var st = new Template(result, Settings.Default.TemplateStartCharacter,
                            Settings.Default.TemplateStopCharacter);

                        st.Add("ObjectID", 23);
                        st.Add("BrowseFlag", "BrowseDirectChildren");
                        st.Add("Filter", "*");
                        st.Add("StartingIndex", index);
                        st.Add("RequestedCount", "1000");

                        var sa = SoapAction.FromTemplate(st.Render());

                        var response = sc.InvokeSoapAction<Envelope>(sa).Deserialize();

                        if (response.Body.BrowseResponse.NumberReturned == 0)
                            break;

                        index += response.Body.BrowseResponse.NumberReturned;

                        var list = response.Body.BrowseResponse.DeserializedResult;

                        using (var db = new LiteDatabase(@"MyData.db"))
                        {
                            var items = db.GetCollection<Item>();

                            items.Upsert(list.Item);
                        }
                    } while (true);
                }
            }
        }
    }
}