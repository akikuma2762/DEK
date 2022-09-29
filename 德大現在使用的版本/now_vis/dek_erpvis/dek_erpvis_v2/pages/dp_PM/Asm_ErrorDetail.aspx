<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Asm_ErrorDetail.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PM.Asm_ErrorDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=LineName %> 維護歷程 | 整廠進度管理看板</title>
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/dp_PM/Asm_ErrorDetail.css" rel="stylesheet" />
    <style>
        #testmodal2 {
            padding: 5px 20px;
        }

        @media screen and (max-width:768px) {
            /*彈跳視窗*/
            #ContentPlaceHolder1_UpdatePanel1 div.input-group textarea,
            #ContentPlaceHolder1_UpdatePanel1 div.input-group span.input-group-btn {
                width: 100%;
                display: block;
            }

                #ContentPlaceHolder1_UpdatePanel1 div.input-group span.input-group-btn input {
                    width: 100%;
                    margin-top: 10px;
                }

            #ContentPlaceHolder1_UpdatePanel1 div.form-group label,
            #ContentPlaceHolder1_UpdatePanel1 div.form-group:nth-child(4) label {
                width: 35%;
                display: inline-block;
            }

            #ContentPlaceHolder1_UpdatePanel1 div.form-group select,
            #ContentPlaceHolder1_UpdatePanel1 div.form-group:nth-child(4) > div {
                width: 63%;
                display: inline-block;
            }

            #testmodal2 {
                padding: 0;
            }

            #ContentPlaceHolder1_UpdatePanel1 div.form-group:nth-child(3) label,
            #ContentPlaceHolder1_UpdatePanel1 div.form-group:nth-child(3) input,
            #ContentPlaceHolder1_UpdatePanel1 div.form-group:nth-child(4) > div select {
                width: 100%;
            }
            /*結案說明*/
            #ContentPlaceHolder1_UpdatePanel1 div.form-group:nth-child(4) > div.form-group {
                width: 100%;
            }

                #ContentPlaceHolder1_UpdatePanel1 div.form-group:nth-child(4) > div.form-group #ContentPlaceHolder1_FileUpload_Close {
                    font-size: 0.7em;
                }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-----------------content------------------>
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁</a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PMD">生產部</a></u></li>
            <li><u><a href="Asm_LineTotalView.aspx">整廠進度管理看板</a></u></li>
            <li><u><a href="Asm_LineOverView.aspx?key=<%=UrlLink %>"><%=LineName %></a></u></li>
            <%=go_back %>
        </ol>
        <br />

        <div class="clearfix"></div>
        <!-----------------/title------------------>

        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_title ">
                        <!-- top tiles -->
                        <div class="row">
                            <h1 class="text-center _mdTitle" style="width: 100%"><b>異常歷程</b></h1>
                            <h3 class="text-center _xsTitle" style="width: 100%"><b>異常歷程</b></h3>
                        </div>

                        <!-- /top tiles -->
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content _select _setborder">
                        <label class="control-label">刀庫編號：<b style="margin-left: 10px;"><%=Key %></b></label><br>
                        <label class="control-label">CCS：<b style="margin-left: 10px;"><%=CCS %></b></label>
                        <asp:PlaceHolder ID="PlaceHolder_hidden" runat="server">
                            <form id="demo-form2" class="form-horizontal form-label-left">
                                <div class="form-group">
                                    <label class="control-label " for="last-name">異常內容 </label>
                                    <div class="input-group col-xs-12">
                                        <asp:TextBox ID="MantStr" runat="server" TextMode="MultiLine" Width="100%" Height="150%" class="style1" onkeyup="autogrow(this);" Style="overflow: hidden;"></asp:TextBox>

                                        <%--<input id="Mant_Str" runat="server" type="text" name="Mant_Str" class="form-control" placeholder="輸入內容...">--%>
                                    </div>
                                </div>

                                <div class="form-group _type">
                                    <label class="control-label3 " for="Error-Type">異常類型</label>
                                    <asp:DropDownList ID="DropDownList_Errorfa" runat="server" class="form-control">
                                    </asp:DropDownList>
                                </div>

                                <div class="form-group _line">
                                    <label class="control-label3 " for="Error-Type">發佈到LINE</label>
                                    <asp:RadioButtonList ID="RadioButtonList_Post" runat="server" CssClass="table-striped" RepeatColumns="2">
                                        <asp:ListItem Selected="True" Value="1">是&nbsp&nbsp&nbsp&nbsp</asp:ListItem>
                                        <asp:ListItem Value="0">否</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="form-group _UpdateImg">
                                    <label class="control-label3 " for="Error-Type">上傳圖片(可多張上傳)<span class="required"></span></label>
                                    <asp:FileUpload ID="FileUpload_image" runat="server" AllowMultiple="True" />
                                </div>
                                <div class="form-group _button col-12 col-md-1 col-md-offset-11">
                                    <input type="button" class="btn btn-primary" name="Mant_Btn" id="Mant_Btn" runat="server" value="新增" onserverclick="Unnamed_ServerClick" style="width: 100%" />
                                </div>
                            </form>
                        </asp:PlaceHolder>
                        <div class="clearfix"></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <div id="_InformationDetail" class="row">
            <asp:TextBox ID="TextBox_num" runat="server" Visible="true" Width="0" Style="display: none"></asp:TextBox>
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow" <%=ErrorTitleDisplayType[0] %>>
                    <div class="x_title">
                        <h2><i class="fa fa-tags"><%=ErrorTitleArray[0]%></i> <small></small></h2>
                        <ul class="nav navbar-right panel_toolbox">

                            <!--   <li><a class="" style="color: darkcyan"><i class="fa fa-user"><%=ErrorTitleDisplayDep[0] %></i></a>
                            </li>
                            <li><a class="" <%=ErrorTitleDisplayStatusColor[0] %>><i class="fa fa-circle"><%=ErrorTitleDisplayStatus[0] %></i></a>
                            </li>-->
                            <li>
                                <input id="bt_del" type="button" class="btn btn-danger" name="table1_bt_del" runat="server" onserverclick="bt_del_ServerClick" value="刪除" style="display: none" />
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">

                        <table id="datatable-checkbox0" class="table table-bordered bulk_action StandardTable" cellspacing="0" width="100%">
                            <thead>
                                <%=ColumnsData%>
                            </thead>
                            <tbody>
                                <%=RowsDataArray[0]%>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
    </div>
    <!-----------------20200424留言板功能---------------------------------------------->
    <div id="exampleModal_Image" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    <div id="testmodal25" style="text-align: center;">
                        <%--                        <img src="http://210.61.157.250:57958/pages/dp_PM/Backup_Error_Image/2020-8-19_%E4%B8%8B%E5%8D%88_02-05-48.jpg" alt="..." width="400px" height="400px">--%>
                        <label id="lbltipAddedComment">test</label>
                    </div>
                </div>
                <!--2019/06/11，送出按鈕=>先透過jQuery進行格式驗證，再透過asp.btn.onclick事件送出 (ru)-->
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>



    <div id="exampleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="col-md-12 col-sm-12 col-xs-12" style="text-align: center">
                            <h1 class="text-center" style="width: 100%" id="word"><b></b></h1>
                        </div>
                    </div>
                </div>
                <div class="modal-body">
                    <div id="testmodal2">

                        <asp:TextBox ID="TextBox_textTemp" runat="server" Visible="true" Width="80px" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="TextBox_ErrorID" runat="server" Visible="true" Width="80px" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="TextBox_acc" runat="server" Style="display: none"></asp:TextBox>

                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="form-group">

                                        <label class="control-label " for="last-name">異常內容 </label>
                                        <div class="input-group col-xs-12">
                                            <asp:TextBox ID="TextContent" runat="server" TextMode="MultiLine" Width="100%" Height="150%" class="style1" onkeyup="autogrow(this);" Style="overflow: hidden;"></asp:TextBox>

                                            <%--<input id="Text_Content" runat="server" type="text" name="Text_Content" class="form-control" placeholder="輸入內容..." />--%>

                                            <span class="input-group-btn">
                                                <asp:Button ID="button_select" runat="server" Text="新增" class="btn btn-primary antosubmit2" OnClick="AddContent_Click" />
                                                <%--                               <button id="btncheck" type="button" class="btn btn-primary antosubmit2">保存</button>--%>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label3 " for="Error-Type">異常類型 </label>
                                        <asp:DropDownList ID="DropDownList_ErrorChild" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group _line">
                                        <label class="control-label3 " for="Error-Type">發佈到LINE</label>
                                        <asp:RadioButtonList ID="RadioButtonList_Upost" runat="server" CssClass="table-striped" RepeatColumns="2">
                                            <asp:ListItem Selected="True" Value="1">是&nbsp&nbsp&nbsp&nbsp</asp:ListItem>
                                            <asp:ListItem Value="0">否</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <asp:PlaceHolder ID="PlaceHolder_image" runat="server">
                                        <div class="form-group">
                                            <label class="control-label3 " for="Error-Type">上傳圖片(可多張上傳)<span class="required">:</span></label>
                                            <asp:FileUpload ID="FileUpload_Content" runat="server" AllowMultiple="True" />
                                        </div>
                                        <asp:CheckBoxList ID="CheckBoxList_UpdateError" runat="server" RepeatColumns="1"></asp:CheckBoxList>
                                    </asp:PlaceHolder>
                                    <div class="form-group">
                                        <label class="control-label3 " for="Error-Type">處理狀態<span class="required">:</span></label>
                                        <div class="">
                                            <asp:DropDownList ID="DropDownList_Status" runat="server" class="form-control" onchange="Change_Content()">
                                            </asp:DropDownList>
                                        </div>
                                        <div id="Case_Close" runat="server" style="display: none">
                                            <%--  <div class="form-group">
                                                <label class="control-label1 " for="Error-Type">異常類型 <span class="required">:</span></label>
                                                <div class="">
                                                    <asp:DropDownList ID="DropDownList_ErrorType" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>--%>
                                            <div class="form-group">
                                                <label class="control-label1 " for="Error-Type">結案說明 <span class="required">:</span></label>
                                                <div class="">
                                                    <asp:TextBox ID="TextBox_Report" runat="server" TextMode="MultiLine" Width="100%" Height="150%" class="style1" onkeyup="autogrow(this);" Style="overflow: hidden;"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label1 " for="Error-Type">結案附檔 <span class="required">:</span></label>
                                                <div class="">
                                                    <asp:FileUpload ID="FileUpload_Close" runat="server" AllowMultiple="True" />
                                                </div>
                                                <asp:CheckBoxList ID="CheckBoxList_Close" runat="server" RepeatColumns="1"></asp:CheckBoxList>
                                            </div>
                                        </div>
                                        <asp:Button ID="Button_Update" runat="server" Text="Button" OnClick="Button_Update_Click" Style="display: none" />
                                        <asp:Button ID="Button_Add" runat="server" Text="Button" OnClick="Button_Add_Click" Style="display: none" />
                                    </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="button_select" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <!--2019/06/11，送出按鈕=>先透過jQuery進行格式驗證，再透過asp.btn.onclick事件送出 (ru)-->
                <div class="modal-footer">
                    <%--                    <asp:Button ID="button_select" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="AddContent_Click" Style="display: none" />--%>
                </div>
            </div>
        </div>
    </div>
    <!-----------------20200424留言板功能---------------------------------------------->





    <!-----------------/content------------------>
    <!-- set Modal -->
    <!--/set Modal-->
    <!-- Modal -->
    <!-- /Modal -->
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
    <script src="../../assets/vendors/datatables.net-colReorder/dataTables.colReorder.min.js"></script>
    <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
    <script src="../../assets/vendors/Create_HtmlCode/HtmlCode20211210.js?version = 1.0"></script>
    <!-----------------20200424留言板功能------------------------------------->
    <script>
        //隨著文字長度變更TEXTBOX長度
        function autogrow(textarea) {
            var adjustedHeight = textarea.clientHeight;
            adjustedHeight = Math.max(textarea.scrollHeight, adjustedHeight);
            if (adjustedHeight > textarea.clientHeight)
                textarea.style.height = adjustedHeight + 'px';
            if (textarea.value == '')
                textarea.style.height = 100 + 'px';
        }

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
        var remind = document.getElementById("word");
        //新增時使用
        function Get_ErrorDetails(ID, Error_ID, Type, acc) {
            remind.innerHTML = "回覆內容";
            document.getElementById('<%=button_select.ClientID %>').value = '新增';
            $('#ContentPlaceHolder1_Text_Content').val('' + '' + '');
            $('#ContentPlaceHolder1_TextBox_acc').val('' + acc + '');
            $('#ContentPlaceHolder1_TextBox_textTemp').val('' + ID + '');
            $('#ContentPlaceHolder1_TextBox_ErrorID').val('' + Error_ID + '');


            document.getElementById('<%=Button_Add.ClientID %>').click();
        }

