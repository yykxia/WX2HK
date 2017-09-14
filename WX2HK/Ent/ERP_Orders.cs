using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WX2HK.Ent
{
    
    public class ERP_Orders
    {
        public order[] orders { get; set; }
    }

    public class order
    {
        public string DDLX { get; set; } //订单类型：01：零售订单 02：经销商订单
        public string XSDD_DDLS { get; set; }
        public string XSDD_DDBH { get; set; }
        public string XSDD_SPKH { get; set; }
        public string XSDD_DJRQ { get; set; }
        public string DDZE { get; set; }
        public string XSFPMX_ZE { get; set; }
        public string XSDD_SPKHMC { get; set; }
        public string XSDD_C9 { get; set; }
        public string XSDD_C10 { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string XSDDMX_DDLS { get; set; }
        public string XSDDMX_DDFL { get; set; }
        public string XSDDMX_WLBH { get; set; }
        public string XSDDMX_BZHSJ { get; set; }
        public string XSDDMX_ZSL { get; set; }
        public string TDSL { get; set; }
    }

}