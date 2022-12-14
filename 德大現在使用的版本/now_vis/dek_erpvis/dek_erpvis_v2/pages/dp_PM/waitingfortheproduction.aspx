<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="waitingfortheproduction.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PM.waitingfortheproduction_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>生產推移圖 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>

    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet"/>
    <link href="../../Content/dp_PM/waitingfortheproduction.css" rel="stylesheet" />

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
            </div>
        </div>
        <div class="clearfix"></div>
        <!-----------------content------------------>
        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true">圖片模式</a>
            </li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" id="profile-tab" role="tab" data-toggle="tab" aria-expanded="false">表格模式</a>
            </li>
             <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content3" id="month_capacity" role="tab" data-toggle="tab" aria-expanded="false">本月產能</a>
            </li>
        </ul>
        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">

                        <div class="col-md-9 col-sm-12 col-xs-12 padding-left-right-4">
                            <div class="col-md-12 col-sm-12 col-xs-12" id="hidepercent">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <div style="text-align: right; width: 100%; padding: 0;">
                                            <button style="display: none" type="button" id="exportChart" title="另存成圖片">
                                                <img src="../../assets/images/download.jpg" style="width: 36.39px; height: 36.39px;">
                                            </button>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 col-sm-12 col-xs-12">

                                                <div class="col-md-12 col-sm-12 col-xs-12">
                                                    <div id="chart_bar"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12 col-sm-12 col-xs-12" id="hidediv">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <div style="text-align: right; width: 100%; padding: 0;">
                                            <button style="display: none" type="button" id="exportimage" title="另存成圖片">
                                                <img src="../../assets/images/download.jpg" style="width: 36.39px; height: 36.39px;">
                                            </button>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 col-sm-12 col-xs-12">

                                                <div class="col-md-12 col-sm-12 col-xs-10">
                                                    <div id="chartContainer" style="height: 500px; max-width: 100%;"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                        </div>
                        <div class="col-md-3 col-sm-12 col-xs-12 padding-left-right-4">
                            <div class="x_panel">
                                <table id="TB" class="table table-ts table-bordered nowrap" cellspacing="0" width="100%">
                                    <thead style='display: none'>
                                        <tr id="tr_row">
                                            <th>狀態</th>
                                            <th>時間</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>本期計畫生產 ：</b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <span style="display: inline-block; width: 45%; text-align: right;"><%= 預定生產_data_y_max %></span>
                                                    <span style="display: inline-block; width: 50%; text-align: right;">(100%)</span>

                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>應有進度： </b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <span style="display: inline-block; width: 45%; text-align: right;"><%= 預計生產量_至今 %></span>
                                                    <span style="display: inline-block; width: 50%; text-align: right;">(<%=應有進度 %>)</span>

                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>實際進度： </b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <span style="display: inline-block; width: 45%; text-align: right;"><%= 實際生產_data_y_max %></span>
                                                    <span style="display: inline-block; width: 50%; text-align: right;">(<%=實際進度 %>)</span>

                                                </b>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>差異： </b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <span style="display: inline-block; width: 45%; text-align: right;"><%=差值 %></span>
                                                    <span style="display: inline-block; width: 50%; text-align: right;">(<%=Int32.Parse(實際進度.Split('%')[0])- Int32.Parse(應有進度.Split('%')[0]) %>%)</span>

                                                </b>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>本期尚未生產： </b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <span style="display: inline-block; width: 45%; text-align: right;"><%= 預定生產_data_y_max - 實際生產_data_y_max %></span>
                                                    <span style="display: inline-block; width: 50%; text-align: right;">(<%=本期未生產_persent%>)</span>

                                                </b>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>異常處理中： </b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <span style="display: inline-block; width: 45%; text-align: right;"><%=sovling %></span>
                                                    <span style="display: inline-block; width: 50%; text-align: right;"></span>
                                                </b>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>


                            <div class="x_panel">
                                <asp:ScriptManager ID="ScriptManager1" runat="server">
                                </asp:ScriptManager>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                    <ContentTemplate>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>廠區</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="dropdownlist_Factory_SelectedIndexChanged">
                                                    <asp:ListItem Value="sowon" Selected="True">立式廠</asp:ListItem>
                                                    <asp:ListItem Value="iTec">臥式廠</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>圖片選擇</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:DropDownList ID="dropdownlist_type" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%">
                                                    <asp:ListItem>生產推移圖</asp:ListItem>
                                                    <asp:ListItem>生產領料圖</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>




                                        <div class="col-md-12 col-sm-12 col-xs-12  col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>選擇產線</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:CheckBoxList ID="checkBoxList_LINE" runat="server" RepeatColumns="2" onclick="select_all()"></asp:CheckBoxList>
                                            </div>
                                        </div>




                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4" >
                                                <span>日期快選</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <div class="btn-group btn-group-justified">
                                                    <asp:LinkButton ID="LinkButton_month" runat="server" CssClass="btn btn-default " OnClick="button_select_Click" Style="text-align: center">當月</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>日期選擇</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:TextBox ID="txt_str" runat="server" Style="" TextMode="Date" CssClass="form-control   text-left"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:TextBox ID="txt_end" runat="server" CssClass="form-control  text-left" TextMode="Date"></asp:TextBox>
                                            </div>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                               <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="col-md-12 col-xs-12 text-align-end">
                                        <asp:Button ID="button_select" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="button_select_Click" Style="display: none" />
                                        <button id="btncheck" type="button" class="btn btn-primary antosubmit2">執行搜索</button>
                                    </div>
                                </div>
                            </div>
                        </div>    
                    </div>
                </div>
            </div>
            <div role="tabpanel" class="tab-pane fade" id="tab_content2" aria-labelledby="profile-tab">
                <div id="waitfor"></div>
            </div>
            <div role="tabpanel" class="tab-pane fade" id="tab_content3" aria-labelledby="profile-tab">
                <div id="capacity"></div>
            </div>
        </div>

        <!-----------------/content------------------>
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
    <script src="../../assets/vendors/datatables.net-colReorder/dataTables.colReorder.min.js"></script>
    <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
    <script src="../../assets/vendors/time/loading.js"></script>
    <!--返回上一個畫面時，要回到上一動-->
    <script src="../../assets/vendors/cookies/cookie_action.js"></script>
    <script src="../../assets/vendors/Create_HtmlCode/HtmlCode20211210.js?version = 1.1"></script>
    <script>
        //$("form input:checkbox").addClass('flat');//flat樣式使checkbox失效 2022/6/1
        function select_all() {
            $('#<%=checkBoxList_LINE.ClientID %> input').change(function ()  {
                var check = $(this).context.checked;
                var val = $(this).val();
                if (val == "" && check == true) {
                    seletedAllItem(true);
                } else if (val == "" && check == false) {
                    seletedAllItem(false);
                } else if (val != "" && check == false) {
                    if ($(this)[0].id.split("_")[3] != 0)
                    {
                        var x = document.getElementById("<%=checkBoxList_LINE.ClientID%>_0");
                        x.checked = false;
                    }
                }
            });
            function seletedAllItem(seleted) {
                $('#<%=checkBoxList_LINE.ClientID%> input').each(function () {
                    $(this).context.checked = seleted;
                });
            }
            function seletedItem(num, seleted) {
                var x = document.getElementById("<%=checkBoxList_LINE.ClientID%>_" + num);
                x.checked = true;
            }
        }



        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();
        });
        var chart = new CanvasJS.Chart("chartContainer", {
            // exportEnabled: true,
            animationEnabled: true,
            theme: "light1",
            title: {
                display: true,
                text: '生產領料圖',
                fontFamily: 'NotoSans',
                fontWeight: 'bolder',
                textAlign: "'center",
            }, subtitles: [{
                text: <%=timerange%>,
                fontFamily: 'NotoSans',
                fontWeight: 'bolder',
                textAlign: "'center",
                fontSize: 18,
            }],
            axisX: {
                title: '日期',
                interval: 1
            },
            axisY: {
                title: '數量',
                lineThickness: 1,
                lineColor: "#d0d0d0",
                gridColor: "transparent",
            }, axisY2: {
                title: "",
                tickLength: 1,
                lineThickness: 0,
                margin: 0,
                valueFormatString: " ",
            },
            toolTip: {
                shared: true,
                contentFormatter: function (e) {//debug
                    var str = "<span>" + e.entries[0].dataPoint.label + "</span><br/>";
                    temp = "";
                    for (var i = 0; i < e.entries.length; i++) {
                        if (e.entries[i].dataSeries.name != "") {
                            var temp = "<span style='color:" + e.entries[i].dataSeries.color + "'>" + e.entries[i].dataSeries.name + ": </span>" + "<strong>" + e.entries[i].dataPoint.y + "</strong> <br/>";
                            str = str.concat(temp);
                        }
                    }
                    return (str);
                }
            },
            legend: {
                fontSize: 15,
                cursor: "pointer",
                itemclick: toggleDataSeries,

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
                    // axisYType: "secondary",
                    name: "實生產",
                    showInLegend: true,
                    dataPoints: [
                        <%=實際生產_data%>
                    ]
                },

                {
                    type: "column",
                    name: "實領數",
                    showInLegend: true,
                    visible: false,
                    dataPoints: [
                    <%=實領料數_data%>
                    ]
                },


            ]
        });

        document.getElementById("exportimage").addEventListener("click", function () {
            chart.exportChart({ format: "png" });
            parent.location.reload();
        });
        // 2019/06/13，更換圖表類型
        function ChangeChartType(Type) {
            for (var i = 0; i < chart.options.data.length; i++) {
                chart.options.data[i].type = Type;
            }
            chart.render();
        }

        //2019/05/20，控制圖例開關
        function toggleDataSeries(e) {
            if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                e.dataSeries.visible = false;
            }
            else {
                e.dataSeries.visible = true;
            }
            chart.render();
        }
        //生產推移圖
        var chart_bar = new CanvasJS.Chart("chart_bar", {
            // exportEnabled: true,
            animationEnabled: true,
            theme: "light1",
            title: {
                display: true,
                text: '生產推移圖',
                fontFamily: 'NotoSans',
                fontWeight: 'bolder',
                textAlign: "'center",
            }, subtitles: [{
                text: <%=timerange%>,
                fontFamily: 'NotoSans',
                fontWeight: 'bolder',
                textAlign: "'center",
                fontSize: 18,
            }],
            axisX: {
                title: '日期',
                interval: 1
            },
            axisY: {
                title: '數量',
                lineThickness: 1,
                lineColor: "#d0d0d0",
                gridColor: "transparent",
            }, legend: {
                fontSize: 15,
                cursor: "pointer",
                itemclick: function (e) {
                    //console.log("legend click: " + e.dataPointIndex);
                    //console.log(e);
                    if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                        e.dataSeries.visible = false;
                    } else {
                        e.dataSeries.visible = true;
                    }

                    e.chart.render();
                }
            },
            toolTip: {
                shared: true,
                contentFormatter: function (e) {//debug
                    var str = "<span>" + e.entries[0].dataPoint.label + "</span><br/>";
                    temp = "";
                    //tooptip顯示
                    for (var i = 0; i < e.entries.length; i++) {
                        var temp = "<span style='color:" + e.entries[i].dataSeries.color + "'>" + e.entries[i].dataSeries.name + ": </span>"
                        temp += "<strong>" + e.entries[i].dataPoint.y + "</strong> <br/>";
                        str = str.concat(temp);
                        temp = e.entries[i].dataPoint.y;
                    }


                    //2019/06/05 10:33 pm....... john-------
                    var tar = 0, tar = 0, ans = 0, e_length = e.entries.length;
                    now = (e.entries[e.entries.length - 1].dataPoint.y);//決定第一條線要跟哪條線相減
                    tar = (e.entries[e.entries.length - e.entries.length].dataPoint.y);
                    if (e_length == 1) {//如果等於你選的那條線

                        var max = chart_bar.data[chart_bar.options.data.length - 1].dataPoints.length - 1;
                        var color = chart_bar.data[chart_bar.options.data.length - 1].color;
                        var name = chart_bar.data[chart_bar.options.data.length - 1].name;
                        now = chart_bar.data[chart_bar.options.data.length - 1].dataPoints[max].y;
                        str += "<strong style='color:" + color + ";'>" + name + " : </strong><strong>" + now + "</strong> <br/>";
                    }

                    ans = (now) - (tar);
                    //var ans = (e.entries[1].dataPoint.y) - (e.entries[0].dataPoint.y);
                    str += "<strong style='color:tomato;'>差值 : </strong><strong>" + ans + "</strong> <br/>";
                    return (str);
                }
            },

            data: []
        });
        document.getElementById("exportChart").addEventListener("click", function () {
            chart_bar.exportChart({ format: "png" });
            parent.location.reload();
        });
        function createPareto_1() {//debug
            var dps = [];
            var yValue = 0;
            for (var i = 0; i < chart.data[0].dataPoints.length; i++) {
                yValue += chart.data[0].dataPoints[i].y;

                if (i == (chart.data[0].dataPoints.length - 1)) {
                    dps.push({
                        lineColor: "blue",
                        label: chart.data[0].dataPoints[i].label,
                        y: yValue,
                        //indexLabel: ""//"生產目標:" + yValue
                    });

                } else {
                    dps.push({
                        lineColor: "blue",
                        label: chart.data[0].dataPoints[i].label,
                        y: yValue
                    });
                }
            }
            chart_bar.addTo("data", {
                type: "line",
                name: "累積生產目標",
                markerSize: 0,
                indexLabelFontSize: 20,
                showInLegend: true,
                dataPoints: dps
            });
            count_0 = yValue;
        }
        function createPareto_2() {//debug
            var dps = [];
            var yValue = 0;
            var _markerType = "";
            var _markerColor = "";

            for (var i = 0; i < chart.data[1].dataPoints.length; i++) {
                yValue += chart.data[1].dataPoints[i].y; //實際產量_累加



                if (yValue < chart_bar.data[0].dataPoints[i].y) {//應生產
                    _markerType = "cross";
                    _markerColor = "tomato";

                } else {
                    _markerType = "triangle";
                    _markerColor = "#6B8E23";
                }

                if (i == (chart.data[0].dataPoints.length - 1)) {
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
            chart_bar.addTo("data", {
                type: "line",
                name: "實際生產量",
                markerSize: 10,
                indexLabelFontSize: 20,
                showInLegend: true,
                dataPoints: dps
            });
            count_1 = yValue;
        }
        chart.render();
        var max = chart.axisY[0].get("maximum");
        chart.options.axisY2.maximum = max;
        chart.render();

        createPareto_1();
        createPareto_2();
        //--生產推移圖結束
        chart_bar.render();
        //防止跑版
        $(document).ready(function () {
            
            //若於表格模式點選第N頁之客戶，則上一頁後返回該客戶(EX：第3頁的A客戶，按下上一頁會在第3頁)
            //return_preaction('waitingfortheproduction=waitingfortheproduction_cust', '#datatable-buttons');

            //$('#example').DataTable({
            //    responsive: true
            //});
            //$('#exampleInTab').DataTable({
            //    responsive: true
            //});

            //$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            //    $($.fn.dataTable.tables(true)).DataTable()
            //        .columns.adjust()
            //        .responsive.recalc();
            //});
        });

        //隱藏推移圖 OR 領料圖
        <%=hide_image%>.style.display = 'none';

        //返回上一頁的時候在原來的TAB
        $(function () {
            var hash = window.location.hash;
            hash && $('ul.nav a[href="' + hash + '"]').tab('show');

            $('.nav-tabs a').click(function (e) {
                $(this).tab('show');
                var scrollmem = $('body').scrollTop() || $('html').scrollTop();
                window.location.hash = this.hash;
                $('html,body').scrollTop(scrollmem);
            });
        });

        create_tablecode('waitfor', '未結案列表', 'table-form', '<%=th.ToString() %>', '<%=tr.ToString() %>');
        //產生相對應的JScode
        set_Table('#table-form');
        //防止頁籤跑版
        loadpage('', '');

        create_tablecode('capacity', '本月生產進度', 'capacity-form', '<%=th_month_capacity.ToString() %>', '<%=tr_month_capacity.ToString() %>');
        //產生相對應的JScode
        set_Table_defaulf_sort('#capacity-form');


    </script>
</asp:Content>
