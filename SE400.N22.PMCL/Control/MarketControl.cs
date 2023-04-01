
using MySql.Data.MySqlClient;
using SE400.N22.PMCL.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SE400.N22.PMCL.Control
{
    public class MarketControl
    {
        public ObservableCollection<MarketModel> LoadDataBinding { get; set; }
        public string MarketName { get; set; }
        private MySqlConnection connection;


        public MarketControl(string marketName, MySqlConnection connection)
        {
            LoadDataBinding = new ObservableCollection<MarketModel>();
            MarketName = marketName;
            this.connection = connection;
            OnMarketNameChanged();
        }
        private async void OnMarketNameChanged()
        {
            switch (MarketName)
            {
                case "ASIANPAINT":
                    //TEST COMMAND
                    MySqlCommand cmd = new MySqlCommand("select /*+ read_from_storage(TIFLASH[ASIANPAINT]) */ * FROM ASIANPAINT; ", connection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows == false)
                    {
                        Console.WriteLine(" D CHAY! ");
                    }
                    while (await reader.ReadAsync())
                    {
                        LoadDataBinding.Add(new MarketModel(reader.GetDateTime(0), 
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

                    break;
            }
        }

    }
}
