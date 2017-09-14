<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuMgmt.aspx.cs" Inherits="WX2HK.admin.MenuMgmt" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" />
        <f:Grid ID="Grid1" runat="server" ShowHeader="false" OnRowCommand="Grid1_RowCommand"
            DataKeyNames="id">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btn_addNew" runat="server" Text="新增"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:BoundField DataField="Title" HeaderText="菜单名称" Width="150px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="ImageUrl" HeaderText="图标路径" Width="150px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="NavigateUrl" HeaderText="页面路径" Width="200px" TextAlign="Center"></f:BoundField>
                <f:BoundField DataField="Remark" HeaderText="功能描述" ExpandUnusedSpace="true"></f:BoundField>
                <f:BoundField DataField="parentTitle" HeaderText="上级菜单" Width="150px" TextAlign="Center"></f:BoundField>
                <f:WindowField ColumnID="WindowField1" Width="80px" WindowID="Window1" HeaderText="编辑"
                    Icon="Pencil" ToolTip="编辑" DataTextFormatString="{0}" DataIFrameUrlFields="Id"
                    DataIFrameUrlFormatString="MenuEdit.aspx?id={0}" />
                <f:LinkButtonField HeaderText="&nbsp;" Width="80px" ConfirmText="删除选中行？" ConfirmTarget="Top"
                    CommandName="Delete" Icon="Delete" ColumnID="Grid1_ctl15" />                            
            </Columns>
        </f:Grid>
        <f:Label ID="label_hidden" runat="server" CssClass="label_hid"></f:Label>
        <f:Window ID="Window1" Title="编辑" Hidden="true" EnableIFrame="true" runat="server"
            EnableMaximize="true" EnableResize="true" Target="Top"
            IsModal="False" Width="600px" Height="450px">
        </f:Window>
    </form>
</body>
</html>
