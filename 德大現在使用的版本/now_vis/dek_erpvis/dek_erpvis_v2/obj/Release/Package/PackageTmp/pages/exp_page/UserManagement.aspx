<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="dek_erpvis_v2.pages.exp_page.UserManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>使用者帳號管理 | 德大機械</title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <link href="../../assets/build/css/custom.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <!-----------------title------------------>
        
        <div class="page-title">
            <div class="title_left">
                <h3>使用者帳號管理</h3>
            </div>
        </div>
        <div class="clearfix"></div>
        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2><%=user_area_text %><small></small></h2>                        
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <p class="text-muted font-13 m-b-30">
                        </p>
                            <table id="datatable-buttons" class="table table-striped table-bordered nowrap" cellspacing="0" width="100%">
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
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12 col-xs-12">
                                    <div class="form-group">
                                        <h5 class="modaltextstyle">
                                            <i class="fa fa-caret-down">使用者名稱</i> <i id=""></i>
                                        </h5>
                                        <div class="row">
                                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                                <input id="demo_vertical2" runat="server" type="text" value="20" name="demo_vertical2" data-bts-button-down-class="btn btn-secondary" data-bts-button-up-class="btn btn-secondary">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <h5 class="modaltextstyle">
                                <i class="fa fa-caret-down">區間快選</i> <i id="cbx_remind"></i>
                            </h5>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
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
    <!-- bootstrap-touchspin-master -->
    <script src="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.js"></script>
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

        $("input[name='ctl00$ContentPlaceHolder1$demo_vertical2']").TouchSpin({
            verticalbuttons: true,
            prefix: '#客戶排行前',
            min: 0,
            max: 100,
            step: 1,
            //decimals: 1,
            boostat: 5,
            maxboostedstep: 10,
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
        var chart = new CanvasJS.Chart("chartContainer", {
            animationEnabled: true,
            theme: "light2",
            title: {
                text: <%=title_text%>,
            },
            //dataPointWidth: 40,
            axisX: {
                interval: 1,
                intervalType: "year"
            },
            axisY: [{
                lineThickness: 1,
                lineColor: "#d0d0d0",
                gridColor: "transparent",
            }, {
                title: "",
                lineColor: "#369EAD",
                tickColor: "#369EAD",
                labelFontColor: "#369EAD",
                titleFontColor: "#369EAD",
                suffix: ""

            }],
            legend: {
                fontSize: 15,
                cursor: "pointer",
                /*verticalAlign: "center",
                horizontalAlign: "right"*/
            },
            toolTip: {
                shared: true,
                content: toolTipContent
            },
            data: [{
                type: "column",
                indexLabelPlacement: "outside",
                //showInLegend: true,
                name: <%= data_name%>,
                dataPoints: [
                    <%=col_data_Points%>
                ]
            }]
        });
        chart.render();
        function toolTipContent(e) {
            var str = "";
            var total = 0;
            var str2;
            for (var i = 0; i < e.entries.length; i++) {
                var str1 = "<span style= 'color:" + e.entries[i].dataSeries.color + "'> " + e.entries[i].dataSeries.name + "</span>: <strong>" + e.entries[i].dataPoint.y + "</strong><br/>";
                total = e.entries[i].dataPoint.y + total;
                str = str.concat(str1);
            }
            str2 = "<span style = 'color:DodgerBlue;'><strong>" + (e.entries[0].dataPoint.label) + "</strong></span><br/>";
            str3 = "<span style = 'color:Tomato'>Total:</span><strong>" + total + "</strong><br/>";
            return (str2.concat(str)).concat(/*str3*/);
        }
    </script>
</asp:Content>
