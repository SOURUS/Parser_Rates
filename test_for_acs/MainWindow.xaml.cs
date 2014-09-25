using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using System.Net;
namespace test_for_acs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DateTime dt = new DateTime();
        public MainWindow()
        {
            InitializeComponent();
            dt = DateTime.Now;
            calendar1.SelectedDate = dt; //по дефолту, сегодняшний день
        }

        public class Valute
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Price { get; set; }

        }

        private List<Valute> LoadCollectionData(XmlDocument file)
        {

            XmlNodeList CharCode_ = file.GetElementsByTagName("CharCode");
            XmlNodeList Name_ = file.GetElementsByTagName("Name");
            XmlNodeList Value_ = file.GetElementsByTagName("Value");

            List<Valute> Valutes = new List<Valute>();

            for (int i = 0; i < 11; i++)
            {
                Valutes.Add(new Valute()
                {
                    ID = Convert.ToString(CharCode_[i].InnerText),
                    Name = Convert.ToString(Name_[i].InnerText),
                    Price = Convert.ToString(Value_[i].InnerText),
                });
            }

            return Valutes;

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            string xml = "";
            string date = calendar1.SelectedDate.Value.ToString("dd'/'MM'/'yyyy");

            try
            {
                WebRequest wrq = WebRequest.Create("http://www.cbr.ru/scripts/XML_daily.asp?date_req=" + date);
                WebResponse wrb = wrq.GetResponse();
                Stream responseStream = null;
                responseStream = wrb.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream, System.Text.Encoding.Default);

                string line = "";
                while ((line = sr.ReadLine()) != null)
                    xml += line;
                sr.Close();
                responseStream.Close();
                doc.LoadXml(xml);
                dataGrid1.ItemsSource = LoadCollectionData(doc);
            }

            catch (Exception ex)
            { label1.Content = "Fail the " + ex; }
        }
    }
}
