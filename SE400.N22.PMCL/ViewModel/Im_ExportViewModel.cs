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
        public int selectedProduct { get; set; }

        public RelayCommand SaveCommand { get; set; }
        public Im_ExportViewModel(MySqlConnection connection)
        {
            selectedProduct = -1;
            today = (DateTime.Now).ToShortDateString();
            isExport = false;
            listProduct = new ObservableCollection<ProductModel>();
            this.connection = connection;
            getProductData();
            SaveCommand = new RelayCommand(o => { Save(); });
        }
        public async void Save(object o = null)
        {
            string format = "yyyy-MM-dd HH:mm:ss";
            string temp = isExport ? "export_rp" : "import_rp";
            try
            {
                MySqlCommand cmd = new MySqlCommand("Insert into " + temp + " (date, pid, amount) values (\"" + DateTime.Parse(today).ToString(format) + "\", \"" + listProduct[selectedProduct].id + "\",\"" + amount + "\");", connection);
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
    }
}
