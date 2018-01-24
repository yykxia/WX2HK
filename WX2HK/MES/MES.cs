using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WX2HK.MES
{
    public class OperateLogs
    {
        public string 容器 { get; set; }
        public int 数量 { get; set; }
        public string 产出时间 { get; set; }//生产时间
        public string 卸绵时间 { get; set; }//卸绵时间
    }

    public class CntrOrderInfo
    {
        public string ItemNo { get; set; }//物料信息
        public string ItemName { get; set; }//物料名称
        public string ItemPram { get; set; }//物料规格
        public int ItemCount { get; set; }//数量
        public List<OperateLogs> Ops { get; set; }
        public List<StorageObj> ObjList { get; set; }
    }

    /// <summary>
    /// 库存对象
    /// </summary>
    public class StorageObj
    {
        public string OrderNo { get; set; }
    }
}