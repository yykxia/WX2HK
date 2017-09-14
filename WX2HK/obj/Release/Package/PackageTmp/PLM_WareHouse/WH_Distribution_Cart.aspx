<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WH_Distribution_Cart.aspx.cs" Inherits="WX2HK.PLM_WareHouse.WH_Distribution_Cart" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src='http://192.168.1.16:8000/CLodopfuncs.js'></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="btn_print" runat="server" Text="直接打印" OnClientClick="javascript:PreviewMytable();" />
        <div>
            12312312312312312312
        </div>
    </form>
    <script>
        function PreviewMytable() {
            var strHTML = document.getElementsByTagName("html")[0].innerHTML;
            LODOP.PRINT_INITA(1, 1, 770, 660, "测试预览功能");
            LODOP.ADD_PRINT_TEXT(10, 60, 300, 200, "这是测试的纯文本，下面是超文本:");
            LODOP.ADD_PRINT_HTM(30, 5, "100%", "80%", strHTML);
            LODOP.PREVIEW();
        };
    </script>
</body>
</html>
