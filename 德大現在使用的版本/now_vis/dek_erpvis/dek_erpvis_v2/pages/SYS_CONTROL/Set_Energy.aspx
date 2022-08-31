<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Set_Energy.aspx.cs" Inherits="dek_erpvis_v2.pages.SYS_CONTROL.Set_Energy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>產量編輯 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
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
        <asp:Label ID="Label_Save" runat="server" Text="" style="display:none"></asp:Label>
        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div id="Set_Energy"></div>
                        <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">
                                        <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                            <div class="col-md-12 col-sm-12 col-xs-12 flex-align col-style">
                                                <div class="col-md-4 col-sm-3 col-xs-4">
                                                    <span>廠區</span>
                                                </div>
                                               <div class="col-md-8 col-sm-9 col-xs-8">
                                                            <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="btn btn-default form-control dropdown-toggle">
                                                                <asp:ListItem Value="sowon" Selected="True">立式廠</asp:ListItem>
                                                                <asp:ListItem Value="dek">大圓盤</asp:ListItem>
                                                                <asp:ListItem Value="iTec">臥式廠</asp:ListItem>
                                                            </asp:DropDownList>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>
                                       <div class="col-md-12 col-sm-12 col-xs-12 col-style">
                                            <div class="col-md-12 col-xs-12 text-align-end">
                                                    <button id="btn_Month" type="button" class="btn btn-primary antosubmit2" data-toggle="modal" data-target="#Insert_Month_Working_People">新增月份工時</button>
                                                </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 col-style">
                                            <div class="col-md-12 col-xs-12 text-align-end">
                                                    <button id="btn_SigleDay" type="button" class="btn btn-primary antosubmit2">新增單日工時</button>
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
    </div>

    <div id="exampleModal_information" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:TextBox ID="TextBox_Number" runat="server" style="display:none"></asp:TextBox>
                <div class="modal-body">
                    <div id="testmodal33" style="padding: 5px 20px;">
                        <div class="form-group">
                            <h5 class="modaltextstyle">
                                <i><b></b></i>
                            </h5>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    請填入產能：<asp:TextBox ID="TextBox_Qty" runat="server" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnchecks" type="button" class="btn btn-primary antosubmit2 ">送出</button>
                    <asp:Button runat="server" Text="提交" ID="Button_Add" OnClick="Button_Add_Click" CssClass="btn btn-primary" Style="display: none" />
                </div>
            </div>
        </div>
    </div>

    <!-- set Modal -->
        <div id="Insert_Month_Working_People" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title modaltextstyle" id="myModalLabel3"><i class="fa fa-file-text"></i>新增月份工時</h4>
                    </div>
                    <div class="modal-body">
                        <div id="Month_Working_People" style="padding: 5px 20px;">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <d>預設每日上班人數:</d><br />
                                            <asp:TextBox ID="Working_People" maxlength="2" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>預設每日工作時數:</b><br />
                                            <asp:TextBox ID="Work_Time" maxlength="2" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>工作站名稱:</b><br />
                                            <asp:DropDownList ID="Workstation" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                            
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>選擇年分:</b><br/>
                                            <select id="select_Year"></select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="btn-group btn-group-justified">
                                            <b>選擇月分:</b><br/>
                                            <select id="select_Month"></select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="Insert_Btn_Cancel" type="button" class="btn btn-default antoclose2" data-dismiss="modal">退出</button>
                       
                        <button id="Insert_btnSave" type="button" onclick="insertValue()" class="btn btn-primary antosubmit2">新增</button>
                    </div>
                </div>
            </div>
        </div>
        <!--/set Modal-->




    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        $(document).ready(function () {
            var date = new Date();
            var Year = date.getFullYear();
            var Month = date.getMonth() + 1;
            //設定年範圍
            for (var i = 2022; i <= 2100; i++) {
                $('#select_Year').append($('<option>').val(i).text(i));
                $('#select_Year').css("width","86px");
            }
            //設定月範圍
            for (var i = 1; i <= 12; i++) {
                $('#select_Month').append($('<option>').val(i).text(i));
                $('#select_Month').css("width", "86px");
            }
            //預設當下年月
            $("#select_Year option").each(function () {
                if ($(this).text() == Year)
                    $(this).attr("selected", "selected");
            });
            $("#select_Month option").each(function () {
                if ($(this).text() == Month) {
                    $(this).attr("selected", "selected");
                }
                    
            });
        });
        


        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();
        });
        $("#btnchecks").click(function () {

            if (parseInt(document.getElementById("<%=TextBox_Qty.ClientID %>").value, 10) >= 0) {
                $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                document.getElementById('<%=Button_Add.ClientID %>').click();
            } else
                alert('請輸入正數');

        });
        //產生表格的HTML碼
        create_tablehtmlcode('Set_Energy', '產量編輯', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString()%>');
        //產生相對應的JScode
        set_Table('#datatable-buttons');
        //防止頁籤跑版
        loadpage('', '');

        function Set_Value(product_Line,capacity) {
            document.getElementById("<%=TextBox_Qty.ClientID%>").value = capacity;
            document.getElementById("<%=TextBox_Number.ClientID %>").value = product_Line;
        }
    </script>
</asp:Content>
