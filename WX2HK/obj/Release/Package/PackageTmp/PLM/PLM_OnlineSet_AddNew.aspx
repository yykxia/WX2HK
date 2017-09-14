<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_OnlineSet_AddNew.aspx.cs" Inherits="WX2HK.PLM.PLM_OnlineSet_AddNew" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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

        .hiddenfield {
            display:none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" OnCustomEvent="PageManager1_CustomEvent" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false"
                    Width="300px" RegionPosition="Left" Layout="Fit" RegionSplit="true"
                    runat="server">
                    <Items>
                        <f:Grid ID="Grid2" ShowBorder="true" ShowHeader="true" Title="ERP订单数据(双击添加)" runat="server" ExpandAllRowExpanders="true"
                            DataKeyNames="productSN" EnableMultiSelect="false" EnableRowClickEvent="true" EnableTextSelection="true"
                             OnRowClick="Grid2_RowDoubleClick" ShowGridHeader="false">
                            <Toolbars>
                                <f:Toolbar runat="server">
                                    <Items>
                                        <f:TriggerBox ID="tgb_orderNo" runat="server" OnTriggerClick="tgb_orderNo_TriggerClick"
                                             EmptyText="请输入订单号筛选" TriggerIcon="Search"></f:TriggerBox>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:TemplateField ColumnID="expander" RenderAsRowExpander="true">
                                    <ItemTemplate>
                                        <p>
                                            <strong>品名：</strong><%# Eval("itemName") %><strong> 数量：</strong><%# Eval("planSum") %>
                                        </p>
                                        <p>
                                            <strong>规格：</strong><%# Eval("itemParm") %>
                                        </p>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField Width="120px" DataField="orderNo" DataFormatString="{0}"
                                    HeaderText="订单号" />
                                <f:BoundField ExpandUnusedSpace="true" DataField="itemNo" DataFormatString="{0}" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center"
                    Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                    <Items>
                        <f:Form ID="SimpleForm1" ShowBorder="false" Title="计划详情" Height="300px"
                            AutoScroll="true" BodyPadding="10px" runat="server">
                            <Rows>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox runat="server" Label="订单号" ShowRedStar="true" ID="txb_workNo" Required="true"></f:TextBox>
                                        <f:CheckBox ID="CkeckBox_enabled" ShowLabel="false" runat="server" Text="临时插单" Checked="False">
                                        </f:CheckBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:DropDownList ID="ddl_line" Label="生产线" EmptyText="请选择" ShowRedStar="true"
                                             AutoSelectFirstItem="false" runat="server" Required="true">
                                        </f:DropDownList>
                                        <f:TextBox runat="server" Label="物料编码" ShowRedStar="true" ID="txb_itemNo" Required="true"></f:TextBox>
                                                                                
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox runat="server" Label="品名" ID="txb_itemName" Required="true"></f:TextBox>
                                        <f:Label runat="server" ID="Label_sn" ShowLabel="false"></f:Label>
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
                                        <f:NumberBox ID="numb_preSet" runat="server" Label="预设" Required="true" ShowRedStar="true"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
<%--                                <f:FormRow>
                                    <Items>
                                        <f:TabStrip ID="TabStrip1" runat="server"></f:TabStrip>
                                    </Items>
                                </f:FormRow>--%>
<%--                                <f:FormRow>
                                    <Items>
                                    </Items>
                                </f:FormRow>--%>
<%--                                <f:FormRow>
                                    <Items>
                                        <f:Image CssClass="photo" ID="Image_product" runat="server" Width="500px">
                                        </f:Image>
                                    </Items>
                                </f:FormRow>--%>
<%--                                <f:FormRow>
                                    <Items>
                                        <f:CheckBox ID="CkeckBox_enabled" ShowLabel="false" runat="server" Text="临时插单" Checked="False">
                                        </f:CheckBox>
                                    </Items>
                                </f:FormRow>--%>
                <%--                <f:FormRow>
                                    <Items>
                                        <f:TextArea runat="server" Label="功能描述" ID="txa_desc"></f:TextArea>
                                    </Items>
                                </f:FormRow>--%>
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
                                        <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose" OnClick="btnClose_Click">
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
        <f:Label ID="label_hidden" runat="server" CssClass="hiddenfield"></f:Label>
    </form>
</body>
</html>
