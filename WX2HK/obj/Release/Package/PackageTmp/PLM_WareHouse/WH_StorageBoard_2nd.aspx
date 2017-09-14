<%@ Page Language="C#" AutoEventWireup="true" Codebehind="WH_StorageBoard_2nd.aspx.cs" Inherits="WX2HK.PLM_WareHouse.WH_StorageBoard_2nd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        table {
            height: 100%;
            font-size:xx-large;
        }
        span {
            white-space:nowrap;
        }
    </style>
    <script src="../js/jquery-3.1.1.js">
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
        <asp:Timer ID="Timer1" runat="server" Interval="30000" OnTick="Timer1_Tick" Enabled="True"></asp:Timer>
        <div style="width: 100%; height: 100%; position: absolute;background-color: #808080; overflow: hidden; left: 0px; top: 0px; right: 0px; bottom: 0px;">
            <div id="div_header" style="height: 5%; background-color: #FFFFFF; overflow: hidden; font-size: xx-large; color: #000099; font-family: 楷体;">模塑仓库库位展示图（二楼）</div>
            <div id="div_A" style="float: left; width: 16.6%; height: 90%;" overflow: hidden;">
                <asp:Table ID="Table_A" runat="server" Width="100%">
                </asp:Table>
            </div>
            <div id="div_B" style="float: left; width: 16.6%; height: 90%; overflow: hidden;">
                <asp:Table ID="Table_B" runat="server" Width="100%"></asp:Table>
            </div>
            <div id="div_C" style="float: left; width: 16.6%; height: 90%; overflow: hidden;">
                <asp:Table ID="Table_C" runat="server" Width="100%"></asp:Table>
            </div>
            <div id="div_D" style="float: left; width: 16.6%; height: 90%; overflow: hidden;">
                <asp:Table ID="Table_D" runat="server" Width="100%"></asp:Table>
            </div>
            <div id="div_L" style="float: left; width: 16.6%; height: 90%; overflow: hidden;">
                <asp:Table ID="Table_L" runat="server" Width="100%"></asp:Table>
            </div>
            <div id="div_P" style="float: left; width: 17%; height: 90%; overflow: hidden;">
                <asp:Table ID="Table_P" runat="server" Width="100%"></asp:Table>
            </div>
