<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_OnlineSet_Print.aspx.cs" Inherits="WX2HK.PLM.PLM_OnlineSet_Print" %>

<!DOCTYPE html>

<html>
<head id="head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="../js/LodopFuncs.js"></script>
<%--    <script src="http://localhost:8000/CLodopfuncs.js?name=CLODOPA"></script>--%>

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
        .label_dtl {
            margin-right:0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="btn_print" runat="server" Text="直接打印" OnClientClick="javascript:PreviewMytable();" />
        <div id="div1">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false"
                 OnRowDataBound="GridView1_RowDataBound" ShowFooter="true">
                <RowStyle Height="150px" />
                <Columns>
                    <asp:TemplateField HeaderText="序号" InsertVisible="False" HeaderStyle-Width="30px"> 
                    <ItemTemplate> 
                    <%#Container.DataItemIndex+1%> 
                    </ItemTemplate> 
                    </asp:TemplateField> 
                    <asp:BoundField DataField="OrderNo" HeaderText="订单号" HeaderStyle-Width="120px" />
                    <asp:BoundField DataField="itemNo" HeaderText="物料编码" HeaderStyle-Width="120px" />
                    <asp:BoundField DataField="ItemParm" HeaderText="规格" />
                    <asp:BoundField DataField="PlanCount" HeaderText="数量" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
<%--                    <asp:TemplateField HeaderText="产品图片">
                        <ItemTemplate>
                            <img width="200" height="73" src='<%#Eval("ImgUrl") %>'  />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="条形码" HeaderStyle-Width="200px">
                        <ItemTemplate>
                            <img width="200" height="150" src='<%#Eval("BarCode") %>'  />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
    <script type="text/javascript"> 
        var LODOP; //声明为全局变量
        var iRadioValue=1;
        function PreviewMytable(){
            LODOP=getLodop();  
            LODOP.PRINT_INIT("");
            var strheadStyle = "<head>" + document.getElementById("head1").innerHTML + "</head>";
            LODOP.ADD_PRINT_TABLE(0, 0, "100%", "100%", strheadStyle + document.getElementById("div1").innerHTML);
            LODOP.SET_PRINT_STYLEA(0, "TableHeightScope", iRadioValue);
            //LODOP.ADD_PRINT_HTM(0, 0, "100%", "100%", strheadStyle + document.getElementById("div1").innerHTML);
            //LODOP.SET_PRINT_STYLEA(0, "HtmWaitMilSecs", 300);
            LODOP.SET_PRINT_PAGESIZE(2, 0, 0, "A4");
            LODOP.PREVIEW();
        };
    </script>
</body>
</html>
