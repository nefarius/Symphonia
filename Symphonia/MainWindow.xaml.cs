using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Antlr4.StringTemplate;
using LiteDB;
using MahApps.Metro.Controls.Dialogs;
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
        }

        private static void UpdateDatabase(ProgressDialogController pdc)
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "Symphonia.DLNA.SOAP.Synology.ContentDirectory.Browse.txt";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var result = reader.ReadToEnd();

                using (var sc = new SoapClient(Settings.Default.MediaServer))
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
                        st.Add("RequestedCount", Settings.Default.BatchSize);

                        var sa = SoapAction.FromTemplate(st.Render());

                        var response = sc.InvokeSoapAction<Envelope>(sa).DeserializedResponse;

                        pdc.Maximum = response.Body.BrowseResponse.TotalMatches;

                        if (response.Body.BrowseResponse.NumberReturned == 0)
                            break;

                        index += response.Body.BrowseResponse.NumberReturned;

                        pdc.SetProgress(index);
                        pdc.SetMessage($"Downloading content information ({((int)(index * 100/pdc.Maximum))}%)");

                        var list = response.Body.BrowseResponse.DeserializedResult;

                        using (var db = new LiteDatabase(@"MyData.db"))
                        {
                            var items = db.GetCollection<Item>();

                            items.Upsert(list.Item);

                            items.EnsureIndex(i => i.Album);
                            items.EnsureIndex(i => i.Author);
                            items.EnsureIndex(i => i.Creator);
                            items.EnsureIndex(i => i.Artist);
                            items.EnsureIndex(i => i.Title);
                            items.EnsureIndex(i => i.Genre);
                        }
                    } while (true);
                }
            }
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var controller = await this.ShowProgressAsync("Updating Database...", "Downloading content information");
            controller.CloseAsync();

            await Task.Factory.StartNew((dynamic data) => UpdateDatabase(data.Controller),
                new { Controller = controller });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var query = SearchQuery.Text;

            using (var db = new LiteDatabase(@"MyData.db"))
            {
                var items = db.GetCollection<Item>();

                try
                {
                    var result = items.Find(
                        i => i.Album.Contains(query)
                        || i.Author.Contains(query)
                        || i.Title.Contains(query));

                    var sb = new StringBuilder();

                    foreach (var item in result)
                    {
                        sb.AppendLine(item.Title);
                    }

                    SearchResult.Text = sb.ToString();
                }
                catch { return;}
            }
        }
    }
}