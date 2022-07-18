<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="waitingfortheproduction_details_ITEC.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PM.waitingfortheproduction_details_ITEC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=cust_sname %>未生產詳細表 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_PM/waitingfortheproduction_details.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <!-----------------title------------------>

        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁 </a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PMD">生產部</a></u></li>
            <li><u><a href="javascript:void()" onclick="history.go(-1)">生產推移圖</a></u></li>
        </ol>

        <br>
        <div class="clearfix"></div>
        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">

                    <div class="x_content">
                        <div class="x_panel">
                            <div class="x_title">
                                <h1 class="text-center _mdTitle" style="width: 100%">
                                    <%=cust_sname %> 未結案詳細表
                                </h1>
                                <h3 class="text-center _xsTitle" style="width: 100%">
                                    <%=cust_sname %> 未結案詳細表
                                </h3>
                                <div class="clearfix"></div>
                            </div>
                            <p class="text-muted font-13 m-b-30">
                            </p>
                            <table id="datatable-buttons" class="table table-ts table-bordered nowrap" cellspacing="0" width="100%">
                                <!--<table id="datatable-responsive" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%">-->
                                <thead>
                                    <tr id="tr_row">
                                        <%=th%>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%= tr %>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-----------------/content------------------>
    </div>

    <!-- jQuery -->
    <script src="../../assets/vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="../../assets/vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- FastClick -->
    <script src="../../assets/vendors/fastclick/lib/fastclick.js"></script>
    <!-- iCheck -->
    <script src="../../assets/vendors/iCheck/icheck.min.js"></script>
    <!-- bootstrap-daterangepicker -->
    <script src="../../assets/vendors/moment/min/moment.min.js"></script>
    <script src="../../assets/vendors/bootstrap-daterangepicker/daterangepicker.js"></script>
    <!-- Switchery -->
    <script src="../../assets/vendors/switchery/dist/switchery.min.js"></script>
    <!-- Select2 -->
    <script src="../../assets/vendors/select2/dist/js/select2.full.min.js"></script>
    <!-- Autosize -->
    <script src="../../assets/vendors/autosize/dist/autosize.min.js"></script>
    <!-- jQuery autocomplete -->
    <script src="../../assets/vendors/devbridge-autocomplete/dist/jquery.autocomplete.min.js"></script>
    <!-- Custom Theme Scripts -->
    <script src="../../assets/build/js/custom.min.js"></script>
    <!-- FloatingActionButton -->
    <script src="../../assets/vendors/FloatingActionButton/js/index.js"></script>
    <!-- canvasjs -->
    <script src="../../assets/vendors/canvas_js/canvasjs.min.js"></script>
    <!-- Datatables -->
    <script src="../../assets/vendors/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="../../assets/vendors/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
    <script src="../../assets/vendors/datatables.net-buttons/js/dataTables.buttons.min.js"></script>
    <script src="../../assets/vendors/datatables.net-buttons-bs/js/buttons.bootstrap.min.js"></script>
    <script src="../../assets/vendors/datatables.net-buttons/js/buttons.flash.min.js"></script>
    <script src="../../assets/vendors/datatables.net-buttons/js/buttons.html5.min.js"></script>
    <script src="../../assets/vendors/datatables.net-buttons/js/buttons.print.min.js"></script>
    <script src="../../assets/vendors/datatables.net-responsive/js/dataTables.responsive.min.js"></script>
    <script src="../../assets/vendors/datatables.net-responsive-bs/js/responsive.bootstrap.js"></script>
    <script src="../../assets/vendors/datatables.net-scroller/js/dataTables.scroller.min.js"></script>
    <script src="../../assets/vendors/datatables.net-colReorder/dataTables.colReorder.min.js"></script>
    <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
    <script>
        $(document).ready(function () {
            $('#example').DataTable({
                responsive: true
            });
            $('#exampleInTab').DataTable({
                responsive: true
            });

            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                $($.fn.dataTable.tables(true)).DataTable()
                    .columns.adjust()
                    .responsive.recalc();
            });
        });
    </script>
</asp:Content>
