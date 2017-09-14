<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WH_offLine.aspx.cs" Inherits="WX2HK.PLM_WareHouse.WH_offLine" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:Timer ID="Timer1" Interval="60" Enabled="true" OnTick="Timer1_Tick" EnableAjaxLoading="false" runat="server">
        </f:Timer>
        <f:RegionPanel ID="RegionPanel1" runat="server">
            <Regions>
                <f:Region runat="server" ID="Region1" Title="下线订单" RegionPosition="Left" Layout="Fit" Width="200px">
                    <%--                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:TriggerBox runat="server" ID="tgb_queryStr" TriggerIcon="Search"
                                     OnTriggerClick="tgb_queryStr_TriggerClick"></f:TriggerBox>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>--%>
                    <Items>
                        <f:Grid ID="Grid1" runat="server" DataKeyNames="TradeNo"
                            EnableRowClickEvent="true" OnRowClick="Grid1_RowClick">
                            <Columns>
                                <f:BoundField DataField="orderNo" HeaderText="订单号" ExpandUnusedSpace="true"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" runat="server" RegionPosition="Center" Layout="VBox" Title="订单详情">
                    <Items>
                        <f:Form ID="orderInfo" runat="server" BodyPadding="10px" BoxFlex="1">
                            <Rows>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox runat="server" Label="物料编码" ID="txb_itemNo" Readonly="true"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox runat="server" Label="订单号" ID="txb_workNo" Readonly="true"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox runat="server" Label="品名" ID="txb_itemName" Readonly="true"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextArea runat="server" Label="规格" ID="TextArea_parm" Readonly="true"></f:TextArea>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox runat="server" Label="工艺要求" ID="txb_itemTech" Readonly="true"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:NumberBox ID="numb_planCount" runat="server" Label="订单总数" Readonly="true"></f:NumberBox>
                                        <f:NumberBox ID="numb_prodCount" runat="server" Label="生产数量" Readonly="true"></f:NumberBox>
                                        <f:NumberBox ID="numb_TransferCount" runat="server" Label="已入库" Readonly="true"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
<%--                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="Label1" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>--%>
                            </Rows>
                        </f:Form>
                        <f:Image CssClass="photo" ID="Image_product" runat="server" Height="350px">
                        </f:Image>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
    </form>
</body>
</html>
