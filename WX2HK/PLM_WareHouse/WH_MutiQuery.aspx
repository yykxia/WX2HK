<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WH_MutiQuery.aspx.cs" Inherits="WX2HK.PLM_WareHouse.WH_MutiQuery" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>模塑仓库-综合查询</title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Grid1" />
        <f:Grid ID="Grid1" runat="server" AllowPaging="true" PageSize="50"
             DataKeyNames="Id" DataIDField="Id" EnableTextSelection="true" OnRowCommand="Grid1_RowCommand">
            <Toolbars>
                <f:Toolbar ID="tlb1" runat="server">
                    <Items>
                        <f:TriggerBox ID="tgb_orderNo" runat="server" Label="按订单号筛选"
                             TriggerIcon="Search" OnTriggerClick="tgb_orderNo_TriggerClick"></f:TriggerBox>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:DatePicker ID="DatePicker1" runat="server" Label="按投产日期"></f:DatePicker>
                        <f:DatePicker ID="DatePicker2" runat="server" Label="至" LabelAlign="Right"></f:DatePicker>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Toolbars>
                <f:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                        <f:RadioButtonList runat="server" ID="rdb_onlineStatus" Label="生产状态" Width="300px">
                            <f:RadioItem Text="完工" Value="0" Selected="true" />
                            <f:RadioItem Text="在产" Value="1" />
                            <f:RadioItem Text="全部" Value="%" />
                        </f:RadioButtonList>
                        <f:Button runat="server" Text="查询" Icon="SystemSearch"
                            ID="btn_multQuery" OnClick="btn_multQuery_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:RowNumberField></f:RowNumberField>
                <f:BoundField DataField="scddcp_jhpch" Width="120px" HeaderText="订单编号"></f:BoundField>
                <f:BoundField DataField="itemNo" Width="150px" HeaderText="物料编码"></f:BoundField>
                <f:BoundField DataField="itemParm" Width="300px" HeaderText="物料规格"></f:BoundField>
                <f:BoundField DataField="BuildTime" DataFormatString="{0:yyyy-MM-dd}" Width="100px" 
                    TextAlign="Center" HeaderText="投产日期"></f:BoundField>
                <f:BoundField DataField="endTime" DataFormatString="{0:yyyy-MM-dd}" Width="100px" 
                    TextAlign="Center" HeaderText="完工日期"></f:BoundField>
                <f:BoundField DataField="ProductSum" Width="100px" TextAlign="Center" HeaderText="生产数量"></f:BoundField>
                <f:BoundField DataField="ConfirmSum" Width="100px" TextAlign="Center" HeaderText="入库总数"></f:BoundField>
                <f:BoundField DataField="BoundSum" Width="100px" TextAlign="Center" HeaderText="库存数量"></f:BoundField>
                <f:BoundField DataField="DistSum" Width="100px" TextAlign="Center" HeaderText="出库数量"></f:BoundField>
                <f:LinkButtonField HeaderText="出入库信息" TextAlign="Center" 
                    CommandName="details" Width="90px" Text="查看"></f:LinkButtonField>
                <f:TemplateField ColumnID="expander" RenderAsRowExpander="true">
                    <ItemTemplate>
                    </ItemTemplate>
                </f:TemplateField>
            </Columns>
        </f:Grid>
    </form>
    <f:Window ID="Window1" Width="500px" Height="600px" Title="出入库明细" Hidden="true"
        EnableCollapse="true" runat="server" EnableResize="false" Layout="Fit"
        IsModal="false" AutoScroll="true" EnableClose="true" Target="Parent">
        <Items>
            <f:Grid ID="Grid_details" runat="server" ShowBorder="false">
                <Columns>
                    <f:BoundField DataField="optType" HeaderText="操作类型" TextAlign="Center" Width="100px">
                    </f:BoundField>
                    <f:BoundField DataField="BarCode" HeaderText="货架号" TextAlign="Center" Width="80px">
                    </f:BoundField>
                    <f:BoundField DataField="StorageCode" HeaderText="库位码" TextAlign="Center" Width="80px">
                    </f:BoundField>
                    <f:BoundField DataField="BoundQty" HeaderText="数量" Width="60px" TextAlign="Center">
                    </f:BoundField>
                    <f:BoundField DataField="boundTime" HeaderText="操作时间"
                         DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" TextAlign="Center" Width="150px">
                    </f:BoundField>
                </Columns>
            </f:Grid>
        </Items>
        <Toolbars>
            <f:Toolbar ID="tlb_commit" runat="server" Position="Bottom">
                <Items>
                    <f:Button ID="btn_close" runat="server" Text="关闭"
                            OnClick="btn_close_Click"></f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
    </f:Window>
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
                        fields: ['StorageCode', 'CntrCode', 'BoundQty'],
                        proxy: {
                            type: 'ajax',
                            url: './Expander_OrderStorage.ashx?id=' + record.getId(),
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
                            text: '库位', dataIndex: 'StorageCode', sortable: false, menuDisabled: true, width: 80
                        }, {
                            text: '货架', dataIndex: 'CntrCode', sortable: false, menuDisabled: true, width: 80
                        }, {
                            text: '数量', dataIndex: 'BoundQty', sortable: false, menuDisabled: true, width: 80
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
                var gridInside = F($(this).attr('Id'));
                gridInside.doLayout();
            });
        }

    </script>
</body>
</html>
