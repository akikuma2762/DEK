<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="UntradedCustomer.aspx.cs" Inherits="dek_erpvis_v2.pages.webservice.UntradedCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>未交易客戶 | 德大機械</title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main" style="height:940px;">
        <!-----------------title------------------>
        <%= path %>
        <div class="page-title">
<br>
        <div class="clearfix"></div>
        <!-----------------/title------------------>
        <div class="page-title">
            <div class="row">
                <div class="col-md-6 col-sm-12 col-xs-12">
                    <h4>[搜尋條件]未交易天數：<u><asp:LinkButton ID="LinkButton1" runat="server" data-toggle="modal" data-target="#exampleModal"><%=timerange %>天</asp:LinkButton></u></h4>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="dashboard_graph  x_panel"  id="Div_Shadow">
                    <div class="x_title">
                        <h2>未交易客戶列表<small></small></h2>
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
    </div>
    <!-- set Modal -->
 <!--   <div class="backdrop">
    </div>
    <div class="fab child" data-subitem="1" data-toggle="modal" data-target="#exampleModal">
        <span>
            <i class="fa fa-search"></i>
        </span>
    </div>
    <div class="fab" id="masterfab">
        <span>
            <i class="fa fa-list-ul"></i>
        </span>
    </div>
    <!--/set Modal-->
    <!-- Modal -->
    <div id="exampleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title modaltextstyle" id="myModalLabel2"><i class="fa fa-file-text"></i>資料檢索精靈</h4>
                </div>
                <div class="modal-body">
                    <div id="testmodal2" style="padding: 5px 20px;">
                        <div class="form-group">
                            <h5 class="modaltextstyle">
                                <i class="fa fa-caret-down"><b>客戶未交易天數</b></i><i id="cbx_remind_day"></i>
                            </h5>
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-5 ">
                                    <asp:DropDownList ID="DropDownList_selectedcondi" runat="server" class="form-control">
                                        <asp:ListItem Value="">---請選擇---</asp:ListItem>
                                        <asp:ListItem Value=">" Selected="True">大於</asp:ListItem>
                                        <asp:ListItem Value="<">小於</asp:ListItem>
                                        <asp:ListItem Value="=">等於</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-7 ">
                                    <div class="form-group">
                                        <asp:TextBox ID="TextBox_dayval" runat="server" class="form-control" Text="365" placeholder="365"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <h5 class="modaltextstyle">
                                <i class="fa fa-caret-down"><b>未交易期間</b> <small><b>(ex:yyyyMMdd)</b></small></i> <i id="cbx_remind"></i>
                            </h5>
                            <fieldset>
                                <div class="control-group">
                                    <div class="controls">
                                        <div class="col-md-11 xdisplay_inputx form-group has-feedback">
                                            <input type="text" class="form-control has-feedback-left" id="txt_time_str" name="txt_time_str" runat="server" value="" placeholder="開始日期" aria-describedby="inputSuccess2Status1" autocomplete="off">
                                            <span class="fa fa-calendar-o form-control-feedback left" aria-hidden="true"></span>
                                            <span id="inputSuccess2Status1" class="sr-only">(success)</span>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                            <fieldset>
                                <div class="control-group">
                                    <div class="controls">
                                        <div class="col-md-11 xdisplay_inputx form-group has-feedback">
                                            <input type="text" class="form-control has-feedback-left" id="txt_time_end" name="txt_time_end" value="" runat="server" placeholder="結束日期" aria-describedby="inputSuccess2Status2" autocomplete="off">
                                            <span class="fa fa-calendar-o form-control-feedback left" aria-hidden="true"></span>
                                            <span id="inputSuccess2Status2" class="sr-only">(success)</span>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default antoclose2" data-dismiss="modal">退出作業</button>
                    <asp:Button ID="button_select" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="button_select_Click" Style="display: none" />
                    <button id="btncheck" type="button" class="btn btn-primary antosubmit2">執行運算</button>
                </div>
            </div>
        </div>
    </div>
    <!-- /Modal -->
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
        $("#btncheck").click(function () {
            var start_time = document.getElementsByName("ctl00$ContentPlaceHolder1$txt_time_str")[0].value;
            var end_time = document.getElementsByName("ctl00$ContentPlaceHolder1$txt_time_end")[0].value;
            var dropdown = document.getElementsByName("ctl00$ContentPlaceHolder1$DropDownList_selectedcondi")[0].value;
            var day_val = document.getElementsByName("ctl00$ContentPlaceHolder1$TextBox_dayval")[0].value;

            if (start_time != "" && end_time != "") { //檢查日期區間
                var re = /^[0-9]+$/;
                if (!re.test(start_time) && !re.test(end_time)) {
                    var remind = document.getElementById("cbx_remind");
                    remind.innerHTML = "只能輸入數字 !";
                    remind.style.color = "#FF3333";
                }
                else {
                    if (start_time.length != 8 || end_time.length != 8) {
                        var remind = document.getElementById("cbx_remind");
                        remind.innerHTML = "輸入日期格式有誤,請重新檢查 ! !";
                        remind.style.color = "#FF3333";
                    } else {
                        if (start_time < end_time) {
                            document.getElementById('<%=button_select.ClientID %>').click();
                        }
                        else {
                            var remind = document.getElementById("cbx_remind");
                            remind.innerHTML = "起始日期有誤,請重新檢查 !";
                            remind.style.color = "#FF3333";
                        }
                    }
                }
            } else if (((start_time != "") && (end_time == "")) || ((start_time == "") && (end_time != ""))) {
                var remind = document.getElementById("cbx_remind");
                remind.innerHTML = "日期不得為空,請重新檢查 !";
                remind.style.color = "#FF3333";
            } else if (dropdown != "" && day_val != "") {
                document.getElementById('<%=button_select.ClientID %>').click();
            } else if (dropdown != "0" && day_val == "") {
                var remind = document.getElementById("cbx_remind_day");
                remind.innerHTML = "天數不得為空,請重新檢查 !";
                remind.style.color = "#FF3333";
            };
        });
    </script>
</asp:Content>
