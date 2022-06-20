<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" validateRequest="false" CodeBehind="Add_Report.aspx.cs" Inherits="dek_erpvis_v2.pages.SYS_CONTROL.Add_Report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=inup %>文章 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <script src="https://cdn.ckeditor.com/4.7.3/standard/ckeditor.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁 </a></u></li>
            <li><u><a href="../SYS_CONTROL/Business_Report.aspx">業務報告</a></u></li>
        </ol>
        <div class="page-title">
            <div class="title_left">
                <h3><%=inup %>文章</h3>
            </div>
            <div class="title_right">
         <style>
        input[type="checkbox"]
        {
            width:18px;
            height:18px;
            cursor:auto;
            -webkit-appearance:default-button;
        }
    </style>
            </div>
        </div>
        <div class="clearfix"></div>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <div class="row">
            <div class="col-md-8 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                     <h2><%=inup %>文章<small></small></h2>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                             標題<asp:TextBox ID="TextBox_Title" runat="server" CssClass="form-control"></asp:TextBox><br>     
                             類型<asp:DropDownList ID="DropDownList_Type" runat="server" class="form-control"></asp:DropDownList><br>                                 
                                公開權限<br>
                            <asp:CheckBoxList ID="CheckBoxList_SYS" runat="server" RepeatColumns="10" Font-Size="14" class="control-label"></asp:CheckBoxList>
                               <textarea name="editor" id="txt" ></textarea>
                               <script>CKEDITOR.replace("editor");</script><br>
                            <div class="row">
                               <div class="col-md-0.1 col-sm-0 col-xs-0 ">
                            <label for="code">&nbsp 驗證碼 * :</label>
                        </div>
                        <div class="col-md-2 col-sm-2 col-xs-5 ">
                            <asp:Label ID="Label_code" runat="server" Text="" class="form-control"> </asp:Label>
                        </div>
                        <div class="col-md-2 col-sm-2 col-xs-5.5 ">
                            <asp:TextBox ID="TextBox_code" runat="server" placeholder="*請輸入左側驗證碼" class="form-control" autocomplete="off"></asp:TextBox>
                     <i id="cbx_remind"></i> 
                        </div>
                    </div><br>
                 
                               <asp:Button ID="buttonadd" runat="server" Text="保存" class="btn btn-secondary" OnClick="button_add_Click" Style="display: none" />
                               <button id="btnadd" type="button" class="btn btn-success">保存</button> 
                               <asp:Button ID="buttoncancel" runat="server" Text="取消" class="btn btn-secondary" OnClick="button_cancel_Click" Style="display: none" />
                               <button id="btncancel" type="button" class="btn btn-success">取消</button> 
                                <asp:Button ID="buttonclear" runat="server" Text="清空" class="btn btn-secondary" OnClick="button_clear_Click" Style="display: none" />
                               <button id="btnclear" type="button" class="btn btn-default">清空</button>
                               <asp:TextBox ID="TextBox_textTemp" runat="server" Visible="true" Width="0"></asp:TextBox>
                                    
                    </div>
                </div>
            </div>
        </div>
        <!-----------------/content------------------>
    </div>
    
    <!--HTML doc edit-->
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
           //新增的按鈕
           $("#btnadd").click(function () {
            var code = document.getElementById("ContentPlaceHolder1_TextBox_code").value;
            var lable = document.getElementById("ContentPlaceHolder1_Label_code").innerText;
               if (code != "") {
                   if (code == lable) {
                       selectedcondi.style.visibility = "visible";
                       var text = CKEDITOR.instances.txt.getData();
                       $('#ContentPlaceHolder1_TextBox_textTemp').val('' + text.toString() + '');
                       document.getElementById('<%=buttonadd.ClientID %>').click();
                   } else {
                       var remind = document.getElementById("cbx_remind");
                       remind.innerHTML = "與驗證碼不同，請重新輸入 !";
                       remind.style.color = "#FF3333";
                   }
               } else {
                   var remind = document.getElementById("cbx_remind");
                       remind.innerHTML = "請輸入驗證碼 !";
                       remind.style.color = "#FF3333";
               }
           });
           //取消的按鈕
           $("#btncancel").click(function () {
               document.getElementById('<%=buttoncancel.ClientID %>').click();
           });
           //清空的按鈕
           $("#btnclear").click(function () {
               document.getElementById('<%=buttonclear.ClientID %>').click();
           });
           //畫面一進來的動作
           window.onload = showdata();
           //TextBox有資料就顯示，沒有就隱藏
           function showdata() {
           var text_value = $('#ContentPlaceHolder1_TextBox_textTemp').val();
               if (text_value != "") {
                   CKEDITOR.instances.txt.setData('' + text_value.toString() + ''); 
               }
           selectedcondi.style.visibility = "hidden";
           }
       </script>

        <script>
       
    </script>
</asp:Content>
