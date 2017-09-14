<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_ProdComfirm_His.aspx.cs" Inherits="WX2HK.PLM.PLM_ProdComfirm_His" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />     
        <f:Grid ID="Grid1" runat="server" Title="入库明细查询" EnableTextSelection="true">
            <Toolbars>
                <f:Toolbar ID="tlb1" runat="server">
                    <Items>
                        <f:DatePicker ID="DatePicker1" runat="server" Label="业务日期" Required="true"></f:DatePicker>
                        <f:Button ID="btn_query" runat="server" Text="查询" Icon="SystemSearch" OnClick="btn_query_Click"></f:Button>
                        <f:Button ID="Button1" EnableAjax="false" DisableControlBeforePostBack="false"
                                runat="server" Text="导出为Excel文件" Icon="PageExcel" OnClick="Button1_Click">
                            </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:BoundField ColumnID="SCDDCP_JHPCH" DataField="SCDDCP_JHPCH" 
                    runat="server" Width="200px" HeaderText="订单号"></f:BoundField>
                <f:BoundField ColumnID="ItemNo" DataField="ItemNo"
                    runat="server" Width="200px" HeaderText="物料编码"></f:BoundField>
                <f:BoundField ColumnID="LSWLEX_C9" DataField="LSWLEX_C9" TextAlign="Center"
                    runat="server" Width="100px" HeaderText="克重"></f:BoundField>
                <f:BoundField ColumnID="LSWLZD_GGXH" DataField="LSWLZD_GGXH"
                    runat="server" ExpandUnusedSpace="true" HeaderText="汇总属性"></f:BoundField>
                <f:BoundField ColumnID="SCDDCP_JHTCSL" DataField="SCDDCP_JHTCSL" 
                    runat="server" Width="100px" TextAlign="Center" HeaderText="计划数"></f:BoundField>
                <f:BoundField ColumnID="BoundQty" DataField="BoundQty"
                     runat="server" Width="100px" TextAlign="Center" HeaderText="本期入库"></f:BoundField>
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
