<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_userMgmt_Edit.aspx.cs" Inherits="WX2HK.PLM.PLM_userMgmt_Edit" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="SimpleForm1" runat="server" />
        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false"
            AutoScroll="true" BodyPadding="10px" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose">
                        </f:Button>
                        <f:Button ID="btnSaveRefresh" Text="保存" runat="server" Icon="SystemSave" ValidateForms="SimpleForm1"
                            OnClick="btnSaveRefresh_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Rows>
                <f:FormRow>
                    <Items>
                        <f:TextBox runat="server" Label="工号" ID="txb_workNo" Required="true"></f:TextBox>
                    </Items>
                </f:FormRow>
                <f:FormRow>
                    <Items>
                        <f:CheckBox ID="CkeckBox_enabled" ShowLabel="false" runat="server" Text="是否启用" Checked="True">
                        </f:CheckBox>
                    </Items>
                </f:FormRow>
                <f:FormRow>
                    <Items>
                        <f:CheckBoxList ID="ckb_roleList" runat="server" Label="所属角色" ColumnNumber="1"></f:CheckBoxList>
                    </Items>
                </f:FormRow>
<%--                <f:FormRow>
                    <Items>
                        <f:TextArea runat="server" Label="功能描述" ID="txa_desc"></f:TextArea>
                    </Items>
                </f:FormRow>--%>
            </Rows>
        </f:Form>        
    </form>
</body>
</html>
