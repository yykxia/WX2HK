<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_ProdConfirm.aspx.cs" Inherits="WX2HK.PLM.PLM_ProdConfirm" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />     
    <f:Grid ID="Grid1" runat="server" Title="生产计划确认"
         EnableCheckBoxSelect="true" DataKeyNames="Id,OrderId" OnRowCommand="Grid1_RowCommand">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:DatePicker ID="DatePicker1" runat="server" Label="库存业务日期" Required="true"></f:DatePicker>
                    <f:Button ID="btn_refresh" runat="server" Text="刷新" OnClick="btn_refresh_Click"></f:Button>
                    <f:Button ID="btn_confirm" Text="批量确认" runat="server" ConfirmText="是否确认入库？" OnClick="btn_confirm_Click"></f:Button>
<%--                    <f:RadioButtonList ID="rbt_query" Label="按完工日期筛选" runat="server" LabelWidth="120px" Width="400px"
                         AutoPostBack="true" OnSelectedIndexChanged="rbt_query_SelectedIndexChanged">
                        <f:RadioItem Text="3天前" Value="3" />
                        <f:RadioItem Text="2天前" Value="2" />
                        <f:RadioItem Text="1天前" Value="1" />
                    </f:RadioButtonList>--%>
                    <f:ToolbarSeparator ID="tlbs1" runat="server"></f:ToolbarSeparator>
                    <f:CheckBox ID="cb_others" Text="未完工" runat="server"
                         AutoPostBack="true" OnCheckedChanged="cb_others_CheckedChanged"></f:CheckBox>
                    <f:Label ID="label1" runat="server" 
                        Text="（<span style='color:red;'>截止该业务日期当天夜班的产量</span>）" EncodeText="false"></f:Label>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Columns>
            <f:BoundField DataField="orderNo" HeaderText="订单号" Width="150px"></f:BoundField>
            <f:BoundField DataField="itemNo" HeaderText="物料编号" Width="170px"></f:BoundField>
            <f:BoundField DataField="itemParm" HeaderText="规格" Width="200px"></f:BoundField>
            <f:BoundField DataField="BindQty" HeaderText="产出" Width="80px" TextAlign="Center"></f:BoundField>
            <f:BoundField DataField="RecordQty" HeaderText="ERP已入库" Width="100px" TextAlign="Center"></f:BoundField>
            <f:BoundField DataField="ConfirmQty" HeaderText="本次入库" Width="80px" TextAlign="Center"></f:BoundField>
            <f:BoundField DataField="planCount" HeaderText="计划数" Width="80px" TextAlign="Center"></f:BoundField>
            <f:BoundField DataField="BoundQty" HeaderText="仓库确认数量" Width="100px" TextAlign="Center"></f:BoundField>
<%--            <f:BoundField DataField="status" HeaderText="当前状态" Width="80px" TextAlign="Center"></f:BoundField>--%>
<%--            <f:BoundField DataField="endTime" HeaderText="完工日期" DataFormatString="{0:MM-dd HH:mm}" Width="100px" TextAlign="Center"></f:BoundField>--%>
            <f:LinkButtonField HeaderText="&nbsp;" Width="80px" Icon="PageWhiteText"
                 ToolTip="详情" CommandName="bindSerials" />
        </Columns>
    </f:Grid>

    </form>
    <f:Window ID="Window1" Width="500px" Height="400px" Title="上架详情"
        EnableCollapse="true" runat="server" EnableResize="false" Layout="Fit"
        IsModal="false" AutoScroll="true" EnableClose="true" Hidden="true">
        <Items>
            <f:Grid ID="Grid2" runat="server">
                <Columns>
                    <f:BoundField DataField="BarCode" Width="100px" TextAlign="Center" HeaderText="货架"></f:BoundField>
                    <f:BoundField DataField="BindQty" Width="80px" TextAlign="Center" HeaderText="数量"></f:BoundField>
                    <f:BoundField DataField="CreateTime" Width="150px" 
                        DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="上架时间"></f:BoundField>
                </Columns>
            </f:Grid>
        </Items>
    </f:Window>
</body>
</html>
