<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Machine_list_info_APS.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_CNC.Aps_Project" MaintainScrollPositionOnPostback="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>機台總覽 | 緯凡金屬</title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <link href="../../assets/build/css/custom_old.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button_old.css" rel="stylesheet" />
    <link rel="stylesheet" href="../../gantt/css/style.css" />
    <link rel="stylesheet" href="../../gantt/css/prettify.min.css" />
    <style>
        .x_panel {
            padding: 14px 14px;
        }

        .tooltip-inner {
            max-width: 700px;
            width: 700px;
            top: 40px;
            font-size: 17px;
            left: 0px;
            display: block;
            text-align: left;
            border: 1px solid black;
            padding: 5px;
        }

        input[type="checkbox"] {
            width: 18px;
            height: 18px;
            cursor: auto;
            -webkit-appearance: default-button;
        }

        .col-md-55,
        .col-xs-1,
        .col-sm-1,
        .col-md-1,
        .col-lg-1,
        .col-xs-2,
        .col-sm-2,
        .col-md-2,
        .col-lg-2,
        .col-xs-3,
        .col-sm-3,
        .col-md-3,
        .col-lg-3,
        .col-xs-4,
        .col-sm-4,
        .col-md-4,
        .col-lg-4,
        .col-xs-5,
        .col-sm-5,
        .col-md-5,
        .col-lg-5,
        .col-xs-6,
        .col-sm-6,
        .col-md-6,
        .col-lg-6,
        .col-xs-7,
        .col-sm-7,
        .col-md-7,
        .col-lg-7,
        .col-xs-8,
        .col-sm-8,
        .col-md-8,
        .col-lg-8,
        .col-xs-9,
        .col-sm-9,
        .col-md-9,
        .col-lg-9,
        .col-xs-10,
        .col-sm-10,
        .col-md-10,
        .col-lg-10,
        .col-xs-11,
        .col-sm-11,
        .col-md-11,
        .col-lg-11,
        .col-xs-12,
        .col-sm-12,
        .col-md-12,
        .col-lg-12 {
            padding-right: 0px;
            padding-left: 0px;
        }

        @media screen and (min-width: 765px) {


            .modal-dialog {
                width: 1700px
            }
        }

        #TC_length {
            display: none;
        }

        #TC_filter {
            display: none;
        }

        #TC_info {
            display: none;
        }

        #TC_paginate {
            display: none;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <ol class='breadcrumb_'>
            <li><u><a href='../index.aspx'>加工部 </a></u></li>
            <li>設備監控看板</li>
        </ol>
        <br>
        <div class="page-title">
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="col-md-6 col-sm-6 col-xs-6">
                        <h4>[搜尋條件]資訊篩選：<u><asp:LinkButton ID="LinkButton1" runat="server" data-toggle="modal" data-target="#exampleModal_time"> 選擇資訊顯示 </asp:LinkButton></u></h4>
                    </div>

                </div>
            </div>
        </div>
        <div class="clearfix"></div>


        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true">圖塊模式</a>
            </li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" role="tab" id="profile-tab" data-toggle="tab" aria-expanded="false">列表模式</a>
            </li>
            <%=show %>
            <div style="text-align: right">
                <%--<button id="test" class="test" onclick="openFullscreen()">Open Video in Fullscreen Mode</button>--%>
                <img src="../../assets/images/MOLDING.PNG" alt="..." width="30px" height="30px">&nbsp 段取中 &nbsp
                <img src="../../assets/images/MOLDED.PNG" alt="..." width="30px" height="30px">&nbsp 段取完成 &nbsp
                <img src="../../assets/images/RUN.PNG" alt="..." width="30px" height="30px">&nbsp 加工中 &nbsp
                <img src="../../assets/images/FINISH.PNG" alt="..." width="30px" height="30px">&nbsp 加工完成 &nbsp
                <img src="../../assets/images/ERROR.PNG" alt="..." width="30px" height="30px">&nbsp 異常  &nbsp
                <img src="../../assets/images/READY.PNG" alt="..." width="30px" height="30px">&nbsp   閒置 &nbsp
            </div>

        </ul>

        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade" id="tab_content2" aria-labelledby="profile-tab">
                <div class="x_panel" id="Div_Shadow">
                    <div class="x_content">

                        <table id="datatable_Info" class="table table-striped table-bordered" style="margin: 0px 0px -10px 0px;">

                            <thead>
                                <%= th %>
                            </thead>
                            <tbody>
                                <%= tr %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <%--以上列表模式--%>
            <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">

                <div class="x_panel" id="Div_Shadow">

                    <div class="x_content">
                        <div class="row">
                            <%=area %>
                        </div>
                    </div>
                </div>
            </div>
            <%--以上甘特模式--%>
            <div role="tabpanel" class="tab-pane fade" id="tab_content3" aria-labelledby="profile-tab2">

                <div class="x_panel" id="Div_Shadow">
                    <div class="gantt_ot" style="width: 100%; height: 100%; margin: 2px auto;">
                        <div class="gantt"></div>
                    </div>
                </div>

            </div>
        </div>
        <!--以上機台清單列表-->
    </div>
    <!--甘特圖模式-->

    <!--甘特圖模式-->
    <!-----以下檢索精靈相關--->
    <!--機台選擇-->
    <!--資訊選擇-->
    <div id="exampleModal_time" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div id="testmodal32" style="padding: 5px 20px;">
                        <div class="form-group">
                            <h5 class="modaltextstyle">
                                <asp:TextBox ID="TextBox_Now" runat="server" Visible="true" Width="0" Style="display: none"></asp:TextBox>
                                <asp:TextBox ID="TextBox_Next" runat="server" Visible="true" Width="0" Style="display: none"></asp:TextBox>
                                <i class="fa fa-caret-down"><b>顯示欄位選擇</b></i> <i id="cbx_remind_fast3"></i>
                            </h5>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <asp:PlaceHolder ID="PlaceHolder_Item" runat="server"></asp:PlaceHolder>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default antoclose2" data-dismiss="modal">退出作業</button>
                    <asp:Button ID="buttontime" runat="server" Text="變更" class="btn btn-primary antosubmit2" OnClick="button_select_Click" />
                </div>
            </div>
        </div>
    </div>
    <!--資訊顯示-->


    <!--簡易報工用-->
    <div id="exampleModal_Report" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div id="testmoda22" style="padding: 5px 20px;">
                        <asp:TextBox ID="TextBox_machstatus" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="TextBox_project" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="TextBox_taskname" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="TextBox_machname" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="TextBox_task" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="TextBox_id" runat="server" Style="display: none"></asp:TextBox>
                        <div id="div_tc" class="col-xs-12 col-sm-12 col-md-12">

                            <div class="col-xs-12 col-sm-12 col-md-12" style="text-align: center">
                                <b style="font-size: 30px">本筆工單內容</b>
                            </div>
                            <br />
                            <div class="col-xs-12 col-sm-12 col-md-12" style="text-align: center">

                                <table id="TC" class="table table-ts table-bordered dt-responsive nowrap " cellspacing="0" width="100%">
                                    <thead>
                                        <tr style="background-color: white">
                                            <th>機台名稱</th>
                                            <th>品名規格</th>
                                            <th>現在數量</th>
                                            <th>目標數量</th>
                                            <th>預計開工</th>
                                            <th>預計結束</th>
                                            <th>製令單號</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>


                        <div class="col-md-12">
                            <hr />
                        </div>

                        <div class="col-md-4">
                        </div>

                        <div class="col-xs-12 col-sm-12 col-md-2">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                工單狀態
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <asp:RadioButtonList ID="RadioButtonList_status" runat="server">
                                    <asp:ListItem Selected="True" Value="入站">入站</asp:ListItem>
                                    <asp:ListItem Value="出站">出站</asp:ListItem>
                                    <asp:ListItem Value="完成">完成</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>

                        <div class="col-xs-12 col-sm-12 col-md-6">
                            <div class="col-xs-12 col-sm-12 col-md-12 ">
                                人員選擇
                            </div>

                            <div class="col-xs-12 col-sm-12 col-md-12 ">

                                <asp:RadioButtonList ID="RadioButtonList_Workman" runat="server">
                                </asp:RadioButtonList>
                                <%--                                    <div id="work_man" class="btn-group" data-toggle="buttons">
                                    </div>
                                    <asp:TextBox ID="TextBox_workman" runat="server" Style="display: none"></asp:TextBox>--%>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <hr />
                        </div>

                        <div class="col-md-4">
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-2">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                成品狀態
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                <asp:RadioButtonList ID="RadioButtonList_ProjectStatus" runat="server">
                                    <asp:ListItem Selected="True" Value="啟動">啟動</asp:ListItem>
                                    <asp:ListItem Value="新增">新增</asp:ListItem>
                                    <asp:ListItem Value="不良">不良</asp:ListItem>
                                </asp:RadioButtonList>

                            </div>
                        </div>


                        <div class="col-xs-12 col-sm-12 col-md-6">

                            <div class="col-xs-12 col-sm-12 col-md-6">
                                數量
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                <asp:TextBox ID="TextBox_QTY" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <div style="text-align: right; padding: 15px">
                        <button type="button" class="btn btn-default antoclose2" data-dismiss="modal">退出作業</button>
                        <button id="btnsave" type="button" class="btn btn-primary antosubmit2 ">送出</button>
                        <asp:Button ID="button_save" runat="server" Text="變更" class="btn btn-primary antosubmit2" OnClick="button_save_Click" Style="display: none" />

                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--簡易報工用-->



    <!--變更狀態用-->
    <div id="exampleModal_status" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div id="testmodal34" style="padding: 5px 20px;">
                        <div class="form-group">
                            <h5 class="modaltextstyle">
                                <i class="fa fa-caret-down" style="width: 100%; text-align: center"><b>狀態變更</b></i>
                                <asp:TextBox ID="TextBox_machine" runat="server"></asp:TextBox>
                                <asp:TextBox ID="TextBox_status" runat="server"></asp:TextBox>


                            </h5>
                            <div class="row">

                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12" style="margin-bottom: 15px">

                                    <%=status_button %>
                                </div>

                                <asp:Button ID="Button_status" runat="server" Text="變更狀態" Style="display: none" OnClick="Button_status_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- /page content -->
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
    <!-- Parsley -->
    <script src="../../assets/vendors/parsleyjs/dist/parsley.min.js"></script>
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
    <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
    <!--甘特圖元件-->

    <script src="../../gantt/js/jquery.fn.gantt.js"></script>
    <script src="../../gantt/js/prettify.min.js"></script>
    <script src="../../assets/vendors/Create_HtmlCode/HtmlCode.js"></script>
    <script>  


        //選擇狀態
        $('#ContentPlaceHolder1_RadioButtonList_status input').change(function () {
            relocation($(this).val());
        });
        relocation($('#ContentPlaceHolder1_RadioButtonList_status input:checked').val());

        function relocation(selValue) {
            var div_id = document.getElementById('div_tc');
            if (selValue == '完成') {
                document.getElementById('btnsave').innerHTML = '完工';
                //div_id.style.display = 'block';
            }

            else {
                document.getElementById('btnsave').innerHTML = '送出';
                // div_id.style.display = 'none';
            }

        }
        //===========================================表格===========================================
        $('#datatable_Info').dataTable(
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
                "order": [[0, "asc"]]
            });
        //============================================================================================


        $(function () {
            //            "use strict";
            //初始化gantt
            $(".gantt").gantt({
                source: [
                <%=Gantt_Data%>
                ],
                navigate: 'scroll',//buttons  scroll
                scale: "days",// months  weeks days  hours
                maxScale: "months",
                minScale: "hours",
                itemsPerPage: 20,
                onRender: function () {
                    if (window.console && typeof console.log === "function") {
                        console.log("chart rendered");
                    }
                }
            });
        });

        function status_change(status) {
            $('#ContentPlaceHolder1_TextBox_status').val('' + status + '');
            document.getElementById('<%=Button_status.ClientID %>').click();
        }
        function print_machine(mach) {
            $('#ContentPlaceHolder1_TextBox_machine').val('' + mach + '');
        }

        function get_information(text) {
            var divname = document.getElementById('machine_information');
            var string_array = text.split(',');
            var value = '';

            for (i = 0; i < string_array.length - 1; i++) {
                value +=
                    ' <div class="col-md-12 col-sm-12 col-xs-12">' +
                    ' <div class="col-md-12 col-sm-12 col-xs-12" style="font-size:22px;background-color:#f0f0f0;">' +
                    ' <b style="color:red;">' + string_array[i] + '</b>' +
                    ' </div>' +
                    ' <br>' +
                    ' <div class="col-md-12 col-sm-12 col-xs-12" style="font-size:22px;">' +
                    ' <b>' + string_array[i + 1].replaceAll('&', ' ') + '</b>' +
                    ' </div>' +
                    ' </div>';
                i++;
            }
            value += '<br><br><div style="font-size:25px;text-align:right;"><b><button type="button" style="width:100px;height:40px;font-size:18px" class="btn btn-success" onclick="javascript:location.href=' + "'" + '../dp_APS/WorkHourList.aspx?key=' + string_array[string_array.length - 1] + "'" + '">進入報工</button></b></div>';



            divname.innerHTML =
                '<div id="Div_Shadow" class="col-md-3 col-sm-12 col-xs-12" style="border:1px blue solid;padding:20px;">' +
                value +
                '</div>';
        }

