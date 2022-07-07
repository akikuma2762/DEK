<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="shipment.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_SD.shipment_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>出貨統計 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
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
        <%=path %>
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
                            <div id="shipment_image"></div>
                        </div>
                        <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <i id="cbx_remind"></i>
                                        <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                            <div class="col-md-12 col-sm-6 col-xs-12 flex-align" style="margin: 0px 0px 5px 0px">
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
                                        <div class="col-md-12 col-sm-6 col-xs-12 flex-align" style="margin: 0px 0px 5px 0px">
                                            <div class="col-md-4 col-sm-3 col-xs-4" >
                                                <span>X座標(值)</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:DropDownList ID="dropdownlist_X" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%">
                                                    <asp:ListItem Value="PLINE_NO" Selected="True">產線</asp:ListItem>
                                                    <asp:ListItem Value="CUST_NO">客戶</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-6 col-xs-12 flex-align" style="margin: 0px 0px 5px 0px">
                                            <div class="col-md-4 col-sm-12 col-xs-4">
                                                <span>顯示筆數</span>
                                            </div>
                                            <div class="col-md-5 col-sm-12 col-xs-5">
                                                <asp:TextBox ID="txt_showCount" runat="server" Text="10" CssClass="form-control text-center" TextMode="Number"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 col-sm-12 col-xs-3">
                                                <span class="flex-align">
                                                    <asp:CheckBox ID="CheckBox_All" runat="server" Text="全部" onclick="checkstatus('ContentPlaceHolder1_CheckBox_All','ContentPlaceHolder1_txt_showCount')" />
                                                </span>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-6 col-xs-12 flex-align" style="margin: 0px 0px 5px 0px">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>日期快選</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <div class="btn-group btn-group-justified" style="margin: 0px 0px 5px 0px">
                                                    <a id="ContentPlaceHolder1_LinkButton_month" class="btn btn-default " onclick=" set_nowmonth()" style="text-align: center">當月</a>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-6 col-xs-12 flex-align" style="margin: 0px 0px 5px 0px">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>起始日期</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:TextBox ID="txt_str" runat="server" Style="" TextMode="Date" CssClass="form-control   text-right"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-6 col-xs-12 flex-align">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>結束日期</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:TextBox ID="txt_end" runat="server" CssClass="form-control  text-right" TextMode="Date"></asp:TextBox>
                                            </div>
                                        </div>


                                    </div>
                                </div>
                                <br />
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
            <div role="tabpanel" class="tab-pane fade" id="tab_content2" aria-labelledby="profile-tab">
                <div id="shipment"></div>
            </div>
        </div>
    </div>
    <%=Use_Javascript.Quote_Javascript()%>
    <script>

        create_imgcode('shipment_image', 'export_image', 'chartContainer');
        //產生圖片
        set_column('chartContainer', '<%=title%>', '<%=subtitle%>', '<%=xText %>', '數量', '已出貨', [<%=col_data_Points%>]);

        //產生表格的HTML碼
        create_tablecode('shipment', '出貨統計列表', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString() %>');
        //產生相對應的JScode
        set_Table('#datatable-buttons');
        //防止頁籤跑版
        loadpage('shipment=shipment_cust', '#datatable-buttons');

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
