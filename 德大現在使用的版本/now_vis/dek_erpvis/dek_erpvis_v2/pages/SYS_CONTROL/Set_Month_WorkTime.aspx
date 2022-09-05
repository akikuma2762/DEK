<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Set_Month_WorkTime.aspx.cs" Inherits="dek_erpvis_v2.pages.SYS_CONTROL.Set_Month_WorkTime" %>

<%--<%@ OutputCache duration="10" varybyparam="None" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>變更資料 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/UntradedCustomer.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <!-----------------title------------------>
        <br />
        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                         <div id="order"></div>
                        
                        <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">

                                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                                        </asp:ScriptManager>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                            <ContentTemplate>
                                               <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                        <span>廠區選擇</span>
                                                    </div>
                                                        <div class="col-md-8 col-sm-9 col-xs-8">                                                       
                                                                <asp:DropDownList ID="DropDownList_Factory" AutoPostBack="true" CssClass="btn btn-default dropdown-toggle form-control"  runat="server" OnSelectedIndexChanged="DropDownList_Factory_SelectedIndexChanged">
                                                                    <asp:ListItem Value="sowon" Selected="True">立式廠</asp:ListItem>
                                                                    <asp:ListItem Value="dek">大圓盤</asp:ListItem>
                                                                    <asp:ListItem Value="iTec">臥式廠</asp:ListItem>
                                                                </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                        <span>產線選擇</span>
                                                    </div>
                                                        <div class="col-md-8 col-sm-9 col-xs-8">
                                                                <asp:DropDownList ID="DropDownList_Product" CssClass="btn btn-default dropdown-toggle form-control"  runat="server">
                                                                </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                        <span>起始月份</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                    <asp:TextBox ID="Select_Month" runat="server" TextMode="month" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>


                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="col-md-12 col-sm-12 col-xs-12 col-style">
                                            <div class="col-md-12 col-xs-12 text-align-end">
                                                    <button id="btn_Month_WorkTime" type="button" class="btn btn-info" data-toggle="modal" data-target="#Insert_Month_WorkTime">新增月份工時</button>
                                                </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 col-style">
                                            <div class="col-md-12 col-xs-12 text-align-end">
                                                    <button id="btn_Day_WorkTime" type="button" class="btn btn-info" data-toggle="modal" data-target="#Insert_Day_WorkTime">新增單日工時</button>
                                                </div>
                                        </div>
                                         <div class="col-md-12 col-sm-12 col-xs-12">
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
    </div>
    <!-----------------/content------------------>
 
    <!-- set Modal -->
        <div id="Insert_Month_WorkTime" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title modaltextstyle" id="Month_WorkTime_Title"><i class="fa fa-file-text"></i>新增月份工時</h4>
                    </div>
                    <div class="modal-body">
                        <div id="Month_WorkTime" style="padding: 5px 20px;">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div id="Working_People_Content" class="btn-group btn-group-justified">
                                            <b>預設每日上班人數:</b><br />
                                            <input  type="text" maxlength="2" id="Working_People" class="int_Value"/>
                                            <span></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div id="Work_Time_Content" class="btn-group btn-group-justified">
                                            <b>預設每日工作時數:</b><br />
                                            <input  type="text" maxlength="2" id="Work_Time" class="int_Value"/>
                                            <span></span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>工作站名稱:</b><br />
                                            <asp:DropDownList ID="Month_Workstation" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>選擇月份:</b><br/>
                                            <input type="month" id="Single_Month" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="Insert_Month_Btn_Cancel" type="button" class="btn btn-default antoclose2" data-dismiss="modal">退出</button>
                        <button id="Insert_Month_Btn_Save" type="button"  class="btn btn-primary antosubmit2">新增</button>
                    </div>
                </div>
            </div>
        </div>
        <!--/set Modal-->
     <!-- set Modal -->
        <div id="Insert_Day_WorkTime" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title modaltextstyle" id="Day_WorkTime_Title"><i class="fa fa-file-text"></i>新增單日工時</h4>
                    </div>
                    <div class="modal-body">
                        <div id="Day_WorkTime" style="padding: 5px 20px;">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div  id="Day_Working_People_Content"class="btn-group btn-group-justified">
                                            <b>預設上班人數:</b><br />
                                            <input  type="text" maxlength="2" id="Day_Working_People" class="int_Value"/>
                                            <span></span> 
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div id="Day_Work_Time_Content" class="btn-group btn-group-justified">
                                            <b>預設工作時數:</b><br />
                                            <input  type="text" maxlength="2" id="Day_Work_Time" class="int_Value"/>
                                            <span></span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>工作站名稱:</b><br />
                                            <asp:DropDownList ID="Day_Workstation" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>選擇日期:</b><br/>
                                            <input type="date" id="Single_Day" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="Insert_Day_Btn_Cancel" type="button" class="btn btn-default antoclose2" data-dismiss="modal">退出</button>
                        <button id="Insert_Day_Btn_Save" type="button" onclick="insertValue()" class="btn btn-primary antosubmit2">新增資料</button>
                    </div>
                </div>
            </div>
        </div>
        <!--/set Modal-->



    <%=Use_Javascript.Quote_Javascript() %>

    <script>
        var warning = {};
        var top_Link = "";
        $(document).ready(function () {

            //20220825 重載DataTable時如未搜尋或重整,則記錄初始狀態
            top_Link = $('#ContentPlaceHolder1_DropDownList_Factory').val().toLowerCase();

            //20220826動態變更主表格標題
            if (top_Link.toLowerCase() == "sowon") {
                $("._mdTitle").text("立式廠 工人工時編輯");
                $("._xsTitle").text("立式廠 工人工時編輯");
            }
            else if (top_Link.toLowerCase() == "dek") {
                $("._mdTitle").text("大圓盤 工人工時編輯");
                $("._xsTitle").text("大圓盤 工人工時編輯");
            } else {
                $("._mdTitle").text("臥式廠 工人工時編輯");
                $("._xsTitle").text("臥式廠 工人工時編輯");

            }

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


        //產生表格的HTML碼
        create_tablehtmlcode('order', '工人工時表', 'table-form', '<%=th.ToString() %>', '<%=tr.ToString() %>');
        //產生相對應的JScode
        set_Table('#table-form');
        $("#Insert_Month_Btn_Save").click(function () {
            var inupu_Null = false;
            var objectLength = Object.keys(warning).length;
            inupu_Null = check_Modal_Input("Month_WorkTime");
            if (inupu_Null) return;
            if (objectLength > 0) {
                alert("輸入資料有誤,請修正資料!");
                return;
            } else {
                var data = {};
                var cookieInfo = readCookie('userInfo');
                var arry = cookieInfo.split("&");
                var obj = {};
                for (var i = 0; i < arry.length; i++) {
                    var arry2 = arry[i].split("=");
                    obj[arry2[0]] = arry2[1];
                }
                data["Factory"] = $("#ContentPlaceHolder1_DropDownList_Factory").val();
                data["Working_People"] = $("#Working_People").val();
                data["Work_Time"] = $("#Work_Time").val();
                data["Workstation"] = $("#ContentPlaceHolder1_Month_Workstation option:selected").text();
                data["Workstation_Num"] = $("#ContentPlaceHolder1_Month_Workstation").val();
                data["Month"] = $("#Single_Month").val().replace(/-/g, "");
                data["Day"] = "";
                data["Mode"] = "Month";
                data["User_Acc"] = obj["user_ACC"];
                console.log(data);
                data = JSON.stringify(data);
                postData(data);
            }

        });

        $("#Insert_Day_Btn_Save").click(function () {
            var inupu_Null = false;
            var objectLength = Object.keys(warning).length;
            inupu_Null = check_Modal_Input("Day_WorkTime");
            if (inupu_Null) return;
            if (objectLength > 0) {
                alert("輸入資料有誤,請修正資料!");
                return;
            } else {
                var data = {};
                var cookieInfo = readCookie('userInfo');
                var arry = cookieInfo.split("&");
                var obj = {};
                for (var i = 0; i < arry.length; i++) {
                    var arry2 = arry[i].split("=");
                    obj[arry2[0]] = arry2[1];
                }
                data["Factory"] = $("#ContentPlaceHolder1_DropDownList_Factory").val();
                data["Working_People"] = $("#Day_Working_People").val();
                data["Work_Time"] = $("#Day_Work_Time").val();
                data["Workstation"] = $("#ContentPlaceHolder1_Day_Workstation option:selected").text();
                data["Workstation_Num"] = $("#ContentPlaceHolder1_Day_Workstation").val();
                data["Month"] = "";
                data["Day"] = $("#Single_Day").val().replace(/-/g, "");
                data["Mode"] = "Day";
                data["User_Acc"] = obj["user_ACC"];
                console.log(data);
                data = JSON.stringify(data);
                postData(data);
            }

        });


        //20220901判斷數字群組
        $(".int_Value").keyup(function () {
            // 驗證輸入字串
            var obj = {};
            const rules = /^[1-9][0-9]*$/;
            var id = $(this).parent().attr("id")
            var value = $(this).val();
            if (!rules.test(value) && value != "") {
                $("#" + id + " span").text("資料格式錯誤!");
                $("#" + id + " span").css("color", "red");
                warning[id] = "";
            } else {
                $("#" + id + " span").text("");
                delete warning[id];
            }
            console.log($('body [class="modal fade in"]'));
            console.log(warning);
        });



        //清空modal資訊
        clearModal();


        $('body').on('show.bs.modal', ".modal", function () {
            console.log($(this).attr("id"));
            var id = $(this).attr("id");
            if (top_Link.toLowerCase() == "sowon") {
                if (id.indexOf("Month") != -1) {
                    $(this).find('.modal-title').text("立式廠 單月工時新增");
                } else {
                    $(this).find('.modal-title').text("立式廠 單日工時新增");
                }
            }
            else if (top_Link.toLowerCase() == "dek") {
                if (id.indexOf("Month") != -1) {
                    $(this).find('.modal-title').text("大圓盤 單月工時新增");
                } else {
                    $(this).find('.modal-title').text("大圓盤 單日工時新增");
                }
            } else {
                if (id.indexOf("Month") != -1) {
                    $(this).find('.modal-title').text("臥式廠 單月工時新增");
                } else {
                    $(this).find('.modal-title').text("臥式廠 單日工時新增");
                }
            }

        });

        function postData(data) {
            var WhatSystem = navigator.userAgent;
            if (WhatSystem.match(/(iphone|ipad|ipod);?/i)) {
            } else {
                $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                document.querySelector(".blockUI.blockMsg.blockPage").style.zIndex = 100000;
                $("#Insert_Month_WorkTime button").disabled = true;
                //document.getElementById('btn_Cancel').disabled = true;
                //document.getElementById('Insert_btnSave').disabled = true;
                //document.getElementById('Insert_Btn_Cancel').disabled = true;
            }
            $.ajax({
                type: "post",
                contentType: "application/json",
                url: "Set_Month_WorkTime.aspx/postData",
                data: "{_data:'" + data + "'}",
                dataType: "json",
                success: function (result) {
                    var results_Data = result.d;
                    //console.log(results_Data);
                    if (results_Data["status"].indexOf("成功") != -1) {

                        if (top_Link.toLowerCase() == "sowon") {
                            $("._mdTitle").text("立式廠 工人工時編輯");
                            $("._xsTitle").text("立式廠 工人工時編輯");
                        }
                        else if (top_Link.toLowerCase() == "dek") {
                            $("._mdTitle").text("大圓盤 工人工時編輯");
                            $("._xsTitle").text("大圓盤 工人工時編輯");
                        } else {
                            $("._mdTitle").text("臥式廠 工人工時編輯");
                            $("._xsTitle").text("臥式廠 工人工時編輯");

                        }
                        alert(results_Data["status"]);
                    } else if (results_Data["status"].indexOf("失敗") != -1) {
                        alert(results_Data["status"]);
                    } else if (results_Data["status"].indexOf("沒有資料") != -1) {
                        alert(results_Data["status"]);
                    }

                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("資料傳輸錯誤,請檢查資料傳遞格式!!");
                    //alert(XMLHttpRequest.status);
                    //alert(XMLHttpRequest.readyState);
                    //alert(textStatus);
                }
                , complete: function (jqXHR) {
                    //關閉loading視窗及修改視窗
                    $('body [class="modal fade in"]').click();
                    $.unblockUI();
                    $(".blockUI").fadeOut("slow");
                    //打開儲存&取消按鈕
                    $("#Insert_Month_WorkTime button").disabled = false;
                    //document.getElementById('btn_Cancel').disabled = false;
                    //document.getElementById('Insert_btnSave').disabled = false;
                    //document.getElementById('Insert_Btn_Cancel').disabled = false;


                    //電腦板新增後跑版
                    $("#table-form_wrapper").css("overflow-x", "auto");

                }
            });

        }










    </script>
</asp:Content>
