<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Production_History.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_CNC.Production_History_new" %>

<%--<%@ OutputCache duration="10" varybyparam="None" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>生產履歷 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/shipment.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        ._select {
            display: none;
        }

        .button_use {
            z-index: 999;
            margin: 0;
            top: 40%;
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
            }

            .exportChart {
                display: none;
            }
        }
    </style>
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class='breadcrumb_'>
            <li><u><a href='../index.aspx'>首頁 </a></u></li>
            <li><u><a href='../SYS_CONTROL/dp_fuclist.aspx?dp=CNC'>加工部</a></u></li>
        </ol>
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
            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true" onclick="show_control()">圖片模式</a>
            </li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" id="profile-tab" role="tab" data-toggle="tab" aria-expanded="false" onclick="showbar()">表格模式</a>
            </li>
        </ul>
        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade  active in   " id="tab_content1" aria-labelledby="home-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">
                        <asp:TextBox ID="TextBox_MachTypeText" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MachTypeValue" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MachGroupText" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="TextBox_MachGroupValue" runat="server" Style="display: none"></asp:TextBox>
                        <div class="col-md-12 col-sm-12 col-xs-12" id="table_information">
                            <div class="col-md-12 col-sm-12 col-xs-12" style="padding-right: 1px">
                                <div class="x_panel">

                                    <div class="x_content">
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div style="text-align: right; width: 100%; padding: 0;">
                                                <button type="button" id="exportChart" title="另存成圖片">
                                                    <img src="../../assets/images/download.jpg" style="width: 36.39px; height: 36.39px;">
                                                </button>
                                            </div>
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div id="chartContainer" style=""></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>



                        <div id="_select" class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <i id="cbx_remind"></i>

                                        <asp:TextBox ID="TextBox_Status" runat="server" Style="display: none" CssClass="form-control   text-left"></asp:TextBox>
                                        <div class="col-md-12 col-sm-6 col-xs-12">
                                            <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                <span>廠區名稱</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:DropDownList ID="DropDownList_factory" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" onchange="dropdownlist_change('ContentPlaceHolder1_DropDownList_factory','ContentPlaceHolder1_DropDownList_Group')"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-6 col-xs-12">
                                            <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                <span>群組名稱</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:DropDownList ID="DropDownList_Group" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" onchange="show_machines('ContentPlaceHolder1_DropDownList_Group')"></asp:DropDownList>
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
                                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                                        </asp:ScriptManager>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                            <ContentTemplate>
                                                <div class="col-md-12 col-sm-6 col-xs-12" style="display: none">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>設備名稱</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:CheckBoxList ID="CheckBoxList_mach" runat="server"></asp:CheckBoxList>
                                                    </div>
                                                </div>

                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>X座標(值)</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:DropDownList ID="DropDownList_x" runat="server" AutoPostBack="True" CssClass="btn btn-default dropdown-toggle" OnSelectedIndexChanged="DropDownListx_SelectedIndexChanged" Width="100%">
                                                            <asp:ListItem Value="product">工藝名稱(運行程式)</asp:ListItem>
                                                            <asp:ListItem Value="machine">設備名稱</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>Y座標(值)</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:DropDownList ID="DropDownList_Y" CssClass="btn btn-default dropdown-toggle" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>日期選擇</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                        <asp:TextBox ID="txt_str" runat="server" Style="" TextMode="Date" CssClass="form-control   text-center" Width="100%"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                        <asp:TextBox ID="txt_end" runat="server" CssClass="form-control  text-center" TextMode="Date" Width="100%"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12" >
                                                    <div class="col-md-4 col-sm-12 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>顯示筆數</span>
                                                    </div>
                                                    <div class="col-md-5 col-sm-12 col-xs-5" style="margin: 0px 0px 5px 0px">
                                                        <asp:TextBox ID="txt_showCount" runat="server" Text="10" CssClass="form-control text-center" TextMode="Number"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-3 col-sm-12 col-xs-3">
                                                        <span>
                                                            <asp:CheckBox ID="CheckBox_All" runat="server" Text="全部" onclick="checkstatus()" />
                                                        </span>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <br />
                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                        <div class="col-md-9 col-xs-8">
                                        </div>
                                        <div class="col-md-3 col-xs-12">
                                            <button id="btncheck" type="button" class="btn btn-primary antosubmit2 ">執行搜索</button>
                                            <asp:Button runat="server" Text="提交" ID="button_select" CssClass="btn btn-primary" Style="display: none" OnClick="button_select_Click" />
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
            <div role="tabpanel" class="tab-pane fade  " id="tab_content2" aria-labelledby="profile-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">

                            <div class="x_content">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h1 class="text-center _mdTitle" style="width: 100%"><b>生產履歷列表</b></h1>
                                        <h3 class="text-center _xsTitle" style="width: 100%"><b>生產履歷列表</b></h3>
                                        <div class="clearfix"></div>
                                    </div>
                                    <p class="text-muted font-13 m-b-30">
                                    </p>
                                    <table id="datatable-buttons" class="table  table-ts table-bordered nowrap" cellspacing="0" width="100%">
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
                </div>
            </div>
        </div>
    </div>

    <!-----------------/content------------------>

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
    <script>
        window.onload = show_value();

        function show_value() {
            var urlname = document.URL;
            if (urlname.indexOf('tab_content2') != -1)
                document.getElementById("div_button").style.display = 'none';


        }

        function showbar() {

            document.getElementById("div_button").style.display = 'none';
        }

        function show_control() {
            var WhatSystem = navigator.userAgent;
            if (WhatSystem.match(/android/i) || WhatSystem.match(/(iphone|ipad|ipod);?/i)) {

            } else
                document.getElementById("div_button").style.display = 'block';
        }
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
            } var chart = new CanvasJS.Chart("chartContainer", {
                //exportEnabled: true,
                colorSet: "greenShades",
                animationEnabled: true,
                theme: "light1",
                title: {
                    text: <%=title_text %>,
                    fontFamily: "NotoSans",
                    fontWeight: "bolder",
                }, subtitles: [{
                    text: '<%=timerange %>',
                    fontFamily: 'NotoSans',
                    fontWeight: 'bolder',
                    textAlign: "'center",
                }],
                axisX: {
                    title: '<%=x_value%>',
                    labelAngle: -180,
                    intervalType: "year"
                },
                axisY: {
                    title: y_text,
                    lineThickness: 1,
                    lineColor: "#d0d0d0",
                    gridColor: "transparent",
                },
                legend: {
                    fontSize: 15,
                    cursor: "pointer",
                    fontFamily: "NotoSans",
                },
                toolTip: {
                    shared: true,
                    content: toolTipContent
                },
                data: [{
                    type: "stackedColumn",
                    fontFamily: "NotoSans",
                    indexLabelPlacement: "outside", indexLabelBackgroundColor: "white",
                    name: y_text,
                    dataPoints: [
                   <%= col_data_Points%>
                    ]
                }]
            });
            chart.render();

        }
        $(".iconimage").on("click", function () {
            hide_div();
        });




        function checkstatus() {
            var checkBox = document.getElementById('ContentPlaceHolder1_CheckBox_All');
            var text = document.getElementById('ContentPlaceHolder1_txt_showCount');
            if (checkBox.checked == true) {
                text.disabled = true;
            } else {
                text.disabled = false;
            }
        }

        //20190605，日期區間格式判斷
        $("#btncheck").click(function () {



            var type = document.getElementById('ContentPlaceHolder1_DropDownList_factory');
            var group = document.getElementById('ContentPlaceHolder1_DropDownList_Group');
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
                document.getElementById('<%=button_select.ClientID %>').click();
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
        function ChangeChartType(Type) {
            for (var i = 0; i < chart_cust.options.data.length; i++) {
                chart_cust.options.data[i].type = Type;
            }
            chart_cust.render();
        }

        var y_text = '<%=y_text%>';


        var chart = new CanvasJS.Chart("chartContainer", {
            //exportEnabled: true,
            colorSet: "greenShades",
            animationEnabled: true,
            theme: "light1",
            title: {
                text: <%=title_text %>,
                fontFamily: "NotoSans",
                fontWeight: "bolder",
            }, subtitles: [{
                text: '<%=timerange %>',
                fontFamily: 'NotoSans',
                fontWeight: 'bolder',
                textAlign: "'center",
            }],
            axisX: {
                title: '<%=x_value%>',
                labelAngle: -180,
                intervalType: "year"
            },
            axisY: {
                title: y_text,
                lineThickness: 1,
                lineColor: "#d0d0d0",
                gridColor: "transparent",
            },
            legend: {
                fontSize: 15,
                cursor: "pointer",
                fontFamily: "NotoSans",
            },
            toolTip: {
                shared: true,
                content: toolTipContent
            },
            data: [{
                type: "stackedColumn",
                fontFamily: "NotoSans",
                indexLabelPlacement: "outside", indexLabelBackgroundColor: "white",
                name: y_text,
                dataPoints: [
                   <%= col_data_Points%>
                ]
            }]
        });
        chart.render();

        function reload_image() {
            var chart = new CanvasJS.Chart("chartContainer", {
                //exportEnabled: true,
                colorSet: "greenShades",
                animationEnabled: true,
                theme: "light1",
                title: {
                    text: <%=title_text %>,
                    fontFamily: "NotoSans",
                    fontWeight: "bolder",
                }, subtitles: [{
                    text: '<%=timerange %>',
                    fontFamily: 'NotoSans',
                    fontWeight: 'bolder',
                    textAlign: "'center",
                }],
                axisX: {
                    title: '<%=x_value%>',
                    labelAngle: -180,
                    intervalType: "year"
                },
                axisY: {
                    title: y_text,
                    lineThickness: 1,
                    lineColor: "#d0d0d0",
                    gridColor: "transparent",
                },
                legend: {
                    fontSize: 15,
                    cursor: "pointer",
                    fontFamily: "NotoSans",
                },
                toolTip: {
                    shared: true,
                    content: toolTipContent
                },
                data: [{
                    type: "stackedColumn",
                    fontFamily: "NotoSans",
                    indexLabelPlacement: "outside", indexLabelBackgroundColor: "white",
                    name: y_text,
                    dataPoints: [
                   <%= col_data_Points%>
                    ]
                }]
            });
            chart.render();

        }


        document.getElementById("exportChart").addEventListener("click", function () {
            chart.exportChart({ format: "png" });
            parent.location.reload();
        });
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
            return (str2.concat(str));
        }
        $(document).ready(function () {

            $('#example').DataTable({
                responsive: true
            });
            $('#exampleInTab').DataTable({
                responsive: true
            });

            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                reload_image();
                $($.fn.dataTable.tables(true)).DataTable()
                    .columns.adjust()
                    .responsive.recalc();
            });
        });
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



    </script>
</asp:Content>
