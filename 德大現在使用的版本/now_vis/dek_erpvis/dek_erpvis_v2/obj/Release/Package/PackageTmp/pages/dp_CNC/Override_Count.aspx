<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Override_Count.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_CNC.Override_Count" %>

<%--<%@ OutputCache duration="10" varybyparam="None" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=DropDownList_Type.SelectedItem.Text %>明細列表 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/UntradedCustomer.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        ._select {
            display: none;
        }

        .button_use {
            z-index: 999;
            margin: 0;
            top: 25%;
            right: 0%;
            -ms-transform: translateY(-30%);
            transform: translateY(-30%);
            width: 33px;
            height: 240px;
            line-height: 235px;
            font-family: verdana;
            text-align: center;
            background: #009efa;
            color: #fff;
            position: fixed;
            bottom: 20px;
            right: 0px;
            text-decoration: none;
            cursor: pointer;
        }

        div.scrollbar {
            width: 100%;
            height: 85px;
            white-space: nowrap;
            overflow-x: scroll;
            overflow-y: hidden;
        }

        .dataTables_scroll {
            overflow: auto;
        }

        @media screen and (max-width:768px) {
            .button_use {
                display: none;
            }

            ._select {
                display: block;
            }
            .drop{
                width:100%
            }
        }
    </style>
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class='breadcrumb_'>
            <li><u><a href='../index.aspx'>首頁 </a></u></li>
            <li><u><a href='../SYS_CONTROL/dp_fuclist.aspx?dp=CNC'>加工部</a></u></li>
        </ol>
        <br />
        <!-----------------/title------------------>
        <asp:TextBox ID="TextBox_MachTypeText" runat="server" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="TextBox_MachTypeValue" runat="server" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="TextBox_MachGroupText" runat="server" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="TextBox_MachGroupValue" runat="server" Style="display: none"></asp:TextBox>

        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div class="col-md-12 col-sm-12 col-xs-12" id="table_information">
                            <div class="x_panel">
                                <div class="x_title">
                                    <h1 class="text-center _mdTitle" style="width: 100%"><b>設備資訊明細列表</b></h1>
                                    <h3 class="text-center _xsTitle" style="width: 100%"><b>設備資訊明細列表</b></h3>
                                    <div class="clearfix"></div>
                                </div>

                                <table id="Override_Datatable" class="table table-ts table-bordered nowrap" cellspacing="0" width="100%">
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
                        <div id="_select" class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <i id="cbx_remind"></i>
                                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                                        </asp:ScriptManager>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                            <ContentTemplate>

                                                 <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                        <span>廠區名稱</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:DropDownList ID="DropDownList_factory" CssClass="btn btn-default dropdown-toggle" runat="server" Width="100%" onchange="dropdownlist_change('ContentPlaceHolder1_DropDownList_factory','ContentPlaceHolder1_DropDownList_Group')"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                        <span>群組名稱</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:DropDownList ID="DropDownList_Group" CssClass="btn btn-default dropdown-toggle" runat="server" Width="100%" onchange="show_machines('ContentPlaceHolder1_DropDownList_Group')"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div id="div_machines" class="col-md-12 col-sm-12 col-xs-12 flex-align col-style" style="display: none">
                                                    <asp:TextBox ID="TextBox_Machines" runat="server" Style="display: none"></asp:TextBox>
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>群組機台</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <label id="machines"></label>
                                                    </div>
                                                </div>

                                                 <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                        <span>類型選擇</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:DropDownList ID="DropDownList_Type"  CssClass="btn btn-default dropdown-toggle"  Width="100%" class="drop" runat="server">
                                                            <asp:ListItem Value="override">進給率</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>


                                                <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                        <span></span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8 flex-align">
                                                        <div class="col-md-5 col-sm-6 col-xs-5 padding-right-4">
                                                            <asp:DropDownList ID="DropDownList_Symbol" CssClass="btn btn-default dropdown-toggle" Width="100%" class="drop" runat="server">
                                                                <asp:ListItem Value=">">></asp:ListItem>
                                                                <asp:ListItem Value="<"><</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-6 col-sm-6 col-xs-4" style="width: 100%;">
                                                            <div class="flex-align">
                                                                <asp:TextBox ID="TextBox_Range" runat="server" Style="width: 100%" TextMode="Number" CssClass="form-control   text-center" class="drop">80</asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                        <span>日期選擇</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:TextBox ID="txt_date" runat="server" Style="" TextMode="Date" CssClass="form-control   text-center"></asp:TextBox>
                                                    </div>
                                                </div>


                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <br />
                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                        <div class="col-md-9 col-xs-8">
                                        </div>
                                        <div class="col-md-3 col-xs-12">
                                            <button id="btncheck" type="button" class="btn btn-primary antosubmit2 ">執行搜索</button>
                                            <asp:Button runat="server" Text="提交" ID="button_select" CssClass="btn btn-primary" Style="display: none" OnClick="button_select_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div id="div_button" class="button_use" onclick="hide_div()">
                    <i id="iconimage" class="fa fa-chevron-left"></i>
                </div>
            </div>
        </div>
    </div>
    <!-----------------/content------------------>
    <!-- set Modal -->

    <!--/set Modal-->
    <!-- Modal -->

    <!-- /Modal -->
    <!-- jQuery -->
    <script src="../../assets/vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="../../assets/vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- FastClick -->
    <script src="../../assets/vendors/fastclick/lib/fastclick.js"></script>
    <!-- NProgress -->
    <script src="../../assets/vendors/nprogress/nprogress.js"></script>
    <!-- bootstrap-progressbar -->
    <script src="../../assets/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js"></script>
    <!-- iCheck -->
    <script src="../../assets/vendors/iCheck/icheck.min.js"></script>
    <!-- bootstrap-daterangepicker -->
    <script src="../../assets/vendors/moment/min/moment.min.js"></script>
    <script src="../../assets/vendors/bootstrap-daterangepicker/daterangepicker.js"></script>
    <!-- bootstrap-wysiwyg -->
    <script src="../../assets/vendors/bootstrap-wysiwyg/js/bootstrap-wysiwyg.min.js"></script>
    <script src="../../assets/vendors/jquery.hotkeys/jquery.hotkeys.js"></script>
    <script src="../../assets/vendors/google-code-prettify/src/prettify.js"></script>
    <!-- jQuery Tags Input -->
    <script src="../../assets/vendors/jquery.tagsinput/src/jquery.tagsinput.js"></script>
    <!-- Switchery -->
    <script src="../../assets/vendors/switchery/dist/switchery.min.js"></script>
    <!-- Select2 -->
    <script src="../../assets/vendors/select2/dist/js/select2.full.min.js"></script>
    <!-- Autosize -->
    <script src="../../assets/vendors/autosize/dist/autosize.min.js"></script>
    <!-- jQuery autocomplete -->
    <script src="../../assets/vendors/devbridge-autocomplete/dist/jquery.autocomplete.min.js"></script>
    <!-- starrr -->
    <script src="../../assets/vendors/starrr/dist/starrr.js"></script>
    <!-- Custom Theme Scripts -->
    <script src="../../assets/build/js/custom.min.js"></script>
    <!-- FloatingActionButton -->
    <script src="../../assets/vendors/FloatingActionButton/js/index.js"></script>
    <!-- canvasjs -->
    <script src="../../assets/vendors/canvas_js/canvasjs.min.js"></script>
    <!-- Datatables -->
    <script src="../../assets/vendors/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="../../assets/vendors/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
    <script src="../../assets/vendors/datatables.net-buttons/js/dataTables.buttons.min.js"></script>
    <script src="../../assets/vendors/datatables.net-buttons-bs/js/buttons.bootstrap.min.js"></script>
    <script src="../../assets/vendors/datatables.net-buttons/js/buttons.flash.min.js"></script>
    <script src="../../assets/vendors/datatables.net-buttons/js/buttons.html5.min.js"></script>
    <script src="../../assets/vendors/datatables.net-buttons/js/buttons.print.min.js"></script>
    <script src="../../assets/vendors/datatables.net-responsive/js/dataTables.responsive.min.js"></script>
    <script src="../../assets/vendors/datatables.net-responsive-bs/js/responsive.bootstrap.js"></script>
    <script src="../../assets/vendors/datatables.net-scroller/js/dataTables.scroller.min.js"></script>
    <script src="../../assets/vendors/datatables.net-colReorder/dataTables.colReorder.min.js"></script>
    <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
    <script src="../../assets/vendors/time/loading.js"></script>
    <script>

        function hide_div() {
            var tablename = document.getElementById("table_information");
            var divname = document.getElementById("_select");
            var icon = document.getElementById("iconimage");


            if (tablename.className == 'col-md-9 col-sm-12 col-xs-12') {
                icon.className = 'fa fa-chevron-left';
                tablename.className = 'col-md-12 col-sm-12 col-xs-12';
                divname.style.display = 'none';

            }
            else {
                icon.className = "fa fa-chevron-right";
                tablename.className = 'col-md-9 col-sm-12 col-xs-12';
                divname.style.display = 'block';
            }

        }
        $(".iconimage").on("click", function () {
            hide_div();
        });




        //===========================================表格===========================================
        //設定datatable      
        $("#btncheck").click(function () {



            var type = document.getElementById('ContentPlaceHolder1_DropDownList_factory');
            var group = document.getElementById('ContentPlaceHolder1_DropDownList_Group');
            var grouptext = '';

            $('#ContentPlaceHolder1_TextBox_MachTypeText').val('' + type.options[type.selectedIndex].text + '');
            $('#ContentPlaceHolder1_TextBox_MachTypeValue').val('' + type.value + '');
            try {
                $('#ContentPlaceHolder1_TextBox_MachGroupText').val('' + group.options[group.selectedIndex].text + '');
                $('#ContentPlaceHolder1_TextBox_MachGroupValue').val('' + group.value + '');
                grouptext = group.options[group.selectedIndex].text;
            }
            catch {

            }
            if (grouptext != '' && grouptext != '--Select--') {
                $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                document.getElementById('<%=button_select.ClientID %>').click();
            }
            else
                alert('請選擇群組');

        });

        $('#Override_Datatable').dataTable(
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
                "order": [[<%=dt_count %>, "asc"]],




                scrollCollapse: true,
                dom: "<'row'<'pull-left'f>'row'<'col-sm-3'>'row'<'col-sm-3'B>'row'<'pull-right'l>>" +
                    "<rt>" +
                    "<'row'<'pull-left'i>'row'<'col-sm-4'>row'<'col-sm-3'>'row'<'pull-right'p>>",

                buttons: [
                    'copy', 'csv', 'excel', 'pdf', 'print'
                ],
            });

        jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');


        //依據X軸內容改變Y軸內容
        function dropdownlist_change(x, y) {
            $('#' + y).empty();


            var ddlist = document.getElementById(x);
            //取得當前X的值
            var now_value = ddlist.value;
            var nowlist = now_value.split(',');
            var ddl = document.getElementById(y);

            for (i = 0; i < nowlist.length - 1; i++) {
                var option = document.createElement("OPTION");
                option.innerHTML = nowlist[i];
                option.value = nowlist[i + 1];
                ddl.options.add(option);
                i++;
            }
            document.getElementById('div_machines').style.display = 'none';

        }

        //依據X軸內容改變Y軸內容
        function show_machines(y) {
            var machine_name = '';
            var ddlist = document.getElementById(y);
            //取得當前X的值
            var now_value = ddlist.value;
            var nowlist = now_value.split('^');
            var label = document.getElementById('machines');
            label.innerHTML = '';
            for (i = 1; i < nowlist.length - 1; i++) {
                if (nowlist[i] != '') {
                    label.innerHTML += nowlist[i] + '<br />';
                    machine_name += nowlist[i] + ',';
                }

            }
            $('#ContentPlaceHolder1_TextBox_Machines').val('' + machine_name + '');
            if (nowlist.length - 1 > 1)
                document.getElementById('div_machines').style.display = 'block';
        }

        window.onload = set_content();

        function set_content() {
            var machine_name = '';
            var mach = document.getElementById("ContentPlaceHolder1_TextBox_Machines").value;
            var label = document.getElementById('machines');
            label.innerHTML = '';
            if (mach != '') {
                var machines = mach.split(',');
                for (i = 0; i < machines.length - 1; i++) {
                    if (machines[i] != '') {
                        label.innerHTML += machines[i] + '<br />';
                        machine_name += machines[i] + ',';
                    }
                }
                $('#ContentPlaceHolder1_TextBox_Machines').val('' + machine_name + '');
                document.getElementById('div_machines').style.display = 'block';
            }
        }
        //20221111新增 選完群組改選回預設時,隱藏群組機台
        $("#ContentPlaceHolder1_DropDownList_Group").on('change', function () {
            var select_val = $("#ContentPlaceHolder1_DropDownList_Group").val();
            if (select_val == "1^")
            {
                document.getElementById('div_machines').style.display = 'none';
            }
        });
    </script>
</asp:Content>
