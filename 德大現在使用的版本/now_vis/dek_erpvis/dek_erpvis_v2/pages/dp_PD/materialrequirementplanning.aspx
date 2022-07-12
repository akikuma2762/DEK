<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="materialrequirementplanning.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PD.materialrequirementplanning_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>物料領用統計表 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" media="screen and (max-width:768px)" />
    <link href="../../Content/dp_PD/materialrequirementplanning.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <!-----------------title------------------>
        <%=path %>
        <br>
        <div class="clearfix"></div>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div id="materialrequirementplanning"></div>
                        <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                            <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                <div class="col-md-5 col-sm-3 col-xs-12">
                                                    <span>廠區</span>
                                                </div>
                                                <div class="col-md-7 col-sm-9 col-xs-12">
                                                    <div class="row">
                                                        <div class="">
                                                            <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="btn btn-default form-control">
                                                                <asp:ListItem Value="sowon" Selected="True">立式廠</asp:ListItem>
                                                                <asp:ListItem Value="dek">大圓盤</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-5 col-sm-3 col-xs-12">
                                                <span>搜尋方式</span>
                                            </div>
                                            <div class="col-md-7 col-sm-9 col-xs-12">
                                                <div class="row">
                                                    <div class="">
                                                        <asp:DropDownList ID="DropDownList_select_type" Font-Names="NotoSans" runat="server" CssClass="btn btn-default form-control">
                                                            <asp:ListItem Value="0" Selected="True">依 分類 檢索</asp:ListItem>
                                                            <asp:ListItem Value="1">依 品號 檢索</asp:ListItem>
                                                            <asp:ListItem Value="2">依 品名規格 檢索</asp:ListItem>
                                                            <asp:ListItem Value="3">均不指定</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 ">
                                            <p></p>
                                            <i id="selected_note"></i>
                                        </div>
                                        <div class="col-md-12 col-xs-12 col-sm-12 flex col-style">
                                            <div class="col-md-5 col-sm-3 col-xs-12" style="margin:5px 0px 5px 0px;">
                                                <span>內容</span>
                                            </div>

                                            <div class="col-md-7 col-sm-9 col-xs-12">
                                                <div class="row">
                                                    <div class="" style="margin-bottom:5px;">

                                                        <asp:DropDownList ID="DropDownList_selectedcondi" Font-Names="NotoSans" runat="server" CssClass="btn btn-default form-control">
                                                            <asp:ListItem Value="內含">內含</asp:ListItem>
                                                            <asp:ListItem Value="等於">等於</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div class="">
                                                        <div id="input_keyword">
                                                            <asp:TextBox ID="TextBox_keyword" data-validate-length-range="6" data-validate-words="2" runat="server" placeholder="請輸入字串"></asp:TextBox>
                                                        </div>
                                                        <div id="warp_droplist" style="display: initial;">
                                                            <asp:DropDownList ID="DropDownList_materialstype" Font-Names="NotoSans" runat="server" CssClass="btn btn-default dropdown-toggle form-control">
                                                                <asp:ListItem Value="壓缸固定鈑" Selected="True">壓缸固定鈑</asp:ListItem>
                                                                <asp:ListItem Value="刀套">刀套</asp:ListItem>
                                                                <asp:ListItem Value="刀臂">刀臂</asp:ListItem>
                                                                <asp:ListItem Value="頂刀爪">頂刀爪</asp:ListItem>
                                                                <asp:ListItem Value="扣刀爪">扣刀爪#50</asp:ListItem>
                                                                <asp:ListItem Value="圓筒凸輪">圓筒凸輪</asp:ListItem>
                                                                <asp:ListItem Value="馬達座">馬達座</asp:ListItem>
                                                                <asp:ListItem Value="鍊輪座">鍊輪座</asp:ListItem>
                                                                <asp:ListItem Value="減速機固定鈑">減速機固定鈑</asp:ListItem>
                                                                <asp:ListItem Value="mazak inte">mazak inte</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="col-md-12 col-xs-12 col-sm-12 col-xs-12 flex col-style">
                                            <div class="col-md-5 col-sm-3 col-xs-12" style="margin:5px 0px 5px 0px;">
                                                <span>位數</span>
                                            </div>
                                             <div class="col-md-7 col-sm-9 col-xs-12">
                                                <div class="row">
                                            <div class="" style="margin-bottom:5px;">
                                                <asp:DropDownList ID="DropDownList_substring" Font-Names="NotoSans" runat="server" CssClass="btn btn-default form-control" onchange="getval(this);">
                                                    <asp:ListItem Value="N" Selected="True">不擷取</asp:ListItem>
                                                    <asp:ListItem Value="Y">擷取</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="">

                                                <div id="input_subs">
                                                    <asp:TextBox ID="TextBox_substring" Font-Names="NotoSans" CssClass="form-control text-center" runat="server" placeholder="只能輸入數字" TextMode="Number" Enabled="false" min="12" max="99" Text="12"></asp:TextBox>
                                                </div>

                                            </div>
                                                    </div>
                                                 </div>

                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-5 col-sm-3 col-xs-12">
                                                <span>安全庫存量</span>
                                            </div>
                                            <div class="col-md-7 col-sm-9 col-xs-12">
                                                <div class="row">
                                                <input id="demo_vertical3" runat="server" type="text" value="1" name="demo_vertical3" data-bts-button-down-class="btn btn-secondary" data-bts-button-up-class="btn btn-secondary nomargin">
                                                    </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-5 col-sm-3 col-xs-12" >
                                                <span>最小採購量</span>
                                            </div>
                                            <div class="col-md-7 col-sm-9 col-xs-12">
                                                <div class="row">
                                                <input id="demo_vertical2" runat="server" type="text" value="2" name="demo_vertical2" data-bts-button-down-class="btn btn-secondary" data-bts-button-up-class="btn btn-secondary nomargin">
                                                    </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                            <div class="col-md-5 col-sm-3 col-xs-12">
                                                <span>日期快選</span>
                                            </div>
                                            <div class="col-md-7 col-sm-9 col-xs-12">
                                                <div class="row">
                                                    <div class="">
                                                    <div class="btn-group btn-group-justified">
                                                        <a id="ContentPlaceHolder1_LinkButton_month" class="btn btn-default " onclick=" set_nowmonth()" style="text-align: center">當月</a>
                                                    </div>
                                                        </div>
                                                </div>
                                            </div>
                                        </div>
                                        <i id="cbx_remind"></i>
                                        <div class="col-md-12 col-sm-12 col-xs-12  flex col-style">
                                            <div class="col-md-5 col-sm-3 col-xs-12" style="margin:5px 0px 5px 0px;">
                                                領料日期
                                            </div>
                                            <div class="col-md-7 col-sm-9 col-xs-12">
                                                <div class="row">
                                                    <div class="">
                                                    <fieldset>
                                                        <div class="control-group">
                                                            <div class="controls">
                                                                <div class="col-md-12 col-sm-12 col-xs-12" style="margin: 0px 0px 5px 0px">
                                                                    <asp:TextBox ID="txt_str" runat="server" Style="" TextMode="Date" CssClass="form-control   text-right"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </fieldset>
                                                    <fieldset>
                                                        <div class="control-group">
                                                            <div class="controls">
                                                                <div class="" style="margin: 0px 0px 5px 0px">
                                                                    <asp:TextBox ID="txt_end" runat="server" Style="" TextMode="Date" CssClass="form-control   text-right"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </fieldset>
                                                        </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div class="col-md-9 col-xs-8">
                                            </div>
                                            <div class="col-md-3 col-xs-12">
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
    </div>
    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        window.onload = change_Radio();
        //選取搜尋前的選項
        function change_Radio() {
            try {
                relocation($('#<%=DropDownList_select_type.ClientID %>').val());
            }
            catch {

            }
        }

        var remind, selected_note;
        input_keyword.style.display = 'none';
        function initialization() {
            remind = document.getElementById("cbx_remind");
            selected_note = document.getElementById("selected_note");

            remind.style.color = "#FF3333"; selected_note.style.color = "#FF3333";
            document.getElementById('<%=TextBox_substring.ClientID %>').disabled = true;
            $("select#<%=DropDownList_substring.ClientID %>").prop('selectedIndex', 0);
        }
        function getval(sel) {
            if (sel.value == "Y") {
                document.getElementById('<%=TextBox_substring.ClientID%>').disabled = false;
                selected_note.innerHTML = "此功能可能會導致搜尋效能降低!(長度範圍 12 ~ 99)";
            }
            if (sel.value == "N") {
                document.getElementById('<%=TextBox_substring.ClientID %>').disabled = true;
                selected_note.innerHTML = "";
            }
        }
        function relocation(selValue) {
            var input_keyword = document.getElementById('input_keyword');
            var warp_droplist = document.getElementById('warp_droplist');
            var selectedcondi = document.getElementById('<%=DropDownList_selectedcondi.ClientID %>');
            var materialstype = document.getElementById('<%=DropDownList_materialstype.ClientID %>');
            switch (selValue) {
                case "0":
                    input_keyword.style.display = 'none';
                    warp_droplist.style.display = 'initial';
                    selectedcondi.disabled = false;
                    materialstype.disabled = false;
                    selected_note.innerHTML = "";
                    break;
                case "1":
                    input_keyword.style.display = 'initial';
                    warp_droplist.style.display = 'none';
                    selectedcondi.disabled = false;
                    materialstype.disabled = false;
                    selected_note.innerHTML = "";
                    break;
                case "2":
                    input_keyword.style.display = 'initial';
                    warp_droplist.style.display = 'none';
                    selectedcondi.disabled = false;
                    materialstype.disabled = false;
                    selected_note.innerHTML = "";
                    break;
                case "3":
                    input_keyword.style.display = 'none';
                    warp_droplist.style.display = 'initial';
                    selectedcondi.disabled = true;
                    materialstype.disabled = true;
                    selected_note.innerHTML = "此功能可能會導致搜尋效能降低!";
                    break;
            }
        }
        function checkthetextcount(id) {
            var text_count = document.getElementById("<%=TextBox_substring.ClientID %>").value;
            var selected = $("#<%=DropDownList_substring.ClientID %> option:selected").val();
            var selValue = $('#<%=DropDownList_select_type.ClientID%>').val();
            var re = /^[0-9]+$/;
            //----------------------------------------
            if (selected == "Y") {
                if (text_count != "") {
                    if (re.test(text_count)) {
                        check_val(selValue, id);
                    } else {
                        remind.innerHTML = "擷取字元只能輸入1-99的數字";
                    }
                } else {
                    remind.innerHTML = "擷取字元不得為空!";
                }
            } else if (selected == "N") {
                check_val(selValue, id);
            }
            //-----------------------------------------
        }
        function check_val(selValue, id) {
            var itme = document.getElementById("<%=TextBox_keyword.ClientID%>").value;// 品號、品名規格
            if (selValue == 0) {
                search_go(id);
            }
            else if (selValue == 1) {
                if (itme != "") {
                    search_go(id);
                } else {
                    remind.innerHTML = "品號不得為空";
                }
            }
            else if (selValue == 2) {
                if (itme != "") {
                    search_go(id);
                } else {
                    remind.innerHTML = "品名規格不得為空!";
                }
            }
            else if (selValue == 3) {
                search_go(id);
            }

        }

        function search_go(id) {

            switch (id) {
                case "btncheck":
                    document.getElementById('<%=button_select.ClientID %>').click();
                    break;
            }
        }

        //evet
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            checkthetextcount("btncheck");
        });
        $('#<%=DropDownList_select_type.ClientID%>').change(function () {
            console.log($(this).val());
            relocation($(this).val());
        });
        $("input[name='ctl00$ContentPlaceHolder1$demo_vertical2']").TouchSpin({
            min: 0,
            max: 100,
            step: 0.1,
            decimals: 1,
            boostat: 5,
            maxboostedstep: 10,
        });
        $("input[name='ctl00$ContentPlaceHolder1$demo_vertical3']").TouchSpin({
            min: 0,
            max: 100,
            step: 0.1,
            decimals: 1,
            boostat: 5,
            maxboostedstep: 10,
        });
        initialization();
        relocation($('#<%=DropDownList_select_type.ClientID%> input:checked').val());




        //產生表格的HTML碼
        create_tablehtmlcode('materialrequirementplanning', '物料領用統計表', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString()%>');
        //產生相對應的JScode
        set_Table('#datatable-buttons');
        //防止頁籤跑版
        loadpage('materialrequirementplanning=materialrequirementplanning_cust', '#datatable-buttons');


        function set_nowmonth() {
            document.getElementById('<%=txt_str.ClientID%>').value = '<%=date_str%>';
            document.getElementById('<%=txt_end.ClientID%>').value = '<%=date_end%>';
        }
    </script>
</asp:Content>
