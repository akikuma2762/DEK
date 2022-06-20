<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="waitingfortheproduction.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_SD.waitingfortheproduction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>未生產分析 | 德大機械</title>
    <link href="../../assets/build/css/custom.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <%=path %>
        <div class="page-title">
            <div class="title_left">
                <h3>未生產分析</h3>
            </div>
            <div class="title_right">
            </div>
        </div>
        <div class="clearfix"></div>
        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="dashboard_graph x_panel">
                    <div class="x_content">
                        <div class="row">
                            <div class="col-md-9 col-sm-12 col-xs-12">
                                <div id="chartContainer" style="height: 500px; max-width: 100%;"></div>
                            </div>
                            <div class="col-md-3 col-sm-12 col-xs-12 text-center">
                                    <hr>
                                <div class="col-md-12 col-sm-3 col-xs-3">
                                    <div class="h2 mb-0 text-primary" style="margin-bottom: 10px;"><%=當前進度 %></div>
                                    <div class="text-muted">當前進度</div>
                                    <hr>
                                </div>
                                <div class="col-md-12 col-sm-3 col-xs-3">
                                    <div class="h2 mb-0 text-warning" style="margin-bottom: 10px;"><%=本月剩餘 %></div>
                                    <div class="text-muted">本月餘剩(<%= to_today %>/<%= total_days %>)</div>
                                    <hr>
                                </div>
                                <div class="col-md-12 col-sm-3 col-xs-3">
                                    <div class="h2 mb-0 text-success" style="margin-bottom: 10px;"><%=生產進度 %></div>
                                    <div class="text-muted">生產進度(<%=實際生產_data_y_max %>/<%=預定生產_data_y_max %>)</div>
                                    <hr>
                                </div>
                                <div class="col-md-12 col-sm-3 col-xs-3">
                                    <div class="h2 mb-0 text-success" style="margin-bottom: 10px;"><%=日均產量_實際生產 %>/<%=日均產量_預定生產 %></div>
                                    <div class="text-muted">日均產量(實際/預估)</div>
                                    <hr>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>未生產列表<small></small></h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li>
                                <a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <p class="text-muted font-13 m-b-30">
                        </p>
                            <table id="datatable" class="table table-striped table-bordered nowrap" cellspacing="0" width="100%">
                            <!--<table id="datatable-responsive" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%">-->
                            <thead>
                                <tr>
                                    <%=th%>
                                </tr>
                            </thead>
                            <tbody>
                                <%= set_table_content() %>
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
                    <h4 class="modal-title modaltextstyle" id="myModalLabel2"><i class="fa fa-file-text"></i> 資料檢索精靈</h4>
                </div>
                <div class="modal-body">
                    <div id="testmodal2" style="padding: 5px 20px;">
                        <div class="form-group" style="font-family: Microsoft JhengHei;font-weight: bold;font-size: 25px;">
                            <h5 class="modaltextstyle">
                                <i class="fa fa-caret-down">選取產線</i>
                            </h5>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <asp:DropDownList ID="DropDownList_LINE" runat="server" class="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified ">
                                        <asp:CheckBoxList ID="CheckBoxList_line" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                            <h5 class="modaltextstyle">
                                <i class="fa fa-caret-down">區間快選</i> <i id="cbx_remind"></i>
                            </h5>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified ">
                                        <asp:LinkButton ID="LinkButton_day" runat="server" CssClass="btn btn-default" OnClick="button_select_Click">日</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton_week" runat="server" CssClass="btn btn-default " OnClick="button_select_Click">週</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton_month" runat="server" CssClass="btn btn-default " OnClick="button_select_Click">月</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton_firsthalf" runat="server" class="btn btn-default " OnClick="button_select_Click">上半年</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton_lasthalf" runat="server" class="btn btn-default " OnClick="button_select_Click">下半年</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton_year" runat="server" class="btn btn-default " OnClick="button_select_Click">全年</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <h5 class="modaltextstyle">
                                <i class="fa fa-caret-down">自定義區間 (ex:yyyyMMdd)</i> <i id="cbx_remind"></i>
                            </h5>
                            <fieldset>
                                <div class="control-group">
                                    <div class="controls">
                                        <div class="col-md-11 xdisplay_inputx form-group has-feedback">
                                            <input type="text" class="form-control has-feedback-left" id="txt_time_str" name="txt_time_str" runat="server" value="" placeholder="開始時間" aria-describedby="inputSuccess2Status1" autocomplete="off">
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
                                            <input type="text" class="form-control has-feedback-left" id="txt_time_end" name="txt_time_end" value="" runat="server" placeholder="結束時間" aria-describedby="inputSuccess2Status2" autocomplete="off">
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
        var chart = new CanvasJS.Chart("chartContainer", {
            animationEnabled: true,
            theme: "light2",
            title: {
                text: <%= titel_text %>,
            },
            axisX: {
                interval: 1
            },
            axisY: {
                lineThickness: 1,
                lineColor: "#d0d0d0",
                gridColor: "transparent",
            },
            toolTip: {
                shared: true
            },
            legend: {
                fontSize: 15,
                cursor: "pointer",
                /*verticalAlign: "center",
                horizontalAlign: "right"*/
            },
            data: [{
                type: "column",
                name: "應生產",
                showInLegend: true,
                dataPoints: [
                        <%=預定生產_data%>
                    ]
                }, {
                        type: "column",
                        name: "實生產",
                        showInLegend: true,
                        dataPoints: [
                        <%=實際生產_data%>
                    ]
                },
            ]
        });
        chart.render();
        createPareto_1();
        createPareto_2();
        function createPareto_1() {
            var dps = [];
            var yValue = 0;
            for (var i = 0; i < chart.data[0].dataPoints.length; i++) {
                yValue += chart.data[0].dataPoints[i].y;

                if (i == (chart.data[0].dataPoints.length - 1)) {
                    dps.push({
                        label: chart.data[0].dataPoints[i].label,
                        y: yValue,
                        //indexLabel: ""//"生產目標:" + yValue
                    });

                } else {
                    dps.push({
                        label: chart.data[0].dataPoints[i].label,
                        y: yValue
                    });
                }
            }
            chart.addTo("data", {
                type: "line",
                name: "累積生產目標",
                markerSize: 0,
                indexLabelFontSize: 20,
                showInLegend: true,
                dataPoints: dps
            });
            count_0 = yValue;
        }
        function createPareto_2() {
            var dps = [];
            var yValue = 0;
            var _markerType = "";
            var _markerColor = "";

            for (var i = 0; i < chart.data[1].dataPoints.length; i++) {
                yValue += chart.data[1].dataPoints[i].y; //實際產量_累加


                if (yValue < chart.data[2].dataPoints[i].y) {
                    _markerType = "cross";
                    _markerColor = "tomato";

                } else {
                    _markerType = "triangle";
                    _markerColor = "#6B8E23";
                }

                if (i == (chart.data[1].dataPoints.length - 1)) {
                    dps.push({
                        label: chart.data[1].dataPoints[i].label,
                        y: yValue,
                        //indexLabel: "累積生產量:" + yValue,
                        markerType: _markerType,
                        markerColor: _markerColor
                    });

                }
                else {
                    dps.push({
                        label: chart.data[1].dataPoints[i].label,
                        y: yValue,
                        markerType: _markerType,
                        markerColor: _markerColor
                    });
                }
            }
            chart.addTo("data", {
                type: "line",
                name: "實際生產量",
                markerSize: 10,
                indexLabelFontSize: 20,
                showInLegend: true,
                dataPoints: dps
            });
            count_1 = yValue;
        }

    </script>
</asp:Content>
