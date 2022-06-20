<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Asm_ELine_details.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PM.Asm_ELine_details" %>

<%--<%@ OutputCache duration="10" varybyparam="None" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>電控專線 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>

    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/shipment.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .dataTables_scroll {
            overflow: auto;
        }
    </style>
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁</a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PMD">生產部</a></u></li>
        </ol>
        <br>
        <div class="clearfix"></div>
        <!-----------------/title------------------>
        <div class="page-title">
            <div class="row">
            </div>
        </div>
        <div class="clearfix"></div>
        <!-----------------content------------------>
        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="<%=html_code[0] %>" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="<%=html_code[1] %>">已完工列表</a>
            </li>
            <li role="presentation" class="<%=html_code[3] %>" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" id="profile-tab" role="tab" data-toggle="tab" aria-expanded="<%=html_code[4] %>">績效表</a>
            </li>
            <%--尚未完成 先隱藏--%>
            <%--            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content3" id="home2-tab" role="tab" data-toggle="tab" aria-expanded="false">計算表</a>
            </li>--%>
        </ul>
        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade <%=html_code[2] %>" id="tab_content1" aria-labelledby="home-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="x_content">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h1 class="text-center _mdTitle" style="width: 100%"><b>已完工列表</b></h1>
                                        <h3 class="text-center _xsTitle" style="width: 100%"><b>已完工列表</b></h3>
                                        <div class="clearfix"></div>
                                    </div>
                                    <p class="text-muted font-13 m-b-30">
                                    </p>
                                    <table id="TB" class="table  table-ts table-bordered nowrap" cellspacing="0" width="100%">
                                        <thead>
                                            <tr id="tr_row">
                                                <%=Finish_th%>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <%= Finish_tr %>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div role="tabpanel" class="tab-pane fade <%=html_code[5] %>" id="tab_content2" aria-labelledby="profile-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">

                        <div class="col-md-9 col-sm-12 col-xs-12">
                            <div class="x_content">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h1 class="text-center _mdTitle" style="width: 100%"><b>績效列表</b></h1>
                                        <h3 class="text-center _xsTitle" style="width: 100%"><b>績效列表</b></h3>
                                        <div class="clearfix"></div>
                                    </div>
                                    <p class="text-muted font-13 m-b-30">
                                    </p>
                                    <table id="datatable-buttons" class="table  table-ts table-bordered nowrap" cellspacing="0" width="100%">
                                        <thead>
                                            <tr id="tr_row">
                                                <%=th%>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <%= tr %>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="x_panel">
                                <div class="x_content">
                                    <div class="col-md-12 col-sm-6 col-xs-12">
                                        <div class="col-md-4 col-sm-3 col-xs-5" style="margin: 5px 0px 5px 0px">
                                            <span>日期選擇</span>
                                        </div>
                                        <div class="col-md-8 col-sm-9 col-xs-7" style="margin: 0px 0px 5px 0px">
                                            <asp:TextBox ID="textbox_dt1" runat="server" Style="" TextMode="Date" CssClass="form-control   text-center"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-12 col-sm-6 col-xs-12">
                                        <div class="col-md-4 col-sm-3 col-xs-5">
                                        </div>
                                        <div class="col-md-8 col-sm-9 col-xs-7" style="margin: 0px 0px 5px 0px">
                                            <asp:TextBox ID="textbox_dt2" runat="server" CssClass="form-control  text-center" TextMode="Date"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-12 col-sm-6 col-xs-12">
                                        <div class="col-md-4 col-sm-3 col-xs-5">
                                            <span>選擇人員</span>
                                        </div>
                                        <div class="col-md-8 col-sm-9 col-xs-7" style="margin: 0px 0px 5px 0px">
                                            <asp:RadioButtonList ID="RadioButtonList_Finish" runat="server"></asp:RadioButtonList>
                                        </div>
                                    </div>

                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                        <div class="col-md-9 col-xs-8">
                                        </div>
                                        <div class="col-md-3 col-xs-12">
                                            <asp:Button ID="button_search" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="button_search_Click" Style="display: none" />
                                            <button id="btnsearch" type="button" class="btn btn-primary antosubmit2">執行搜索</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>


            <div role="tabpanel" class="tab-pane fade " id="tab_content3" aria-labelledby="home2-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="x_content">
                                <div class="x_panel">

                                    <div class="x_title">
                                        <h1 class="text-center _mdTitle" style="width: 100%"><b>電控組裝點數表</b></h1>
                                        <h3 class="text-center _xsTitle" style="width: 100%"><b>電控組裝點數表</b></h3>
                                        <div class="clearfix"></div>
                                    </div>
                                    <p class="text-muted font-13 m-b-30">
                                    </p>

                                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                                    </asp:ScriptManager>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                        <ContentTemplate>
                                            <asp:PlaceHolder ID="PlaceHolder_Performance" runat="server"></asp:PlaceHolder>
                                            <asp:Button ID="button_btncalculation" runat="server" Text="計算" class="btn btn-secondary" OnClick="button_btncalculation_Click" Style="display: none" />
                                            <asp:TextBox ID="TextBox_Result" runat="server" Style="display: none"></asp:TextBox>
                                            <asp:PlaceHolder ID="PlaceHolder_hide" runat="server" Visible="false">
                                                <table id="Point_Info" class="table  table-ts table-bordered nowrap" cellspacing="0" width="100%">
                                                    <thead>
                                                        <tr id="tr_row">
                                                            <%=point_th%>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <%= point_tr %>
                                                    </tbody>
                                                </table>
                                            </asp:PlaceHolder>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>


        </div>
    </div>

    <!-----------------/content------------------>

    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        $(document).ready(function () {
            //完工的表格
            $('#Finish_Info').DataTable({
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
                scrollX: true,
                scrollY: '50vh',
                scrollCollapse: true,
                dom: "<'row'<'pull-left'f>'row'<'col-sm-3'>'row'<'col-sm-3'B>'row'<'pull-right'l>>" +
                    "<rt>" +
                    "<'row'<'pull-left'i>'row'<'col-sm-4'>row'<'col-sm-3'>'row'<'pull-right'p>>",

                buttons: [
                    'copy', 'csv', 'excel', 'pdf', 'print'
                ],

            });
            jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');





        });

        $(document).ready(function () {

            $('#example').DataTable({
                responsive: true
            });
            $('#exampleInTab').DataTable({
                responsive: true
            });

            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                $($.fn.dataTable.tables(true)).DataTable()
                    .columns.adjust()
                    .responsive.recalc();
            });
        });

        $("#btnsearch").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_search.ClientID %>').click();
        });


        $("#btncalculation").click(function () {
            var list = '';
            <%=js_code%>
            $('#<%=TextBox_Result.ClientID%>').val('' + list + '');
            document.getElementById('<%=button_btncalculation.ClientID %>').click();
        });






    </script>
</asp:Content>
