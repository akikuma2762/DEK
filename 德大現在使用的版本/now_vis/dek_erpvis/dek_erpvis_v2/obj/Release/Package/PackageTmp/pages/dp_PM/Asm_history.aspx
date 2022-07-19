<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Asm_history.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PM.Asm_history" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>維護歷史搜尋 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <link href="../../Content/dp_PM/Asm_history.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/Default_input.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁</a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PMD">生產部</a></u></li>


        </ol>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_panel">
                        <div class="x_content">
                            <div id="Asm_history"></div>

                            <div class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                                <!--計數器用-->
                                <label id="Record_Number" style="display: none">0</label>
                                <!--後端運算用-->
                                <asp:TextBox ID="TextBox_Record" runat="server" Style="display: none"></asp:TextBox>
                                <div class="x_panel">
                                    <div class="x_content">
                                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                                        </asp:ScriptManager>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                            <ContentTemplate>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-12 col-xs-5" style="margin: 5px 0px 5px 0px">
                                                        <span>廠區選擇</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-12 col-xs-7" style="margin: 0px 0px 5px 0px">
                                                        <asp:DropDownList ID="DropDownList_type" runat="server" AutoPostBack="true" class="form-control text-center" OnSelectedIndexChanged="DropDownList_type_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Value="0">立式</asp:ListItem>
                                                            <asp:ListItem Value="1">臥式</asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-6 col-xs-12">
                                                    <div class="col-md-4 col-sm-12 col-xs-5" style="margin: 5px 0px 5px 0px">
                                                        <span>產線名稱</span>
                                                    </div>
                                                    <div class="col-md-8 col-sm-12 col-xs-7" style="margin: 0px 0px 5px 0px">
                                                        <asp:DropDownList ID="DropDownList_Line" runat="server" class="form-control text-center">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="col-md-12 col-sm-6 col-xs-12">
                                            <div class="col-md-4 col-sm-3 col-xs-5" style="margin: 5px 0px 5px 0px">
                                                <span>日期選擇</span>
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-7" style="margin: 0px 0px 5px 0px">
                                                <asp:TextBox ID="textbox_dt1" runat="server" Style="" TextMode="Date" CssClass="form-control   text-center"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-6 col-xs-12">
                                            <div class="col-md-4 col-sm-3 col-xs-5">
                                            </div>
                                            <div class="col-md-8 col-sm-9 col-xs-7" style="margin: 0px 0px 5px 0px">
                                                <asp:TextBox ID="textbox_dt2" runat="server" CssClass="form-control  text-center" TextMode="Date"></asp:TextBox>
                                            </div>
                                        </div>




                                        <div id="add_element"></div>
                                        <div class="col-md-12 col-sm-12 col-xs-12 col-style">
                                            <div class="col-md-12 col-xs-12 text-align-end">
                                                <button type="button" class="btn btn-info" style="margin: 0px;" onclick="add_content('add_element')">新增項目</button>
                                            </div>
                                        </div>

                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div class="col-md-6 col-xs-8">
                                            </div>
                                            <div class="col-md-3 col-xs-12">
                                                <button id="btnprint" type="button" class="btn btn-success">列印問題</button>
                                                <asp:Button ID="Button_Print" runat="server" Text="列印問題" Style="display: none" class="btn btn-success" OnClick="Button_Print_Click" Visible="false" />
                                            </div>
                                            <div class="col-md-3 col-xs-12" style="text-align: right;">
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
    </div>

    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        //防止切換頁籤時跑版
        $(document).ready(function () {
            //$('#example').DataTable({
            //    responsive: true
            //});
            //$('#exampleInTab').DataTable({
            //    responsive: true
            //});
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                $($.fn.dataTable.tables(true)).DataTable()
                    .columns.adjust()
                    .responsive.recalc();
            });
            //網頁一開始，產生8個TEXTBOX給使用者輸入
            for (i = 0; i < 3; i++)
                add_content('add_element');
        });
        $(document).ready(function () {
            var tharray = [];
            $('#tr_row > th').each(function () {
                tharray.push($(this).text())
            })
            if (tharray[0] == '沒有資料載入')
                document.getElementById('btnprint').style.display = 'none';
            else
                document.getElementById('btnprint').style.display = 'initial';
        });
        $("#btncheck").click(function () {
            var Number = '';
            //組合使用者選的刀庫 || 製令
            for (i = 0; i < parseInt(document.getElementById('Record_Number').innerHTML, 10); i++)
                Number += document.getElementById('textbox_' + i).value + '#';


            //寫入TEXTBOX內，讓後端進行運算
            document.getElementById('<%=TextBox_Record.ClientID %>').value = Number;
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=button_select.ClientID %>').click();

        });

        $("#btnprint").click(function () {
            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=Button_Print.ClientID %>').click();
            $.unblockUI();
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
                ' <span>刀庫編號</span>' +
                ' </div>' +
                ' <div class="col-md-8 col-sm-12 col-xs-7" style="margin: 5px 0px 5px 0px">' +
                `<input type="text" id="textbox_${document.getElementById('Record_Number').innerHTML}" name="textbox_${document.getElementById('Record_Number').innerHTML}" class="form-control text-center"  >` +
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
        create_tablehtmlcode('Asm_history', '組裝歷史查詢', 'table-form', "<%=ColumnsData.ToString() %>", "<%=RowsData.ToString() %>");
        //產生相對應的JScode
        set_Table('#table-form');
    </script>
</asp:Content>
