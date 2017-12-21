using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WX2HK.Ent
{
    public class WMS_GetMaterials
    {
        public int Result { get; set; }
        public Material Material { get; set; }
    }

    public class Material
    {
        public string Name { get; set; }
        public string No { get; set; }
        public string Unit { get; set; }
        public string Type { get; set; }
        public string Remark { get; set; }
    }

    public class MaterialType
    {
        public string TypeNo { get; set; }
        public string TypeName { get; set; }
        public string ParentType { get; set; }
    }
}