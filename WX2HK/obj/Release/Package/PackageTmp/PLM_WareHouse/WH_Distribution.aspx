<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WH_Distribution.aspx.cs" Inherits="WX2HK.PLM_WareHouse.WH_Distribution" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>模塑仓库管理-配送</title>
    <style>
        .SumLabel {
            color:red;
        }
        .label_hid {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" OnCustomEvent="PageManager1_CustomEvent" />
        <f:Panel ID="Panel1" runat="server" ShowBorder="false" ShowHeader="false" Layout="Region">
            <Items>
                <f:Grid ID="Grid2" ShowBorder="true" ShowHeader="true" Title="领料单明细" runat="server"
                    EnableTextSelection="true" EnableRowClickEvent="true" Height="200px" DataKeyNames="SCDDCP_JHPCH,SourceNo"
                    RegionSplit="true" RegionPosition="Top" EnableCollapse="true" OnRowClick="Grid2_RowClick">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:TriggerBox ID="trgb_sjdh" runat="server" OnTriggerClick="trgb_sjdh_TriggerClick"
                                    TriggerIcon="Search" EmptyText="输入领料单号" Label="单号">
                                </f:TriggerBox>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:BoundField DataField="KCCKD2_FLBH" HeaderText="序号" TextAlign="Center"
                            Width="50px">
                        </f:BoundField>
                        <f:BoundField DataField="kcckd2_wlbh" HeaderText="物料编号"
                            Width="180px">
                        </f:BoundField>
                        <f:BoundField DataField="LSWLZD_WLMC" HeaderText="物料名称"
                            Width="120px">
                        </f:BoundField>
                        <f:BoundField DataField="KCCKD2_PCH" HeaderText="批次号"
                            Width="120px">
                        </f:BoundField>
                        <f:BoundField DataField="KCCKD2_QLSL" HeaderText="请领数量"
                            TextAlign="Center" Width="80px">
                            </f:BoundField>
                            <f:BoundField DataField="OutSum" HeaderText="已发数量"
                                TextAlign="Center" Width="80px">
                            </f:BoundField>
                            <f:BoundField DataField="TempSum" HeaderText="待发数量"
                                TextAlign="Center" Width="80px">
                            </f:BoundField>
                        <f:BoundField DataField="LSWLZD_GGXH" HeaderText="规格型号"
                            ExpandUnusedSpace="true">
                        </f:BoundField>
                    </Columns>
                </f:Grid>
                <f:Grid ID="Grid1" runat="server" Title="可用库存" BodyPadding="5px"
                    Width="550px" RegionPosition="Left" RegionSplit="true" EnableCollapse="true"
                    AllowCellEditing="true" ClicksToEdit="1" OnRowCommand="Grid1_RowCommand"
                    EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit" DataKeyNames="id">
                    <Toolbars>
                        <f:Toolbar runat="server" ID="tlb1">
                            <Items>
                                <f:Button ID="Button2" EnablePostBack="false" runat="server" Text="全选/取消">
                                    <Menu ID="Menu1" runat="server">
                                        <f:MenuButton ID="btnSelectRows" OnClick="btnSelectRows_Click" runat="server" Text="全选">
                                        </f:MenuButton>
                                        <f:MenuButton ID="btnUnselectRows" OnClick="btnUnselectRows_Click" runat="server" Text="取消全选">
                                        </f:MenuButton>
                                    </Menu>
                                </f:Button>
                                <f:Button runat="server" ID="btn_confirm" Text="添加" OnClick="btn_confirm_Click"></f:Button>
                                <f:CheckBox Text="包含其他订单" ID="cb_otherOrders" runat="server"
                                     OnCheckedChanged="cb_otherOrders_CheckedChanged" AutoPostBack="true"></f:CheckBox>
                                <f:ToolbarFill ID="tlbf1" runat="server"></f:ToolbarFill>
                                <f:DropDownList ID="ddl_DistLine" runat="server" Label="配送至:" LabelAlign="Right"
                                    AutoSelectFirstItem="false" EmptyText="请选择流水线" >
<%--                                    <f:ListItem Text="1E" Value="1E" />
                                    <f:ListItem Text="1W" Value="1W" />
                                    <f:ListItem Text="SE" Value="SE" />
                                    <f:ListItem Text="SW" Value="SW" />
                                    <f:ListItem Text="1#" Value="1#" />
                                    <f:ListItem Text="2#" Value="2#" />
                                    <f:ListItem Text="3#" Value="3#" />
                                    <f:ListItem Text="4#" Value="4#" />
                                    <f:ListItem Text="5#" Value="5#" />
                                    <f:ListItem Text="6#" Value="6#" />
                                    <f:ListItem Text="7#" Value="7#" />--%>
                                </f:DropDownList>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:CheckBoxField DataField="selected" RenderAsStaticField="false" ColumnID="isSel"
                            AutoPostBack="true" CommandName="CheckBox1" Width="30px">
                        </f:CheckBoxField>
                        <f:BoundField DataField="StorageCode" HeaderText="库位" Width="80px"></f:BoundField>
                        <f:BoundField DataField="orderNo" HeaderText="订单号" Width="100px"></f:BoundField>
                        <f:BoundField DataField="CntrCode" TextAlign="Center" HeaderText="货架号" Width="80px"></f:BoundField>
                        <f:BoundField DataField="BoundQty" TextAlign="Center" HeaderText="存量" Width="60px"></f:BoundField>
                        <f:BoundField DataField="BoundDays" HeaderText="库龄(天)" Width="80px" TextAlign="Center"></f:BoundField>
                        <f:RenderField DataField="BoundQty" TextAlign="Center" HeaderText="出库数量" Width="80px">
                            <Editor>
                                <f:TextBox ID="txb_outQty" runat="server"></f:TextBox>
                            </Editor>
                        </f:RenderField>
                    </Columns>
                    <Toolbars>
                        <f:Toolbar Position="Bottom" runat="server" ID="tlb_Sum">
                            <Items>
                                <f:Label ID="label1" runat="server" Text="已选中："></f:Label>
                                <f:Label ID="label_Sum" runat="server" EncodeText="false" ShowEmptyLabel="true"></f:Label>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:Grid>
                <f:Grid ID="Grid3" runat="server" Title="待出库清单" DataKeyNames="strorageId,scatSign"
                      RegionPosition="Center" OnRowCommand="Grid3_RowCommand">
                    <Columns>
                        <f:BoundField DataField="StorageCode" Width="80px" HeaderText="库位"></f:BoundField>
                        <f:BoundField DataField="CntrCode" Width="80px" HeaderText="货架号"></f:BoundField>
                        <f:BoundField DataField="distCount" Width="60px" TextAlign="Center" HeaderText="拟发"></f:BoundField>
                        <f:BoundField DataField="distLine" Width="80px" TextAlign="Center" HeaderText="流水线"></f:BoundField>
                        <f:LinkButtonField HeaderText="&nbsp;" Width="80px" ConfirmText="是否移除？" ConfirmTarget="Top"
                            CommandName="move" Text="移除" />          
                    </Columns>
                    <Toolbars>
                        <f:Toolbar Position="Bottom" runat="server" ID="Toolbar1">
                            <Items>
                                <f:Label ID="label2" runat="server" Text="合计："></f:Label>
                                <f:Label ID="label_distSum" runat="server" EncodeText="false"></f:Label>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:Grid>
<%--                <f:Grid ID="Grid4" runat="server" Title="配送合计"
                      RegionPosition="Right" Width="200px">
                    <Columns>
                                
                    </Columns>
                    <Toolbars>
                        <f:Toolbar Position="Bottom" runat="server" ID="Toolbar3">
                            <Items>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:Grid>--%>
            </Items>
        </f:Panel>
        <f:Window ID="Window2" Width="400px" Height="300px" Icon="Cart" Title="配送车"
            EnableCollapse="true" runat="server" EnableResize="false" Layout="Fit"
            IsModal="false" AutoScroll="true" EnableClose="false">
            <Items>
                <f:Grid ID="Grid_cart" runat="server" ShowBorder="false">
                    <Columns>
                        <f:BoundField DataField="SourceNo" HeaderText="配送码" Width="100px">
                        </f:BoundField>
                        <f:BoundField DataField="itemNo" HeaderText="物料编码" ExpandUnusedSpace="true">
                        </f:BoundField>
                        <f:BoundField DataField="distSum" HeaderText="数量" Width="60px" TextAlign="Center">
                        </f:BoundField>
                    </Columns>
                </f:Grid>
            </Items>
            <Toolbars>
                <f:Toolbar ID="tlb_commit" runat="server" Position="Bottom">
                    <Items>
                        <f:Button ID="btn_dist" runat="server" Text="生成配送单" Icon="CartAdd"
                                OnClick="btn_dist_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:Window>
    </form>
</body>
</html>
