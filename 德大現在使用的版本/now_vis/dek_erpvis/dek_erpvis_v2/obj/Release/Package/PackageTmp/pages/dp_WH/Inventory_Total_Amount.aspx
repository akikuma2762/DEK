<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Inventory_Total_Amount.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_WH.Inventory_Total_Amount_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>庫存數量列表 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <%=path %>
        <br>
        <div class="clearfix"></div>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div id="Inventory_Total_Amount"></div>
                        <div class="col-md-3 col-sm-12 col-xs-12 padding-left-right-4">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                                        </asp:ScriptManager>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                            <ContentTemplate>

                                                <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                                    <div class="col-md-12 col-sm-12 col-xs-12  col-style">
                                                        <div class="col-md-4 col-sm-3 col-xs-4">
                                                            <span>廠區</span>
                                                        </div>
                                                        <div class="col-md-8 col-sm-9 col-xs-8">
                                                            <asp:DropDownList ID="dropdownlist_Factory" runat="server" AutoPostBack="true" CssClass="btn btn-default form-control" OnSelectedIndexChanged="dropdownlist_Factory_SelectedIndexChanged">
                                                                <asp:ListItem Value="sowon" Selected="True">立式廠</asp:ListItem>
                                                                <asp:ListItem Value="dek">大圓盤</asp:ListItem>
                                                                <asp:ListItem Value="iTec">臥式廠</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </asp:PlaceHolder>
                                                <div class="col-md-12 col-sm-12 col-xs-12 col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-12">
                                                        <span>類別選擇</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-12">
                                                        <asp:CheckBoxList ID="CheckBoxList_type" Font-Names="NotoSans" runat="server" CssClass="table-striped" onclick="all_check('ContentPlaceHolder1_CheckBoxList_type')">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-12 col-xs-12 col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-12">
                                                        <span>存放倉位</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-12">
                                                        <asp:CheckBoxList ID="CheckBoxList_spaces" Font-Names="NotoSans" runat="server" CssClass="table-striped" onclick="all_check('ContentPlaceHolder1_CheckBoxList_spaces')">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                        <div class="col-md-12 col-xs-12 text-align-end">
                                            <asp:Button ID="button_select" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="button_select_Click" Style="display: none" />
                                            <button id="btncheck" type="button" class="btn btn-primary antosubmit2">執行運算</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-----------------/content------------------>
    </div>
    <%= Use_Javascript.Quote_Javascript() %>
    <script>
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            var selectedValues = [];

            $("[id*=CheckBoxList_spaces] input:checked").each(function () {
                selectedValues.push($(this).val());
            });
            document.getElementById('<%=button_select.ClientID %>').click();
        });

        //產生表格的HTML碼
        create_tablehtmlcode('Inventory_Total_Amount', '庫存數量列表', 'table-form', '<%=th.ToString() %>', '<%=tr.ToString()%>');
        //產生相對應的JScode
        set_Table('#table-form');
        //防止頁籤跑版
        loadpage('', '');
    </script>
</asp:Content>
