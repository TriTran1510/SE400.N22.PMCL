using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE400.N22.PMCL.Models
{
    internal class ImportAndExportModel
    {
        public string id { get; set; }
        public DateTime date { get; set; }
        public string product { get; set; }
        public int amount { get; set; }
        public ImportAndExportModel(string id, DateTime date, string product, int amount)
        {
            this.id = id;
            this.date = date;
            this.product = product;
            this.amount = amount;
        }
    }
}
