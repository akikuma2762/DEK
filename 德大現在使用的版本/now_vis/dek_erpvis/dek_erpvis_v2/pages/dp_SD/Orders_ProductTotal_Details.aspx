<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Orders_ProductTotal_Details.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_SD.Orders_DayTotal_Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=workday %>  <%=LineName%> 生產明細 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/Orders_Details.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main" style="height: 930px;">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁 </a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=SLS">業務部</a></u></li>
            <li><u><a onclick="history.go(-1)">訂單數量與金額統計</a></u></li>
        </ol>
        <br>
        <div class="clearfix"></div>


        <div id="order_detail"></div>
    </div>
    <%=Use_Javascript.Quote_Javascript() %>
    <script>

        //產生表格的HTML碼
        create_tablecode('order_detail', '<%=workday %> <%=LineName%> 生產明細', 'table-form', '<%=th.ToString() %>', '<%=tr.ToString() %>');
        //產生相對應的JScode
        set_Table('#table-form');
        //防止頁籤跑版
        loadpage('', '');

    </script>
</asp:Content>
