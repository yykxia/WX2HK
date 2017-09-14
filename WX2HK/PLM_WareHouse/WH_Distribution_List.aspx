<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WH_Distribution_List.aspx.cs" Inherits="WX2HK.PLM_WareHouse.WH_Distribution_List" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>模塑仓库-配送查询</title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" runat="server">
            <Regions>
                <f:Region RegionPosition="Left" Width="300px" runat="server" Layout="Fit">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" EnableRowClickEvent="true" OnRowClick="Grid1_RowClick">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <f:TriggerBox ID="trgb_sjdh" runat="server" OnTriggerClick="trgb_sjdh_TriggerClick"
                                            TriggerIcon="Search" Label="订单号">
                                        </f:TriggerBox>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="SourceNo" HeaderText="领料单号" ExpandUnusedSpace="true"></f:BoundField>
                                <f:BoundField DataField="distCount" HeaderText="配送合计" TextAlign="Center" Width="80px"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region RegionPosition="Center" runat="server" Layout="Fit">
                    <Items>
                        <f:Grid ID="Grid2" runat="server" OnRowCommand="Grid2_RowCommand"
                             DataKeyNames="id,strorageId" EnableCheckBoxSelect="true">                           
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="btn_remove" runat="server" Text="移除"
                                             ConfirmText="确认移除当前选中数据？" OnClick="btn_remove_Click"></f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="StorageCode" HeaderText="库位" Width="100px"></f:BoundField>
                                <f:BoundField DataField="CntrCode" HeaderText="货架号" Width="100px"></f:BoundField>
                                <f:BoundField DataField="BoundQty" HeaderText="出库数量" TextAlign="Center" Width="80px"></f:BoundField>
                                <f:BoundField DataField="addTime" HeaderText="添加时间" DataFormatString="{0:MM-dd HH:mm}" Width="120px"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
    </form>
</body>
</html>
