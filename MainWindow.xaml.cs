using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Xml.Linq;

namespace LinQWithXML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var url = "http://api.nbp.pl/api/exchangerates/rates/c/usd/last/10/?format=xml";

            HttpClient cli = new HttpClient();
            var resp = cli.GetAsync(url);
            var web = resp.Result.Content;
            var cont = new StreamReader(web.ReadAsStream());
            var xml = cont.ReadToEnd();

            XDocument body = new XDocument();

            body = XDocument.Parse(xml);

            var rows = from c in body.Descendants("Rate")
                       select new {
                           key = c.Element("No").Value,
                           date = c.Element("EffectiveDate").Value,
                           width = c.Element("Bid").Value,
                           height = c.Element("Ask").Value,
                           text = $"{c.Element("EffectiveDate").Value} - {c.Element("Bid").Value} - {c.Element("Ask").Value}",
                       };

            lista.DisplayMemberPath = "text";
            lista.SelectedValuePath = "key";
            lista.ItemsSource = rows;
        }

        private void lista_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            display.Content = lista.SelectedValue;
        }
    }
}
