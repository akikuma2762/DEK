<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="pick_list.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PD.pick_list_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>領料單 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>

    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_PM/Asm_history.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .row_update {
            margin-left: -3.8px;
        }
    </style>

    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁</a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PCD">資材部</a></u></li>
        </ol>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true">領料總表</a>
            </li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" id="profile-tab" role="tab" data-toggle="tab" aria-expanded="false">個別領料單</a>
            </li>
        </ul>
        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">
                <div class="row row_update">
                    <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel Div_Shadow">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div id="pick_list"></div>
                                <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                                    <div class="x_panel">
                                        <div class="x_content">
                                            <!--計數器用-->
                                            <label id="Record_Number" style="display: none">0</label>
                                            <!--後端運算用-->
                                            <asp:TextBox ID="TextBox_Record" runat="server" Style="display: none"></asp:TextBox>
                                            <asp:PlaceHolder ID="PlaceHolder_hide" runat="server">
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-3 col-xs-4" style="margin: 5px 0px 5px 0px">
                                                        <span>廠區</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-9 col-xs-8">
                                                        <asp:DropDownList ID="dropdownlist_Factory" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%">
                                                            <asp:ListItem Value="sowon" Selected="True">立式廠</asp:ListItem>
                                                            <asp:ListItem Value="dek">大圓盤</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </asp:PlaceHolder>
                                            <div id="add_element"></div>
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                            </div>

                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="col-md-9 col-xs-8">
                                                    <button type="button" class="btn btn-info" onclick="add_content('add_element')">新增項目</button>
                                                </div>
                                                <div class="col-md-3 col-xs-4">
                                                    <asp:Button ID="button_select" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="Unnamed_ServerClick" Style="display: none" />
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
            <div role="tabpanel" class="tab-pane fade" id="tab_content2" aria-labelledby="profile-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="x_content">
                                <%=all_div %>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%=Use_Javascript.Quote_Javascript() %>
    <script>

        //防止切換頁籤時跑版
        $(document).ready(function () {
            //需加，避免DIV跑版
            jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');

            //網頁一開始，產生8個TEXTBOX給使用者輸入
            for (i = 0; i < 9; i++)
                add_content('add_element');
        });


        //產生表格的HTML碼
        create_tablehtmlcode('pick_list', '總領料單', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString()%>');
        //產生相對應的JScode
        set_Table('#datatable-buttons');
        //防止頁籤跑版
        loadpage('', '');


        $("#btncheck").click(function () {
            var Number = '';
            //組合使用者選的刀庫 || 製令
            for (i = 0; i < parseInt(document.getElementById('Record_Number').innerHTML, 10); i++)
                Number += document.getElementById('textbox_' + i).value + '#';
            //寫入TEXTBOX內，讓後端進行運算
            document.getElementById('<%=TextBox_Record.ClientID %>').value = Number;
            //執行事件
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();
        });

        //新增文本輸入框的事件
        function add_content(div) {
            var Number = '';

            //先儲存數值，以免按下新增時要重打
            for (i = 0; i < parseInt(document.getElementById('Record_Number').innerHTML, 10); i++) {
                Number += `textbox_${i}` + '#' + document.getElementById('textbox_' + i).value + '#';
            }
            //取得要新增的DIV名稱
            var divname = document.getElementById(div);
            //新增控制項到該DIV內
            divname.innerHTML = divname.innerHTML +
                '<div class="col-md-12 col-sm-12 col-xs-12">' +
                '<div class="col-md-4 col-sm-12 col-xs-5" style="margin: 5px 0px 5px 0px">' +
                ' <span>刀庫編號 /' +
                ' <br />' +
                '製令單號</span>' +
                ' </div>' +
                ' <div class="col-md-8 col-sm-12 col-xs-7" style="margin: 15px 0px 5px 0px">' +
                `<input type="text" id="textbox_${document.getElementById('Record_Number').innerHTML}" name="textbox_${document.getElementById('Record_Number').innerHTML}" class="form-control text-left"  >` +
                '  </div>'
            '</div>';

            //回復原本的數值
            if (document.getElementById('Record_Number').innerHTML != '0') {
                var valuelist = Number.split("#");
                for (j = 0; j < valuelist.length - 1; j++) {
                    document.getElementById(valuelist[j]).value = valuelist[j + 1];
                    j++;
                }
            }
            //把Label+1(需先轉換 文字->數字)
            document.getElementById('Record_Number').innerHTML = parseInt(document.getElementById('Record_Number').innerHTML, 10) + 1;
        }

        //產生個別資料表時，需用到的JS
        <%=all_js %>
    </script>
</asp:Content>
