<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="stockanalysis.aspx.cs" Inherits="dek_erpvis_v2.pages.webservice.stockanalysis" %>

<%--<%@ OutputCache duration="10" varybyparam="None" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>成品庫存分析 | 德大機械</title>
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>

        input[type="checkbox"] {
            width: 18px;
            height: 18px;
            cursor: auto;
            -webkit-appearance: default-button;
        }



          .col-xs-1,
        .col-sm-1,
        .col-md-1,
        .col-lg-1,
        .col-xs-2,
        .col-sm-2,
        .col-md-2,
        .col-lg-2,
        .col-xs-3,
        .col-sm-3,
        .col-md-3,
        .col-lg-3,
        .col-xs-4,
        .col-sm-4,
        .col-md-4,
        .col-lg-4,
        .col-xs-5,
        .col-sm-5,
        .col-md-5,
        .col-lg-5,
        .col-xs-6,
        .col-sm-6,
        .col-md-6,
        .col-lg-6,
        .col-xs-7,
        .col-sm-7,
        .col-md-7,
        .col-lg-7,
        .col-xs-8,
        .col-sm-8,
        .col-md-8,
        .col-lg-8,
        .col-xs-9,
        .col-sm-9,
        .col-md-9,
        .col-lg-9,
        .col-xs-10,
        .col-sm-10,
        .col-md-10,
        .col-lg-10,
        .col-xs-11,
        .col-sm-11,
        .col-md-11,
        .col-lg-11,
        .col-xs-12,
        .col-sm-12,
        .col-md-12,
        .col-lg-12 {
            padding-right: 4px;
            padding-left: 4px;
        }
    </style>
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <%=path %>
        <br>
        <div class="clearfix"></div>
        <!-----------------/title------------------>

        <div class="page-title">
            <div class="row">
                <%--  <div class="col-md-6 col-sm-12 col-xs-12">
                    <h4>[搜尋條件]：<u><asp:LinkButton ID="LinkButton1" runat="server" data-toggle="modal" data-target="#exampleModal" Style="font-size: 25px">設定搜尋條件</asp:LinkButton></u></h4>
                </div>--%>
            </div>
        </div>
        <div class="clearfix"></div>
        <!-----------------content------------------>
        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true">圖片模式</a>
            </li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" id="profile-tab" role="tab" data-toggle="tab" aria-expanded="false">表格模式</a>
            </li>
        </ul>
        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">
                <div class="x_panel" id="Div_Shadow">
                    <div class="row">

                        <div class="col-md-9 col-sm-12 col-xs-12">
                            <div class="col-md-6 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <div style="text-align: right; width: 100%; padding: 0;">
                    <%--                        <button type="button" id="exportChart" title="另存成圖片">
                                                <img src="../../assets/images/download.jpg" style="width: 36.39px; height: 36.39px;">
                                            </button>--%>
                                            <div class="row">
                                                <div class="col-md-10 col-sm-10 col-xs-10">
                                                    <div id="chartpie" style="height: 450px; width: 100%; margin: 0px auto;"></div>
                                                </div>
                                                <br>
                                                <br>
                                                <div class="col-md-2 col-sm-4 col-xs-4" style="text-align: center">
                                                    <div class="h2 mb-0 text-primary" style="margin-bottom: 10px; color: #221715;"><%=_val總庫存 %></div>
                                                    <div class="text-muted">庫存總數</div>
                                                    <hr>
                                                </div>

                                                <div class="col-md-2 col-sm-4 col-xs-4" style="text-align: center">
                                                    <div class="h2 mb-0 text-primary" style="margin-bottom: 10px; color: #1b5295;"><%=_val一般庫存 %></div>
                                                    <div class="text-muted">一般庫存</div>
                                                    <hr>
                                                </div>
                                                <div class="col-md-2 col-sm-4 col-xs-4" style="text-align: center">
                                                    <div class="h2 mb-0 text-warning" style="margin-bottom: 10px; color: #d93f47;"><%=_val逾期庫存 %></div>
                                                    <div class="text-muted">逾期庫存</div>
                                                    <hr>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <div class="row">
                                            <div class="col-md-12 col-sm-12 col-xs-10">
                                                <div style="text-align: right; width: 100%; padding: 0;">
                                 <%--                   <button type="button" id="exportimage" title="另存成圖片">
                                                        <img src="../../assets/images/download.jpg" style="width: 36.39px; height: 36.39px;">
                                                    </button>--%>
                                                </div>
                                                <div id="chartContainer" style="height: 450px; width: 100%; margin: 0px auto;"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="col-md-3 col-sm-12 col-xs-12">
                            <div class="dashboard_graph x_panel" style="height: 1000%">
                                <div class="x_content">
                                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                                    </asp:ScriptManager>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                        <ContentTemplate>
                                            <div class="col-md-12 col-sm-6 col-xs-12">
                                                <asp:RadioButtonList ID="RadioButtonList_Type" RepeatColumns="2" AutoPostBack="true" Visible="false" runat="server"  CssClass="table-striped" OnSelectedIndexChanged="RadioButtonList_Type_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Selected="True">選擇庫存天數&nbsp&nbsp</asp:ListItem>
                                                    <asp:ListItem Value="1">選擇庫存期間</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <asp:PlaceHolder ID="PlaceHolder_day" runat="server">
                                                <%--                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-12" style="margin: 5px 0px 5px 0px">
                                                        <span>庫存天數</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-12" style="margin: 0px 0px 5px 0px">
                                                        <div class="btn-group btn-group-justified">
                                                            <asp:DropDownList ID="DropDownList_dayval" runat="server" class="form-control">
                                                                <asp:ListItem Value="0">ALL</asp:ListItem>
                                                                <asp:ListItem Value="1">1</asp:ListItem>
                                                                <asp:ListItem Selected="True" Value="30">30</asp:ListItem>
                                                                <asp:ListItem Value="60">60</asp:ListItem>
                                                                <asp:ListItem Value="90">90</asp:ListItem>
                                                                <asp:ListItem Value="180">180</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>--%>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-12 col-xs-12" style="margin: 5px 0px 5px 0px">
                                                        <span>庫存天數</span>
                                                    </div>
                                                    <div class="col-md-5 col-sm-12 col-xs-9" style="margin: 0px 0px 5px 0px">

                                                        <asp:TextBox ID="txt_showCount" runat="server" Text="30" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-3 col-sm-12 col-xs-3" style="margin: 5px 0px 5px 0px">
                                                        <span>
                                                            <asp:CheckBox ID="CheckBox_All" runat="server" Text="全部" AutoPostBack="true" Style="font-weight: normal" OnCheckedChanged="CheckBox_All_CheckedChanged" />
                                                        </span>
                                                    </div>
                                                </div>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder ID="PlaceHolder_range" runat="server" Visible="false">
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-12" style="margin: 5px 0px 5px 0px">
                                                        <span>日期快選</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-12" style="margin: 0px 0px 5px 0px">
                                                        <div class="btn-group btn-group-justified">
                                                            <asp:LinkButton ID="LinkButton_month" Style="text-align: left" runat="server" CssClass="btn btn-default " OnClick="button_select_Click">當月</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-12" style="margin: 0px 0px 5px 0px">
                                                        <span>庫存區間</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-12" style="margin: 5px 0px 5px 0px">
                                                        <asp:TextBox ID="txt_str" runat="server" CssClass="form-control   text-left" TextMode="Date"></asp:TextBox>

                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-12">
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-12" style="margin: 5px 0px 5px 0px">
                                                        <asp:TextBox ID="txt_end" runat="server" CssClass="form-control  text-left" TextMode="Date"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </asp:PlaceHolder>


                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                                                                                  <div class="col-md-12 col-sm-12 col-xs-12">
                                        <div class="col-md-9 col-xs-8">
                                        </div>
                                        <div class="col-md-3 col-xs-4">
                                                                 <button id="btncheck" type="button" class="btn btn-primary antosubmit2">執行搜索</button>
                                        <asp:Button ID="button_select" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="button_select_Click" Style="display: none" />                        
                                        </div>
                                    </div>

                                  
                                </div>
                            </div>
                        </div>



                    </div>
                </div>
            </div>
            <div role="tabpanel" class="tab-pane fade" id="tab_content2" aria-labelledby="profile-tab">
                <div class="x_panel" id="Div_Shadow">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">

                            <div class="x_content">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h1 class="text-center" style="width: 100%"><b><%=Table_Title %></b>
                                        </h1>
                                        <div class="clearfix"></div>
                                    </div>
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
                                            <%=tr %>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-----------------/content------------------>
    </div>
    <!-- set Modal -->
    <!--<div class="backdrop">
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
    <script src="../../assets/vendors/time/loading.js"></script>
    <script>
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();
        });
        var chartpie = new CanvasJS.Chart("chartpie",
            {
                //exportEnabled: true,
                animationEnabled: true,
                title: {
                    text: '總體庫存分析',
                    fontFamily: "NotoSans",
                    fontWeight: "bolder",
                },
                subtitles: [{
                    text: <%=title_text%>,
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
                    indexLabelFontSize: 18,
                    dataPoints: [
                    <%=pie_data_points%>
                    ]
                }]
            });
        document.getElementById("exportChart").addEventListener("click", function () {
            chartpie.exportChart({ format: "png" });
            parent.location.reload();
        });
        var chart = new CanvasJS.Chart("chartContainer", {
            //exportEnabled: true,
            animationEnabled: true,
            theme: "light2",
            title: {
                text: '各產線庫存分析 ',
                fontFamily: "NotoSans",
                fontWeight: "bolder",
            }, subtitles: [{
                text: <%=title_text%>,
                fontFamily: 'NotoSans',
                fontWeight: 'bolder',
                textAlign: "'center",
            }],
            axisX: {
                title: '產線',
                intervalType: "year"
            },
            axisY: {
                title: '數量',
                lineThickness: 1,
                lineColor: "#d0d0d0",
                gridColor: "transparent",
            },
            legend: {
                fontSize: 15,
                cursor: "pointer",
            },
            toolTip: {
                shared: true,
                content: toolTipContent
            },
            data: [{
                type: "stackedColumn",
                showInLegend: true,
                color: "#5b59ac",
                //把%數加進去了
                name: "一般庫存" + '<%=nstock %>',
                dataPoints: [
                <%=col_data_points_nor %>
                ]
            },
                {
                    type: "stackedColumn",
                    showInLegend: true,
                    color: "#ff4d4d",
                    //把%數加進去了
                    name: "逾期庫存" + '<%=ostock %>',
                    dataPoints: [
                <%=col_data_points_sply %>
                    ]
                }]
        });
        document.getElementById("exportimage").addEventListener("click", function () {
            chart.exportChart({ format: "png" });
            parent.location.reload();
        });
        chart.render();
        chartpie.render();
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
