<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Lost_Material.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PM.Lost_Material" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>欠料表 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>

    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_PM/Asm_history.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .row_update {
            margin-left: -3.8px;
        }
    </style>

    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁</a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PMD">生產部</a></u></li>
        </ol>
        <asp:CheckBoxList ID="CheckBoxList_Custom" RepeatColumns="10" runat="server"></asp:CheckBoxList>
        <div class="col-md-12 col-sm-6 col-xs-12" style="display: none">
            <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                <span>廠區</span>
            </div>
            <div class="col-md-8 col-sm-9 col-xs-8">
                <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%">
                    <asp:ListItem Value="sowon">立式廠</asp:ListItem>
                    <asp:ListItem Value="dek" Selected="True">大圓盤</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <div class="x_panel Div_Shadow">
            <div class="row">
                <div class="col-md-9 col-sm-12 col-xs-12">
                    <div class="x_content">
                        <%=all_div %>
                    </div>
                </div>
                <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="dashboard_graph x_panel">
                            <div class="x_content">
                                <div class="col-md-12 col-sm-6 col-xs-12">
                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                        <span>起始日期</span>
                                    </div>
                                    <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                        <asp:TextBox ID="txt_str" runat="server" Style="" TextMode="Date" CssClass="form-control   text-center"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-12 col-sm-6 col-xs-12">
                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                        <span>結束日期</span>
                                    </div>
                                    <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                        <asp:TextBox ID="txt_end" runat="server" CssClass="form-control  text-center" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
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
                        <br />

                    </div>
                </div>
            </div>
        </div>
    </div>

    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        //防止切換頁籤時跑版
        $(document).ready(function () {
            //需加，避免DIV跑版
            jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
        });

        //查詢事件
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=Button_select.ClientID %>').click();
        });

        //產生個別資料表時，需用到的JS
        <%=all_js %>


    </script>
</asp:Content>
