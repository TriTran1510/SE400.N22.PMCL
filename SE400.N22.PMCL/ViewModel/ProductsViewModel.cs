﻿using MySql.Data.MySqlClient;
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
    internal class ProductsViewModel
    {
        public string? productName { get; set; }
        public string? description { get; set; }
        public string? productType { get; set; }
        private MySqlConnection connection { get; set; }
        public ObservableCollection<ProductTypeModel> listProductType { get; set; }
        public ObservableCollection<ProductModel> listProduct { get; set; }

        public RelayCommand SaveCommand { get; set; }
        public int SelectedProductType { get; set; }
        public ProductsViewModel(MySqlConnection connection)
        {
            this.connection = connection;
            SaveCommand = new RelayCommand(o => { Save(); });
            listProductType = new ObservableCollection<ProductTypeModel>();
            listProduct = new ObservableCollection<ProductModel>();
            SelectedProductType = -1;
            getProductTypeData();
            getProductData();
        }

        public async void Save(object o = null)
        {
            foreach(var pro in listProduct)
            {
                if (pro.productName == productName)
                {
                    MySqlCommand cmd = new MySqlCommand("Begin;\nUpdate product set type=\"" + listProductType[SelectedProductType].id + "\", description=\""+description+"\" where id="+pro.id+";", connection);
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
                    return;
                }
            }
            try
            {
                MySqlCommand cmd = new MySqlCommand("Begin;\nInsert into product (name, type, description) values (\"" + productName + "\", \"" + listProductType[SelectedProductType].id + "\",\"" + description + "\");", connection);
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
        public async void getProductTypeData()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM warehouse.producttype", connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (await reader.ReadAsync())
            {
                listProductType.Add(new ProductTypeModel(reader.GetString(0),reader.GetString(1), reader.GetString(2)));
            }
            await reader.CloseAsync();
        }
        public async void getProductData()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT product.id, product.name, producttype.type, product.description FROM warehouse.product INNER JOIN producttype on product.type = producttype.id;", connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (await reader.ReadAsync())
            {
                listProduct.Add(new ProductModel(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
            }
            await reader.CloseAsync();
        }
    }
}