<%--            <script src="../js/muqruujs.js"></script>--%>
            <div id="div_annc" style="position: absolute; bottom: 0px; width: 100%; left: 0px; height: 5%; background-color: #000000; color: #FF0000; overflow:hidden;">
            <asp:Label ID="label_anuncInfo" runat="server" Font-Size="x-large"></asp:Label>
            </div>
        </div>
    </form>
        <script type="text/javascript">
            //公告滚动效果
            var div_anncInfo = document.getElementById('div_annc');
            var labelWidth = $("#<%=label_anuncInfo.ClientID%>").width();
            var divWidth = $("#div_annc").width();
            var scrollLength = labelWidth - divWidth;
            if (labelWidth > divWidth) {
                runScroll();
            }
            function runScroll() {
                t_annc = setInterval("show_annc()", 10);
            }
            function show_annc() {
                div_anncInfo.scrollLeft += 1;
                if (div_anncInfo.scrollLeft > (scrollLength - 1)) {
                    setTimeout(function () {
                        div_anncInfo.scrollLeft = 0;
                    }, 5000); //延迟5秒
                }
            }
            //A
            var sum = 16; //sum为未滚动前显示的行数
            var divHeight_A = document.getElementById('div_A').clientHeight;
            var table_A = document.getElementById('<%=Table_A.ClientID%>');
            var rows_A = table_A.rows.length;//table行数
            if ($("#<%=Table_A.ClientID%>").height() > divHeight_A) { //divHeight_A为DIV的高度
                scroll();
            }
            function scroll() {
                t_A = setInterval("show()", 1000) //1000滚动时间间隔为1秒 如要改动，下面1000也要跟着改动
            }
            function show() {
                $("#<%=Table_A.ClientID%> tr:first").appendTo($("#<%=Table_A.ClientID%>"));
                sum = sum + 1;
                //if (sum == rows_A) {         //rows_A为table数据总行数
                //    setTimeout(function () {
                //        window.location.reload();
                //    }, 500); //500为滚动到最后一条之后0.5秒后刷新页面
                //}
            }
            //B
            var sum_B = 16;
            var divHeight_B = document.getElementById('div_B').clientHeight;
            var table_B = document.getElementById('<%=Table_B.ClientID%>');
            var rows_B = table_B.rows.length;//table行数
            if ($("#<%=Table_B.ClientID%>").height() > divHeight_B) { //divHeight_B为DIV的高度
                scroll_B();
            }
            function scroll_B() {
                t_B = setInterval("show_B()", 1000) //1000滚动时间间隔为1秒 如要改动，下面1000也要跟着改动
            }
            function show_B() {
                $("#<%=Table_B.ClientID%> tr:first").appendTo($("#<%=Table_B.ClientID%>"));
                sum_B = sum_B + 1;
                //if (sum_B == rows_B) {         //rows_B为table数据总行数
                //    setTimeout(function () {
                //        window.location.reload();
                //    }, 500); //500为滚动到最后一条之后0.5秒后刷新页面
                //}
            }
            //C
            var sum_C = 16;
            var divHeight_C = document.getElementById('div_C').clientHeight;
            var table_C = document.getElementById('<%=Table_C.ClientID%>');
            var rows_C = table_C.rows.length;//table行数
            if ($("#<%=Table_C.ClientID%>").height() > divHeight_C) { //divHeight_C为DIV的高度
                scroll_C();
            }
            function scroll_C() {
                t_WH = setInterval("show_C()", 1000) //1000滚动时间间隔为1秒 如要改动，下面1000也要跟着改动
            }
            function show_C() {
                $("#<%=Table_C.ClientID%> tr:first").appendTo($("#<%=Table_C.ClientID%>"));
                sum_C = sum_C + 1;
                //if (sum_C == rows_C) {         //rows_C为table数据总行数
                //    setTimeout(function () {
                //        window.location.reload();
                //    }, 500); //500为滚动到最后一条之后0.5秒后刷新页面
                //}
            }

            //D
            var sum_D = 16;
            var divHeight_D = document.getElementById('div_D').clientHeight;
            var table_D = document.getElementById('<%=Table_D.ClientID%>');
            var rows_D = table_D.rows.length;//table行数
            if ($("#<%=Table_D.ClientID%>").height() > divHeight_D) { //divHeight_D为DIV的高度
                scroll_D();
            }
            function scroll_D() {
                t_D = setInterval("show_D()", 1000) //1000滚动时间间隔为1秒 如要改动，下面1000也要跟着改动
            }
            function show_D() {
                $("#<%=Table_D.ClientID%> tr:first").appendTo($("#<%=Table_D.ClientID%>"));
                sum_D = sum_D + 1;
                //if (sum_D == rows_D) {         //rows_D为table数据总行数
                //    setTimeout(function () {
                //        window.location.reload();
                //    }, 500); //500为滚动到最后一条之后0.5秒后刷新页面
                //}
            }

            //L
            var sum_L = 16;
            var divHeight_L = document.getElementById('div_L').clientHeight;
            var table_L = document.getElementById('<%=Table_L.ClientID%>');
            var rows_L = table_L.rows.length;//table行数
            if ($("#<%=Table_L.ClientID%>").height() > divHeight_L) { //divHeight_L为DIV的高度
                scroll_L();
            }
            function scroll_L() {
                t_L = setInterval("show_L()", 1000) //1000滚动时间间隔为1秒 如要改动，下面1000也要跟着改动
            }
            function show_L() {
                $("#<%=Table_L.ClientID%> tr:first").appendTo($("#<%=Table_L.ClientID%>"));
                sum_L = sum_L + 1;
                //if (sum_L == rows_L) {         //rows_L为table数据总行数
                //    setTimeout(function () {
                //        window.location.reload();
                //    }, 500); //500为滚动到最后一条之后0.5秒后刷新页面
                //}
            }

            //P
            var sum_P = 16;
            var divHeight_P = document.getElementById('div_P').clientHeight;
            var table_P = document.getElementById('<%=Table_P.ClientID%>');
            var rows_P = table_P.rows.length;//table行数
            if ($("#<%=Table_P.ClientID%>").height() > divHeight_P) { //divHeight_P为DIV的高度
                scroll_P();
            }
            function scroll_P() {
                t_P = setInterval("show_P()", 1000) //1000滚动时间间隔为1秒 如要改动，下面1000也要跟着改动
            }
            function show_P() {
                $("#<%=Table_P.ClientID%> tr:first").appendTo($("#<%=Table_P.ClientID%>"));
                sum_P = sum_P + 1;
                //if (sum_P == rows_P) {         //rows_P为table数据总行数
                //    setTimeout(function () {
                //        window.location.reload();
                //    }, 500); //500为滚动到最后一条之后0.5秒后刷新页面
                //}
            }
        </script>
</body>
</html>
