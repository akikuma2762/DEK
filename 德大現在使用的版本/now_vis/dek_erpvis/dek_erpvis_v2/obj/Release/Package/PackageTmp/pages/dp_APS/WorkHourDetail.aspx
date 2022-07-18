<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="WorkHourDetail.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_APS.WorkHourDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>APSList | 緯凡金屬股份有限公司</title>
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-----------------title------------------>
    <div class="right_col" role="main" >
        <%=Super_Link %>
        <br>
        <div class="">
            <div class="page-title">
                <div class="title_left" style="width: 100%;">
                    <h3>&nbsp 報工明細<small></small></h3>
                </div>
            </div>
        </div>
        <asp:TextBox ID="TextBox_textTemp" runat="server" Visible="true" Width="0" Style="display: none"></asp:TextBox>
        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel" id="Div_Shadow">

                    <div class="x_content">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                群組編號：<%=G_Order %>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                送料單號：<%=O_Order %>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                品名規格：<%=N_Order %>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                工藝名稱：<%=T_Order %>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <asp:Button ID="Button_Add" runat="server" Text="新增報工" Class="btn btn-success" OnClick="Button_Add_Click" Style="display:none" />
                            </div>
                        </div>
                        <br>
                        <hr>

                        <table id="datatable-buttons" class="table table-bordered nowrap table-ts" cellspacing="0" width="100%">
                            <thead>
                                <tr style="background-color: #FFFFFF">
                                    <%=th%>
                                </tr>
                            </thead>
                            <tbody>
                                <%=tr %>
                            </tbody>
                        </table>
                        <asp:TextBox ID="TextBox1" runat="server" Visible="true" Width="0" Style="display: none"></asp:TextBox>
                        <asp:Button ID="button_delete" runat="server" Text="搜尋" class="btn btn-secondary" OnClick="button_delete_Click" Style="display: none" />
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
    <!-- NProgress -->
    <script src="../../assets/vendors/nprogress/nprogress.js"></script>
    <!-- bootstrap-progressbar -->
    <script src="../../assets/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js"></script>
    <!-- moment -->
    <script src="../../assets/vendors/moment/min/moment.min.js"></script>
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
        function get_id(id) {
            var r = confirm("確定要刪除該筆資料嗎");
            if (r == true) {
                $('#ContentPlaceHolder1_TextBox_textTemp').val('' + id + '');
                document.getElementById('<%=button_delete.ClientID %>').click();
            }
        }
    </script>

</asp:Content>
