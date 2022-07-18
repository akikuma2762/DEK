<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Machine_list_info_details.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_CNC.Machine_list_info_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=machine %> 問題回報表 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/UntradedCustomer.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        div.scrollbar {
            width: 100%;
            height: 85px;
            white-space: nowrap;
            overflow-x: scroll;
            overflow-y: hidden;
        }

        .dataTables_scroll {
            overflow: auto;
        }
    </style>
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class='breadcrumb_'>
            <li><u><a href='../index.aspx'>首頁 </a></u></li>
            <li><u><a href='../SYS_CONTROL/dp_fuclist.aspx?dp=CNC'>加工部</a></u></li>
            <li><u><a href="javascript:void()" onclick="history.go(-1)">設備監控面板</a></u></li>
        </ol>
        <br />
        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div class="col-md-9 col-sm-12 col-xs-12">
                            <div class="x_panel">
                                <div class="x_title">
                                    <h1 class="text-center _mdTitle" style="width: 100%"><b><%=machine %> 問題回報表</b></h1>
                                    <h3 class="text-center _xsTitle" style="width: 100%"><b><%=machine %> 問題回報表</b></h3>
                                    <div class="clearfix"></div>
                                </div>
                                <%=area %>
                                <hr />
                                <table id="Status_Datatable" class="table table-ts table-bordered nowrap" cellspacing="0" width="100%">
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
                        <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <div id="chartpie" style="height: 350px; width: 100%; margin: 0px auto;"></div>
                                    </div>
                                </div>
                            </div>
                            <%=total_worktime %>
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">

                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div class="col-md-5 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                <span>日期選擇</span>
                                            </div>
                                            <div class="col-md-7 col-sm-9 col-xs-8" style="margin: 5px 0px 5px 0px">
                                                <div class="row">
                                                    <asp:TextBox ID="TextBox_date" runat="server" TextMode="Date" Width="100%"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div class="col-md-5 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                <span>持續時間</span>
                                            </div>
                                            <div class="col-md-6 col-sm-8 col-xs-7" style="margin: 5px 0px 5px 0px">
                                                <div class="row">
                                                    <asp:TextBox ID="TextBox_time" runat="server" TextMode="Number" Text="0" Width="90%"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1 col-sm-1 col-xs-1" style="margin: 5px 0px 5px 0px">
                                                <div class="row">
                                                    (分)
                                                </div>
                                            </div>
                                        </div>
                                        <div></div>
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div class="col-md-5 col-sm-3 col-xs-4">
                                                <span>設備狀態</span>
                                            </div>
                                            <div class="col-md-7 col-sm-9 col-xs-8">
                                                <div class="row">
                                                    <asp:CheckBoxList ID="CheckBoxList_status" RepeatColumns="2" runat="server">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </div>
                                        <div></div>
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div class="col-md-9 col-xs-8">
                                            </div>
                                            <div class="col-md-3 col-xs-12">
                                                <asp:Button ID="button_select" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="button_select_Click" Style="display: none" />
                                                <button id="btncheck" type="button" class="btn btn-primary antosubmit2">執行搜索</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-----------------/content------------------>
    <!-- set Modal -->
    <div id="exampleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title modaltextstyle" id="myModalLabel2"><i class="fa fa-file-text"></i>修改資料</h4>
                </div>
                <asp:TextBox ID="TextBox_ID" runat="server" Style="display: none"></asp:TextBox>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                    <ContentTemplate>

                        <div class="modal-body">
                            <div id="testmodal2" style="padding: 5px 20px;">
                                <asp:Button ID="Button_Search" runat="server" Text="Button" OnClick="Button_Search_Click" Style="display: none" />
                                <div class="form-group">
                                    <div class="row">
                                        <asp:TextBox ID="TextBox_RecordTime" runat="server" Style="display: none"></asp:TextBox>
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <div class="btn-group btn-group-justified">
                                                <b>問題類型 </b>
                                                <asp:DropDownList ID="DropDownList_Status" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <div class="btn-group btn-group-justified">
                                                <b>內容填寫 </b>
                                                <asp:TextBox ID="TextBox_content" runat="server" TextMode="MultiLine" Style="resize: none; width: 100%"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="modal-footer">
                    <button type="button" class="btn btn-default antoclose2" data-dismiss="modal">退出</button>
                    <asp:Button ID="Button_submit" runat="server" Text="Button" OnClick="Button_submit_Click" Style="display: none" />
                    <button id="btnsubmit" type="button" class="btn btn-primary antosubmit2">儲存</button>

                </div>
            </div>
        </div>
    </div>
    <!--/set Modal-->
    <!-- Modal -->

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
    <script src="../../assets/vendors/datatables.net-colReorder/dataTables.colReorder.min.js"></script>
    <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
    <script src="../../assets/vendors/time/loading.js"></script>
    <script>

        //設定datatable
        $('#Status_Datatable').DataTable({
            destroy: true,
            language: {
                "processing": "處理中...",
                "loadingRecords": "載入中...",
                "lengthMenu": "顯示 _MENU_ 項結果",
                "zeroRecords": "沒有符合的結果",
                "info": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
                "infoEmpty": "顯示第 0 至 0 項結果，共 0 項",
                "infoFiltered": "(從 _MAX_ 項結果中過濾)",
                "infoPostFix": "",
                "search": "搜尋:",
                "paginate": {
                    "first": "第一頁",
                    "previous": "上一頁",
                    "next": "下一頁",
                    "last": "最後一頁"

                }
            },


            scrollCollapse: true,
            "order": [[<%=dt_count %>, "asc"]],
            dom: "<'row'<'pull-left'f>'row'<'col-sm-3'>'row'<'col-sm-3'B>'row'<'pull-right'l>>" +
                "<rt>" +
                "<'row'<'pull-left'i>'row'<'col-sm-4'>row'<'col-sm-3'>'row'<'pull-right'p>>",

            buttons: [
                'copy', 'csv', 'excel', 'pdf', 'print'
            ],

        });

        jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');


        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();
        });
        //防止切換頁籤時跑版
        $(document).ready(function () {


            $('#example').DataTable({
                responsive: true
            });
            $('#exampleInTab').DataTable({
                responsive: true
            });
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                $($.fn.dataTable.tables(true)).DataTable()
                    .columns.adjust()
                    .responsive.recalc();
            });
            jQuery.fn.dataTable.Api.register('page.jumpToData()', function (data, column) {
                var pos = this.column(column, { order: 'current' }).data().indexOf(data);

                if (pos >= 0) {
                    var page = Math.floor(pos / this.page.info().length);
                    this.page(page).draw(false);
                }

                return this;
            });
            var time = document.getElementById("ContentPlaceHolder1_TextBox_RecordTime").value;
            if (time != '') {
                var table = $('#Status_Datatable').DataTable();
                table.page.jumpToData(time.replaceAll('*', ' '), 1)
            }
            $('#ContentPlaceHolder1_TextBox_RecordTime').val('' + '' + '');


        });
        function set_id(ID, type, detail, starttime) {
            if (detail == '') {
                $("select#ContentPlaceHolder1_DropDownList_Status").prop('selectedIndex', 0);
                $('#ContentPlaceHolder1_TextBox_content').val('' + '');
            }
            else {
                document.getElementById('ContentPlaceHolder1_DropDownList_Status').value = type;
                $('#ContentPlaceHolder1_TextBox_content').val('' + detail.replaceAll("^", "'").replaceAll('#', '"').replaceAll("$", " ").replaceAll('@', '\r\n') + '');
            }

            $('#ContentPlaceHolder1_TextBox_ID').val('' + ID + '');
            $('#ContentPlaceHolder1_TextBox_RecordTime').val('' + starttime + '');
            //      document.getElementById('<%=Button_Search.ClientID %>').click();
        }
        $("#btnsubmit").click(function () {
            var WhatSystem = navigator.userAgent;
            if (WhatSystem.match(/(iphone|ipad|ipod);?/i)) {
            } else {
                $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                document.querySelector(".blockUI.blockMsg.blockPage").style.zIndex = 1000000;
                document.getElementById('btnsubmit').disabled = true;
            }


            document.getElementById('<%=Button_submit.ClientID %>').click();
        });

        //圓餅圖
        var chartpie = new CanvasJS.Chart("chartpie",
            {
                //exportEnabled: true,
                animationEnabled: true,
                title: {
                    text: '狀態比例統計',
                    fontFamily: "NotoSans",
                    fontWeight: "bolder",
                    fontSize: 20,
                },
                subtitles: [{
                    fontFamily: 'NotoSans',
                    fontWeight: 'bolder',
                    textAlign: "'center",
                }],
                legend: {
                    fontSize: 15,
                    cursor: "pointer",
                },
                data: [{
                    showInLegend: true,
                    type: "pie",
                    startAngle: 240,
                    yValueFormatString: "##0'%'",
                    indexLabel: "{label} {y}",
                    indexLabelFontSize: 15,
                    dataPoints: [
                    <%=pie_data_points%>
                    ]
                }]
            });
        chartpie.render();
        //status_bar
        var Color_OPERATE = "rgb(127, 255, 0)";
        var Color_ALARM = "rgb(255, 0, 0)";
        var Color_STOP = "rgb(255, 230, 0)";
        var Color_DISCONNECT = "rgb(189, 189, 189)";
        var Color_EMERGENCY = "rgb(255, 0, 0)";
        var Color_SUSPEND = "rgb(128, 128, 0)";
        var Color_MANUAL = "rgb(0, 239, 239)";
        var Color_WARMUP = "rgb(137, 15, 39)";

        var Color_OPERATE_new = "rgb(0, 180, 0)";
        var Color_DISCONNECT_new = "rgb(169, 169, 169)";
        var Color_ALARM_new = "rgb(255, 0, 0)";
        var Color_EMERGENCY_new = "rgb(255, 0, 255)";
        var Color_SUSPEND_new = "rgb(255, 255, 0)";
        var Color_STOP_new = "rgb(255, 155, 50)";
        var Color_MANUAL_new = "rgb(2, 205, 252)";
        var Color_WARMUP_new = "rgb(178, 34, 34)";

        var Color_Bg = "rgb(255, 255, 255)";
        var Color_Font = "rgb(0,0,0)";
        var Color_Scale = "rgb(127, 127, 127)";

        var StrFirstDay = '<%=str_First_Day%> 00:00:00';
        var StrLastDay = '<%=str_First_Day%> 11:59:59';

        var StrTodayStart = '<%=str_First_Day%> 12:00:00';
        var StrTodayEnd = '<%=str_First_Day%> 23:59:59';


        var dev_list = '<%=str_Dev_Name%>';
        var StrMachID;

        for (var i = 0; i < dev_list.split(',').length; i++) {
            StrMachID = '' + dev_list.split(',')[i] + '';
            draw_Axial(StrMachID, '');
            draw_Axial(StrMachID, '1');
        }

        var sqlcondition = '<%=condition%>'
        var Min_hh = 12;
        var Max_hh = 24;

        var Time_rang = 1;//時間距離

        var mTimer_status;
        var Timer_Count_status = 0;//閃爍機制
        //  var loadTime_status = 1000;//20000//60000
        mTimer_status = setTimeout(function () { draw_Axial(StrMachID, ''); }, 1000);//20190722++status bar即時更新
        mTimer_status2 = setTimeout(function () { draw_Axial(StrMachID, '1'); }, 2000);//20190722++status bar即時更新




        function draw_AxialData(StrMachID, StrFirstDay, StrLastDay, style, SQLCondition) {
            $.ajax({
                type: 'POST',
                url: "../../webservice/dpCNC_MachData.asmx/GetMachStatus_bar",
                data: { Mach_ID: StrMachID, First_Day: StrFirstDay, Last_Day: StrLastDay, condition: SQLCondition },
                dataType: 'xml',
                success: function (docxml) {
                    if ($(docxml).find("ROOT").length > 0) {
                        //清空畫布內容
                        var canvastx = document.getElementById('ma_canvas_' + StrMachID + style).getContext("2d");
                        canvastx.clearRect(0, 0, 100000, 100000);

                        var root = $(docxml);
                        root.find('Group').each(function () {
                            var element = $(this);
                            var start_time = element.attr('Start_time');
                            var cycle_time = element.attr('Cycle_time');
                            var nc_status = element.attr('Nc_Status');
                            var Start_time_line = element.attr('Start_time_line');
                            var End_time_line = element.attr('End_time_line');

                            draw_AxialWork(parseFloat(start_time), parseFloat(cycle_time), nc_status, StrMachID, Start_time_line, End_time_line, style);
                        });
                        draw_AxialTime(StrMachID, style);
                    }
                },
                error: function (xhr) { var msg = xhr.responseText; }
            });
        }
        function draw_Axial(StrMachID, type) {
            var clientHeight = '70';//避免高度一直累加40
            var clientWidth = document.getElementById('ma_div_' + StrMachID + type).clientWidth;  //
            var canvas = document.getElementById('ma_canvas_' + StrMachID + type);
            if (canvas.getContext) {
                var ctx = canvas.getContext("2d");
                ctx.canvas.width = clientWidth;
                ctx.canvas.height = clientHeight;
                ctx.fillStyle = Color_Bg;
                ctx.fillRect(0, 0, clientWidth, clientHeight);
                if (type == '')
                    draw_AxialData(StrMachID, StrFirstDay, StrLastDay, type, sqlcondition);
                else
                    draw_AxialData(StrMachID, StrTodayStart, StrTodayEnd, type, sqlcondition);
            }
        }


        function draw_AxialWork(X_Line_Start, X_Line_End, nc_status, StrMachID, Start_time_line, End_time_line, type) {
            var clientHeight = document.getElementById('ma_div_' + StrMachID + type).clientHeight;
            var clientWidth = document.getElementById('ma_div_' + StrMachID + type).clientWidth;
            var canvas = document.getElementById('ma_canvas_' + StrMachID + type);
            if (canvas.getContext) {


                var width_y = 0;
                //畫線的顏色
                var ctx = canvas.getContext("2d");
                var Width_Size = (clientWidth) / (Min_hh * 60);
                var X_Start = Width_Size * X_Line_Start;
                var X_End = Width_Size * X_Line_End;
                if ($(window).width() < 768) {
                    ctx.font = '12pt Calibri';
                } else {
                    ctx.font = '12pt Calibri';
                }
                ctx.fillStyle = Color_Font;//寫字
                switch (nc_status) {
                    case "OPERATE":
                        width_y = 20;
                        ctx.fillStyle = Color_OPERATE_new;
                        break;
                    case "MANUAL":
                        width_y = 20;
                        ctx.fillStyle = Color_MANUAL_new;
                        break;
                    case "WARMUP":
                        width_y = 20;
                        ctx.fillStyle = Color_WARMUP_new;//Color_WARMUP
                        break;
                    case "ALARM":
                        width_y = 50;
                        ctx.fillStyle = Color_ALARM_new;
                        break;
                    case "EMERGENCY":
                        width_y = 50;
                        ctx.fillStyle = Color_EMERGENCY_new;//Color_EMERGENCY
                        break;
                    case "STOP":
                        width_y = 40;
                        ctx.fillStyle = Color_STOP_new;
                        break;
                    case "SUSPEND":
                        width_y = 40;
                        ctx.fillStyle = Color_SUSPEND_new;
                        break;
                    case "DISCONNECT":
                        width_y = 20;
                        ctx.fillStyle = Color_DISCONNECT;
                        break;
                    default:
                        width_y = 10;
                        ctx.fillStyle = Color_Scale;
                }
                ctx.fillRect(X_Start, 50, X_End, -(width_y));//畫顏色
                //ctx.fillRect(X_Start, 10, X_End, 40);//畫顏色




            }
        }
        //畫時間刻度
        function draw_AxialTime(StrMachID, type) {
            var clientHeight = document.getElementById('ma_div_' + StrMachID + type).clientHeight;
            var clientWidth = document.getElementById('ma_div_' + StrMachID + type).clientWidth;
            var canvas = document.getElementById('ma_canvas_' + StrMachID + type);
            if (canvas.getContext) {
                var ctx = canvas.getContext("2d");
                var t = 0;
                if ($(window).width() < 768) {
                    ctx.font = '12pt Calibri';
                } else {
                    ctx.font = '12pt Calibri';
                }

                if (type == '') {
                    t = (clientWidth) / Min_hh;
                    ctx.fillStyle = Color_Font;
                    ctx.fillText("0h", 0, 14);
                    var j = 0;
                    for (i = 1; i < Min_hh + 1; i++) {
                        if (j <= Min_hh) {
                            j += Time_rang;
                            var offset = 10;
                            if (j == Min_hh) offset = 20
                            var x = parseInt(j * t, 10) - offset;
                            ctx.fillText(j, x, 14);
                        }
                        var x = parseInt(i * t, 10);
                        ctx.beginPath();
                        ctx.moveTo(x, 22);//灰色線條"||"
                        ctx.lineTo(x, 50);
                        ctx.stroke();
                    }
                }
                else if (type == '1') {

                    t = (clientWidth) / Min_hh;
                    ctx.fillStyle = Color_Font;
                    ctx.fillText("12h", 0, 14);
                    var j = 0;
                    for (i = 1; i < Min_hh + 1; i++) {

                        if (j <= Min_hh) {
                            j += Time_rang;
                            var offset = 10;
                            if (j == Min_hh) offset = 20
                            var x = parseInt(j * t, 10) - offset;
                            ctx.fillText((j + 12), x, 14);
                        }
                        var x = parseInt(i * t, 10);
                        ctx.beginPath();
                        ctx.moveTo(x, 22);//灰色線條"||"
                        ctx.lineTo(x, 50);
                        ctx.stroke();
                    }
                }



                ctx.beginPath();
                ctx.moveTo(0, 50);//灰色線條"一一"
                ctx.lineTo((clientWidth), 50);
                ctx.stroke();
            }
        }





<%--        window.onload = show_bar();
        function show_bar() {
            <%=js_code %>
        }--%>




    </script>
</asp:Content>

