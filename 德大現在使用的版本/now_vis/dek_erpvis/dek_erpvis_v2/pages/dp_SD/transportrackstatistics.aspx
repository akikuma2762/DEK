<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="transportrackstatistics.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_SD.transportrackstatistics_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>運輸架未歸還統計 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" media="screen and (max-width:768px)" />
    <link href="../../Content/dp_SD/transportrackstatistics.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <%=path %>
        <br>
        <div class="clearfix"></div>
        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true">圖片模式</a>
            </li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" id="profile-tab" role="tab" data-toggle="tab" aria-expanded="false">表格模式</a>
            </li>
        </ul>
        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">
                        <div class="col-md-9 col-sm-12 col-xs-12 _setborder">
                            <div id="transportrackstatistics_img"></div>
                        </div>
                        <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                            <div class="col-md-12 col-sm-6 col-xs-12 flex-align col-style">
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
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div class="col-md-9 col-xs-8">
                                            </div>
                                            <div class="col-md-3 col-xs-12">
                                                <button id="btncheck" type="button" class="btn btn-primary antosubmit2 ">執行搜索</button>
                                                <asp:Button runat="server" Text="提交" ID="Button_select" CssClass="btn btn-primary" Style="display: none" OnClick="button_select_Click" />
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
                <div id="transportrackstatistics"></div>
            </div>
        </div>
        <!-----------------/content------------------>
    </div>
    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        create_imgcode('transportrackstatistics_img', 'export_image', 'chartContainer')
        //產生圖片
        set_stackedColumn('chartContainer', '<%=title%>', '', '運輸架名稱', '數量', '異常數量', [<%=col_data_points_nor %>], '未歸還數量', [<%=col_data_points_sply %>], '', '');

        //產生表格的HTML碼
        create_tablecode('transportrackstatistics', '運輸架未歸還列表', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString() %>');
        //產生相對應的JScode
        set_Table('#datatable-buttons');
        //防止頁籤跑版
        loadpage('', '');

        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=Button_select.ClientID %>').click();
        });
    </script>
</asp:Content>
