using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;
using SE400.N22.PMCL.Control;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace SE400.N22.PMCL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlConnection connection;
        
        List<String> lsBank = new List<String>();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            string connectionString = ConfigurationManager.ConnectionStrings["TiDBConnectionString"].ConnectionString;
            connection = new MySqlConnection(connectionString);
            connection.Open();

            // Execute SQL queries or commands here
            MySqlCommand cmd = new MySqlCommand("SHOW TABLES", connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

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
        
        private void scrollMenu_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            scv.ScrollToHorizontalOffset(scv.HorizontalOffset - e.Delta);
            e.Handled = true;
        }

        private Boolean checkAvailable(List<String> lsStock,string name)
        {
            foreach (String s in lsStock)
            {
                if (name == s) { return true; }
            }
            return false;
        }

        private void SbBank_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                if(checkAvailable(lsBank,SbBank.Text.ToString().ToUpper()) == true)
                {
                    MarketControl marketcontrol = new MarketControl(SbBank.Text.ToUpper(), connection);
                    this.DataContext = marketcontrol;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Not available for you right now!","Notification", MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }
    }
}
