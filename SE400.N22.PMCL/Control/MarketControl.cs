
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
using System.Windows.Controls;
using System.Windows.Input;
using SE400.N22.PMCL.Command;

namespace SE400.N22.PMCL.Control
{
    public class MarketControl: INotifyPropertyChanged
    {
        public ObservableCollection<MarketModel> LoadDataBinding { get; set; }
        public ObservableCollection<String> LoadListYear { get; set; }
        public Boolean isLoaded { get; set; }
        public int SelectedYear { get; set; }
        public string MarketName { get; set; }

        private MySqlConnection connection;
        public SeriesCollection SeriesCollection { get; set; }

        public List<String> Labels { get; set; }

        public Func<double, string> Formatter { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        public ICommand onYearChanged { get; set; }

        public MarketControl(string marketName, MySqlConnection connection)
        {
            LoadDataBinding = new ObservableCollection<MarketModel>();
            LoadListYear = new ObservableCollection<String>();
            isLoaded = false;
            SelectedYear = 0;
            MarketName = marketName;
            this.connection = connection;
            OnMarketNameChanged();
            onYearChanged = new RelayCommand(null, p=>onYearChangedCommand());
        }

        public async void Chart(String Market, String year)
        {
            MySqlCommand cmd = new MySqlCommand("Alter table " + MarketName + " SET tiflash replica 1;" +
                        "\nselect /*+ read_from_storage(TIFLASH[" + MarketName + "]) */ date,close FROM " + MarketName + " WHERE year(date) = " + year + " ;", connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<String> lsdate = new List<String>();
            ChartValues<float> lsPrices = new ChartValues<float>();
            if (reader.HasRows == false)
            {
                Console.WriteLine(" D CHAY! ");
            }
            while (await reader.ReadAsync())
            {
                DateTime dt = DateTime.Parse(reader.GetString(0));
                lsdate.Add(dt.ToString("dd/MM/yyyy"));
                lsPrices.Add(reader.GetFloat(1));
            }
            //Create chart
            SeriesCollection = new SeriesCollection()
            {
                new LineSeries
                {
                    Title = "Year " + year,Values = lsPrices
                }
            };
            Labels = lsdate;
            Formatter = value => value.ToString("C");
            OnPropertyChanged(new PropertyChangedEventArgs("SeriesCollection"));
            OnPropertyChanged(new PropertyChangedEventArgs("Labels"));
            reader.Close();
        }

        private async void OnMarketNameChanged()
        {
            getYear(MarketName);
            isLoaded = true;
            
            Chart(MarketName, LoadListYear[SelectedYear]);
            MySqlCommand cmd = new MySqlCommand("Alter table "+ MarketName + " SET tiflash replica 1;" +
                        "\nselect /*+ read_from_storage(TIFLASH[" + MarketName + "]) */ * FROM "+ MarketName + " ORDER BY date ASC LIMIT 300; ", connection);
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
        private async void getYear(string marketName)
        {
            MySqlCommand cmd = new MySqlCommand("select year(date) from " + marketName + " group by year(date) order by year(date) asc;", connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (await reader.ReadAsync())
            {
                string year = reader.GetString(0);
                LoadListYear.Add(year);
            }
            reader.Close();
        }

        public void onYearChangedCommand(object o = null)
        {
            Chart(MarketName, LoadListYear[SelectedYear]);
        }

    }
}
