<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WH_TreatPlan.aspx.cs" Inherits="WX2HK.PLM_WareHouse.WH_TreatPlan" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" runat="server">
            <Regions>
                <f:Region RegionPosition="Left" runat="server" Layout="Fit" Width="600px" RegionSplit="true">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" EnableRowClickEvent="true"
                             OnRowClick="Grid1_RowClick" DataKeyNames="OnlineId">
                            <Columns>
                                <f:BoundField DataField="OrderNo" Width="120px" HeaderText="订单号"></f:BoundField>
                                <f:BoundField DataField="ItemNo" Width="160px" HeaderText="物料编码"></f:BoundField>
                                <f:BoundField DataField="remainQty" Width="60px" TextAlign="Center" HeaderText="待处理"></f:BoundField>
                                <f:BoundField DataField="minProdTime" Width="130px" TextAlign="Center"
                                     DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="最早产出时间"></f:BoundField>
                                <f:BoundField DataField="ItemParm" ExpandUnusedSpace="true" HeaderText="规格"></f:BoundField>
                            </Columns>

                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region RegionPosition="Center" runat="server" Layout="Fit">
                    <Items>
                        <f:Grid ID="Grid2" runat="server" Title="库位明细">
                            <Columns>
                                <f:BoundField DataField="StorageCode" Width="80px" TextAlign="Center" HeaderText="库位"></f:BoundField>
                                <f:BoundField DataField="CntrCode" Width="80px" TextAlign="Center" HeaderText="货架"></f:BoundField>
                                <f:BoundField DataField="BoundQty" Width="80px" TextAlign="Center" HeaderText="数量"></f:BoundField>
                                <f:BoundField DataField="CreateTime" Width="130px" TextAlign="Center"
                                     DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="生产时间"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
    </form>
</body>
</html>
