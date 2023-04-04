
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

namespace SE400.N22.PMCL.Control
{
    public class MarketControl
    {
        public ObservableCollection<MarketModel> LoadDataBinding { get; set; }
        public Boolean isLoaded { get; set; }
        public string MarketName { get; set; }
        private MySqlConnection connection;
        public SeriesCollection SeriesCollection { get; set; }

        public String[] Labels { get; set; }

        public Func<double, string> Formatter { get; set; }

        public MarketControl(string marketName, MySqlConnection connection)
        {
            LoadDataBinding = new ObservableCollection<MarketModel>();
            isLoaded = false;
            MarketName = marketName;
            this.connection = connection;
            OnMarketNameChanged();
        }

        public async void Chart()
        {
            SeriesCollection = new SeriesCollection(){
                new LineSeries
                {
                    Title="Stock", Values = new ChartValues<double>{400,500,600,700}
                },
                new LineSeries
                {
                    Title="Stock1", Values = new ChartValues<double>{450,550,650,750},
                    PointGeometry=null
                },
                new LineSeries
                {
                    Title="Stock2", Values = new ChartValues<double>{490,590,690,790},
                    PointGeometry=DefaultGeometries.Square,
                    PointGeometrySize=15
                }
            };
            Labels = new[] { "2015", "2016", "2017", "2018" };
            Formatter = value => value.ToString("C");

            //SeriesCollection.Add(new LineSeries
            //{
            //    Title = "Hola",
            //    Values = new ChartValues<double> { 5, 3, 2 },
            //    LineSmoothness = 0,
            //    //PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    //PointGeometrySize = 50,
            //    //PointForeground = Brushes.Green,

            //});
            //SeriesCollection[3].Values.Add(5d);
        }

        private async void OnMarketNameChanged()
        {
            Chart();
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
            isLoaded = true;
            
            //switch (MarketName)
            //{
            //    case "ASIANPAINT":
            //        //TEST COMMAND
            //        MySqlCommand cmd = new MySqlCommand("Alter table ASIANPAINT SET tiflash replica 1;" +
            //            "\nselect /*+ read_from_storage(TIFLASH[ASIANPAINT]) */ * FROM ASIANPAINT LIMIT 200; ", connection);
            //        MySqlDataReader reader = cmd.ExecuteReader();
            //        if (reader.HasRows == false)
            //        {
            //            Console.WriteLine(" D CHAY! ");
            //        }
            //        while (await reader.ReadAsync())
            //        {
            //            LoadDataBinding.Add(new MarketModel(reader.GetString(0), 
            //                reader.GetString(1), 
            //                reader.GetString(2), 
            //                reader.GetFloat(3), 
            //                reader.GetFloat(4), 
            //                reader.GetFloat(5), 
            //                reader.GetFloat(6), 
            //                reader.GetFloat(7), 
            //                reader.GetFloat(8), 
            //                reader.GetInt32(9)));
            //        }
            //        reader.Close();

            //        break;

            //    case "AXISBANK":
            //        MySqlCommand cmd1= new MySqlCommand("Alter table AXISBANK SET tiflash replica 1;" +
            //            "\nselect /*+ read_from_storage(TIFLASH[AXISBANK]) */ * FROM AXISBANK LIMIT 200; ", connection);
            //        MySqlDataReader reader1= cmd1.ExecuteReader();
            //        if (reader1.HasRows == false)
            //        {
            //            Console.WriteLine(" D CHAY! ");
            //        }
            //        while (await reader1.ReadAsync())
            //        {
            //            LoadDataBinding.Add(new MarketModel(reader1.GetString(0),
            //                reader1.GetString(1),
            //                reader1.GetString(2),
            //                reader1.GetFloat(3),
            //                reader1.GetFloat(4),
            //                reader1.GetFloat(5),
            //                reader1.GetFloat(6),
            //                reader1.GetFloat(7),
            //                reader1.GetFloat(8),
            //                reader1.GetInt32(9)));
            //        }
            //        reader1.Close();

            //        break;
            //}


        }

    }
}
