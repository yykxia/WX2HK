<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_Print_WHCode.aspx.cs" Inherits="WX2HK.PLM.PLM_Print_WHCode" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="../js/LodopFuncs.js"></script>
    <script src="../js/jquery-3.1.1.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="Label1" runat="server" Text="货位大区:"></asp:Label>
            <input id="areaCode" type="text">
        <asp:Label ID="Label2" runat="server" Text="二级区域"></asp:Label>
        <input id="middleArea" type="text" />
        <asp:Label ID="Label3" runat="server" Text="库号"></asp:Label>
        <input id="numb" type="text" /><br /><br /><br />
        打印长度:<input id="tx_width" type="text">CM  宽度:<input id="tx_height" type="text">CM
        <asp:Button ID="btn_print" runat="server" Text="打印" OnClientClick="javascript:PreviewMytable();" /><br />
<%--        <asp:Image ID="img_whCode" runat="server" />
        <div>
        <asp:Label ID="Label4" runat="server" Text="货位大区:"></asp:Label>
            <asp:TextBox ID="txb_Area" runat="server"></asp:TextBox>
        <asp:Label ID="Label5" runat="server" Text="二级区域"></asp:Label>
        <asp:TextBox ID="txb_MidArea" runat="server"></asp:TextBox>
        <asp:Label ID="Label6" runat="server" Text="库号"></asp:Label>
        <asp:TextBox ID="txb_numb" runat="server"></asp:TextBox>
        <asp:Button ID="btn_build" runat="server" Text="生成条码" OnClick="btn_build_Click" />
        </div>--%>
    </form>
    <script>
        var LODOP; //声明为全局变量
        
        function PreviewMytable() {
            var printWidth = $("#tx_width").val();//打印宽度
            var printHeight = $("#tx_height").val();//打印高度
            var printBarCode = $("#areaCode").val() + $("#middleArea").val() + $("#numb").val();//条码
            var A4Weight = 21;//A4纸宽度
            var A4Height = 29.8;//A4高度
            var marginTop = 3;//打印格式的上边距
            var marginLeft = (A4Height - printWidth) / 2;//打印格式的左边距
            var marginTop_Text = marginTop + printHeight;//打印格式的上边距
            var marginLeft_Text = (A4Height - 5) / 2;//打印格式的左边距
            LODOP = getLodop(); 
            LODOP.PRINT_INIT("打印库位条码");
            LODOP.SET_PRINT_PAGESIZE(2, 0, 0, "A4");
            LODOP.ADD_PRINT_BARCODE("15cm", marginLeft + 'cm', printWidth + 'cm', printHeight + 'cm', "128Auto", printBarCode);
            LODOP.SET_PRINT_STYLE("FontSize", 40);
            LODOP.SET_PRINT_STYLE("Bold", true);
            LODOP.ADD_PRINT_TEXT(marginTop_Text + 'cm', marginLeft_Text + 'cm', 5 + 'cm', 4 + 'cm', printBarCode);
            LODOP.PRINT();
        }
    </script>
</body>
</html>
