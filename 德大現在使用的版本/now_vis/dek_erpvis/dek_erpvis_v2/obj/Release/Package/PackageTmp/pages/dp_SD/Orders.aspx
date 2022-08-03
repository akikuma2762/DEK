<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_SD.Orders_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>訂單統計 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/Orders.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <!-----------------title------------------>
    <style>
        #indexlabel {
            color: black;
            position: absolute;
            font-size: 16px;
        }
    </style>
    <div class="right_col" role="main">

        <%= path %><br>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <!--以上狀態統計色塊-->
        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true">圖片模式</a>
            </li>
            <li role="presentation" class=""><a href="#tab_content2" id="profile-tab" role="tab" data-toggle="tab" aria-expanded="false">表格模式</a>
            </li>
            <li role="presentation" class=""><a href="#tab_content3" id="profile-tab2" role="tab" data-toggle="tab" aria-expanded="false">每月訂單數量</a>
            </li>
            <li role="presentation" class=""><a href="#tab_content4" id="profile-tab3" role="tab" data-toggle="tab" aria-expanded="false">每月訂單產能</a>
            </li>
        </ul>
        <div id="myTabContent" class="tab-content">
            <!--圖片及控制項-->
            <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div class="col-md-9 col-sm-12 col-xs-12 padding-left-right-4">
                            <div id="order_image"></div>
                        </div>
                        <div class="col-md-3 col-sm-12 col-xs-12 padding-left-right-4">
                            <%-- 2019.08.05，訂單總計資訊(右上角) --%>
                            
                                <div class="x_panel">
                                    <div class="x_content clearfix">

                                        <h3 style="color: black">訂單<%=yText %>總計：</h3>
                                        <div style="display: none; width: 18%; text-align: right;" id="total_title" runat="server">
                                            <div class="h2 text-success count" style="color: darkred"><b>NTD</b></div>
                                        </div>
                                        <div style="display: inline-block; width: 55%; text-align: right;" id="total_content" runat="server">
                                            <div class="h2 text-success count" style="color: darkred"><b><%=add_total %></b></div>
                                        </div>

                                        <h3 style="color: black">本月訂單總計：</h3>
                                        <div style="display: none; width: 18%; text-align: right;" id="month_title" runat="server">
                                            <div class="h2 text-success count" style="color: darkred"><b>NTD</b></div>
                                        </div>
                                        <div style="display: inline-block; width: 55%; text-align: right;" id="month_content" runat="server">
                                            <div class="h2 text-success count" style="color: darkred"><b><%=Total_All %></b></div>
                                        </div>

                                        <h3 style="color: black">逾期訂單總計：</h3>
                                        <div style="display: none; width: 18%; text-align: right;" id="over_title" runat="server">
                                            <div class="h2 text-success count" style="color: darkred"><b>NTD</b></div>
                                        </div>
                                        <div style="display: inline-block; width: 55%; text-align: right;" id="over_content" runat="server">
                                            <div class="h2 text-success count" style="color: darkred"><b><%=Overdue_Total %></b></div>
                                        </div>
                                        <div runat="server" id="divBlock">
                                            <h2 style="color: black"><%=right_title %>： <strong style="color: darkred">
                                                <br>
                                                <%=排行內總計 %></strong></h2>
                                            <h2 style="color: black">佔總訂單： <strong class="_ColorDark">
                                                <br>
                                                <%=rate %> %</strong></h2>
                                            <div class="progress progress_sm" style="width: 95%;">
                                                <div class="progress-bar bg-green" role="progressbar" data-transitiongoal="<%=rate %>"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            
                            <%-- 2019.08.05，篩選條件 --%>
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                            <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                <div class="col-md-4 col-sm-3 col-xs-4">
                                                    <span>廠區</span>
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-8">
                                                    <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="btn btn-default form-control">
                                                        <asp:ListItem Value="sowon" Selected="True">立式廠</asp:ListItem>
                                                        <asp:ListItem Value="dek">大圓盤</asp:ListItem>
                                                        <asp:ListItem Value="iTec">臥式廠</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>X座標(值)</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:DropDownList ID="dropdownlist_X" runat="server" CssClass="btn btn-default form-control">
                                                    <asp:ListItem Value="PLINE_NO" Selected="True">產線</asp:ListItem>
                                                    <asp:ListItem Value="CUST_NO">客戶</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>Y座標(值)</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:DropDownList ID="dropdownlist_y" runat="server" CssClass="btn btn-default form-control  text-center">
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>日期快選</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <div class="btn-group btn-group-justified">
                                                    <a id="ContentPlaceHolder1_LinkButton_month" class="btn btn-default " onclick=" set_nowmonth()" style="text-align: center">當月</a>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>起始時間</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:TextBox ID="txt_str" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>結束時間</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:TextBox ID="txt_end" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>訂單狀態</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:DropDownList ID="DropDownList_orderStatus" runat="server" CssClass="btn btn-default form-control">
                                                    <asp:ListItem Value="0" Selected="True">訂單總數</asp:ListItem>
                                                    <asp:ListItem Value="1">已結案訂單</asp:ListItem>
                                                    <asp:ListItem Value="2">未結案訂單</asp:ListItem>
                                                    <asp:ListItem Value="5">未結案訂單(製令結案)</asp:ListItem>
                                                    <asp:ListItem Value="6">未結案訂單(製令未結案)</asp:ListItem>
                                                    <asp:ListItem Value="3">已排程</asp:ListItem>
                                                    <asp:ListItem Value="4">未排程</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>顯示筆數</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8 flex-align">
                                                <div class="col-md-5 col-sm-6 col-xs-5">
                                                    <asp:TextBox ID="txt_showCount" runat="server" Text="10" CssClass="form-control text-center" TextMode="Number"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6 col-sm-6 col-xs-4 padding-left-right-4 ">
                                                    <div class="flex-align">
                                                        <asp:CheckBox ID="CheckBox_All" runat="server" Text="全部" onclick="checkstatus('ContentPlaceHolder1_CheckBox_All','ContentPlaceHolder1_txt_showCount')" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="col-md-12 col-xs-12 text-align-end">
                                                    <button id="btncheck" type="button" class="btn btn-primary antosubmit2 ">執行搜索</button>
                                                    <asp:Button runat="server" Text="提交" ID="Button_select" CssClass="btn btn-primary" Style="display: none" OnClick="Button_submit_Click" />
                                                </div>
                                            </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!--圖片及控制項-->
            <!--整理後表格-->
            <div role="tabpanel" class="tab-pane fade" id="tab_content2" aria-labelledby="profile-tab">
                <div id="order"></div>
            </div>
            <div role="tabpanel" class="tab-pane fade" id="tab_content3" aria-labelledby="profile-tab2">
                <div id="order_month"></div>
            </div>
            <div role="tabpanel" class="tab-pane fade" id="tab_content4" aria-labelledby="profile-tab3">
                <div id="order_month_mk"></div>
            </div>
            <!--整理後表格-->

        </div>
    </div>
    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        create_imgcode('order_image', 'export_image', 'chartContainer')
        //產生圖片
        set_stackedColumn('chartContainer', '<%=title%>', '<%=subtitle%>', '<%=xText %>', '<%=yText %>', '逾期訂單<%=chart_unit %>', [<%=chart_Overdue %>], '本月訂單<%=chart_unit %>', [<%=chartData %>], '<%=dropdownlist_X.ClientID%>', '<%=dropdownlist_y.ClientID%>');

        //產生表格的HTML碼
        create_tablecode('order', '訂單<%=yText %>統計', 'table-form', '<%=th.ToString() %>', '<%=tr.ToString() %>');
        //產生相對應的JScode
        set_Table('#table-form');
        //防止頁籤跑版
        loadpage('Order=Order_cust', '#table-form');

        //產生表格的HTML碼
        create_tablecode('order_month', '各月訂單統計', 'total-form', '<%=th_month.ToString() %>', '<%=tr_month.ToString() %>');
        //產生相對應的JScode
        set_Table('#total-form');

        //產生表格的HTML碼
        create_tablecode('order_month_mk', '各月訂單產能', 'MK-form', '<%=th_month_capacity.ToString() %>', '<%=tr_month_capacity.ToString() %>');
        //產生相對應的JScode
        set_Table('#MK-form');

        //$('#order_months').dataTable(
        //    {
        //        destroy: true,
        //        language: {
        //            "processing": "處理中...",
        //            "loadingRecords": "載入中...",
        //            "lengthMenu": "顯示 _MENU_ 項結果",
        //            "zeroRecords": "沒有符合的結果",
        //            "info": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
        //            "infoEmpty": "顯示第 0 至 0 項結果，共 0 項",
        //            "infoFiltered": "(從 _MAX_ 項結果中過濾)",
        //            "infoPostFix": "",
        //            "search": "搜尋:",
        //            "paginate": {
        //                "first": "第一頁",
        //                "previous": "上一頁",
        //                "next": "下一頁",
        //                "last": "最後一頁"
        //            }
        //        },
        //        "aLengthMenu": [10, 25, 50, 100],
        //        "order": [[1, "asc"]],
        //        scrollCollapse: true,
        //        dom: "<'row'<'pull-left'f>'row'<'col-sm-3'>'row'<'col-sm-3'B>'row'<'pull-right'l>>" +
        //            "<rt>" +
        //            "<'row'<'pull-left'i>'row'<'col-sm-4'>row'<'col-sm-3'>'row'<'pull-right'p>>",

        //        buttons: [
        //            {
        //                extend: 'copy', //className: 'copyButton',
        //                text: 'copy',
        //            },
        //            {
        //                extend: 'csv', //className: 'copyButton',
        //                text: 'csv',
        //            }
        //            , {
        //                extend: 'print', //className: 'copyButton',
        //                text: 'print',
        //            }
        //        ],
        //    });
        //防止頁籤跑版
        loadpage('', '');

        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=Button_select.ClientID %>').click();
        });
        //避免全選沒取消
        $(document).ready(function () {
            var checkBox = document.getElementById('<%=CheckBox_All.ClientID%>');
            var text = document.getElementById('<%=txt_showCount.ClientID%>');
            if (checkBox.checked == true) {
                text.disabled = true;
            } else {
                text.disabled = false;
            }


        });

        function set_nowmonth() {
            document.getElementById('<%=txt_str.ClientID%>').value = '<%=date_str%>';
            document.getElementById('<%=txt_end.ClientID%>').value = '<%=date_end%>';

        }

    </script>
</asp:Content>
