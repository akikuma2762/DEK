<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Asm_NoSolve.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PM.Asm_NoSolve" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>異常排程編號 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
  
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/table.css" rel="stylesheet" />
    <link href="../../Content/dp_PM/Asm_LineOverView.css" rel="stylesheet" />
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁</a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PMD">生產部</a></u></li>

        </ol>
        <!-----------------title------------------>
        <div class="row tile_time">
            <h1 class="text-center _mdTitle" style="width: 100%; margin-bottom: 15px"><b>異常排程編號</b></h1>
            <h3 class="text-center _xsTitle" style="width: 100%; margin-bottom: 15px"><b>異常排程編號</b></h3>
        </div>

        <!-- top tiles -->

        <!-----------------/title------------------>

        <!-----------------content------------------>
        <div class="row">
            <div id="_Title" class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel Div_Shadow">

                    <div class="x_content">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="x_panel">
                                <table id="TB" class="table table-bordered" border="1" cellspacing="0" style="width: 100%">
                                    <tr id="tr_row">
                                        <%=th%>
                                    </tr>
                                    <tbody>
                                        <%=tr%>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <%=Use_Javascript.Quote_Javascript() %>
</asp:Content>
