<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Shipment.aspx.cs" Inherits="dek_erpvis_v2.page_webservice.Shipment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>出貨統計 | 德大機械</title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
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
                <div class="col-md-6 col-sm-12 col-xs-12">
                    <h4>[搜尋條件]出貨日期：<u><asp:LinkButton ID="LinkButton1" runat="server" data-toggle="modal" data-target="#exampleModal"><%=timerange %></asp:LinkButton></u></h4>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="dashboard_graph x_panel" id="Div_Shadow">
                    <div class="x_content">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div style="text-align: right; width: 100%; padding: 0;">
   <%--                                 <button type="button" id="exportChart" title="另存成圖片">
                                        <img src="../../assets/images/download.jpg" style="width: 36.39px; height: 36.39px;">
                                    </button>--%>
                                </div>
                                <div class="col-md-12 col-sm-12 col-xs-10">
                                    <div id="chartContainer" style="height: 500px; max-width: 100%; margin: 0px auto;"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--20190605，客戶出貨數量排行_1，建立長條圖區塊(ru)-->
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">

                <div class="dashboard_graph x_panel" id="Div_Shadow">
                    <div class="x_content">
                         <div style="text-align: right; width: 100%; padding: 0;">
        <%--                            <button type="button" id="exportimage" title="另存成圖片">
                                        <img src="../../assets/images/download.jpg" style="width: 36.39px; height: 36.39px;">
                                    </button>--%>
                                </div>
                        <div class="row">
                            <div class="col-md-9 col-sm-12 col-xs-12">
                               
                                <div class="col-md-12 col-sm-12 col-xs-10">

                                    <div id="chartContainer_cust" style="height: 500px; max-width: 100%; margin: 0px auto;"></div>
                                </div>
                            </div>
                            <!--2019/06/05，新增更換圖表類型_1-->
                            <div class="col-md-3 col-sm-12 col-xs-12">
                                <hr>
                                <div class="col-md-12 col-sm-6 col-xs-12">
                                    <div class="h4 mb-0 text-success" style="margin-bottom: 10px;" align="center">
                                        <button id="btnLineChart" type="button" class="btn btn-default fa fa-line-chart" style="height: 50px; width: 50%;" aria-hidden="true" onclick="ChangeChartType('line')">折線圖</button>
                                    </div>
                                </div>
                                <div class="col-md-12 col-sm-6 col-xs-12">
                                    <div class="h4 mb-0 text-success" style="margin-bottom: 10px;" align="center">
                                        <button id="btnBarChart" type="button" class="btn btn-default fa fa-bar-chart" style="height: 50px; width: 50%;" aria-hidden="true" onclick="ChangeChartType('column')">長條圖</button>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-13 col-sm-12 col-xs-12">
                <div class="x_panel" id="Div_Shadow">
                    <div class="x_title">
                        <h2>出貨統計列表<small></small></h2>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <p class="text-muted font-13 m-b-30">
                        </p>
                        <table id="datatable-buttons" class="table  table-ts table-bordered nowrap" cellspacing="0" width="100%">
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
                                <i class="fa fa-caret-down"><b>客戶排行名次</b></i> <i id=""></i>
                            </h5>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="input-group">
                                        <span class="input-group-addon"><b>前</b></span>
                                        <input type="text" class="form-control" value="20" id="demo_vertical2" name="demo_vertical2" runat="server" />
                                        <span class="input-group-addon"><b>名</b></span>
                                    </div>
                                </div>
                            </div>
                            <h5 class="modaltextstyle">
                                <i class="fa fa-caret-down"><%--<%=date_name %>--%><b>出貨日期快選</b></i> <i id="cbx_remind"></i>
                            </h5>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <asp:LinkButton ID="LinkButton_month" runat="server" CssClass="btn btn-default " OnClick="button_select_Click">當月</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton_firsthalf" runat="server" class="btn btn-default " OnClick="button_select_Click">上半年</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton_lasthalf" runat="server" class="btn btn-default " OnClick="button_select_Click">下半年</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton_year" runat="server" class="btn btn-default " OnClick="button_select_Click">全年</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <h5 class="modaltextstyle">
                                <i class="fa fa-caret-down"><%--<%=date_name %>--%><b>出貨日期 </b><small><b>(ex:yyyyMMdd)</b></small></i> <i id="cbx_remind"></i>
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
                    <!--20190605，日期快選_1，觸發 OnClick 事件(ru)-->
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
        //20190605，日期區間格式判斷
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
        //前N名的客戶
        CanvasJS.addColorSet("greenShades", [
            "#4656cc",
            "#3d1b41",
            "#c3cdf5",
            "#e43849",
            "#ea601b",
            "#991f42",
            "#f5b025",
            "#84ac52",
            "#5db0c3",
            "#1ea1d1",
            "#18478e",
            "#003c55"]);
        //20190605，客戶出貨數量排行_2，長條圖參數設定(ru)
        var chart_cust = new CanvasJS.Chart("chartContainer_cust", {
            //exportEnabled: true,
            colorSet: "greenShades",
            animationEnabled: true,
            theme: "light1",
            title: {
                text: <%=title_text_cust %>,
                fontFamily: "NotoSans",
                fontWeight: "bolder",
            },
            //dataPointWidth: 60,
            axisX: {
                interval: 1,
                intervalType: "year"
            },
            axisY: {
                lineThickness: 1,
                lineColor: "#d0d0d0",
                gridColor: "transparent",
            },
            legend: {
                fontSize: 15,
                cursor: "pointer",
                fontFamily: "NotoSans",
                /*verticalAlign: "center",
                horizontalAlign: "right"*/
            },
            toolTip: {
                shared: true,
                content: toolTipContent
            },
            data: [{
                type: "stackedColumn",

                //showInLegend: true,
                //color: "#79c0f8",
                indexLabelPlacement: "outside", indexLabelBackgroundColor: "white",
                name: "已出貨",
                dataPoints: [
                    <%= col_data_Points_cust %>
                ]
            }]
        });
        document.getElementById("exportimage").addEventListener("click", function () {
            chart_cust.exportChart({ format: "png" });
            parent.location.reload();
        });

        // 2019/06/05，更換圖表類型_2
        //chart_cust.options.data[0].type = "column";
        function ChangeChartType(Type) {
            for (var i = 0; i < chart_cust.options.data.length; i++) {
                chart_cust.options.data[i].type = Type;
            }
            chart_cust.render();
        }

        //各產線出貨量
        var chart = new CanvasJS.Chart("chartContainer", {
            //exportEnabled: true,
            colorSet: "greenShades",
            animationEnabled: true,
            theme: "light1",
            title: {
                text: <%=title_text %>,
                fontFamily: "NotoSans",
                fontWeight: "bolder",
            },
            //dataPointWidth: 60,
            axisX: {
                interval: 1,
                intervalType: "year"
            },
            axisY: {
                lineThickness: 1,
                lineColor: "#d0d0d0",
                gridColor: "transparent",
            },
            legend: {
                fontSize: 15,
                cursor: "pointer",
                fontFamily: "NotoSans",
                /*verticalAlign: "center",
                horizontalAlign: "right"*/
            },
            toolTip: {
                shared: true,
                content: toolTipContent
            },
            data: [{
                type: "stackedColumn",
                fontFamily: "NotoSans",
                //showInLegend: true,
                //color: "#79c0f8",
                indexLabelPlacement: "outside", indexLabelBackgroundColor: "white",
                name: "已出貨",
                dataPoints: [
                   <%= col_data_Points%>
                ]
            }]
        });
        chart.render();
        chart_cust.render();
        document.getElementById("exportChart").addEventListener("click", function () {
            chart.exportChart({ format: "png" });
            parent.location.reload();
        });

        window.onload = ChangeChartType("stackedColumn");
        function toolTipContent(e) {
            var str = "";
            var total = 0;
            var str2, str3;
            for (var i = 0; i < e.entries.length; i++) {
                var str1 = "<span style= 'color:" + e.entries[i].dataSeries.color + "'> " + e.entries[i].dataSeries.name + "</span>: <strong>" + e.entries[i].dataPoint.y + "</strong><br/>";
                total = e.entries[i].dataPoint.y + total;
                str = str.concat(str1);
            }
            str2 = "<span style = 'color:DodgerBlue;'><strong>" + (e.entries[0].dataPoint.label) + "</strong></span><br/>";
            str3 = "<span style = 'color:Tomato'>Total:</span><strong>" + total + "</strong><br/>";
            return (str2.concat(str)).concat(str3);
        }
    </script>
</asp:Content>

