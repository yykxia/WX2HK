﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Code128_WH : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string num = Request["num"].ToString();
        //string num = "KM20110715002";
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        System.Drawing.Image myimg = BarCodeHelper.MakeBarcodeImage_WH(num,6,true,300);
        myimg.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        Response.ClearContent();
        Response.ContentType = "image/Gif";
        Response.BinaryWrite(ms.ToArray());
        Response.End();

    }
}
