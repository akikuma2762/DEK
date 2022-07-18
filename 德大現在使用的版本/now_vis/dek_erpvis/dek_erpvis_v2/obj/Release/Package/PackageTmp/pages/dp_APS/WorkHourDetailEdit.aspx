<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="WorkHourDetailEdit.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_APS.WorkHourDetailEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>APSList | 緯凡金屬股份有限公司</title>
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-----------------title------------------>
    <div class="right_col" role="main">
        <%=Super_Link %>
        <br>
        <div class="">
            <div class="page-title">
                <div class="title_left" style="width: 100%;">
                    <h3>&nbsp <%=pagename %><small></small></h3>
                </div>
            </div>
        </div>

        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel" id="Div_Shadow">
                    <div class="x_content">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <!--  <div class="col-md-3 col-sm-4 col-xs-12">
                                群組編號：<%=G_Order %>
                            </div>-->
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                品名規格：<%=P_Order %>
                            </div>
                            <div class="col-md-3 col-sm-4 col-xs-12">
                                送料單號：<%=O_Order %>
                            </div>
                            <div class="col-md-3 col-sm-4 col-xs-12">
                                工藝名稱：<%=T_Order %>
                            </div>
                            <div class="col-md-3 col-sm-4 col-xs-12">
                                目前數量：<%=CurrentPiece %>
                            </div>
                            <div class="col-md-3 col-sm-4 col-xs-12">
                                需求數量：<%=TargetPiece %>
                            </div>
                        </div>
                        <br>
                        <br>


                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="col-md-3 col-sm-6 col-xs-12">
                                報工人員：<asp:TextBox ID="TextBox_Man" runat="server" Width="180px"></asp:TextBox>
                            </div>
                            <div class="col-md-9 col-sm-6 col-xs-12">
                                報工狀態：<%--<asp:DropDownList ID="DropDownList_Status" runat="server" Width="180px"></asp:DropDownList>--%>
                                <asp:TextBox ID="TextBox_status" runat="server" Style="display: none"></asp:TextBox>
                                <div class="btn-group" data-toggle="buttons">
                                    <%-- <label class="btn btn-default" onclick="status('段取中')">
                                        <input type="radio" name="段取中" id="段取中" class="radio" value="段取中">
                                        段取中
                                    </label>
                                    <label class="btn btn-default" onclick="status('段取完成')">
                                        <input type="radio" name="段取完成" id="段取完成" class="radio" value="段取完成">
                                        段取完成
                                    </label>
                                    <label class="btn btn-default" onclick="status('加工中')">
                                        <input type="radio" name="加工中" id="加工中" class="radio" value="加工中">
                                        加工中
                                    </label>
                                    <label class="btn btn-default" onclick="status('加工完成')">
                                        <input type="radio" name="加工完成" id="加工完成" class="radio" value="加工完成">
                                        加工完成
                                    </label>
                                    <label class="btn btn-default" onclick="status('異常')">
                                        <input type="radio" name="異常" id="異常" class="radio" value="異常">
                                        異常
                                    </label>
                                    <label class="btn btn-default" onclick="status('閒置')">
                                        <input type="radio" name="閒置" id="閒置" class="radio" value="閒置">
                                        閒置
                                    </label>--%>
                                    <%=status_button %>
                                </div>

                            </div>
                        </div>
                        <br>
                        <br>
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="col-md-3 col-sm-6 col-xs-12">
                                報工日期：<asp:TextBox ID="TextBox_Date" runat="server" TextMode="Date" Width="180px"></asp:TextBox>
                            </div>
                            <div class="col-md-3 col-sm-6 col-xs-12">
                                報工時間：<asp:TextBox ID="TextBox_Time" runat="server" TextMode="Time" Width="180px"></asp:TextBox>
                            </div>
                        </div>
                        <br>
                        <br>
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="col-md-3 col-sm-6 col-xs-12">
                                報工數量：<asp:TextBox ID="TextBox_Count" runat="server" TextMode="Number" Text="0" Width="180px"></asp:TextBox>
                            </div>
                            <div class="col-md-3 col-sm-6 col-xs-12">
                                <asp:Button ID="Button_Save" runat="server" class="btn btn-success" Text="儲存" OnClick="Button_Save_Click" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-----------------/content------------------>
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
        function status(now_status) {
            $('#ContentPlaceHolder1_TextBox_status').val('' + now_status + '');
        }
    </script>

</asp:Content>

