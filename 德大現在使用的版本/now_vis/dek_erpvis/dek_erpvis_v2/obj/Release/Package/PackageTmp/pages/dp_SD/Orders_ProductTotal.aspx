<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Orders_ProductTotal.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_SD.Orders_ProductTotal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>產能統計 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
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
                        <div id="Orders_ProductTotal"></div>
                        <%=Energy.ToString() %>
                        <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="col-md-5 col-sm-3 col-xs-4">
                                                    <span>廠區</span>
                                                </div>
                                                <div class="col-md-7 col-sm-9 col-xs-8">
                                                    <div class="row">
                                                        <div class="col-md-12 col-sm-7 col-xs-6">
                                                            <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="btn btn-default dropdown-toggle" Width="98%">
                                                                <asp:ListItem Value="sowon" Selected="True">立式廠</asp:ListItem>
                                                                <asp:ListItem Value="dek">大圓盤</asp:ListItem>
                                                                <asp:ListItem Value="iTec">臥式廠</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="col-md-5 col-sm-3 col-xs-4">
                                                    <span>模式</span>
                                                </div>
                                                <div class="col-md-7 col-sm-9 col-xs-8">
                                                    <div class="row">
                                                        <div class="col-md-12 col-sm-7 col-xs-6">
                                                            <asp:DropDownList ID="dropdownlist_model" runat="server" CssClass="btn btn-default dropdown-toggle" Width="98%">
                                                                <asp:ListItem Value="day"  Selected="True">日產能</asp:ListItem>
                                                                <asp:ListItem Value="month">月產能</asp:ListItem>

                                                            </asp:DropDownList>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>
                                    </div>
                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                        <div class="col-md-5 col-sm-3 col-xs-4">
                                            <span>日期選擇</span>
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

                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                        <div class="col-md-5 col-sm-3 col-xs-4">
                                            <span></span>
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



                                    <div class="col-md-12 col-sm-12 col-xs-12">
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
        create_tablehtmlcode('Orders_ProductTotal', '<%=title %>', 'Orders_ProductTotals', '<%=th.ToString() %>', '<%=tr.ToString()%>');
        //產生相對應的JScode
        $('#Orders_ProductTotals').dataTable(
            {
                destroy: true,
                language: {
                    "processing": "處理中...",
                    "loadingRecords": "載入中...",
                    "lengthMenu": "顯示 _MENU_ 項結果",
                    "zeroRecords": "沒有符合的結果",
                    "info": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
                    "infoEmpty": "顯示第 0 至 0 項結果，共 0 項",
                    "infoFiltered": "(從 _MAX_ 項結果中過濾)",
                    "infoPostFix": "",
                    "search": "搜尋:",
                    "paginate": {
                        "first": "第一頁",
                        "previous": "上一頁",
                        "next": "下一頁",
                        "last": "最後一頁"
                    }
                },
                "aLengthMenu": [10, 25, 50, 100],
                "order": [[<%=order%>, "asc"]],
                scrollCollapse: true,
                dom: "<'row'<'pull-left'f>'row'<'col-sm-3'>'row'<'col-sm-3'B>'row'<'pull-right'l>>" +
                    "<rt>" +
                    "<'row'<'pull-left'i>'row'<'col-sm-4'>row'<'col-sm-3'>'row'<'pull-right'p>>",

                buttons: [
                    {
                        extend: 'copy', //className: 'copyButton',
                        text: 'copy',
                    },
                    {
                        extend: 'csv', //className: 'copyButton',
                        text: 'csv',
                    }
                    , {
                        extend: 'print', //className: 'copyButton',
                        text: 'print',
                    }
                ],
            });
        //防止頁籤跑版
        loadpage('Orders_DayTotal=Orders_DayTotal_cust', '#Orders_ProductTotals');



    </script>
</asp:Content>
