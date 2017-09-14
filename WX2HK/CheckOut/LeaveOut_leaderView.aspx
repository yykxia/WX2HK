<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaveOut_leaderView.aspx.cs" Inherits="WX2HK.CheckOut.LeaveOut_leaderView" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="maximum-scale=1.0,minimum-scale=1.0,user-scalable=0,width=device-width,initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>请假申请-明细</title>
    <style>
        .result img
        {
            border: 1px solid #CCCCCC;
            max-width: 550px;
            padding: 3px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="SimpleForm1" />
        <f:SimpleForm ID="SimpleForm1" BodyPadding="5px" LabelWidth="90px" AutoScroll="true"
            runat="server" ShowBorder="True" ShowHeader="false"
            Title="请假内容">
            <Items>
                <f:Label ID="label_dept" runat="server" Label="部门"></f:Label>
                <f:Label ID="label_name" runat="server" Label="申请人"></f:Label>
                <f:Label ID="label_reqTime" runat="server" Label="提交时间"></f:Label>
                <f:Label ID="label_status" runat="server" Label="当前状态"></f:Label>
                <f:Label ID="label_personDtl" runat="server" Label="请假人员明细"></f:Label>
                <f:Label ID="label_startTime" runat="server" Label="起始时间"></f:Label>
                <f:Label ID="label_endTime" runat="server" Label="截至时间"></f:Label>
                <f:Label ID="label_days" runat="server" Label="天数"></f:Label>
                <f:Label ID="label_hours" runat="server" Label="小时"></f:Label>
                <f:Label ID="label_desc" runat="server" Label="补充说明"></f:Label>
                <f:Label ID="label_pic" runat="server" Label="是否有附图"></f:Label>
                <f:Button ID="btn_loadImg" runat="server" Text="加载图片" OnClick="btn_loadImg_Click"></f:Button>
                <f:Label ID="labResult" CssClass="result" EncodeText="false" runat="server">
                </f:Label>
<%--                <f:Label runat="server" ID="label_hidden" Hidden="true"></f:Label>--%>
<%--                <f:ContentPanel ID="content1" runat="server" ShowHeader="false" AutoScroll="true">
                    <asp:Image ID="Image1" runat="server" Height="200px" />
                </f:ContentPanel>--%>
            </Items>
            <Toolbars>
                <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                    <Items>
                        <f:Button runat="server" ID="btnSubmit" ConfirmText="确认提交？"
                            ValidateForms="SimpleForm1" Text="同意申请" OnClick="btnSubmit_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:SimpleForm>
        <input type=hidden id="hidden_field" runat="server">
    </form>
<%--    <script>
        wx.config({
            debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            appId: 'wx3068893beff3a241', // 必填，企业号的唯一标识，此处填写企业号corpid
            timestamp: '<%= TimeStampStr %>',// 必填，生成签名的时间戳
            nonceStr: '<%= NonceStr %>', // 必填，生成签名的随机串
            signature: '<%= MsgSigStr %>',// 必填，签名，见附录1
            jsApiList: ['chooseImage', 'previewImage', 'uploadImage', 'downloadImage'] // 必填，需要使用的JS接口列表，这里用到：拍照或从手机相册中选图，预览图片，上传图片，下载图片
        });
        //通过ready接口处理成功验证

        wx.ready(function () {


        });

    </script>--%>
</body>
</html>
