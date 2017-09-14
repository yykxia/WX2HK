<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WH_StorageBoard.aspx.cs" Inherits="WX2HK.PLM_WareHouse.WH_StorageBoard" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
            <div id="div_header" style="height: 5%; background-color: #FFFFFF; overflow: hidden; font-size: xx-large; color: #000099; font-family: 楷体;">B3模塑仓库库位展示图（三楼）</div>
            <div id="div_WH" style="float: left; width: 25%; height: 90%;" overflow: hidden;">
                <asp:Table ID="Table_WH" runat="server" Width="100%">
                </asp:Table>
            </div>
            <div id="div_WQ" style="float: left; width: 25%; height: 90%; overflow: hidden;">
                <asp:Table ID="Table_WQ" runat="server" Width="100%"></asp:Table>
            </div>
            <div id="div_EQ" style="float: left; width: 25%; height: 90%; overflow: hidden;">
                <asp:Table ID="Table_EQ" runat="server" Width="100%"></asp:Table>
            </div>
            <div id="div_EH" style="float: left; width: 25%; height: 90%; overflow: hidden;">
                <asp:Table ID="Table_EH" runat="server" Width="100%"></asp:Table>
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
            if (labelWidth > divWidth)
            {
                runScroll();
            }
            function runScroll() {
                t_annc = setInterval("show_annc()", 10);
            }
            function show_annc() {
                div_anncInfo.scrollLeft += 1;
                if (div_anncInfo.scrollLeft > (scrollLength-1)) {
                    setTimeout(function () {
                        div_anncInfo.scrollLeft = 0;
                    }, 5000); //延迟5秒
                }
            }

            var rollInterval = 1500;
            //WQ
            var sum = 16; //sum为未滚动前显示的行数
            var divHeight_WQ = document.getElementById('div_WQ').clientHeight;
            var table_WQ = document.getElementById('<%=Table_WQ.ClientID%>');
            var rows_WQ = table_WQ.rows.length;//table行数
            if ($("#<%=Table_WQ.ClientID%>").height() > divHeight_WQ) { //divHeight_WQ为DIV的高度
                scroll();
            }
            function scroll() {
                t_WQ = setInterval("show()", rollInterval) //1000滚动时间间隔为1秒 如要改动，下面1000也要跟着改动
            }
            function show() {
                $("#<%=Table_WQ.ClientID%> tr:first").appendTo($("#<%=Table_WQ.ClientID%>"));
                sum = sum + 1;
                //if (sum == rows_WQ) {         //rows_WQ为table数据总行数
                //    setTimeout(function () {
                //        window.location.reload();
                //    }, 500); //500为滚动到最后一条之后0.5秒后刷新页面
                //}
            }
            //WH
            var sum_WH = 16;
            var divHeight_WH = document.getElementById('div_WH').clientHeight;
            var table_WH = document.getElementById('<%=Table_WH.ClientID%>');
            var rows_WH = table_WH.rows.length;//table行数
            if ($("#<%=Table_WH.ClientID%>").height() > divHeight_WH) { //divHeight_WH为DIV的高度
                scroll_WH();
            }
            function scroll_WH() {
                t_WH = setInterval("show_WH()", rollInterval) //1000滚动时间间隔为1秒 如要改动，下面1000也要跟着改动
            }
            function show_WH() {
                $("#<%=Table_WH.ClientID%> tr:first").appendTo($("#<%=Table_WH.ClientID%>"));
                sum_WH = sum_WH + 1;
                //if (sum_WH == rows_WH) {         //rows_WH为table数据总行数
                //    setTimeout(function () {
                //        window.location.reload();
                //    }, 500); //500为滚动到最后一条之后0.5秒后刷新页面
                //}
            }
            //EQ
            var sum_EQ = 16;
            var divHeight_EQ = document.getElementById('div_EQ').clientHeight;
            var table_EQ = document.getElementById('<%=Table_EQ.ClientID%>');
            var rows_EQ = table_EQ.rows.length;//table行数
            if ($("#<%=Table_EQ.ClientID%>").height() > divHeight_EQ) { //divHeight_EQ为DIV的高度
                scroll_EQ();
            }
            function scroll_EQ() {
                t_WH = setInterval("show_EQ()", rollInterval) //1000滚动时间间隔为1秒 如要改动，下面1000也要跟着改动
            }
            function show_EQ() {
                $("#<%=Table_EQ.ClientID%> tr:first").appendTo($("#<%=Table_EQ.ClientID%>"));
                sum_EQ = sum_EQ + 1;
                //if (sum_EQ == rows_EQ) {         //rows_EQ为table数据总行数
                //    setTimeout(function () {
                //        window.location.reload();
                //    }, 500); //500为滚动到最后一条之后0.5秒后刷新页面
                //}
            }

            //EH
            var sum_EH = 16;
            var divHeight_EH = document.getElementById('div_EH').clientHeight;
            var table_EH = document.getElementById('<%=Table_EH.ClientID%>');
            var rows_EH = table_EH.rows.length;//table行数
            if ($("#<%=Table_EH.ClientID%>").height() > divHeight_EH) { //divHeight_EH为DIV的高度
                scroll_EH();
            }
            function scroll_EH() {
                t_EH = setInterval("show_EH()", rollInterval) //1000滚动时间间隔为1秒 如要改动，下面1000也要跟着改动
            }
            function show_EH() {
                $("#<%=Table_EH.ClientID%> tr:first").appendTo($("#<%=Table_EH.ClientID%>"));
                sum_EH = sum_EH + 1;
                //if (sum_EH == rows_EH) {         //rows_EH为table数据总行数
                //    setTimeout(function () {
                //        window.location.reload();
                //    }, 500); //500为滚动到最后一条之后0.5秒后刷新页面
                //}
            }
        </script></body>
</html>
