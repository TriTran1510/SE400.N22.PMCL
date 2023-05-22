using MySql.Data.MySqlClient;
using SE400.N22.PMCL.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE400.N22.PMCL.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand StockViewCommand { get; set; }
        public RelayCommand ProductsViewCommand { get; set; }
        public RelayCommand ProductTypeViewCommand { get; set; }
        public RelayCommand Im_ExportViewCommand { get; set; }

        public Im_ExportViewModel Im_ExportVM { get; set; }
        public ProductTypeViewModel ProductTypeVM { get; set; }
        public ProductsViewModel ProductsVM { get; set; }

        private object _currentView;
        private MySqlConnection connection { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value; 
                OnPropertyChanged();
            }  
        }
        public MainViewModel()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TiDBConnectionString"].ConnectionString;
            connection = new MySqlConnection(connectionString);
            connection.Open();
 
            ProductsVM = new ProductsViewModel(connection);
            ProductTypeVM = new ProductTypeViewModel(connection);
            Im_ExportVM = new Im_ExportViewModel(connection);
            CurrentView = ProductsVM;

            ProductsViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProductsVM;
                ProductTypeVM = new ProductTypeViewModel(connection);
                Im_ExportVM = new Im_ExportViewModel(connection);
            });

            ProductTypeViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProductTypeVM;
                ProductsVM = new ProductsViewModel(connection);
                Im_ExportVM = new Im_ExportViewModel(connection);
            });

            Im_ExportViewCommand = new RelayCommand(o =>
            {
                CurrentView = Im_ExportVM;
                ProductsVM = new ProductsViewModel(connection);
                ProductTypeVM = new ProductTypeViewModel(connection);
            });
        }
    }
}
