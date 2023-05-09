using System.Configuration;
using System.Windows;
using MySql.Data.MySqlClient;

namespace SE400.N22.PMCL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlConnection connection;
        public MainWindow()
        {
            InitializeComponent();

            // Execute SQL queries or commands here
            //MySqlCommand cmd = new MySqlCommand("SHOW TABLES", connection);
            //MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            //DataTable dataTable = new DataTable();
            //adapter.Fill(dataTable);

        }

        private void Closebtn_Click(object sender, RoutedEventArgs e)
        {
            connection.Close();
            this.Close();
        }

        private void Minimizebtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
