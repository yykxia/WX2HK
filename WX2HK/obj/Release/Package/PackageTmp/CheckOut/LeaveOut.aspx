<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaveOut.aspx.cs" Inherits="WX2HK.CheckOut.LeaveOut" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="maximum-scale=1.0,minimum-scale=1.0,user-scalable=0,width=device-width,initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>请假单-填写申请</title>
    <style type="text/css">
            .control_hidden {
                display:none;
        }

    </style>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.1.0.js">
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="SimpleForm1" />
        <f:SimpleForm ID="SimpleForm1" BodyPadding="5px" LabelWidth="80px" AutoScroll="true"
            runat="server" ShowBorder="True" ShowHeader="false"
            Title="填写请假单">
            <Items>
                <f:Label ID="label_dept" runat="server" Label="部门"></f:Label>
                <f:Label ID="label_name" runat="server" Label="申请人"></f:Label>
                <f:DatePicker runat="server" Required="true" EnableEdit="false" Label="起始日期" EmptyText="请选择日期"
                    ID="DatePicker1" ShowRedStar="True">
                </f:DatePicker>
                <f:TimePicker ID="TimePicker1" ShowRedStar="True" EnableEdit="false" Label="时间" Increment="30"
                    Required="true" EmptyText="请选择时间" runat="server">
                </f:TimePicker> 
                <f:DatePicker runat="server" Required="true" EnableEdit="false" Label="截止日期" EmptyText="请选择日期"
                     CompareControl="DatePicker1" CompareOperator="GreaterThanEqual"
                     CompareMessage="结束日期应该大于开始日期" ID="DatePicker2" ShowRedStar="True">
                </f:DatePicker>
                <f:TimePicker ID="TimePicker2" ShowRedStar="True" EnableEdit="false" Label="时间" Increment="30"
                    Required="true" EmptyText="请选择时间" runat="server">
                </f:TimePicker> 
                <f:NumberBox ID="numbbox_days" runat="server" Label="天数"></f:NumberBox>
                <f:NumberBox ID="numbbox_hours" runat="server"
                    NoDecimal="false" DecimalPrecision="1" Label="小时"></f:NumberBox>
                <f:RadioButtonList ID="RadioButtonList_goal" ColumnNumber="3" Label="请假类型" Required="true" runat="server">
                </f:RadioButtonList>
                <f:TextArea ID="TextArea_desc" runat="server" Label="补充说明">
                </f:TextArea>
                <f:MenuSeparator ID="MenuSeparator1" runat="server"></f:MenuSeparator>
<%--                <f:Button ID="btn_selPushUsers" runat="server" Text="推送信息至" OnClick="btn_selPushUsers_Click" ></f:Button>
                <f:TextArea ID="TextArea_userList" runat="server" ShowLabel="false">
                </f:TextArea>--%>
                <f:Button ID="btn_chooseImages" Text="添加照片" runat="server"></f:Button>
                <f:Button ID="btn_swapImages" Text="移除" runat="server"></f:Button>
                <f:ContentPanel ID="content1" runat="server" ShowHeader="false" AutoScroll="true">
                    <asp:Image ID="Image1" runat="server" Height="200px" />
                </f:ContentPanel>
<%--                <f:Button ID="btn_confirmImg" runat="server" Text="确认图片可用" OnClick="btn_confirmImg_Click"></f:Button>--%>
<%--                <f:Button ID="btn_test" runat="server" Text="推送" OnClick="btn_test_Click"></f:Button>--%>
<%--                <f:TextArea ID="TextArea_respContent" runat="server" CssClass="control_hidden"></f:TextArea>
                <f:Grid ID="Grid_pushList" runat="server" CssClass="control_hidden">
                    <Columns>
                        <f:BoundField DataField="userid" />
                    </Columns>
                </f:Grid>--%>
            </Items>
            <Toolbars>
                <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                    <Items>
                        <f:ToolbarFill runat="server"></f:ToolbarFill>
                        <f:Button runat="server" ID="btnSubmit" ConfirmText="确认提交？" Width="60px"
                             OnClick="btnSubmit_Click" ValidateForms="SimpleForm1" Text="提交">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:SimpleForm>
        <input type=hidden id="hidden_field" runat="server">
        <f:Window ID="Window1" Title="推送对象" Hidden="true" EnableIFrame="true" runat="server"
            EnableMaximize="true" EnableResize="true" Target="Self"
            IsModal="True" AutoScroll="true">
<%--            <Toolbars>
                <f:Toolbar runat="server" ID="ToolBar_window" Position="Bottom">
                    <Items>
                        <f:Button runat="server" ID="btn_windowClose" Icon="SystemClose"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>--%>
        </f:Window>
    </form>
    <script>


        wx.config({
            debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            appId: 'wx3068893beff3a241', // 必填，企业号的唯一标识，此处填写企业号corpid
            timestamp: '<%= TimeStamp %>',// 必填，生成签名的时间戳
            nonceStr: '<%= Nonce %>', // 必填，生成签名的随机串
            signature: '<%= MsgSig %>',// 必填，签名，见附录1
            jsApiList: ['chooseImage', 'previewImage', 'uploadImage', 'downloadImage'] // 必填，需要使用的JS接口列表，这里用到：拍照或从手机相册中选图，预览图片，上传图片，下载图片
        });
        //通过ready接口处理成功验证

        wx.ready(function () {
            var id = '<%=btn_chooseImages.ClientID %>';
            var btn = document.getElementById(id);
            var img = document.getElementById('<%=Image1.ClientID %>');
            var hiddenfield = document.getElementById('hidden_field')
            var btn_clear = document.getElementById('<%=btn_swapImages.ClientID %>');
            //config信息验证后会执行ready方法，所有接口调用都必须在config接口获得结果之后
            //定义images用来保存选择的本地图片ID，和上传后的服务器图片ID
            btn.onclick = function () {
                
                wx.chooseImage({
                    count: 1, // 默认9
                    sizeType: ['original', 'compressed'], // 可以指定是原图还是压缩图，默认二者都有
                    sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
                    success: function (res) {
                        var localIds = res.localIds; // 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片
                        img.src = localIds[0];//显示所选图片
                        wx.uploadImage({
                            localId: localIds[0],
                            isShowProgressTips: 1,// 默认为1，显示进度提示
                            success: function (res) {
                                hiddenfield.value = res.serverId; // 返回图片的服务器端ID
                            }
                        });

                    }

                });


            };
            
            //清除照片
            btn_clear.onclick = function () {
                img.src = null;
                hiddenfield.value = null;
            };


        });

    </script>
</body>
</html>
