<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaveOut_subList.aspx.cs" Inherits="WX2HK.CheckOut.LeaveOut_subList" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="maximum-scale=1.0,minimum-scale=1.0,user-scalable=0,width=device-width,initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>审批列表-考勤</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowHeader="false">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:RadioButton ID="rbtnFirstAuto" Label="状态" Checked="true" GroupName="MyRadioGroup1"
                            Text="未审批" runat="server" OnCheckedChanged="rbtnFirstAuto_CheckedChanged" AutoPostBack="true">
                        </f:RadioButton>
                        <f:RadioButton ID="rbtnSecondAuto" GroupName="MyRadioGroup1"
                            OnCheckedChanged="rbtnFirstAuto_CheckedChanged" AutoPostBack="true" ShowEmptyLabel="true" Text="已审批" runat="server">
                        </f:RadioButton>                    
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
                                       <strong>申请日期：</strong> <%# Eval("reqDte") %> <strong>申请人：</strong><%# Eval("name") %>
                                    </p>
                                </div>
                            </ItemTemplate>     
                        </f:TemplateField>
                        <f:RowNumberField  EnablePagingNumber="true" />
                        <f:BoundField DataField="viewTyp" ExpandUnusedSpace="true"></f:BoundField>
<%--                        <f:HyperLinkField DataTextField="viewTyp" DataTextFormatString="{0}" ExpandUnusedSpace="true" Target="_self"
                            DataNavigateUrlFields="tabId,linkPage" DataNavigateUrlFormatString="https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx3068893beff3a241&redirect_uri=http%3a%2f%2fhkoa.hkfoam.com%3a30018%2fCheckOut%2f{1}.aspx?mainId={0}&response_type=code&scope=SCOPE&state=STATE#wechat_redirect" />--%>
                        <f:WindowField ColumnID="myWindowField" Width="80px" WindowID="Window1"
                            Icon="Pencil" DataTextFormatString="{0}" DataIFrameUrlFields="tabId,linkPage,sort"
                            DataIFrameUrlFormatString="{1}.aspx?mainId={0}&spsort={2}" DataWindowTitleField="viewTyp"
                            DataWindowTitleFormatString="{0}-明细" />                    

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
