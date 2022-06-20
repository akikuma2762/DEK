<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="recordsofchangetheorder_details.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_SD.recordsofchangetheorder_details_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=cust_name %> 訂單變更歷程 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/recordsofchangetheorder_details.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁 </a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=SLS">業務部</a></u></li>
            <li><u><a href="javascript:void()" onclick="history.go(-1)">訂單變更紀錄</a></u></li>
        </ol>
        <br>
        <div class="clearfix"></div>
        <div id="_List" class="row top_tiles">
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-6">
                <div class="tile-stats dashboard_graph x_panel Div_Shadow">
                    <div class="icon"><i class="fa fa-file-text-o"></i></div>
                    <div class="h3 text-secondary">強迫結案總計</div>
                    <div class="count text-success"><%=HTML_客戶單號變更總次數 %> 次</div>
                    <p></p>
                </div>
            </div>
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-6">
                <div class="tile-stats dashboard_graph x_panel Div_Shadow">
                    <div class="icon"><i class="fa fa-cube"></i></div>
                    <div class="h3 text-secondary">品號變更總計</div>
                    <div class="count text-success"><%=HTML_品號變更總次數 %> 次</div>
                    <p></p>
                </div>
            </div>
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-6">
                <div class="tile-stats dashboard_graph x_panel Div_Shadow">
                    <div class="icon"><i class="fa fa-cart-plus"></i></div>
                    <div class="h3 text-secondary">數量變更總計</div>
                    <div class="count text-success"><%=HTML_數量變更總次數 %> 次</div>
                    <p></p>
                </div>
            </div>
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-6">
                <div class="tile-stats dashboard_graph x_panel Div_Shadow">
                    <div class="icon"><i class="fa fa-calendar"></i></div>
                    <div class="h3 text-secondary ">交期變更總計</div>
                    <div class="count text-success"><%=HTML_交期變更總次數 %> 次</div>
                    <p></p>
                </div>
            </div>
        </div>
        <div id="recordsofchangetheorder_details"></div>
    </div>
    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        //產生表格的HTML碼
        create_tablecode('recordsofchangetheorder_details', '<%=cust_name %>訂單變更歷程', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString() %>');
        //產生相對應的JScode
        set_Table('#datatable-buttons');
        //防止頁籤跑版
        loadpage('', '');
    </script>
</asp:Content>
