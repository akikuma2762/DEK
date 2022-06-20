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
                    <div class="row">
                        <div id="SupplierShortage"></div>
                        <div class="col-md-3 col-sm-12 col-xs-12">
                            <div class="col-md-12 col-sm-12 col-xs-12 _summount _setborder">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <h3 style="color: black">總未交量：</h3>
                                        <div class="h2 text-success count" style="color: darkred"><b><%=未交量總計 %> </b></div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12 col-sm-12 col-xs-12 _select _setborder">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                                        </asp:ScriptManager>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                            <ContentTemplate>
                                                <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                                        <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                            <span style="margin-left: -3px">廠區</span>
                                                        </div>
                                                        <div class="col-md-8 col-sm-9 col-xs-8">
                                                            <div class="row">
                                                                <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="form-control  text-center" Style="width: 90.2%; margin-left: 28px" OnSelectedIndexChanged="dropdownlist_Factory_SelectedIndexChanged" AutoPostBack="true">
                                                                    <asp:ListItem Value="sowon,Eip" Selected="True">立式廠</asp:ListItem>
                                                                    <asp:ListItem Value="dek,dek">大圓盤</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:PlaceHolder>
                                                <div class="col-12">
                                                    <div class="col-md-5 col-sm-10 col-xs-12" style="margin: 5px 0px 5px 0px">
                                                        <span>供應商代碼</span>
                                                    </div>
                                                    <div class="col-md-7 col-sm-9 col-xs-12">
                                                        <div class="row">
                                                            <fieldset>
                                                                <div class="control-group">
                                                                    <div class="controls">
                                                                        <div class="col-md-12 col-xs-12" style="margin: 0px 0px 5px 0px">
                                                                            <asp:TextBox ID="textbox_dt1" CssClass="form-control  text-center" Text="" Width="100%" runat="server" placeholder="請輸入供應商代碼"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </fieldset>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="col-md-5 col-sm-10 col-xs-12" style="margin: 5px 0px 5px 0px">
                                                        <span>供應商簡稱</span>
                                                    </div>
                                                    <div class="col-md-7 col-sm-9 col-xs-12">
                                                        <div class="row">
                                                            <fieldset>
                                                                <div class="control-group">
                                                                    <div class="controls">
                                                                        <div class="col-md-12 col-xs-12" style="margin: 0px 0px 5px 0px">
                                                                            <asp:TextBox ID="textbox_dt2" CssClass="form-control  text-center" Text="" Width="100%" runat="server" placeholder="請輸入供應商簡稱"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </fieldset>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="col-md-5 col-sm-10 col-xs-12" style="margin: 5px 0px 5px 0px">
                                                        <span>品號</span>
                                                    </div>
                                                    <div class="col-md-7 col-sm-9 col-xs-12">
                                                        <div class="row">
                                                            <fieldset>
                                                                <div class="control-group">
                                                                    <div class="controls">
                                                                        <div class="col-md-12 col-xs-12" style="margin: 0px 0px 5px 0px">
                                                                            <asp:TextBox ID="textbox_item" CssClass="form-control  text-center" Text="" Width="100%" runat="server" placeholder="請輸入品號關鍵字"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </fieldset>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="col-md-5 col-sm-10 col-xs-12" style="margin: 5px 0px 5px 0px">
                                                        <span>催料單號</span>
                                                    </div>
                                                    <div class="col-md-7 col-sm-9 col-xs-12">
                                                        <div class="row">
                                                            <fieldset>
                                                                <div class="control-group">
                                                                    <div class="controls">
                                                                        <div class="col-md-12 col-xs-12" style="margin: 0px 0px 5px 0px">
                                                                            <asp:TextBox ID="textbox_BillNo" CssClass="form-control  text-center" Text="" Width="100%" runat="server" placeholder="請輸入催料單號"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </fieldset>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-12">
                                                    <div class="col-md-5 col-sm-3 col-xs-12" style="margin: 5px 0px 5px 0px">
                                                        <span>催料預交日</span>
                                                    </div>
                                                    <div class="col-md-7 col-sm-9 col-xs-12">
                                                        <div class="row">
                                                            <fieldset>
                                                                <div class="control-group">
                                                                    <div class="controls">
                                                                        <div class="col-md-12 col-xs-12" style="margin: 0px 0px 5px 0px">
                                                                            <asp:TextBox ID="txt_str" runat="server" Style="" TextMode="Date" Width="100%" CssClass="form-control   text-center"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </fieldset>
                                                            <fieldset>
                                                                <div class="control-group">
                                                                    <div class="controls">
                                                                        <div class="col-md-12 col-xs-12">
                                                                            <asp:TextBox ID="txt_end" runat="server" Style="" Width="100%" TextMode="Date" CssClass="form-control   text-center"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </fieldset>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div></div>
                                            <div class="col-md-9 col-xs-8">
                                            </div>
                                            <div class="col-md-3 col-xs-12" style="margin: 10px 0px 0px 0px">
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
            create_code_noshdrow('SupplierShortage', '未交物料列表', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString() %>');
            //產生相對應的JScode
            set_Table('#datatable-buttons');
            //防止頁籤跑版
            loadpage('', '');


            $("#btncheck").click(function () {
                $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                document.getElementById('<%=button_select.ClientID %>').click();
            });
        </script>
    </div>
</asp:Content>
