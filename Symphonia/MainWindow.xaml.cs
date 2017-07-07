using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
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
        private static readonly ReaderWriterLockSlim DbLock = new ReaderWriterLockSlim();
        private static readonly LiteDatabase ContentDb = new LiteDatabase(Settings.Default.DatabaseName);

        public MainWindow()
        {
            InitializeComponent();

            ViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals("SearchQuery"))
                    Task.Run(() =>
                    {
                        var query = ViewModel.SearchQuery.ToLower();

                        try
                        {
                            if (!DbLock.TryEnterReadLock(10)) return;

                            var items = ContentDb.GetCollection<Item>();

                            var result = items.Find(Query.Contains("FullTextIndex", query));

                            var sb = new StringBuilder();

                            foreach (var item in result)
                                sb.AppendLine($"{item.Author} - {item.Title}");

                            ViewModel.SearchResult = sb.ToString();
                        }
                        catch
                        {
                            ViewModel.SearchResult = string.Empty;
                        }
                        finally
                        {
                            DbLock.ExitReadLock();
                        }
                    });
            };
        }

        private MainViewModel ViewModel => (MainViewModel) Resources["MainViewModel"];

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
                        pdc.SetMessage($"Downloading content information ({(int) (index * 100 / pdc.Maximum)}%)");

                        Task.Factory.StartNew((dynamic data) =>
                            {
                                DbLock.EnterWriteLock();

                                try
                                {
                                    var items = ContentDb.GetCollection<Item>();

                                    items.Upsert(data.Result.Item);

                                    BsonMapper.Global.Entity<Item>().Index<Item>(
                                        "FullTextIndex",
                                        item =>
                                            $"{item.Album?.ToLower()} " +
                                            $"{item.Author?.ToLower()} " +
                                            $"{item.Creator?.ToLower()} " +
                                            $"{item.Artist?.ToLower()} " +
                                            $"{item.Title?.ToLower()} " +
                                            $"{item.Genre?.ToLower()}");
                                }
                                finally
                                {
                                    DbLock.ExitWriteLock();
                                }
                            },
                            new {Result = response.Body.BrowseResponse.DeserializedResult});
                    } while (true);
                }

                pdc.CloseAsync();
            }
        }

        private async void UpdateButton(object sender, RoutedEventArgs e)
        {
            var controller = await this.ShowProgressAsync("Updating Database...", "Downloading content information");

            await Task.Factory.StartNew((dynamic data) => UpdateDatabase(data.Controller),
                new {Controller = controller});
        }
    }
}