<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="UntradedCustomer.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_SD.UntradedCustomer_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>未交易客戶 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/UntradedCustomer.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <!-----------------title------------------>
        <%= path %>
        <br />
        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div id="UntradedCustomer"></div>
                        <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                            <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                <div class="col-md-5 col-sm-3 col-xs-4">
                                                    <span>廠區</span>
                                                </div>
                                                <div class="col-md-7 col-sm-9 col-xs-8">
                                                    <div class="row">
                                                        <div class="col-md-12 col-sm-7 col-xs-6">
                                                            <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="btn btn-default dropdown-toggle" Width="98%">
                                                                <asp:ListItem Value="sowon" Selected="True">立式廠</asp:ListItem>
                                                                <asp:ListItem Value="dek">大圓盤</asp:ListItem>
                                                           <%--      <asp:ListItem Value="iTec">臥式廠</asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-5 col-sm-3 col-xs-4">
                                                <span>未交易天數</span>
                                            </div>
                                            <div class="col-md-7 col-sm-9 col-xs-8">
                                                <div class="row">
                                                    <div class="col-md-7 col-sm-7 col-xs-6">
                                                        <asp:DropDownList ID="DropDownList_selectedcondi" runat="server" class="form-control text-center">
                                                            <asp:ListItem Value="">---請選擇---</asp:ListItem>
                                                            <asp:ListItem Value=">" Selected="True">大於</asp:ListItem>
                                                            <asp:ListItem Value="<">小於</asp:ListItem>
                                                            <asp:ListItem Value="=">等於</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-5 col-sm-5 col-xs-6">
                                                        <asp:TextBox ID="TextBox_dayval" runat="server" TextMode="Number" class="form-control text-center" Text="365" Width="95%" placeholder="365"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                        <div class="col-md-5 col-sm-3 col-xs-4">
                                            <span>起始日期</span>
                                        </div>
                                        <div class="col-md-7 col-sm-9 col-xs-8">
                                            <div class="row">
                                                <fieldset>
                                                    <div class="control-group">
                                                        <div class="controls">
                                                            <div class="col-md-12  col-xs-12">
                                                                <asp:TextBox ID="txt_str" runat="server" Style="" TextMode="Date" Width="96%" CssClass="form-control  text-center"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </fieldset>

                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                        <div class="col-md-5 col-sm-3 col-xs-4">
                                            <span>結束日期</span>
                                        </div>
                                        <div class="col-md-7 col-sm-9 col-xs-8">
                                            <div class="row">

                                                <fieldset>
                                                    <div class="control-group">
                                                        <div class="controls">
                                                            <div class="col-md-12  col-xs-12" style="margin: 5px 0;">
                                                                <asp:TextBox ID="txt_end" runat="server" Style="" Width="96%" TextMode="Date" CssClass="form-control  text-center"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>
                                        </div>
                                    </div>



                                    <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                        <div class="col-md-9 col-xs-8">
                                        </div>
                                        <div class="col-md-3 col-xs-12">
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

    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();
        });
        //產生表格的HTML碼
        create_tablehtmlcode('UntradedCustomer', '未交易客戶列表', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString()%>');
        //產生相對應的JScode
        set_Table('#datatable-buttons');
        //防止頁籤跑版
        loadpage('', '');
    </script>
</asp:Content>
