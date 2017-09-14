<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_OnlineSet_CountAndAnls.aspx.cs" Inherits="WX2HK.PLM.PLM_OnlineSet_CountAndAnls" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta name="viewport" content="maximum-scale=1.0,minimum-scale=1.0,user-scalable=0,width=device-width,initial-scale=1.0" />
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
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />
        <f:Grid ID="Grid1" runat="server" ShowHeader="false" AutoScroll="true"
            EnableSummary="true" SummaryPosition="Flow" DataKeyNames="tradeNo">
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb1">
                    <Items>
                        <f:DatePicker runat="server" Required="true" EnableEdit="false" Label="起始日期" EmptyText="请选择日期"
                            ID="DatePicker1" ShowRedStar="True">
                        </f:DatePicker>
<%--                        <f:TimePicker ID="TimePicker1" EnableEdit="false" Label="时间" LabelWidth="40px" Increment="15"
                            EmptyText="请选择时间" runat="server">
                        </f:TimePicker> --%>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb2">
                    <Items>
                        <f:DatePicker runat="server" Required="true" EnableEdit="false" Label="截止日期" EmptyText="请选择日期"
                             CompareControl="DatePicker1" CompareOperator="GreaterThanEqual"
                             CompareMessage="结束日期应该大于开始日期" ID="DatePicker2" ShowRedStar="True">
                        </f:DatePicker>
<%--                        <f:TimePicker ID="TimePicker2" EnableEdit="false" Label="时间" LabelWidth="40px" Increment="15"
                            EmptyText="请选择时间" runat="server">
                        </f:TimePicker> --%>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb3">
                    <Items>
                        <f:DropDownList ID="ddl_line" Label="产线" LabelWidth="40px" 
                             runat="server">
                        </f:DropDownList>
<%--                        <f:DropDownList ID="ddl_banzhi" Label="班次" LabelWidth="40px" 
                             runat="server">
                            <f:ListItem Text="所有" Value="%" />
                            <f:ListItem Text="C（白）" Value="C" />
                            <f:ListItem Text="D（夜）" Value="D" />
                        </f:DropDownList>--%>
                        <f:CheckBox ID="ckb_onlineStatus" runat="server" Text="已完工订单"></f:CheckBox>
                    </Items>
                </f:Toolbar>
            </Toolbars>
<%--            <Toolbars>
                <f:Toolbar runat="server" ID="tlb4">
                    <Items>
                        
                    </Items>
                </f:Toolbar>
            </Toolbars>--%>
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb5">
                    <Items>
                        <f:Button ID="btn_filter" Text="查询" runat="server" Icon="SystemSearch" OnClick="btn_filter_Click"></f:Button>
                        <f:Button ID="Button1" EnableAjax="false" DisableControlBeforePostBack="false"
                                runat="server" Text="导出为Excel文件" Icon="PageExcel" OnClick="Button1_Click">
                            </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:BoundField DataField="orderNo" HeaderText="订单号" Width="150px"></f:BoundField>
                <f:BoundField DataField="itemNo" HeaderText="物料编号" Width="150px"></f:BoundField>
                <f:BoundField DataField="itemName" HeaderText="品名" Width="100px"></f:BoundField>
                <f:BoundField DataField="itemParm_size" HeaderText="规格" Width="120px"></f:BoundField>
                <f:BoundField DataField="planCount" HeaderText="订单数量" Width="100px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="itemParm_weight" HeaderText="克重" TextAlign="Center" Width="80px"></f:BoundField>
                <f:BoundField DataField="itemParm_color" HeaderText="颜色" TextAlign="Center" Width="80px"></f:BoundField>
                <f:BoundField DataField="itemParm_sfjz" HeaderText="是否卷装" TextAlign="Center" Width="80px"></f:BoundField>
                <f:BoundField DataField="bindTotal" HeaderText="累计" TextAlign="Center" Width="80px"></f:BoundField>
                <f:BoundField DataField="BindSum" ColumnID="BindSum" HeaderText="统计数量" TextAlign="Center" Width="80px"></f:BoundField>
                <f:BoundField DataField="bindSumC" ColumnID="bindSumC" HeaderText="白班" Width="80px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="bindSumD" ColumnID="bindSumD" HeaderText="夜班" Width="80px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="bindSumCS" HeaderText="吊架白" Width="80px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="bindSumDS" HeaderText="吊架夜" Width="80px" TextAlign="Center"></f:BoundField>
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
