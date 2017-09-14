<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_OnlineSet.aspx.cs" Inherits="WX2HK.PLM.PLM_OnlineSet" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager" runat="server" AutoSizePanelID="Grid1" />
        <f:Grid ID="Grid1" runat="server" ShowHeader="false" EnableTextSelection="true"
             EnableCheckBoxSelect="true" DataKeyNames="Id" EnableCollapse="true" DataIDField="Id">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:RadioButtonList ID="RadioButtonList1" Label="状态" LabelWidth="40px" AutoPostBack="true"
                             OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" runat="server" Width="200px">
                            <f:RadioItem Text="上线" Value="1" Selected="true" />
                            <f:RadioItem Text="下线" Value="0" />
                            <f:RadioItem Text="所有" Value="%" />
                        </f:RadioButtonList>
                        <f:ToolbarSeparator ID="tlbsp1" runat="server"></f:ToolbarSeparator>
                        <f:DropDownList ID="ddl_line" Label="产线" LabelWidth="40px" AutoPostBack="true" 
                             runat="server" OnSelectedIndexChanged="ddl_line_SelectedIndexChanged">
                        </f:DropDownList>
                        <f:Button ID="btn_selfRefresh" runat="server" Text="刷新数据" OnClick="btn_selfRefresh_Click"></f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></f:ToolbarSeparator>
                        <f:DatePicker ID="DatePicker1" Label="按投产日期" LabelWidth="80px" runat="server">
                        </f:DatePicker>
                        <f:DatePicker ID="DatePicker2" Label="至" CompareControl="DatePicker1"
                                CompareOperator="GreaterThanEqual" CompareMessage="结束日期应大于开始时间！" LabelWidth="30px" runat="server">
                        </f:DatePicker>

                        <f:Button ID="btn_refresh" runat="server" Text="筛选" Icon="PageRefresh" OnClick="btn_refresh_Click"></f:Button>
                        <f:ToolbarFill ID="tlbf" runat="server"></f:ToolbarFill>
                        <f:Button ID="btn_delete" runat="server" Text="删除" Icon="Delete" ConfirmText="删除后不可恢复！确认删除?"
                             ConfirmIcon="Warning" OnClick="btn_delete_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Toolbars>
                <f:Toolbar ID="tlb2" runat="server">
                    <Items>
                        <f:Button ID="btn_addNew" runat="server" Text="新增产线计划" Icon="Add"></f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server"></f:ToolbarSeparator>
                        <f:Button ID="btn_offLine" runat="server" Text="手工下线" Icon="StopRed"
                             ConfirmText="确认下线？" OnClick="btn_offLine_Click"></f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></f:ToolbarSeparator>
                        <f:Button ID="btn_print" runat="server" Text="打印派工单" Icon="Printer" OnClick="btn_print_Click"></f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator4" runat="server"></f:ToolbarSeparator>
                        <f:Button ID="btn_changeLine" runat="server" Text="产线调整" Icon="ArrowRedo" OnClick="btn_changeLine_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:BoundField DataField="OrderNo" Width="100px" HeaderText="订单号"></f:BoundField>
                <f:BoundField DataField="ItemNo" Width="150px" HeaderText="物料编号"></f:BoundField>
                <f:BoundField DataField="LineName" TextAlign="Center" Width="60px" HeaderText="产线"></f:BoundField>
                <f:TemplateField Width="60px" HeaderText="状态" TextAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("pStatus").ToString()=="下线"?"<span style=\"color:red;\">"+Eval("pStatus").ToString()+"</span>":Eval("pStatus").ToString() %>'></asp:Label>
                    </ItemTemplate>
                </f:TemplateField>
