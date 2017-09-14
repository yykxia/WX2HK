<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WH_TreatCount_New.aspx.cs" Inherits="WX2HK.PLM_WareHouse.WH_TreatCount_New" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        .x-grid-row-summary .x-grid-cell-inner {
            font-weight: bold;
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit">
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb1">
                    <Items>
                        <f:DatePicker runat="server" Required="true" EnableEdit="false"
                             Label="起始日期" EmptyText="请选择日期"
                            ID="DatePicker1" ShowRedStar="True">
                        </f:DatePicker>
                        <f:TimePicker ID="TimePicker1" runat="server" Increment="30" EnableEdit="true"></f:TimePicker>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb2" >
                    <Items>
                        <f:DatePicker runat="server" Required="true" EnableEdit="false"
                             Label="截止日期" EmptyText="请选择日期"
                            CompareControl="DatePicker1" CompareOperator="GreaterThanEqual"
                            CompareMessage="结束日期应该大于开始日期" ID="DatePicker2" ShowRedStar="True">
                        </f:DatePicker>
                        <f:TimePicker ID="TimePicker2" runat="server" Increment="30" EnableEdit="true"></f:TimePicker>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb3">
                    <Items>
                        <f:DropDownList ID="ddl_class" Label="班次"
                            runat="server">
                            <f:ListItem Text="C" Value="C" />
                            <f:ListItem Text="D" Value="D" />
                        </f:DropDownList>
                        <f:DropDownList ID="ddl_group" Label="班组" LabelAlign="Right"
                            runat="server">
                            <f:ListItem Text="A" Value="A" />
                            <f:ListItem Text="B" Value="B" />
                        </f:DropDownList>
                        <f:Button ID="btn_filter" Text="查询" runat="server" Icon="SystemSearch" OnClick="btn_filter_Click"></f:Button>
<%--                        <f:Button ID="Button1" EnableAjax="false" DisableControlBeforePostBack="false"
                            runat="server" Text="导出为Excel文件" Icon="PageExcel" OnClick="Button1_Click">
                        </f:Button>--%>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Grid runat="server" ID="Grid1" Title="统计结果" EnableSummary="true"
                     SummaryPosition="Flow" DataKeyNames="tradeNo">
                    <Columns>
                        <f:BoundField DataField="OrderNo" Width="100px" HeaderText="订单号"></f:BoundField>
                        <f:BoundField DataField="ItemNo" Width="180px" HeaderText="物料编码"></f:BoundField>
                        <f:BoundField DataField="ItemParm" Width="300px" HeaderText="规格"></f:BoundField>
                        <f:BoundField DataField="BindQty" ColumnID="BindQty"
                             Width="80px" TextAlign="Center" HeaderText="数量"></f:BoundField>
                        <f:BoundField DataField="SQty" ColumnID="SQty"
                             Width="80px" TextAlign="Center" HeaderText="吊挂总数"></f:BoundField>
                        <f:BoundField DataField="BQty" ColumnID="BQty"
                             Width="80px" TextAlign="Center" HeaderText="推车总数"></f:BoundField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>

    </form>
</body>
</html>
