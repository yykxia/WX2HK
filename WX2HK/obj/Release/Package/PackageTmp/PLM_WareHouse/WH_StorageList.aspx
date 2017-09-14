<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WH_StorageList.aspx.cs" Inherits="WX2HK.PLM_WareHouse.WH_StorageList" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>模塑仓库管理-货位明细</title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" EnableCollapse="true"
                    Width="200px" RegionPosition="Left" Layout="Fit" RegionSplit="true"
                    runat="server">
                    <Items>
                        <f:Grid ID="Grid2" ShowBorder="true" ShowHeader="true" Title="库位列表" runat="server"
                            DataKeyNames="StorageCode" EnableTextSelection="true" 
                            EnableRowClickEvent="true" OnRowClick="Grid2_RowClick">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Toolbars>
                                <f:Toolbar ID="toolbar3" runat="server">
                                    <Items>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="StorageCode" HeaderText="库位号" 
                                    TextAlign="Center" ExpandUnusedSpace="true"></f:BoundField>
                                <f:BoundField DataField="AvailablePos" HeaderText="存量" 
                                    TextAlign="Center" Width="60px"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center"
                    Layout="Fit" BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" Title="库位明细" OnRowCommand="Grid1_RowCommand"
                             DataKeyNames="StorageId,OnlineId,isLocked">
                            <Columns>
                                <f:BoundField DataField="orderNo" HeaderText="订单号" Width="100px"></f:BoundField>
                                <f:BoundField DataField="BoundQty" TextAlign="Center" HeaderText="数量" Width="80px"></f:BoundField>
                                <f:BoundField DataField="CntrCode" TextAlign="Center" HeaderText="货架" Width="80px"></f:BoundField>
                                <f:BoundField DataField="ItemTech" HeaderText="工艺要求" Width="150px"></f:BoundField>
                                <f:BoundField DataField="itemParm" HeaderText="规格" ExpandUnusedSpace="true"></f:BoundField>
                                <f:LinkButtonField HeaderText="异常处理" TextAlign="Center" 
                                    CommandName="remove" Width="80px" Text="移除"></f:LinkButtonField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>

        </f:RegionPanel>
    </form>
        <f:Window ID="Window1" Width="400px" Height="300px" Title="移除原因" Hidden="true"
            EnableCollapse="true" runat="server" EnableResize="false" Layout="Fit"
            IsModal="false" AutoScroll="true" EnableClose="true" Target="Parent">
            <Items>
                <f:SimpleForm ID="SimpleForm1" runat="server" BodyPadding="10px" LabelAlign="Top">
                    <Items>
                        <f:Label ID="label_info" runat="server" Label="货架信息"></f:Label>
                        <f:RadioButtonList ID="rdbt_specType" runat="server"
                             Required="true" Label="处理类型">
                            <f:RadioItem Text="配送异常" Value="1" />
                            <f:RadioItem Text="转储异常" Value="2" />
                            <f:RadioItem Text="其他" Value="3" />
                            </f:RadioButtonList>
                        <f:TextArea ID="txa_others" runat="server" Label="其他描述"></f:TextArea>
                    </Items>
                </f:SimpleForm>
            </Items>
            <Toolbars>
                <f:Toolbar ID="tlb_commit" runat="server" Position="Bottom">
                    <Items>
                        <f:Button ID="btn_confirm" runat="server" Text="确认" ValidateForms="SimpleForm1"
                                OnClick="btn_confirm_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:Window>
</body>
</html>
