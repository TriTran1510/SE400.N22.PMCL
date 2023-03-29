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
using System.Windows.Markup;
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
                MySqlCommand cmd = new MySqlCommand("SHOW TABLES", connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridCustomers.DataContext = dataTable;

                lsStockName.ItemsSource = dataTable.Rows.OfType<DataRow>()
                .Select(dr => dr.Field<String>("Tables_in_MarketStock")).ToList();

            }
        }

        private void Closebtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Minimizebtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void scrollMenu_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
