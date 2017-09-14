<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_OnlineSet_ChangeLine.aspx.cs" Inherits="WX2HK.PLM.PLM_OnlineSet_ChangeLine" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="SimpleForm1" />
        <f:SimpleForm ID="SimpleForm1" runat="server" ShowHeader="false">
            <Items>
                <f:Label ID="Label1" runat="server" Label="原产线"></f:Label>
                <f:DropDownList ID="ddl_line" Label="调整为"
                        runat="server"></f:DropDownList>
            </Items>
            <Toolbars>
                <f:Toolbar ID="tlb1" runat="server" Position="Top">
                    <Items>
                        <f:Button ID="btn_ok" runat="server" Text="确认" OnClick="btn_ok_Click"></f:Button>
                        <f:Button ID="btn_exit" runat="server" Text="退出" OnClick="btn_exit_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:SimpleForm>
    </form>
</body>
</html>
