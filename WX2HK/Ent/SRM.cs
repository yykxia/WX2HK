using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WX2HK.Ent
{
    public class SRM
    {

        public class RemainSend
        {
            public string supplierNo { get; set; }
            public int remainCounts { get; set; }
            public Item[] Items { get; set; }
        }

        public class Item
        {
            public string contactNo { get; set; }
            public string orderNo { get; set; }
            public string materialNo { get; set; }
            public string materialName { get; set; }
            public double contactCount { get; set; }
            public double curCount { get; set; }
            public string deliveryDate { get; set; }
        }

    }
}