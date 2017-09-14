<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OneImage.aspx.cs" Inherits="WX2HK.OneImage" %>

<!DOCTYPE html>

<html>
<head id="Head1" runat="server">
    <title></title>
    <link href="cloud-zoom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jqery_14.js"></script>
    <script type="text/javascript">
        $(function () {
            $('.cloud-zoom').attr('rel', 'adjustX:30');
        })
    </script>
    <script src="js/cloud-zoom.1.0.2.js" type="text/javascript"></script>
    <style type="text/css">
        body
        {
            background: #f9f9f9;
        }
        ul, li
        {
            list-style: none;
            margin: 0px;
            padding: 0px;
        }
        img
        {
            border: none;
        }
        a.cloud-zoom img
        {
            border: 1px solid #f5f5f5;
            height: 393px;
            width: 358px;
        }
        .block
        {
            margin: 30px auto;
            text-align: left;
        }
        .block ul
        {
            clear: both;
        }
        .block ul img
        {
            border: 1px solid #f5f5f5;
            float: left;
        }
        p.author
        {
            clear: both;
            text-align: center;
            color: #999;
            font-size: 11px;
        }
        p.author a
        {
            text-decoration: none;
            color: #666;
            border-bottom: 1px dashed #CCC;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="text-align: left">
    <div class="block">
        <a runat="server" href="images/1.jpg" class="cloud-zoom" id="zoom1">
            <img id="OriImg1" runat="server" src="images/s1.jpg" align="left" />
        </a>
    </div>
    </form>
</body>
</html>
