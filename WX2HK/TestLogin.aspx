<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestLogin.aspx.cs" Inherits="WX2HK.TestLogin" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        .red span {
            font-weight: bold;
            color: Red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:Timer runat="server" ID="Timer1" Interval="1" Enabled="false" OnTick="Timer1_Tick"></f:Timer>
        <f:Window ID="Window1" runat="server" Title="系统登录" Icon="Key"  IsModal="false" EnableClose="false"
            WindowPosition="GoldenSection" Layout="HBox" Width="450px" Height="220px">
            <Items>
            <f:Image ID="imageLogin" ImageUrl="~/res/images/login/login_1.png" runat="server"
                ImageWidth="150px" Width="160px">
            </f:Image>
                <f:SimpleForm ID="SimpleForm1" LabelAlign="Top" BoxFlex="1" runat="server" LabelWidth="45px"
                BodyPadding="20px 10px" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <f:TextBox ID="tbxUserPhoneNumb" FocusOnPageLoad="true" Label="手机号" Required="true" runat="server">
                        </f:TextBox>
                        <f:TextBox ID="tbxVerfCode" Label="验证码" Required="true" runat="server">
                        </f:TextBox>
                    </Items>
                </f:SimpleForm>
            </Items>
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" ToolbarAlign="Right" Position="Bottom">
                    <Items>
                        <f:LinkButton ID="lkb_SendVerfCode" Text="发送验证码" runat="server" OnClick="lkb_SendVerfCode_Click"></f:LinkButton>
                        <f:Label runat="server" ID="Label_timer" CssClass="red" Text="(" Hidden="true"></f:Label>
                        <f:Label runat="server" ID="Label1" CssClass="red" Text="30" Hidden="true"></f:Label>
                        <f:Label runat="server" ID="Label2" CssClass="red" Text=")" Hidden="true"></f:Label>
                        <f:Button ID="btnLogin" Text="登录" Type="Submit" EnableAjax="false" Icon="LockOpen" ValidateForms="SimpleForm1" ValidateTarget="Top"
                            runat="server" OnClick="btnLogin_Click">
                        </f:Button>
                        <f:Button ID="btnReset" Text="重置" Type="Reset" EnablePostBack="false"
                            runat="server">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:Window>
    </form>
</body>
</html>
