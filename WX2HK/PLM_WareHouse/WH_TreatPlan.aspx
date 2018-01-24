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
                <f:Region RegionPosition="Top" runat="server" Layout="Fit" Height="400px" RegionSplit="true">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" EnableRowClickEvent="true"
                             OnRowClick="Grid1_RowClick" DataKeyNames="OnlineId">
                            <Toolbars>
                                <f:Toolbar ID="tlb1" runat="server">
                                    <Items>
                                        <f:ToolbarFill ID="tlbf1" runat="server"></f:ToolbarFill>
                                        <f:Button ID="btn_commit" runat="server" Text="设定"
                                            OnClick="btn_commit_Click"></f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="OrderNo" Width="120px" HeaderText="订单号"></f:BoundField>
                                <f:BoundField DataField="ItemNo" Width="160px" HeaderText="物料编码"></f:BoundField>
                                <f:BoundField DataField="remainQty" Width="60px" TextAlign="Center" HeaderText="待处理"></f:BoundField>
                                <f:BoundField DataField="minProdTime" Width="130px" TextAlign="Center"
                                     DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="最早产出时间"></f:BoundField>
                                <f:BoundField DataField="ItemParm" ExpandUnusedSpace="true" HeaderText="规格"></f:BoundField>
                                <%--<f:LinkButtonField CommandName="LevelUp" DataTextField=""></f:LinkButtonField>--%>
                                <f:CheckBoxField RenderAsStaticField="false" TextAlign="Center"
                                    ColumnID="LevelUp" DataField="levelUp" HeaderText="优先处理"></f:CheckBoxField>
                            </Columns>

                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region RegionPosition="Center" runat="server" Layout="HBox">
                    <Items>
                        <f:Grid ID="Grid_wating" runat="server" Title="待处理库位" Width="400px"
                             DataKeyNames="Id" EnableMultiSelect="true" EnableCheckBoxSelect="true">
                            <Toolbars>
                                <f:Toolbar ID="tlb2" runat="server">
                                    <Items>
                                        <f:DropDownList ID="DDL_Emple" Label="人员"
                                            runat="server" EnableEdit="true"></f:DropDownList>
                                        <f:Button ID="Btn_Pick" runat="server" Text="手动分配"
                                            OnClick="Btn_Pick_Click"></f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
<%--                                <f:CheckBoxField RenderAsStaticField="false" TextAlign="Center" 
                                    Width="40px" ColumnID="cb_field"></f:CheckBoxField>--%>
                                <f:BoundField DataField="StorageCode" Width="80px" TextAlign="Center" HeaderText="库位"></f:BoundField>
                                <f:BoundField DataField="CntrCode" Width="80px" TextAlign="Center" HeaderText="货架"></f:BoundField>
                                <f:BoundField DataField="BoundQty" Width="80px" TextAlign="Center" HeaderText="数量"></f:BoundField>
                                <f:BoundField DataField="CreateTime" Width="130px" TextAlign="Center"
                                     DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="生产时间"></f:BoundField>
                            </Columns>
                        </f:Grid>

                        <f:Grid ID="Grid_doing" runat="server" BoxFlex="1" Title="正在处理" 
                            OnRowCommand="Grid_doing_RowCommand" DataKeyNames="Id">
                            <Columns>
                                <f:BoundField DataField="StorageCode" Width="80px" TextAlign="Center" HeaderText="库位"></f:BoundField>
                                <f:BoundField DataField="CntrCode" Width="80px" TextAlign="Center" HeaderText="货架"></f:BoundField>
                                <f:BoundField DataField="BoundQty" Width="80px" TextAlign="Center" HeaderText="数量"></f:BoundField>
                                <f:BoundField DataField="CreateTime" Width="130px" TextAlign="Center"
                                     DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="生产时间"></f:BoundField>
                                <f:BoundField DataField="WorkerNo" HeaderText="人员" TextAlign="Center" Width="80px"></f:BoundField>
                                <f:LinkButtonField DataTextField="Remove" Width="60px" CommandName="Remove"></f:LinkButtonField>
                                <f:LinkButtonField DataTextField="Done" Width="60px" CommandName="Done"
                                    ConfirmText="是否确认完工？"></f:LinkButtonField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
    </form>
</body>
</html>
