using MySql.Data.MySqlClient;
using SE400.N22.PMCL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE400.N22.PMCL.ViewModel
{
    internal class StockViewModel
    {
        private MySqlConnection connection { get; set; }

        public StockViewModel(MySqlConnection connection)
        {
            this.connection = connection;
        }

    }
}
