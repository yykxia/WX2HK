<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="barCodePrint.aspx.cs" Inherits="WX2HK.others.barCodePrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>定制车间-条码打印</title>
    <script type="text/javascript" src="../js/LodopFuncs.js"></script>
    <script src="../js/jquery.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 10px; margin-right: 0px; margin-bottom: 0px; margin-left: 10px;">
<%--            宽度：<input id="printWidth" type="text" value="50" />mm<br />
            高度：<input id="printHeight" type="text" value="40" />mm<br /><br />--%>
            <f:PageManager ID="PageManager1" runat="server" />
            <f:SimpleForm ID="SimpleForm1" runat="server" BodyPadding="5px" Width="500px">
                <Items>
                    <f:GroupPanel ID="GroupPanel_printerSet" runat="server" Title="打印机设置">
                        <Items>
                        <f:TextBox ID="txb_printWidth" runat="server" Label="宽度(mm)" Text="50"></f:TextBox>
                        <f:TextBox ID="txb_printHeight" runat="server" Label="高度(mm)" Text="40"></f:TextBox>
                        </Items>
                    </f:GroupPanel>
                    <f:GroupPanel ID="GroupPanel1" runat="server" Title="数据相关">
                        <Items>
                            <f:DatePicker ID="DatePicker1" runat="server" EnableEdit="false" AutoPostBack="true"
                                OnTextChanged="DatePicker1_DateSelect" Label="打印日期"></f:DatePicker>
                            <f:TextBox ID="txb_curBarCode" runat="server" Label="起始条码"></f:TextBox>
                            <f:NumberBox ID="nmb_printCount" runat="server" Label="条码数量" NoDecimal="true"></f:NumberBox>
                        </Items>
                    </f:GroupPanel>
                </Items>
                <Toolbars>
                    <f:Toolbar runat="server" ID="tlb1" Position="Bottom">
                        <Items>
                            <f:Button ID="btn_print" runat="server" Text="打印" OnClick="btn_print_Click"></f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
            </f:SimpleForm>
<%--            条码范围：<input style="border:1px solid" id="stratNumb" type="text" /> 至 
            <input style="border:1px solid" id="endNumb" type="text" /><br /><br />
            <input type="button" value="打印" onclick="print();" />--%>
            <f:HiddenField ID="hiddenValue" runat="server"></f:HiddenField>
        </div>
    </form>
    <script>
        var LODOP; //声明为全局变量 
        function print() {
            try {
                LODOP = getLodop();
                var printWidth = $('#<%=txb_printWidth.ClientID %>-inputEl').val();//纸张宽度
                var printHeight = $('#<%=txb_printHeight.ClientID %>-inputEl').val();//纸张高度
                var startNumb = $('#<%=txb_curBarCode.ClientID %>-inputEl').val();//起始条码
                var endNumb = $('#<%=nmb_printCount.ClientID %>-inputEl').val();//最大条码
                if (isInteger(endNumb)) {
                    LODOP.PRINT_INIT("条码打印");
                    for (var barCode = 0; barCode < endNumb; barCode++) {
                        LODOP.ADD_PRINT_BARCODE("5mm", "5mm", "40mm", "30mm", "", Number(startNumb) + Number(barCode));
                        LODOP.SET_PRINT_PAGESIZE(1, printWidth, printHeight, "");
                        LODOP.PRINT();
                    }
                } else {
                    alert("条码范围不是有效整数！");
                    return;
                }

                location.reload();

            } catch (e) {
                alert(e.name + ": " + e.message);
            }
        }

        function isInteger(obj) {
            return !isNaN(obj) && obj != "";
        }
        
    </script>
</body>
</html>
