using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WX2HK.Ent
{

    public class ERP_Orders
    {
        public Order[] orders { get; set; }//订单列表
    }

    public class Order
    {
        public string XSTD_YWBH { get; set; }//销售类型
        public string XSTD_TDLS { get; set; }//流水号
        public string XSTD_TDBH { get; set; }//业务单号
        public string XSTD_YWRQ { get; set; }//业务日期
        public string XSTD_DJRQ { get; set; }//单据日期
        public string XSTD_SPKH { get; set; }//客户编号
        public string XSTD_SPKHMC { get; set; }//客户名称
        public string XSTD_C9 { get; set; }//备注（联系人 联系电话）
        public string XSTD_C10 { get; set; }//送货地址
        public Item[] items { get; set; }//产品明细
    }

    public class Item
    {
        public string XSTDMX_TDLS { get; set; }//销售流水id
        public string XSTDMX_TDFL { get; set; }//分录号
        public string XSTDMX_WLBH { get; set; }//产品编码
        public string XSTDMX_ZSL { get; set; }//销售数量
        public string XSTDMX_BZHSJ { get; set; }//销售单价
        public string XSTDMX_BHSE { get; set; }//总金额
    }


}