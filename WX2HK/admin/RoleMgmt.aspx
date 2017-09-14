<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleMgmt.aspx.cs" Inherits="WX2HK.admin.RoleMgmt" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>角色管理</title>
    <style type="text/css">
        .label_hid {
            display:none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />
        <f:Grid ID="Grid1" runat="server" ShowHeader="false" OnRowCommand="Grid1_RowCommand"
            DataKeyNames="id" AllowCellEditing="true" ClicksToEdit="2">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btn_addNew" runat="server" Text="新增"></f:Button>
                        <f:Button ID="btn_save" runat="server" Text="保存编辑" OnClick="btn_save_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:RenderField DataField="roleName" ColumnID="roleName" TextAlign="Center" HeaderText="角色名称" Width="200px">
                    <Editor>
                        <f:TextBox ID="tbxEditorRoleName" Required="true" runat="server">
                        </f:TextBox>
                    </Editor>                
                </f:RenderField>
                <f:RenderField DataField="roleDesc" ColumnID="roleDesc" TextAlign="Center" HeaderText="角色描述" ExpandUnusedSpace="true">
                    <Editor>
                        <f:TextBox ID="tbxEditorRoleDesc" runat="server">
                        </f:TextBox>
                    </Editor>                
                </f:RenderField>
                <f:RenderCheckField Width="100px" ColumnID="UseStatus" DataField="UseStatus" HeaderText="是否启用" />
                <f:LinkButtonField HeaderText="&nbsp;" Width="80px" ConfirmText="删除选中行？" ConfirmTarget="Top"
                    CommandName="Delete" Icon="Delete" ColumnID="Grid1_ctl15" />                            
            </Columns>
        </f:Grid>
        <f:Label ID="label_hidden" runat="server" CssClass="label_hid"></f:Label>
    </form>
</body>
</html>
