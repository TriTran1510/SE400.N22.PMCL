using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace SE400.N22.PMCL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["TiDBConnectionString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                
                    connection.Open();
                    // Execute SQL queries or commands here
                    MySqlCommand cmd = new MySqlCommand("SELECT high,low from MarketStock.TECHM limit 10;", connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataSet dataTable = new DataSet();
                    adapter.Fill( dataTable, "LoadDataBinding");
                    dataGridCustomers.DataContext = dataTable;
                
            }
        }
    }
}
