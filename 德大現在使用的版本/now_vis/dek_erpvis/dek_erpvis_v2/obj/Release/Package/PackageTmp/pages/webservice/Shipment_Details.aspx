﻿<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Shipment_Details.aspx.cs" Inherits="dek_erpvis_v2.pages.webservice.Shipment_Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=cust_name %>出貨詳細表 | 德大機械</title>
    <%=color %>
        <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main" style="height:930px;">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁 </a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=SLS">業務部</a></u></li>
            <li><u><a href="../webservice/shipment.aspx">出貨統計表</a></u></li>
            <li>出貨統計明細</li>
        </ol>
<br>
        <div class="clearfix"></div>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel" id="Div_Shadow">
                    <div class="x_title">
                        <h2><%=cust_name %>出貨詳細表<small></small></h2>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
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
        <!-----------------/content------------------>
        <!-- Modal -->
        <div id="exampleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h1>
                            <i id="dlg_title"></i>
                        </h1>
                    </div>
                    <!--不要忘記我QQ-->
                    <div class="modal-body">
                        <table id="dataTables-modal" class="table table-ts table-bordered dt-responsive nowrap " cellspacing="0" width="100%">
                            <thead>
                                <tr style="background-color:white">
                                    <th>#</th>
                                    <th>出貨日期</th>
                                   <%-- <th>訂單號碼</th>--%>
                                    <th>出貨單號</th>
                                    <th>刀庫編號</th>
                                    <th>CCS</th>
                                    
                                    <%--<th>數量</th>--%>
                                    <th>訂單備註</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <!-- /Modal -->
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
    <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
    <script>
        function GetShipment_details(dt_start, dt_end, cust_name, item_num) {
            $.ajax({
                type: 'POST',
                dataType: 'xml',
                url: "../../webservice/WebService_ToJson.asmx/GetShipment_details",
                data: { start: dt_start, end: dt_end, custom: cust_name, item: item_num }, //MR4AK0004A10,20180101,20181231
                success: function (xml) {

                    $(xml).find("ROOT_PIE").each(function (i) {
                        if ($(xml).find("ROOT_PIE").length > 0) {
                            var code = $(this).attr("item").valueOf();
                            //var name = $(this).attr("item_name").valueOf(); 
                            var thisTable = $('#dataTables-modal').dataTable();
                            thisTable.fnClearTable();
                            $(this).children().each(function (j) {
                                addData = [];
                                addData.push($(this).attr("序列").valueOf());
                                addData.push($(this).attr("出貨日期").valueOf());
                                addData.push($(this).attr("出貨單號").valueOf());
                                addData.push($(this).attr("刀庫編號").valueOf());
                                addData.push($(this).attr("CCS").valueOf());
                                addData.push($(this).attr("訂單備註").valueOf());
                                thisTable.fnAddData(addData);
                            })
                            document.getElementById("dlg_title").innerHTML = code;
                        }
                    });
                },
                error: function (data, errorThrown) {
                    alert("Fail");
                }
            });
        }
    </script>
</asp:Content>