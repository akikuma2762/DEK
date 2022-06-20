<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="rights_mag_details.aspx.cs" Inherits="dek_erpvis_v2.pages.SYS_CONTROL.rights_mag_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <title><%=selected_user_name %>權限管理 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <style>
        .Div_Shadow {
            box-shadow: 3px 3px 9px gray;
        }

        @media screen and (max-width:768px) {
            #Content2 {
                overflow: hidden;
            }

            .Div_Shadow {
                box-shadow: none;
            }

            .x_content {
                padding: 0;
            }

                .x_content > .col-xs-12 {
                    padding: 0;
                }

            ._TopNav li {
                display: inline-block;
            }

            #ContentPlaceHolder1_checkBoxListSLS,
            #ContentPlaceHolder1_checkBoxListPCD,
            #ContentPlaceHolder1_checkBoxListWHE,
            #ContentPlaceHolder1_checkBoxListPMD,
            #ContentPlaceHolder1_checkBoxListCNC {
                font-size: 0.7em;
            }

            #ContentPlaceHolder1_Panel_contr label.control-label {
                width: 100%;
                padding: 5px 5px 5px 10px;
                background-color: #26B99A;
                color: white;
            }

            #ContentPlaceHolder1_checkBoxListSLS tbody tr,
            #ContentPlaceHolder1_checkBoxListPCD tbody tr,
            #ContentPlaceHolder1_checkBoxListWHE tbody tr,
            #ContentPlaceHolder1_checkBoxListPMD tbody tr,
            #ContentPlaceHolder1_checkBoxListCNC tbody tr {
                width: 50%;
                display: inline-block;
                margin: 5px 0;
            }

                #ContentPlaceHolder1_checkBoxListSLS tbody tr .switchery,
                #ContentPlaceHolder1_checkBoxListPCD tbody tr .switchery,
                #ContentPlaceHolder1_checkBoxListWHE tbody tr .switchery,
                #ContentPlaceHolder1_checkBoxListPMD tbody tr .switchery,
                #ContentPlaceHolder1_checkBoxListCNC tbody tr .switchery {
                    margin-right: 5px;
                }
            /*safe*/
            #ContentPlaceHolder1_Button_right {
                width: 100%;
            }
            /*資料維護*/
            ._info_update {
                font-size: 0.7em;
            }

            #info_update label {
                width: 40%;
                line-height: 30px;
            }

            #info_update input {
                width: 55%;
                display: inline-block;
                margin: 0 0 5px 0;
            }
            /*生日*/
            ._birthday > div.row {
                width: 55%;
                display: inline-block;
                height: 34px;
                margin: 0;
            }

            #info_update ._birthday {
                height: 39px;
                line-height: 39px;
            }

                #info_update ._birthday > label {
                    height: 39px;
                    line-height: 39px;
                }

            ._birthday > div.row .xdisplay_inputx {
                padding: 0;
            }

            ._birthday > div.row input {
                width: 100% !important;
                height: 34px;
                line-height: 34px;
            }

            #ContentPlaceHolder1_DropDownList_dmt {
                width: 55%;
                display: inline-block;
                height: 34px;
                margin: 0;
            }
            /*原密碼*/
            #ContentPlaceHolder1_Panel1 {
                display: none;
                /*height:70px;
                overflow:hidden;
                text-overflow: ellipsis;*/
            }
            /*#ContentPlaceHolder1_Panel1 #ContentPlaceHolder1_Label_psd {
                font-size:0.7em;
            }*/
            #btninfo, #btncancel {
                width: 46%;
                display: inline-block;
                margin: 0 1%;
            }
            /*變更密碼*/
            #password_update {
                font-size: 0.7em;
                padding-left: 10px;
                overflow: hidden;
            }

                #password_update > div.row > div.col-xs-12 {
                    padding: 0;
                }

                #password_update div.col-12 {
                    margin-bottom: 5px;
                }

                #password_update label {
                    width: 30%;
                    display: inline-block;
                }

                #password_update input {
                    width: 65%;
                    display: inline-block;
                }

            ._CheckPass label {
                width: 45%;
                display: inline-block;
            }

            ._CheckPass > div {
                width: 55%;
                display: inline-block;
                margin: 0;
            }

                ._CheckPass > div > div {
                    padding: 0;
                }

            #password_update ._VerificationCode input {
                width: 100%;
            }

            ._CheckPass > div.row {
                width: 65%;
            }

            #btnpassword, #btnCancel {
                width: 47%;
            }
        }

        input[type="checkbox"] {
            width: 18px;
            height: 18px;
            cursor: auto;
            -webkit-appearance: default-button;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main" onkeydown="if(event.keyCode==13) return false;">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>檔案管理表單</h3>
                </div>
                <div class="title_right">
                </div>
            </div>
            <div class="clearfix"></div>
            <!-----------------content------------------>
            <div class="row">
                <div class="x_panel Div_Shadow">
                    <div class="x_title">
                        <h3><%=selected_user_name %> </h3>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <div class="col-md-3 col-sm-3 col-xs-12 _TopNav">
                            <ul class="nav nav-tabs tabs-left">
                                <li class="active"><a href="#info_view" data-toggle="tab" ><strong>權限設定</strong></a></li>
                                <li><a href="#info_update" data-toggle="tab">資料維護</a></li>
                                <li><a href="#password_update" data-toggle="tab">變更密碼</a></li>
                            </ul>
                        </div>
                        <div class="col-md-9 col-sm-9 col-xs-12">
                            <!-- Tab panes -->
                            <div class="tab-content">
                                <div class="tab-pane active" id="info_view">
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <p class="lead">
                                                <label class="control-label ">權限設定</label>
                                            </p>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <asp:Panel ID="Panel_contr" runat="server">
                                            </asp:Panel>
                                        </div>
                                        <hr />
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <p class="lead">
                                                <label class="control-label ">高等權限設定</label>
                                            </p>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <asp:CheckBoxList ID="CheckBoxList_Power" runat="server"></asp:CheckBoxList>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <asp:Button ID="Button_right" runat="server" class="btn btn-success" Text="提交表單" OnClick="Button_right_Click" />
                                        </div>
                                    </div>
                                    <br />
                                </div>
                                <div class="tab-pane" id="info_update">
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <p class="lead">
                                                <label>資料維護</label>
                                            </p>
                                            <div class="_info_update">
                                                <div class="col-12">
                                                    <label for="fullname">姓名Full Name * :</label>
                                                    <asp:TextBox ID="TextBox_fullname" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-12">
                                                    <label for="fullname">手機Phone Number * :</label>
                                                    <asp:TextBox ID="TextBox_num" runat="server" class="form-control" placeholder=" ex. 0911234567"></asp:TextBox>
                                                </div>
                                                <div class="col-12 _birthday">
                                                    <label for="heard">生日Birthday * :</label>
                                                    <asp:TextBox ID="TextBox_Birth" runat="server"  TextMode="Date" class="form-control"></asp:TextBox>

                                                    <%--<div class="row">
                                                        <fieldset>
                                                            <div class="control-group">
                                                                <div class="controls">
                                                                    <div class="col-md-12 col-sm-12 col-xs-12 xdisplay_inputx form-group has-feedback">
                                                                        <input type="text" class="form-control has-feedback-left" id="TextBox_birthday" name="TextBox_birthday" runat="server" value="" placeholder="ex. yyyyMMdd" aria-describedby="inputSuccess2Status1" autocomplete="off">
                                                                        <span class="fa fa-calendar-o form-control-feedback left" aria-hidden="true"></span>
                                                                        <span id="inputSuccess2Status1" class="sr-only">(success)</span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                    </div>--%>
                                                </div>
                                                <div class="col-12">
                                                    <label for="email">電郵Email * :</label>
                                                    <asp:TextBox ID="TextBox_email" runat="server" class="form-control" placeholder=" ex. ABC123@deta.com" TextMode="Email"></asp:TextBox>
                                                </div>
                                                <div class="col-12">
                                                    <label for="heard">部門Department * :</label>
                                                    <asp:DropDownList ID="DropDownList_dmt" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-12">
                                                    <asp:Panel ID="Panel1" runat="server" Visible="False">
                                                        <label for="psd">原密碼 :</label>
                                                        <asp:Label ID="Label_psd" runat="server" Text="Label"></asp:Label>
                                                    </asp:Panel>
                                                    <div class=" row">
                                                        <p><i id="info_remind"></i></p>
                                                    </div>
                                                </div>
                                                <br />
                                                <asp:Button ID="Button_info" runat="server" Style="display: none;" OnClick="Button_info_Click" />
                                                <button id="btninfo" class="btn btn-success" type="button">提交表單</button>
                                                <button id="btncancel" class="btn btn-default" type="button">取消操作</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane" id="password_update">
                                    <div class="row">
                                        <div class="col-md-6 col-sm-8 col-xs-12">
                                            <p class="lead">
                                                <label>變更密碼</label>
                                            </p>
                                            <div class="_password_update">
                                                <div class="col-12">
                                                    <label for="password1">新密碼  * :</label>
                                                    <asp:TextBox ID="TextBox_pwd1" class="form-control" runat="server" placeholder=" New Password" TextMode="Password"></asp:TextBox>
                                                </div>
                                                <div class="col-12">
                                                    <label for="password2">再次輸入密碼 * :</label>
                                                    <asp:TextBox ID="TextBox_pwd2" class="form-control" runat="server" placeholder=" New Password again" TextMode="Password"></asp:TextBox>
                                                </div>
                                                <div class="col-12 _CheckPass">
                                                    <label for="code" class="">驗證碼 * :</label>
                                                    <div class="row">
                                                        <div class="col-md-4 col-sm-4 col-xs-4 ">
                                                            <asp:Label ID="Label_code" runat="server" Text="" class="form-control text-center"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8 col-sm-8 col-xs-8 _VerificationCode">
                                                            <asp:TextBox ID="TextBox_code" runat="server" placeholder="*請輸入左側驗證碼" class="form-control" autocomplete="off"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                            </div>
                                        </div>
                                        <div class=" row">
                                            <p><i id="pwd_remind"></i></p>
                                        </div>
                                        <asp:Button ID="Button_pwd" runat="server" Style="display: none;" OnClick="Button_pwd_Click" />
                                        <button id="btnpassword" class="btn btn-success" type="button">提交表單</button>
                                        <button id="btnCancel" class="btn btn-default" type="button">取消操作</button>
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
    <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
    <script>
        //  $("input:checkbox").addClass('js-switch');
        $("#btninfo").click(function () {
            var fullname = document.getElementById("ContentPlaceHolder1_TextBox_fullname").value;
            //var birthday = document.getElementById("ContentPlaceHolder1_TextBox_birthday").value;
            var number = document.getElementById("ContentPlaceHolder1_TextBox_num").value;
            var email = document.getElementById("ContentPlaceHolder1_TextBox_email").value;
            var e = document.getElementById("ContentPlaceHolder1_DropDownList_dmt");
            var dpm = e.options[e.selectedIndex].value;
            var remind = document.getElementById("info_remind");
            var re = /^[0-9]+$/; remind.style.color = "#FF3333";
            if (fullname != "") {
                if (re.test(number) && number.length == 10) {
                    if (email != "") {
                        if (dpm != "NULL") {
                            document.getElementById('<%=Button_info.ClientID %>').click();
                        } else {
                            remind.innerHTML = "* 部門Department,不得為空!";
                        }
                    } else {
                        remind.innerHTML = "* 電郵Email,不得為空!";
                    }
                } else {
                    remind.innerHTML = "* 手機Phone Number,有誤!請重新檢查!";
                }
            } else {
                remind.innerHTML = "* 姓名Full Name,不得為空!";
            }
        });
        $("#btnpassword").click(function () {
            var pwd1 = document.getElementById("ContentPlaceHolder1_TextBox_pwd1").value;
            var pwd2 = document.getElementById("ContentPlaceHolder1_TextBox_pwd2").value;
            var code = document.getElementById("ContentPlaceHolder1_TextBox_code").value;
            var label = document.getElementById("ContentPlaceHolder1_Label_code").value;
            var remind = document.getElementById("pwd_remind");
            remind.style.color = "#FF3333";
            if (pwd1 != "" && pwd2 != "") {
                if (pwd1 == pwd2) {
                    if (code != "") {
                        if (code != label) {
                            document.getElementById('<%=Button_pwd.ClientID %>').click();
                        } else {
                            remind.innerHTML = "* 驗證碼錯誤,請重新檢查!";
                        }
                    } else {
                        remind.innerHTML = "* 驗證碼,不得為空!";
                    }
                } else {
                    remind.innerHTML = "* 您的新密碼不一致!";
                }
            } else {
                remind.innerHTML = "* 新密碼,不得為空!";
            }
        });
        $(function () {
            $('#ContentPlaceHolder1_TextBox_birthday').daterangepicker({
                singleDatePicker: true,
                autoUpdateInput: false,
                singleClasses: "picker_3",
                locale: {
                    format: 'YYYYMMDD',
                    cancelLabel: 'Clear',
                    daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
                    monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
                }
            });
            $('#ContentPlaceHolder1_TextBox_birthday').on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format('YYYYMMDD'));
            });
        });
    </script>
</asp:Content>
