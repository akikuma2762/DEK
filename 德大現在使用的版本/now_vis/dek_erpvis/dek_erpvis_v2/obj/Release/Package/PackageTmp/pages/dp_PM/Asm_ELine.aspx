<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Asm_ELine.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PM.Asm_ELine" %>

<%--<%@ OutputCache duration="10" varybyparam="None" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>電控盒產線 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_PM/Asm_LineOverView.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .tooltip-inner {
            max-width: 150px;
            /* If max-width does not work, try using width instead */
            width: 150px;
        }

        input[type="radio"] {
            width: 20px;
            height: 20px;
            cursor: auto;
            -webkit-appearance: default-button;
        }
    </style>
    <div class="right_col" role="main">
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁</a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PMD">生產部</a></u></li>
        </ol>
        <!-----------------title------------------>
        <br />
        <!-----------------/title------------------>

        <!-----------------content------------------>
        <!--整體表的顯示-->
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_title">
                        <button type="button" class="btn btn-primary" style="position: absolute; right: 0; font-size: 20px" data-toggle="modal" data-target="#exampleModalAdd">新增插單</button>
                        <h1 class="text-center _mdTitle" style="width: 100%"><b>電控盒產線</b></h1>
                        <h3 class="text-center _xsTitle" style="width: 100%"><b>電控盒產線</b></h3>

                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="x_panel">

                                <table id="TB" class="table table-ts table-bordered nowrap" cellspacing="0" width="100%">
                                    <thead>
                                        <tr id="tr_row">
                                            <%=th%>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%= tr %>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>



    <!--Model-->
    <!--現場人員報工-->
    <div id="exampleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title modaltextstyle" id="myModalLabel2"><i class="fa fa-file-text"></i>電控產線報工</h4>
                </div>
                <asp:TextBox ID="TextBox_Product" runat="server" Style="display: none"></asp:TextBox>
                <asp:TextBox ID="TextBox_Date" runat="server" Style="display: none"></asp:TextBox>
                <asp:TextBox ID="TextBox_Canopencount" runat="server" Style="display: none"></asp:TextBox>
                <asp:TextBox ID="TextBox_Canshipmentcount" runat="server" Style="display: none"></asp:TextBox>
                <asp:TextBox ID="TextBox_Canclosecount" runat="server" Style="display: none"></asp:TextBox>
                <asp:TextBox ID="TextBox_startman" runat="server" Style="display: none"></asp:TextBox>
                <div class="modal-body">
                    <div id="testmodal2" style="padding: 5px 20px;">

                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>產品名稱：</b>
                                        <asp:Label ID="Label_Product" runat="server" Text="Label"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>預交日期：</b>
                                        <asp:Label ID="Label_Date" runat="server" Text="Label"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>報工狀態：</b>
                                        <asp:RadioButtonList ID="RadioButtonList_Status" RepeatColumns="4" runat="server">
                                            <asp:ListItem Value="0" Selected="True" Text="啟動" />
                                            <asp:ListItem Value="1" Text="完成" />
                                            <asp:ListItem Value="2" Text="出貨" />
                                            <%--<asp:ListItem Value="3" Text="狀態回復" />--%>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--啟動-->
                        <div id="Action">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>剩餘數量：</b>
                                            <asp:Label ID="Label_Count" runat="server" Text="Label"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>報工日期：</b>
                                            <asp:TextBox ID="TextBox_Actionday" runat="server" Width="50%" TextMode="Date"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>報工數量：</b>
                                            <asp:TextBox ID="TextBox_Count" runat="server" Width="50%" TextMode="Number"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>報工人員：</b>
                                            <asp:RadioButtonList ID="RadioButtonList_ReportMan" runat="server"></asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--完成-->
                        <div id="Report" style="display: none">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>剩餘數量：</b>
                                            <asp:Label ID="Label_Remaincount" runat="server" Text="Label"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>報工日期：</b>
                                            <asp:TextBox ID="TextBox_Reportday" runat="server" Width="50%" TextMode="Date"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>報工數量：</b>
                                            <asp:TextBox ID="TextBox_Reportcount" runat="server" Width="50%" TextMode="Number"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>報工人員：</b>
                                            <asp:RadioButtonList ID="RadioButtonList_ReportedMan" runat="server"></asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--出貨-->
                        <div id="Shipment" style="display: none">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>剩餘數量：</b>
                                            <asp:Label ID="Label_Shimpment" runat="server" Text="Label"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>出貨數量：</b>
                                            <asp:TextBox ID="TextBox_Shimpment" runat="server" Width="50%" TextMode="Number"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--狀態回復-->
                        <div id="StatusBack" style="display: none">
                            <asp:RadioButtonList ID="RadioButtonList_StatusBack" RepeatColumns="3" runat="server">
                                <asp:ListItem Value="0" Text="未動工" />
                                <asp:ListItem Value="1" Text="啟動" />
                                <asp:ListItem Value="2" Selected="True" Text="完成" />
                            </asp:RadioButtonList>

                            <div id="noworkback" style="display: none">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <div class="btn-group btn-group-justified">
                                                <b>可返回未動工數量：</b>
                                                <asp:Label ID="Label_noworkback" runat="server" Text="Label"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <div class="btn-group btn-group-justified">
                                                <b>返回數量：</b>
                                                <asp:TextBox ID="TextBox_noworkback" runat="server" Width="50%" TextMode="Number"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="actionback" style="display: none">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <div class="btn-group btn-group-justified">
                                                <b>可返回啟動數量：</b>
                                                <asp:Label ID="Label_actionback" runat="server" Text="Label"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <div class="btn-group btn-group-justified">
                                                <b>返回數量：</b>
                                                <asp:TextBox ID="TextBox_actionback" runat="server" Width="50%" TextMode="Number"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="finishback">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <div class="btn-group btn-group-justified">
                                                <b>可返回完成數量：</b>
                                                <asp:Label ID="Label_finishback" runat="server" Text="Label"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <div class="btn-group btn-group-justified">
                                                <b>返回數量：</b>
                                                <asp:TextBox ID="TextBox_finishback" runat="server" Width="50%" TextMode="Number"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default antoclose2" data-dismiss="modal">退出</button>
                    <asp:Button ID="Button_Save" runat="server" Text="Button" OnClick="Button_Save_Click" Style="display: none" />
                    <button id="btnSave" type="button" class="btn btn-primary antosubmit2">儲存</button>
                </div>
            </div>
        </div>
    </div>
    <!--新增插單-->
    <div id="exampleModalAdd" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title modaltextstyle" id="myModalLabel3"><i class="fa fa-file-text"></i>新增插單</h4>
                </div>
                <div class="modal-body">
                    <div id="testmodal3" style="padding: 5px 20px;">

                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>產品名稱：</b>
                                        <asp:TextBox ID="TextBox_AddProductName" Width="100%" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>預交日期：</b>
                                        <asp:TextBox ID="TextBox_AddProductDate" Width="100%" runat="server" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>預交數量：</b>
                                        <asp:TextBox ID="TextBox_AddProductTotal" Width="100%" runat="server" TextMode="Number"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default antoclose2" data-dismiss="modal">退出</button>
                    <asp:Button ID="Button_Add" runat="server" Text="Button" OnClick="Button_Add_Click" Style="display: none" />
                    <button id="btnAdd" type="button" class="btn btn-primary antosubmit2">儲存</button>
                </div>
            </div>
        </div>
    </div>
    <!--Model-->
    <!-----------------/content------------------>
    <!-- set Modal -->

    <!--/set Modal-->
    <!-- Modal -->

    <!-- /Modal -->
    <!-- jQuery -->
    <%=Use_Javascript.Quote_Javascript() %>
    <script>

        //選擇狀態
        $('#<%=RadioButtonList_ReportedMan.ClientID %> input').change(function () {
            relocationchange($(this));
        });

        function relocationchange(selValue) {
            var rb = document.getElementById("<%=RadioButtonList_ReportedMan.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            var label = rb.getElementsByTagName("label");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked)
                    label[i].innerHTML = '<b style="font-size:50px">' + radio[i].value + '</b>';
                else
                    label[i].innerHTML = '<b style="font-size:18px">' + radio[i].value + '</b>';
            }
        }

        //選擇狀態
        $('#<%=RadioButtonList_ReportMan.ClientID %> input').change(function () {
            relocationchanges($(this));
        });

        function relocationchanges(selValue) {
            var rb = document.getElementById("<%=RadioButtonList_ReportMan.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            var label = rb.getElementsByTagName("label");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked)
                    label[i].innerHTML = '<b style="font-size:50px">' + radio[i].value + '</b>';
                else
                    label[i].innerHTML = '<b style="font-size:18px">' + radio[i].value + '</b>';
            }
        }


        //產品名稱 日期 可啟動數量 可報工數量 啟動者
        function Calculate_Number(product, date, canopencount, canreportcount, startman, canshipmentcount, total) {
            //若已存在回報人員，則顯示完成
            if (startman != '') {
                $("#<%=RadioButtonList_Status.ClientID %> :radio[value='1']").prop("checked", true);
                document.getElementById('Report').style.display = 'initial';
                document.getElementById('Action').style.display = 'none';
                document.getElementById('Shipment').style.display = 'none';
                document.getElementById('StatusBack').style.display = 'none';
            }
            //沒有則顯示啟動
            else {
                $("#<%=RadioButtonList_Status.ClientID %> :radio[value='0']").prop("checked", true);
                document.getElementById('Report').style.display = 'none';
                document.getElementById('Action').style.display = 'initial';
                document.getElementById('Shipment').style.display = 'none';
                document.getElementById('StatusBack').style.display = 'none';
            }
            //出貨
            if (canopencount == '0' && canreportcount == '0' && canshipmentcount != '0') {
                $("#<%=RadioButtonList_Status.ClientID %> :radio[value='2']").prop("checked", true);
                document.getElementById('Report').style.display = 'none';
                document.getElementById('Action').style.display = 'none';
                document.getElementById('Shipment').style.display = 'initial';
                document.getElementById('StatusBack').style.display = 'none';
            }
            //直接顯示返回狀態(預設為啟動)
            else if (canopencount == '0' && canreportcount == '0' && canshipmentcount == '0') {
                $("#<%=RadioButtonList_Status.ClientID %> :radio[value='3']").prop("checked", true);
                document.getElementById('Report').style.display = 'none';
                document.getElementById('Action').style.display = 'none';
                document.getElementById('Shipment').style.display = 'none';
                document.getElementById('StatusBack').style.display = 'none';
            }


            //產品名稱
            $('#<%=TextBox_Product.ClientID %>').val('' + product + '');
            document.getElementById("<%=Label_Product.ClientID %>").innerHTML = product;
            //預交日期
            $('#<%=TextBox_Date.ClientID %>').val('' + date + '');
            document.getElementById("<%=Label_Date.ClientID %>").innerHTML = date;
            //可啟動數量
            $('#<%=TextBox_Canopencount.ClientID %>').val('' + canopencount + '');
            $('#<%=TextBox_Count.ClientID%>').val('' + canopencount + '');
            document.getElementById("<%=Label_Count.ClientID%>").innerHTML = canopencount;
            //可回報數量
            $('#<%=TextBox_Canclosecount.ClientID%>').val('' + canreportcount + '');
            $('#<%=TextBox_Reportcount.ClientID%>').val('' + canreportcount + '');
            document.getElementById("<%=Label_Remaincount.ClientID%>").innerHTML = canreportcount;
            //可出貨數量
            $('#<%=TextBox_Canshipmentcount.ClientID%>').val('' + canshipmentcount + '');
            $('#<%=TextBox_Shimpment.ClientID%>').val('' + canshipmentcount + '');
            document.getElementById("<%=Label_Shimpment.ClientID%>").innerHTML = canshipmentcount;

            $('#<%=TextBox_startman.ClientID%>').val('' + startman + '');
            if (startman != '') {
                $("#<%=RadioButtonList_ReportedMan.ClientID%> :radio[value='" + startman + "']").prop("checked", true);
                relocationchange('');
            }

            //出貨完畢 可返回完成 啟動 未動工
            if (canopencount == '0' && canreportcount == '0' && canshipmentcount == '0') {
                document.getElementById("<%=Label_noworkback.ClientID%>").innerHTML = total;
                document.getElementById("<%=Label_actionback.ClientID%>").innerHTML = total;
                document.getElementById("<%=Label_finishback.ClientID%>").innerHTML = total;
                $('#<%=TextBox_noworkback.ClientID%>').val('' + total + '');
                $('#<%=TextBox_actionback.ClientID%>').val('' + total + '');
                $('#<%=TextBox_finishback.ClientID%>').val('' + total + '');
            }
            //已完成 可返回啟動 未動工
            else if (canopencount == '0' && canreportcount == '0' && canshipmentcount != '0') {
                document.getElementById("<%=Label_noworkback.ClientID%>").innerHTML = total;
                document.getElementById("<%=Label_actionback.ClientID%>").innerHTML = total;
                document.getElementById("<%=Label_finishback.ClientID%>").innerHTML = '0';
                $('#<%=TextBox_noworkback.ClientID%>').val('' + total + '');
                $('#<%=TextBox_actionback.ClientID%>').val('' + total + '');
                $('#<%=TextBox_finishback.ClientID%>').val('' + '0' + '');
            }
            //已啟動 可返回未動工
            else if (canopencount == '0' && canreportcount != '0' && canshipmentcount == '0') {
                document.getElementById("<%=Label_noworkback.ClientID%>").innerHTML = total;
                document.getElementById("<%=Label_actionback.ClientID%>").innerHTML = '0';
                document.getElementById("<%=Label_finishback.ClientID%>").innerHTML = '0';
                $('#<%=TextBox_noworkback.ClientID%>').val('' + total + '');
                $('#<%=TextBox_actionback.ClientID%>').val('' + '0' + '');
                $('#<%=TextBox_finishback.ClientID%>').val('' + '0' + '');
            }
            //未啟動，皆為0
            else if (canopencount != '0' && canreportcount != '0' && canshipmentcount == '0') {
                document.getElementById("<%=Label_noworkback.ClientID%>").innerHTML = '0';
                document.getElementById("<%=Label_actionback.ClientID%>").innerHTML = '0';
                document.getElementById("<%=Label_finishback.ClientID%>").innerHTML = '0';
                $('#<%=TextBox_noworkback.ClientID%>').val('' + '0' + '');
                $('#<%=TextBox_actionback.ClientID%>').val('' + '0' + '');
                $('#<%=TextBox_finishback.ClientID%>').val('' + '0' + '');
            }
            //混合情況
            else {
                document.getElementById("<%=Label_noworkback.ClientID%>").innerHTML = '0';
                document.getElementById("<%=Label_actionback.ClientID%>").innerHTML = '0';
                document.getElementById("<%=Label_finishback.ClientID%>").innerHTML = '0';
                $('#<%=TextBox_noworkback.ClientID%>').val('' + '0' + '');
                $('#<%=TextBox_actionback.ClientID%>').val('' + '0' + '');
                $('#<%=TextBox_finishback.ClientID%>').val('' + '0' + '');
            }
        }

        //選擇狀態
        $('#<%=RadioButtonList_Status.ClientID%> input').change(function () {
            relocation($(this).val());
        });
        relocation($('#<%=RadioButtonList_Status.ClientID%> input:checked').val());

        function relocation(selValue) {
            var canaction = document.getElementById('Action');
            var canclose = document.getElementById('Report');
            var canshipment = document.getElementById('Shipment');
            var canback = document.getElementById('StatusBack');
            switch (selValue) {
                case "0":
                    canaction.style.display = 'initial';
                    canclose.style.display = 'none';
                    canshipment.style.display = 'none';
                    canback.style.display = 'none';
                    break;
                case "1":
                    canaction.style.display = 'none';
                    canclose.style.display = 'initial';
                    canshipment.style.display = 'none';
                    canback.style.display = 'none';
                    break;
                case "2":
                    canclose.style.display = 'none';
                    canaction.style.display = 'none';
                    canshipment.style.display = 'initial';
                    canback.style.display = 'none';
                    break;
                case "3":
                    canclose.style.display = 'none';
                    canaction.style.display = 'none';
                    canshipment.style.display = 'none';
                    canback.style.display = 'none';
                    break;
            }
        }

        //選擇須返回的狀態

        $('#<%=RadioButtonList_StatusBack.ClientID%> input').change(function () {
            relocationback($(this).val());
        });
        relocationback($('#<%=RadioButtonList_StatusBack.ClientID%> input:checked').val());

        function relocationback(selValue) {
            var backnowork = document.getElementById('noworkback');
            var backaction = document.getElementById('actionback');
            var backfinish = document.getElementById('finishback');
            switch (selValue) {
                case "0":
                    backnowork.style.display = 'initial';
                    backaction.style.display = 'none';
                    backfinish.style.display = 'none';
                    break;
                case "1":
                    backnowork.style.display = 'none';
                    backaction.style.display = 'initial';
                    backfinish.style.display = 'none';
                    break;
                case "2":
                    backnowork.style.display = 'none';
                    backaction.style.display = 'none';
                    backfinish.style.display = 'initial';
                    break;
            }
        }

        //報工按鈕
        $("#btnSave").click(function () {
            var count;
            var standard;
            var selectedvalue = $("#<%=RadioButtonList_Status.ClientID%> input:radio:checked").val();
            var selectMan;
            //啟動中
            if (selectedvalue == '0') {
                count = document.getElementById("<%=TextBox_Count.ClientID%>").value;
                standard = document.getElementById("<%=Label_Count.ClientID%>").innerText;
                selectMan = $("#<%=RadioButtonList_ReportMan.ClientID%> input:radio:checked").val();
            }
            //完成
            else if (selectedvalue == '1') {
                count = document.getElementById("<%=TextBox_Reportcount.ClientID%>").value;
                standard = document.getElementById("<%=Label_Remaincount.ClientID%>").innerText;
                selectMan = $("#<%=RadioButtonList_ReportedMan.ClientID%> input:radio:checked").val();
            }
            //出貨
            else if (selectedvalue == '2') {
                count = document.getElementById("<%=TextBox_Shimpment.ClientID%>").value;
                standard = document.getElementById("<%=Label_Shimpment.ClientID%>").innerText;
                selectMan = '000';
            }
            if (standard == 0) {
                alert('已無數量可填寫');
            } else {
                if (parseInt(standard, 10) < parseInt(count, 10)) {
                    alert('超過數量，請重新填寫');
                } else {
                    if (selectMan == null)
                        alert('未選取人員');
                    else {


                        if (selectedvalue == '1') {

                            answer = confirm("目前選取的人為      " + selectMan + "      確定要送出嗎??");
                            if (answer) {
                                var WhatSystem = navigator.userAgent;
                                if (WhatSystem.match(/(iphone|ipad|ipod);?/i)) {
                                } else {
                                    $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                                    document.querySelector(".blockUI.blockMsg.blockPage").style.zIndex = 1000000;
                                    document.getElementById('btnSave').disabled = true;
                                }
                                document.getElementById('<%=Button_Save.ClientID %>').click();
                            }

                        }
                        else {
                            var WhatSystem = navigator.userAgent;
                            if (WhatSystem.match(/(iphone|ipad|ipod);?/i)) {
                            } else {
                                $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                                document.querySelector(".blockUI.blockMsg.blockPage").style.zIndex = 1000000;
                                document.getElementById('btnSave').disabled = true;
                            }
                            document.getElementById('<%=Button_Save.ClientID %>').click();
                        }
                    }

                }
            }

        });



        //新增插單
        $("#btnAdd").click(function () {
            var ProductName = document.getElementById("<%=TextBox_AddProductName.ClientID%>").value;
            var ProductDate = document.getElementById("<%=TextBox_AddProductDate.ClientID%>").value;
            var ProductCount = document.getElementById("<%=TextBox_AddProductTotal.ClientID%>").value;

            if (ProductName != '' && ProductDate != '' && ProductCount != '') {
                var WhatSystem = navigator.userAgent;
                if (WhatSystem.match(/(iphone|ipad|ipod);?/i)) {
                } else {
                    $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                    document.querySelector(".blockUI.blockMsg.blockPage").style.zIndex = 1000000;
                    document.getElementById('btnAdd').disabled = true;
                }

                document.getElementById('<%=Button_Add.ClientID %>').click();
            }
            else {
                alert('資訊未填寫完成，請檢查一下!!');
            }

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
