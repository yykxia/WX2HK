<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C2M2ERP.aspx.cs" Inherits="WX2HK.MES.C2M2ERP" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            color: #0000FF;
            font-size: xx-large;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
<%--    <div class="auto-style1" style="text-align: center">
        <strong>定制线-ERP接口</strong></div>--%>
     <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="WorderGrid" />
        <f:Grid ID="WorderGrid" runat="server" ShowHeader="true" AutoScroll="true"  AllowPaging="true" PageSize="100"  
            EnableSummary="true" SummaryPosition="Flow"  KeepCurrentSelection="true" Title="定制线-ERP接口"
             EnableCheckBoxSelect="true"  CheckBoxSelectOnly="true" OnPageIndexChange="WorderGrid_PageIndexChange"   >
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb1">
                    <Items>
                        <f:DatePicker runat="server" Required="true" EnableEdit="false" Label="起始日期" EmptyText="请选择日期"
                            ID="DatePicker1" DateFormatString="yyyy-MM-dd"  ShowRedStar="True" >
                        </f:DatePicker>

                    </Items>
                </f:Toolbar>

            </Toolbars>
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:DatePicker runat="server" Required="true" EnableEdit="false" Label="结束日期" EmptyText="请选择日期"
                             CompareControl="DatePicke1" DateFormatString="yyyy-MM-dd"  CompareOperator="GreaterThanEqual"
                             CompareMessage="结束日期应该大于开始日期" ID="DatePicker2" ShowRedStar="True">
                         </f:DatePicker>

                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb3">
                    <Items>
                        <f:DropDownList ID="ddl_line" Label="单据类型" LabelWidth="100px" Width="300px" runat="server">
                           <f:ListItem Text="--请选择--" Value="Choice" Selected="true"/>
                            <f:ListItem Text="MES工单-ERP生产订单" Value="Worder" />
                            <f:ListItem Text="MES销售-ERP入库领料" Value="Sale" />
                        </f:DropDownList>
                       
                    </Items>
                </f:Toolbar>
            </Toolbars>

            <Toolbars>
                <f:Toolbar runat="server" ID="tlb5">
                    <Items>
                        <f:Button ID="btn_filter" Text="查询" runat="server" Icon="SystemSearch" OnClick="btn_filter_Click" ></f:Button>
                        <f:Button ID="btn_middle" Text="导入中间库" runat="server" Icon="SystemSearch" OnClick="btn_middle_Click" Enabled="false"  ></f:Button>
                        <f:Button ID="Button1" EnableAjax="false" DisableControlBeforePostBack="false"
                                runat="server" Text="ERP生单" Icon="PageExcel" OnClick="Button1_Click" Enabled="false"  >
                            </f:Button>
                          <f:Label ID="labResult" EncodeText="false" runat="server" >
                         </f:Label>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                 <f:TemplateField Width="60px" >
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                    </ItemTemplate>
                </f:TemplateField>
              
            </Columns>
             </f:Grid>
    </form>
</body>
</html>
