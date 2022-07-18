<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Asm_Cahrt_Error.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PM.Asm_Cahrt_Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>異常統計分析 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/dp_PM/Asm_Cahrt_Error.css" rel="stylesheet" />


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
    </style>
    <!-----------------content------------------>
    <div class="right_col" role="main">
        <!------------------TitleRef----------------------->
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁</a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PMD">生產部</a></u></li>
        </ol>
        <!-----------------title------------------>
        <div class="page-title">
            <div class="row">
            </div>
        </div>
        <div class="clearfix"></div>
        <!-----------------content------------------>

        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true">圖片模式</a></li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" id="profile-tab" role="tab" data-toggle="tab" aria-expanded="false">表格模式</a></li>
        </ul>

        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="col-md-9 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <div class="row">
                                            <div class="col-md-12 col-sm-8 col-xs-12">
                                                <div style="text-align: right; width: 100%; padding: 0;">
                                                    <button style="display:none" type="button" id="exportChart" title="另存成圖片">
                                                        <img src="../../assets/images/download.jpg" style="width: 36.39px; height: 36.39px;">
                                                    </button>
                                                </div>
                                                <div class="col-md-12 col-sm-12 col-xs-12  _setborder">
                                                    <div id="chartpie_Count" style=""></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-12 col-xs-12 _select">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <div class="dashboard_graph x_panel">
                                        <div class="x_content">
                                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                                            </asp:ScriptManager>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                                <ContentTemplate>
                                                    <div class="col-md-12 col-sm-6 col-xs-12">
                                                        <div class="col-md-4 col-sm-3 col-xs-5" style="margin: 5px 0px 5px 0px">
                                                            <span>選擇廠區</span>
                                                        </div>
                                                        <div class="col-md-8 col-sm-9 col-xs-7">
                                                            <asp:DropDownList ID="dropdownlist_X" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" OnSelectedIndexChanged="dropdownlist_X_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Value="0" Selected="True">立式</asp:ListItem>
                                                                <asp:ListItem Value="1">臥式</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                                        <div class="col-md-4 col-sm-3 col-xs-12">
                                                            <span>選擇產線</span>
                                                        </div>
                                                        <div class="col-md-8 col-sm-9 col-xs-12">
                                                            <asp:CheckBoxList ID="CheckBoxList_Line" runat="server" CssClass="table-striped" Font-Size="20px"></asp:CheckBoxList>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="col-md-4 col-sm-3 col-xs-5" style="margin: 5px 0px 5px 0px">
                                                    <span>Y座標(值)</span>
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-7">
                                                    <asp:DropDownList ID="dropdownlist_y" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%">
                                                        <asp:ListItem Value="0" Selected="True">次數</asp:ListItem>
