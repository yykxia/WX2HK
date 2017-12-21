using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WX2HK.Ent
{
    public class MES_Department
    {
        public string DeptNo { get; set; }//部门编码
        public string DeptName { get; set; }//部门名称
        public List<MES_DeptLines> Lines { get; set; }//产线
    }
}