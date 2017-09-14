<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_OffLine_WHMgmt.aspx.cs" Inherits="WX2HK.PLM.PLM_OffLine_WHMgmt" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />
        <f:Grid ID="Grid1" runat="server" ShowHeader="false" DataKeyNames="id"
             EnableCheckBoxSelect="true" EnableTextSelection="true" >
            <Toolbars>
                <f:Toolbar ID="tlb1" runat="server">
                    <Items>
                        <f:TriggerBox ID="tgb_wlbh" runat="server" OnTriggerClick="tgb_wlbh_TriggerClick" MinLength="8"
                            EmptyText="请输入物料编号筛选" TriggerIcon="Search"></f:TriggerBox>
                        <f:TriggerBox ID="tgb_orderNo" runat="server" OnTriggerClick="tgb_orderNo_TriggerClick" MinLength="8"
                            EmptyText="请输入订单号筛选" TriggerIcon="Search"></f:TriggerBox>
                    </Items>
                </f:Toolbar>
            </Toolbars>
             <Toolbars>                
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btn_print" runat="server" Text="打印下架清单" Icon="Printer" OnClick="btn_print_Click"></f:Button>
                    </Items>
                </f:Toolbar> 
            </Toolbars>
            <Columns>
                <f:BoundField DataField="orderNo" HeaderText="订单号" Width="150px"></f:BoundField>
                <f:BoundField DataField="itemNo" HeaderText="物料编号" Width="150px"></f:BoundField>
                <f:BoundField DataField="itemParm" HeaderText="规格" ExpandUnusedSpace="true"></f:BoundField>
                <f:BoundField DataField="buildTime" HeaderText="生产日期"
                     TextAlign="Center" DataFormatString="{0:yyyy-MM-dd}" Width="120px"></f:BoundField>
                <f:BoundField DataField="endTime" HeaderText="完工日期"
                     TextAlign="Center" DataFormatString="{0:yyyy-MM-dd}" Width="120px"></f:BoundField>
                <f:BoundField DataField="planCount" HeaderText="计划投产" Width="100px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="onlineSum" HeaderText="实际生产" Width="100px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="others" HeaderText="备注" TextAlign="Center" Width="100px"></f:BoundField>                                
            </Columns>
        </f:Grid>
    </form>
    <f:Window ID="Window1" Hidden="true" EnableIFrame="true" runat="server"
        EnableMaximize="true" EnableResize="true" Target="Parent"
        IsModal="False" Width="900px" Height="650px">
    </f:Window>
</body>
</html>
