<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="materialrequirementplanning_details.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PD.materialrequirementplanning_details_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%=item_name %>歷史用料統計表 | <%= WebUtils.GetAppSettings("Company_Name") %></title>
    <%=color %>
    <link href="../../Content/table.css" rel="stylesheet" media="screen and (max-width:768px)" />
    <link href="../../Content/dp_PD/materialrequirementplanning_details.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
    <style>
        #pie_image {
            height: 400px;
            max-width: 920px;
            margin: 0px auto;
        }

        @media screen and (max-width:768px) {
            #pie_image {
                height: 400px;
                width: 100%;
                margin: 0px auto 30px;
            }
        }

        #column_image {
            height: 400px;
            max-width: 920px;
            margin: 0px auto;
        }

        @media screen and (max-width:768px) {
            #column_image {
                height: 400px;
                width: 100%;
                margin: 0px auto 30px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <!-----------------title------------------>
        <ol class="breadcrumb_">
            <li><u><a href="../index.aspx">首頁 </a></u></li>
            <li><u><a href="../SYS_CONTROL/dp_fuclist.aspx?dp=PCD">資材部</a></u></li>
            <li><u><a onclick="history.go(-1)">歷史用料統計表</a></u></li>
        </ol>
        <br>
        <div class="clearfix"></div>
        <!-----------------/title------------------>
        <!-----------------content------------------>
        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true">圖片模式</a>
            </li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" id="profile-tab" role="tab" data-toggle="tab" aria-expanded="false">表格模式</a>
            </li>
        </ul>
        <asp:RadioButtonList ID="RadioButtonList_select_type" runat="server" Style="display: none">
            <asp:ListItem Value="4" Selected="True"></asp:ListItem>
        </asp:RadioButtonList>
        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">
                <div class="x_panel Div_Shadow">
                    <div class="row">
                        <div id="img_pie"></div>
                        <div id="img_column"></div>
                    </div>
                </div>
            </div>
            <div role="tabpanel" class="tab-pane fade" id="tab_content2" aria-labelledby="profile-tab">
                <div id="materialrequirementplanning_details"></div>
            </div>
        </div>

        <!-----------------/content------------------>
    </div>
    <%=Use_Javascript.Quote_Javascript() %>
    <script>
        //產生圖片用div
        create_imghtmlcode('img_pie', 'export_image', 'pie_image', '5', '');
        //產生圖片
        set_pie('pie_image', '<%=item_name %>領用記錄', '<%=date_str%>~<%=date_end%>', [<%=pie_data_points%>]);

        create_imghtmlcode('img_column', 'exports_image', 'column_image', '7', '');
        set_manystackColumn('column_image', '<%=item_name %>領用記錄', '<%=date_str%>~<%=date_end%>', [<%=col_data_points %>]);


        //產生表格的HTML碼
        create_tablecode('materialrequirementplanning_details', ' <%=item_name %>領用紀錄', 'datatable-buttons', '<%=th.ToString() %>', '<%=tr.ToString()%>');
        //產生相對應的JScode
        set_Table('#datatable-buttons');
        //防止頁籤跑版
        loadpage('', '');
    </script>
</asp:Content>
