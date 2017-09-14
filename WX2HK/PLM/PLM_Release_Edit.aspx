<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLM_Release_Edit.aspx.cs" Inherits="WX2HK.PLM.PLM_Release_Edit" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" ShowHeader="false" ShowBorder="false"
             Layout="VBox">
            <Items>
                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" Height="150px"
                    AutoScroll="true" BodyPadding="10px" runat="server">
                    <Rows>
<%--                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" Label="订单号" ShowRedStar="true" ID="txb_workNo" Required="true"></f:TextBox>
                            </Items>
                        </f:FormRow>                                --%>
                        <f:FormRow>
                            <Items>
                                <f:DatePicker runat="server" Required="true" EnableEdit="false" Label="起始日期" EmptyText="请选择日期"
                                    ID="DatePicker1" ShowRedStar="True">
                                </f:DatePicker>
                                <f:TimePicker ID="TimePicker1" EnableEdit="false" Label="时间" LabelWidth="40px" Increment="15"
                                    EmptyText="请选择时间" runat="server">
                                </f:TimePicker> 
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:DatePicker runat="server" Required="true" EnableEdit="false" Label="截止日期" EmptyText="请选择日期"
                                        CompareControl="DatePicker1" CompareOperator="GreaterThanEqual"
                                        CompareMessage="结束日期应该大于开始日期" ID="DatePicker2" ShowRedStar="True">
                                </f:DatePicker>
                                <f:TimePicker ID="TimePicker2" EnableEdit="false" Label="时间" LabelWidth="40px" Increment="15"
                                    EmptyText="请选择时间" runat="server">
                                </f:TimePicker> 
                            </Items>
                        </f:FormRow>
                    </Rows>
                    <Toolbars>
                        <f:Toolbar runat="server" ID="tlb1">
                            <Items>
                                <f:Button ID="btn_filter" Text="筛选" runat="server" OnClick="btn_filter_Click"></f:Button>
                                <f:Button ID="btn_release" Text="批量下架" runat="server" OnClick="btn_release_Click" ConfirmText="确认清空当前上架数据？"></f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:Form>
                <f:Grid ID="Grid1" runat="server" ShowBorder="false" Title="上架明细" AllowSorting="true" SortField="createTime"
                     DataKeyNames="id" OnSort="Grid1_Sort" BoxFlex="1">
                    <Columns>
                        <f:BoundField DataField="BarCode" HeaderText="条码号" TextAlign="Center" Width="80px"></f:BoundField> 
                        <f:BoundField DataField="bindQty" HeaderText="上架数" TextAlign="Center" Width="60px"></f:BoundField> 
                        <f:BoundField DataField="createTime" HeaderText="操作时间" TextAlign="Center" Width="150px" SortField="createTime"></f:BoundField> 
                        <f:BoundField DataField="excUser" HeaderText="班组" TextAlign="Center" Width="60px"></f:BoundField> 
                        <f:BoundField DataField="lineName" HeaderText="产线" TextAlign="Center" Width="60px"></f:BoundField> 
                    </Columns>
                </f:Grid>                       
            </Items>
        </f:Panel>     
    </form>
</body>
</html>
