<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelfWorkDtl.aspx.cs" Inherits="WX2HK.dailyWork.SelfWorkDtl" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="maximum-scale=1.0,minimum-scale=1.0,user-scalable=0,width=device-width,initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>工作汇报</title>
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
                <f:RadioButtonList ID="RadioButtonList_goal" Label="自评" Required="true" runat="server">
                    <f:RadioItem Text="1分" Value="1" />
                    <f:RadioItem Text="2分" Value="2" />
                    <f:RadioItem Text="3分" Value="3" />
                    <f:RadioItem Text="4分" Value="4" />
                    <f:RadioItem Text="5分" Value="5" />
                </f:RadioButtonList>

                <f:DropDownList ID="ddl_resp" runat="server" Label="工作 职责" AutoPostBack="true" AutoSelectFirstItem="false"
                     EmptyText="请选择相应工作职责" OnSelectedIndexChanged="ddl_resp_SelectedIndexChanged" >

                </f:DropDownList>


                <f:Grid ID="Grid1" CssStyle="margin-top:10px" runat="server" Title="工作内容" OnRowCommand="Grid1_RowCommand" >
                    <Columns>
                        <f:BoundField DataField="id" Width="60px" HeaderText="职责ID"></f:BoundField>
                        <f:TemplateField HeaderText="工作内容" ExpandUnusedSpace="true">
                            <ItemTemplate>
                                <asp:TextBox TextMode="MultiLine" ID="tbxWorkDtl" MaxLength="500"
                                     Height="100px" runat="server" Width="100%" Text='<%# Eval("workDtl") %>'></asp:TextBox>
                            </ItemTemplate>
                        </f:TemplateField>
<%--                        <f:RenderField DataField="workDtl" ColumnID="workDtl" HtmlEncode="false" HeaderText="工作内容" FieldType="String" ExpandUnusedSpace="true">
                            <Editor>
                                <f:TextArea ID="txtareaContext" Required="true" Width="98%" runat="server"></f:TextArea>
                            </Editor>
                        </f:RenderField>--%>
                        <f:LinkButtonField ColumnID="Delete" HeaderText="&nbsp;" Width="30px"
                            Icon="Delete" />                    

                    </Columns>
                </f:Grid>
<%--                <f:TextArea ID="TextArea_context" runat="server" Height="200px"
                     AutoGrowHeightMax="300" Required="true" AutoGrowHeight="true" Label="工作 内容">
                </f:TextArea>--%>
            </Items>
            <Toolbars>
                <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                    <Items>
                        <f:Button runat="server" ID="btnSubmit" ConfirmText="确认提交？"
                             OnClick="btn_sub_Click" ValidateForms="SimpleForm1" Text="提交">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:SimpleForm>

    </form>
</body>
</html>
