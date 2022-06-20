<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Production_History_old.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_CNC.Production_History" %>

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

    <div class="right_col" role="main">
        <!-----------------title------------------>
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
            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true" onclick="a()">圖片模式</a>
            </li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" id="profile-tab" role="tab" data-toggle="tab" aria-expanded="false" onclick="b()">表格模式</a>
            </li>
        </ul>
        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">
                        <div class="col-md-9 col-sm-12 col-xs-12 _setborder">
                            <div class="dashboard_graph x_panel">
                                <div class="x_content">
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div style="text-align: right; width: 100%; padding: 0;">
                                                <button style="display: none" type="button" id="exportChart" title="另存成圖片">
                                                    <img src="../../assets/images/download.jpg" style="width: 36.39px; height: 36.39px;">
                                                </button>
                                            </div>
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div id="chartContainer"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <i id="cbx_remind"></i>
                                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                                        </asp:ScriptManager>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                            <ContentTemplate>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>設備廠區</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:DropDownList ID="DropDownList_factory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_factory_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>設備群組</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:DropDownList ID="DropDownList_Group" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_Group_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>設備名稱</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:CheckBoxList ID="CheckBoxList_mach" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CheckBoxList_mach_SelectedIndexChanged"  ></asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>X座標(值)</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:DropDownList ID="DropDownList_x" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListx_SelectedIndexChanged">
                                                            <asp:ListItem Value="product">產品名稱</asp:ListItem>
                                                            <asp:ListItem Value="machine">機台名稱</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>Y座標(值)</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:DropDownList ID="DropDownList_Y" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>日期選擇</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                        <asp:TextBox ID="txt_str" runat="server" Style="" TextMode="Date" CssClass="form-control   text-left"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                        <asp:TextBox ID="txt_end" runat="server" CssClass="form-control  text-left" TextMode="Date"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-12 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>顯示筆數</span>
                                                    </div>
                                                    <div class="col-md-5 col-sm-12 col-xs-5" style="margin: 0px 0px 5px 0px">
                                                        <asp:TextBox ID="txt_showCount" runat="server" Text="10" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-3 col-sm-12 col-xs-3">
                                                        <span>
                                                            <asp:CheckBox ID="CheckBox_All" runat="server" Text="全部" AutoPostBack="true" OnCheckedChanged="CheckBox_All_CheckedChanged" />
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
            <div role="tabpanel" class="tab-pane fade" id="tab_content2" aria-labelledby="profile-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">
                        <div class="col-md-13 col-sm-12 col-xs-12">

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
    <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
    <script src="../../assets/vendors/time/loading.js"></script>
    <script>

        //20190605，日期區間格式判斷
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();
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
                title: '<%=DropDownList_Y.SelectedItem.Text%>',
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
                name: '<%=DropDownList_Y.SelectedItem.Text%>',
                dataPoints: [
                   <%= col_data_Points%>
                ]
            }]
        });
        chart.render();
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
                $($.fn.dataTable.tables(true)).DataTable()
                    .columns.adjust()
                    .responsive.recalc();
            });
        });
    </script>
</asp:Content>
