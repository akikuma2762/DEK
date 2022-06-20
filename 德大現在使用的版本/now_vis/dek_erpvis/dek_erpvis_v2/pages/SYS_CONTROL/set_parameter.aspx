<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="set_parameter.aspx.cs" Inherits="dek_erpvis_v2.pages.SYS_CONTROL.set_parameter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=page_name %> | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <style>
        input[type="checkbox"] {
            width: 18px;
            height: 18px;
            cursor: auto;
            -webkit-appearance: default-button;
        }

        #date_str {
            width: 25%;
        }

        .Div_Shadow {
            box-shadow: 3px 3px 9px gray;
        }

        hr {
            display: block;
        }

        #textbox_companyname {
            width: 30%;
        }

        @media screen and (max-width:768px) {
            .Div_Shadow {
                box-shadow: none;
            }

            #_Title {
                font-size: 0.8em;
                padding: 0;
                overflow: hidden;
            }

            .form-group .col-xs-5 {
                line-height: 30px;
            }

            .form-group input {
                height: 30px;
            }

            #ContentPlaceHolder1_RadioButtonList1 {
                width: 100%;
            }

                #ContentPlaceHolder1_RadioButtonList1 tbody tr {
                    width: 30%;
                    display: inline-block;
                }

                    #ContentPlaceHolder1_RadioButtonList1 tbody tr td {
                        height: 32px;
                        line-height: 30px;
                    }

                        #ContentPlaceHolder1_RadioButtonList1 tbody tr td input {
                            margin-right: 5px;
                        }

                        #ContentPlaceHolder1_RadioButtonList1 tbody tr td label {
                            line-height: 30px;
                        }

            #ContentPlaceHolder1_Panel_people div.col-xs-12 {
                padding-left: 20px;
            }

                #ContentPlaceHolder1_Panel_people div.col-xs-12 > label, #ContentPlaceHolder1_Panel_people div.col-xs-12 label table {
                    width: 100%;
                }

                    #ContentPlaceHolder1_Panel_people div.col-xs-12 label table tbody tr {
                        width: 50%;
                        height: 40px !important;
                        display: inline-block;
                    }

                        #ContentPlaceHolder1_Panel_people div.col-xs-12 label table tbody tr td input {
                            margin-right: 5px;
                            line-height: 40px !important;
                        }

                        #ContentPlaceHolder1_Panel_people div.col-xs-12 label table tbody tr td label {
                            line-height: 40px !important;
                        }
            /*ContentPlaceHolder1_Panel_people*/
            #ContentPlaceHolder1_Panel_people div.col-xs-12 {
                padding-left: 10px;
            }

            #ContentPlaceHolder1_Panel_people .control-label {
                background-color: #337ab7;
                color: white;
                height: 30px;
                line-height: 30px;
                text-align: center;
            }

            hr {
                display: none;
            }

            #ContentPlaceHolder1_Panel_people .col-xs-12 label span {
                width: 40%;
                display: inline-block;
                margin-top: 10px;
            }

            #ContentPlaceHolder1_timeset {
                width: 60%;
                display: inline-block;
            }

            #textbox_companyname {
                width: 100% !important;
            }
            /*_SelectPage*/
            #ContentPlaceHolder1_SLS,
            #ContentPlaceHolder1_PCD,
            #ContentPlaceHolder1_WHE,
            #ContentPlaceHolder1_PMD,
            #ContentPlaceHolder1_CNC {
                width: 100%;
                margin: 10px 0 5px 0;
            }

            #ContentPlaceHolder1_checkBoxListSLSvisrd tbody tr,
            #ContentPlaceHolder1_checkBoxListPCDvisrd tbody tr,
            #ContentPlaceHolder1_checkBoxListWHEvisrd tbody tr,
            #ContentPlaceHolder1_checkBoxListPMDvisrd tbody tr,
            #ContentPlaceHolder1_checkBoxListCNCvisrd tbody tr {
                width: 50%;
                display: inline-block;
            }

                #ContentPlaceHolder1_checkBoxListSLSvisrd tbody tr td input,
                #ContentPlaceHolder1_checkBoxListPCDvisrd tbody tr td input,
                #ContentPlaceHolder1_checkBoxListWHEvisrd tbody tr td input,
                #ContentPlaceHolder1_checkBoxListPMDvisrd tbody tr td input,
                #ContentPlaceHolder1_checkBoxListCNCvisrd tbody tr td input {
                    margin-right: 5px;
                }
            /*safe*/
            #btncheck {
                width: 100%;
            }
            /*-----*/

        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-----------------title------------------>
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>&nbsp <%=page_name %></h3>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <!--跑版加入這個即可<div class="row"></div>-->
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div class="form-group">
                            <div class="row">
                                <div class="form-group ">
                                    <div class="col-md-2 col-xs-5">
                                        <label for="date_str">每月起算日</label>
                                    </div>
                                    <div class="col-md-3 col-xs-7">
                                        <input type="number" value="26" id="date_str" name="date_str" class="form-control col-12" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-md-2 col-xs-5">
                                        <label for="date_end">每月結算日</label>
                                    </div>
                                    <div class="col-md-3 col-xs-7">
                                        <input type="number" value="25" id="date_end" name="date_end" class="form-control col-12" runat="server" />
                                        <i id="cbx_remind"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-md-2 col-xs-5">
                                        <label>上傳個人照片</label>
                                    </div>
                                    <div class="col-md-10 col-xs-7">
                                        <asp:FileUpload ID="FileUpload_userimage" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--                        <div class="form-group">
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-md-2 col-xs-12">
                                        <label>變更刷新秒數<br>(設備監控面版用)</label>
                                    </div>
                                    <div class="col-md-10 col-xs-12">
                                        <asp:TextBox ID="TextBox_Time" CssClass="form-control" Text="" runat="server" placeholder="請輸入秒數" Style="width: 30%;" TextMode="Number"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>--%>

                        <asp:TextBox ID="TextBox_page" runat="server" style="display:none"></asp:TextBox>
                        <!--20191115開會提到要添加圖片上傳功能-->
                        <div class="form-group">
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-md-2 col-xs-12">
                                        <label>修改個人版面</label>
                                    </div>
                                    <div class="col-md-10 col-xs-12">
                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" CssClass="table-striped"
                                            OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged"
                                            AutoPostBack="True">
                                            <asp:ListItem Text="淡紫色" Value="custom"></asp:ListItem>
                                            <asp:ListItem Text="深綠色" Value="custom_old"></asp:ListItem>
                                            <asp:ListItem Text="自訂顏色" Value="custom_person"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:Button ID="jump" runat="server" Text="前往顏色設定" Visible="false" OnClick="jump_Click" class="btn btn-default" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-md-2 col-xs-12">
                                        <label>選擇顯示畫面</label>
                                    </div>
                                    <asp:Panel ID="Panel_people" runat="server">
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>

                        <hr />
                        <asp:Panel ID="Panel_foruser" runat="server" Visible="false">
                            <div class="form-group">
                                <div class="row">
                                    <div class="form-group">
                                        <div class="col-md-2 col-xs-5">
                                            <label>上傳公司圖片</label>
                                        </div>
                                        <div class="col-md-10 col-xs-7" style="padding: 0">
                                            <asp:FileUpload ID="FileUpload_companyimage" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="form-group">
                                <div class="row">
                                    <div class="form-group">
                                        <div class="col-md-2 col-xs-5">
                                            <label>更改公司名稱</label>
                                        </div>
                                        <div class="col-md-3 col-xs-7" style="padding: 0">
                                            <asp:TextBox ID="textbox_companyname" CssClass="form-control" Text="" runat="server" placeholder="請輸入貴公司名稱" onkeydown=KeyDown()></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <label id="_SelectPage">選擇所需頁面</label><p>
                                <!--20191115開會提到要添加能修改頁面名稱，以及選擇所需之頁面-->

                        </asp:Panel>
                        <asp:Button ID="button_select" runat="server" Text="保存" class="btn btn-secondary" OnClick="button_select_Click" Style="display: none" />
                        <button id="btncheck" type="button" class="btn btn-primary antosubmit2">保存</button>

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

        $("#btncheck").click(function () {
            var start_time = document.getElementsByName("ctl00$ContentPlaceHolder1$date_str")[0].value;
            var end_time = document.getElementsByName("ctl00$ContentPlaceHolder1$date_end")[0].value;

            if (start_time != "" && end_time != "") {
                var re = /^[0-9]+$/;
                //var re = /^([1-9]{1}|[12]\d|3[1-2])$/;
                if (!re.test(start_time) && !re.test(end_time)) {
                    var remind = document.getElementById("cbx_remind");
                    alert("只能輸入數字");
                } else {
                    if (parseInt(start_time) < 1 || parseInt(start_time) > 31 || parseInt(end_time) < 1 || parseInt(end_time) > 31) {
                        var remind = document.getElementById("cbx_remind");
                        alert("起算或結算日期需介於 1 ~ 31 天之間 !");
                    } else {
                        printname();
                        document.getElementById('<%=button_select.ClientID %>').click();
                    }

                }
            }
            else
                alert("請填入起始與結束日期!");
        });
        function printname() {
            //var name = document.getElementById('ContentPlaceHolder1_Orders').value;
            //name += ',' + document.getElementById('ContentPlaceHolder1_Orders').value;
            var pagename = '';
            <%=page_names %>
            $('#ContentPlaceHolder1_TextBox_page').val('' + pagename + '');
        }
        function KeyDown() {
            if (event.keyCode == 13) {
                try {
                    document.getElementById('<%=jump.ClientID%>').disabled = true;
                }
                catch {

                }
                var start_time = document.getElementsByName("ctl00$ContentPlaceHolder1$date_str")[0].value;
                var end_time = document.getElementsByName("ctl00$ContentPlaceHolder1$date_end")[0].value;

                if (start_time != "" && end_time != "") {
                    var re = /^[0-9]+$/;
                    //var re = /^([1-9]{1}|[12]\d|3[1-2])$/;
                    if (!re.test(start_time) && !re.test(end_time)) {
                        var remind = document.getElementById("cbx_remind");
                        alert("只能輸入數字");
                    } else {
                        if (parseInt(start_time) < 1 || parseInt(start_time) > 31 || parseInt(end_time) < 1 || parseInt(end_time) > 31) {
                            var remind = document.getElementById("cbx_remind");
                            alert("起算或結算日期需介於 1 ~ 31 天之間 !");
                        } else {
                            printname();
                            document.getElementById('<%=button_select.ClientID %>').click();
                    }

                }
            }
            else
                alert("請填入起始與結束日期!");
            }
        }

    </script>
</asp:Content>
