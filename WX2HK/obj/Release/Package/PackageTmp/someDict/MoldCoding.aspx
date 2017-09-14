<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoldCoding.aspx.cs" Inherits="WX2HK.someDict.MoldCoding" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>模塑车间-模具编码</title>
    <style type="text/css">
        .label_hid {
            display:none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" ShowBorder="false" ShowHeader="false" Layout="Region">
            <Items>
                <f:Grid runat="server" ID="Grid1" RegionPosition="Left" RegionSplit="true" EnableCollapse="true"
                    Width="250px" Title="大类" ShowBorder="true" ShowHeader="true" BodyPadding="5px"
                    OnRowCommand="Grid1_RowCommand" EnableRowClickEvent="true" 
                    OnRowClick="Grid1_RowClick" DataKeyNames="id">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnNew" Text="新增" Icon="Add" runat="server" OnClick="Button3_Click">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:RenderField ColumnID="MJTypeName" DataField="MJTypeName" HeaderText="名称" ExpandUnusedSpace="true">
                            <Editor>
                                <f:TextBox ID="txb_MJTypeName" runat="server"></f:TextBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField ColumnID="MJTypeCode" DataField="MJTypeCode" HeaderText="代码" Width="60px">
                            <Editor>
                                <f:TextBox ID="txb_MJTypeCode" runat="server"></f:TextBox>
                            </Editor>
                        </f:RenderField>
                        <f:WindowField ColumnID="myWindowField" Width="60px" TextAlign="Center" WindowID="Window1" HeaderText="编辑"
                            Icon="Pencil" ToolTip="编辑" DataTextFormatString="{0}" DataIFrameUrlFields="Id"
                            DataIFrameUrlFormatString="MoldCoding_TypeEdit.aspx?id={0}" />
                        <f:LinkButtonField ColumnID="Delete1" HeaderText="&nbsp;" Width="40px" EnablePostBack="false"
                            Icon="Delete" ConfirmText="确认删除？" />
                    </Columns>
                </f:Grid>
                <f:Grid runat="server" ID="Grid2" RegionPosition="Center"
                    Title="属性" ShowBorder="true" ShowHeader="true" BodyPadding="5px"
                     AllowCellEditing="true" OnRowCommand="Grid2_RowCommand" DataKeyNames="id">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:Button ID="Button1" Text="新增" Icon="Add" OnClick="Button1_Click" runat="server">
                                </f:Button>
                                <f:FileUpload runat="server" ID="fileData" ButtonText="导入" ButtonIcon="PageWhiteExcel" AutoPostBack="true"
                                     OnFileSelected="fileData_FileSelected" Label="文件路径">
                                </f:FileUpload>
                                <f:ToolbarFill ID="tlbf" runat="server"></f:ToolbarFill>
                                <f:Button ID="Button2" Text="保存" Icon="SystemSave" OnClick="Button2_Click" runat="server">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:RowNumberField EnablePagingNumber="true" />
                        <f:BoundField ColumnID="TopTypeId" DataField="TopTypeId" HeaderText="大类" Width="60px"></f:BoundField>
                        <f:RenderField ColumnID="Length" DataField="Length" HeaderText="长度" Width="60px">
                            <Editor>
                                <f:TextBox ID="txb_Length" runat="server"></f:TextBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField ColumnID="LenCode" DataField="LenCode" HeaderText="长度编码" Width="80px">
                            <Editor>
                                <f:TextBox ID="txb_LenCode" runat="server"></f:TextBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField ColumnID="Width" DataField="Width" HeaderText="宽度" Width="60px">
                            <Editor>
                                <f:TextBox ID="txb_Width" runat="server"></f:TextBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField ColumnID="WidCode" DataField="WidCode" HeaderText="宽度编码" Width="80px">
                            <Editor>
                                <f:TextBox ID="txb_WidCode" runat="server"></f:TextBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField ColumnID="Height" DataField="Height" HeaderText="高度" Width="60px">
                            <Editor>
                                <f:TextBox ID="txb_Height" runat="server"></f:TextBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField ColumnID="HeiCode" DataField="HeiCode" HeaderText="高度编码" Width="80px">
                            <Editor>
                                <f:TextBox ID="txb_HeiCode" runat="server"></f:TextBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField ColumnID="StockQty" DataField="StockQty" HeaderText="数量" Width="100px">
                            <Editor>
                                <f:TextBox ID="txb_StockQty" runat="server"></f:TextBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField ColumnID="batchNo" DataField="batchNo" HeaderText="批次" Width="80px">
                            <Editor>
                                <f:TextBox ID="txb_batchNo" runat="server"></f:TextBox>
                            </Editor>
                        </f:RenderField>
                        <f:LinkButtonField HeaderText="&nbsp;" Width="80px" ConfirmText="删除选中行？" ConfirmTarget="Top"
                            CommandName="Delete" Icon="Delete" ColumnID="Delete2" />
                    </Columns>
                </f:Grid>
                <f:Grid runat="server" ID="Grid3" RegionPosition="Right" RegionSplit="true" EnableCollapse="true"
                    Width="300px" Title="编码" ShowBorder="true" ShowHeader="true" BodyPadding="5px" ShowGridHeader="false">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar3" runat="server">
                            <Items>
                                <f:Button ID="btn_saveAs" Text="导出" Icon="FolderGo" runat="server"
                                     EnableAjax="false" DisableControlBeforePostBack="false" OnClick="btn_saveAs_Click">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:BoundField DataField="unionCode" HeaderText="编码信息"
                             TextAlign="Center" ExpandUnusedSpace="true"></f:BoundField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Label ID="label_hidden" runat="server" CssClass="label_hid"></f:Label>
        <f:Label ID="label_hidden2" runat="server" CssClass="label_hid"></f:Label>
    </form>
        <f:Window ID="Window1" Title="编辑" Hidden="true" EnableIFrame="true" runat="server"
            EnableMaximize="true" EnableResize="true" Target="Top"
            IsModal="True" Width="600px" Height="450px">
        </f:Window>
</body>
</html>