<%--        set_machinelist('choose_status', '啟動,新增,不良,', '', '', 'machchange');
        set_machinelist('work_man', <%= workmans %>, '', '', 'workmanchange');--%>
        function set_value(status, project, taskname, mach, task, id, now_status) {
            $('#ContentPlaceHolder1_TextBox_machstatus').val('' + status + '');
            $('#ContentPlaceHolder1_TextBox_project').val('' + project + '');
            $('#ContentPlaceHolder1_TextBox_taskname').val('' + taskname + '');
            $('#ContentPlaceHolder1_TextBox_machname').val('' + mach + '');
            $('#ContentPlaceHolder1_TextBox_task').val('' + task + '');
            $('#ContentPlaceHolder1_TextBox_id').val('' + id + '');
            $("#ContentPlaceHolder1_RadioButtonList_status :radio[value='入站']").prop("checked", true);
            // document.getElementById('div_tc').style.display = 'none';
            $.ajax({
                type: 'POST',
                dataType: 'xml',
                url: "../../webservice/dp_aps.asmx/Now_Task",
                data: { Machine: mach, },
                success: function (xml) {
                    $(xml).find("ROOT_PIE").each(function (i) {
                        if ($(xml).find("ROOT_PIE").length > 0) {
                            var code;
                            var thisTable = $('#TC').dataTable();
                            thisTable.fnClearTable();
                            $(this).children().each(function (j) {
                                addData = [];
                                addData.push($(this).attr("機台名稱").valueOf());
                                addData.push($(this).attr("品名規格").valueOf());
                                addData.push($(this).attr("現在數量").valueOf());
                                addData.push($(this).attr("目標數量").valueOf());
                                addData.push($(this).attr("預計開工").valueOf());
                                addData.push($(this).attr("預計結束").valueOf());
                                addData.push($(this).attr("製令單號").valueOf());


                                thisTable.fnAddData(addData);
                            })

                        }
                    });
                },
                error: function (data, errorThrown) {
                    alert("Fail");
                }
            });

            //document.getElementById("dlg_titles").innerHTML = mach;


        }
        //function machchange(status) {
        //    $('#ContentPlaceHolder1_TextBox_projectstatus').val('' + status + '');
        //    if (status == '啟動') {
        //        $('#ContentPlaceHolder1_TextBox_QTY').val('' + '0' + '');
        //        document.getElementById("ContentPlaceHolder1_TextBox_QTY").disabled = true;
        //    }
        //    else {
        //        $('#ContentPlaceHolder1_TextBox_QTY').val('' + '' + '');
        //        document.getElementById("ContentPlaceHolder1_TextBox_QTY").disabled = false;
        //    }
        //}

        //function workmanchange(man) {
        //    $('#ContentPlaceHolder1_TextBox_workman').val('' + man + '');

        //}


        //當按鈕按下的時候，先執行LOADING的JS事件，在進行後台的計算
        $("#btnsave").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.querySelector(".blockUI.blockMsg.blockPage").style.zIndex = 1000000;
            document.getElementById('<%=button_save.ClientID %>').click();
        });







    </script>

</asp:Content>
