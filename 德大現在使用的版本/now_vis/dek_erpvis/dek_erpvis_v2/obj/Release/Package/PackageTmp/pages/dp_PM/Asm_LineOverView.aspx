<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Asm_LineOverView.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PD.Asm_LineOverView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=LineName %> 整廠進度管理看板 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_PM/Asm_LineOverView.css" rel="stylesheet" />
    <style>
        #testmodal2 {
            padding: 5px 20px;
        }
         #ContentPlaceHolder1_RadioButtonList_select_type>tbody>tr>td>span>label {
                font-size:3rem;
                margin-top:5px;
            }
        @media screen and (max-width:768px) {
            #myModalLabel2 i {
                margin-right: 10px;
            }

            #testmodal2 {
                padding: 0;
            }

                #testmodal2 div.form-group h5 {
                    width: 35%;
                    display: inline-block;
                }

                #testmodal2 div.form-group div,
                #testmodal2 div.form-group select,
                #testmodal2 div.form-group fieldset {
                    width: 63%;
                    display: inline-block;
                }

                    #testmodal2 div.form-group:last-child fieldset,
                    #testmodal2 div.form-group fieldset select {
                        width: 100%;
                    }

                #testmodal2 div.form-group fieldset {
                    height: 45px;
                    line-height: 45px;
                }

            #ContentPlaceHolder1_RadioButtonList_select_type {
                width: 100%;
            }

            #testmodal2 div.form-group fieldset tbody tr {
                width: 32%;
                display: inline-block;
            }

                #testmodal2 div.form-group fieldset tbody tr input {
                    margin-right: 5px;
                }

            .modal-footer .btn-primary, #btncheck {
                width: 47%;
                display: inline-block;
            }

            #exampleModal .modal-body {
                padding-bottom: 0;
            }

            #exampleModal .modal-footer {
                width: 100%;
                padding: 10px;
                margin-bottom: 10px;
            }
        }


          /*手機*/
        @media screen and (max-width: 765px) {

            .tooltip-inner {
                max-width: 200px;
                /* If max-width does not work, try using width instead */
                width: 200px;
                 filter: alpha(opacity=0);
            }

  
            .tooltip.in{opacity:1!important; }
        }
        /*電腦*/
        @media screen and (min-width: 765px) {

            .tooltip-inner {
                max-width: 250px;
                /* If max-width does not work, try using width instead */
                width: 250px;
				        font-size: 18px;
                         filter: alpha(opacity=0);
            }

            .tooltip.in{opacity:1!important;}
        }
		
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁</a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PMD">生產部</a></u></li>
            <li><u><a href="Asm_LineTotalView.aspx">整廠進度管理看板</a></u></li>
        </ol>
        <!-----------------title------------------>
        <div class="row tile_time">
            <h1 class="text-center _mdTitle" style="width: 100%; margin-bottom: 15px"><b>產線名稱：<% =LineName%></b></h1>
            <h3 class="text-center _xsTitle" style="width: 100%; margin-bottom: 15px"><b>產線名稱：<% =LineName%></b></h3>
        </div>
        <asp:Button ID="Button_Jump" runat="server" Text="Button" OnClick="Button_Jump_Click" Style="display: none" />
        <asp:TextBox ID="TextBox_textTemp" runat="server" Visible="true" Width="80px" Style="display: none"></asp:TextBox>
        <!-- top tiles -->

        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">

                    <div class="x_content">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="x_panel">
                                <div class="row tile_count" style="margin-top: 0px; margin-bottom: -20px">
                                    <div class="col-md-offset-4 col-md-1 col-sm-3 col-xs-6 tile_stats_count">
                                        <span class="count_top"><i class="fa fa-clock-o"></i>今日在線</span>
                                        <div class="count blue"><%=OnLinePiece %><span style="height: 10px"><%=PieceUnit%></span></div>
                                        <!--<span class="count_bottom"><i class="green"><i class="fa fa-sort-asc"></i>0% </i>survive</span>-->
                                    </div>
                                    <div class="col-md-1 col-sm-3 col-xs-6 tile_stats_count">
                                        <span class="count_top"><i class="fa fa-clock-o"></i>今日完成</span>
                                        <div class="count green"><% =FinishPiece%><span style="height: 10px"><%=PieceUnit%></span></div>
                                        <!--<span class="count_bottom"><i class="green"><i class="fa fa-sort-asc"></i>0% </i>survive</span>-->
                                    </div>
                                    <div class="col-md-1 col-sm-3 col-xs-6 tile_stats_count">
                                        <span class="count_top"><i class="fa fa-clock-o"></i>今日異常</span>
                                        <div class="count red"><%=alarm_total %><span style="height: 10px"><%=PieceUnit%></span></div>
                                        <!--<span class="count_bottom"><i class="red"><i class="fa fa-sort-asc"></i>0% </i>survive</span>-->
                                    </div>
                                    <div class="col-md-1 col-sm-3 col-xs-6 tile_stats_count">
                                        <span class="count_top"><i class="fa fa-clock-o"></i>當下落後</span>
                                        <div class="count black"><%=behind %><span style="height: 10px"><%=PieceUnit%></span></div>
                                    </div>
                                </div>
                            </div>
                            <div class="x_panel">
                                <table id="TB" class="table table-bordered" border="1" cellspacing="0" style="width: 100%">
                                    <tr id="tr_row">
                                        <%=ColumnsData%>
                                    </tr>
                                    <tbody>
                                        <%=RowsData%>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-----------------/content------------------>
        <!-- Modal -->
        <div id="exampleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title" id="myModalLabel2"><i class="fa fa-file-text"></i>狀態變更精靈</h4>
                    </div>
                    <div class="modal-body">
                        <div id="testmodal2">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <div class="form-group">
                                        <h3>
                                            <i class="fa fa-caret-down">排程編號</i>
                                        </h3>
                                        <div class="btn-group btn-group-justified" data-toggle="buttons">
                                            <asp:TextBox ID="TextBox_Number" runat="server" ReadOnly="true" Width="100%"></asp:TextBox>
                                            <asp:TextBox ID="TextBox_show" runat="server" Style="display: none" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <div class="form-group">
                                        <h3>
                                            <i class="fa fa-caret-down">派工人員:</i>
                                        </h3>
                                        <fieldset>
                                            <asp:TextBox ID="TextBox_Dispatch" runat="server" Style="display: none" Width="100%"></asp:TextBox>
                                            <div id ="selection"></div>
                                        </fieldset>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="col-md-4 col-sm-12 col-xs-12">
                                    <div class="form-group">
                                        <h3>
                                            <i class="fa fa-caret-down">進度選擇:</i>
                                        </h3>
                                        <fieldset>
                                            <asp:DropDownList ID="DropDownList_progress" runat="server"></asp:DropDownList>
                                        </fieldset>
                                    </div>                                 
                                    <div class="form-group">
                                        <h3>
                                            <i class="fa fa-caret-down">狀態選擇:</i>
                                        </h3>

                                        <asp:RadioButtonList ID="RadioButtonList_select_type" runat="server" RepeatDirection="Vertical">
                                            <asp:ListItem Value="0" Selected="True" style="color: deepskyblue">啟動</asp:ListItem>
                                            <asp:ListItem Value="1" style="color: red">暫停</asp:ListItem>
                                            <asp:ListItem Value="3" style="color: black">跑合</asp:ListItem>
                                            <asp:ListItem Value="2" style="color: green">完成</asp:ListItem>
                                        </asp:RadioButtonList>

                                    </div>
                                </div>
                                <div class="col-md-8 col-sm-12 col-xs-12">
                                    <h3>
                                        <i class="fa fa-caret-down">問題回報:</i>
                                    </h3>
                                    <asp:TextBox ID="TextBox_Report" runat="server" TextMode="MultiLine" Style="resize: none; width: 100%; height: 165px"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">取消</button>
                        <button id="btncheck" type="button" class="btn btn-success">送出</button>
                        <asp:Button ID="button_select" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="button_select_Click" Style="display: none" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- /Modal -->

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
        function ChangeStatus(X, percent) {
            var key = document.getElementsByName("ctl00$ContentPlaceHolder1$Text2")[0].value = X;//Text_Story  
        };

        function SetValue(number, percent, Report,error_str,dispatch) {

            $('#ContentPlaceHolder1_TextBox_Number').val('' + number + '');
            $('#ContentPlaceHolder1_TextBox_show').val('' + number + '');
            $('#ContentPlaceHolder1_TextBox_Report').val('' + Report.replaceAll("^", "'").replaceAll('#', '"').replaceAll("$", " ").replaceAll('@', '\r\n') + '');
            $("#ssss option").each(function (index) {

                if ($(this).text() == dispatch) {
                    console.log($(this).text(), dispatch, index);
                    $("#ssss")[0].selectedIndex = index;
                    console.log($("#ssss"));
                }
            })
            var error_ary = error_str.split("/");
            top["err_count"] = error_ary[0];
            console.log(error_ary);
            selectElement('ContentPlaceHolder1_DropDownList_progress', percent);
            checkpower();
        }
        //防止切換頁籤時跑版
        $(document).ready(function () {
            var factory = <%=Factory_Json%>;
            var dispatch = JSON.parse('<%=Dispatch_Json%>');
            console.log(factory, dispatch);
            SetSelectGroup(factory, dispatch, "ssss");
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
            //20221005 新增判斷網路導致資料讀取異常
            var _Data = '<%=_Data%>';
            if (_Data.indexOf("null") != -1) alert('伺服器回應 : 無法載入資料,請聯絡德科人員或檢查您的網路連線。');
        });


        function selectElement(id, valueToSelect) {
            let element = document.getElementById(id);
            element.value = valueToSelect;
        }
        $("#btncheck").click(function () {

            //20221128新增前端判斷異常數量 status =2 為完成
            if (top["err_count"] != 0 && top["status"]==2) {
              
                
                alert("異常數量不為零!,無法完成!");
            } else {

                var WhatSystem = navigator.userAgent;
                if (WhatSystem.match(/(iphone|ipad|ipod);?/i)) {
                } else {
                    $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                    document.querySelector(".blockUI.blockMsg.blockPage").style.zIndex = 1000000;
                    document.getElementById('btncheck').disabled = true;
                }
                var val = $("#selection option:selected").val();
                console.log(val);
                $('#ContentPlaceHolder1_TextBox_Dispatch').val(val);
                console.log($('#ContentPlaceHolder1_TextBox_Dispatch').val());
                document.getElementById('<%=button_select.ClientID %>').click();

            }
           
        });
        function jump_Asm_ErrorDetail(paramer) {

            var WhatSystem = navigator.userAgent;
            if (WhatSystem.match(/(iphone|ipad|ipod);?/i)) {
            } else {
                $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                $.unblockUI();
            }

            $('#ContentPlaceHolder1_TextBox_textTemp').val('' + paramer + '');
            document.getElementById('<%=Button_Jump.ClientID %>').click();
        }
        function checkpower() {
            var power = '<%=power %>';
            if (power == 'PMD') {
                document.getElementById('btncheck').style.visibility = 'visible';
            }
            else {
                document.getElementById('btncheck').style.visibility = 'hidden';
            }
        }
        //20221005查看 table資料是否正常
        top['xData'] = '<%=X_Data%>';

        //20221130新增 判斷modal點擊哪種狀態
        $("#ContentPlaceHolder1_RadioButtonList_select_type input").click(function () {
            top["status"] = $(this).val();
            console.log($(this).val());
        });

        //20221221 新增select群組生成
        function SetSelectGroup(factory, dispatch,id) {
            var htmlText = "";
            var f_Belong_Factory;
            var w_Belong_Factory;
            htmlText = `<select id="${id}" style=\"width:100%;\">`;
            for (var i = 0; i < factory.length; i++)
            {
                f_Belong_Factory = factory[i]["Belong_Factory"];
                htmlText += `<optgroup label="${f_Belong_Factory}">`
                for (var j = 0; j < dispatch.length; j++)
                {
                    w_Belong_Factory = dispatch[j].Belong_Factory;
                    if (w_Belong_Factory == f_Belong_Factory)
                    {
                        var user_id = dispatch[j].USER_ID;
                        var user_name = dispatch[j].USER_Name;
                        htmlText += `<option value="${user_id}">${user_name}</option>`
                    }
                }
                htmlText += `</optgroup >`
            }
            htmlText += "</select>";

            $("#selection").html(htmlText);
        }
        $("#selection").change(function () {
            console.log($("#selection option:selected").val());
            var val = $("#selection option:selected").val();
            $('#ContentPlaceHolder1_TextBox_Dispatch').val(val);
            $('#ContentPlaceHolder1_TextBox_Dispatch').text(val);
            console.log($('#ContentPlaceHolder1_TextBox_Dispatch').text());

        });


    </script>
</asp:Content>
