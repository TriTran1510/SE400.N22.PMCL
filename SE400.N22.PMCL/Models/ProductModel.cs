using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE400.N22.PMCL.Models
{
    internal class ProductModel
    {
        public ProductModel(string id, string productName, string productType, string productTypeID, string description="")
        {
            this.id = id;
            this.productName = productName;
            this.productType = productType;
            this.description = description;
            this.productTypeID = productTypeID;
        }
        public string id { get; set; }
        public string productName { get; set; }
        public string productType { get; set; }
        public string productTypeID { get; set; }
        public string description { get; set; }

    }
}
