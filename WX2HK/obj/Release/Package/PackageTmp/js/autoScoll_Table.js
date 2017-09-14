function tableShow(tableId, divId) {
    var sum_WH = 17;
    var divHeight = document.getElementById(divId).clientHeight;
    var table = document.getElementById(tableId);
    var rows = table.rows.length;//table行数
    if ($("#" + tableId).height() > divHeight) { //divHeight_WQ为DIV的高度
        scroll();
    }
    function scroll() {
        t = setInterval("show()", 1000) //1000滚动时间间隔为1秒 如要改动，下面1000也要跟着改动
        $("#"+tableId).mouseover(function () {
            clearInterval(t);
        }).mouseout(function () {
            t = setInterval("show()", 1000);
        });
    }
    function show() {
        $("#" + tableId + " tr:first").appendTo($("#" + tableId));
        sum = sum + 1;
        //if (sum == rows_WQ) {         //rows_WQ为table数据总行数
        //    setTimeout(function () {
        //        window.location.reload();
        //    }, 500); //500为滚动到最后一条之后0.5秒后刷新页面
        //}
    }
}