
using LiveCharts.Wpf;
using LiveCharts;
using MySql.Data.MySqlClient;
using SE400.N22.PMCL.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Forms.VisualStyles;

namespace SE400.N22.PMCL.Control
{
    public class MarketControl
    {
        public ObservableCollection<MarketModel> LoadDataBinding { get; set; }
        public ObservableCollection<String> LoadListYear { get; set; }
        public Boolean isLoaded { get; set; }
        public int SelectedYear { get; set; }
        public string MarketName { get; set; }
        //public string SelectedYear { get; set; }
        private MySqlConnection connection;
        private MySqlConnection connection2;
        public SeriesCollection SeriesCollection { get; set; }

        public List<String> Labels { get; set; }

        public Func<double, string> Formatter { get; set; }

        public MarketControl(string marketName, MySqlConnection connection, MySqlConnection connection2)
        {
            LoadDataBinding = new ObservableCollection<MarketModel>();
            LoadListYear = new ObservableCollection<String>();
            isLoaded = false;
            MarketName = marketName;
            this.connection = connection;
            this.connection2 = connection2;
            OnMarketNameChanged();
        }

        public async void Chart(String Market, String year)
        {
            MySqlCommand cmd = new MySqlCommand("Alter table " + MarketName + " SET tiflash replica 1;" +
                        "\nselect /*+ read_from_storage(TIFLASH[" + MarketName + "]) */ date,close FROM " + MarketName + " WHERE year(date) = " + year + " ;", connection2);
            MySqlDataReader reader1 = cmd.ExecuteReader();
            List<String> lsdate = new List<String>();
            ChartValues<float> lsPrices = new ChartValues<float>();
            if (reader1.HasRows == false)
            {
                Console.WriteLine(" D CHAY! ");
            }
            while (await reader1.ReadAsync())
            {
                DateTime dt = DateTime.Parse(reader1.GetString(0));
                lsdate.Add(dt.ToString("dd/MM/yyyy"));
                lsPrices.Add(reader1.GetFloat(1));
            }

            SeriesCollection = new SeriesCollection()
            {
                new LineSeries
                {
                    Title = "Year " + year,Values = lsPrices
                }
            };
            Labels = lsdate;
            Formatter = value => value.ToString("C");
            
        }

        private async void OnMarketNameChanged()
        {
            for(int i = 2000;i<=2020;i++)
            {
                LoadListYear.Add(i.ToString());
            }
            isLoaded = true;
            
            Chart(MarketName, "2000");
            MySqlCommand cmd = new MySqlCommand("Alter table "+ MarketName + " SET tiflash replica 1;" +
                        "\nselect /*+ read_from_storage(TIFLASH[" + MarketName + "]) */ * FROM "+ MarketName + " LIMIT 300; ", connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows == false)
            {
                Console.WriteLine(" D CHAY! ");
            }
            while (await reader.ReadAsync())
            {
                int mydelay = 1;
                await Task.Delay(mydelay);
                LoadDataBinding.Add(new MarketModel(reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetFloat(3),
                    reader.GetFloat(4),
                    reader.GetFloat(5),
                    reader.GetFloat(6),
                    reader.GetFloat(7),
                    reader.GetFloat(8),
                    reader.GetInt32(9)));
            }
            reader.Close();  

        }

    }
}
