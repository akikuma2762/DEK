﻿<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Business_Report.aspx.cs" Inherits="dek_erpvis_v2.pages.SYS_CONTROL.Business_Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>業務報告 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <link href="../../assets/build/css/custom.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
            <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁 </a></u></li>
            <li><u><a href="../SYS_CONTROL/Business_Report.aspx">業務報告</a></u></li>
        </ol>
        <!-----------------title------------------>
        <div class="page-title">
            <div class="title_left">
                <h3>業務報告</h3>
            </div>
        </div>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <div class="row">
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <div style="width:60%"><h2>選取日期<%=date_str %> - <%=date_end %><small></small></h2></div>
                        <asp:Button ID="buttonjump" runat="server" Text="新增文章" class="btn btn-secondary" OnClick="button_jump_Click" Style="display: none" />
                    <button id="btnjump" type="button" class="btn btn-success" style="position:absolute;right:0">新增文章</button>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <p class="text-muted font-13 m-b-30">
                        </p>
                        <table id="datatable-buttons" class="table table-active table-striped table-hover" cellspacing="0" width="100%" valign="center">
                            <thead>
                                <tr style="display:none">
                                    <%=th %>
                                </tr>
                            </thead>
                            <tbody>
                                <%=tr %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <!-----------------/content------------------>
    </div>
    <!-- set Modal -->
    <div class="backdrop">
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
                                <i class="fa fa-caret-down">搜尋標題關鍵字</i>
                            </h5>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <asp:TextBox ID="textbox_item" CssClass="form-control" Text="" runat="server" placeholder="請輸入關鍵字"></asp:TextBox>
                                </div>
                            </div>
                            <h5 class="modaltextstyle">
                            <i class="fa fa-caret-down">類型</i><i id="cbx_remind_day"></i>
                        </h5>
                        <div class="row">
                            <div class="col-md-12 col-sm-4 col-xs-12 ">
                                <asp:DropDownList ID="DropDownList_selectedcondi" runat="server" class="form-control">

                                </asp:DropDownList>
                            </div>
                            </div>

                            <h5 class="modaltextstyle">
                            <i class="fa fa-caret-down">公告部門</i><i id="cbx_remind_day"></i>
                        </h5>
                        <div class="row">
                            <div class="col-md-12 col-sm-4 col-xs-12 ">
                                <asp:DropDownList ID="DropDownList_Part" runat="server" class="form-control">
       
                                </asp:DropDownList>
                            </div>
                            </div>

                            <h5 class="modaltextstyle">
                                <i class="fa fa-caret-down">日期快選</i> <i id="cbx_remind"></i>
                            </h5>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <asp:LinkButton ID="LinkButton_month" runat="server" CssClass="btn btn-default " OnClick="button_select_Click">月</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton_firsthalf" runat="server" class="btn btn-default " OnClick="button_select_Click">上半年</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton_lasthalf" runat="server" class="btn btn-default " OnClick="button_select_Click">下半年</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton_year" runat="server" class="btn btn-default " OnClick="button_select_Click">全年</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <h5 class="modaltextstyle">
                                <i class="fa fa-caret-down">最後更新日期 <small>(ex:yyyyMMdd)</small></i>
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
    <!-- bootstrap-touchspin-master -->
    <script src="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.js"></script>
    <!-- Datatables -->
    <script src="../../assets/vendors/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="../../assets/vendors/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
    <script src="../../assets/vendors/datatables.net-buttons/js/dataTables.buttons.js"></script>
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
        $("#btnjump").click(function()
        { 
            document.getElementById('<%=buttonjump.ClientID %>').click();

        });

        $("#btncheck").click(function () {
            var start_time = document.getElementsByName("ctl00$ContentPlaceHolder1$txt_time_str")[0].value;
            var end_time = document.getElementsByName("ctl00$ContentPlaceHolder1$txt_time_end")[0].value;
            if (start_time != "" && end_time != "") {
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
            } else {
                var remind = document.getElementById("cbx_remind");
                remind.innerHTML = "日期不得為空,請重新檢查 !";
                remind.style.color = "#FF3333";
            }
        });
        $(function () {
            $('#ContentPlaceHolder1_txt_time_str,#ContentPlaceHolder1_txt_time_end').daterangepicker({
                singleDatePicker: true,
                autoUpdateInput: false,
                singleClasses: "picker_3",
                locale: {
                    cancelLabel: 'Clear',
                    daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
                    monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
                }
            });
            $('#ContentPlaceHolder1_txt_time_str,#ContentPlaceHolder1_txt_time_end').on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format('YYYYMMDD'));
            });
            $('#ContentPlaceHolder1_txt_time_str,#ContentPlaceHolder1_txt_time_end').on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
            });
        });

    </script>
</asp:Content>
