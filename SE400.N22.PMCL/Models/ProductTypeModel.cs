using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE400.N22.PMCL.Models
{
    internal class ProductTypeModel
    {
        public ProductTypeModel(string id, string productTypeName, string description)
        {
            this.id = id;
            this.productTypeName = productTypeName;
            this.description = description;
        }
        public string id { get; set; }
        public string productTypeName { get; set; }
        public string description { get; set; }

    }
}
