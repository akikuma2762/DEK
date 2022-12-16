<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Asm_Production_Progress.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PM.Asm_Production_Progress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>生產推移圖 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>

    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet"/>
    <link href="../../Content/dp_PM/waitingfortheproduction.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <!-----------------title------------------>
        <%=path %>
        <br>
        <div class="clearfix"></div>
        <!-----------------/title------------------>
        <div class="page-title">
            <div class="row">
            </div>
        </div>
        <div class="clearfix"></div>
        <!-----------------content------------------>
        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_assm" id="assm_progress" role="tab" data-toggle="tab" aria-expanded="false">立式</a>
            </li>
             <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_hor" id="hor_progress" role="tab" data-toggle="tab" aria-expanded="false">臥式</a>
            </li>
        </ul>
        <div id="myTabContent" class="tab-content">
            
            <div role="tabpanel" class="tab-pane fade active in" id="tab_assm" aria-labelledby="assm_progress">
                <div id="assmTable"></div>
            </div>
            <div role="tabpanel" class="tab-pane fade" id="tab_hor" aria-labelledby="hor_progress">
                <div id="horTable"></div>
            </div>
        </div>

        <!-----------------/content------------------>
    </div>

    <!-- /Modal -->
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
    <script src="../../assets/vendors/time/loading.js"></script>
    <!--返回上一個畫面時，要回到上一動-->
    <script src="../../assets/vendors/cookies/cookie_action.js"></script>
    <script src="../../assets/vendors/Create_HtmlCode/HtmlCode20211210.js?version = 1.1"></script>
    <script>
        //$("form input:checkbox").addClass('flat');//flat樣式使checkbox失效 2022/6/1
       
        $(document).ready(function () {
            
            //若於表格模式點選第N頁之客戶，則上一頁後返回該客戶(EX：第3頁的A客戶，按下上一頁會在第3頁)
            //return_preaction('waitingfortheproduction=waitingfortheproduction_cust', '#datatable-buttons');

            //$('#example').DataTable({
            //    responsive: true
            //});
            //$('#exampleInTab').DataTable({
            //    responsive: true
            //});

            //$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            //    $($.fn.dataTable.tables(true)).DataTable()
            //        .columns.adjust()
            //        .responsive.recalc();
            //});
        });

        //返回上一頁的時候在原來的TAB
        $(function () {
            var hash = window.location.hash;
            hash && $('ul.nav a[href="' + hash + '"]').tab('show');

            $('.nav-tabs a').click(function (e) {
                $(this).tab('show');
                var scrollmem = $('body').scrollTop() || $('html').scrollTop();
                window.location.hash = this.hash;
                $('html,body').scrollTop(scrollmem);
            });
        });

        create_tablecode('assmTable', '立式每日生產進度表', 'assm-form', '<%=Assm_th.ToString() %>', '<%=Assm_tr.ToString() %>');
        //產生相對應的JScode
        set_Table_defaulf_sort('#assm-form');
        //防止頁籤跑版
        loadpage('', '');

        create_tablecode('horTable', '臥式每日生產進度表', 'hor-form', '<%=Hor_th.ToString() %>', '<%=Hor_tr.ToString() %>');
        //產生相對應的JScode
        set_Table_defaulf_sort('#hor-form');


    </script>
</asp:Content>
