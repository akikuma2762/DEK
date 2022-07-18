<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="WorkHourList.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_APS.WorkHourList" %>

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
            <li><u><a href="../dp_APS/OrderList.aspx">報工清單</a></u></li>
            <li>報工列表</li>
        </ol>
        <br>
        <div class="">
            <div class="page-title">
                <div class="title_left" style="width: 100%;">
                    <h3>&nbsp 報工列表<small></small></h3>
                </div>
            </div>
        </div>
        <asp:TextBox ID="TextBox_textTemp" runat="server" Visible="true" Width="1000px" Style="display: none"></asp:TextBox>

        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel" id="Div_Shadow">
                    <div class="x_content">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="col-md-6 col-sm-12 col-xs-12">
                                送料單號：<%=order_num %>
                                <br />
                                品名規格：<%=product_name %>
                                <br />
                                母件編號：<%=product_num %>
                                <br />
                            </div>
                            <!------------------------------------------------------------------------->
                            <div class="col-md-6 col-sm-12 col-xs-12">
                                <div class="col-md-3 col-sm-4 col-xs-12" style="margin: 0 auto;">
                                    開始時間
                                    <br>
                                    <asp:TextBox ID="TextBox_Start" runat="server" TextMode="Date"></asp:TextBox>
                                </div>
                                <div class="col-md-3 col-sm-4 col-xs-12">
                                    結束時間<br>
                                    <asp:TextBox ID="TextBox_End" runat="server" TextMode="Date"></asp:TextBox>
                                </div>
                                <div class="col-md-2 col-sm-2 col-xs-12">
                                    <br>
                                    <asp:DropDownList ID="DropDownList_Resource" runat="server"></asp:DropDownList>
                                </div>
                                <div class="col-md-4 col-sm-2 col-xs-12">
                                    <br>
                                    <button id="btnsearch" type="button" class="btn btn-primary antosubmit2 ">搜尋</button>

                                    <asp:Button ID="Button_Search" runat="server" class="btn btn-primary antosubmit2" Text="搜尋" OnClick="Button_Search_Click" Style="display: none" />
                                </div>
                                <br>
                                <br>
                                <br>
                                <br>
                            </div>

                        </div>

                        <table id="datatable-buttons" class="table table-bordered nowrap table-ts" cellspacing="0" width="100%">
                            <thead>
                                <tr style="background-color: #FFFFFF">
                                    <%=th%>
                                </tr>
                            </thead>
                            <tbody>
                                <%=tr %>
                            </tbody>
                        </table>

                    </div>
                </div>
            </div>
        </div>
        <!-----------------/content------------------>
    </div>


    <div id="exampleModal_information" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div id="testmodal33" style="padding: 5px 20px;">
                        <div class="form-group">
                            <h5 class="modaltextstyle">
                                <i><b></b></i>
                            </h5>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                                    </asp:ScriptManager>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="True">
                                        <ContentTemplate>
                                            請填入數量：<asp:TextBox ID="TextBox_Qty" runat="server"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btncheck" type="button" class="btn btn-primary antosubmit2 ">送出</button>
                    <asp:Button runat="server" Text="提交" ID="Button_Add" OnClick="Button_Add_Click" CssClass="btn btn-primary" Style="display: none" />
                </div>
            </div>
        </div>
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
    <script>
        //當按鈕按下的時候，先執行LOADING的JS事件，在進行後台的計算
        $("#btnsearch").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });

            document.getElementById('<%=Button_Search.ClientID %>').click();
        });

        //執行動作的時候
        function start(WorkHourID, Project, C_TaskName, TaskName) {
            $('#ContentPlaceHolder1_TextBox_textTemp').val('' + WorkHourID + ',' + Project + ',' + C_TaskName + ',' + TaskName + ',' + TaskName + '');
            document.getElementById('<%=Button_Add.ClientID %>').click();
        }
        function Add(WorkHourID, Project, C_TaskName, TaskName) {
            $('#ContentPlaceHolder1_TextBox_textTemp').val('' + WorkHourID + ',' + Project + ',' + C_TaskName + ',' + TaskName + '');
        }


        $("#btncheck").click(function () {
            document.getElementById('<%=Button_Add.ClientID %>').click();
        });


    </script>
</asp:Content>
