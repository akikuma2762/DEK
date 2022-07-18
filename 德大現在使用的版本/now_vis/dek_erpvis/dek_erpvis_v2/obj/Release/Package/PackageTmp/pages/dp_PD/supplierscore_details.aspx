<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="supplierscore_details.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PD.supplierscore_details_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=sup_sname %>達交率 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet"/>
    <link href="../../Content/dp_PD/supplierscore_details.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main" style="height: 930px;">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁 </a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PCD">資材部</a></u></li>
            <li><u><a href="javascript:void()" onclick="history.go(-1)">供應商達交率</a></u></li>
        </ol>
        <br>
        <div id="supplierscore_details"></div>
    </div>
    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        //產生表格的HTML碼
        create_tablecode('supplierscore_details', '<%=sup_sname %>達交率明細表', 'table-form', '<%=th.ToString() %>', '<%=tr.ToString() %>');
        //產生相對應的JScode
        set_Table('#table-form');
        //防止頁籤跑版
        loadpage('', '');
    </script>
</asp:Content>
