using MySql.Data.MySqlClient;
using SE400.N22.PMCL.Core;
using SE400.N22.PMCL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SE400.N22.PMCL.ViewModel
{
    internal class ProductTypeViewModel
    {
        public string? productTypeName { get; set; }
        public string? description { get; set; }
        private MySqlConnection connection { get; set; }
        public ObservableCollection<ProductTypeModel> listProductType { get; set; }

        public RelayCommand SaveCommand { get; set; }

        public ProductTypeViewModel(MySqlConnection connection)
        {
            this.connection = connection;
            SaveCommand = new RelayCommand(o => { Save(); });
            listProductType = new ObservableCollection<ProductTypeModel>();
        }

        public async void Save(object o = null)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("Insert into producttype (type, description) values (\"" + productTypeName + "\",\"" + description + "\");", connection);
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
        public async void getData()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM warehouse.producttype", connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (await reader.ReadAsync())
            {
                listProductType.Add(new ProductTypeModel(reader.GetString(0),reader.GetString(1), reader.GetString(2)));
            }
            await reader.CloseAsync();
        }
    }
}
