using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE400.N22.PMCL.Data
{
    public class MarketModel
    {
        public MarketModel(String date, string symbol, string series, float prev_close, float open, float high, float low, float last, float close, int volumn, float vwap = 0, long turnover = 0, int trade = 0, int deliverable_volumn = 0, float percent_deliverble = 0)
        {
            DateTime dt = DateTime.Parse(date);
            this.date = dt.ToString("dd/MM/yyyy");
            this.symbol = symbol;
            this.series = series;
            this.prev_close = prev_close;
            this.open = open;
            this.high = high;
            this.low = low;
            this.last = last;
            this.close = close;
            this.vwap = vwap;
            this.volumn = volumn;
            this.turnover = turnover;
            this.trade = trade;
            this.deliverable_volumn = deliverable_volumn;
            this.percent_deliverble = percent_deliverble;
        }

        public String date { get; set; }
        public string symbol { get; set; }
        public string series { get; set; }
        public float prev_close { get; set; }
        public float open { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float last { get; set; }
        public float close { get; set; }
        public float vwap { get; set; }
        public int volumn { get; set; }
        public long turnover { get; set; }
        public int trade { get; set; }
        public int deliverable_volumn { get; set; }
        public float percent_deliverble { get; set; }

    }
}
