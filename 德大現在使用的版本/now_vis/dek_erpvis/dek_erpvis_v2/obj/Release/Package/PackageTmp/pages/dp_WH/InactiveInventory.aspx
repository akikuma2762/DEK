<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="InactiveInventory.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_WH.InactiveInventory_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>呆滯物料統計表 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" media="screen and (max-width:768px)" />
    <link href="../../Content/dp_WH/InactiveInventory.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .th_centet {
            text-align: center;
        }
    </style>
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <%=path %>
        <br>
        <div class="clearfix"></div>

        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div id="InactiveInventory"></div>
                        <div class="col-md-3 col-sm-12 col-xs-12 padding-left-right-4" runat="server" id="div_present">
                            <div class="x_panel">
                                <table id="TB" class="table table-ts table-bordered nowrap" cellspacing="0" width="100%">
                                    <thead>
                                        <tr id="tr_row">
                                            <th class="th_centet">使用率</th>
                                            <th class="th_centet">呆滯金額</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>0%~25% ：</b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <%=persent[0] %>
                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>26%~50% ：</b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <%=persent[1] %>
                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>51%~75% ：</b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <%=persent[2] %>
                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>75%~100% ：</b>
                                            </td>
                                            <td align="center" style='font-size: 20px; color: black'>
                                                <b>
                                                    <%=persent[3] %>
                                                </b>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="dashboard_graph x_panel">
                                    <div class="x_content">

                                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                                        </asp:ScriptManager>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                            <ContentTemplate>
                                                <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                                    <div class="col-md-12 col-sm-12 col-xs-12  col-style">
                                                        <div class="col-md-4 col-sm-3 col-xs-4">
                                                            <span>廠區</span>
                                                        </div>
                                                        <div class="col-md-8 col-sm-9 col-xs-8">
                                                            <asp:DropDownList ID="dropdownlist_Factory" runat="server" AutoPostBack="true" CssClass="btn btn-default dropdown-toggle" Width="100%" OnSelectedIndexChanged="dropdownlist_Factory_SelectedIndexChanged">
                                                                <asp:ListItem Value="sowon" Selected="True">立式廠</asp:ListItem>
                                                                <asp:ListItem Value="dek">大圓盤</asp:ListItem>
                                                                                            <asp:ListItem Value="iTec">臥式廠</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </asp:PlaceHolder>
                                                <div class="col-md-12 col-sm-12 col-xs-12  col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" >
                                                        <span>存放儲位</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                        <asp:CheckBoxList ID="CheckBoxList_spaces" Font-Names="NotoSans" runat="server" onclick="all_check('ContentPlaceHolder1_CheckBoxList_spaces')" CssClass="table-striped">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <hr />
                                                <div class="col-md-12 col-sm-12 col-xs-12  col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" >
                                                        <span>庫存天數</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                        <asp:TextBox ID="TextBoxdayval" class="form-control" runat="server" TextMode="Number" Text="180"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <hr />
                                                <div class="col-md-12 col-sm-12 col-xs-12  col-style">
                                                    <div class="col-md-4 col-sm-3 col-xs-4">
                                                        <span>物料類別</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8" style="margin: 0px 0px 5px 0px">
                                                        <asp:DropDownList ID="DropDownList_itemtype" Style="display: none" Width="100%" Font-Names="NotoSans" runat="server" CssClass="btn btn-default dropdown-toggle">
                                                        </asp:DropDownList>
                                                        <asp:CheckBoxList ID="CheckBoxList_itemtype" Font-Names="NotoSans" runat="server" CssClass="table-striped" onclick="all_check('ContentPlaceHolder1_CheckBoxList_itemtype')"></asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="col-md-12 col-sm-1 col-xs-12" style="margin: 0px 0px 0px 10px">
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12  col-style">
                                                <div class="col-md-12 col-xs-12 text-align-end">
                                                <asp:Button ID="button_select" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="button_select_Click" Style="display: none" />
                                                <button id="btncheck" type="button" class="btn btn-primary antosubmit2">執行運算</button>
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
    </div>

    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        $("#btncheck").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();
        });

        //產生表格的HTML碼
        create_tablehtmlcode('InactiveInventory', '呆滯物料統計表', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString()%>');
        //產生相對應的JScode
        set_Table('#datatable-buttons');
        //防止頁籤跑版
        loadpage('', '');
    </script>
</asp:Content>
