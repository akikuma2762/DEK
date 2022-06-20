<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Asm_ErrorSearch.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PM.Asm_ErrorSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>維護歷史搜尋 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <link href="../../Content/dp_PM/Asm_ErrorSearch.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁</a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PMD">生產部</a></u></li>
            <li><u><a href="Asm_ErrorSearch.aspx">組裝異常歷史查詢</a></u></li>
            <br />
            <div class="row">
                <div class="col-md-10 col-sm-10 col-xs-10" style="text-align: center; font-size: 27px">
                </div>
                <div class="col-md-2 col-sm-12 col-xs-12">
                    <li style="position: absolute; left: 0">
                        <input name="ctl00$ContentPlaceHolder1$bt_Ver" type="submit" id="ContentPlaceHolder1_bt_Ver" class="btn btn-primary" value="立式">
                        <input name="ctl00$ContentPlaceHolder1$bt_Hor" type="submit" id="ContentPlaceHolder1_bt_Hor" class="btn btn-warning" value="臥式">
                    </li>
                    <br />
                </div>
                <br />
        </ol>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">

                    <div class="clearfix"></div>

                    <div class="x_panel" id="Div_Shadow">
                        <div class="x_content">
                            <!--<form id="demo-form2" class="form-horizontal form-label-left">-->
                            <div class="form-group">
                                <label class="control-label3 " for="Process-Status">刀庫編號[關鍵字搜尋]<span class="required">:</span></label>
                                <div class="input-group">
                                    <input id="Mant_Search" runat="server" type="text" name="Mant_Search" class="form-control" placeholder="EX:C4AQV-180000" onkeydown="Unnamed_ServerClick">
                                    <span class="input-group-btn">
                                        <input type="button" class="btn btn-primary" name="Mant_Btn" runat="server" value="搜尋" onserverclick="Unnamed_ServerClick" />
                                    </span>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label2 " for="Process-Line">產線名稱<span class="required">:</span></label>
                                <div class="">
                                    <asp:DropDownList ID="DropDownList_Line" runat="server" class="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label3 " for="Process-ErrorType">異常類型<span class="required">:</span></label>
                                <div class="">
                                    <asp:DropDownList ID="DropDownList_ErrorType" runat="server" class="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <!--</form>-->
                            <div class="clearfix"></div>
                        </div>
                    </div>


                    <div class="clearfix"></div>
                    <div class="x_panel" id="Div_Shadow">
                        <div class="x_content">
                            <table id="datatable-buttons" class="table  table-ts table-bordered nowrap" border="1" cellspacing="0" width="100%">
                                <thead>
                                    <tr id="tr_row">
                                        <%=ColumnsData %>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%=RowsData %>
                                </tbody>
                            </table>
                        </div>
                    </div>


                </div>
            </div>
        </div>
    </div>
    <!-----------------/content------------------>
    <!-- 20191218總經理覺得佔位置，所以移除 </div>
     <div class="fab child" data-subitem="1" data-toggle="modal" data-target="#exampleModal">
        <span>
            <i class="fa fa-search"></i>
        </span>
    </div>
    <div class="fab" id="masterfab">
        <span>
            <i class="fa fa-list-ul"></i>
        </span>
    </div>-->
    <!--/set Modal-->
    <!-- Modal1 -->
    <div id="exampleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title modaltextstyle" id="myModalLabel2"><i class="fa fa-file-text"></i>選單功能</h4>
                </div>
                <div class="modal-body">
                    <div id="testmodal2" style="padding: 5px 20px;">
                        <div class="form-group">

                            <h5 class="modaltextstyle">
                                <i class="fa fa-caret-down">廠區選擇</i> <i id="cbx_remind_fast"></i>
                            </h5>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="col-md-11 xdisplay_inputx form-group has-feedback">
                                        <input id="bt_Ver" type="submit" class="btn btn-primary" value="立式" runat="server" onserverclick="bt_Ver_ServerClick" />
                                        <input id="bt_Hor" type="submit" class="btn btn-warning" value="臥式" runat="server" onserverclick="bt_Ver_ServerClick" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
    <!-- set Modal -->
    <!-- set Modal -->
    <!--/set Modal-->
    <!-- Modal -->
    <!-- /Modal -->
    <!-- jQuery -->
    <script src="../../assets/vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="../../assets/vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- bootstrap-progressbar -->
    <script src="../../assets/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js"></script>
    <!-- bootstrap-daterangepicker -->
    <script src="../../assets/vendors/moment/min/moment.min.js"></script>
    <script src="../../assets/vendors/bootstrap-daterangepicker/daterangepicker.js"></script>

    <!-- iCheck -->
    <script src="../../assets/vendors/iCheck/icheck.min.js"></script>
    <!-- bootstrap-wysiwyg -->
    <script src="../../assets/vendors/bootstrap-wysiwyg/js/bootstrap-wysiwyg.min.js"></script>
    <script src="../../assets/vendors/jquery.hotkeys/jquery.hotkeys.js"></script>
    <script src="../../assets/vendors/google-code-prettify/src/prettify.js"></script>
    <!-- jQuery Tags Input -->
    <script src="../../assets/vendors/jquery.tagsinput/src/jquery.tagsinput.js"></script>
    <!-- Switchery -->
    <script src="../../assets/vendors/switchery/dist/switchery.min.js"></script>
    <!-- Select2 -->
    <script src="../../assets/vendors/select2/dist/js/select2.full.min.js"></script>
    <!-- Parsley -->
    <script src="../../assets/vendors/parsleyjs/dist/parsley.min.js"></script>
    <!-- Autosize -->
    <script src="../../assets/vendors/autosize/dist/autosize.min.js"></script>
    <!-- jQuery autocomplete -->
    <script src="../../assets/vendors/devbridge-autocomplete/dist/jquery.autocomplete.min.js"></script>
    <!-- starrr -->
    <script src="../../assets/vendors/starrr/dist/starrr.js"></script>
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
        // $('#test').keydown(function (event) {
        //     alert(event.which);
        //     if (event.which == 13) {
        //         alert("按下Enter了");
        //     }
        // });        //防止切換頁籤時跑版
        $(document).ready(function () {
            $('#example').DataTable({
                responsive: true
            });
            $('#exampleInTab').DataTable({
                responsive: true
            });

            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                $($.fn.dataTable.tables(true)).DataTable()
                    .columns.adjust();
            });
        });

    </script>
</asp:Content>
