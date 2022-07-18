<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="stockanalysis.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_WH.stockanalysis_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>成品庫存分析 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_WH/stockanalysis.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        #column_image {
            height: 400px;
            max-width: 920px;
            margin: 0px auto;
        }

        @media screen and (max-width:768px) {
            #column_image {
                height: 400px;
                width: 100%;
                margin: 0px auto 30px;
            }
        }
    </style>

    <div class="right_col" role="main">
        <!-----------------title------------------>
        <%=path %>
        <br>
        <div class="clearfix"></div>
        <!-----------------/title------------------>
        <div class="clearfix"></div>
        <!-----------------content------------------>
        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true">圖片模式</a>
            </li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" id="profile-tab" role="tab" data-toggle="tab" aria-expanded="false">表格模式</a>
            </li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content3" id="profile-tabmoney" role="tab" data-toggle="tab" aria-expanded="false">庫存明細</a>
            </li>
        </ul>
        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">
                <div class="x_panel Div_Shadow">
                    <div class="">

                        <div class="col-md-9 col-sm-12 col-xs-12 padding-left-right-4">
                            <div class="col-md-6 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <div style="text-align: right; width: 100%; padding: 0;">
                                            <button style="display: none" type="button" id="exportChart" title="另存成圖片">
                                                <img src="../../assets/images/download.jpg" style="width: 36.39px; height: 36.39px;">
                                            </button>
                                            <div class="row">
                                                <div class="col-md-10 col-sm-10 col-xs-12">
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

                            <div id="img_column"></div>

                        </div>
                        <div class="col-md-3 col-sm-12 col-xs-12 padding-left-right-4">
                            <div class="dashboard_graph x_panel">
                                <div class="x_content">
                                    <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>廠區</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%">
                                                    <asp:ListItem Value="sowon" Selected="True">立式廠</asp:ListItem>
                                                    <asp:ListItem Value="dek">大圓盤</asp:ListItem>
                                                    <asp:ListItem Value="iTec">臥式廠</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </asp:PlaceHolder>
                                    <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                        <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                            <span>庫存天數</span>
                                        </div>
                                        <div class="col-md-8 col-sm-9 col-xs-8 flex-align">
                                            <div class="col-md-5 col-sm-6 col-xs-5">
                                                <asp:TextBox ID="txt_showCount" runat="server" Text="30" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-4 padding-left-right-4">
                                                <div class="flex-align">
                                                    <asp:CheckBox ID="CheckBox_All" runat="server" Text="全部" onclick="checkstatus('ContentPlaceHolder1_CheckBox_All','ContentPlaceHolder1_txt_showCount')" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                   <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="col-md-12 col-xs-12 text-align-end">
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
                <div id="stockanalysis"></div>
            </div>

            <div role="tabpanel" class="tab-pane fade" id="tab_content3" aria-labelledby="profile-tab">
                <div id="money"></div>
            </div>
        </div>
        <!-----------------/content------------------>
    </div>
    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();
        });

        //產生表格的HTML碼
        create_tablecode('stockanalysis', '逾期數量列表', 'table-form', '<%=th.ToString() %>', '<%=tr.ToString() %>');
        //產生相對應的JScode
        set_Table('#table-form');
        //防止頁籤跑版
        loadpage('stockanalysis=stockanalysis_cust', '#table-form');

        //產生表格的HTML碼
        create_tablecode('money', '庫存金額列表', 'total-form', '<%=th_money.ToString() %>', '<%=tr_money.ToString() %>');
        //產生相對應的JScode
        set_Table('#total-form');
        //防止頁籤跑版
        loadpage('', '');

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


        create_imghtmlcode('img_column', 'exports_image', 'column_image', '6', '');
        set_stackedColumn('column_image', '各產線庫存分析', '<%=subtitle%>', '產線', '數量', '逾期庫存', [<%=col_data_points_nor %>], '一般庫存', [<%=col_data_points_sply %>], '', '');


        //產生圖片
        set_pie('chartpie', '總體庫存分析', '<%=subtitle%>', [<%=pie_data_points%>]);

    </script>
</asp:Content>