<%--        $("#btncheck").click(function () {
            document.getElementById('<%=button_select.ClientID %>').click();
        });--%>
        //刪除時使用
        function deletes(ID, Error_ID, msg) {
            var r = confirm('確定要刪除嗎');
            if (r == true) {
                // alert(ID);
                $('#ContentPlaceHolder1_TextBox_num').val('' + ID + '');
                document.getElementById('<%=bt_del.ClientID %>').click();
            }
        }
        //更新時使用
        function updates(ID, Error_ID, Type) {
            remind.innerHTML = "修改內容";
            document.getElementById("ContentPlaceHolder1_TextBox_textTemp").value = '';
            document.getElementById("ContentPlaceHolder1_TextBox_ErrorID").value = '';
            $('#ContentPlaceHolder1_TextBox_textTemp').val('' + ID + '');
            $('#ContentPlaceHolder1_TextBox_ErrorID').val('' + Error_ID + '');
            document.getElementById('<%=Button_Update.ClientID %>').click();
        }

        function show_image(url, type) {


            //計算圖片大小的
            var img_url = url;

            // 创建对象
            var img = new Image();

            // 改变图片的src
            img.src = img_url

            var img_width;
            var img_height;

            // 判断是否有缓存
            if (img.complete) {
                // 打印
                img_width = img.width;
                img_height = img.height;
            } else {
                // 加载完成执行
                img.onload = function () {
                    // 打印
                    img_width = img.width;
                    img_height = img.height;
                }
            }
            //影片
            if (type == 'video') {
                if ($(window).width() < 768) {
                    document.getElementById('lbltipAddedComment').innerHTML = ' <video  width="100%" height="auto" src="' + url + '" controls="" href=\"javascript: void()\" ></video>';
                } else {
                    if (img_width > img_height) {
                        document.getElementById('lbltipAddedComment').innerHTML = ' <video  width="800px" height="450px" src="' + url + '" controls="" href=\"javascript: void()\" ></video>';
                    }
                    else {
                        document.getElementById('lbltipAddedComment').innerHTML = ' <video  width="450px" height="800px" src="' + url + '" controls="" href=\"javascript: void()\" ></video>';
                    }
                }
            }//圖片
            else {
                if ($(window).width() < 768) {
                    document.getElementById('lbltipAddedComment').innerHTML = ' <img src="' + url + '" alt="..." width="100%" height="auto" >';
                } else {
                    if (img_width > img_height) {
                        document.getElementById('lbltipAddedComment').innerHTML = ' <img src="' + url + '" alt="..." width="800px" height="450px" >';
                    }
                    else {
                        document.getElementById('lbltipAddedComment').innerHTML = ' <img src="' + url + '" alt="..." width="450px" height="800px" >';
                    }
                }
            }
        }
        function Click_Num(ID) {
            if (document.getElementById(ID).className == "fa fa-folder-open-o")
                document.getElementById(ID).className = "fa fa-folder-o";
            else
                document.getElementById(ID).className = "fa fa-folder-open-o";
        }
    </script>
    <!-----------------20200424留言板功能------------------------------------->
</asp:Content>
