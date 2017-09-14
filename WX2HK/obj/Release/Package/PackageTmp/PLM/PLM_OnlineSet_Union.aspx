<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_OnlineSet_Union.aspx.cs" Inherits="WX2HK.PLM.PLM_OnlineSet_Union" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        /*.photo {
            height: 150px;
            line-height: 150px;
            overflow: hidden;
        }*/

            .photo img {
                max-height:200px;
                vertical-align: middle;
            }

        .labelcolor span{
            color:red;
        }
        .x-grid-row .x-grid-cell-inner
        {
            white-space: normal;
            word-break: break-all;
        }
        .hiddenfield {
            display:none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" EnableCollapse="true"
                    Width="950px" RegionPosition="Left" Layout="Fit" RegionSplit="true"
                    runat="server">
                    <Items>
                        <f:Grid ID="Grid2" ShowBorder="true" ShowHeader="true" Title="ERP订单数据" runat="server"
                            DataKeyNames="productSN" EnableTextSelection="true" AllowCellEditing="true" ClicksToEdit="1">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <f:TriggerBox ID="tgb_wlbh" runat="server" OnTriggerClick="tgb_wlbh_TriggerClick"
                                             EmptyText="请输入物料编号筛选" TriggerIcon="Search"></f:TriggerBox>
                                        <f:TriggerBox ID="tgb_orderNo" runat="server" OnTriggerClick="tgb_orderNo_TriggerClick"
                                             EmptyText="请输入订单号筛选" TriggerIcon="Search"></f:TriggerBox>
                                        <f:Label ID="Label1" CssClass="labelcolor" runat="server"></f:Label>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Toolbars>
                                <f:Toolbar ID="toolbar3" runat="server">
                                    <Items>
                                        <f:DatePicker ID="DatePicker1" Label="计划日期" LabelWidth="80px" runat="server">
                                        </f:DatePicker>
                                        <f:DatePicker ID="DatePicker2" Label="至" CompareControl="DatePicker1"
                                             CompareOperator="GreaterThanEqual" CompareMessage="结束日期应大于开始时间！" LabelWidth="30px" runat="server">
                                        </f:DatePicker>
                                        <f:Button ID="btn_search" runat="server" Text="查 询" Icon="SystemSearch" OnClick="btn_search_Click"></f:Button>
                                        <f:ToolbarSeparator ID="tlbsp1" runat="server"></f:ToolbarSeparator>
                                        <f:Button ID="btn_Add" runat="server" Text="生 成" Icon="PageGo" OnClick="btn_Add_Click"></f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:CheckBoxField TextAlign="Center" ColumnID="CanRead" DataField="CanRead"
                                     RenderAsStaticField="false" Width="40px"></f:CheckBoxField>
                                <f:BoundField Width="120px" DataField="orderNo" DataFormatString="{0}"
                                    HeaderText="订单号" />
                                <f:BoundField DataField="itemNo" HeaderText="物料编码" 
                                    DataFormatString="{0}" Width="120px" />
                                <f:BoundField DataField="itemScale" HeaderText="比例" 
                                    Width="60px" DataFormatString="{0}" Hidden="true" />
                                <f:BoundField DataField="RequireParm" HeaderText="规格" DataFormatString="{0}" ExpandUnusedSpace="true"/>
                                <f:BoundField DataField="itemName" HeaderText="品名" DataFormatString="{0}" Width="100px"/>
                                <f:BoundField DataField="planSum" HeaderText="计划" DataFormatString="{0}" TextAlign="Center" Width="60px"/>
                                <f:BoundField DataField="historySum" HeaderText="已排产" DataFormatString="{0}" TextAlign="Center" Width="60px"/>
                                <f:RenderField Width="75px" ColumnID="lastSum" DataField="lastSum" FieldType="Int"
                                    TextAlign="Center" HeaderText="单次排产">
                                    <Editor>
                                        <f:NumberBox ID="tbxEditorCount" NoDecimal="true" NoNegative="false" runat="server">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField DataField="itemTech" HeaderText="工艺要求" DataFormatString="{0}" Width="80px"/>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center"
                    Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                    <Items>
                        <f:Form ID="SimpleForm1" ShowBorder="false" Title="计划详情" Height="300px"
                            AutoScroll="true" BodyPadding="10px" runat="server" LabelWidth="80px">
                            <Rows>
                                <f:FormRow>
                                    <Items>
                                        <f:DropDownList ID="ddl_line" Label="生产线" EmptyText="请选择" ShowRedStar="true"
                                             AutoSelectFirstItem="false" runat="server" Required="true">
                                        </f:DropDownList>
                                        <f:CheckBox ID="CkeckBox_enabled" ShowLabel="false" runat="server" Text="临时插单" Checked="False">
                                        </f:CheckBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox runat="server" Label="物料编码" ShowRedStar="true" ID="txb_itemNo" Required="true"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox runat="server" Label="订单号" ShowRedStar="true" ID="txb_workNo" Required="true"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox runat="server" Label="品名" ID="txb_itemName" Required="true"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextArea runat="server" Label="规格" ID="TextArea_parm" Required="true"></f:TextArea>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox runat="server" Label="工艺要求" ID="txb_itemTech" Required="true"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:NumberBox ID="numb_planCount" runat="server" Label="排产数量" Required="true" ShowRedStar="true"></f:NumberBox>
                                        <f:NumberBox ID="numb_redCount" runat="server" Label="预警数量" Required="true" ShowRedStar="true" Text="0"></f:NumberBox>
                                        <f:NumberBox ID="numb_preSet" runat="server" Label="预设" Required="true" ShowRedStar="true" Text="8"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>                       
                        <f:Panel ID="Panel1" ShowHeader="false" BodyPadding="10px" BoxFlex="1"
                            ShowBorder="true" runat="server">
                            <Toolbars>
                                <f:Toolbar ID="tlb_top" runat="server" Position="Top">
                                    <Items>
                                        <f:FileUpload runat="server" ID="filePhoto" ShowRedStar="false" ShowLabel="false"
                                            ButtonText="上传产品图片" ButtonOnly="true" Required="false" ButtonIcon="ImageAdd"
                                            AutoPostBack="true" AcceptFileTypes="image/*" OnFileSelected="filePhoto_FileSelected" >
                                        </f:FileUpload>
                                        <f:Label ID="Label_desc" CssClass="labelcolor" runat="server" Text="（图片最佳显示的宽高比例为5:2）"></f:Label>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Items>
                                <f:Image CssClass="photo" ID="Image_product" runat="server">
                                </f:Image>
                            </Items>
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server" Position="Bottom">
                                    <Items>
                                        <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose">
                                        </f:Button>
                                        <f:Button ID="btnSaveRefresh" Text="保存" runat="server" Icon="SystemSave" ValidateForms="SimpleForm1"
                                            OnClick="btnSaveRefresh_Click" >
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                        </f:Panel>                    
                    </Items>
                </f:Region>
            </Regions>

        </f:RegionPanel>
        <f:Grid ID="Grid_hidden" runat="server" CssClass="hiddenfield">
            <Columns>
                <f:BoundField DataField="orderId"></f:BoundField>
                <f:BoundField DataField="orderCount"></f:BoundField>
                <f:BoundField DataField="orderNo"></f:BoundField>
            </Columns>
        </f:Grid>
        <f:Label ID="label_hidden" runat="server" CssClass="hiddenfield"></f:Label>
        <f:Label ID="label_hidden1" runat="server" CssClass="hiddenfield"></f:Label>
    </form>
</body>
</html>
