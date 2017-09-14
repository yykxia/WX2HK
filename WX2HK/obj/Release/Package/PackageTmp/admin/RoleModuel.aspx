<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleModuel.aspx.cs" Inherits="WX2HK.admin.RoleModuel" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>角色模块权限管理</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" Split="true"
                    Width="200px" Position="Left" Layout="Fit"
                    BodyPadding="5px 0 5px 5px" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableRowSelectEvent="true"
                            OnRowSelect="Grid1_RowSelect" DataKeyNames="id">
                            <Columns>
                                <f:BoundField DataField="roleName" SortField="roleName" ExpandUnusedSpace="true" HeaderText="角色名称" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center"
                    Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Left" BodyPadding="5px 5px 5px 0"
                    runat="server">
                    <Items>
                        <f:Grid ID="Grid2" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                             DataKeyNames="id">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="btnGroupUpdate" Icon="GroupEdit" runat="server" Text="更新当前角色的模块权限"
                                            OnClick="btnGroupUpdate_Click">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>

                                <f:BoundField DataField="Title" HeaderText="模块标题" Width="150px" />

                                <f:CheckBoxField TextAlign="Center" ColumnID="CanRead" DataField="CanRead" HeaderText="浏览"
                                    RenderAsStaticField="false" Width="50px" />
                                <f:BoundField DataField="remark" HeaderText="功能描述" ExpandUnusedSpace="true" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
    </form>
</body>
</html>
