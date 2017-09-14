<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarMgmt_sub.aspx.cs" Inherits="WX2HK.carMgmt.CarMgmt_sub" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="maximum-scale=1.0,minimum-scale=1.0,user-scalable=0,width=device-width,initial-scale=1.0" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>测试</title>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.1.0.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>

        </div>
    </form>
    <script>

        wx.config({
            debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            appId: 'wx3068893beff3a241', // 必填，企业号的唯一标识，此处填写企业号corpid
            timestamp: '<%=car_TimeStamp %>',// 必填，生成签名的时间戳
            nonceStr: '<%= car_Nonce %>', // 必填，生成签名的随机串
            signature: '<%= car_MsgSig %>',// 必填，签名，见附录1
            jsApiList: ['getLocation'] // 必填，需要使用的JS接口列表，这里用到：获取地理位置
        });

        wx.ready(function () {
            wx.getLocation({
                type: 'wgs84', // 默认为wgs84的gps坐标，如果要返回直接给openLocation用的火星坐标，可传入'gcj02'
                success: function (res) {
                    var latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
                    var longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
                    var speed = res.speed; // 速度，以米/每秒计
                    var accuracy = res.accuracy; // 位置精度
                }
            });
        });
    </script>
</body>
</html>
