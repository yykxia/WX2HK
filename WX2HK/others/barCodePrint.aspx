<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="barCodePrint.aspx.cs" Inherits="WX2HK.others.barCodePrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="../js/LodopFuncs.js"></script>
    <script src="../js/jquery-3.1.1.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            宽度：<input id="printWidth" type="text" value="80" />mm<br />
            高度：<input id="printHeight" type="text" value="70" />mm<br /><br />
            条码范围：<input style="border:1px solid" id="stratNumb" type="text" /> 至 
            <input style="border:1px solid" id="endNumb" type="text" /><br /><br />
            <input type="button" value="打印" onclick="print();" />
        </div>
    </form>
    <script>
        var LODOP; //声明为全局变量 
        function print() {
            try {
                LODOP = getLodop();
                var printWidth = $("#printWidth").val();//纸张宽度
                var printHeight = $("#printHeight").val();//纸张高度
                var startNumb = $("#stratNumb").val();//起始条码
                var endNumb = $("#endNumb").val();//最大条码
                if (isInteger(startNumb) && isInteger(endNumb)) {
                    LODOP.PRINT_INIT("条码打印");
                    for (var barCode = startNumb; barCode <= endNumb; barCode++) {
                        LODOP.ADD_PRINT_BARCODE("5mm", "5mm", "60mm", "30mm", "", barCode);
                        LODOP.SET_PRINT_PAGESIZE(1, printWidth, printHeight, "");
                        LODOP.PRINT();
                    }
                } else {
                    alert("条码范围不是有效整数！");
                    return;
                }
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
