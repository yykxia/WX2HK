<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_WX_ProductCount.aspx.cs" Inherits="WX2HK.PLM.PLM_WX_ProductCount" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta name="viewport" content="maximum-scale=1.0,minimum-scale=1.0,user-scalable=0,width=device-width,initial-scale=1.0" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>模塑车间-生产统计</title>
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
             EnableSummary="true" SummaryPosition="Flow" DataKeyNames="id">
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb1">
                    <Items>
                        <f:RadioButtonList ID="rdbtn1" runat="server" AutoPostBack="true" Width="300px"
                             OnSelectedIndexChanged="rdbtn1_SelectedIndexChanged">
                            <f:RadioItem Text="昨日" Value="day" Selected="true" />
                            <f:RadioItem Text="上周" Value="week" />
                            <f:RadioItem Text="上月" Value="month" />
                        </f:RadioButtonList>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:BoundField DataField="lineName" HeaderText="产线" Width="100px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="bindSum" ColumnID="bindSum" HeaderText="产量" Width="80px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="sumC" ColumnID="sumC" HeaderText="白班" Width="80px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="sumD" ColumnID="sumD" HeaderText="夜班" Width="80px" TextAlign="Center"></f:BoundField>
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
