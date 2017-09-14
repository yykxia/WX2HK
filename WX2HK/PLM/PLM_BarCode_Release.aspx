<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_BarCode_Release.aspx.cs" Inherits="WX2HK.PLM.PLM_BarCode_Release" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />     
    <f:Grid ID="Grid1" runat="server" Title="在线订单汇总" DataKeyNames="tradeNo">
        <Toolbars>
            <f:Toolbar runat="server">
                <Items>
                    <f:RadioButtonList ID="rbt_query" Label="按完工日期筛选" runat="server" LabelWidth="120px" Width="400px"
                         AutoPostBack="true" OnSelectedIndexChanged="rbt_query_SelectedIndexChanged">
                        <f:RadioItem Text="3天前" Value="3" />
                        <f:RadioItem Text="2天前" Value="2" />
                        <f:RadioItem Text="1天前" Value="1" />
                    </f:RadioButtonList>
                    <f:ToolbarFill runat="server"></f:ToolbarFill>
                    <f:Button ID="btn_refresh" runat="server" Text="刷新全部" OnClick="btn_refresh_Click"></f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Columns>
            <f:BoundField DataField="orderNo" HeaderText="订单号" Width="150px"></f:BoundField>
            <f:BoundField DataField="itemNo" HeaderText="物料编号" Width="150px"></f:BoundField>
            <f:BoundField DataField="itemParm" HeaderText="规格" Width="200px"></f:BoundField>
            <f:BoundField DataField="planCount" HeaderText="计划" Width="80px" TextAlign="Center"></f:BoundField>
            <f:BoundField DataField="bindTotal" HeaderText="产出" Width="80px" TextAlign="Center"></f:BoundField>
            <f:BoundField DataField="bindSum" HeaderText="线上" Width="80px" TextAlign="Center"></f:BoundField>
            <f:BoundField DataField="offLineCount" HeaderText="入库" Width="80px" TextAlign="Center"></f:BoundField>
            <f:BoundField DataField="status" HeaderText="当前状态" Width="80px" TextAlign="Center"></f:BoundField>
            <f:BoundField DataField="endTime" HeaderText="完工日期" DataFormatString="{0:MM-dd HH:mm}" Width="100px" TextAlign="Center"></f:BoundField>
            <f:WindowField ColumnID="WindowField1" Width="80px" WindowID="Window1" HeaderText="操作"
                Icon="Pencil" ToolTip="点击查看" DataTextFormatString="{0}" DataIFrameUrlFields="tradeNo"
                DataIFrameUrlFormatString="PLM_Release_Edit.aspx?OnlineId={0}" />
        </Columns>
    </f:Grid>
    <f:Window ID="Window1" Title="上架明细" Hidden="true" EnableIFrame="true" runat="server"
        EnableMaximize="true" EnableResize="true" Target="Parent"
        IsModal="False" Width="600px" Height="450px">
    </f:Window>
    </form>
</body>
</html>
