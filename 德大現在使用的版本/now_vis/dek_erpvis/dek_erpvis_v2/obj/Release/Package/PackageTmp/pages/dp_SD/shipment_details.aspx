<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="shipment_details.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_SD.shipment_details_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=cust_name %>出貨詳細表 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_SD/shipment_details.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="_Title" class="right_col" role="main" style="height: 930px;">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁 </a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=SLS">業務部</a></u></li>
            <li><u><a href="javascript:void()" onclick="history.go(-1)">出貨統計表</a></u></li>
        </ol>
        <br>
        <div class="clearfix"></div>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <div id="shipment_details"></div>
        <!-----------------/content------------------>
        <!-- Modal -->
        <div id="exampleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h1>
                            <i id="dlg_title"></i>
                        </h1>
                    </div>
                    <div class="modal-body">
                        <table id="dataTables-modal" class="table table-ts table-bordered dt-responsive nowrap " cellspacing="0" width="100%">
                            <thead>
                                <tr style="background-color: white">
                                    <th>#</th>
                                    <th>出貨日期</th>
                                    <th>出貨單號</th>
                                    <th>製造批號</th>
                                    <th>CCS</th>
                                    <th>數量</th>
                                    <th style="width: 20%;">備註</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <!-- /Modal -->
    </div>
    <%=Use_Javascript.Quote_Javascript() %>
    <script>

        //產生表格的HTML碼
        create_tablecode('shipment_details', '<%=cust_name %> 訂單詳細表', 'table-form', '<%=th.ToString() %>', '<%=tr.ToString() %>');
        //產生相對應的JScode
        set_Table('#table-form');
        //防止頁籤跑版
        loadpage('', '');

        function GetShipment_details(time_str, time_end, cust, code,Factory) {
            $.ajax({
                type: 'POST',
                dataType: 'xml',
                url: "../../webservice/dp_SD.asmx/GetShipment_details_New",
                data: { date_str: time_str, date_end: time_end, cust_name: cust, item_code: code, Factory: Factory }, //MR4AK0004A10,20180101,20181231
                success: function (xml) {

                    $(xml).find("ROOT_PIE").each(function (i) {
                        if ($(xml).find("ROOT_PIE").length > 0) {
                            var code = $(this).attr("item_code").valueOf();
                            var thisTable = $('#dataTables-modal').dataTable();
                            thisTable.fnClearTable();
                            $(this).children().each(function (j) {
                                addData = [];
                                addData.push($(this).attr("序列").valueOf());
                                addData.push($(this).attr("出貨日期").valueOf());
                                addData.push($(this).attr("出貨單號").valueOf());
                                addData.push($(this).attr("製造批號").valueOf());
                                addData.push($(this).attr("CCS").valueOf());
                                addData.push($(this).attr("數量").valueOf());
                                addData.push($(this).attr("訂單備註").valueOf());
                                thisTable.fnAddData(addData);
                            })
                            document.getElementById("dlg_title").innerHTML = code;
                        }
                    });
                },
                error: function (data, errorThrown) {
                    alert("Fail");
                }
            });
        }

    </script>
</asp:Content>
