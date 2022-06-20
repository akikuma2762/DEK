<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Color_Change.aspx.cs" Inherits="dek_erpvis_v2.pages.SYS_CONTROL.Color_Change" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>顏色設定 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <link href="../../assets/build/css/custom_person.css" rel="stylesheet">
    <link href="../../assets/build/css/Change_Table_Button_person.css" rel="stylesheet">
    <!--<link href="../../assets/build/css/custom.css" rel="stylesheet" />
    <link href="../../assets/build/css/Change_Table_Button.css" rel="stylesheet" />-->
    <style>
        #colortest {
            background: var(--Color_Label);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-----------------title------------------>
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <!--跑版加入這個即可<div class="row"></div>-->
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="dashboard_graph x_panel" id="Div_Shadow">
                    <div class="x_content">
                        <div class="row">

                            <!--變更顏色的數值-->
                            <div class="controls">
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <h2>側欄顏色</h2>
                                    <input type="color" id="Color_Sidebar" name="Color_Sidebar" list="colors" value="#000000">
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <h2>側欄文字顏色</h2>
                                    <input type="color" id="Color_SidebarText" name="Color_SidebarText" list="colors" value="#ffffff">
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <h2>標籤顏色</h2>
                                    <input type="color" id="Color_Label" name="Color_Label" list="colors" value="#000000">
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <h2>背景顏色</h2>
                                    <input type="color" id="Color_Background" name="Color_Background" list="colors" value="#000000">
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <h2>欄位顏色</h2>
                                    <input type="color" id="Color_Column" name="Color_Column" list="colors" value="#000000">
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <h2>欄位文字顏色</h2>
                                    <input type="color" id="Color_ColumnText" name="Color_ColumnText" list="colors" value="#ffffff">
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <h2>表格顏色</h2>
                                    <input type="color" id="Color_DataTable" name="Color_DataTable" list="colors" value="#000000">
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <h2>表格文字顏色</h2>
                                    <input type="color" id="Color_DataTableText" name="Color_DataTableText" list="colors" value="#ffffff">
                                </div>
                            </div>
                            <%--                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <h2>側欄顏色</h2>
                                <div class="controlS" style="width: 80%;">
                                    <div class="input">
                                        <!--得到range的數值    
                                            <input type="range" name="ageInputName" id="ageInputId" value="24" min="1" max="100" oninput="ageOutputId.value = ageInputId.value">
                                            <output name="ageOutputName" id="ageOutputId">24</output>
                                            -->
                                        <label for="RedB">R：</label><input id="RedSV" name="RedSV" style="width: 80px" disabled="disabled">
                                        <input id="RedS" type="range" name="RedS" min="0" max="255" value="0" data-unit="" oninput="RedSV.value = RedS.value" />
                                    </div>
                                    <div class="input">
                                        <label for="GreenB">G：</label><input id="GreenSV" name="GreenSV" style="width: 80px" disabled="disabled">
                                        <input id="GreenS" type="range" name="GreenS" min="0" max="255" value="0" data-unit="" oninput="GreenSV.value = GreenS.value" />
                                    </div>
                                    <div class="input">
                                        <label for="BlueB">B：</label><input id="BlueSV" name="BlueSV" style="width: 80px" disabled="disabled">
                                        <input id="BlueS" type="range" name="BlueS" min="0" max="255" value="0" data-unit="" oninput="BlueSV.value = BlueS.value" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <h2>側欄文字顏色</h2>
                                <div class="controlTS" style="width: 80%;">
                                    <div class="input">
                                        <!--得到range的數值    
                                            <input type="range" name="ageInputName" id="ageInputId" value="24" min="1" max="100" oninput="ageOutputId.value = ageInputId.value">
                                            <output name="ageOutputName" id="ageOutputId">24</output>
                                            -->
                                        <label for="RedB">R：</label><input id="RedTSV" name="RedTSV" style="width: 80px" disabled="disabled">
                                        <input id="RedTS" type="range" name="RedTS" min="0" max="255" value="0" data-unit="" oninput="RedTSV.value = RedTS.value" />
                                    </div>
                                    <div class="input">
                                        <label for="GreenB">G：</label><input id="GreenTSV" name="GreenTSV" style="width: 80px" disabled="disabled">
                                        <input id="GreenTS" type="range" name="GreenTS" min="0" max="255" value="0" data-unit="" oninput="GreenTSV.value = GreenTS.value" />
                                    </div>
                                    <div class="input">
                                        <label for="BlueB">B：</label><input id="BlueTSV" name="BlueTSV" style="width: 80px" disabled="disabled">
                                        <input id="BlueTS" type="range" name="BlueTS" min="0" max="255" value="0" data-unit="" oninput="BlueTSV.value = BlueTS.value" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <h2>標籤顏色</h2>
                                <div class="controlL" style="width: 80%;">
                                    <div class="input">
                                        <label for="RedB">R：</label><input id="RedLV" name="RedLV" style="width: 80px" disabled="disabled">
                                        <input id="RedL" type="range" name="RedL" min="0" max="255" value="0" data-unit="" oninput="RedLV.value = RedL.value" />
                                    </div>
                                    <div class="input">
                                        <label for="GreenB">G：</label><input id="GreenLV" name="GreenLV" style="width: 80px" disabled="disabled">
                                        <input id="GreenL" type="range" name="GreenL" min="0" max="255" value="0" data-unit="" oninput="GreenLV.value = GreenL.value" />
                                    </div>
                                    <div class="input">
                                        <label for="BlueB">B：</label><input id="BlueLV" name="BlueLV" style="width: 80px" disabled="disabled">
                                        <input id="BlueL" type="range" name="BlueL" min="0" max="255" value="0" data-unit="" oninput="BlueLV.value = BlueL.value" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <!--背景顏色-->
                                <h2>背景顏色</h2>
                                <div class="controlB" style="width: 80%;">
                                    <div class="input">
                                        <label for="RedB">R：</label><input id="RedBV" name="RedBV" style="width: 80px" disabled="disabled">
                                        <input id="RedB" type="range" name="RedB" min="0" max="255" value="0" data-unit="" oninput="RedBV.value = RedB.value" />
                                    </div>

                                    <div class="input">
                                        <label for="GreenB">G：</label><input id="GreenBV" name="GreenBV" style="width: 80px" disabled="disabled">
                                        <input id="GreenB" type="range" name="GreenB" min="0" max="255" value="0" data-unit="" oninput="GreenBV.value = GreenB.value" />
                                    </div>

                                    <div class="input">
                                        <label for="BlueB">B：</label><input id="BlueBV" name="BlueBV" style="width: 80px" disabled="disabled">
                                        <input id="BlueB" type="range" name="BlueB" min="0" max="255" value="0" data-unit="" oninput="BlueBV.value = BlueB.value" />
                                    </div>

                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <!--欄位顏色-->
                                <h2>欄位顏色</h2>
                                <div class="controlT" style="width: 80%;">
                                    <div class="input">
                                        <label for="RedB">R：</label><input id="RedTV" name="RedTV" style="width: 80px" disabled="disabled">
                                        <input id="RedT" type="range" name="RedT" min="0" max="255" value="0" data-unit="" oninput="RedTV.value = RedT.value" />
                                    </div>

                                    <div class="input">
                                        <label for="GreenB">G：</label><input id="GreenTV" name="GreenTV" style="width: 80px" disabled="disabled">
                                        <input id="GreenT" type="range" name="GreenT" min="0" max="255" value="0" data-unit="" oninput="GreenTV.value = GreenT.value" />
                                    </div>

                                    <div class="input">
                                        <label for="BlueB">B：</label><input id="BlueTV" name="BlueTV" style="width: 80px" disabled="disabled">
                                        <input id="BlueT" type="range" name="BlueT" min="0" max="255" value="0" data-unit="" oninput="BlueTV.value = BlueT.value" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <!--欄位文字顏色-->
                                <h2>欄位文字顏色</h2>
                                <div class="controlTT" style="width: 80%;">
                                    <div class="input">
                                        <label for="RedB">R：</label><input id="RedTTV" name="RedTTV" style="width: 80px" disabled="disabled">
                                        <input id="RedTT" type="range" name="RedTT" min="0" max="255" value="0" data-unit="" oninput="RedTTV.value = RedTT.value" />
                                    </div>

                                    <div class="input">
                                        <label for="GreenB">G：</label><input id="GreenTTV" name="GreenTTV" style="width: 80px" disabled="disabled">
                                        <input id="GreenTT" type="range" name="GreenTT" min="0" max="255" value="0" data-unit="" oninput="GreenTTV.value = GreenTT.value" />
                                    </div>

                                    <div class="input">
                                        <label for="BlueB">B：</label><input id="BlueTTV" name="BlueTTV" style="width: 80px" disabled="disabled">
                                        <input id="BlueTT" type="range" name="BlueTT" min="0" max="255" value="0" data-unit="" oninput="BlueTTV.value = BlueTT.value" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <!--表格顏色-->
                                <h2>表格顏色</h2>
                                <div class="controlZ" style="width: 80%;">
                                    <div class="input">
                                        <label for="RedB">R：</label><input id="RedZV" name="RedZV" style="width: 80px" disabled="disabled">
                                        <input id="RedZ" type="range" name="RedZ" min="0" max="255" value="0" data-unit="" oninput="RedZV.value = RedZ.value" />
                                    </div>

                                    <div class="input">
                                        <label for="GreenB">G：</label><input id="GreenZV" name="GreenZV" style="width: 80px" disabled="disabled">
                                        <input id="GreenZ" type="range" name="GreenZ" min="0" max="255" value="0" data-unit="" oninput="GreenZV.value = GreenZ.value" />
                                    </div>

                                    <div class="input">
                                        <label for="BlueB">B：</label><input id="BlueZV" name="BlueZV" style="width: 80px" disabled="disabled">
                                        <input id="BlueZ" type="range" name="BlueZ" min="0" max="255" value="0" data-unit="" oninput="BlueZV.value = BlueZ.value" />
                                    </div>

                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <!--表格顏色-->
                                <h2>表格文字顏色</h2>
                                <div class="controlTZ" style="width: 80%;">
                                    <div class="input">
                                        <label for="RedB">R：</label><input id="RedTZV" name="RedTZV" style="width: 80px" disabled="disabled">
                                        <input id="RedTZ" type="range" name="RedTZ" min="0" max="255" value="0" data-unit="" oninput="RedTZV.value = RedTZ.value" />
                                    </div>

                                    <div class="input">
                                        <label for="GreenB">G：</label><input id="GreenTZV" name="GreenTZV" style="width: 80px" disabled="disabled">
                                        <input id="GreenTZ" type="range" name="GreenTZ" min="0" max="255" value="0" data-unit="" oninput="GreenTZV.value = GreenTZ.value" />
                                    </div>

                                    <div class="input">
                                        <label for="BlueB">B：</label><input id="BlueTZV" name="BlueTZV" style="width: 80px" disabled="disabled">
                                        <input id="BlueTZ" type="range" name="BlueTZ" min="0" max="255" value="0" data-unit="" oninput="BlueTZV.value = BlueTZ.value" />
                                    </div>

                                </div>
                            </div>--%>
                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <h2>表格測試</h2>
                                <table class="table-bordered nowrap table-ts" id="tr_row " width="80%">
                                    <thead>
                                        <tr id="tr_row">
                                            <%=th %>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%=tr %>
                                    </tbody>
                                </table>
                            </div>
                            <br>

                            <div class="clearfix"></div>
                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <input type="button" value="儲存" onclick="Saveclick()" class="btn btn-primary antosubmit2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <input type="button" value="返回參數設定畫面" onclick="backpage()" class="btn btn-danger">
                                <asp:Button ID="button_select" runat="server" Text="保存" class="btn btn-secondary" OnClick="button_select_Click" Style="display: none" />
                            </div>


                        </div>
                    </div>
                </div>
                <!-----------------/content------------------>
            </div>
        </div>
    </div>
    <!-- jQuery -->
    <script src="../../assets/vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="../../assets/vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- FastClick -->
    <script src="../../assets/vendors/fastclick/lib/fastclick.js"></script>
    <!-- iCheck -->
    <script src="../../assets/vendors/iCheck/icheck.min.js"></script>
    <!-- bootstrap-daterangepicker -->
    <script src="../../assets/vendors/moment/min/moment.min.js"></script>
    <script src="../../assets/vendors/bootstrap-daterangepicker/daterangepicker.js"></script>
    <!-- Switchery -->
    <script src="../../assets/vendors/switchery/dist/switchery.min.js"></script>
    <!-- Select2 -->
    <script src="../../assets/vendors/select2/dist/js/select2.full.min.js"></script>
    <!-- Autosize -->
    <script src="../../assets/vendors/autosize/dist/autosize.min.js"></script>
    <!-- jQuery autocomplete -->
    <script src="../../assets/vendors/devbridge-autocomplete/dist/jquery.autocomplete.min.js"></script>
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
    <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
    <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
    <script>
        //存入cookies的變數名稱
        var colorname = ["Sidebar_Color=", "SidebarText_Color=",
            "Label_Color=", "Background_Color=",
            "Column_Color=", "ColumnText_Color=",
            "DataTable_Color=", "DataTableText_Color="
        ];

                //連接伺服器
                  <%=give_js%>

        //---------------------顏色調整相關JS---------------------
        // 選擇網頁上的元素
        const inputs = document.querySelectorAll('.controls input')

        // 當使用者拉動更新元素時設定 CSS 變數值
        function updateProperty() {
            const suffix = this.dataset.sizing || '';
            document.documentElement.style.setProperty(`--${this.name}`, this.value + suffix);
        }

        // 監控每一個 input 元素
        inputs.forEach(input => {
            input.addEventListener('input', updateProperty)
        })

        //---------------------顏色調整相關JS---------------------

        //---------------------把cookies或是DataTable的資料回填至Text內部---------------------

        var colors = ["Color_Sidebar", "Color_SidebarText",
            "Color_Label", "Color_Background",
            "Color_Column", "Color_ColumnText",
            "Color_DataTable", "Color_DataTableText"];
        if (colorvalue.length == 8) {
            for (var i = 0; i < colors.length; i++) {
                document.getElementById(colors[i]).value = colorvalue[i];
            }
        }
        else {
            for (var i = 0; i < colors.length; i++) {
                if (i == 1 || i == 5 || i == 7)
                    document.getElementById(colors[i]).value = "#ffffff";
                else
                    document.getElementById(colors[i]).value = "#000000";
            }
        }
        //---------------------把cookies或是DataTable的資料回填至Text內部---------------------

        //將數值存入cookies
        function Saveclick() {
            //背景顏色
            var ColorSidebar = document.getElementById("Color_Sidebar").value;
            //側欄顏色
            var ColorSidebarText = document.getElementById("Color_SidebarText").value;
            //側欄文字顏色
            var ColorLabel = document.getElementById("Color_Label").value;
            //標籤
            var ColorBackground = document.getElementById("Color_Background").value;
            //欄位
            var ColorColumn = document.getElementById("Color_Column").value;
            //欄位文字
            var ColorColumnText = document.getElementById("Color_ColumnText").value;
            //表格
            var ColorDataTable = document.getElementById("Color_DataTable").value;
            //表格文字
            var ColorDataTableText = document.getElementById("Color_DataTableText").value;
            //用陣列去跑迴圈比較簡潔
            var colornum = [ColorSidebar, ColorSidebarText,
                ColorLabel, ColorBackground,
                ColorColumn, ColorColumnText,
                ColorDataTable, ColorDataTableText
            ];

            for (var i = 0; i < colorname.length; i++) {
                //先清除後寫入
                document.cookie = colorname[i] + "; expires=Thu, 01 Jan 1970 00:00:00 GMT";
                document.cookie = colorname[i] + colornum[i] + ";path=/";
            }
            alert('儲存成功!'); location.href = 'set_parameter.aspx';
        }
        function backpage() {
            document.getElementById('<%=button_select.ClientID %>').click();
        }
    </script>
</asp:Content>
