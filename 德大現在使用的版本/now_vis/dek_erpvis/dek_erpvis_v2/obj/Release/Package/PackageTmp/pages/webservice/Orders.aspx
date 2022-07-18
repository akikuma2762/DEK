﻿<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="dek_erpvis_v2.pages.webservice.Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>訂單統計 | 德大機械</title>
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-----------------title------------------>

    <div class="right_col" role="main">
        <%= path %><br>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-9 col-sm-12 col-xs-12">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="dashboard_graph x_panel" id="Div_Shadow">
                        <div class="x_content">
                            <div class="col-md-12 col-sm-12 col-xs-10">
                                <div id="chartContainer" style="height: 520px; max-width: 100%; margin: 0px auto;"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-12 col-xs-12">
                <%-- 2019.08.05，訂單總計資訊(右上角) --%>
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="dashboard_graph x_panel" id="Div_Shadow">
                        <div class="x_content">
                            <p><%=right_title %></p>
                            <div class="h2 mb-0 text-success" style="margin-bottom: 10px;"><b><%=排行內總計 %></b></div>
                            <div runat="server" id="divBlock">
                                <div class="progress progress_sm" style="width: 95%;">
                                    <div class="progress-bar bg-green" role="progressbar" data-transitiongoal="<%=rate %>"></div>
                                </div>
                                <button type="button" class="btn btn-success btn-sm">訂單總<%=yString %> <strong><%=Total %></strong></button>
                                <button type="button" class="btn btn-success btn-sm">佔總訂單 <strong><%=rate %> %</strong></button>
                            </div>
                        </div>
                    </div>
                </div>
                <%-- 2019.08.05，篩選條件 --%>
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="dashboard_graph x_panel" id="Div_Shadow" style="height: 1000%">
                        <div class="x_content">
                            <i id="cbx_remind"></i>
                            <div class="col-md-12 col-sm-6 col-xs-12">
                                <div class="col-md-4 col-sm-3 col-xs-12">
                                    <span>X 座標</span>
                                </div>
                                <div class="col-md-8 col-sm-9 col-xs-12">
                                    <asp:DropDownList ID="dropdownlist_X" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Button_submit_Click">
                                        <asp:ListItem Value="Line">產線</asp:ListItem>
                                        <asp:ListItem Value="Custom">客戶</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-6 col-xs-12">
                                <div class="col-md-4 col-sm-3 col-xs-12">
                                    <span>Y 座標</span>
                                </div>
                                <div class="col-md-8 col-sm-9 col-xs-12">
                                    <%--為控制"金額"使用權限，此下拉選單改為動態產生
                                        <asp:ListItem Value="AMOUNT">金額</asp:ListItem>
                                    <asp:ListItem Value="QUANTITY" Selected="True">數量</asp:ListItem>--%>
                                    <asp:DropDownList ID="dropdownlist_y" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Button_submit_Click">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-6 col-xs-12">
                                <div class="col-md-4 col-sm-3 col-xs-12">
                                    <span>預交日期</span>
                                </div>
                                <div class="col-md-8 col-sm-9 col-xs-12">
                                    <asp:TextBox ID="textbox_dt1" runat="server" Text="" placeholder="yyyyMMdd" CssClass="form-control has-feedback-left"></asp:TextBox>
                                    <span class="fa fa-calendar-o form-control-feedback left" aria-hidden="true"></span>
                                    <span id="inputSuccess2Status1" class="sr-only">(success)</span>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-6 col-xs-12">
                                <div class="col-md-4 col-sm-3 col-xs-12">
                                    <span></span>
                                </div>
                                <div class="col-md-8 col-sm-9 col-xs-12">
                                    <asp:TextBox ID="textbox_dt2" runat="server" Text="" placeholder="yyyyMMdd" CssClass="form-control has-feedback-left"></asp:TextBox>
                                    <span class="fa fa-calendar-o form-control-feedback left" aria-hidden="true"></span>
                                    <span id="inputSuccess2Status2" class="sr-only">(success)</span>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-6 col-xs-12">
                                <div class="col-md-4 col-sm-3 col-xs-12">
                                    <span>日期快選</span>
                                </div>
                                <div class="col-md-8 col-sm-9 col-xs-12">
                                    <div class="btn-group btn-group-justified">
                                        <asp:LinkButton ID="LinkButton_month" runat="server" CssClass="btn btn-default " OnClick="Button_submit_Click">當月</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-6 col-xs-12">
                                <div class="col-md-4 col-sm-3 col-xs-12">
                                    <span>訂單狀態</span>
                                </div>
                                <div class="col-md-8 col-sm-9 col-xs-12">
                                    <asp:DropDownList ID="DropDownList_orderStatus" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Button_submit_Click">
                                        <asp:ListItem Value="All" Selected="True">訂單總數</asp:ListItem>
                                        <asp:ListItem Value="Finished">已結案訂單</asp:ListItem>
                                        <asp:ListItem Value="Unfinished">未結案訂單</asp:ListItem>
                                        <asp:ListItem Value="Scheduled">已排程</asp:ListItem>
                                        <asp:ListItem Value="Unscheduled">未排程</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-6 col-xs-12">
                                <div class="col-md-4 col-sm-3 col-xs-12">
                                    <span>顯示筆數</span>
                                </div>
                                <div class="col-md-8 col-sm-9 col-xs-12">
                                    <%--2019.07.31，因下拉選單只能選擇特定筆數，調整成輸入框，預設筆數: 10
                                        <asp:DropDownList ID="dropdownlist_count" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Button_submit_Click">
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                        <asp:ListItem Value="10" Selected="True">10</asp:ListItem>
                                        <asp:ListItem Value="20">20</asp:ListItem>
                                        <asp:ListItem Value="25">25</asp:ListItem>
                                        <asp:ListItem Value="50">50</asp:ListItem>
                                    </asp:DropDownList>--%>
                                    <asp:TextBox ID="txt_showCount" runat="server" Text="10" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-6 col-xs-12">
                                <div class="col-md-4 col-sm-3 col-xs-12">
                                    <span>顯示圖型</span>
                                </div>
                                <div class="col-md-8 col-sm-9 col-xs-12">
                                    <asp:DropDownList ID="dropdownlist_chartType" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Button_submit_Click">
                                        <asp:ListItem Value="line">折線圖</asp:ListItem>
                                        <asp:ListItem Value="column" Selected="True">長條圖</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <br />
                        <button id="btncheck" type="button" class="btn btn-primary antosubmit2 ">執行運算</button>
                        <asp:Button runat="server" Text="執行運算2" ID="Button_submit" CssClass="btn btn-primary antosubmit2" OnClick="Button_submit_Click" Style="display: none" />
                    </div>
                </div>
            </div>
        </div>
        <!--Table-->
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel" id="Div_Shadow">
                    <div class="x_title">
                        <h2>訂單統計<small></small></h2>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <p class="text-muted font-13 m-b-30"></p>
                        <table id="datatable-buttons" class="table  table-ts table-bordered nowrap" cellspacing="0" width="100%">
                            <thead>
                                <tr id="tr_row">
                                    <%=th %>
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
      var remind = document.getElementById("cbx_remind");
      remind.style.color = "#FF3333";
      $("#btncheck").click(function () {
          var start_time = document.getElementsByName("ctl00$ContentPlaceHolder1$textbox_dt1")[0].value;
          var end_time = document.getElementsByName("ctl00$ContentPlaceHolder1$textbox_dt2")[0].value;
          if (start_time != "" && end_time != "") {
              var re = /^[0-9]+$/;
              if (!re.test(start_time) && !re.test(end_time)) {
                  remind.innerHTML = "只能輸入數字 !";
              }
              else {
                  if (start_time.length != 8 || end_time.length != 8) {
                      remind.innerHTML = "輸入日期格式有誤,請重新檢查 ! !";
                  } else {
                      if (start_time < end_time) {
                          document.getElementById('<%=Button_submit.ClientID %>').click();
                        }
                        else {
                            remind.innerHTML = "起始日期有誤,請重新檢查 !";
                        }
                    }
                }
            } else {
                remind.innerHTML = "日期不得為空,請重新檢查 !";
            }
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
      var chart = new CanvasJS.Chart("chartContainer", {
          //exportEnabled: true,
          animationEnabled: true,
          colorSet: "greenShades",
          theme: "light1",
          title: {
              text: '<%=title%>',
                fontFamily: "NotoSans",
                fontWeight: "bolder",
                //fontFamily: "arial",
            },
            subtitles: [{
                text: '<%=subtitle%>',
                fontFamily: "NotoSans",
                fontWeight: "bolder",
                fontSize: 18,
            }],
            axisX: {
                //interval: 1,
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
                indexLabelPlacement: "outside", indexLabelBackgroundColor: "white",
                name: '<%=chart_unit %>',
                dataPoints: [
                        <%=chartData %>
                    //{ y: 263, label: '40盤', indexLabel: '263' }, { y: 44, label: '50盤', indexLabel: '44' }, { y: 10, label: 'MAZAK', indexLabel: '10' }, { y: 152, label: 'T1', indexLabel: '152' }, { y: 8, label: '臥式大圓盤', indexLabel: '8' }, { y: 108, label: '鍊式', indexLabel: '108' },
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

