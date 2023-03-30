
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE400.N22.PMCL.Control
{
    internal class MarketControl
    {
        public string MarketName { get; set; }
        private MySqlConnection connection;

        public MarketControl(string marketName, MySqlConnection connection)
        {
            MarketName = marketName;
            this.connection = connection;
            OnMarketNameChanged();
        }
        private void OnMarketNameChanged()
        {
            switch (MarketName)
            {
                case "ADANIPORTS":
                    break;
                case "ASIANPAINT":
                    //TEST COMMAND
                    MySqlCommand cmd = new MySqlCommand("select /*+ read_from_storage(TIFLASH[ASIANPAINT]) */ date FROM ASIANPAINT GROUP BY date ORDER BY date; ", connection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows==false)
                    {
                        Console.WriteLine(" D CHAY! ");
                    }
                    while (reader.Read())
                    {
                        MarketName += reader.GetString(0);
                    }
                    break;
            }
        }
    }
}
