<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MES_BomList.aspx.cs" Inherits="WX2HK.MES.MES_BomList" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Bom清单</title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" ShowHeader="false" Layout="VBox">
            <Toolbars>
                <f:Toolbar runat="server" ID="tlb1">
                    <Items>
                        <f:TextBox runat="server" Label="成品编码" ID="txb_cpCode">
                        </f:TextBox>
                        <f:Button ID="btn_query" runat="server" Text="生成" OnClick="btn_query_Click"></f:Button>
                        <f:TextBox runat="server" Label="订单号" ID="txb_orderNo">
                        </f:TextBox>
                        <f:Button ID="btn_orderQuery" runat="server" Text="查询" OnClick="btn_orderQuery_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Grid runat="server" EnableRowLines="True" EnableTextSelection="true" Title="BOM维护" ID="Grid1" BoxFlex="1">
                    <Toolbars>
                        <f:Toolbar ID="tlb2" runat="server">
                            <Items>
                                <f:Button ID="Button1" EnableAjax="false" DisableControlBeforePostBack="false"
                                        runat="server" Text="导出Excel" Icon="PageExcel" OnClick="Button1_Click">
                                    </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:BoundField DataField="PARENTID" HeaderText="父级流水码*" Width="100px" TextAlign="Center" ID="ctl10" ColumnID="Panel1_Grid1_ctl10"></f:BoundField>
                        <f:BoundField DataField="PWL" HeaderText="父级物料编码" Width="120px" ID="ctl11" ColumnID="Panel1_Grid1_ctl11"></f:BoundField>
                        <f:BoundField DataField="PWLMC" HeaderText="父级物料名称" Width="120px" ID="ctl12" ColumnID="Panel1_Grid1_ctl12"></f:BoundField>
                        <f:BoundField DataField="ID" HeaderText="子级流水码*" Width="100px" TextAlign="Center" ID="ctl13" ColumnID="Panel1_Grid1_ctl13"></f:BoundField>
                        <f:BoundField DataField="CWL" HeaderText="子级物料编码" Width="120px" ID="ctl14" ColumnID="Panel1_Grid1_ctl14" ></f:BoundField>
                        <f:BoundField DataField="CWLMC" HeaderText="子级物料名称" Width="120px" ID="ctl15" ColumnID="Panel1_Grid1_ctl15"></f:BoundField>
                        <f:BoundField DataField="PCOUNT" HeaderText="父级数量*" Width="100px" TextAlign="Center" ID="ctl16" ColumnID="Panel1_Grid1_ctl16"></f:BoundField>
                        <f:BoundField DataField="JSBOM_XS" HeaderText="子级数量*" Width="100px" TextAlign="Center" ID="ctl17" ColumnID="Panel1_Grid1_ctl17"></f:BoundField>
                        <f:BoundField DataField="LSWLZD_FPL" HeaderText="损耗系数*" Width="100px" TextAlign="Center" ID="ctl18" ColumnID="Panel1_Grid1_ctl18"></f:BoundField>
                        <f:BoundField DataField="llgx" HeaderText="领料工序" Width="100px" TextAlign="Center" ID="ctl19" ColumnID="Panel1_Grid1_ctl19"></f:BoundField>
                    </Columns>
                </f:Grid>
                <f:Grid runat="server" EnableRowLines="True" EnableTextSelection="true" Title="物料信息" ID="Grid2" BoxFlex="1">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="Button2" EnableAjax="false" DisableControlBeforePostBack="false"
                                        runat="server" Text="导出Excel" Icon="PageExcel" OnClick="Button2_Click">
                                    </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:BoundField DataField="lswlzd_wlbh" HeaderText="物料编码*" Width="120px"></f:BoundField>
                        <f:BoundField DataField="lswlzd_wlmc" HeaderText="物料名称*" Width="120px"></f:BoundField>
                        <f:BoundField DataField="LSWLZD_GGXH" HeaderText="物料型号*" Width="200px"></f:BoundField>
                        <f:BoundField DataField="jldw" HeaderText="计量单位*" Width="80px" TextAlign="Center"></f:BoundField>
                        <f:BoundField DataField="ly" HeaderText="来源属性*" Width="120px" TextAlign="Center"></f:BoundField>
                        <f:BoundField DataField="LSWLEX_C50" HeaderText="物料类型*" Width="100px" TextAlign="Center"></f:BoundField>
                    </Columns>
                </f:Grid>
                <f:Grid runat="server" EnableRowLines="True" EnableTextSelection="true" Title="物料版本" ID="Grid3" BoxFlex="1">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:Button ID="Button3" EnableAjax="false" DisableControlBeforePostBack="false"
                                        runat="server" Text="导出Excel" Icon="PageExcel" OnClick="Button3_Click">
                                    </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:BoundField DataField="jswlzd_lsbh" HeaderText="流水码*" Width="100px" TextAlign="Center"></f:BoundField>
                        <f:BoundField DataField="JSWLZD_WLBH" HeaderText="物料编码*" Width="120px"></f:BoundField>
                        <f:BoundField DataField="jswlzd_bbbh" HeaderText="版本号*" Width="80px" TextAlign="Center"></f:BoundField>
                        <f:BoundField DataField="isNew" HeaderText="是否最新版*" Width="100px" TextAlign="Center"></f:BoundField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
