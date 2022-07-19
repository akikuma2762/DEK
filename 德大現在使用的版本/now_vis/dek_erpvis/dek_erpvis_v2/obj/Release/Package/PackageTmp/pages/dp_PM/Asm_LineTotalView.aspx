<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Asm_LineTotalView.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PD.Asm_LineTotalView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>整廠進度管理看板 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <link href="../../Content/dp_PM/Asm_LineTotalView.css" rel="stylesheet" />
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁</a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PMD">生產部</a></u></li>
        </ol>
        <div class="col-12">
            <div class="row">
                <div class="col-md-10 col-sm-10 col-xs-12 text-center">
                    <h1 class="text-center _mdTitle" style="width: 100%; margin-bottom: 15px"><b>整廠進度管理看板</b></h1>
                    <h3 class="text-center _xsTitle" ><b>整廠進度管理看板</b></h3>
                </div>
            </div>
        </div>
        <!-----------------title------------------>
        <!-- top tiles -->

        <!-- /top tiles -->
        <!------------------Title--------------------->
        <!-----------------/Title--------------------->
        <div class="clearfix"></div>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <div class="col-12">
            <asp:Button ID="Button_Jump" runat="server" Text="Button" OnClick="Button_Jump_Click" Style="display: none" />
            <asp:TextBox ID="TextBox_textTemp" runat="server" Visible="true" Width="80px" Style="display: none"></asp:TextBox>

            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="x_panel Div_Shadow">

                        <div class="x_content">
                            <div class="col-md-9 col-sm-12 col-xs-12 padding-left-right-4">
                                <div class="x_panel">
                                    <div class="row tile_count" style="margin-top: 0px; margin-bottom: -20px;padding:0 10px">
                                        <div class="col-md-offset-2 col-md-2 col-sm-3 col-xs-6 tile_stats_count">
                                            <span class="count_top"><i class="fa fa-clock-o"></i>今日在線</span>
                                            <div class="count blue"><%=TotalOnLinePiece %><span style="height: 10px"><%=PieceUnit%></span></div>
                                            <!--<span class="count_bottom"><i class="green"><i class="fa fa-sort-asc"></i>0% </i>survive</span>-->
                                        </div>
                                        <div class="col-md-2 col-sm-3 col-xs-6 tile_stats_count">
                                            <span class="count_top"><i class="fa fa-clock-o"></i>今日完成</span>
                                            <div class="count green"><% =TotalFinishPiece%><span style="height: 10px"><%=PieceUnit%></span></div>
                                            <!--<span class="count_bottom"><i class="green"><i class="fa fa-sort-asc"></i>0% </i>survive</span>-->
                                        </div>
                                        <div class="col-md-2 col-sm-3 col-xs-6 tile_stats_count">
                                            <span class="count_top"><i class="fa fa-clock-o"></i>今日異常</span>
                                            <div class="count red"><%=no_solved %><span style="height: 10px"><%=PieceUnit%></span></div>
                                            <!--<span class="count_bottom"><i class="red"><i class="fa fa-sort-asc"></i>0% </i>survive</span>-->
                                        </div>
                                        <div class="col-md-2 col-sm-3 col-xs-6 tile_stats_count">
                                            <span class="count_top"><i class="fa fa-clock-o"></i>當下落後</span>
                                            <div class="count black"><%=behind %><span style="height: 10px"><%=PieceUnit%></span></div>
                                        </div>
                                    </div>
                                    <hr />
                                    <table id="TB" class="table table-bordered" cellspacing="0" width="100%">
                                        <tr id="tr_row">
                                            <%=ColumnsData%>
                                        </tr>
                                        <tbody>
                                            <%=RowsData%>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-12 col-xs-12 padding-left-right-4">
                                <div class="col-md-12 col-sm-12 col-xs-12" style="text-align: center;">
                                    <div class="dashboard_graph x_panel" <%--style="height: 1000%"--%>>

                                        <div class="col-md-12 col-sm-6 col-xs-12 flex-align col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>選擇廠區</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:DropDownList ID="dropdownlist_X" runat="server" CssClass="btn btn-default form-control" >
                                                    <asp:ListItem Value="0" Selected="True">立式</asp:ListItem>
                                                    <asp:ListItem Value="1">臥式</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                       <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="col-md-12 col-xs-12 text-align-end">
                                                <button id="btncheck" type="button" class="btn btn-primary antosubmit2">執行搜索</button>
                                                <%--<input name="ctl00$ContentPlaceHolder1$bt_Ver" type="submit" id="ContentPlaceHolder1_bt_Ver" class="btn btn-primary" value="確定">--%>
                                            </div>
                                        </div>

                                        <%-- <input name="ctl00$ContentPlaceHolder1$bt_Hor" type="submit" id="ContentPlaceHolder1_bt_Hor" class="btn btn-warning" value="臥式">--%>
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
    <!-- set Modal -->
    <!-- 20191218總經理覺得佔位置，所以移除<div class="backdrop">
    </div>
    <div class="fab child" data-subitem="1" data-toggle="modal" data-target="#exampleModal">
        <span>
            <i class="fa fa-search"></i>
        </span>
    </div>
    <div class="fab" id="masterfab">
        <span>
            <i class="fa fa-list-ul"></i>
        </span>
    </div>-->
    <!--/set Modal-->
    <!-- Modal1 -->
    <div id="exampleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title modaltextstyle" id="myModalLabel2"><i class="fa fa-file-text"></i>選單功能</h4>
                </div>
                <div class="modal-body">
                    <div id="testmodal2" style="padding: 5px 20px;">
                        <div class="form-group">

                            <h5 class="modaltextstyle">
                                <i class="fa fa-caret-down">廠區選擇</i> <i id="cbx_remind_fast"></i>
                            </h5>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="col-md-11 xdisplay_inputx form-group has-feedback">
                                        <input id="bt_Ver" type="submit" class="btn btn-primary" value="立式" runat="server" onserverclick="bt_Hor_ServerClick" />
                                        <input id="bt_Hor" type="submit" class="btn btn-warning" value="臥式" runat="server" onserverclick="bt_Hor_ServerClick" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
    <!-- jQuery -->
    <script src="../../assets/vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="../../assets/vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- bootstrap-progressbar -->
    <script src="../../assets/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js"></script>
    <!-- iCheck -->
    <script src="../../assets/vendors/iCheck/icheck.min.js"></script>
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

        function jump_AsmLineOverView(paramer) {
            var WhatSystem = navigator.userAgent;
            if (WhatSystem.match(/(iphone|ipad|ipod);?/i)) {
            } else {
                $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                $.unblockUI();
            }
            $('#ContentPlaceHolder1_TextBox_textTemp').val('' + paramer + '');
            document.getElementById('<%=Button_Jump.ClientID %>').click();
        }
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=bt_Ver.ClientID %>').click();
        });
        //防止切換頁籤時跑版
        $(document).ready(function () {
            $('#example').DataTable({
                responsive: true
            });
            $('#exampleInTab').DataTable({
                responsive: true
            });

            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                $($.fn.dataTable.tables(true)).DataTable()
                    .columns.adjust();
            });
        });
    </script>
</asp:Content>
