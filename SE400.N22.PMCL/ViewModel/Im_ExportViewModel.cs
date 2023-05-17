using MySql.Data.MySqlClient;
using SE400.N22.PMCL.Core;
using SE400.N22.PMCL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE400.N22.PMCL.ViewModel
{
    internal class Im_ExportViewModel
    {
        public bool isExport { get; set; }
        public string today { get; set; }
        public string amount { get; set; }
        private MySqlConnection connection { get; set; }
        public ObservableCollection<ProductModel> listProduct { get; set; }
        public ObservableCollection<ImportAndExportModel> listImportAndExportProduct { get; set; }
        public int selectedProduct { get; set; }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand OnToggleButtonClick { get; set; }
        public RelayCommand SelectionChanged { get; set; }
        public Im_ExportViewModel(MySqlConnection connection)
        {
            selectedProduct = -1;
            today = (DateTime.Now).ToShortDateString();
            isExport = false;
            listProduct = new ObservableCollection<ProductModel>();
            listImportAndExportProduct = new ObservableCollection<ImportAndExportModel>();
            this.connection = connection;
            getProductData();
            getImportAndExportProductData();
            SaveCommand = new RelayCommand(o => { Save(); });
            OnToggleButtonClick = new RelayCommand(o => { onChanged(); });
        }
        public void onChanged(object o = null)
        {
            listImportAndExportProduct.Clear();
            getImportAndExportProductData();
        }
        public async void Save(object o = null)
        {

                string format = "yyyy-MM-dd HH:mm:ss";
                string temp = isExport ? "export_rp" : "import_rp";
                try
                {
                    MySqlCommand cmd = new MySqlCommand("Begin; \nInsert into " + temp + " (date, pid, amount) values (\"" + DateTime.Parse(today).ToString(format) + "\", \"" + listProduct[selectedProduct].id + "\",\"" + amount + "\");\nCommit;", connection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (await reader.ReadAsync())
                    {

                    }
                    await reader.CloseAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
        }
        public async void getProductData()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM warehouse.product", connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (await reader.ReadAsync())
            {
                listProduct.Add(new ProductModel(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(4)));
            }
            await reader.CloseAsync();
        }
        public async void getImportAndExportProductData()
        {
            if (!isExport)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT import_rp.id, import_rp.date, product.name, import_rp.amount FROM warehouse.import_rp INNER JOIN product on import_rp.pid = product.id;", connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    listImportAndExportProduct.Add(new ImportAndExportModel(reader.GetString(0), reader.GetDateTime(1), reader.GetString(2), reader.GetInt32(3)));
                }
                await reader.CloseAsync();
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand("SELECT export_rp.id, export_rp.date, product.name, export_rp.amount FROM warehouse.export_rp INNER JOIN product on export_rp.pid = product.id;", connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    listImportAndExportProduct.Add(new ImportAndExportModel(reader.GetString(0), reader.GetDateTime(1), reader.GetString(2), reader.GetInt32(3)));
                }
                await reader.CloseAsync();
            }
        }
    }
}
