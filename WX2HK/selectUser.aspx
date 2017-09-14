<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selectUser.aspx.cs" Inherits="WX2HK.selectUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <%--        <f:Grid ID="Grid2" BoxFlex="1" AutoScroll="true" ShowBorder="true" ShowHeader="true" Title="人员明细" runat="server"
            DataKeyNames="userid" ShowGridHeader="false">
            <Columns>
                <f:BoundField DataField="name" DataFormatString="{0}" Width="80px"
                    HeaderText="姓名" />
                <f:BoundField ExpandUnusedSpace="true" DataField="mobile" DataFormatString="{0}"
                    HeaderText="手机号" />
            </Columns>
        </f:Grid>--%>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false"
                    Width="200px" Position="Left" Layout="Fit" runat="server">
                    <Items>
                        <f:Grid ID="Grid2" AutoScroll="true" ShowBorder="true" ShowHeader="true" Title="人员明细" runat="server"
                            DataKeyNames="userid" ShowGridHeader="false" EnableRowClickEvent="true" OnRowClick="Grid2_RowClick">
                            <Toolbars>
                                <f:Toolbar runat="server">
                                    <Items>
                                        <f:TriggerBox ID="tgbox1" runat="server" ShowLabel="false" TriggerIcon="Search"
                                             EmptyText="输入姓名查询" OnTriggerClick="tgbox1_TriggerClick"></f:TriggerBox>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="name" DataFormatString="{0}" Width="80px"
                                    HeaderText="姓名" />
                                <f:BoundField ExpandUnusedSpace="true" DataField="mobile" DataFormatString="{0}"
                                    HeaderText="手机号" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center"
                    Layout="Fit" BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" Title="已选" runat="server" DataKeyNames="userid" RowHeight="20px"
                             ShowBorder="false" AutoScroll="true" ShowGridHeader="false" OnRowCommand="Grid1_RowCommand">
                            <Columns>
                                <f:BoundField DataField="name" ExpandUnusedSpace="true" />
                                <f:LinkButtonField CommandName="Delete" Width="40px" Icon="Delete" TextAlign="Center" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb_bottom" Position="Bottom">
                    <Items>
                        <f:Button ID="btn_return" runat="server" Icon="ArrowUndo" Text="返回" OnClick="btn_return_Click"></f:Button>
                        <f:Button ID="btn_sub" runat="server" Icon="UserAdd" Text="确认" OnClick="btn_sub_Click"></f:Button>

                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:RegionPanel>
    </form>
</body>
</html>
