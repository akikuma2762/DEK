<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Analysis_alarm_count.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_CNC.Analysis_alarm_count" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>異常歷程統計 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" media="screen and (max-width:768px)" />
    <link href="../../Content/dp_CNC/Analysis_alarm_count.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" target="_blank">
    <style>
        ._select {
            display: none;
        }

        .button_use {
            z-index: 999;
            margin: 0;
            top: 35%;
            right: 0%;
            -ms-transform: translateY(-30%);
            transform: translateY(-30%);
            width: 33px;
            height: 240px;
            line-height: 235px;
            font-family: verdana;
            text-align: center;
            background: #009efa;
            color: #fff;
            position: fixed;
            bottom: 20px;
            right: 0px;
            text-decoration: none;
            cursor: pointer;
        }

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

        @media screen and (max-width:768px) {
            .button_use {
                display: none;
            }

            ._select {
                display: block;
            }            .exportChart{
                  display: none;
            }
        }
    </style>
    <!-- page content -->
    <div class="right_col" role="main">
        <ol class='breadcrumb_'>
            <li><u><a href='../index.aspx'>首頁 </a></u></li>
            <li><u><a href='../SYS_CONTROL/dp_fuclist.aspx?dp=CNC'>加工部</a></u></li>
        </ol>
        <br>

        <div class="clearfix"></div>

        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true" onclick="show_control()">圖片模式</a>
            </li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" id="profile-tab" role="tab" data-toggle="tab" aria-expanded="false" onclick="showbar()">表格模式</a>
            </li>
        </ul>
        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">

 <div class="col-md-12 col-sm-12 col-xs-12" id="table_information">
                            <div class="col-md-12 col-sm-12 col-xs-12" style="padding-right: 1px">
                                <div class="x_panel">
                                    <asp:TextBox ID="TextBox_MachTypeText" runat="server" Style="display: none"></asp:TextBox>
                                    <asp:TextBox ID="TextBox_MachTypeValue" runat="server" Style="display: none"></asp:TextBox>
                                    <asp:TextBox ID="TextBox_MachGroupText" runat="server" Style="display: none"></asp:TextBox>
                                    <asp:TextBox ID="TextBox_MachGroupValue" runat="server" Style="display: none"></asp:TextBox>
                                    <div class="x_content">
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div style="text-align: right; width: 100%; padding: 0;">
                                                <button  type="button" id="exportChart" title="另存成圖片">
                                                    <img src="../../assets/images/download.jpg" style="width: 36.39px; height: 36.39px;">
                                                </button>
                                            </div>
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div id="mach_list" style=""></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                           <div id="_select" class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="x_panel">
                                <div class="x_content">
                                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                                    </asp:ScriptManager>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                        <ContentTemplate>
                                            <div class="col-md-12 col-sm-6 col-xs-12">
                                                <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                    <span>廠區名稱</span>
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-8">
                                                    <asp:DropDownList ID="DropDownList_MachType" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" onchange="dropdownlist_change('ContentPlaceHolder1_DropDownList_MachType','ContentPlaceHolder1_DropDownList_MachGroup')"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-6 col-xs-12">
                                                <div class="col-md-4 col-sm-12 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                    <span>群組名稱</span>
                                                </div>
                                                <div class="col-md-8 col-sm-12 col-xs-8">
                                                    <asp:DropDownList ID="DropDownList_MachGroup" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" onchange="show_machines('ContentPlaceHolder1_DropDownList_MachGroup')"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div id="div_machines" class="col-md-12 col-sm-6 col-xs-12" style="display: none">
                                                <asp:TextBox ID="TextBox_Machines" runat="server" Style="display: none"></asp:TextBox>
                                                <div class="col-md-4 col-sm-12 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                    <span>群組機台</span>
                                                </div>
                                                <div class="col-md-8 col-sm-12 col-xs-8">
                                                    <label id="machines"></label>
                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-6 col-xs-12">
                                                <div class="col-md-4 col-sm-12 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                    <span>X座標(值)</span>
                                                </div>
                                                <div class="col-md-8 col-sm-12 col-xs-8">
                                                    <asp:DropDownList ID="DropDownList_X" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%">
                                                        <asp:ListItem Value="Error_Season"> 異常原因 </asp:ListItem>
                                                        <asp:ListItem Value="Machine_name"> 設備名稱 </asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-6 col-xs-12">
                                                <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                    <span>Y座標(值)</span>
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-8">
                                                    <asp:DropDownList ID="DropDownList_Y" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%">
                                                        <asp:ListItem Value="count"> 次數 </asp:ListItem>
                                                        <asp:ListItem Value="time"> 時間 </asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>


                                            <div class="col-md-12 col-sm-6 col-xs-12">
                                                <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                    <span>日期選擇</span>
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                    <asp:TextBox ID="textbox_st" runat="server" TextMode="Date" CssClass="form-control   text-center" Style="width: 100%"></asp:TextBox>

                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-6 col-xs-12">
                                                <div class="col-md-4 col-sm-3 col-xs-4"></div>
                                                <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                    <asp:TextBox ID="textbox_ed" runat="server" CssClass="form-control  text-center" TextMode="Date" Style="width: 100%"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-6 col-xs-12">
                                                <div class="col-md-4 col-sm-12 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                    <span>顯示筆數</span>
                                                </div>
                                                <div class="col-md-5 col-sm-12 col-xs-5" style="margin: 0px 0px 5px 0px">
                                                    <asp:TextBox ID="txt_showCount" runat="server" Text="10" CssClass="form-control text-center" TextMode="Number"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3 col-sm-12 col-xs-3" style="margin: 5px 0px 5px 0px">
                                                    <span>
                                                        <asp:CheckBox ID="CheckBox_All" runat="server" Text="全部" onclick="checkstatus()" />
                                                    </span>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                        <div class="col-md-9 col-xs-8">
                                        </div>
                                        <div class="col-md-3 col-xs-12">
                                            <button id="btncheck" type="button" class="btn btn-primary antosubmit2">執行搜索</button>
                                            <asp:LinkButton ID="LinkButton_ok" runat="server" OnClick="Select_MachGroupClick" class="btn btn-primary antosubmit2" Style="display: none">執行搜索</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
               <div id="div_button" class="button_use" onclick="hide_div()">
                <i id="iconimage" class="fa fa-chevron-left"></i>
            </div>
            <div role="tabpanel" class="tab-pane fade" id="tab_content2" aria-labelledby="profile-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12" id="alarm_detail_count" style="padding-right: 1px">
                            <div class="x_content">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h1 class="text-center _mdTitle" style="width: 100%"><b>異常歷程列表</b></h1>
                                        <h3 class="text-center _xsTitle" style="width: 100%"><b>異常歷程列表</b></h3>
                                        <div class="clearfix"></div>
                                    </div>
                                    <table id="Alarm_Datatable" class="table table-ts table-bordered nowrap" cellspacing="0" width="100%">
                                        <thead>
                                            <tr id="tr_row">
                                                <%=error_AlarmInfo_th %>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <%=error_count_tr.ToString() %>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                               
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-----以下檢索精靈相關--->
    <!-- set Modal -->


    <!--/set Modal-->
    <!-----以上檢索精靈相關----->
    <!-- /page content -->
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
    <!-- Parsley -->
    <script src="../../assets/vendors/parsleyjs/dist/parsley.min.js"></script>
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
    <!-- Resources chart break -->
    <script src="https://www.amcharts.com/lib/4/core.js"></script>
    <script src="https://www.amcharts.com/lib/4/charts.js"></script>
    <script src="https://www.amcharts.com/lib/4/themes/animated.js"></script>
    <script src="../../assets/vendors/time/loading.js"></script>

    <script src="../../assets/vendors/Create_HtmlCode/HtmlCode.js"></script>
    <script>

        function showbar() {

            document.getElementById("div_button").style.display = 'none';
        }

        function show_control() {
            var WhatSystem = navigator.userAgent;
            if (WhatSystem.match(/android/i) || WhatSystem.match(/(iphone|ipad|ipod);?/i)) {

            } else
                document.getElementById("div_button").style.display = 'block';
        }
        //===========================================表格===========================================
        //設定datatable
        function hide_div() {
            var tablename = document.getElementById("table_information");
            var divname = document.getElementById("_select");
            var icon = document.getElementById("iconimage");


            if (tablename.className == 'col-md-9 col-sm-12 col-xs-12') {
                icon.className = 'fa fa-chevron-left';
                tablename.className = 'col-md-12 col-sm-12 col-xs-12';
                divname.style.display = 'none';

            }
            else {
                icon.className = "fa fa-chevron-right";
                tablename.className = 'col-md-9 col-sm-12 col-xs-12';
                divname.style.display = 'block';
            }
            var _ch = new CanvasJS.Chart("mach_list",
                {
                    colorSet: "greenShades",
                    //exportEnabled: true,
                    animationEnabled: true,
                    title: {
                        text: '異常歷程統計',
                        fontFamily: 'NotoSans',
                        fontWeight: 'bolder',
                        textAlign: "'center",
                        //fontFamily: "arial",
                    },
                    subtitles: [{
                        text: '<%=timerange %>',
                    fontFamily: 'NotoSans',
                    fontWeight: 'bolder',
                    textAlign: "'center",
                }],
                axisX: {
                    title: '<%=DropDownList_X.SelectedItem.Text%>',

                    labelAngle: -180,
                    intervalType: "year"
                },
                axisY: {
                    title: '<%=unit%>',

                    lineThickness: 1,
                    lineColor: "#d0d0d0",
                    gridColor: "transparent",
                },
                legend: { cursor: "pointer" },
                data: [{
                    //20191115開會討論由圓餅圖改直方圖
                    type: "column", toolTipContent: "{label}: <strong>{y}<%=error_unit%></strong>", indexLabel: "{y}", indexLabelBackgroundColor: "white",
                    indexLabelPlacement: "outside",
                    dataPoints: [
                    <%=
                    Chart_Count
                    %>
                       ]
                   }
                   ]
               }); _ch.render()
        }
        $(".iconimage").on("click", function () {
            hide_div();
        });

        $('#Alarm_Datatable').dataTable(
            {
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
        //===========================================折線===========================================

        $("#btncheck").click(function () {

            var type = document.getElementById('ContentPlaceHolder1_DropDownList_MachType');
            var group = document.getElementById('ContentPlaceHolder1_DropDownList_MachGroup');
            var grouptext = '';

            $('#ContentPlaceHolder1_TextBox_MachTypeText').val('' + type.options[type.selectedIndex].text + '');
            $('#ContentPlaceHolder1_TextBox_MachTypeValue').val('' + type.value + '');
            try {
                $('#ContentPlaceHolder1_TextBox_MachGroupText').val('' + group.options[group.selectedIndex].text + '');
                $('#ContentPlaceHolder1_TextBox_MachGroupValue').val('' + group.value + '');
                grouptext = group.options[group.selectedIndex].text;
            }
            catch {

            }
            if (grouptext != '' && grouptext != '--Select--') {
                $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                document.getElementById('<%=LinkButton_ok.ClientID %>').click();
            }
            else
                alert('請選擇群組');
        });
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
        var _ch = new CanvasJS.Chart("mach_list",
            {
                colorSet: "greenShades",
                //exportEnabled: true,
                animationEnabled: true,
                title: {
                    text: '異常歷程統計',
                    fontFamily: 'NotoSans',
                    fontWeight: 'bolder',
                    textAlign: "'center",
                    //fontFamily: "arial",
                },
                subtitles: [{
                    text: '<%=timerange %>',
                    fontFamily: 'NotoSans',
                    fontWeight: 'bolder',
                    textAlign: "'center",
                }],
                axisX: {
                    title: '<%=DropDownList_X.SelectedItem.Text%>',

                    labelAngle: -180,
                    intervalType: "year"
                },
                axisY: {
                    title: '<%=unit%>',

                    lineThickness: 1,
                    lineColor: "#d0d0d0",
                    gridColor: "transparent",
                },
                legend: { cursor: "pointer" },
                data: [{
                    //20191115開會討論由圓餅圖改直方圖
                    type: "column", toolTipContent: "{label}: <strong>{y}<%=error_unit%></strong>", indexLabel: "{y}", indexLabelBackgroundColor: "white",
                    indexLabelPlacement: "outside",
                    dataPoints: [
                    <%=
                    Chart_Count
                    %>
                    ]
                }
                ]
            }); _ch.render()
        document.getElementById("exportChart").addEventListener("click", function () {
            _ch.exportChart({ format: "png" });
            parent.location.reload();
        });

        //===========================================時間搜尋===========================================     
        $(function () {
            $('#ContentPlaceHolder1_txt_time_str,#ContentPlaceHolder1_txt_time_end').daterangepicker({
                timePicker: true,
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
                $(this).val(picker.startDate.format('YYYYMMDDhhmm,A'));//時間格式
            });
            $('#ContentPlaceHolder1_txt_time_str,#ContentPlaceHolder1_txt_time_end').on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
            });
        });
        $(document).ready(function () {

            jQuery.fn.dataTable.Api.register('page.jumpToData()', function (data, column) {
                var pos = this.column(column, { order: 'current' }).data().indexOf(data);

                if (pos >= 0) {
                    var page = Math.floor(pos / this.page.info().length);
                    this.page(page).draw(false);
                }

                return this;
            });

            var table = $('#example').DataTable();
            table.page(5).draw(false);

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

        });


        //依據X軸內容改變Y軸內容
        function dropdownlist_change(x, y) {
            $('#' + y).empty();


            var ddlist = document.getElementById(x);
            //取得當前X的值
            var now_value = ddlist.value;
            var nowlist = now_value.split(',');
            var ddl = document.getElementById(y);

            for (i = 0; i < nowlist.length - 1; i++) {
                var option = document.createElement("OPTION");
                option.innerHTML = nowlist[i];
                option.value = nowlist[i + 1];
                ddl.options.add(option);
                i++;
            }
            document.getElementById('div_machines').style.display = 'none';
        }

        //依據X軸內容改變Y軸內容
        function show_machines(y) {
            var machine_name = '';
            var ddlist = document.getElementById(y);
            //取得當前X的值
            var now_value = ddlist.value;
            var nowlist = now_value.split('^');
            var label = document.getElementById('machines');
            label.innerHTML = '';
            for (i = 1; i < nowlist.length - 1; i++) {
                if (nowlist[i] != '') {
                    label.innerHTML += nowlist[i] + '<br />';
                    machine_name += nowlist[i] + ',';
                }

            }
            $('#ContentPlaceHolder1_TextBox_Machines').val('' + machine_name + '');
            if (nowlist.length - 1 > 1)
                document.getElementById('div_machines').style.display = 'block';
        }

        window.onload = set_content();

        function set_content() {
            var machine_name = '';
            var mach = document.getElementById("ContentPlaceHolder1_TextBox_Machines").value;
            var label = document.getElementById('machines');
            label.innerHTML = '';
            if (mach != '') {
                var machines = mach.split(',');
                for (i = 0; i < machines.length - 1; i++) {
                    if (machines[i] != '') {
                        label.innerHTML += machines[i] + '<br />';
                        machine_name += machines[i] + ',';
                    }
                }
                $('#ContentPlaceHolder1_TextBox_Machines').val('' + machine_name + '');
                document.getElementById('div_machines').style.display = 'block';
            }
        }
    //============================================================================================
    </script>
</asp:Content>
