using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WX2HK.Ent
{    
    public class ERP_Orders
    {
        public Order[] orders { get; set; }
    }

    public class Order
    {
        public string DDLX { get; set; }
        public string XSTD_TDLS { get; set; }
        public string XSTD_TDBH { get; set; }
        public string XSTD_YWRQ { get; set; }
        public string XSTD_SPKH { get; set; }
        public string XSTD_SPKHMC { get; set; }
        public string XSTD_C9 { get; set; }
        public string XSTD_C10 { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string XSTDMX_TDLS { get; set; }
        public string XSTDMX_TDFL { get; set; }
        public string XSTDMX_WLBH { get; set; }
        public string XSTDMX_ZSL { get; set; }
        public string XSTDMX_BZHSJ { get; set; }
        public string XSTDMX_BHSE { get; set; }
    }

}