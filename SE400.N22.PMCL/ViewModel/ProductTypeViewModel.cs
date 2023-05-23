using MySql.Data.MySqlClient;
using SE400.N22.PMCL.Core;
using SE400.N22.PMCL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SE400.N22.PMCL.ViewModel
{
    internal class ProductTypeViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string? productTypeName { get; set; }
        public string? description { get; set; }
        private MySqlConnection connection { get; set; }
        public ObservableCollection<ProductTypeModel> listProductType { get; set; }
        public int selectedProductType { get; set; }
        private bool isUpdate { get; set; }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand SelectionChanged { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        public ProductTypeViewModel(MySqlConnection connection)
        {
            this.connection = connection;
            isUpdate = false;
            selectedProductType = -1;
            SaveCommand = new RelayCommand(o => { Save(); });
            SelectionChanged = new RelayCommand(o => { selectionChanged(); });
            listProductType = new ObservableCollection<ProductTypeModel>();
            DeleteCommand = new RelayCommand(o => { delete(); });
            getData();
        }
        public async void delete(object o = null)
        {
            MySqlCommand cmd = new MySqlCommand("Begin;\nDelete from producttype where id="+ listProductType[selectedProductType].id +";", connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (await reader.ReadAsync())
            {

            }
            await reader.CloseAsync();
            await Task.Delay(10000);
            MySqlCommand cmd2 = new MySqlCommand("Commit;", connection);
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            while (await reader2.ReadAsync())
            {

            }
            await reader2.CloseAsync();
            reload();
        }
        public void reload()
        {
            listProductType.Clear();
            getData();
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(listProductType)));
        }
        public void selectionChanged(object o = null)
        {
            isUpdate = true;
            productTypeName = listProductType[selectedProductType].productTypeName;
            description = listProductType[selectedProductType].description;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(productTypeName)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(description)));
        }

        public async void Save(object o = null)
        {
            if (!isUpdate)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("Begin;\nInsert into producttype (type, description) values (\"" + productTypeName + "\",\"" + description + "\");", connection);
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
                await Task.Delay(5000);
                MySqlCommand cmd2 = new MySqlCommand("Commit;", connection);
                MySqlDataReader reader2 = cmd2.ExecuteReader();
                while (await reader2.ReadAsync())
                {

                }
                await reader2.CloseAsync();
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand("Begin;\nUpdate producttype set type=\"" + productTypeName + "\", description=\"" + description + "\" where id=" + listProductType[selectedProductType].id + ";", connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (await reader.ReadAsync())
                {

                }
                await reader.CloseAsync();
                await Task.Delay(5000);
                MySqlCommand commit = new MySqlCommand("Commit;", connection);
                MySqlDataReader commitReader2 = commit.ExecuteReader();
                while (await commitReader2.ReadAsync())
                {

                }
                await commitReader2.CloseAsync();
                isUpdate = true;               
            }
            reload();
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
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
