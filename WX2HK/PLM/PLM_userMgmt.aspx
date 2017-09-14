<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_userMgmt.aspx.cs" Inherits="WX2HK.PLM.PLM_userMgmt" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" />
        <f:Grid ID="Grid1" runat="server" ShowHeader="false"
             EnableCheckBoxSelect="true" DataKeyNames="id">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btn_addNew" runat="server" Text="新增"></f:Button>
                        <f:Button ID="btn_unUse" runat="server" Text="删除" OnClick="btn_unUse_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:BoundField DataField="chineseName" HeaderText="姓名" Width="100px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="userId" HeaderText="工号" Width="100px" TextAlign="Center"></f:BoundField>
<%--                <f:BoundField DataField="oprtLevel" HeaderText="操作权限" Width="150px" TextAlign="Center"></f:BoundField>--%>
                <f:WindowField ColumnID="WindowField1" Width="80px" WindowID="Window1" HeaderText="编辑"
                    Icon="Pencil" ToolTip="编辑" DataTextFormatString="{0}" DataIFrameUrlFields="userId,userType"
                    DataIFrameUrlFormatString="PLM_userMgmt_Edit.aspx?userId={0}" />
            </Columns>
        </f:Grid>
        <f:Label ID="label_hidden" runat="server" CssClass="label_hid"></f:Label>
        <f:Window ID="Window1" Title="编辑" Hidden="true" EnableIFrame="true" runat="server"
            EnableMaximize="true" EnableResize="true" Target="Parent"
            IsModal="False" Width="600px" Height="450px">
        </f:Window>
    </form>
</body>
</html>
