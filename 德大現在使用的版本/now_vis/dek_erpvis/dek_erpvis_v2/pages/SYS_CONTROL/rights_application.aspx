﻿<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="rights_application.aspx.cs" Inherits="dek_erpvis_v2.pages.SYS_CONTROL.rights_application" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=page_name %> | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
     <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <link href="../../Content/index.css" rel="stylesheet" />
    <link href="../../Content/phone.css" rel="stylesheet" media="screen and (max-width:768px)" />
    <link href="../../Content/table.css" rel="stylesheet" media="screen and (max-width:768px)" />
    <style>
        .Div_Shadow {
            box-shadow: 3px 3px 9px gray;
        }
        @media screen and (max-width:768px) {
            .Div_Shadow {
                box-shadow: none;
            }
            #btn_delete ,#btn_submit{
                width:47%;
            }
            #_Application {
                font-size:0.7em;
            }
            .x_panel , ._checkbtn{
                padding:0;
            }
            .x_panel>div.x_title h2 ,.x_panel > div.x_content p{
                font-size:0.8em;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="">
                    <h2>&nbsp <%=page_name %></h2>
                </div>

                <div class="title_right">
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-8 col-sm-8 col-xs-12">
                    <div class="x_panel Div_Shadow">
                        <div class="x_title">
                            <h2>系統用戶瀏覽申請表單</h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <p><%=notice %></p>
                            <div class="row text-right">
                                <div class="col-md-12 col-sm-12 col-xs-12 _checkbtn">
                                    <button id="btn_delete" type="button" class="btn btn-default">拒絕</button>
                                    <asp:Button ID="Button_delete" runat="server" Text="" style="display: none;" OnClick="Button_delete_Click" />
                                    <button id="btn_submit" type="button" class="btn btn-success">核准</button>
                                    <asp:Button ID="Button_submit" runat="server" Text="" style="display: none;" OnClick="Button_submit_Click" />
                                </div>
                            </div>
                            <div id="_Application" class="table-responsive">
                                <table class="table table-striped jambo_table bulk_action">
                                    <thead>
                                        <tr id="tr_row"  class="headings">
                                            <th>
                                                <input type="checkbox" id="check-all" class="flat">
                                            </th>
                                            <th class="column-title">所屬部門 </th>
                                            <th class="column-title">用戶名稱 </th>
                                            <th class="column-title">用戶帳號 </th>
                                            <th class="column-title">申請頁面 </th>
                                            <th class="bulk-actions" colspan="7">
                                                <a class="antoo" style="color: #fff; font-weight: 500;">批量選取 ( <span class="action-cnt"></span>) <i class="fa fa-chevron-down"></i></a>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%=set_table_content() %>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- jQuery -->
    <script src="../../assets/vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="../../assets/vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- FastClick -->
    <script src="../../assets/vendors/fastclick/lib/fastclick.js"></script>
    <!-- NProgress -->
    <script src="../../assets/vendors/nprogress/nprogress.js"></script>
    <!-- bootstrap-progressbar -->
    <script src="../../assets/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js"></script>
    <!-- iCheck -->
    <script src="../../assets/vendors/iCheck/icheck.min.js"></script>
    <!-- bootstrap-daterangepicker -->
    <script src="../../assets/vendors/moment/min/moment.min.js"></script>
    <script src="../../assets/vendors/bootstrap-daterangepicker/daterangepicker.js"></script>
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
    <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
    <script>
        function checked_count() {
            var selectedValues = [];
            $("form input:checked").each(function () {
                selectedValues.push($(this).val());
            });
            if (selectedValues[0] == "on") {
                delete selectedValues[0];
            }
            if (selectedValues.length == 0) {
                alert("至少選一項!");
                return 0;
            } else {
                return 1;
            }
        }
        $("#btn_submit").click(function () {
            var ans = checked_count();
            if (ans >= 1) {
                document.getElementById('<%=Button_submit.ClientID %>').click();
            }
        })
        $("#btn_delete").click(function () {
            var ans = checked_count();
            if (ans >= 1) {
                document.getElementById('<%=Button_delete.ClientID %>').click();
            }
        })
    </script>
</asp:Content>
