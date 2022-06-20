<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Write_ShipmentInformation.aspx.cs" Inherits="dek_erpvis_v2.pages.SYS_CONTROL.Write_ShipmentInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>出貨撰寫 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
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

        <!-----------------/title------------------>
        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <label id="Record_Number" style="display:none">0</label>
                        <asp:TextBox ID="TextBox_Content" runat="server" style="display:none"></asp:TextBox>
                        <div id="Write_ShipmentInformation">
                        </div>
                        <div class="col-md-10 col-xs-8">
                            <button type="button" class="btn btn-info" onclick="add_divcontent('Write_ShipmentInformation')">新增項目</button>
                        </div>
                         <div class="col-md-2 col-xs-4" >
                       <asp:Button ID="Button_Add" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="Button_Add_Click" Style="display: none" />
                        <button id="btncheck" type="button" class="btn btn-primary antosubmit2" style="position:absolute;right:2px">儲存</button>
                    </div>
                    </div>
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
        //進入後事件
        $(document).ready(function () {
            //網頁一開始，產生3個TEXTBOX給使用者輸入
            for (i = 0; i < 3; i++)
                add_divcontent('Write_ShipmentInformation');
        });

        //新增填寫內容
        function add_divcontent(div) {
            //司機的名字部分
            var driver = '<%=WebUtils.GetAppSettings("driver_name") %>';
            var driver_list = driver.split(',');
            var driver_dropdownlistvalue = '';
            for (x = 0; x < driver_list.length; x++)
                driver_dropdownlistvalue += `<option>${driver_list[x]}</option>`;

            //時段
            var period = '<%=WebUtils.GetAppSettings("period") %>';
            var period_list = period.split(',');
            var period_dropdownlistvalue = '';
            for (x = 0; x < period_list.length; x++)
                period_dropdownlistvalue += `<option>${period_list[x]}</option>`;

            var information = '';
            //預存數值，避免刷新(存入日期 客戶 機號 機號備註 司機 司機備註 趟數)
            for (i = 0; i < parseInt(document.getElementById('Record_Number').innerHTML, 10); i++) {
                information += `textbox_date${i}` + 'Ω' + document.getElementById('textbox_date' + i).value + 'Ω';
                information += `textbox_custom${i}` + 'Ω' + document.getElementById('textbox_custom' + i).value + 'Ω';
                information += `textbox_machineid${i}` + 'Ω' + document.getElementById('textbox_machineid' + i).value + 'Ω';
                information += `textbox_machineremark${i}` + 'Ω' + document.getElementById('textbox_machineremark' + i).value + 'Ω';
                information += `dropdownlist_driver${i}` + 'Ω' + document.getElementById('dropdownlist_driver' + i).value + 'Ω';
                information += `textbox_driverremark${i}` + 'Ω' + document.getElementById('textbox_driverremark' + i).value + 'Ω';
                information += `dropdownlist_period${i}` + 'Ω' + document.getElementById('dropdownlist_period' + i).value + 'Ω';
            }
            //取得要新增的DIV名稱
            var divname = document.getElementById(div);
            divname.innerHTML = divname.innerHTML +
                '<div class="col-md-12 col-sm-12 col-xs-12">' +
                    '<div class="col-md-3 col-sm-12 col-xs-12">' +
                        '<div class="col-md-4 col-sm-12 col-xs-5" align="center" style="margin: 5px 0px 5px 0px">' +
                            '<span style="font-size:20px">日期 </span>' +
                        '</div>' +
                        '<div class="col-md-8 col-sm-12 col-xs-7" style="margin: 5px 0px 5px 0px">' +
                            `<input type="date" onkeydown=KeyDown() id="textbox_date${document.getElementById('Record_Number').innerHTML}" name="textbox_date${document.getElementById('Record_Number').innerHTML}" class="form-control text-center"  >` +
                        '</div>' +
                    '</div>' +
                    '<div class="col-md-3 col-sm-12 col-xs-12">' +
                        '<div class="col-md-4 col-sm-12 col-xs-5" align="center" style="margin: 5px 0px 5px 0px">' +
                            '<span style="font-size:20px">客戶 </span>' +
                        '</div>' +
                        '<div class="col-md-8 col-sm-12 col-xs-7" style="margin: 5px 0px 5px 0px">' +
                            `<input type="text" onkeydown=KeyDown() id="textbox_custom${document.getElementById('Record_Number').innerHTML}" name="textbox_custom${document.getElementById('Record_Number').innerHTML}" class="form-control text-center"  >` +
                        '</div>' +
                    '</div>' +
                    '<div class="col-md-3 col-sm-12 col-xs-12">' +
                        '<div class="col-md-4 col-sm-12 col-xs-5" align="center" style="margin: 5px 0px 5px 0px">' +
                            '<span style="font-size:20px">機號 </span>' +
                        '</div>' +
                        '<div class="col-md-8 col-sm-12 col-xs-7" style="margin: 5px 0px 5px 0px">' +
                            `<textarea name="textbox_machineid${document.getElementById('Record_Number').innerHTML}" style="resize:none;width:251.83px;overflow:hidden" rows="2" cols="20" id="textbox_machineid${document.getElementById('Record_Number').innerHTML}" onkeydown="KeyDown()" onkeyup="autogrow(this);"></textarea>` +
                        '</div>' +
                    '</div>' +
                    '<div class="col-md-3 col-sm-12 col-xs-12">' +
                        '<div class="col-md-4 col-sm-12 col-xs-5" align="center" style="margin: 5px 0px 5px 0px">' +
                            '<span style="font-size:20px">機號備註 </span>' +
                        '</div>' +
                        '<div class="col-md-8 col-sm-12 col-xs-7" style="margin: 5px 0px 5px 0px">' +
                            `<textarea name="textbox_machineremark${document.getElementById('Record_Number').innerHTML}" style="resize:none;width:251.83px;overflow:hidden" rows="2" cols="20" id="textbox_machineremark${document.getElementById('Record_Number').innerHTML}" onkeydown="KeyDown()" onkeyup="autogrow(this);"></textarea>`+
                        '</div>' +
                    '</div>' +
                '</div>' +

                '<div class="col-md-12 col-sm-12 col-xs-12">' +
                    '<div class="col-md-3 col-sm-12 col-xs-12">' +
                        '<div class="col-md-4 col-sm-12 col-xs-5" align="center" style="margin: 5px 0px 5px 0px">' +
                            '<span style="font-size:20px">司機 </span>' +
                        '</div>' +
                        '<div class="col-md-8 col-sm-12 col-xs-7" style="margin: 5px 0px 5px 0px">' +
                          /*  `<input type="text" onkeydown=KeyDown() id="textbox_driver${document.getElementById('Record_Number').innerHTML}" name="textbox_driver${document.getElementById('Record_Number').innerHTML}" class="form-control text-center"  >` +*/

                            ` <select class="form-control text-center" id="dropdownlist_driver${document.getElementById('Record_Number').innerHTML}" >` +
                                driver_dropdownlistvalue +
                            `</select>` +
                        '</div>' +
                    '</div>' +
                    '<div class="col-md-3 col-sm-12 col-xs-12">' +
                        '<div class="col-md-4 col-sm-12 col-xs-5" align="center" style="margin: 5px 0px 5px 0px">' +
                            '<span style="font-size:20px">司機備註 </span>' +
                        '</div>' +
                        '<div class="col-md-8 col-sm-12 col-xs-7" style="margin: 5px 0px 5px 0px">' +              
                            `<textarea name="textbox_driverremark${document.getElementById('Record_Number').innerHTML}" style="resize:none;width:251.83px;overflow:hidden" rows="2" cols="20" id="textbox_driverremark${document.getElementById('Record_Number').innerHTML}"  onkeydown="KeyDown()" onkeyup="autogrow(this);"></textarea>` +
                        '</div>' +
                    '</div>' +
                    '<div class="col-md-3 col-sm-12 col-xs-12">' +
                        '<div class="col-md-4 col-sm-12 col-xs-5" align="center" style="margin: 5px 0px 5px 0px">' +
                            '<span style="font-size:20px">時段 </span>' +
                        '</div>' +
                        '<div class="col-md-8 col-sm-12 col-xs-7" align="center" style="margin: 5px 0px 5px 0px">' +
                        //    `<input type="number" onkeydown=KeyDown() min="1" id="textbox_tripnumber${document.getElementById('Record_Number').innerHTML}" name="textbox_tripnumber${document.getElementById('Record_Number').innerHTML}" class="form-control text-center"  >` +
                            ` <select class="form-control text-center" id="dropdownlist_period${document.getElementById('Record_Number').innerHTML}" >` +
                                period_dropdownlistvalue +
                            `</select>` +
                        '</div>' +
                    '</div>' +

                    '<div class="col-md-12 col-sm-12 col-xs-12">' +
                        '<hr />'
                    '</div>' +

                '</div>';


            //回復原本的數值
            if (document.getElementById('Record_Number').innerHTML != '0') {
                var valuelist = information.split("Ω");
                for (j = 0; j < valuelist.length - 1; j++) {
                    document.getElementById(valuelist[j]).value = valuelist[j + 1];
                    j++;
                }
            }
            //把Label+1(需先轉換 文字->數字)
            document.getElementById('Record_Number').innerHTML = parseInt(document.getElementById('Record_Number').innerHTML, 10) + 1;
        }


        $("#btncheck").click(function () {
            var information = '';
            //組合使用者選的刀庫 || 製令
            for (i = 0; i < parseInt(document.getElementById('Record_Number').innerHTML, 10); i++) {
                if (document.getElementById('textbox_date' + i).value != '' || document.getElementById('textbox_machineid' + i).value != '') {
                    information += document.getElementById('textbox_date' + i).value + 'Ω';
                    information += document.getElementById('textbox_custom' + i).value + 'Ω';
                    information += document.getElementById('textbox_machineid' + i).value + 'Ω';
                    information += document.getElementById('textbox_machineremark' + i).value + 'Ω';
                    information += document.getElementById('dropdownlist_driver' + i).value + 'Ω';
                    information += document.getElementById('textbox_driverremark' + i).value + 'Ω';
                    information += document.getElementById('dropdownlist_period' + i).value + 'Ω';
                }
            }
            if (information != '') {
                //寫入TEXTBOX內，讓後端進行運算
                document.getElementById('<%=TextBox_Content.ClientID %>').value = information;
                document.getElementById('<%=Button_Add.ClientID%>').disabled = false;
                document.getElementById('<%=Button_Add.ClientID %>').click();
            }
            else
                alert('未填寫任何資料');

        });

        function KeyDown() {
            if (event.keyCode == 13) {
                try {
                    document.getElementById('<%=Button_Add.ClientID%>').disabled = true;
               }
               catch {

               }
            }
        }

    </script>
</asp:Content>
