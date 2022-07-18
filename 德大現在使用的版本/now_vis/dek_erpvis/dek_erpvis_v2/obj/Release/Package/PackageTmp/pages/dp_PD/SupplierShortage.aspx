<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="SupplierShortage.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PD.SupplierShortage_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>供應商催料 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/dp_PD/SupplierShortage.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../assets/vendors/jquery/dist/jquery-ui.min.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <%= path %>
        <br>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div id="SupplierShortage"></div>
                        <div class="col-md-3 col-sm-12 col-xs-12 padding-left-right-4">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <h3 style="color: black">總未交量：</h3>
                                        <div class="h2 text-success count" style="color: darkred"><b><%=未交量總計 %> </b></div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12 col-sm-12 col-xs-12 ">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">

                                        <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                            <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                <div class="col-md-4 col-sm-3 col-xs-4">
                                                    <span>廠區</span>
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-8">                                                   
                                                        <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="btn btn-default form-control  text-center" OnSelectedIndexChanged="dropdownlist_Factory_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Value="sowon,Eip" Selected="True">立式廠</asp:ListItem>
                                                            <asp:ListItem Value="dek,dek">大圓盤</asp:ListItem>
                                                        </asp:DropDownList>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>供應商代碼</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:TextBox ID="textbox_dt1" CssClass="form-control  text-center border-radius-3" Text="" runat="server" placeholder="請輸入供應商代碼"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>供應商簡稱</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:TextBox ID="textbox_dt2" CssClass="form-control  text-center border-radius-3" Text="" runat="server" placeholder="請輸入供應商簡稱"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>品號</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:TextBox ID="textbox_item" CssClass="form-control  text-center border-radius-3" Text="" runat="server" placeholder="請輸入品號關鍵字"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>催料單號</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:TextBox ID="textbox_BillNo" CssClass="form-control  text-center border-radius-3" Text="" runat="server" placeholder="請輸入催料單號"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>催料預交日</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">

                                                <asp:TextBox ID="txt_str" runat="server" TextMode="Date" CssClass="form-control   text-center"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:TextBox ID="txt_end" runat="server" CssClass="form-control text-right" TextMode="Date"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-12 col-xs-12 text-align-end">
                                                <asp:Button ID="button_select" runat="server" Text="搜尋" class="btn btn-primary" OnClick="button_select_Click" Style="display: none" />
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
            //產生表格的HTML碼
            create_tablehtmlcode('SupplierShortage', '未交物料列表', 'table-form', '<%=th.ToString() %>', '<%=tr.ToString() %>');
            //產生相對應的JScode
            set_Table('#table-form');
            //防止頁籤跑版
            loadpage('', '');


            $("#btncheck").click(function () {
                $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                document.getElementById('<%=button_select.ClientID %>').click();
        });
        </script>
    </div>
</asp:Content>
