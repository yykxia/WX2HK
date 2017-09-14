<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OverTime_leaderView.aspx.cs" Inherits="WX2HK.CheckOut.OverTime_leaderView" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="maximum-scale=1.0,minimum-scale=1.0,user-scalable=0,width=device-width,initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>加班申请-明细</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="SimpleForm1" />
        <f:SimpleForm ID="SimpleForm1" BodyPadding="5px" LabelWidth="100px" AutoScroll="true"
            runat="server" ShowBorder="True" ShowHeader="false"
            Title="加班内容">
            <Items>
                <f:Label ID="label_dept" runat="server" Label="部门"></f:Label>
                <f:Label ID="label_name" runat="server" Label="申请人"></f:Label>
                <f:Label ID="label_reqTime" runat="server" Label="提交时间"></f:Label>
                <f:Label ID="label_status" runat="server" Label="当前状态"></f:Label>
                <f:Label ID="label_personDtl" runat="server" Label="加班人员明细"></f:Label>
                <f:Label ID="label_startTime" runat="server" Label="起始时间"></f:Label>
                <f:Label ID="label_endTime" runat="server" Label="截至时间"></f:Label>
                <f:Label ID="label_hours" runat="server" Label="加班时长"></f:Label>
                <f:Label ID="label_desc" runat="server" Label="补充说明"></f:Label>
<%--                <f:Label runat="server" ID="label_hidden" Hidden="true"></f:Label>--%>
<%--                <f:ContentPanel ID="content1" runat="server" ShowHeader="false" AutoScroll="true">
                    <asp:Image ID="Image1" runat="server" Height="200px" />
                </f:ContentPanel>--%>
            </Items>
            <Toolbars>
                <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                    <Items>
<%--                        <f:ToolbarFill runat="server"></f:ToolbarFill>--%>
                        <f:Button runat="server" ID="btnSubmit" ConfirmText="确认审批？"
                            ValidateForms="SimpleForm1" Text="同意申请" OnClick="btnSubmit_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:SimpleForm>
    </form>
</body>
</html>
