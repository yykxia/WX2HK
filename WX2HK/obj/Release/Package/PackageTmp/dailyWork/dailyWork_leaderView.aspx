<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dailyWork_leaderView.aspx.cs" Inherits="WX2HK.dailyWork.dailyWork_leaderView" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="maximum-scale=1.0,minimum-scale=1.0,user-scalable=0,width=device-width,initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>日工作明细</title>
    <style>
        .x-grid-row .x-grid-cell-inner
        {
            white-space: normal;
            word-break: break-all;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="SimpleForm1" />
        <f:SimpleForm ID="SimpleForm1" BodyPadding="5px" LabelWidth="40px" EnableCollapse="true" AutoScroll="true"
            runat="server" ShowBorder="True" ShowHeader="false">
            <Items>
                <f:Label ID="label_dept" runat="server" Label="部门"></f:Label>
                <f:Label ID="label_name" runat="server" Label="姓名"></f:Label>
                <f:Label ID="label_date" runat="server" Label="日期"></f:Label>
                <f:Label ID="label_selfGoal" Label="自评分" runat="server"></f:Label>
                <f:RadioButtonList ID="RadioButtonList_goal" Label="评分" Required="true" runat="server">
                    <f:RadioItem Text="1分" Value="1" />
                    <f:RadioItem Text="2分" Value="2" />
                    <f:RadioItem Text="3分" Value="3" />
                    <f:RadioItem Text="4分" Value="4" />
                    <f:RadioItem Text="5分" Value="5" />
                </f:RadioButtonList>
                <f:TextArea ID="TextArea_context" runat="server" Height="100px"
                        AutoGrowHeightMax="200" AutoGrowHeight="true" Label="工作 评价">
                </f:TextArea>
                <f:Label runat="server" ID="label_hidden" Hidden="true"></f:Label>
                <f:Grid ID="Grid1" CssStyle="margin-top:10px" runat="server" Title="工作内容" >
                    <Columns>
                        <f:TemplateField RenderAsRowExpander="true">
                            <ItemTemplate>
                                <div class="expander">
                                    <p>
                                       <strong>补充说明：</strong> <%# Eval("ForDeclare") %>
                                    </p>
                                </div>
                            </ItemTemplate>     
                        </f:TemplateField>

                        <f:RowNumberField />
                        <f:BoundField DataField="workDetail" HeaderText="明细" HtmlEncode="false" ExpandUnusedSpace="true" />
                    </Columns>
                </f:Grid>
            </Items>
            <Toolbars>
                <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                    <Items>
                        <f:ToolbarFill runat="server"></f:ToolbarFill>
                        <f:Button runat="server" ID="btnSubmit" ConfirmText="确认提交？" Width="56px"
                                OnClick="btnSubmit_Click" ValidateForms="SimpleForm1" Text="已 阅">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:SimpleForm>
    </form>
</body>
</html>
