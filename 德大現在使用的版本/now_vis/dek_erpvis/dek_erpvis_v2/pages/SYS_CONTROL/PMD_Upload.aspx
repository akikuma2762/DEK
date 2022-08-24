<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="PMD_Upload.aspx.cs" Inherits="dek_erpvis_v2.pages.SYS_CONTROL.PMD_Upload1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>變更資料 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>

    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/UntradedCustomer.css" rel="stylesheet" />
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
        <br />
        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">

                        <div id="Change_DataTable"></div>

                        <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                                        </asp:ScriptManager>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                            <ContentTemplate>

                                                <div class="col-md-12 col-sm-12 col-xs-12 col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                        <span>廠區選擇</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                      
                                                                <asp:DropDownList ID="DropDownList_Type" AutoPostBack="true" CssClass="btn btn-default form-control" runat="server" OnSelectedIndexChanged="DropDownList_Type_SelectedIndexChanged">
                                                                    <asp:ListItem Value="Ver" Selected="True">立式廠</asp:ListItem>
                                                                    <asp:ListItem Value="Hor">臥式廠</asp:ListItem>
                                                                </asp:DropDownList>

                                                          
                                                    </div>
                                                </div>

                                                <div class="col-md-12 col-sm-12 col-xs-12 col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                        <span>產線選擇</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                      
                                                                <asp:DropDownList ID="DropDownList_Product" CssClass="btn btn-default form-control"  runat="server">
                                                                </asp:DropDownList>
                                                          
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>



                                        <div class="col-md-12 col-sm-12 col-xs-12 col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span style="margin-top: 3px; margin-bottom: 5px">排程關鍵字</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                               
                                                        <asp:TextBox ID="TextBox_keyWord" CssClass="form-control text-center"  runat="server"></asp:TextBox>
                                                    
                                            </div>
                                        </div>


                                        <div class="col-md-12 col-sm-12 col-xs-12 col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>組裝日期</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:TextBox ID="txt_str" runat="server" Style="" TextMode="Date" CssClass="form-control  text-center"></asp:TextBox>
                                            </div>
                                        </div>
                                         <div class="col-md-12 col-sm-12 col-xs-12 col-style">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span></span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <asp:TextBox ID="txt_end" runat="server" Style=""  TextMode="Date" CssClass="form-control  text-center"></asp:TextBox>
                                            </div>
                                    </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 col-style">
                                            <div class="col-md-12 col-xs-12 text-align-end">
                                            <asp:Button ID="button_select" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="button_select_Click" Style="display: none" />
                                            <button id="btncheck" type="button" class="btn btn-primary antosubmit2">執行搜索</button>
                                         </div>
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
    <div id="exampleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title modaltextstyle" id="myModalLabel2"><i class="fa fa-file-text"></i>編輯資料</h4>
                </div>
                <div class="modal-body">
                    <div id="testmodal2" style="padding: 5px 20px;">
                        <asp:Button ID="Button_Delete" runat="server" Text="Button" OnClick="Button_Delete_Click" Style="display: none" />
                        <asp:TextBox ID="TextBox_OrderNum" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="TextBox_Schedule" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="TextBox_WorkNumber" runat="server" Style="display: none"></asp:TextBox>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>組裝編號：</b><br />
                                        <asp:TextBox ID="TextBox_Order" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>排程編號：</b><br />
                                        <asp:TextBox ID="TextBox_Number" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>進度：</b><br />
                                        <asp:DropDownList ID="DropDownList_Percent" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>狀態：</b><br />
                                        <asp:DropDownList ID="DropDownList_Status" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>工作站名稱：</b><br />
                                        <asp:DropDownList ID="DropDownList_Work" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>組裝日：</b><br />
                                        <asp:TextBox ID="TextBox_Date" runat="server" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>實際組裝時間：</b><br />
                                        <asp:TextBox ID="TextBox_Truedate" runat="server" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id-"btn_Cancel" type="button" class="btn btn-default antoclose2" data-dismiss="modal">退出</button>
                    <asp:Button ID="Button_Save" runat="server" Text="Button" OnClick="Button_Save_Click" Style="display: none" />
                    <button id="btnSave" type="button" class="btn btn-primary antosubmit2">儲存</button>
                </div>
            </div>
        </div>
    </div>


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
    <script src="../../assets/vendors/Create_HtmlCode/HtmlCode20211210.js"></script>
    <script>

        function Set_Value(Order, Number, Percent, Status, WorkNumber, Date, TrueDate) {
            $('#ContentPlaceHolder1_TextBox_Order').val('' + Order + '');
            $('#ContentPlaceHolder1_TextBox_Number').val('' + Number + '');
            document.getElementById('ContentPlaceHolder1_DropDownList_Percent').value = Percent + '%';
            document.getElementById('ContentPlaceHolder1_DropDownList_Status').value = Status;
            document.getElementById('ContentPlaceHolder1_DropDownList_Work').value = WorkNumber;

            $('#ContentPlaceHolder1_TextBox_OrderNum').val('' + Order + '');
            $('#ContentPlaceHolder1_TextBox_Schedule').val('' + Number + '');
            $('#ContentPlaceHolder1_TextBox_WorkNumber').val('' + WorkNumber + '');

            $('#ContentPlaceHolder1_TextBox_Date').val('' + Date + '');
            $('#ContentPlaceHolder1_TextBox_Truedate').val('' + TrueDate + '');
        }
        //刪除
        function Delete_Value(Order, Number, WorkNumber) {
            $('#ContentPlaceHolder1_TextBox_OrderNum').val('' + Order + '');
            $('#ContentPlaceHolder1_TextBox_Schedule').val('' + Number + '');
            $('#ContentPlaceHolder1_TextBox_WorkNumber').val('' + WorkNumber + '');

            answer = confirm("您確定要刪除嗎??");
            if (answer) {
                document.getElementById('<%=Button_Delete.ClientID %>').click();
            }
        }
        //確認修改鈕
        $("#btnSave").click(function () {

            var WhatSystem = navigator.userAgent;
            if (WhatSystem.match(/(iphone|ipad|ipod);?/i)) {
            } else {
                $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                document.querySelector(".blockUI.blockMsg.blockPage").style.zIndex = 10000;
                document.getElementById('btnSave').disabled = true;
                document.getElementById('btn_Cancel').disabled = true;
            }
            //20220824 需求:新增刪頁面不跳轉,改使用AJAX傳輸資料,停用aps.net button元件
            //document.getElementById('<%=Button_Save.ClientID %>').click();  
            var data = get_Item_Data();
            data["Act_Type"] = "Update";
            data = JSON.stringify(data);
            console.log(data)
            PostData_Ajax(data);
            
        });

        //執行搜索
        $("#btncheck").click(function () {

            var WhatSystem = navigator.userAgent;
            if (WhatSystem.match(/(iphone|ipad|ipod);?/i)) {
            } else {
                $.blockUI({ message: '<img src="../../images/loading.gif" />' });

            }

            document.getElementById('<%=button_select.ClientID %>').click();
        });

        //防止切換頁籤時跑版
        $(document).ready(function () {
            //$('#example').DataTable({
            //    responsive: true
            //});
            //$('#exampleInTab').DataTable({
            //    responsive: true
            //});

            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                $($.fn.dataTable.tables(true)).DataTable()
                    .columns.adjust();
            });
        });

        function PostData_Ajax(data) {
            $.ajax({
                type: "post",
                contentType: "application/json",
                url: "PMD_Upload.aspx/PostData",
                data: "{_data:'" + data + "'}",
                dataType: "json",
                success: function (result) {
                    console.log(result.d["status"]);
                    if (result.d["status"] == "失敗") {
                        alert("修改失敗");
                    } else if(result.d["status"] == "成功") {
                        console.log(result.d["th"]);
                        create_tablehtmlcode('Change_DataTable', '變更資料', 'table-form', result.d["th"], result.d["tr"]);

                        stateSave_Table('#table-form');
                        console.log("成功");
                    }
                    //關閉loading視窗及修改視窗
                    $('#exampleModal').click();
                    $.unblockUI();
                    $(".blockUI").fadeOut("slow");
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#exampleModal').click();
                    $.unblockUI();
                    $(".blockUI").fadeOut("slow");
                    alert("資料傳輸錯誤,請檢查資料傳遞格式!!");
                    //alert(XMLHttpRequest.status);
                    //alert(XMLHttpRequest.readyState);
                    //alert(textStatus);
                }
                , complete: function (jqXHR) {
                    document.getElementById('btnSave').disabled = false;
                    document.getElementById('btn_Cancel').disabled = false;
                }
            })

        }

        function get_Item_Data() {
            var TextBox_Order = $('#ContentPlaceHolder1_TextBox_Order').val();
            var TextBox_Number = $('#ContentPlaceHolder1_TextBox_Number').val();
            var DropDownList_Percent = $("#ContentPlaceHolder1_DropDownList_Percent").val();
            var DropDownList_Status = $("#ContentPlaceHolder1_DropDownList_Status").val();
            var DropDownList_Work = $("#ContentPlaceHolder1_DropDownList_Work").val();
            var TextBox_OrderNum = $('#ContentPlaceHolder1_TextBox_OrderNum').val();
            var TextBox_Schedule = $('#ContentPlaceHolder1_TextBox_Schedule').val();
            var TextBox_WorkNumber = $('#ContentPlaceHolder1_TextBox_WorkNumber').val();
            var TextBox_Date = $('#ContentPlaceHolder1_TextBox_Date').val();
            var TextBox_Truedate = $('#ContentPlaceHolder1_TextBox_Truedate').val();
            var Link = $('#ContentPlaceHolder1_DropDownList_Type').val();
            var txt_str = $('#ContentPlaceHolder1_txt_str').val();
            var txt_end = $('#ContentPlaceHolder1_txt_end').val();
            var Act_Type = "Update";
            var data = {
                "TextBox_Order": `${TextBox_Order}`, "TextBox_Number": `${TextBox_Number}`,
                "DropDownList_Percent": `${DropDownList_Percent}`, "DropDownList_Status": `${DropDownList_Status}`,
                "DropDownList_Work": `${DropDownList_Work}`,
                "TextBox_OrderNum": `${TextBox_OrderNum}`, "TextBox_Schedule": `${TextBox_Schedule}`,
                "TextBox_WorkNumber": `${TextBox_WorkNumber}`, "TextBox_Date": `${TextBox_Date}`,
                "TextBox_Truedate": `${TextBox_Truedate}`, "Link": `${Link}`,
                "txt_str": `${txt_str}`, "txt_end": `${txt_end}`, "Act_Type": `${Act_Type}`
            };
            console.log("初始", data);
            return data;
        }



        //$('#Change_DataTable').dataTable(
        //    {
        //        destroy: true,
        //        language: {
        //            "processing": "處理中...",
        //            "loadingRecords": "載入中...",
        //            "lengthMenu": "顯示 _MENU_ 項結果",
        //            "zeroRecords": "沒有符合的結果",
        //            "info": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
        //            "infoEmpty": "顯示第 0 至 0 項結果，共 0 項",
        //            "infoFiltered": "(從 _MAX_ 項結果中過濾)",
        //            "infoPostFix": "",
        //            "search": "搜尋:",
        //            "paginate": {
        //                "first": "第一頁",
        //                "previous": "上一頁",
        //                "next": "下一頁",
        //                "last": "最後一頁"

        //            }
        //        },


        //        scrollCollapse: true,
        //        "order": [[2, "asc"], [3, "asc"], [4, "asc"], [5, "asc"]],
        //        dom: "<'row'<'pull-left'f>'row'<'col-sm-3'>'row'<'col-sm-3'B>'row'<'pull-right'l>>" +
        //            "<rt>" +
        //            "<'row'<'pull-left'i>'row'<'col-sm-4'>row'<'col-sm-3'>'row'<'pull-right'p>>",

        //        buttons: [
        //            'copy', 'csv', 'excel', 'pdf', 'print'
        //        ],

        //    });
        //jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
        create_tablehtmlcode('Change_DataTable', '變更資料', 'table-form', '<%=th.ToString() %>',"<%=tr.ToString()%>");
        //產生相對應的JScode
        set_Table('#table-form');
        //防止頁籤跑版
        loadpage('', '');

        


    </script>
</asp:Content>
