<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dailywork_CheckList.aspx.cs" Inherits="WX2HK.dailyWork.dailywork_CheckList" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta name="viewport" content="maximum-scale=1.0,minimum-scale=1.0,user-scalable=0,width=device-width,initial-scale=1.0" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>审批列表-工作汇报</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowHeader="false">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:RadioButtonList ID="RadioButtonList1" ShowLabel="false" runat="server"
                             AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                            <f:RadioItem Text="未阅" Value="0" Selected="true" />
                            <f:RadioItem Text="已阅" Value="1" />
                        </f:RadioButtonList>
<%--                        <f:RadioButton ID="rbtnFirstAuto" Label="状态" Checked="true" GroupName="MyRadioGroup1"
                            Text="未阅" runat="server" OnCheckedChanged="rbtnFirstAuto_CheckedChanged" AutoPostBack="true">
                        </f:RadioButton>
                        <f:RadioButton ID="rbtnSecondAuto" GroupName="MyRadioGroup1"
                            OnCheckedChanged="rbtnFirstAuto_CheckedChanged" AutoPostBack="true" ShowEmptyLabel="true" Text="已阅" runat="server">
                        </f:RadioButton>                    --%>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Grid ID="Grid1" runat="server" ShowHeader="false" ShowGridHeader="false"
                     ExpandAllRowExpanders="true" AllowPaging="true" PageSize="25" OnPageIndexChange="Grid1_PageIndexChange">
                    <Columns>
                        <f:TemplateField RenderAsRowExpander="true">
                            <ItemTemplate>
                                <div class="expander">
                                    <p>
                                       <strong>提交人员：</strong> <%# Eval("name") %> 
                                    </p>
                                </div>
                            </ItemTemplate>     
                        </f:TemplateField>
                        <f:RowNumberField  EnablePagingNumber="true" />
                        <f:BoundField DataField="workDte" DataFormatString="{0:yyyy-MM-dd}" ExpandUnusedSpace="true"></f:BoundField>
                        <f:WindowField ColumnID="WindowField" Width="80px" WindowID="Window1"
                            Icon="PagePaintbrush" DataTextFormatString="{0}" DataIFrameUrlFields="id"
                            DataIFrameUrlFormatString="dailyWork_leaderView.aspx?mainId={0}"
                            DataWindowTitleFormatString="工作明细" />                    
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" Hidden="true" EnableIFrame="true" runat="server" AutoScroll="true"
            CloseAction="HidePostBack" EnableMaximize="false" EnableResize="false"
             Target="Top" IsModal="False">
            <Toolbars>
                <f:Toolbar ID="tlb_window" runat="server" Position="Bottom">
                    <Items>
                        <f:Button Text="关闭" ID="btn_closeWindow" runat="server" OnClick="btn_closeWindow_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:Window>

    </form>
</body>
</html>
