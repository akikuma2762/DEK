<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Previous_Shipment.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_SD.Previous_Shipment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>預出貨列表 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <%=color %>
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/UntradedCustomer.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <!-----------------title------------------>
        <%= path %>
        <br />
        <!-----------------/title------------------>
        <asp:TextBox ID="TextBox_Delete" runat="server" Style="display: none"></asp:TextBox>
        <asp:Button ID="Button_Delete" runat="server" Style="display: none" Text="" OnClick="Button_Delete_Click" />
        <asp:TextBox ID="TextBox_Update" runat="server" Style="display: none"></asp:TextBox>
        <asp:Button ID="Button_Update" runat="server" Text="" Style="display: none" OnClick="Button_Update_Click" />
        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div id="Previous_Shipment"></div>
                        <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <div class="col-md-12 col-sm-6 col-xs-12">
                                            <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                <span>起始日期</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                <asp:TextBox ID="txt_str" runat="server" Style="" TextMode="Date" CssClass="form-control   text-center"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-6 col-xs-12">
                                            <div class="col-md-4 col-sm-3 col-xs-4">
                                                <span>結束日期</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                <asp:TextBox ID="txt_end" runat="server" CssClass="form-control  text-center" TextMode="Date"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                        <div class="col-md-9 col-xs-8">
                                        </div>
                                        <div class="col-md-3 col-xs-12">
                                            <button id="btncheck" type="button" class="btn btn-primary antosubmit2 ">執行搜索</button>
                                            <asp:Button runat="server" Text="提交" ID="Button_select" CssClass="btn btn-primary" Style="display: none" OnClick="button_select_Click" />
                                        </div>
                                    </div>
                                </div>
                                <br />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--更新的modal-->
    <div id="exampleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title modaltextstyle" id="myModalLabel2"><i class="fa fa-file-text"></i>編輯資料</h4>
                </div>
                <div class="modal-body">
                    <div id="testmodal2" style="padding: 5px 20px;">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>出貨日期：</b><br />
                                        <asp:TextBox ID="TextBox_Date" runat="server" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>客戶：</b><br />
                                        <asp:TextBox ID="TextBox_Custom" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>機號：</b><br />
                                        <asp:TextBox ID="TextBox_MachineOrder" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>機號備註：</b><br />
                                        <asp:TextBox ID="TextBox_MachineRemark" runat="server" TextMode="MultiLine" style="resize:none;width:251.83px;overflow:hidden" onkeydown="KeyDown()" onkeyup="autogrow(this);" ></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>司機：</b><br />
                                        <asp:DropDownList ID="DropDownList_driver" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>司機備註：</b><br />
                                        <asp:TextBox ID="TextBox_DriverRemark" runat="server" TextMode="MultiLine" style="resize:none;width:251.83px;overflow:hidden" onkeydown="KeyDown()" onkeyup="autogrow(this);" ></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="btn-group btn-group-justified">
                                        <b>時段：</b><br />
                                        <asp:DropDownList ID="DropDownList_period" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default antoclose2" data-dismiss="modal">退出</button>

                    <button id="btnSave" type="button" class="btn btn-primary antosubmit2">儲存</button>

                </div>
            </div>
        </div>
    </div>


    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        function autogrow(textarea) {
            var adjustedHeight = textarea.clientHeight;
            adjustedHeight = Math.max(textarea.scrollHeight, adjustedHeight);
            if (adjustedHeight > textarea.clientHeight)
                textarea.style.height = adjustedHeight + 'px';
            if (textarea.value == '')
                textarea.style.height = 100 + 'px';
        }
        //產生表格的HTML碼
        create_tablehtmlcode('Previous_Shipment', '預出貨列表', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString()%>');
        //產生相對應的JScode
        set_Table('#datatable-buttons');
        //防止頁籤跑版
        loadpage('', '');

        //刪除
        function delete_row(id) {
            var r = confirm("確定要刪除該筆資料嗎");
            if (r == true) {
                $('#<%=TextBox_Delete.ClientID %>').val('' + id + '');
                document.getElementById('<%=Button_Delete.ClientID %>').click();
            }
        }

        //編輯
        function update_row(id, message) {
            var list = message.replaceAll('Θ','\n').split('Ω');
            document.getElementById('<%=TextBox_Date.ClientID %>').value = list[0];
            document.getElementById('<%=TextBox_Custom.ClientID %>').value = list[1];
            document.getElementById('<%=TextBox_MachineOrder.ClientID %>').value = list[2];
            document.getElementById('<%=TextBox_MachineRemark.ClientID %>').value = list[3];
            document.getElementById('<%=DropDownList_driver.ClientID %>').value = list[4];
            document.getElementById('<%=TextBox_DriverRemark.ClientID %>').value = list[5];
            document.getElementById('<%=DropDownList_period.ClientID %>').value = list[6];
            document.getElementById('<%=TextBox_Update.ClientID %>').value = id;
        }

        //編輯按鈕事件
        $("#btnSave").click(function () {
            var WhatSystem = navigator.userAgent;
            if (WhatSystem.match(/(iphone|ipad|ipod);?/i)) {
            } else {
                $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            }
            document.querySelector(".blockUI.blockMsg.blockPage").style.zIndex = 1000000;
            document.getElementById('<%=Button_Update.ClientID%>').disabled = false;
            document.getElementById('<%=Button_Update.ClientID %>').click();
        });

        //查詢事件
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=Button_select.ClientID %>').click();
        });

        function KeyDown() {
            if (event.keyCode == 13) {
                try {
                    document.getElementById('<%=Button_Update.ClientID%>').disabled = true;
                    }
                    catch {

                    }
                }
            }
    </script>
</asp:Content>
