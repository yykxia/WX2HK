<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoldCoding_TypeEdit.aspx.cs" Inherits="WX2HK.someDict.MoldCoding_TypeEdit" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="SimpleForm1" />
        <f:SimpleForm ID="SimpleForm1" runat="server" ShowHeader="false" BodyPadding="10px">
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:Button ID="btn_save" Icon="Disk" runat="server" Text="保存编辑" 
                            ValidateForms="SimpleForm1" OnClick="btn_save_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:TextBox ID="txb_MJTypeName" runat="server" Label="类型名称" Required="true" ShowRedStar="true"></f:TextBox>
                <f:TextBox ID="txb_MJTypeCode" runat="server" Label="类型编码" Required="true" ShowRedStar="true"></f:TextBox>
            </Items>
        </f:SimpleForm>
    </form>
</body>
</html>