<%--                <f:BoundField DataField="pStatus" TextAlign="Center" Width="60px" HeaderText="状态"></f:BoundField>--%>
                <f:BoundField DataField="BuildTime" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" Width="120px" HeaderText="投产日期"></f:BoundField>
                <f:BoundField DataField="ItemParm" ExpandUnusedSpace="true" HeaderText="产品规格"></f:BoundField>
                <f:BoundField DataField="ItemName" Width="150px" HeaderText="品名"></f:BoundField>
                <f:BoundField DataField="PlanCount" TextAlign="Center" Width="60px" HeaderText="投产"></f:BoundField>
                <f:BoundField DataField="OnlineSum" TextAlign="Center" Width="60px" HeaderText="完成"></f:BoundField>
                <f:WindowField TextAlign="Center" HeaderText="附件" DataTextField="ismlOrno" DataTextFormatString="{0}" WindowID="Window2" DataIFrameUrlFields="ImgURL"
                    DataIFrameUrlFormatString="~/OneImage.aspx?FileName={0}" Width="50px"   />
                <f:BoundField DataField="tempDesc" TextAlign="Center" Width="60px" HeaderText="描述"></f:BoundField>
                <f:TemplateField ColumnID="expander" RenderAsRowExpander="true">
                    <ItemTemplate>
                    </ItemTemplate>
                </f:TemplateField>
            </Columns>
        </f:Grid>
        <f:Window ID="Window1" Hidden="true" EnableIFrame="true" runat="server"
            EnableMaximize="true" EnableResize="true" Target="Parent"
            IsModal="False" Width="900px" Height="650px">
        </f:Window>

        <f:Window ID="Window2" Width="800" Height="450" AutoScroll="true" Hidden="true" EnableIFrame="true" runat="server" Target="Parent"
            EnableMaximize="true" EnableResize="true"  Title="详细信息">
            <Toolbars>
                <f:Toolbar ID="Toolbar2" runat="server" Position="Bottom" CssStyle="text-align:right">
                    <Items>
                        <f:Button ID="btnClose" Text="关闭" Icon="SystemClose" EnablePostBack="true" runat="server" OnClick="btnClose_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:Window>
        
        <f:Window ID="Window3" Hidden="true" EnableIFrame="true" runat="server"
            EnableMaximize="true" EnableResize="true" Target="Parent" Title="调整产线"
            IsModal="False" Width="400px" Height="300px">
        </f:Window>
    </form>
<script src="../js/jquery.min.js" type="text/javascript"></script>
    <script>

        var grid1 = '<%= Grid1.ClientID %>';

        F.ready(function () {
            var grid1Cmp = F(grid1);

            // 展开行扩展列事件
            grid1Cmp.view.on('expandbody', function (rowNode, record, expandRow) {

                var tplEl = Ext.get(expandRow).query('.f-grid-tpl')[0];
                if (!Ext.String.trim(tplEl.innerHTML)) {

                    var store = Ext.create('Ext.data.Store', {
                        fields: ['orderCount', 'orderNo', 'planSum', 'planProdDate'],
                        proxy: {
                            type: 'ajax',
                            url: './Expander_Grid_Data.ashx?id=' + record.getId(),
                            reader: {
                                type: 'json',
                                root: 'data',
                                totalProperty: 'total'
                            }
                        },
                        autoLoad: true,
                        listeners: {
                            load: function () {
                                rowExpandersDoLayout();
                            }
                        }
                    });

                    Ext.create('Ext.grid.Panel', {
                        renderTo: tplEl,
                        header: false,
                        border: true,
                        draggable: false,
                        enableDragDrop: false,
                        enableColumnResize: false,
                        cls: 'gridinrowexpander',
                        store: store,
                        columns: [{
                            text: '订单号', dataIndex: 'orderNo', sortable: false, menuDisabled: true, width: 150
                        }, {
                            text: '计划日期', dataIndex: 'planProdDate', sortable: false, menuDisabled: true, width: 80
                        }, {
                            text: '计划数量', dataIndex: 'planSum', sortable: false, menuDisabled: true, width: 80
                        }, {
                            text: '实际排产', dataIndex: 'orderCount', sortable: false, menuDisabled: true, width: 80
                        }]
                    });
                } else {
                    rowExpandersDoLayout();
                }
            });

            // 折叠行扩展列事件
            grid1Cmp.view.on('collapsebody', function (rowNode, record, expandRow) {
                rowExpandersDoLayout();
            });

        });


        // 重新布局表格和行扩展列中的表格（解决出现纵向滚动条时的布局问题）
        function rowExpandersDoLayout() {
            var grid1Cmp = F(grid1);

            grid1Cmp.doLayout();

            $('.x-grid-item:not(.x-grid-item-collapsed) .gridinrowexpander').each(function () {
                var gridInside = F($(this).attr('id'));
                gridInside.doLayout();
            });
        }

    </script>
</body>
</html>
