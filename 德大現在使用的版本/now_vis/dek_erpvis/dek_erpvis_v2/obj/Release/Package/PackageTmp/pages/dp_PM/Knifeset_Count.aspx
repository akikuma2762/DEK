<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Knifeset_Count.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PM.Knifeset_Count" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>刀套數量統計表 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/Orders_Details.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main" style="height: 930px;">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁 </a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PMD">生產部</a></u></li>
        </ol>
        <br>
        <div class="clearfix"></div>


        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true">刀套總數</a>
            </li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" id="profile-tab" role="tab" data-toggle="tab" aria-expanded="false">每日刀套需求數量</a>
            </li>
        </ul>

        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">
                <div class="row row_update">
                    <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel Div_Shadow">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div id="Knifeset_Count"></div>
                                <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                                    <div class="x_panel">
                                        <div class="x_content">
                                             <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
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
                                            <div class="col-md-12 col-sm-6 col-xs-12">
                                                <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                    <span>起始日期</span>
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                    <asp:TextBox ID="txt_str" runat="server" Style="" TextMode="Date" CssClass="form-control   text-right"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-6 col-xs-12">
                                                <div class="col-md-4 col-sm-3 col-xs-4">
                                                      <span>結束日期</span>
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                    <asp:TextBox ID="txt_end" runat="server" CssClass="form-control  text-right" TextMode="Date"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="col-md-9 col-xs-8">
                                                </div>
                                                <div class="col-md-3 col-xs-4">
                                                    <asp:Button ID="button_select" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="Unnamed_ServerClick" Style="display: none" />
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
                <div id="Knifeset_Count_total"></div>
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
        create_code_noshdrow('Knifeset_Count', '刀套數量統計表', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString() %>');
        //產生相對應的JScode
        set_Table('#datatable-buttons');
        //防止頁籤跑版
        loadpage('', '');

        //產生表格的HTML碼
        create_tablecode('Knifeset_Count_total', '每日刀套數量統計表', 'total_count', '<%=th_date.ToString() %>', '<%=tr_date.ToString() %>');
        //產生相對應的JScode
        set_Table('#total_count');
        //防止頁籤跑版
        loadpage('', '');
    </script>
</asp:Content>
