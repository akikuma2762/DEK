<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="recordsofchangetheorder.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_SD.recordsofchangetheorder_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>訂單變更紀錄 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" media="screen and (max-width:768px)" />
    <link href="../../Content/dp_SD/recordsofchangetheorder.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <%= path %>
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
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="col-md-9 col-sm-12 col-xs-12 _setborder">
                                <div id="recordsofchangetheorder_img"></div>
                            </div>
                                                    <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder" runat="server" id="div_present">
                            <div class="x_panel">
                                <table id="TB" class="table table-ts table-bordered nowrap" cellspacing="0" width="100%">
                                    <thead style="display:none">
                                        <tr id="tr_row">
                                            <th class="th_centet">使用率</th>
                                            <th class="th_centet">呆滯金額</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>本月訂單總變更數量 ：</b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <%=count_list[0] %>
                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>他月交期變更至本月 ：</b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <%=count_list[1] %>
                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>本月交期變更至他月 ：</b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <%=count_list[2] %>
                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>本月交期提前或落後 ：</b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <%=count_list[3] %>
                                                </b>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                            <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <div class="dashboard_graph x_panel">
                                        <div class="x_content">
                                            <asp:PlaceHolder ID="PlaceHolder_hide" runat="server" >
                                                <div class="col-md-12 col-sm-12 col-xs-12" style="display:none">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>廠區</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%">
                                                            <asp:ListItem Value="Eip" Selected="True">立式廠</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </asp:PlaceHolder>
                                            <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                <div class="col-md-4 col-sm-3 col-xs-4">
                                                    <span>顯示筆數</span>
                                                </div>
                                                 <div class="col-md-8 col-sm-9 col-xs-8 flex-align">
                                                <div class="col-md-5 col-sm-6 col-xs-5 padding-left-0">
                                                    <asp:TextBox ID="txt_showCount" runat="server" Text="10" CssClass="form-control text-center" TextMode="Number"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6 col-sm-6 col-xs-4">
                                                    <span class="flex-align">
                                                        <asp:CheckBox ID="CheckBox_All" runat="server" Text="全部" onclick="checkstatus('ContentPlaceHolder1_CheckBox_All','ContentPlaceHolder1_txt_showCount')" />
                                                    </span>
                                                </div>
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
                                                    <span>變更日期</span>
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-8">
                                                    <asp:TextBox ID="txt_str" runat="server" Style="" TextMode="Date" CssClass="form-control   text-center"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                <div class="col-md-4 col-sm-3 col-xs-4">
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-8">
                                                    <asp:TextBox ID="txt_end" runat="server" Style="" TextMode="Date" CssClass="form-control   text-center"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="col-md-12 col-xs-12 text-align-end">
                                                    <asp:Button ID="button_select" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="button_select_Click" Style="display: none" />
                                                    <button id="btncheck" type="button" class="btn btn-primary antosubmit2">執行搜索</button>
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
                <div id="recordsofchangetheorder"></div>
            </div>
        </div>
    </div>
    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        // 2019/06/11，資料送出前，進行日期格式驗證
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();
        });

        //產生表格的HTML碼
        create_tablecode('recordsofchangetheorder', '訂單變更紀錄列表', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString() %>');
        //產生相對應的JScode
        set_Table('#datatable-buttons');
        //防止頁籤跑版
        loadpage('recordsofchangetheorder=recordsofchangetheorder_cust', '#datatable-buttons');

        create_imgcode('recordsofchangetheorder_img', 'export_image', 'chartContainer')
        //產生圖片
        set_column('chartContainer', '<%=title%>', '<%=subtitle%>', '客戶', '數量', '變更次數', [<%=col_data_Points%>]);
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
