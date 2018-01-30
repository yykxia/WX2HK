<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WH_TreateCount.aspx.cs" Inherits="WX2HK.PLM_WareHouse.WH_TreateCount" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" Layout="HBox">
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb1">
                    <Items>
                        <f:DatePicker runat="server" Required="true" EnableEdit="false" Label="起始日期" EmptyText="请选择日期"
                            ID="DatePicker1" ShowRedStar="True" Width="300px">
                        </f:DatePicker>
                        <f:DatePicker runat="server" Required="true" EnableEdit="false" Label="截止日期" EmptyText="请选择日期"
                            CompareControl="DatePicker1" CompareOperator="GreaterThanEqual" LabelAlign="Right"
                            CompareMessage="结束日期应该大于开始日期" ID="DatePicker2" ShowRedStar="True">
                        </f:DatePicker>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb3">
                    <Items>
                        <f:DropDownList ID="ddl_group" Label="班组" Width="300px"
                            runat="server">
                            <f:ListItem Text="A" Value="A" />
                            <f:ListItem Text="B" Value="B" />
                            <f:ListItem Text="T" Value="T" />
                        </f:DropDownList>
                        <f:DropDownList ID="ddl_WorkerNo" Label="人员" LabelWidth="40px"
                            runat="server">
                        </f:DropDownList>
                        <f:Button ID="btn_filter" Text="查询" runat="server" Icon="SystemSearch" OnClick="btn_filter_Click"></f:Button>
                        <f:Button ID="Button1" EnableAjax="false" DisableControlBeforePostBack="false"
                            runat="server" Text="导出为Excel文件" Icon="PageExcel" OnClick="Button1_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Grid ID="Grid_CB" runat="server" Title="推车白班" BoxFlex="1">
                    <Columns>
                        <f:BoundField DataField="orderNo" Width="100px" HeaderText="订单号"></f:BoundField>
                        <f:BoundField DataField="bindSumCB" Width="60px" HeaderText="数量"></f:BoundField>
                        <f:BoundField DataField="itemParm" ExpandUnusedSpace="true" HeaderText="规格"></f:BoundField>
                    </Columns>
                </f:Grid>
                <f:Grid ID="Grid_DB" runat="server" Title="推车夜班" BoxFlex="1">
                    <Columns>
                        <f:BoundField DataField="orderNo" Width="100px" HeaderText="订单号"></f:BoundField>
                        <f:BoundField DataField="bindSumDB" Width="60px" HeaderText="数量"></f:BoundField>
                        <f:BoundField DataField="itemParm" ExpandUnusedSpace="true" HeaderText="规格"></f:BoundField>
                    </Columns>
                </f:Grid>
                <f:Grid ID="Grid_CS" runat="server" Title="吊挂白班" BoxFlex="1">
                    <Columns>
                        <f:BoundField DataField="orderNo" Width="100px" HeaderText="订单号"></f:BoundField>
                        <f:BoundField DataField="bindSumCS" Width="60px" HeaderText="数量"></f:BoundField>
                        <f:BoundField DataField="itemParm" ExpandUnusedSpace="true" HeaderText="规格"></f:BoundField>
                    </Columns>
                </f:Grid>
                <f:Grid ID="Grid_DS" runat="server" Title="吊挂夜班" BoxFlex="1">
                    <Columns>
                        <f:BoundField DataField="orderNo" Width="100px" HeaderText="订单号"></f:BoundField>
                        <f:BoundField DataField="bindSum_DS" Width="60px" HeaderText="数量"></f:BoundField>
                        <f:BoundField DataField="itemParm" ExpandUnusedSpace="true" HeaderText="规格"></f:BoundField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>

    </form>
</body>
</html>
