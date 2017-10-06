<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_classInit.aspx.cs" Inherits="WX2HK.PLM.PLM_classInit" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />
        <f:Grid ID="Grid1" runat="server" ShowHeader="false">
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb1">
                    <Items>
                        <f:DatePicker ID="DatePicker1" runat="server" Label="起始:"></f:DatePicker>
                        <f:DatePicker ID="DatePicker2" runat="server" Label="至:"></f:DatePicker>
                        <f:Button runat="server" ID="btn_refresh" Text="刷新" OnClick="btn_refresh_Click"></f:Button>
                        <f:Button runat="server" ID="btn_create" Text="生成" OnClick="btn_create_Click"></f:Button>
                        <f:ToolbarFill runat="server" ID="tlbf1"></f:ToolbarFill>
                        <f:Button runat="server" ID="btn_save" Text="保存" OnClick="btn_save_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:BoundField DataField="InitDay" Width="200px" HeaderText="日期"></f:BoundField>
                <f:CheckBoxField DataField="CheckFlag" Width="100px"
                    ColumnID="CheckBoxField1" TextAlign="Center"
                    RenderAsStaticField="false" HeaderText="是否验厂"></f:CheckBoxField>
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
