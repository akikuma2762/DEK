<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" validateRequest="false" CodeBehind="Business_Report_detail.aspx.cs" Inherits="dek_erpvis_v2.pages.SYS_CONTROL.Business_Report_detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>報告詳細紀錄 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
       <link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <script src="https://cdn.ckeditor.com/4.7.3/standard/ckeditor.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁 </a></u></li>
            <li><u><a href="../SYS_CONTROL/Business_Report.aspx">業務報告</a></u></li>
        <br>
        </ol>
        <div class="page-title">
            <div class="title_left">
             
            </div>
            <div class="title_right">
            </div>
        </div>
        <div class="clearfix"></div>
        <!-----------------/title------------------>
  
        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <div style="position:absolute;right:0;"> 
                        <asp:PlaceHolder ID="PlaceHolder_hidden" runat="server">
                        <asp:Button ID="buttonupdate" runat="server" Text="編輯文章" class="btn btn-secondary" OnClick="button_update_Click" Style="display: none" />
                         <button id="btnupdate" type="button" class="btn btn-success" >編輯文章</button>
                            <asp:Button ID="buttondelete" runat="server" Text="刪除文章" class="btn btn-secondary" OnClick="button_delete_Click" Style="display: none" />
                         <button id="btndelete" type="button" class="btn btn-success" >刪除文章</button>  
                        </asp:PlaceHolder> 
                       </div>
                    <div style="font-size:24px;font-weight:bold;width:60%"><b><%=title_name %></b></div>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content" >
                            
                                
                       </div>
                    <div style="position:absolute;right:0;font-size:16px;color:red;position:absolute;bottom:0;"><I>最後更新日期：<%=time %></I></div>
                               <div style="font-size:20px"><%=detail %><hr></div> 
                                       
                    </div><asp:Button ID="buttonreturn" runat="server" Text="返回上一頁" class="btn btn-secondary" OnClick="button_return_Click" Style="display: none" />
                         <button id="btnreturn" type="button" class="btn btn-default">返回上一頁</button> 
                </div>
             
            </div>
        </div>
        
    </div>
   <!-----------------/content------------------>
    <!-- jQuery -->
    <script src="../../assets/vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="../../assets/vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- FastClick -->
    <script src="../../assets/vendors/fastclick/lib/fastclick.js"></script>
    <!-- iCheck -->
    <script src="../../assets/vendors/iCheck/icheck.min.js"></script>
    <!-- bootstrap-daterangepicker -->
    <script src="../../assets/vendors/moment/min/moment.min.js"></script>
    <script src="../../assets/vendors/bootstrap-daterangepicker/daterangepicker.js"></script>
    <!-- Switchery -->
    <script src="../../assets/vendors/switchery/dist/switchery.min.js"></script>
    <!-- Select2 -->
    <script src="../../assets/vendors/select2/dist/js/select2.full.min.js"></script>
    <!-- Autosize -->
    <script src="../../assets/vendors/autosize/dist/autosize.min.js"></script>
    <!-- jQuery autocomplete -->
    <script src="../../assets/vendors/devbridge-autocomplete/dist/jquery.autocomplete.min.js"></script>
    <!-- Custom Theme Scripts -->
    <script src="../../assets/build/js/custom.min.js"></script>
    <!-- FloatingActionButton -->
    <script src="../../assets/vendors/FloatingActionButton/js/index.js"></script>
    <!-- canvasjs -->
    <script src="../../assets/vendors/canvas_js/canvasjs.min.js"></script>
    <!-- bootstrap-touchspin-master -->
    <script src="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.js"></script>
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
        var selectedcondi = document.getElementById('ContentPlaceHolder1_TextBox_textTemp');
        //更新的按鈕事件
        $("#btnupdate").click(function()
        {
            document.getElementById('<%=buttonupdate.ClientID %>').click();          
        });
        //刪除的按鈕事件
        $("#btndelete").click(function()
        {
            answer = confirm("你確定要刪除該篇文章嗎嗎??");
            if (answer) {
               document.getElementById('<%=buttondelete.ClientID %>').click();
                }
        });
        $("#btnreturn").click(function()
        {
           document.getElementById('<%=buttonreturn.ClientID %>').click();
        });
    </script>

</asp:Content>