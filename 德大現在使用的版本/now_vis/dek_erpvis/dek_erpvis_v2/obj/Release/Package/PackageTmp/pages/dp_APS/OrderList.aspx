<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="OrderList.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_APS.OrderList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>APSList | 緯凡金屬股份有限公司</title>
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-----------------title------------------>
    <div class="right_col" role="main" >
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁 </a></u></li>
            <li>報工清單</li>
        </ol>
        <br>

        <div class="">
            <div class="page-title">
                <div class="title_left" style="width: 100%;">
                    <h3>&nbsp 報工清單<small></small></h3>
                </div>
            </div>
        </div>

        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">

                    <div class="x_content">
                        <p class="text-muted font-13 m-b-30">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="col-md-2 col-sm-4 col-xs-12">
                                    開始時間<br>
                                    <asp:TextBox ID="TextBox_textTemp" runat="server" Style="display: none"></asp:TextBox>
                                    <asp:Button ID="Button_change" runat="server" Text="Button" OnClick="Button_change_Click" Style="display: none" />
                                    <asp:TextBox ID="TextBox_Start" runat="server" TextMode="Date"></asp:TextBox>
                                </div>
                                <div class="col-md-2 col-sm-4 col-xs-12">
                                    結束時間<br>
                                    <asp:TextBox ID="TextBox_End" runat="server" TextMode="Date"></asp:TextBox>
                                </div>
                                <div class="col-md-7 col-sm-4 col-xs-12">
                                    <br>
                                    <button id="btnsearch" type="button" class="btn btn-primary antosubmit2 ">搜尋</button>
                                    <asp:Button ID="Button_Search" runat="server" class="btn btn-primary antosubmit2" Text="搜尋" OnClick="Button_Search_Click" Style="display: none" />
                                </div>
                                <br>
                                <br>
                                <hr>
                            </div>
                        </p>
                        <ul id="myTab" class="nav nav-tabs" role="tablist">
                            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true">未結案</a>
                            </li>
                            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" role="tab" id="profile-tab" data-toggle="tab" aria-expanded="false">已結案</a>
                            </li>
                            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content3" role="tab" id="profile-tab1" data-toggle="tab" aria-expanded="false">無效</a>
                            </li>
                        </ul>
                        <div id="myTabContent" class="tab-content">

                            <!--以下已結案-->
                            <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">
                                <div class="x_panel" id="Div_Shadow">
                                    <div class="x_content">
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                        </div>
                                        <p class="text-muted font-13 m-b-30">
                                        </p>
                                        <table id="datatable_Open" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%">
                                            <thead>
                                                <tr style="background-color: #FFFFFF">
                                                    <%=Open_th%>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <%=Open_tr %>
                                            </tbody>
                                        </table>

                                    </div>
                                </div>
                            </div>

                            <%--以下未結案--%>
                            <div role="tabpanel" class="tab-pane fade" id="tab_content2" aria-labelledby="profile-tab">
                                <div class="x_panel" id="Div_Shadow">
                                    <div class="x_content">
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                        </div>
                                        <p class="text-muted font-13 m-b-30">
                                        </p>
                                        <table id="datatable_Close" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%">
                                            <thead>
                                                <tr style="background-color: #FFFFFF">
                                                    <%=Close_th%>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <%=Close_tr %>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>


                            <%--以下無效--%>
                            <div role="tabpanel" class="tab-pane fade" id="tab_content3" aria-labelledby="profile-tab1">
                                <div class="x_panel" id="Div_Shadow">
                                    <div class="x_content">
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                        </div>
                                        <p class="text-muted font-13 m-b-30">
                                        </p>
                                        <table id="datatable_Fail" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%">
                                            <thead>
                                                <tr style="background-color: #FFFFFF">
                                                    <%=Failure_th%>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <%=Failure_tr %>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>






                            <!--全部的詳細資訊部分-->
                        </div>
                    </div>



                </div>
            </div>
        </div>
        <!-----------------/content------------------>
    </div>
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
    <!-- moment -->
    <script src="../../assets/vendors/moment/min/moment.min.js"></script>
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
    <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
    <script src="../../assets/vendors/time/loading.js"></script>
    <script>
        //當按鈕按下的時候，先執行LOADING的JS事件，在進行後台的計算
        $("#btnsearch").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=Button_Search.ClientID %>').click();
        });



        $('#datatable_Close').dataTable(
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
        $('#datatable_Open').dataTable(
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
        $('#datatable_Fail').dataTable(
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
        //----------------------------------------------------------------------------------------------------


        function Go_WorkHourList(obj) {
            document.location.href = "WorkHourList.aspx?OrderNum=" + obj.id;
        }
        function Go_OrderStatusEdit(obj, starttime, endtime) {
            document.location.href = "OrderStatusEdit.aspx?OrderNum=" + obj.id + ",StartTime=" + starttime + ",EndTime=" + endtime;
        }
        function func(str) {

            var e = document.getElementById("ContentPlaceHolder1_DropDownList_" + str);
            var strUser = e.options[e.selectedIndex].text;
            $('#ContentPlaceHolder1_TextBox_textTemp').val('' + str + '^' + strUser + '');
            document.getElementById('<%=Button_change.ClientID %>').click();
        }
    </script>
</asp:Content>
