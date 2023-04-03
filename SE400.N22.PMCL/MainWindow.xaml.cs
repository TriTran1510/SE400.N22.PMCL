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

namespace SE400.N22.PMCL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        MySqlConnection connection;
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

            lsStockName.ItemsSource = dataTable.Rows.OfType<DataRow>()
            .Select(dr => dr.Field<String>("Tables_in_MarketStock")).ToList();

            this.Chart();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Chart()
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
            Labels = new[] { "2015", "2016", "2017" };
            Formatter = value => value.ToString("C");

            SeriesCollection.Add(new LineSeries
            {
                Title="Hola",Values = new ChartValues<double> { 5,3,2},
                LineSmoothness=0,
                PointGeometry= Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize=50,
                PointForeground=Brushes.Green,

            });
            SeriesCollection[3].Values.Add(5d);
        }

        private void Closebtn_Click(object sender, RoutedEventArgs e)
        {
            connection.Close();
            this.Close();
        }

        public SeriesCollection SeriesCollection { get; set; }

        public String[] Labels { get; set; }

        public Func<double,string> Formatter { get; set; }

        private void Minimizebtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void OnRadioClick(object sender, RoutedEventArgs e)
        {
            RadioButton ck = sender as RadioButton;
            if (ck.IsChecked.Value)
            {
                MarketControl marketcontrol = new MarketControl(ck.Content.ToString(), connection);
                this.DataContext = marketcontrol;
            }
            
        }
        private void scrollMenu_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            scv.ScrollToHorizontalOffset(scv.HorizontalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
