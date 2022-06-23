<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage.Master" AutoEventWireup="true" CodeBehind="Machine_list_info.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_CNC.Machine_list_info" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>機台總覽 | <%= WebUtils.GetAppSettings("Company_Name") %></title>

    <link href="../../Content/dp_CNC/Machine_list_info.css" rel="stylesheet" />
    <link href="../../Content/Default.css" rel="stylesheet" />
    <link href="../../Content/Default_input.css" rel="stylesheet" />

    <%=color %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        /*手機*/
        @media screen and (max-width: 765px) {
            .ButtonStyle {
                height: 20px;
                font-size: 14px;
                line-height: 9px;
            }

            .col-xs-6 {
                width: 100%
            }

            .div_height {
                height: 2px;
            }

            .masonry {
                column-count: 1;
                column-gap: 0;
            }

            .item {
                position: relative;
                    margin-bottom: 100px;
                counter-increment: item-counter;
            }

            .button_use {
                display: none;
            }
        }
        /*電腦*/
        @media screen and (min-width: 765px) {
            .ButtonStyle {
                height: 28px;
                font-size: 18px;
                line-height: 18px;
            }

            .div_height {
                height: 11px;
            }

            .masonry {
                column-count: 2;
                column-gap: 0;
            }

            .item {
                position: relative;
                counter-increment: item-counter;
            }

            .modal-lg2 {
                width: 1700px
            }

            #Next_Task {
                padding-top: 7%
            }
        }

        #Table_next_length {
            display: none;
        }

        #Table_next_filter {
            display: none;
        }

        #Table_next_info {
            display: none;
        }

        #Table_next_paginate {
            display: none;
        }

        #Table_now_length {
            display: none;
        }

        #Table_now_filter {
            display: none;
        }

        #Table_now_info {
            display: none;
        }

        #Table_now_paginate {
            display: none;
        }


        input[type="radio"] {
            width: 18px;
            height: 18px;
            cursor: auto;
            -webkit-appearance: default-button;
        }

        .dataTables_scroll {
            overflow: auto;
        }

        #fn-gantt-hint {
            display: none;
            position: absolute;
            left: 0;
            top: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.6);
            z-index: 99;
        }
        .GP_Tr,.GP_Td {width:100%
        }
        .GP_Content {
            display:flex;
            align-items:center;
            justify-content:center;
            height:40px;
        }
        #ContentPlaceHolder1_CheckBoxList_cloumn {
            width:100%;
        }
        .GP_Btn {
            width:100%;
            border:0px;
            background-color:rgb(51, 122, 183);
        }
            .GP_Btn:hover {
                background-color:rgb(127, 177, 219);
                transition:1s;
            }
            .GP_Btn:active {
                transition:0s;
                background-color:rgb(40, 96, 144);
               border-left: 2px solid black;border-top: 2px solid black;
               border-right: 1px solid rgb(202, 198, 198);border-bottom: 1px solid rgb(202, 198, 198);
            }
        #GP1,#GP2 {
            margin:0px;
            display: flex;
            justify-content: center;
            align-items: center;
            color:white;
        }
        #GP_Title {
            width:90%;
        }
        .gp1>td,.gp2>td {
            width:50%;
        }
    </style>
    <link rel="stylesheet" href="../../gantt/css/style.css" />
    <link rel="stylesheet" href="../../gantt/css/prettify.min.css" />
    <div class="right_col" role="main">
        <ol class='breadcrumb_'>
            <li><u><a href='../index.aspx'>首頁 </a></u></li>
            <li><u><a href='../SYS_CONTROL/dp_fuclist.aspx?dp=CNC'>加工部</a></u></li>
        </ol>
        <br>
        <div class="page-title">
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <h1 class="text-center _mdTitle" style="width: 100%; margin: 0px 0px 17px 0px"><b>設備監控看板</b>  </h1>
                    <h3 class="text-center _xsTitle" style="width: 100%; margin: 0px 0px 17px 0px"><b>設備監控看板</b>  </h3>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <div id="_Chart">
            <%--兩div間隙--%>
            <div class="x_panel Div_Shadow">
                <div class="col-md-2 col-sm-12 col-xs-12">
                    <div class="x_panel">
                        <div class="col-md-7 col-sm-12 col-xs-12">
                            <h1><strong>稼動比例</strong></h1>
                            <%--<H1 align="center" style="font-size:56px; color:black; margin-top:-10px;margin-bottom:-2px;"><b>77.6</b></H1>--%>
                            <h3><b id="Oper_Rate"></b><b><span>&nbsp%</span></b></h3>
                        </div>
                        <div class="col-md-1 col-sm-12 col-xs-12"></div>
                        <div class="col-md-4 col-sm-12 col-xs-12">
                            <img class='img-rounded' src='/pages/dp_CNC/Mach_Picture/VIS icons-25.png' alt='...' style="">
                        </div>
                    </div>
                </div>

                <div style="display: none" class="col-md-2 col-sm-12 col-xs-12">
                    <div class="x_panel" style="display: none">
                        <div class="col-md-7 col-sm-12 col-xs-12">
                            <h1><strong>生產進度</strong></h1>
                            <h3><b id="Parts_Rate"></b><b><span>&nbsp%</span></b></h3>
                        </div>
                        <div class="col-md-1 col-sm-12 col-xs-12"></div>
                        <div class="col-md-4 col-sm-12 col-xs-12">
                            <img class='img-rounded' src='/pages/dp_CNC/Mach_Picture/VIS icons-25.png' alt='...'>
                        </div>
                    </div>
                </div>

                <div class="col-md-2 col-sm-12 col-xs-12">
                    <div class="x_panel" style="background-color: #00b400">
                        <div class="col-md-7 col-sm-12 col-xs-12">
                            <h1><strong>運 轉</strong></h1>
                            <h3><b id="OperCount"></b><b><span id="OperTotal">&nbsp 台</span></b></h3>
                        </div>
                        <div class="col-md-1 col-sm-12 col-xs-12"></div>
                        <div class="col-md-4 col-sm-12 col-xs-12">
                            <img class='img-rounded' src='/pages/dp_CNC/Mach_Picture/VIS icons-26.png' alt='...'>
                        </div>
                    </div>
                </div>

                <div class="col-md-2 col-sm-12 col-xs-12">
                    <div class="x_panel" style="background-color: #ff9b32">
                        <div class="col-md-7 col-sm-12 col-xs-12">
                            <h1><strong>待 機</strong></h1>
                            <h3><b id="StopCount"></b><b><span id="StopTotal">&nbsp 台</span></b></h3>
                        </div>
                        <div class="col-md-1 col-sm-12 col-xs-12"></div>
                        <div class="col-md-4 col-sm-12 col-xs-12">
                            <img class='img-rounded' src='/pages/dp_CNC/Mach_Picture/VIS icons-28.png' alt='...'>
                        </div>
                    </div>
                </div>

                <div class="col-md-2 col-sm-12 col-xs-12">
                    <div class="x_panel" style="background-color: #FF0000">
                        <div class="col-md-7 col-sm-12 col-xs-12">
                            <h1><strong>警 報</strong></h1>
                            <h3><b id="AlarmCount"></b><b><span id="AlarmTotal">&nbsp 台</span></b></h3>
                        </div>
                        <div class="col-md-1 col-sm-12 col-xs-12"></div>
                        <div class="col-md-4 col-sm-12 col-xs-12">
                            <img class='img-rounded' src='/pages/dp_CNC/Mach_Picture/VIS icons-29.png' alt='...'>
                        </div>
                    </div>
                </div>

                <div class="col-md-2 col-sm-12 col-xs-12">
                    <div class="x_panel" style="background-color: #a9a9a9">
                        <div class="col-md-7 col-sm-12 col-xs-12">
                            <h1><strong>離 線</strong></h1>
                            <h3><b id="DiscCount"></b><b><span id="DiscTotal">&nbsp 台</span></b></h3>
                        </div>
                        <div class="col-md-1 col-sm-12 col-xs-12"></div>
                        <div class="col-md-4 col-sm-12 col-xs-12">
                            <img class='img-rounded' src='/pages/dp_CNC/Mach_Picture/VIS icons-30.png' alt='...'>
                        </div>
                    </div>
                </div>

                <div class="col-md-2 col-sm-12 col-xs-12">
                    <div class="x_panel" style="background-color: #CA8EFF">
                        <div class="col-md-7 col-sm-12 col-xs-12">
                            <h1><strong>其 它</strong></h1>
                            <h3><b id="Others"></b><b><span id="OthersTotal">&nbsp 台</span></b></h3>
                        </div>
                        <div class="col-md-1 col-sm-12 col-xs-12"></div>
                        <div class="col-md-4 col-sm-12 col-xs-12">
                            <img class='img-rounded' src='/pages/dp_CNC/Mach_Picture/VIS icons-31.png' alt='...'>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <!--以上狀態統計色塊-->
        <ul id="myTab" class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true" onclick="show_control()">表格模式</a>
            </li>
            <li role="presentation" class="" style="box-shadow: 3px 3px 9px gray;"><a href="#tab_content2" role="tab" id="profile-tab" data-toggle="tab" onclick="showbar()" aria-expanded="false">圖片模式</a>
            </li>
            <%=show_gantt_image %>
        </ul>
        <div id="myTabContent" class="tab-content">
            <div role="tabpanel" class="tab-pane fade active in " id="tab_content1" aria-labelledby="home-tab">

                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel Div_Shadow">
                            <asp:TextBox ID="TextBox_MachTypeText" runat="server" Style="display: none"></asp:TextBox>
                            <asp:TextBox ID="TextBox_MachTypeValue" runat="server" Style="display: none"></asp:TextBox>
                            <asp:TextBox ID="TextBox_MachGroupText" runat="server" Style="display: none"></asp:TextBox>
                            <asp:TextBox ID="TextBox_MachGroupValue" runat="server" Style="display: none"></asp:TextBox>

                            <div class="x_content">
                                <asp:Button ID="Search_save" runat="server" Text="Button" OnClick="Search_save_Click" Style="display: none" />
                                <asp:Button ID="Search_Next" runat="server" Text="Button" OnClick="Search_Next_Click" Style="display: none" />
                                <asp:TextBox ID="TextBox_Machine" runat="server" Style="display: none"></asp:TextBox>
                                <asp:TextBox ID="TextBox_Number" runat="server" Style="display: none"></asp:TextBox>
                                <asp:Label ID="Label_backgroundact" runat="server" Text="0" Style="display: none"></asp:Label>
                                <asp:Label ID="Label_onclickact" runat="server" Text="0" Style="display: none"></asp:Label>

                                <div class="col-md-12 col-sm-12 col-xs-12" id="table_information">
                                    <div class="x_panel">
                                        <table id="datatable_mach" class="table  table-ts table-bordered nowrap" border="1" cellspacing="0" width="100%">
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
                                </div>

                                <div id="_select" class="col-md-3 col-sm-12 col-xs-12 _select _setborder">
                                    <div class="col-md-12 col-sm-12 col-xs-12 ">
                                        <div class="dashboard_graph x_panel">
                                            <div class="x_content">
                                                <asp:ScriptManager ID="ScriptManager1" runat="server">
                                                </asp:ScriptManager>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="TextBox_Status" runat="server" Style="display: none" CssClass="form-control   text-left"></asp:TextBox>
                                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                                            <div class="col-md-5 col-sm-3 col-xs-5" style="margin: 5px 0px 5px -5px">
                                                                <span>廠區名稱</span>
                                                            </div>
                                                            <div class="col-md-7 col-sm-9 col-xs-7" style="margin: 0px 0px 0px 5px">
                                                                <div class="row">
                                                                    <div class="col-md-12 col-sm-12 col-xs-12 ">
                                                                        <asp:DropDownList ID="DropDownList_MachType" CssClass="btn btn-default dropdown-toggle" runat="server" Width="100%" onchange="dropdownlist_change('ContentPlaceHolder1_DropDownList_MachType','ContentPlaceHolder1_DropDownList_MachGroup')"></asp:DropDownList>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                                            <div class="col-md-5 col-sm-3 col-xs-5" style="margin: 5px 0px 5px -5px">
                                                                <span>群組名稱</span>
                                                            </div>
                                                            <div class="col-md-7 col-sm-9 col-xs-7" style="margin: 0px 0px 0px 5px">
                                                                <div class="row">
                                                                    <div class="col-md-12 col-sm-12 col-xs-12 ">
                                                                        <asp:DropDownList ID="DropDownList_MachGroup" runat="server" CssClass="btn btn-default dropdown-toggle" Width="100%" onchange="show_machines('ContentPlaceHolder1_DropDownList_MachGroup')"></asp:DropDownList>
                                                                    </div>
                                                                    <asp:CheckBoxList ID="CheckBoxList_dev" runat="server"></asp:CheckBoxList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div id="div_machines" class="col-md-12 col-sm-6 col-xs-12" style="display: none">
                                                            <asp:TextBox ID="TextBox_Machines" runat="server" Style="display: none"></asp:TextBox>
                                                            <div class="col-md-5 col-sm-3 col-xs-5" style="margin: 5px 0px 5px 0px">
                                                                <span>群組機台</span>
                                                            </div>
                                                            <div class="col-md-7 col-sm-9 col-xs-7">
                                                                <label id="machines"></label>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                                            <div class="col-md-5 col-sm-3 col-xs-5" style="margin: 5px 0px 5px -5px">
                                                                <span>選擇欄位</span>
                                                            </div>
                                                            <div class="col-md-7 col-sm-9 col-xs-7" style="margin: 0px 0px 0px 5px">
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                                            
                                                            <div class="row">
                                                                <div class="col-md-12 col-sm-12 col-xs-12 ">
                                                                    <asp:CheckBoxList ID="CheckBoxList_cloumn" RepeatColumns="2" runat="server"></asp:CheckBoxList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="col-md-9 col-xs-8">
                                                </div>
                                                <div class="col-md-3 col-xs-12">
                                                    <asp:LinkButton ID="LinkButton_ok" runat="server" Style="position: absolute; right: 0; display: none" OnClick="Select_MachGroupClick" class="btn btn-primary antosubmit2">選擇</asp:LinkButton>
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
            <div id="div_button" runat="server" class="button_use" onclick="hide_div()">
                <i id="iconimage" class="fa fa-chevron-left"></i>
            </div>
            <%--以上列表模式--%>
            <div role="tabpanel" class="tab-pane fade " id="tab_content2" aria-labelledby="profile-tab">
                <div class="x_panel Div_Shadow">
                    <div class="x_content">
                        <div class="row">

                            <div class="masonry">
                                <%=area %>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <%--以上圖塊模式--%>
            <div role="tabpanel" class="tab-pane fade" id="tab_content3" aria-labelledby="profile-tab2">

                <div class="x_panel" id="Div_Shadow">
                    <div class="gantt_ot" style="width: 100%; height: 100%; margin: 2px auto;">
                        <div class="gantt"></div>
                    </div>
                </div>
            </div>
            <%--以上甘特圖模式--%>
        </div>

        <!--以上機台清單列表-->
    </div>
    <!-----以下檢索精靈相關--->
    <!-- set Modal -->


    <div id="Next_Task" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-lg2">
            <div class="modal-content">

                <div class="modal-body">
                    <div id="testmoda22" style="padding: 5px 20px;">

                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">

                            <div id="div_next" class="col-xs-12 col-sm-12 col-md-12" style="border-width: 3px; border-style: solid; border-color: #000; margin-bottom: 10px">
                                <div class="col-xs-12 col-sm-12 col-md-12" style="text-align: center">
                                    <b style="font-size: 30px">下筆工單內容</b>
                                    <hr style="border-bottom: 2px solid #E6E9ED; margin-top: 0px; margin-bottom: 0px" />
                                </div>
                                <br />
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <table id="Table_next" class="table table-ts table-bordered dt-responsive nowrap " cellspacing="0" width="100%">
                                        <thead>
                                            <tr style="background-color: white">
                                                <th style="text-align: center">設備名稱</th>
                                                <th style="text-align: center">操作人員</th>
                                                <th style="text-align: center">製令單號</th>
                                                <th style="text-align: center">客戶名稱</th>
                                                <th style="text-align: center">產品名稱</th>
                                                <th style="text-align: center">料件編號</th>
                                                <th style="text-align: center">工藝名稱</th>

                                                <th style="text-align: center">實際生產數量</th>
                                                <th style="text-align: center">預計生產數量</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>




                            <div id="div_now" class="col-xs-12 col-sm-12 col-md-12" style="border-width: 3px; border-style: solid; border-color: #000; margin-bottom: 10px">

                                <div class="col-xs-12 col-sm-12 col-md-12" style="text-align: center">
                                    <b style="font-size: 30px">本筆工單報工</b>
                                    <hr style="border-bottom: 2px solid #E6E9ED; margin-top: 0px; margin-bottom: 0px" />
                                </div>
                                <br />
                                <div class="col-xs-12 col-sm-12 col-md-12" style="margin-bottom: 20px">
                                    <asp:TextBox ID="TextBox_MachNext" runat="server" Visible="true" Width="80px" Style="display: none"></asp:TextBox>
                                    <asp:TextBox ID="TextBox_Order" runat="server" Visible="true" Width="80px" Style="display: none"></asp:TextBox>
                                    <asp:TextBox ID="TextBox_hope" runat="server" Visible="true" Width="80px" Style="display: none"></asp:TextBox>
                                    <asp:TextBox ID="TextBox_true" runat="server" Visible="true" Width="80px" Style="display: none"></asp:TextBox>



                                    <table id="Table_now" class="table table-ts table-bordered dt-responsive nowrap " cellspacing="0" width="100%">
                                        <thead>
                                            <tr style="background-color: white">
                                                <th style="text-align: center">設備名稱</th>
                                                <th style="text-align: center">操作人員</th>
                                                <th style="text-align: center">製令單號</th>
                                                <th style="text-align: center">客戶名稱</th>
                                                <th style="text-align: center">產品名稱</th>
                                                <th style="text-align: center">料件編號</th>
                                                <th style="text-align: center">工藝名稱</th>
                                                <th style="text-align: center">實際生產數量</th>
                                                <th style="text-align: center">預計生產數量</th>


                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>

                                <div class="col-xs-12 col-sm-12 col-md-12" style="display: none">
                                    <div class="col-md-5 ">
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-1">
                                        工單狀態
                                    </div>


                                    <div class="col-xs-12 col-sm-12 col-md-1 ">
                                        <asp:DropDownList ID="DropDownList_status" runat="server">
                                            <asp:ListItem Selected="True" Value="入站">入站</asp:ListItem>
                                            <asp:ListItem Value="暫停">暫停</asp:ListItem>
                                            <asp:ListItem Value="取消暫停">取消暫停</asp:ListItem>
                                            <asp:ListItem Value="出站">出站</asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                </div>
                                <div class="col-md-12" style="display: none">
                                    <br />
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-12" style="display: none">
                                    <div class="col-md-5 ">
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-1 ">
                                        校機人員
                                    </div>

                                    <div class="col-xs-12 col-sm-12 col-md-1 ">
                                        <asp:DropDownList ID="DropDownList_check" runat="server" onchange="change_staff('ContentPlaceHolder1_DropDownList_check','#ContentPlaceHolder1_TextBox_check')"></asp:DropDownList>
                                        <asp:TextBox ID="TextBox_check" runat="server" Style="display: none"></asp:TextBox>
                                    </div>

                                </div>


                                <div class="col-md-12" style="display: none">
                                    <br />
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="col-md-5 ">
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-1 ">
                                        操作人員
                                    </div>

                                    <div class="col-xs-12 col-sm-12 col-md-1 ">
                                        <asp:DropDownList ID="DropDownList_work" runat="server" onchange="change_staff('ContentPlaceHolder1_DropDownList_work','#ContentPlaceHolder1_TextBox_work')"></asp:DropDownList>
                                        <asp:TextBox ID="TextBox_work" runat="server" Style="display: none"></asp:TextBox>
                                    </div>

                                </div>


                                <div class="col-md-12" style="display: none">
                                    <br />
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-12" style="display: none">
                                    <div class="col-md-5 ">
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-1 ">
                                        良品數量
                                    </div>

                                    <div class="col-xs-12 col-sm-12 col-md-1 ">
                                        <asp:TextBox ID="TextBox_QTY" runat="server"></asp:TextBox>

                                    </div>

                                </div>


                                <div id="div_br" class="col-md-12" style="display: none">
                                    <br />
                                </div>
                                <div id="div_mistake" class="col-xs-12 col-sm-12 col-md-12" style="display: none">
                                    <div class="col-md-5 ">
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-1 ">
                                        不良數量
                                    </div>

                                    <div class="col-xs-12 col-sm-12 col-md-1 ">
                                        <asp:TextBox ID="TextBox_mistake" runat="server" TextMode="Number"></asp:TextBox>

                                    </div>

                                </div>
                                <div class="col-md-12">
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>


                    <div style="text-align: center; padding: 15px;">

                        <button type="button" class="btn btn-default antoclose2" data-dismiss="modal" style="margin-top: 25px">退出作業</button>
                        <button id="button_save" type="button" class="btn btn-primary antosubmit2" style="margin-top: 25px">儲存</button>
                        <button id="button_next" type="button" class="btn btn-primary antosubmit2" style="margin-top: 25px">完工</button>
                        <%--<asp:Button ID="Button_Next" runat="server" Text="執行檢索" class="btn btn-secondary" OnClick="bt_Next_Click" Style="display: none" />--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--/set Modal-->
    <!-- Modal -->
    <!-- /Modal -->
    <!-- jQuery -->
    <script src="../../assets/vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="../../assets/vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- bootstrap-progressbar -->
    <script src="../../assets/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js"></script>
    <!-- iCheck -->
    <script src="../../assets/vendors/iCheck/icheck.min.js"></script>
    <!-- bootstrap-wysiwyg -->
    <script src="../../assets/vendors/bootstrap-wysiwyg/js/bootstrap-wysiwyg.min.js"></script>
    <script src="../../assets/vendors/jquery.hotkeys/jquery.hotkeys.js"></script>
    <script src="../../assets/vendors/google-code-prettify/src/prettify.js"></script>
    <!-- jQuery Tags Input -->
    <script src="../../assets/vendors/jquery.tagsinput/src/jquery.tagsinput.js"></script>
    <!-- Switchery -->
    <script src="../../assets/vendors/switchery/dist/switchery.min.js"></script>
    <!-- Select2 -->
    <script src="../../assets/vendors/select2/dist/js/select2.full.min.js"></script>
    <!-- Parsley -->
    <script src="../../assets/vendors/parsleyjs/dist/parsley.min.js"></script>
    <!-- Autosize -->
    <script src="../../assets/vendors/autosize/dist/autosize.min.js"></script>
    <!-- jQuery autocomplete -->
    <script src="../../assets/vendors/devbridge-autocomplete/dist/jquery.autocomplete.min.js"></script>
    <!-- starrr -->
    <script src="../../assets/vendors/starrr/dist/starrr.js"></script>
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
    <script src="../../assets/vendors/datatables.net-colReorder/dataTables.colReorder.min.js"></script>
    <script src="../../assets/vendors/time/loading.js"></script>
    <script src="../../gantt/js/jquery.fn.gantt.js"></script>
    <script src="../../gantt/js/prettify.min.js"></script>
    <script src="../../assets/vendors/Create_HtmlCode/HtmlCode20211210.js"></script>
    <script>  
        function testtop() {
            var RD = document.getElementsByClassName("bar ganttRedDark");
            for (i = 0; i < RD.length; i++)
                RD[i].style.top = (parseInt(RD[i].style.top.replaceAll('px', '')) + 23) + 'px';

            var GD = document.getElementsByClassName("bar ganttGreenDark");
            for (i = 0; i < GD.length; i++)
                GD[i].style.top = (parseInt(GD[i].style.top.replaceAll('px', '')) + 23) + 'px';

            var OD = document.getElementsByClassName("bar ganttOrangeDark");
            for (i = 0; i < OD.length; i++)
                OD[i].style.top = (parseInt(OD[i].style.top.replaceAll('px', '')) + 23) + 'px';

            var BD = document.getElementsByClassName("bar ganttBlueDark");
            for (i = 0; i < BD.length; i++)
                BD[i].style.top = (parseInt(BD[i].style.top.replaceAll('px', '')) + 23) + 'px';

            var PD = document.getElementsByClassName("bar ganttPurpleDark");
            for (i = 0; i < PD.length; i++)
                PD[i].style.top = (parseInt(PD[i].style.top.replaceAll('px', '')) + 23) + 'px';

        }


        function hide_div() {
            var tablename = document.getElementById("table_information");
            var divname = document.getElementById("_select");
            var icon = document.getElementById("iconimage");


            if (tablename.className == 'col-md-9 col-sm-12 col-xs-12') {
                icon.className = 'fa fa-chevron-left';
                tablename.className = 'col-md-12 col-sm-12 col-xs-12';
                divname.style.display = 'none';

            }
            else {
                icon.className = "fa fa-chevron-right";
                tablename.className = 'col-md-9 col-sm-12 col-xs-12';
                divname.style.display = 'block';
            }
        }
        $(".iconimage").on("click", function () {
            hide_div();
        });
       $("#button_save").click(function () {
            var ddlcount = document.getElementById('<%=DropDownList_work.ClientID%>').options.length;
            if (ddlcount == 0) {
                alert('未選取人員');
            }
            else {
                var now_staff = document.getElementById('Table_now').getElementsByTagName("td")[1].innerText;
                var change_staff = document.getElementById("ContentPlaceHolder1_TextBox_work").value;
                if (now_staff == change_staff) {
                    alert('操作人員相同，請重新選擇');
                }
                else {
                    $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                    document.querySelector(".blockUI.blockMsg.blockPage").style.zIndex = 1000000;
                    document.getElementById('<%=Search_save.ClientID %>').click();
                }
            }
        });
        $("#button_next").click(function () {
            var now_staff = document.getElementById('Table_now').getElementsByTagName("td")[1].innerText;
            var now_qty = document.getElementById("ContentPlaceHolder1_TextBox_true").value;
            var ext_qty = document.getElementById("ContentPlaceHolder1_TextBox_hope").value;
            var ddlcount = document.getElementById('<%=DropDownList_work.ClientID%>').options.length;
            if (now_staff != '') {
                if (ddlcount != 0) {
                    //避免有人修改數據導致資料異常
                    try {
                        document.getElementById('ContentPlaceHolder1_DropDownList_work').value = now_staff;
                        $('#ContentPlaceHolder1_TextBox_work').val('' + now_staff + '');
                       
                    }
                    catch {
                    }
                    if (parseInt(now_qty, 10) < parseInt(ext_qty, 10)) {
                        var r = confirm(" 是否要提前結案?");
                        if (r == true) {
                            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                            document.querySelector(".blockUI.blockMsg.blockPage").style.zIndex = 1000000;
                            document.getElementById('<%=Search_Next.ClientID %>').click();
                        }
                    }
                    else {
                        $.blockUI({ message: '<img src="../../images/loading.gif" />' });
                        document.querySelector(".blockUI.blockMsg.blockPage").style.zIndex = 1000000;
                        document.getElementById('<%=Search_Next.ClientID %>').click();
                    }
                }
            }
            else
                alert('人員不存在');
        });

        function Next_Craft(mach, num, now_qty, ext_qty) {
            if (now_qty == '')
                now_qty = '0';
            if (ext_qty == '')
                ext_qty = '0';
            if (parseInt(now_qty, 10) < parseInt(ext_qty, 10)) {
                var r = confirm(" 是否要提前結案?");
                if (r == true) {
                    $('#ContentPlaceHolder1_TextBox_Machine').val('' + mach + '');
                    $('#ContentPlaceHolder1_TextBox_Number').val('' + num + '');
                    $('#ContentPlaceHolder1_TextBox_MachNext').val('' + mach + '');
                    $('#ContentPlaceHolder1_TextBox_Order').val('' + num + '');
                    document.getElementById('<%=Search_Next.ClientID %>').click();
                }
            }
            else {


                $('#ContentPlaceHolder1_TextBox_Machine').val('' + mach + '');
                $('#ContentPlaceHolder1_TextBox_Number').val('' + num + '');
                $('#ContentPlaceHolder1_TextBox_MachNext').val('' + mach + '');
                $('#ContentPlaceHolder1_TextBox_Order').val('' + num + '');

                document.getElementById('<%=Search_Next.ClientID %>').click();
            }

        }
        $("#btncheck").click(function () {
            var type = document.getElementById('ContentPlaceHolder1_DropDownList_MachType');
            var group = document.getElementById('ContentPlaceHolder1_DropDownList_MachGroup');
            var grouptext = '';

            $('#ContentPlaceHolder1_TextBox_MachTypeText').val('' + type.options[type.selectedIndex].text + '');
            $('#ContentPlaceHolder1_TextBox_MachTypeValue').val('' + type.value + '');
            try {
                $('#ContentPlaceHolder1_TextBox_MachGroupText').val('' + group.options[group.selectedIndex].text + '');
                $('#ContentPlaceHolder1_TextBox_MachGroupValue').val('' + group.value + '');
                grouptext = group.options[group.selectedIndex].text;
            }
            catch {

            }

            $.blockUI({ message: '<img src="../../images/loading.gif" />' });
            document.getElementById('<%=LinkButton_ok.ClientID %>').click();

        });

        //顯示該機台下一筆工作
        function Next_Task(mach, noworder, now_status, now_qty, ext_qty, now_info, next_info, staff) {

            if (now_qty == '')
                now_qty = '0';
            if (ext_qty == '')
                ext_qty = '0';

            $('#ContentPlaceHolder1_TextBox_MachNext').val('' + mach + '');
            $('#ContentPlaceHolder1_TextBox_Order').val('' + noworder + '');
            $('#ContentPlaceHolder1_TextBox_hope').val('' + ext_qty + '');
            $('#ContentPlaceHolder1_TextBox_true').val('' + now_qty + '');

            //填入資料到DROPDOWNLIST
            var staff_list = staff.replaceAll('*', ' ').split('^');
            $('#ContentPlaceHolder1_DropDownList_check').empty();
            $('#ContentPlaceHolder1_DropDownList_work').empty();
            var drop_check = document.getElementById('ContentPlaceHolder1_DropDownList_check');
            var drop_work = document.getElementById('ContentPlaceHolder1_DropDownList_work');

            for (i = 0; i < staff_list.length - 1; i++) {
                var option = document.createElement("OPTION");
                option.innerHTML = staff_list[i];
                option.value = staff_list[i];
                drop_check.options.add(option);
                $('#ContentPlaceHolder1_TextBox_check').val('' + staff_list[0] + '');

                var option2 = document.createElement("OPTION");
                option2.innerHTML = staff_list[i];
                option2.value = staff_list[i];
                drop_work.options.add(option2);
                $('#ContentPlaceHolder1_TextBox_work').val('' + staff_list[0] + '');

            }

            //顯示下筆排成表格
            var next_list = next_info.replaceAll('*', ' ').split('^');

            var nexttable = $('#Table_next').dataTable();
            nexttable.fnClearTable();
            var next_data = [];
            if (next_list.length > 1) {
                for (i = 0; i < next_list.length - 1; i++)
                    next_data.push('<center>' + next_list[i] + '</center>');
                nexttable.fnAddData(next_data);
            }



            //顯示當筆排成表格
            var now_list = now_info.replaceAll('*', ' ').split('^');
            var now_data = [];
            var nowtable = $('#Table_now').dataTable();
            nowtable.fnClearTable();
            if (now_list.length > 1) {
                for (i = 0; i < now_list.length - 1; i++)
                    now_data.push('<center>' + now_list[i] + '</center>');
                nowtable.fnAddData(now_data);
            }

        }

        //變更表格人員
        function change_staff(drop, text) {
            var type = document.getElementById(drop);
            $(text).val('' + type.options[type.selectedIndex].text + '');
        }



        //初始設定為false
        var action_ok = '0';
        //去偵測cookie是否存在
        var test = "action_status" + '=';
        var x = document.cookie.split(';');
        var y = '';
        for (var i = 0; i < x.length; i++) {
            var item = x[i].trim();
            if (item.indexOf(test) == 0) {
                y = item.substring(test.length, item.length);
                break;
            }
        }
        //如果不存在 加入cookie
        if (y == '')
            document.cookie = "action_status=" + action_ok + ";";
        //如果存在 就讓他=cookie的值
        else
            action_ok = y;

        //表格閃爍機制
        var mTimer;
        var Timer_Count = 0;//閃爍機制
        var loadTime = <%=Refresh_Time%>;
        mTimer = setTimeout(function () { GetMachineData(); }, loadTime);




        var Color_OPERATE_new = "rgb(0, 180, 0)";
        var Color_DISCONNECT_new = "rgb(169, 169, 169)";
        var Color_ALARM_new = "rgb(255, 0, 0)";
        var Color_EMERGENCY_new = "rgb(255, 0, 255)";
        var Color_SUSPEND_new = "rgb(255, 255, 0)";
        var Color_STOP_new = "rgb(255, 155, 50)";
        var Color_MANUAL_new = "rgb(2, 205, 252)";
        var Color_WARMUP_new = "rgb(178, 34, 34)";
        var Oper_Count = 0;
        var Stop_Count = 0;
        var Alarm_Count = 0;
        var Disc_Count = 0;
        var Other = 0;
        var Dev_Count_Oper = 0;
        var Dev_Count_Parts = 0;
        var Oper_Rate = 0;
        var Parts_Rate = 0;

        function GetMachineData() {
            clearTimeout(mTimer);
            $.ajax({
                type: 'POST',
                dataType: 'xml',
                url: "../../webservice/dpCNC_MachData.asmx/GetMachineData",
                data: { acc: '<%=acc%>', machine:'<%=machine_list%>' },
                success: function (xml) {
                    $(xml).find("ROOT_MACH").each(function (i) { //待增加新選項資料,否則下方程式會錯誤 20220615
                        $(this).children("Group").each(function (j) {
                            var Dev_Name = $(this).attr("Dev_Name").valueOf();
                            var check_staff = $(this).attr("checkMachStaff").valueOf();
                            var custom_name = $(this).attr("prodCustomName").valueOf();
                            var product_number = $(this).attr("prodNo").valueOf();
                            var curParts = $(this).attr("curParts").valueOf();
                            var count_today_rate = $(this).attr("partsRate").valueOf();
                            var count_today = $(this).attr("prod_count").valueOf();
                            var operate_rate = $(this).attr("operRate").valueOf();
                            var mach_status = $(this).attr("status").valueOf();
                            var work_staff = $(this).attr("workStaff").valueOf();
                            var product_name = $(this).attr("prodName").valueOf();
                            var program_run = $(this).attr("progRun").valueOf();
                            var exp_product_count_day = $(this).attr("tarParts").valueOf();
                            var finish_time = $(this).attr("partsTime").valueOf();
                            var alarm_mesg = $(this).attr("alarmMesg").valueOf();
                            var craft_name = $(this).attr("craftName").valueOf();
                            var manu_id = $(this).attr("manuId").valueOf();

                            var acts = $(this).attr("acts").valueOf();
                            var spindleload = $(this).attr("spindleload").valueOf();
                            var spindlespeed = $(this).attr("spindlespeed").valueOf();
                            var spindletemp = $(this).attr("spindletemp").valueOf();
                            var prog_main = $(this).attr("prog_main").valueOf();
                            var prog_main_cmd = $(this).attr("prog_main_cmd").valueOf();
                            var prog_run_cmd = $(this).attr("prog_run_cmd").valueOf();
                            var override = $(this).attr("override").valueOf();
                            var run_time = $(this).attr("run_time").valueOf();
                            var cut_time = $(this).attr("cut_time").valueOf();
                            var poweron_time = $(this).attr("poweron_time").valueOf();
                            var complete_time = $(this).attr("complete_time").valueOf();
                            var order_number = $(this).attr("order_number").valueOf();
                            var count = $(this).attr("count").valueOf();
                            var can_next = $(this).attr("can_next").valueOf();
                            var now_detailstatus = $(this).attr("now_detailstatus").valueOf();
                            var now_list = $(this).attr("now_list").valueOf();
                            var next_list = $(this).attr("next_list").valueOf();
                            var staff = $(this).attr("staff").valueOf();
                            //20220622新增
                            var spindle_shock = $(this).attr("spindle_shock").valueOf();
                            var spindle_side_temp = $(this).attr("spindle_side_temp").valueOf();
                            var daoku_motor_electric = $(this).attr("daoku_motor_electric").valueOf();
                            var spindle_koudao_shock = $(this).attr("spindle_koudao_shock").valueOf();
                            var spindle_position = $(this).attr("spindle_position").valueOf();
                            var spindle_ladao_position = $(this).attr("spindle_ladao_position").valueOf();
                            var oil_level = $(this).attr("oil_level").valueOf();
                            var ball_screw_hightemp = $(this).attr("ball_screw_hightemp").valueOf();
                            var tool_oil_temp = $(this).attr("tool_oil_temp").valueOf();
                            var tool_oil_pressure = $(this).attr("tool_oil_pressure").valueOf();
                            var qiexieye_concentration = $(this).attr("qiexieye_concentration").valueOf();
                            var qiexieye_temp = $(this).attr("qiexieye_temp").valueOf();
                            var air_pressure = $(this).attr("air_pressure").valueOf();

                            //紀錄表格元素的陣列
                            var tablearray = ['設備名稱', 'mach_name', Dev_Name,
                                '校機人員', 'check_staff', check_staff,
                                '操作人員', 'work_staff', work_staff,
                                '設備狀態', 'mach_status', mach_status,
                                '設備稼動率', 'operate_rate', operate_rate,
                                '製令單號', 'manu_id', manu_id,
                                '客戶名稱', 'custom_name', custom_name,
                                '產品名稱', 'product_name', product_name,
                                '料件編號', 'product_number', product_number,
                                '工藝名稱', 'craft_name', craft_name,
                                '生產進度(目前/預計)', 'count_today_rate', count_today_rate,
                                '應完工時間', 'complete_time', complete_time,
                                '預計完工時間', 'finish_time', finish_time,
                                '主軸轉速', 'acts', acts,
                                '主軸負載', 'spindleload', acts,
                                '主軸速度', 'spindlespeed', spindlespeed,
                                '主軸溫度', 'spindletemp', spindletemp,
                                '主程式', 'prog_main', prog_main,
                                '主程式註解', 'prog_main_cmd', prog_main_cmd,
                                '運行程式', 'program_run', program_run,
                                '運行程式註解', 'prog_run_cmd', prog_run_cmd,
                                '進給率', 'override', override,
                                '運轉時間', 'run_time', run_time,
                                '切削時間', 'cut_time', cut_time,
                                '通電時間', 'poweron_time', poweron_time,
                                '異警資訊', 'alarm_mesg', alarm_mesg,
                                '工單報工', 'next_button', '',
                                '主軸震動', 'spindle_shock', spindle_shock,
                                '主軸側溫度', 'spindle_side_temp', spindle_side_temp,
                                '刀庫馬達電流_損壞預警', 'daoku_motor_electric', daoku_motor_electric,
                                '主軸扣刀_震動監視', 'spindle_koudao_shock', spindle_koudao_shock,
                                '主軸定位_在位確認', 'spindle_position', spindle_position,
                                '主軸拉刀_在位確認', 'spindle_ladao_position', spindle_ladao_position,
                                '潤滑油_油位檢知', 'oil_level', oil_level,
                                'ball_screw_高溫監視', 'ball_screw_hightemp', ball_screw_hightemp,
                                '治具油壓_溫度監視', 'tool_oil_temp', tool_oil_temp,
                                '治具油壓_壓力監視', 'tool_oil_pressure', tool_oil_pressure,
                                '切屑液_濃度檢知', 'qiexieye_concentration', qiexieye_concentration,
                                '切屑液_溫度檢知', 'qiexieye_temp', qiexieye_temp,
                                '氣壓源_壓力檢知', 'air_pressure', air_pressure]





                            if (operate_rate != "--")//統計稼動率 PANEL 使用
                            {
                                Dev_Count_Oper++;
                                Oper_Rate += parseFloat(operate_rate);
                            }
                            if (count_today_rate != "--")//統計生產進度 PANEL 使用
                            {
                                Dev_Count_Parts++;
                                Parts_Rate += parseFloat(count_today_rate);
                            }

                            if (mach_status == "運轉" || mach_status == "OPERATE") {
                                mach_status = "運轉";
                                try {
                                    document.getElementById(Dev_Name).style.color = "#000000";
                                    document.getElementById(Dev_Name).style.backgroundColor = "#00b400";

                                }
                                catch {

                                }

                                Oper_Count++;
                            }

                            else if (mach_status == "離線" || mach_status == "DISCONNECT") {
                                mach_status = "離線";
                                try {
                                    document.getElementById(Dev_Name).style.color = "#000000";
                                    document.getElementById(Dev_Name).style.backgroundColor = "#a9a9a9";

                                }
                                catch {

                                }
                                Disc_Count++;
                            }
                            else if (mach_status == "警報" || mach_status == "ALARM") {
                                mach_status = "警報";
                                try {
                                    document.getElementById(Dev_Name).style.color = "#000000";
                                    document.getElementById(Dev_Name).style.backgroundColor = "#ff0000";

                                }
                                catch {

                                }
                                Alarm_Count++;
                            }
                            else if (mach_status == "警告" || mach_status == "EMERGENCY") {
                                mach_status = "警告";
                                try {
                                    document.getElementById(Dev_Name).style.color = "#000000";
                                    document.getElementById(Dev_Name).style.backgroundColor = "#ff00ff";

                                }
                                catch {

                                }
                                Other++;
                            }
                            else if (mach_status == "暫停" || mach_status == "SUSPEND") {
                                mach_status = "暫停";
                                try {
                                    document.getElementById(Dev_Name).style.color = "#000000";
                                    document.getElementById(Dev_Name).style.backgroundColor = "#ffff00";

                                }
                                catch {

                                }
                                Other++;
                            }
                            else if (mach_status == "待機" || mach_status == "STOP") {
                                mach_status = "待機";
                                try {
                                    document.getElementById(Dev_Name).style.color = "#000000";
                                    document.getElementById(Dev_Name).style.backgroundColor = "#ff9b32";

                                }
                                catch {

                                }
                                Stop_Count++;
                            }
                            else if (mach_status == "手動" || mach_status == "MANUAL") {
                                mach_status = "手動";
                                try {
                                    document.getElementById(Dev_Name).style.color = "#000000";
                                    document.getElementById(Dev_Name).style.backgroundColor = "#02cdfc";

                                }
                                catch {

                                }
                                Other++;
                            }
                            else if (mach_status == "暖機" || mach_status == "WARMUP") {
                                mach_status = "暖機";
                                try {
                                    document.getElementById(Dev_Name).style.color = "#000000";
                                    document.getElementById(Dev_Name).style.backgroundColor = "#b22222";

                                }
                                catch {

                                } Other++;
                            }
                            var switch_cnc = '<%=WebUtils.GetAppSettings("switch_cnc")%>';

                            if (switch_cnc != '0') {
                                
                                if ($("#datatable_mach thead th")[0].innerText == "工單報工") {//第一筆TD不是工單報工,背景會強制轉白 20220614
                                    document.getElementById(Dev_Name).getElementsByTagName("td")[0].style.backgroundColor = "#ffffff";
                                };
                            }   

                            //取得目前欄位排序順序
                            var tharray = [];
                            $('#tr_row > th').each(function () {
                                tharray.push($(this).text())
                            })

                            //所有資料的陣列
                            for (j = 0; j < tharray.length; j++) {
                                //欄位名稱
                                var column = tablearray[tablearray.indexOf(tharray[j]) + 1];
                                //label ID
                                var td_column = Dev_Name + '_' + tablearray[tablearray.indexOf(tharray[j]) + 1];
                                //欄位值
                                var td_value = tablearray[tablearray.indexOf(tharray[j]) + 2];

                                if (column == 'mach_name') {

                                }
                                else if (column == 'mach_status') {
                                    document.getElementById(td_column).innerHTML = change_status(td_value);
                                }
                                else if (column == 'operate_rate') {
                                    document.getElementById(td_column).innerHTML = check_tdvalue(td_value) + ' %';
                                }
                                else if (column == 'count_today_rate') {
                                    document.getElementById(td_column).innerHTML = check_tdvalue(td_value) + ' %  ' + ' ( ' + check_tdvalue(count_today) + ' / ' + check_tdvalue(exp_product_count_day) + ' )';
                                }
                                else if (column == 'complete_time') {
                                    document.getElementById(td_column).innerHTML = check_value(td_value.substring(0, 11));
                                }
                                else if (column == 'finish_time') {
                                    document.getElementById(td_column).innerHTML = check_value(td_value.substring(0, 11));
                                }
                                else if (column == 'next_button') {


                                    if (switch_cnc == '0') {

                                        if (order_number != '0') {
                                            if (can_next == 'can')
                                                document.getElementById(td_column).innerHTML = '<input type="button" style="background-color:#10A0FF;width:100%;height:40px;;border-radius:15px" onclick=Next_Craft("' + Dev_Name + '","' + order_number + '","' + count_today + '","' + exp_product_count_day + '")  value="完工" >';
                                            else
                                                document.getElementById(td_column).innerHTML = '<input type="button" style="background-color:#BEBEBE;width:100%;height:40px;;border-radius:15px"   disabled="disabled" value="完工" >';
                                        }

                                        else {
                                            document.getElementById(td_column).innerHTML = '<input type="button"  style="background-color:#BEBEBE;width:100%;height:40px;;border-radius:15px"    disabled="disabled" value="完工" >';
                                        }
                                    }
                                    else {

                                        if (order_number != '0') {
                                            if (can_next == 'can')
                                                document.getElementById(td_column).innerHTML = ' <a href="javascript:void(0)"><img src="../../assets/images/canclick.png" width="50px" height="50px" data-toggle="modal" data-target="#Next_Task" onclick=Next_Task("' + Dev_Name + '","' + order_number + '","' + now_detailstatus + '","' + count_today + '","' + exp_product_count_day + '","' + now_list + '","' + next_list + '","' + staff + '")  /></a>';
                                            else
                                                document.getElementById(td_column).innerHTML = '<img src="../../assets/images/unclick.png" width="50px" height="50px"  onclick=alert("工單已完結，請由可視化主控台指派") />';
                                        }

                                        else {
                                            document.getElementById(td_column).innerHTML = '<img src="../../assets/images/unclick.png" width="50px" height="50px"  onclick=alert("工單已完結，請由可視化主控台指派") />';
                                        }
                                    }




                                }
                                else {
                                    document.getElementById(td_column).innerHTML = check_value(td_value);
                                }
                            }

                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[2].innerText = check_tdvalue(count_today_rate) + ' % ( ' + check_tdvalue(count_today) + ' / ' + check_tdvalue(exp_product_count_day) + ' )';//生產進度
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[3].innerText = check_tdvalue(operate_rate) + ' %';//稼動率
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[4].innerText = check_value(complete_time.substring(0, 11));//應完工時間
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[5].innerText = check_value(finish_time.substring(0, 11));//預計完工時間

                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[6].innerText = check_value(acts);//主軸轉速
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[7].innerText = check_value(spindlespeed);//主軸速度
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[8].innerText = check_value(prog_main);//主程式
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[9].innerText = check_value(program_run);//運行程式
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[10].innerText = check_value(override);//進給率
                            // document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[11].innerText = check_value(check_staff);//校機人員
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[11].innerText = check_value(work_staff);//加工人員
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[12].innerText = check_value(manu_id);//製令單號
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[13].innerText = check_value(product_number);//料件編號
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[14].innerText = check_value(craft_name);//工藝名稱
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[15].innerText = check_value(spindleload);//主軸負載
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[16].innerText = check_value(spindletemp);//主軸溫度
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[17].innerText = check_value(prog_main_cmd);//主程式註解
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[18].innerText = check_value(prog_run_cmd);//運行程式註解
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[19].innerText = check_value(run_time);//運轉時間
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[20].innerText = check_value(poweron_time);//通電時間
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[21].innerText = check_value(cut_time);//切削時間
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[22].innerText = check_value(custom_name);//客戶名稱
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[23].innerText = check_value(product_name);//產品名稱
                            document.getElementById("chart_" + Dev_Name).getElementsByTagName("span")[24].innerText = check_value(alarm_mesg);//異景資訊

                            function check_value(value) {
                                if (value == '')
                                    value = '\n';
                                return value;
                            }

                            function check_tdvalue(value) {
                                if (value.length == '0' || value.length == 0)
                                    value = '0';
                                return value;
                            }

                            function change_status(value) {
                                switch (value) {
                                    case "OPERATE":
                                        value = '運轉';
                                        break;
                                    case "DISCONNECT":
                                        value = '離線';
                                        break;
                                    case "ALARM":
                                        value = '警報';
                                        break;
                                    case "EMERGENCY":
                                        value = '警告';
                                        break;
                                    case "SUSPEND":
                                        value = '暫停';
                                        break;
                                    case "STOP":
                                        value = '待機';
                                        break;
                                    case "MANUAL":
                                        value = '手動';
                                        break;
                                    case "WARMUP":
                                        value = '暖機';
                                        break;
                                }
                                return value;
                            }

                            var statusString = document.getElementById("ma_canvas_status" + Dev_Name);
                            switch (mach_status) {
                                case "運轉":
                                case "OPERATE":
                                    statusString.innerText = "運轉";
                                    statusString.style.backgroundColor = "#00b400";
                                    statusString.style.color = "black";
                                    break;
                                case "離線":
                                case "DISCONNECT":
                                    statusString.innerText = "離線";
                                    statusString.style.backgroundColor = "#a9a9a9";
                                    statusString.style.color = "black";
                                    break;

                                case "警報":
                                case "ALARM":
                                    statusString.innerText = "警報";
                                    statusString.style.backgroundColor = "#ff0000";
                                    statusString.style.color = "black";
                                    break;
                                case "警告":
                                case "EMERGENCY":
                                    statusString.innerText = "警告";
                                    statusString.style.backgroundColor = "#ff00ff";
                                    statusString.style.color = "black";
                                    break;
                                case "暫停":
                                case "SUSPEND":
                                    statusString.innerText = "暫停";
                                    statusString.style.backgroundColor = "#ffff00";
                                    statusString.style.color = "black";
                                    break;
                                case "待機":
                                case "STOP":
                                    statusString.innerText = "待機";
                                    statusString.style.backgroundColor = "#ff9b32";
                                    statusString.style.color = "black";
                                    break;

                                case "手動":
                                case "MANUAL":
                                    statusString.innerText = "手動";
                                    statusString.style.backgroundColor = "#02cdfc";
                                    statusString.style.color = "black";
                                    break;

                                case "暖機":
                                case "WARMUP":
                                    statusString.innerText = "暖機";
                                    statusString.style.backgroundColor = "#b22222";
                                    statusString.style.color = "black";
                                    break;
                            }
                        })
                    });
                    document.getElementById("OperCount").innerText = Oper_Count;
                    document.getElementById("StopCount").innerText = Stop_Count;
                    document.getElementById("AlarmCount").innerText = Alarm_Count;
                    document.getElementById("DiscCount").innerText = Disc_Count;
                    document.getElementById("Others").innerText = Other;

                    document.getElementById("OperTotal").innerText = '  /  ' + (Oper_Count + Stop_Count + Alarm_Count + Disc_Count + Other) + ' 台';
                    document.getElementById("StopTotal").innerText = '  /  ' + (Oper_Count + Stop_Count + Alarm_Count + Disc_Count + Other) + ' 台';
                    document.getElementById("AlarmTotal").innerText = '  /  ' + (Oper_Count + Stop_Count + Alarm_Count + Disc_Count + Other) + ' 台';
                    document.getElementById("DiscTotal").innerText = '  /  ' + (Oper_Count + Stop_Count + Alarm_Count + Disc_Count + Other) + ' 台';
                    document.getElementById("OthersTotal").innerText = '  /  ' + (Oper_Count + Stop_Count + Alarm_Count + Disc_Count + Other) + ' 台';


                    if (Oper_Rate == 0)
                        document.getElementById("Oper_Rate").innerText = 0;
                    else
                        document.getElementById("Oper_Rate").innerText = (Oper_Rate / Dev_Count_Oper).toFixed(1);
                    if (Parts_Rate == 0)
                        document.getElementById("Parts_Rate").innerText = 0;
                    else
                        document.getElementById("Parts_Rate").innerText = (Parts_Rate / Dev_Count_Parts).toFixed(1);
                    Oper_Count = 0;
                    Stop_Count = 0;
                    Alarm_Count = 0;
                    Disc_Count = 0;
                    Other = 0;
                    Dev_Count_Oper = 0;
                    Dev_Count_Parts = 0;
                    Oper_Rate = 0;
                    Parts_Rate = 0
                }
            });
            mTimer = setTimeout(function () { GetMachineData(); }, loadTime);
            Timer_Count++;
        }


        //status_bar
        var Color_OPERATE = "rgb(127, 255, 0)";
        var Color_ALARM = "rgb(255, 0, 0)";
        var Color_STOP = "rgb(255, 230, 0)";
        var Color_DISCONNECT = "rgb(189, 189, 189)";
        var Color_EMERGENCY = "rgb(255, 0, 0)";
        var Color_SUSPEND = "rgb(128, 128, 0)";
        var Color_MANUAL = "rgb(0, 239, 239)";
        var Color_WARMUP = "rgb(137, 15, 39)";
        var Color_Bg = "rgb(255, 255, 255)";
        var Color_Font = "rgb(0,0,0)";
        var Color_Scale = "rgb(127, 127, 127)";


        var StrFirstDay = '<%=str_First_Day%>';
        var StrLastDay = '<%=str_Last_Day%>';
        var dev_list = '<%=str_Dev_Name%>';
        var StrMachID;


        //for (var i = 0; i < dev_list.split(',').length; i++) {
        //    StrMachID = '' + dev_list.split(',')[i] + '';
        //    try {
        //        draw_Axial(StrMachID)
        //    } catch { }

        //}

        var Max_hh = 24;
        var Time_rang = 1;//時間距離

        var mTimer_status;
        var Timer_Count_status = 0;//閃爍機制
        var loadTime_status = 60000;//20000//60000
        var Active = true;

        Active = false;
        //  mTimer_status = setTimeout(function () { draw_Axial(StrMachID); }, loadTime_status);//20190722++status bar即時更新



        function draw_AxialData(StrMachID, StrFirstDay, StrLastDay) {

            $.ajax({
                type: 'POST',
                url: "../../webservice/dpCNC_MachData.asmx/GetMachStatus",
                data: { Mach_ID: StrMachID, First_Day: StrFirstDay, Last_Day: StrLastDay },
                dataType: 'xml',
                success: function (docxml) {
                    if ($(docxml).find("ROOT").length > 0) {

                        //清空畫布內容
                        var canvastx = document.getElementById('ma_canvas_' + StrMachID).getContext("2d");
                        canvastx.clearRect(0, 0, 100000, 100000);
                        //var canvas = document.getElementById('ma_canvas_' + StrMachID);
                        //canvas.width = document.getElementById('ma_div_' + StrMachID).clientWidth;

                        var root = $(docxml);
                        root.find('Group').each(function () {
                            var element = $(this);
                            var start_time = element.attr('Start_time');
                            var cycle_time = element.attr('Cycle_time');
                            var nc_status = element.attr('Nc_Status');
                            var Start_time_line = element.attr('Start_time_line');
                            var End_time_line = element.attr('End_time_line');

                            draw_AxialWork(parseFloat(start_time), parseFloat(cycle_time), nc_status, StrMachID, Start_time_line, End_time_line);
                        });
                        draw_AxialTime(StrMachID);
                    }
                },
                error: function (xhr) { var msg = xhr.responseText; }
            });
        }

        function draw_Axial(StrMachID) {

            // var clientHeight_ = document.getElementById('ma_div_' + StrMachID).clientHeight;
            var clientHeight = '60';//避免高度一直累加40
            var clientWidth = document.getElementById('ma_div_' + StrMachID).clientWidth;
            var canvas = document.getElementById('ma_canvas_' + StrMachID);

            if (canvas.getContext) {
                var ctx = canvas.getContext("2d");
                ctx.canvas.width = clientWidth;
                ctx.canvas.height = clientHeight;
                ctx.fillStyle = Color_Bg;
                ctx.fillRect(0, 0, clientWidth, clientHeight);
                draw_AxialData(StrMachID, StrFirstDay, StrLastDay);
            }
            mTimer_status = setTimeout(function () { draw_Axial(StrMachID); }, loadTime_status);
            Timer_Count_status++;//20190722++status bar即時更新
        }
        function draw_AxialWork(X_Line_Start, X_Line_End, nc_status, StrMachID, Start_time_line, End_time_line) {
            //    var clientHeight = document.getElementById('ma_div_' + StrMachID).clientHeight;
            var clientWidth = document.getElementById('ma_div_' + StrMachID).clientWidth;
            var canvas = document.getElementById('ma_canvas_' + StrMachID);

            if (canvas.getContext) {
                var ctx = canvas.getContext("2d");
                var Width_Size = (clientWidth) / (Max_hh * 60);
                var X_Start = Width_Size * X_Line_Start;
                var X_End = Width_Size * X_Line_End;

                if ($(window).width() < 768) {
                    ctx.font = '7pt Calibri';
                } else {
                    ctx.font = '10pt Calibri';
                }
                ctx.fillStyle = Color_Font;//寫字
                switch (nc_status) {
                    case "OPERATE":
                        width_y = 20;
                        ctx.fillStyle = Color_OPERATE_new;
                        break;
                    case "MANUAL":
                        width_y = 20;
                        ctx.fillStyle = Color_MANUAL_new;
                        break;
                    case "WARMUP":
                        width_y = 20;
                        ctx.fillStyle = Color_WARMUP_new;//Color_WARMUP
                        break;
                    case "ALARM":

                        ctx.fillStyle = Color_ALARM_new;
                        break;
                    case "EMERGENCY":

                        ctx.fillStyle = Color_EMERGENCY_new;//Color_EMERGENCY
                        break;
                    case "STOP":

                        ctx.fillStyle = Color_STOP_new;
                        break;
                    case "SUSPEND":

                        ctx.fillStyle = Color_SUSPEND_new;
                        break;
                    case "DISCONNECT":

                        ctx.fillStyle = Color_DISCONNECT;
                        break;
                    default:

                        ctx.fillStyle = Color_Scale;
                }
                ctx.fillRect(X_Start, 10, X_End, 40);//畫顏色
            }
        }
        function draw_AxialTime(StrMachID) {
            var clientHeight = document.getElementById('ma_div_' + StrMachID).clientHeight;
            var clientWidth = document.getElementById('ma_div_' + StrMachID).clientWidth;
            var canvas = document.getElementById('ma_canvas_' + StrMachID);

            if (canvas.getContext) {
                var ctx = canvas.getContext("2d");
                var t = clientWidth / Max_hh;
                if ($(window).width() < 768) {
                    ctx.font = '7pt Calibri';
                } else {
                    ctx.font = '12pt Calibri';
                }

                ctx.fillStyle = Color_Font;
                ctx.fillText("0h", 0, 14);
                var j = 0;
                for (i = 1; i < Max_hh + 1; i++) {

                    if (j <= Max_hh) {
                        j += Time_rang;
                        var offset = 10;
                        if (j == Max_hh)
                            offset = 20
                        var x = parseInt(j * t, 10) - offset;
                        ctx.fillText(j, x, 14);
                    }
                    var x = parseInt(i * t, 10);
                    ctx.beginPath();
                    ctx.moveTo(x, 22);//灰色線條"||"
                    ctx.lineTo(x, 50);
                    ctx.stroke();
                }


                ctx.beginPath();
                ctx.moveTo(0, 50);//灰色線條"一一"
                ctx.lineTo(clientWidth, 50);
                ctx.stroke();
            }
        }


        window.onload = show_value();
        function show_value() {



            document.getElementById("OperCount").innerText = <%=run %> ;
            document.getElementById("StopCount").innerText = <%=rest%>;
            document.getElementById("AlarmCount").innerText = <%=alert%>;
            document.getElementById("DiscCount").innerText = <%=noline%>;
            document.getElementById("Others").innerText = <%=other%>;

            document.getElementById("OperTotal").innerText = '  /  ' + <%=run+rest+alert+noline+other%>+ ' 台';
            document.getElementById("StopTotal").innerText = '  /  ' + <%=run+rest+alert+noline+other%>+ ' 台';
            document.getElementById("AlarmTotal").innerText = '  /  ' + <%=run+rest+alert+noline+other%>+ ' 台';
            document.getElementById("DiscTotal").innerText = '  /  ' + <%=run+rest+alert+noline+other%>+ ' 台';
            document.getElementById("OthersTotal").innerText = '  /  ' + <%=run+rest+alert+noline+other%>+ ' 台';



            document.getElementById("Oper_Rate").innerText = <%=percent%>;
            document.getElementById("Parts_Rate").innerText = <%=progress%>;


            var urlname = document.URL;
            if (urlname.indexOf('tab_content2') != -1) {
                document.getElementById("ContentPlaceHolder1_div_button").style.display = 'none';
                                <%=js_code%>
            }
            if (urlname.indexOf('tab_content3') != -1) {
                document.getElementById("ContentPlaceHolder1_div_button").style.display = 'none';
                gantt_show();
            }

            var WhatSystem = navigator.userAgent;
            if (WhatSystem.match(/android/i) || WhatSystem.match(/(iphone|ipad|ipod);?/i)) {

                document.getElementById("ContentPlaceHolder1_div_button").style.display = 'none';
            }
        }

        function showbar() {
            var label_onclick = document.getElementById("ContentPlaceHolder1_Label_onclickact").innerText;

            if (label_onclick == '0') {
                <%=js_code%>
            }
            document.getElementById("ContentPlaceHolder1_Label_onclickact").innerHTML = '1';

            document.getElementById("ContentPlaceHolder1_div_button").style.display = 'none';
        }

        function show_control() {
            var WhatSystem = navigator.userAgent;
            if (WhatSystem.match(/android/i) || WhatSystem.match(/(iphone|ipad|ipod);?/i)) {

            } else

                document.getElementById("ContentPlaceHolder1_div_button").style.display = 'block';
        }


        //防止切換頁籤時跑版
        $(document).ready(function () {

            $('#datatable_mach').DataTable({
                destroy: true,
                language: {
                    "processing": "處理中...",
                    "loadingRecords": "載入中...",
                    "lengthMenu": "顯示 _MENU_ 項結果",
                    "zeroRecords": "沒有符合的結果",
                    "info": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
                    "infoEmpty": "顯示第 0 至 0 項結果，共 0 項",
                    "infoFiltered": "(從 _MAX_ 項結果中過濾)",
                    "infoPostFix": "",
                    "search": "搜尋:",
                    "paginate": {
                        "first": "第一頁",
                        "previous": "上一頁",
                        "next": "下一頁",
                        "last": "最後一頁"

                    }
                },
                colReorder: true,
                "order": [[1, "asc"]],

                scrollCollapse: true,

                "lengthMenu": [[-1, 10, 25, 50, 100], ["All", 10, 25, 50, 100]],
                dom: "<'row'<'pull-left'f>'row'<'col-sm-3'>'row'<'col-sm-3'B>'row'<'pull-right'l>>" +
                    "<rt>" +
                    "<'row'<'pull-left'i>'row'<'col-sm-4'>row'<'col-sm-3'>'row'<'pull-right'p>>",

                buttons: [
                    'copy', 'csv', 'excel', 'pdf', 'print'
                ],

            });

            jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
            $('#example').DataTable({
                responsive: true
            });
            $('#exampleInTab').DataTable({
                responsive: true
            });

            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                $($.fn.dataTable.tables(true)).DataTable()
                    .columns.adjust();
            });
        });
        //-----------------------------

        function change_icon(Icon_ID, Text_ID) {
            if (document.getElementById(Icon_ID).className == "fa fa-chevron-circle-down") {
                document.getElementById(Icon_ID).className = "fa fa-chevron-circle-up";
                document.getElementById(Text_ID).innerText = "收起詳細資料";
            }
            else {
                document.getElementById(Icon_ID).className = "fa fa-chevron-circle-down";
                document.getElementById(Text_ID).innerText = "展開詳細資料";
            }
        }
        function gantt_show() {

            document.getElementById("ContentPlaceHolder1_div_button").style.display = 'none';
            $(".gantt").gantt({
                source: [
                <%=Gantt_Data%>
                ],
                navigate: 'scroll',//buttons  scroll
                scale: "hours",// months  weeks days  hours
                maxScale: "months",
                minScale: "hours",
                itemsPerPage: 20,
                onItemClick: function (data) {//有数据范围内的点击事件
                    //	 alert(name);
                },


            });
            setTimeout(function () { testtop(); }, 900);
        }

        function jump(url) {
            document.location.href = "Machine_list_info_details.aspx?key=" + url;
        }
        function change_showview(mach) {
            var images = document.getElementById(mach + '_img');
            var videos = document.getElementById(mach + '_video');
            //影片隱藏時
            if (videos.style.display == 'none') {
                videos.style.display = 'initial';
                images.style.display = 'none';
            }
            else {
                videos.style.display = 'none';
                images.style.display = 'initial';
            }
        }

        function gotocamera(url) {
            if (url != '此機台無安裝攝影機')
                window.open(url);
            else
                alert(url);
        }
        //返回上一頁的時候在原來的TAB
        $(function () {
            var hash = window.location.hash;
            hash && $('ul.nav a[href="' + hash + '"]').tab('show');

            $('.nav-tabs a').click(function (e) {
                $(this).tab('show');
                var scrollmem = $('body').scrollTop() || $('html').scrollTop();
                window.location.hash = this.hash;
                $('html,body').scrollTop(scrollmem);
            });
        });

        //依據X軸內容改變Y軸內容
        function dropdownlist_change(x, y) {
            $('#' + y).empty();


            var ddlist = document.getElementById(x);
            //取得當前X的值
            var now_value = ddlist.value;
            var nowlist = now_value.split(',');
            var ddl = document.getElementById(y);

            for (i = 0; i < nowlist.length - 1; i++) {
                var option = document.createElement("OPTION");
                option.innerHTML = nowlist[i];
                option.value = nowlist[i + 1];
                ddl.options.add(option);
                i++;
            }
            document.getElementById('div_machines').style.display = 'none';

        }

        function show_machines(y) {
            var machine_name = '';
            var ddlist = document.getElementById(y);
            //取得當前X的值
            var now_value = ddlist.value;
            var nowlist = now_value.split('^');
            var label = document.getElementById('machines');
            label.innerHTML = '';
            for (i = 1; i < nowlist.length - 1; i++) {
                if (nowlist[i] != '') {
                    label.innerHTML += nowlist[i] + '<br />';
                    machine_name += nowlist[i] + ',';
                }

            }
            $('#ContentPlaceHolder1_TextBox_Machines').val('' + machine_name + '');
            //if (nowlist.length - 1 > 1)
            //    document.getElementById('div_machines').style.display = 'block';
        }

        window.onload = set_content();

        function set_content() {
            var machine_name = '';
            var mach = document.getElementById("ContentPlaceHolder1_TextBox_Machines").value;
            var label = document.getElementById('machines');
            label.innerHTML = '';
            if (mach != '') {
                var machines = mach.split(',');
                for (i = 0; i < machines.length - 1; i++) {
                    if (machines[i] != '') {
                        label.innerHTML += machines[i] + '<br />';
                        machine_name += machines[i] + ',';
                    }
                }
                $('#ContentPlaceHolder1_TextBox_Machines').val('' + machine_name + '');
                //document.getElementById('div_machines').style.display = 'block';
            }
        }


        //抓取加跟減的事件
        $(document).click(function (event) {
            var text = $(event.target).text();
            if (text == '+' || text == '-')
                setTimeout(function () { testtop(); }, 1250);
        });
        //要做在load執行
        console.log("123");
        var gp1_tr = "<tr class=GP_Tr><td colspan=2 class=GP_Td> <div class=GP_Content><button type=button class=GP_Btn id=GP1><div id=GP_Title>工單資訊</div><div id=GP1_Arrow class='fa fa-chevron-up'></div></button></div></td></tr>";
        var gp2_tr = "<tr class=GP_Tr><td colspan=2 class=GP_Td> <div class=GP_Content><button type=button class=GP_Btn id=GP2><div id=GP_Title>設備資訊</div><div id=GP2_Arrow class='fa fa-chevron-up'></div></button></div></td></tr>";
        var checkbox_Id = ($('#ContentPlaceHolder1_CheckBoxList_cloumn input[type="checkbox"]').length) - 1;
        var arryList = ["check_staff", "work_staff", "manu_id", "custom_name", "product_name", "product_number", "craft_name", "count_today_rate", "complete_time", "finish_time","next_button"];
        var gp1_td = "";
        var gp2_td = "";
        var count_gp1 = 0;
        var count_gp1_loop = 0;
        var count_gp1_last = 0;
        var count_gp2 = 0;
        var count_gp2_loop = 0;
        var count_gp2_last = 0;
        console.log(typeof checkbox_Id, checkbox_Id);

        for (var i = 0; i <= checkbox_Id; i++) {
            if (arryList.indexOf($(`#ContentPlaceHolder1_CheckBoxList_cloumn_${i}`).val()) != -1) {
                count_gp1++;
            } else {
                count_gp2++;
            }
        }

        for (var i = 0; i <= checkbox_Id; i++) {
            if (arryList.indexOf($(`#ContentPlaceHolder1_CheckBoxList_cloumn_${i}`).val()) != -1) {
                count_gp1_loop++;
                count_gp1_last++;
                //console.log($(`#ContentPlaceHolder1_CheckBoxList_cloumn_${i}`).parent()[0].outerHTML);

                if (count_gp1_loop < 2) {
                    gp1_td = "<tr class=gp1>";
                    gp1_td += $(`#ContentPlaceHolder1_CheckBoxList_cloumn_${i}`).parent()[0].outerHTML;
                    if (count_gp1_last == count_gp1) {
                        gp1_td += "</tr>";
                        gp1_tr += gp1_td;
                    }
                } else {
                    count_gp1_loop = 0;
                    gp1_td += $(`#ContentPlaceHolder1_CheckBoxList_cloumn_${i}`).parent()[0].outerHTML;
                    gp1_td += "</tr>";
                    gp1_tr += gp1_td;
                }
            } else {
                count_gp2_loop++;
                count_gp2_last++;
                //console.log($(`#ContentPlaceHolder1_CheckBoxList_cloumn_${i}`).parent()[0].outerHTML);

                if (count_gp2_loop < 2) {
                    gp2_td = "<tr class=gp2>";
                    gp2_td += $(`#ContentPlaceHolder1_CheckBoxList_cloumn_${i}`).parent()[0].outerHTML;
                    if (count_gp2_last == count_gp2) {
                        gp2_td += "</tr>";
                        gp2_tr += gp2_td;
                    }
                } else {
                    count_gp2_loop = 0;
                    gp2_td += $(`#ContentPlaceHolder1_CheckBoxList_cloumn_${i}`).parent()[0].outerHTML;
                    gp2_td += "</tr>";
                    gp2_tr += gp2_td;
                }


            }
        }
        
        

        document.getElementById("ContentPlaceHolder1_CheckBoxList_cloumn").innerHTML = "";
        document.getElementById("ContentPlaceHolder1_CheckBoxList_cloumn").innerHTML += gp1_tr;
        document.getElementById("ContentPlaceHolder1_CheckBoxList_cloumn").innerHTML += gp2_tr;
        $("#GP1").click(function () {
            $(".gp1").toggle();
            $("GP1_Arrow").removeClass();
            if ($("#GP1_Arrow").attr("class") == "fa fa-chevron-up") {
                $("#GP1_Arrow").removeClass();
                $("#GP1_Arrow").addClass("fa fa-chevron-down");
            } else {
                $("#GP1_Arrow").removeClass();
                $("#GP1_Arrow").addClass("fa fa-chevron-up");
            }
        });
        $("#GP2").click(function () {
            $(".gp2").toggle();
            $("GP2_Arrow").removeClass();
            if ($("#GP2_Arrow").attr("class") == "fa fa-chevron-up") {
                $("#GP2_Arrow").removeClass();
                $("#GP2_Arrow").addClass("fa fa-chevron-down");
            } else {
                $("#GP2_Arrow").removeClass();
                $("#GP2_Arrow").addClass("fa fa-chevron-up");
            }
        });

        
        
    </script>
</asp:Content>
