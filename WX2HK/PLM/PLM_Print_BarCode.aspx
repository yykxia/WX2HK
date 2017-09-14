<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_Print_BarCode.aspx.cs" Inherits="WX2HK.PLM.PLM_Print_BarCode" %>

<!DOCTYPE html>

<html>
<head id="head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        table{
            width: 100%;
            border: 1px solid;
            table-layout:fixed;
        }
        table Caption{
            font-size:x-large;
            font-weight:bold;
        }
        tr td {
            border: 1px solid;
            word-wrap:break-word;
        }
    </style>
    <script type="text/javascript" src="../js/LodopFuncs.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="Label1" runat="server" Text="条码范围:"></asp:Label>
        <asp:TextBox ID="txb_code1" runat="server"></asp:TextBox>
        <asp:Label ID="Label2" runat="server" Text="至"></asp:Label>
        <asp:TextBox ID="txb_code2" runat="server"></asp:TextBox>
        <asp:Button ID="btn_build" runat="server" Text="生成条码" OnClick="btn_build_Click" />
        <asp:Button ID="btn_print" runat="server" Text="直接打印" OnClientClick="javascript:PreviewMytable();" />
    <div id="div1">
        <asp:GridView ID="GridView1" runat="server" ShowHeader="False" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img src='<%#Eval("ImgUrl") %>'  />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img src='<%#Eval("ImgUrl1") %>'  />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img src='<%#Eval("ImgUrl2") %>'  />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img src='<%#Eval("ImgUrl3") %>'  />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
    <script type="text/javascript">
        var LODOP; //声明为全局变量
        var iRadioValue = 1;
        function PreviewMytable() {
            LODOP = getLodop();
            LODOP.PRINT_INIT("");
            var strheadStyle = "<head>" + document.getElementById("head1").innerHTML + "</head>";
            LODOP.ADD_PRINT_TABLE(0, 0, "100%", "100%", strheadStyle + document.getElementById("div1").innerHTML);
            LODOP.SET_PRINT_STYLEA(0, "TableHeightScope", iRadioValue);
            //LODOP.ADD_PRINT_HTM(0, 0, "100%", "100%", strheadStyle + document.getElementById("div1").innerHTML);
            //LODOP.SET_PRINT_STYLEA(0, "HtmWaitMilSecs", 300);
            LODOP.SET_PRINT_PAGESIZE(1, 0, 0, "A4");
            LODOP.PREVIEW();
        };
    </script>
</body>
</html>
