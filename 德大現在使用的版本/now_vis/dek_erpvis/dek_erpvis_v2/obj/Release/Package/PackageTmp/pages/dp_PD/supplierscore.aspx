<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="supplierscore.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PD.supplierscore_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>供應商達交率 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/dp_PD/supplierscore.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <%= path %><br>
        <div class="page-title">
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div id="supplierscore"></div>
                        <div class="col-md-3 col-sm-12 col-xs-12 padding-left-right-4">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                            <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                <div class="col-md-4 col-sm-3 col-xs-4">
                                                    <span>廠區</span>
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-8">
                                                    <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="btn btn-default form-control  text-center">
                                                        <asp:ListItem Value="sowon" Selected="True">立式廠</asp:ListItem>
                                                        <asp:ListItem Value="dek">大圓盤</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>

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
                                                交貨日期
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
                                            <div class="col-md-9 col-xs-8">
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
        </div>
    </div>
    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();
        });

        //產生表格的HTML碼
        create_tablehtmlcode('supplierscore', '供應商達交率', 'table-form', '<%=th.ToString() %>', '<%=tr.ToString() %>');
        //產生相對應的JScode
        set_Table('#table-form');
        //防止頁籤跑版
        loadpage('supplierscore=supplierscore_cust', '#datatable-buttons');
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();
        });

        function set_nowmonth() {
            document.getElementById('<%=txt_str.ClientID%>').value = '<%=date_str%>';
            document.getElementById('<%=txt_end.ClientID%>').value = '<%=date_end%>';
        }
    </script>
</asp:Content>
