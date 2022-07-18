<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="xy_test.aspx.cs" Inherits="dek_erpvis_v2.pages.exp_page.xy_test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../assets/build/css/custom.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-----------------title------------------>
    <div class="right_col" role="main">
        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div>
                    <span>X : </span>
                    <asp:dropdownlist id="dropdownlist_X" runat="server">
                        <asp:ListItem Value="PLINE_NO" Selected="True">產線</asp:ListItem>
                        <asp:ListItem Value="CUST_NO" >客戶</asp:ListItem>
                    </asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div>
                    <span>Y : </span>
                    <asp:dropdownlist id="dropdownlist_y" runat="server">
                        <asp:ListItem Value="AMOUNT">金額</asp:ListItem>
                        <asp:ListItem Value="QUANTITY" Selected="True">數量</asp:ListItem>
                    </asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div>
                    <span>TIME : </span>
                    <asp:textbox id="textbox_dt1" runat="server" text="20190526"></asp:textbox>
                    ~
                    <asp:textbox id="textbox_dt2" runat="server" text="20190625"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div>
                    <span>COUNT : </span>
                    <asp:dropdownlist id="dropdownlist_count" runat="server">
                        <asp:ListItem Value="5">5</asp:ListItem>
                        <asp:ListItem Value="10" Selected="True">10</asp:ListItem>
                        <asp:ListItem Value="20">20</asp:ListItem>
                        <asp:ListItem Value="25">25</asp:ListItem>
                        <asp:ListItem Value="50">50</asp:ListItem>
                    </asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <span>STATUS : </span>
                <asp:dropdownlist id="DropDownList_orderStatus" runat="server">
                    <asp:ListItem Value="0" Selected="True">訂單總數</asp:ListItem>
                    <asp:ListItem Value="1">已結案訂單</asp:ListItem>
                    <asp:ListItem Value="2">未結案訂單</asp:ListItem>
                    <asp:ListItem Value="3">已排程</asp:ListItem>
                    <asp:ListItem Value="4">未排程</asp:ListItem>
                </asp:dropdownlist>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div>
                    <span>TYPE : </span>
                    <asp:dropdownlist id="dropdownlist_chartType" runat="server">
                        <asp:ListItem Value="line">line</asp:ListItem>
                        <asp:ListItem Value="column" Selected="True">column</asp:ListItem>
                    </asp:dropdownlist>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div>
                    <br />
                    <asp:button runat="server" text="提交" id="Button_submit" onclick="Button_submit_Click" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="dashboard_graph x_panel">
                    <div class="x_content">
                        <div id="chartContainer" style="height: 500px; max-width: 100%; margin: 0px auto;"></div>
                    </div>
                </div>
            </div>
        </div>
        <!-----------------/content------------------>
    </div>
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
    <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
    <script>
        var chart = new CanvasJS.Chart("chartContainer", {
            animationEnabled: true,
            theme: "light2",
            title: {
                text: '<%=title%>',
            },
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
            },
            toolTip: {
                shared: true,
            },
            data: [{
                type:  '<%=chartType%>',
                indexLabelPlacement: "outside",
                name: '<%= unit %>',
                dataPoints: [
                <%=chartData%>
                    //{ y: 263, label: '40盤', indexLabel: '263' }, { y: 44, label: '50盤', indexLabel: '44' }, { y: 10, label: 'MAZAK', indexLabel: '10' }, { y: 152, label: 'T1', indexLabel: '152' }, { y: 8, label: '臥式大圓盤', indexLabel: '8' }, { y: 108, label: '鍊式', indexLabel: '108' },
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

        $(function () {
            $('#dt1,#dt2').daterangepicker({
                singleDatePicker: true,
                autoUpdateInput: false,
                locale: {
                    cancelLabel: 'Clear'
                }
            });
            $('#dt1,#dt2').on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format('YYYYMMDD'));
            });
            $('#dt1,#dt2').on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
            });
        });
    </script>
</asp:Content>