<%--                                                        <asp:ListItem Value="1">分鐘</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="col-md-12 col-sm-6 col-xs-12">
                                                <div class="col-md-4 col-sm-12 col-xs-5" style="margin: 5px 0px 5px 0px">
                                                    <span>顯示筆數</span>
                                                </div>
                                                <div class="col-md-5 col-sm-12 col-xs-4" style="margin: 0px 0px 5px 0px">

                                                    <asp:TextBox ID="txt_showCount" runat="server" Text="10" CssClass="form-control text-center" TextMode="Number"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3 col-sm-12 col-xs-3" style="margin: 5px 0px 5px 0px;">
                                                    <span>
                                                                <asp:CheckBox ID="CheckBox_All" runat="server" Text="全部" onclick="checkstatus()" />
                                                    </span>
                                                </div>
                                            </div>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="True">
                                                <ContentTemplate>
                                                    <div class="col-md-12 col-sm-6 col-xs-12">
                                                        <div class="col-md-4 col-sm-3 col-xs-5" style="margin: 5px 0px 5px 0px">
                                                            <span>日期快選</span>
                                                        </div>
                                                        <div class="col-md-8 col-sm-9 col-xs-7">
                                                            <div class="btn-group btn-group-justified" style="margin: 0px 0px 5px 0px">
                                                                <asp:LinkButton ID="date_month" runat="server" CssClass="btn btn-default " OnClick="LinkButton_day_Click" Style="text-align: center">當月</asp:LinkButton>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 col-sm-6 col-xs-12">
                                                        <div class="col-md-4 col-sm-3 col-xs-5" style="margin: 5px 0px 5px 0px">
                                                            <span>異常日期</span>
                                                        </div>
                                                        <div class="col-md-8 col-sm-9 col-xs-7" style="margin: 0px 0px 5px 0px">
                                                            <asp:TextBox ID="textbox_dt1" runat="server" Style="" TextMode="Date" CssClass="form-control   text-center"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 col-sm-6 col-xs-12">
                                                        <div class="col-md-4 col-sm-3 col-xs-5">
                                                        </div>
                                                        <div class="col-md-8 col-sm-9 col-xs-7" style="margin: 0px 0px 5px 0px">
                                                            <asp:TextBox ID="textbox_dt2" runat="server" CssClass="form-control  text-center" TextMode="Date"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="col-md-9 col-xs-8">
                                                </div>
                                                <div class="col-md-3 col-xs-12">
                                                    <button id="btncheck" type="button" class="btn btn-primary antosubmit2 ">執行搜索</button>
                                                    <asp:Button ID="button_select" runat="server" Text="執行檢索" class="btn btn-secondary" Style="display: none" OnClick="button_select_Click" />
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
            <div role="tabpanel" class="tab-pane fade" id="tab_content2" aria-labelledby="profile-tab">

                <div class="x_panel Div_Shadow">

                    <div class="x_content">
                        <div class="x_panel">
                            <div class="x_title" style="text-align: center">
                                <h1 class="text-center _mdTitle" style="width: 100%"><b>異常統計列表</b></h1>
                                <h3 class="text-center _xsTitle" style="width: 100%"><b>異常統計列表</b></h3>
                                <div class="clearfix"></div>
                            </div>
                            <table id="datatable-buttons" class="table  table-ts table-bordered nowrap" cellspacing="0" width="100%">
                                <thead>
                                    <tr id="tr_row">
                                        <%=ColumnsData%>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%=RowsData %>
                                </tbody>
                            </table>
                        </div>

                    </div>
                </div>
            </div>
        </div>


    </div>



    <!-- Modal1 -->

    <!-- /Modal1 -->
    <!--/set Modal-->
    <!-- Modal -->
    <!-- /Modal -->
    <!-- jQuery -->
    <script src="../../assets/vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="../../assets/vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- bootstrap-progressbar -->
    <script src="../../assets/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js"></script>
    <!-- bootstrap-daterangepicker -->
    <script src="../../assets/vendors/moment/min/moment.min.js"></script>
    <script src="../../assets/vendors/bootstrap-daterangepicker/daterangepicker.js"></script>

    <!-- iCheck -->
    <script src="../../assets/vendors/iCheck/icheck.min.js"></script>
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
    <script src="../../assets/vendors/time/loading.js"></script>
    <!-----------------------Chart-------------------------------------->
    <script>
        function checkstatus() {
            var checkBox = document.getElementById('ContentPlaceHolder1_CheckBox_All');
            var text = document.getElementById('ContentPlaceHolder1_txt_showCount');
            if (checkBox.checked == true) {
                text.disabled = true;
            } else {
                text.disabled = false;
            }
        }
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
        });
        function pageLoad() { //autoPostBack會使原先綁訂的監聽及js失效,當返回用戶端時都會執行 pageLoad() 方法,將事件綁定的設定寫在 pageLoad() 方法中即可
            $('#ContentPlaceHolder1_CheckBoxList_Line input').change(function () {
                var check = $(this).context.checked;
                var val = $(this).val();
                if (val == "" && check == true) {
                    seletedAllItem(true);
                } else if (val == "" && check == false) {
                    seletedAllItem(false);
                } else if (val != "" && check == false) {
                    if ($(this)[0].id.split("_")[3] != 0)//ContentPlaceHolder1[1]_checkBoxList[2]_LINE_0[3]
                    {
                        var x = document.getElementById("ContentPlaceHolder1_CheckBoxList_Line_0");
                        x.checked = false;
                        //seletedItem(7, true);
                    }
                }
            });
            function seletedAllItem(seleted) {
                $('#ContentPlaceHolder1_CheckBoxList_Line input').each(function () {
                    $(this).context.checked = seleted;
                });
            }
            function seletedItem(num, seleted) {
                var x = document.getElementById("ContentPlaceHolder1_CheckBoxList_Line_" + num);
                x.checked = true;
            }
        }
       
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();
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
        var chartpieC = new CanvasJS.Chart("chartpie_Count",
            {
                colorSet: "greenShades",
                // exportEnabled: true,
                animationEnabled: true,
                title: {
                    text: "異常次數統計", fontFamily: "NotoSans",
                    fontWeight: "bolder",
                }, subtitles: [{
                    text: '<%=time_area_text%>',
                    fontFamily: 'NotoSans',
                    fontWeight: 'bolder',
                    textAlign: "'center",
                }],
                axisX: {
                    title: '異常類型',
                    interval: 1,
                    intervalType: "year"
                },
                axisY: {
                    title:'<%=y_value%>',
                    lineThickness: 1,
                    lineColor: "#d0d0d0",
                    gridColor: "transparent",
                },
                legend: { cursor: "pointer" },
                data: [{
                    //20191115開會討論由圓餅圖改直方圖
                    type: "column", toolTipContent: "{label}: <strong>{y}</strong>", indexLabel: "{y}", indexLabelBackgroundColor: "white",
                    dataPoints: [
                    <%=
                    ChartData_Count
                    %>
                    ]
                }]
            });
        chartpieC.render();

        document.getElementById("exportChart").addEventListener("click", function () {
            chartpieC.exportChart({ format: "png" });
            parent.location.reload();
        });
        var chartpieT = new CanvasJS.Chart("chartpie_Times",
            {
                colorSet: "greenShades",
                //exportEnabled: true,
                animationEnabled: true,
                title: {
                    text: "時間" + '<%=TimeUnit %>', fontFamily: "NotoSans",
                    fontWeight: "bolder",
                },
                axisX: {
                    interval: 1,
                    intervalType: "year"
                },
                axisY: {
                    lineThickness: 1,
                    lineColor: "#d0d0d0",
                    gridColor: "transparent",
                    //reversed: true, //第四象限 正值
                    //break作法
                    /*scaleBreaks: {
                        customBreaks: [{
                            startValue:300000,
                            endValue:  800000
                        }]
                    }*/
                },
                legend: { cursor: "pointer" },
                data: [{
                    //20191115開會討論由圓餅圖改直方圖
                    type: "column",
                    toolTipContent: "{label}: <strong>{y}" + '<%=TimeUnit %>' + "</strong>", indexLabel: "{y}" + '<%=TimeUnit %>', indexLabelBackgroundColor: "white",
                    dataPoints: [
                    <%=ChartData_Time%>
                    ]
                }]
            });
        chartpieT.render();
        document.getElementById("exportimage").addEventListener("click", function () {
            chartpieT.exportChart({ format: "png" });
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
            str3 = "<span style = 'color:Tomato'>Total:</span><strong>" + total + "</strong><br/>";
            return (str2.concat(str)).concat(str3);
        }

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
        });

    </script>
</asp:Content>
