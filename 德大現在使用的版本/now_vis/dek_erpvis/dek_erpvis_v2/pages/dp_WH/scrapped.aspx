<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="scrapped.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_WH.scrapped_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>報廢數量統計表 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_WH/scrapped.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <!-----------------title------------------>
        <%=path %>
        <div class="clearfix"></div>
        <br />
        <!-----------------/title------------------>
        <div class="clearfix"></div>
        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div id="scrapped"></div>
                        <div class="col-md-3 col-sm-12 col-xs-12 _select">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                                            <div class="h2 mb-0 text-success" style="margin-bottom: 10px;">
                                                <h3 class="_mdTitle" style="color: black">總報廢金額：</h3>
                                                <h4 class="_xsTitle" style="color: black">總報廢金額：</h4>
                                                <div class="h2 text-success count" style="color: darkred"><b><%=總報廢金額 %>元</b></div>
                                            </div>
                                        </asp:PlaceHolder>

                                        <h3 class="_mdTitle" style="color: black">最大金額品名規格：</h3>
                                        <h5 class="_xsTitle" style="color: black">最大金額品名規格：</h5>
                                        <div class="h4 text-success count" style="color: darkred"><b><%=最大金額品名規格 %></b></div>
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
                                                <asp:PlaceHolder ID="PlaceHolder2" runat="server">
                                                    <div class="col-md-12 col-sm-6 col-xs-12">
                                                        <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                            <span>廠區</span>
                                                        </div>
                                                        <div class="col-md-8 col-sm-9 col-xs-8">
                                                            <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="dropdownlist_Factory_SelectedIndexChanged">
                                                                <asp:ListItem Value="sowon" Selected="True">立式廠</asp:ListItem>
                                                                <asp:ListItem Value="dek">大圓盤</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </asp:PlaceHolder>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>日期快選</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <div class="btn-group btn-group-justified" style="margin: 0px 0px 5px 0px">
                                                            <a id="ContentPlaceHolder1_LinkButton_month" class="btn btn-default " onclick=" set_nowmonth()" style="text-align: center">當月</a>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>報廢日期</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                        <asp:TextBox ID="txt_str" runat="server" CssClass="form-control   text-center" TextMode="Date" OnTextChanged="txt_date_TextChanged" AutoPostBack="true"></asp:TextBox>

                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                        <asp:TextBox ID="txt_end" runat="server" CssClass="form-control  text-center" TextMode="Date" OnTextChanged="txt_date_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <asp:PlaceHolder ID="PlaceHolder_hide" runat="server" Visible="false">
                                                    <div class="col-md-12 col-sm-6 col-xs-12">
                                                        <div class="col-md-4 col-sm-3 col-xs-12">
                                                            <span>選取人員</span>
                                                        </div>
                                                        <div class="col-md-8 col-sm-9 col-xs-12">
                                                            <asp:CheckBoxList ID="CheckBoxList_staff" runat="server" CssClass="table-striped"></asp:CheckBoxList>
                                                        </div>
                                                    </div>
                                                </asp:PlaceHolder>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div class="col-md-9 col-xs-8">
                                            </div>
                                            <div class="col-md-3 col-xs-12">
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
            </div>
        </div>
        <!-----------------/content------------------>
    </div>

    <%=Use_Javascript.Quote_Javascript() %>
    <script>

        //產生表格的HTML碼
        create_tablehtmlcode('scrapped', '報廢數量統計表', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString()%>');
        //產生相對應的JScode
        set_Table('#datatable-buttons');
        //防止頁籤跑版
        loadpage('', '');


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
