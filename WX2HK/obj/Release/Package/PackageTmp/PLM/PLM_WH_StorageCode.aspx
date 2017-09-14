<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_WH_StorageCode.aspx.cs" Inherits="WX2HK.PLM.PLM_WH_StorageCode" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        img {
            width:120mm;
            height:120mm;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="Label3" runat="server" Text="大区:"></asp:Label>
        <asp:DropDownList ID="ddl_area" runat="server">
            <asp:ListItem Text="W" Value="W"></asp:ListItem>
            <asp:ListItem Text="E" Value="E"></asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="Label4" runat="server" Text="二级区域:"></asp:Label>
        <asp:DropDownList ID="ddl_middleArea" runat="server">
            <asp:ListItem Text="H" Value="H"></asp:ListItem>
            <asp:ListItem Text="Q" Value="Q"></asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="Label1" runat="server" Text="条码范围:"></asp:Label>
        <asp:TextBox ID="txb_code1" runat="server"></asp:TextBox>
        <asp:Label ID="Label2" runat="server" Text="至"></asp:Label>
        <asp:TextBox ID="txb_code2" runat="server"></asp:TextBox>
        <asp:Button ID="btn_build" runat="server" Text="生成条码" OnClick="btn_build_Click" />
        <asp:Button ID="btn_print" runat="server" Text="直接打印" OnClientClick="javascript:PreviewMytable();" /><br /><br />        
        <asp:Label ID="Label5" runat="server" Text="特殊编码:"></asp:Label>
        <asp:TextBox ID="txb_special" runat="server"></asp:TextBox>
        <asp:Button ID="btn_special" runat="server" Text="生成" OnClick="btn_special_Click"/>
    <div id="div1">
        <asp:GridView ID="GridView1" runat="server" ShowHeader="False" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img src='<%#Eval("ImgUrl") %>'  />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
