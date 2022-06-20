<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Worktime_Total.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_CNC.Worktime_Total" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>人員進出站統計 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/Orders_Details.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        ._select {
            display: none;
        }

        .button_use {
            z-index: 999;
            margin: 0;
            top: 40%;
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

            .exportChart {
                display: none;
            }

            .exportimage {
                display: none;
            }
        }

        .copyButton {
            box-shadow: 9px 9px 15px gray;
            width: 87px;
            text-align: center;
        }
    </style>
    <div class="right_col" role="main" style="height: 930px;">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁 </a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=CNC">加工部</a></u></li>
            <li>人員進出站統計</li>
        </ol>
        <br>
        <div class="clearfix"></div>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div class="row">
                            <div id="work_total"></div>

                            <div id="_select" class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div class="col-md-12 col-sm-12 col-xs-12">

                                                <asp:TextBox ID="TextBox_MachTypeText" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:TextBox ID="TextBox_MachTypeValue" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:TextBox ID="TextBox_MachGroupText" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:TextBox ID="TextBox_MachGroupValue" runat="server" Style="display: none"></asp:TextBox>
                                                <div class="col-md-4 col-sm-3 col-xs-5" style="margin: 5px 0px 5px 0px">
                                                    廠區名稱
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-7">
                                                    <div class="row">
                                                        <fieldset>
                                                            <div class="control-group">
                                                                <div class="controls">
                                                                    <div class="col-md-12 col-xs-12" style="margin: 0px 0px 5px 0px">
                                                                        <asp:DropDownList ID="DropDownList_MachType" CssClass="btn btn-default dropdown-toggle" runat="server" Width="100%" onchange="dropdownlist_change('ContentPlaceHolder1_DropDownList_MachType','ContentPlaceHolder1_DropDownList_MachGroup')"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                    </div>
                                                </div>

                                                <div class="col-md-4 col-sm-3 col-xs-5" style="margin: 5px 0px 5px 0px">
                                                    群組名稱
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-7">
                                                    <div class="row">
                                                        <fieldset>
                                                            <div class="control-group">
                                                                <div class="controls">
                                                                    <div class="col-md-12 col-xs-12" style="margin: 0px 0px 5px 0px">
                                                                        <asp:DropDownList ID="DropDownList_MachGroup" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" onchange="show_machines('ContentPlaceHolder1_DropDownList_MachGroup')"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                    </div>
                                                </div>
                                                <div id="div_machines" class="col-md-12 col-sm-6 col-xs-12" style="display: none">
                                                    <asp:TextBox ID="TextBox_Machines" runat="server" Style="display: none"></asp:TextBox>
                                                    <div class="col-md-4 col-sm-12 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>群組機台</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-12 col-xs-8">
                                                        <label id="machines"></label>
                                                    </div>
                                                </div>
                                                <div class="col-md-4 col-sm-3 col-xs-5" style="margin: 5px 0px 5px 0px">
                                                    開始時間
                                                </div>
                                                <div class="col-md-8 col-sm-9 col-xs-7">
                                                    <div class="row">
                                                        <fieldset>
                                                            <div class="control-group">
                                                                <div class="controls">
                                                                    <div class="col-md-12 col-xs-12" style="margin: 0px 0px 5px 0px">
                                                                        <asp:TextBox ID="TextBox_Start" runat="server" TextMode="Date" CssClass="form-control   text-left"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                        <fieldset>
                                                            <div class="control-group">
                                                                <div class="controls">
                                                                    <div class="col-md-12 col-xs-12" style="margin: 0px 0px 5px 0px">
                                                                        <asp:TextBox ID="TextBox_End" runat="server" TextMode="Date" CssClass="form-control   text-left"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                    </div>
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
                        <div id="div_button" class="button_use" onclick="hide_div()">
                            <i id="iconimage" class="fa fa-chevron-left"></i>
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
        create_tablecode_noshdrow('work_total', '人員進出站統計', 'Override_Datatable', '<%=th%>', '<%=tr%>');
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
                "order": [[0, "asc"] <%=order%>],
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
                        text: '匯出csv',

                    }
                    , {
                        extend: 'print', //className: 'copyButton',
                        text: 'print',

                    }
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

        //防止頁籤跑版
        loadpage('', '');
        $("#btncheck").click(function () {
            var type = document.getElementById('ContentPlaceHolder1_DropDownList_MachType');
            var group = document.getElementById('ContentPlaceHolder1_DropDownList_MachGroup');
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
    </script>
</asp:Content>
