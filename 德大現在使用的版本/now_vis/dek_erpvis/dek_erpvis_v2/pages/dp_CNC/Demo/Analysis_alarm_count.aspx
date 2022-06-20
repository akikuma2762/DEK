<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Analysis_alarm_count.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_CNC.Analysis_alarm_count" %>
<html class=" ">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">

    <!-- Meta, title, CSS, favicons, etc. -->
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- Bootstrap -->
    <link href="../../assets/vendors/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Font Awesome -->
    <link href="../../assets/vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <!-- NProgress -->
    <link href="../../assets/vendors/nprogress/nprogress.css" rel="stylesheet">
    <!-- iCheck -->
    <link href="../../assets/vendors/iCheck/skins/flat/green.css" rel="stylesheet">

    <!-- Select2 -->
    <link href="../../assets/vendors/select2/dist/css/select2.min.css" rel="stylesheet">
    <!-- Switchery -->
    <link href="../../assets/vendors/switchery/dist/switchery.min.css" rel="stylesheet">
    <!-- starrr -->
    <link href="../../assets/vendors/starrr/dist/starrr.css" rel="stylesheet">
    <!-- bootstrap-daterangepicker -->
    <link href="../../assets/vendors/bootstrap-daterangepicker/daterangepicker.css" rel="stylesheet">
    <!-- Custom Theme Style -->
    <link href="../../assets/build/css/custom.min.css" rel="stylesheet">
    <!-- Datatables -->
    <link href="../../assets/vendors/datatables.net-bs/css/dataTables.bootstrap.min.css" rel="stylesheet">
    <link href="../../assets/vendors/datatables.net-buttons-bs/css/buttons.bootstrap.min.css" rel="stylesheet">
    <link href="../../assets/vendors/datatables.net-fixedheader-bs/css/fixedHeader.bootstrap.min.css" rel="stylesheet">
    <link href="../../assets/vendors/datatables.net-responsive-bs/css/responsive.bootstrap.min.css" rel="stylesheet">
    <link href="../../assets/vendors/datatables.net-scroller-bs/css/scroller.bootstrap.min.css" rel="stylesheet">
    <!-- FloatingActionButton -->
    <link rel="stylesheet" href="../../assets/vendors/FloatingActionButton/css/animate.min.css">
    <link rel="stylesheet" href="../../assets/vendors/FloatingActionButton/css/family.css">
    <link rel="stylesheet" href="../../assets/vendors/FloatingActionButton/css/style.css">
    <style>
        #loading {
            position: fixed;
            z-index: 400;
            width: 100%;
            height: 100%;
            top: 0;
            left: 0%;
            text-align: center;
            font-size: 0.9rem;
            color: #595758;
            background-color: #ffffff;
        }
    </style>

    <title>異常次數統計 | 德大機械</title>
    <link href="../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.css" rel="stylesheet" type="text/css" media="all">
    <link href="../../assets/build/css/custom.css" rel="stylesheet">
    <title>

    </title>
</head>

<body class="nav-md footer_fixed " style="font-family: Microsoft JhengHei;">
    <!--<div id="loading">
        <img src="../../images/loading.gif" alt="loading.." />
    </div>-->
    <form method="post" action="./Analysis_alarm_count.aspx" id="form1">
        <div class="aspNetHidden">
            <input type="hidden" name="__EVENTTARGET" id="__EVENTTARGET" value="">
            <input type="hidden" name="__EVENTARGUMENT" id="__EVENTARGUMENT" value="">
            <input type="hidden" name="__LASTFOCUS" id="__LASTFOCUS" value="">
            <input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="3TG/E4jiYxbkOQykKzaxuDMi0VU7BghoQxJqYCZLdcqUszLltBBiKGv9eaga1wEP5HHyuEJu/2wJ0rEwqzOyAGJFbSk8nveygSyT0Ww7asDYS5hBRauw9Dk8JW0W1DFP2CXvMXtltUwpKe/DB8Iinvs/rC2ZDKaIESiapW9GYaaGOxNs44Y4LRHvn00cgsyQc9rDgnzae47E1/ORxBFFfmGZoFMYE7ONCtJsyaYTSrWybxz0b0j5r4C0PoPyljAVBLeOVjYmnz+gPnwqV0YJLw==">
        </div>

        <script type="text/javascript">
            //<![CDATA[
            var theForm = document.forms['form1'];
            if (!theForm) {
                theForm = document.form1;
            }

            function __doPostBack(eventTarget, eventArgument) {
                if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                    theForm.__EVENTTARGET.value = eventTarget;
                    theForm.__EVENTARGUMENT.value = eventArgument;
                    theForm.submit();
                }
            }
            //]]>
        </script>

        <script src="/WebResource.axd?d=pynGkmcFUV13He1Qd6_TZCeMjrZ26b_W0uEr_V2iIhXARWQSjpmgiDCOaQR1UrCbaEnTT95Xe-yvIeH_PxUCag2&amp;t=636995627080000000" type="text/javascript"></script>

        <script src="/ScriptResource.axd?d=xKHJeHG1PQeJ9W2S-7YdJOnlgzfbjwois99g4fpatMbXGuIRiz00AcKW0ta7l8J3-zaVOKCdoEeRiidiW2bwV1JeL-PT9eOznUluOCpETj_gRbiSWT0oLe_PWfHudnrPXKBQA9MHoKRz6s8OcV_7thngLZEdqmjkH8Ej9XHp0SY1&amp;t=4065f642" type="text/javascript"></script>
        <script type="text/javascript">
            //<![CDATA[
            if (typeof(Sys) === 'undefined') throw new Error('ASP.NET Ajax 用戶端架構無法載入。');
            //]]>
        </script>

        <script src="/ScriptResource.axd?d=GBZXTGRyPvsbqIFyUyWWcP9G_2eoFEkpNHWaFGh7Ub48qtMHhk9t0dIY3-Q1lLFdzp7ML9mD0Ox0Rg5NXyfl123mHrrFTb0yxkGRt7MHSQbv_iRQj3I_wzNPHwUTpMpvf4FStnecCtgb_G1HNXuBd-0eA7SXWRKo7as1yrGWpS7F7uOAHMa3GvMLcQIVsOkh0&amp;t=4065f642" type="text/javascript"></script>
        <div class="aspNetHidden">

            <input type="hidden" name="__VIEWSTATEGENERATOR" id="__VIEWSTATEGENERATOR" value="AE2C77D4">
            <input type="hidden" name="__EVENTVALIDATION" id="__EVENTVALIDATION" value="v7Zb8WPLgX8uFxYySrqIBILRh+2Y0A1/OUWvQ46qoGpQr6Tl+b0OUMaJsaS6gYnb1+UfAw9+dNZArNno3vHH9FjHFS/ACvtKJKGL7inYKySYU/JmCBYDaFQu1FvG92GgR6y19CJ6UlrBTewhA0Tk1nwWTJCUXoL6WAPqtBrQVrw2RYt8YdqB6HJ4q//z3ZjWO4NsIVcRoI4xWRFzk+d+ckyPZ2GPuTjoPgY1DxXhP+hvz3fjzecyXO3jh3eLHzle1EOGqdNZJeyOvLK3+QdOWPCMU2sAtuzr+9tASfhRZdrA5/ONYYIxuI5k3qvg/EKpbbcoULOYJW0W2sg8XKT1hFllKB2cZujp9Rnn0ODRMC3L9JS676D82geFlVASjurPUm0S30kwg8QQ8SlbIakSlw==">
        </div>
        <div class="container body">
            <div class="main_container">
                <div class="col-md-3 left_col">
                    <div class="left_col scroll-view">
                        <div class="navbar nav_title" style="border: 0;">
                            <a href="/pages/index.aspx" class="site_title"><span class="center" style="color: #ffffff;"><strong>dek 德科智能科技</strong></span></a>
                        </div>
                        <div class="clearfix"></div>
                        <!-- menu profile quick info -->
                        <div class="profile clearfix">
                            <div class="profile_pic">
                                <img src="../../assets/images/img.jpg" alt="..." class="img-circle profile_img">
                            </div>
                            <div class="profile_info">
                                <span style="font-size: 18px; color: #ffffff;">歡迎您 ,</span>
                                <p style="font-size: 18px; color: #ffffff;"></p>
                                <!--<h2>deta user</h2>-->
                            </div>
                        </div>
                        <!-- /menu profile quick info -->

                        <br>

                        <!-- sidebar menu -->
                        <div id="sidebar-menu" class="main_menu_side hidden-print main_menu">
                            <div class="menu_section">
                                <h3 id="showbox" class="text-center"></h3>
                                <!--<div id ="showbox" class="text-center" style="color:#ffffff"></div>-->
                                <!--2018/12/22 12:00:00-->
                                <ul class="nav side-menu">
                                    <!--<a href="/pages/SYS_CONTROL/dp_fuclist.aspx?dp=SLS">-->
                                    <li><a><i class="fa fa-suitcase"></i>業務部 <span class="fa fa-chevron-down"></span></a>
                                        <ul class="nav child_menu">
                                            <li><a href="/pages/dp_SD/Orders.aspx">訂單數量及金額統計</a></li>
                                            <li><a href="/pages/dp_SD/recordsofchangetheorder.aspx">訂單變更紀錄</a></li>
                                            <li><a href="/pages/dp_SD/shipment.aspx">出貨統計表</a></li>
                                            <li><a href="/pages/dp_SD/transportrackstatistics.aspx">運輸架未歸還統計</a></li>
                                            <li><a href="/pages/dp_SD/UntradedCustomer.aspx">未交易客戶</a></li>

                                            <!--<li><a href="/pages/dp_SD/stockanalysis.aspx">01成品庫存分析</a></li>
                                            <li><a href="/pages/dp_SD/waitingfortheproduction.aspx">02未生產分析</a></li>
                                            <li><a href="/pages/dp_SD/shipment.aspx">03出貨統計表</a></li>
                                            <li><a href="/pages/dp_SD/orderstatistics.aspx?type=count">04訂單數量統計</a></li>
                                            <li><a href="/pages/dp_SD/orderstatistics.aspx?type=sales">05訂單金額統計</a></li>
                                            <li><a href="/pages/dp_SD/recordsofchangetheorder.aspx">06訂單變更紀錄</a></li>
                                            <li><a href="/pages/dp_SD/transportrackstatistics.aspx">07運輸架未歸統計</a></li>-->
                                        </ul>
                                    </li>
                                    <!--<a href="/pages/SYS_CONTROL/dp_fuclist.aspx?dp=PCD">-->
                                    <li><a><i class="fa fa-cog"></i>資材部 <span class="fa fa-chevron-down"></span></a>
                                        <ul class="nav child_menu">
                                            <li><a href="/pages/dp_PD/materialrequirementplanning.aspx">物料領用統計表</a></li>
                                            <li><a href="/pages/dp_PD/supplierscore.aspx">供應商達交率</a></li>
                                            <li><a href="/pages/dp_PD/SupplierShortage.aspx">供應商催料總表</a></li>
                                            <!--<li><a href="/pages/dp_PD/materialrequirementplanning.aspx">01物料領用統計表</a></li>
                                            <li><a href="/pages/dp_PD/supplierscore.aspx">02供應商達交率</a></li>-->
                                        </ul>
                                    </li>
                                    <!--<a href="/pages/SYS_CONTROL/dp_fuclist.aspx?dp=WHE">-->
                                    <li><a><i class="fa fa-cubes"></i>倉管部 <span class="fa fa-chevron-down"></span></a>
                                        <ul class="nav child_menu">
                                            <li><a href="/pages/dp_WH/stockanalysis.aspx">成品庫存分析</a></li>
                                            <li><a href="/pages/dp_WH/InactiveInventory.aspx">呆滯物料統計表</a></li>
                                            <li><a href="/pages/dp_WH/scrapped.aspx">報廢數量統計表</a></li>
                                            <!--<li><a href="/pages/dp_WH/InactiveInventory.aspx">01呆滯物料統計</a></li>
                                            <li><a href="/pages/dp_WH/scrapped.aspx">02報廢數量統計</a></li>-->
                                        </ul>
                                    </li>
                                    <!--<a href="/pages/SYS_CONTROL/dp_fuclist.aspx?dp=PMD">-->
                                    <li><a><i class="fa fa-bar-chart"></i>生產部 <span class="fa fa-chevron-down"></span></a>
                                        <ul class="nav child_menu">
                                            <li><a href="/pages/dp_PM/waitingfortheproduction.aspx">生產推移圖</a></li>
                                            <li><a href="/pages/dp_PM/Asm_LineTotalView.aspx">整廠進度管理看板</a></li>
                                            <li><a href="/pages/dp_PM/Asm_Cahrt_Error.aspx">異常統計分析</a></li>
                                            <li><a href="/pages/dp_PM/Asm_history.aspx">組裝歷史查詢</a></li>
                                            <li><a href="/pages/dp_PM/Asm_ErrorSearch.aspx">組裝異常歷史查詢</a></li>
                                            <!--<li><a href="/pages/dp_PM/Asm_LineTotalView.aspx">01整廠進度管理看板</a></li>
                                            <li><a href="/pages/dp_PM/Asm_ErrorSearch.aspx">02組裝異常歷程查詢</a></li>
                                            <li><a href="/pages/dp_PM/Asm_Cahrt_Error.aspx">03組裝異常原因分析</a></li>
                                            <li><a href="/pages/dp_SD/waitingfortheproduction.aspx?st=25,edt=26">04生產推移圖</a></li>-->
                                        </ul>
                                    </li>
                                    <li><a><i class="fa fa-filter"></i>加工部 <span class="fa fa-chevron-down"></span></a>
                                        <ul class="nav child_menu">
                                            <li><a href="/pages/dp_CNC/Machine_list_info.aspx">設備監控看板</a></li>
                                            <li><a href="/pages/dp_CNC/Analysis_oper_rate.aspx">稼動比例統計</a></li>
                                            <li><a href="/pages/dp_CNC/Analysis_alarm_count.aspx">異常次數統計</a></li>
                                        </ul>
                                    </li>
                                    <li><a href="/pages/SYS_CONTROL/QA.aspx"><i class="fa fa-info-circle"></i>簡易導覽 <span class="fa fa-chevron-down"></span><span id=""></span></a>
                                    </li>
                                    <li><a href="/pages/SYS_CONTROL/set_parameter.aspx"><i class="fa fa-cog"></i>參數設定 <span class="fa fa-chevron-down"></span><span id=""></span></a>
                                    </li>
                                    <li><a><i class="fa fa-code"></i>系統管理 <span class="fa fa-chevron-down"></span><span id="notice_parents"></span></a>
                                        <ul class="nav child_menu">
                                            <li><a href="/pages/SYS_CONTROL/rights_mag.aspx">01權限管理</a></li>
                                            <li><a href="/pages/SYS_CONTROL/rights_application.aspx">02待審權限<span id="notice_child"></span></a></li>
                                        </ul>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <!-- /sidebar menu -->

                        <!-- /menu footer buttons -->
                        <div class="sidebar-footer hidden-small">
                            <a data-toggle="tooltip" data-placement="top" title="" onclick="footerbuttons('')" data-original-title="Settings"><span class="glyphicon glyphicon-cog" aria-hidden="true"></span>
                            </a>
                            <a data-toggle="tooltip" data-placement="top" title="" data-original-title="FullScreen">
                                <span class="glyphicon glyphicon-fullscreen" aria-hidden="true"></span>
                            </a>
                            <a data-toggle="tooltip" data-placement="top" title="" data-original-title="Lock">
                                <span class="glyphicon glyphicon-eye-close" aria-hidden="true"></span>
                            </a>
                            <a data-toggle="tooltip" data-placement="top" title="" id="btn_check" data-original-title="Logout">
                                <span class="glyphicon glyphicon-off" aria-hidden="true"></span>
                            </a>
                        </div>
                        <!-- /menu footer buttons -->
                    </div>
                </div>

                <!-- top navigation -->
                <div class="top_nav">
                    <div class="nav_menu">
                        <nav>
                            <div class="nav toggle">
                                <a id="menu_toggle" style="color: #5A738E;"><i class="fa fa-bars"></i></a>
                            </div>

                            <ul class="nav navbar-nav navbar-right">
                                <li class="">
                                    <a href="javascript:;" class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                        <img src="../../assets/images/img.jpg" alt="">
                                        <span class=" fa fa-angle-down"></span>
                                    </a>
                                    <ul class="dropdown-menu dropdown-usermenu pull-right">
                                        <li>
                                            <a href="/pages/SYS_CONTROL/profile.aspx">個人檔案管理</a>
                                        </li>
                                        <li>
                                            <a data-toggle="modal" data-target="#logoutModal">
                                                <i class="fa fa-sign-out pull-right"></i>Log Out
                                            </a>
                                        </li>
                                    </ul>
                                </li>
                            </ul>
                        </nav>
                    </div>

                </div>
                <!-- /top navigation -->
                <!-- page content -->

                <!-- page content -->
                <div class="right_col" role="main" style="min-height: 683px;">
                    <ol class="breadcrumb_">
                        <li><u><a href="../index.aspx">加工部 </a></u></li>
                        <li><u><a href="Analysis_alarm_count.aspx">異常次數統計</a></u></li>
                    </ol>
                    <div class="">
                        <div class="x_panel">
                            <h1><b><p align="center">分析時間：2019-09-01 00:00:00 ~ 2019-09-29 21:09:02</p></b><p><b></b></p></h1>
                        </div>
                        <div class="row">
                            <div class="col-md-6 col-sm-12 col-xs-12" style="padding-right:1px">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h1><b>異常統計(單位:次數)</b></h1>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div class="col-md-10 col-sm-12 col-xs-12">
                                            <div id="mach_list" style="height: 450px;max-width: 920px; margin: 0px 0px 0px -23px;">
                                                <div class="canvasjs-chart-container" style="position: relative; text-align: left; cursor: auto;">
                                                    <canvas class="canvasjs-chart-canvas" width="285" height="360" style="width: 357px; height: 450px; position: absolute;"></canvas>
                                                    <canvas class="canvasjs-chart-canvas" width="285" height="360" style="width: 357px; height: 450px; position: absolute; -webkit-tap-highlight-color: transparent;"></canvas>
                                                    <div class="canvasjs-chart-toolbar" style="position: absolute; right: 1px; top: 1px; border: 1px solid transparent;">
                                                        <button state="menu" type="button" title="More Options" style="background-color: white; color: black; border: none; user-select: none; padding: 5px 12px; cursor: pointer; float: left; width: 40px; height: 25px; outline: 0px; vertical-align: baseline; line-height: 0; display: inline;"><img style="height:95%;" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAeCAYAAABE4bxTAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAAJcEhZcwAADsMAAA7DAcdvqGQAAADoSURBVFhH7dc9CsJAFATgRxIIBCwCqZKATX5sbawsY2MvWOtF9AB6AU8gguAJbD2AnZ2VXQT/Ko2TYGCL2OYtYQc+BuYA+1hCtnCVwMm27SGaXpDJIAiCvCkVR05hGOZNN3HkFMdx3nQRR06+76/R1IcFLJlNQEWlmWlBTwJtKLKHynehZqnjOGM0PYWRVXk61C37p7xlZ3Hk5HneCk1dmMH811xGoKLSzDiQwIBZB4ocoPJdqNkDt2yKlueWRVGUtzy3rPwo3sWRU3nLjuLI6OO67oZM00wMw3hrmpZx0XU9syxrR0T0BeMpb9dneSR2AAAAAElFTkSuQmCC" alt="More Options"></button>
                                                        <div tabindex="-1" style="position: absolute; user-select: none; cursor: pointer; right: 0px; top: 25px; min-width: 120px; outline: 0px; font-size: 14px; font-family: Arial, Helvetica, sans-serif; padding: 5px 0px; text-align: left; line-height: 10px; background-color: white; box-shadow: rgb(136, 136, 136) 2px 2px 10px; display: none;">
                                                            <div style="padding: 12px 8px; background-color: white; color: black;">Print</div>
                                                            <div style="padding: 12px 8px; background-color: white; color: black;">Save as JPEG</div>
                                                            <div style="padding: 12px 8px; background-color: white; color: black;">Save as PNG</div>
                                                        </div>
                                                    </div>
                                                    <div class="canvasjs-chart-tooltip" style="position: absolute; height: auto; box-shadow: rgba(0, 0, 0, 0.1) 1px 1px 2px 2px; z-index: 1000; pointer-events: none; display: none; border-radius: 5px;">
                                                        <div style=" width: auto;height: auto;min-width: 50px;line-height: auto;margin: 0px 0px 0px 0px;padding: 5px;font-family: Calibri, Arial, Georgia, serif;font-weight: normal;font-style: italic;font-size: 14px;color: #000000;text-shadow: 1px 1px 1px rgba(0, 0, 0, 0.1);text-align: left;border: 2px solid gray;background: rgba(255,255,255,.9);text-indent: 0px;white-space: nowrap;border-radius: 5px;-moz-user-select:none;-khtml-user-select: none;-webkit-user-select: none;-ms-user-select: none;user-select: none;} "> Sample Tooltip</div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="h2 mb-0 text-primary" style="margin-bottom: 10px; color: #f77b71; text-align: center;">
                                            150
                                            <h5>(次)</h5>
                                        </div>
                                        <div class="text-muted" style="text-align: center;">Top1</div>
                                        <hr>
                                        <div class="h2 mb-0 text-primary" style="margin-bottom: 10px; text-align: center;">
                                            90
                                            <h5>(次)</h5>
                                        </div>
                                        <div class="text-muted" style="text-align: center;">Top2</div>
                                        <hr>
                                        <div class="h2 mb-0 text-primary" style="margin-bottom: 10px; text-align: center;">
                                            50
                                            <h5>(次)</h5>
                                        </div>
                                        <div class="text-muted" style="text-align: center;">Top3</div>
                                        <hr>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-12 col-xs-12" style="padding-left:5px">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h1><b>異常統計(單位:時間)</b></h1>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div class="col-md-10 col-sm-12 col-xs-12">
                                            <div id="mach_listTime" style="height: 450px;max-width: 920px; margin: 0px 0px 0px -23px;">
                                                <div class="canvasjs-chart-container" style="position: relative; text-align: left; cursor: auto;">
                                                    <canvas class="canvasjs-chart-canvas" width="282" height="360" style="width: 353px; height: 450px; position: absolute;"></canvas>
                                                    <canvas class="canvasjs-chart-canvas" width="282" height="360" style="width: 353px; height: 450px; position: absolute; -webkit-tap-highlight-color: transparent; cursor: default;"></canvas>
                                                    <div class="canvasjs-chart-toolbar" style="position: absolute; right: 1px; top: 1px; border: 1px solid transparent;">
                                                        <button state="menu" type="button" title="More Options" style="background-color: white; color: black; border: none; user-select: none; padding: 5px 12px; cursor: pointer; float: left; width: 40px; height: 25px; outline: 0px; vertical-align: baseline; line-height: 0; display: inline;"><img style="height:95%;" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAeCAYAAABE4bxTAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAAJcEhZcwAADsMAAA7DAcdvqGQAAADoSURBVFhH7dc9CsJAFATgRxIIBCwCqZKATX5sbawsY2MvWOtF9AB6AU8gguAJbD2AnZ2VXQT/Ko2TYGCL2OYtYQc+BuYA+1hCtnCVwMm27SGaXpDJIAiCvCkVR05hGOZNN3HkFMdx3nQRR06+76/R1IcFLJlNQEWlmWlBTwJtKLKHynehZqnjOGM0PYWRVXk61C37p7xlZ3Hk5HneCk1dmMH811xGoKLSzDiQwIBZB4ocoPJdqNkDt2yKlueWRVGUtzy3rPwo3sWRU3nLjuLI6OO67oZM00wMw3hrmpZx0XU9syxrR0T0BeMpb9dneSR2AAAAAElFTkSuQmCC" alt="More Options"></button>
                                                        <div tabindex="-1" style="position: absolute; user-select: none; cursor: pointer; right: 0px; top: 25px; min-width: 120px; outline: 0px; font-size: 14px; font-family: Arial, Helvetica, sans-serif; padding: 5px 0px; text-align: left; line-height: 10px; background-color: white; box-shadow: rgb(136, 136, 136) 2px 2px 10px; display: none;">
                                                            <div style="padding: 12px 8px; background-color: white; color: black;">Print</div>
                                                            <div style="padding: 12px 8px; background-color: white; color: black;">Save as JPEG</div>
                                                            <div style="padding: 12px 8px; background-color: white; color: black;">Save as PNG</div>
                                                        </div>
                                                    </div>
                                                    <div class="canvasjs-chart-tooltip" style="position: absolute; height: auto; box-shadow: rgba(0, 0, 0, 0.1) 1px 1px 2px 2px; z-index: 1000; pointer-events: none; display: none; border-radius: 5px;">
                                                        <div style=" width: auto;height: auto;min-width: 50px;line-height: auto;margin: 0px 0px 0px 0px;padding: 5px;font-family: Calibri, Arial, Georgia, serif;font-weight: normal;font-style: italic;font-size: 14px;color: #000000;text-shadow: 1px 1px 1px rgba(0, 0, 0, 0.1);text-align: left;border: 2px solid gray;background: rgba(255,255,255,.9);text-indent: 0px;white-space: nowrap;border-radius: 5px;-moz-user-select:none;-khtml-user-select: none;-webkit-user-select: none;-ms-user-select: none;user-select: none;} "> Sample Tooltip</div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="h2 mb-0 text-primary" style="margin-bottom: 10px; color: #f77b71; text-align: center;">
                                            6052.27
                                            <h5>(分鐘)</h5>
                                        </div>
                                        <div class="text-muted" style="text-align: center;">Top1</div>
                                        <hr>
                                        <div class="h2 mb-0 text-primary" style="margin-bottom: 10px; text-align: center;">
                                            494.75
                                            <h5>(分鐘)</h5>
                                        </div>
                                        <div class="text-muted" style="text-align: center;">Top2</div>
                                        <hr>
                                        <div class="h2 mb-0 text-primary" style="margin-bottom: 10px; text-align: center;">
                                            4.33
                                            <h5>(分鐘)</h5>
                                        </div>
                                        <div class="text-muted" style="text-align: center;">Top3</div>
                                        <hr>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!---------------->
                        <div class="row">
                            <div class="col-md-6 col-sm-12 col-xs-12" id="alarm_detail_count" style="padding-right:1px">
                                <div class="x_panel">
                                    <ul class="nav navbar-right panel_toolbox">
                                        <li>
                                            <a class="collapse-link">
                                                <i class="fa fa-chevron-circle-up"></i>
                                            </a>
                                        </li>
                                    </ul>
                                    <div class="x_title">
                                        <h1><strong>異常歷程(單位:次數)</strong></h1>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div id="datatable_error_count_wrapper" class="dataTables_wrapper form-inline dt-bootstrap no-footer">
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <table id="datatable_error_count" class="table table-striped table-bordered dataTable no-footer" style="margin: 0px 0px -10px 0px;" role="grid" aria-describedby="datatable_error_count_info">
                                                        <thead>
                                                            <tr style="background-color:dimgray;color: white;" role="row">
                                                                <th class="sorting" tabindex="0" aria-controls="datatable_error_count" rowspan="1" colspan="1" aria-label="設備名稱: activate to sort column ascending" style="width: 124.004px;">設備名稱</th>
                                                                <th class="sorting" tabindex="0" aria-controls="datatable_error_count" rowspan="1" colspan="1" aria-label="異常訊息: activate to sort column ascending" style="width: 262.004px;">異常訊息</th>
                                                                <th class="sorting" tabindex="0" aria-controls="datatable_error_count" rowspan="1" colspan="1" aria-label="起始時間: activate to sort column ascending" style="width: 212.004px;">起始時間</th>
                                                                <th class="sorting_desc" tabindex="0" aria-controls="datatable_error_count" rowspan="1" colspan="1" aria-sort="descending" aria-label="持續時間: activate to sort column ascending" style="width: 107.004px;">持續時間</th>
                                                                <th class="sorting" tabindex="0" aria-controls="datatable_error_count" rowspan="1" colspan="1" aria-label="作業人員: activate to sort column ascending" style="width: 107px;">作業人員</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>

                                                            <tr role="row" class="odd">
                                                                <td>J-KAFO137+</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:37</td>
                                                                <td class="sorting_1">5:53:54</td>
                                                                <td>dek_user1</td>
                                                            </tr>
                                                            <tr role="row" class="even">
                                                                <td>B-瑞峰本體 MV-20</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:37</td>
                                                                <td class="sorting_1">5:53:54</td>
                                                                <td></td>
                                                            </tr>
                                                            <tr role="row" class="odd">
                                                                <td>I-KAFO168</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:37</td>
                                                                <td class="sorting_1">5:53:53</td>
                                                                <td>Michael</td>
                                                            </tr>
                                                            <tr role="row" class="even">
                                                                <td>A-KAFO BMC2616</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:37</td>
                                                                <td class="sorting_1">5:53:53</td>
                                                                <td></td>
                                                            </tr>
                                                            <tr role="row" class="odd">
                                                                <td>H-KAFO B3116</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:37</td>
                                                                <td class="sorting_1">5:53:53</td>
                                                                <td>Michael</td>
                                                            </tr>
                                                            <tr role="row" class="even">
                                                                <td>F-KAFO137</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:37</td>
                                                                <td class="sorting_1">5:53:50</td>
                                                                <td>Michael</td>
                                                            </tr>
                                                            <tr role="row" class="odd">
                                                                <td>H-KAFO B3116</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:39</td>
                                                                <td class="sorting_1">5:53:48</td>
                                                                <td>Michael</td>
                                                            </tr>
                                                            <tr role="row" class="even">
                                                                <td>H-KAFO B3116</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:45</td>
                                                                <td class="sorting_1">5:53:45</td>
                                                                <td></td>
                                                            </tr>
                                                            <tr role="row" class="odd">
                                                                <td>D-瑞峰EV-10</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:46</td>
                                                                <td class="sorting_1">5:53:44</td>
                                                                <td></td>
                                                            </tr>
                                                            <tr role="row" class="even">
                                                                <td>C-瑞峰臥式</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:48</td>
                                                                <td class="sorting_1">5:53:42</td>
                                                                <td>Kevin</td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-12 col-xs-12" id="alarm_detail_time" style="padding-left:5px">
                                <div class="x_panel">
                                    <ul class="nav navbar-right panel_toolbox">
                                        <li>
                                            <a class="collapse-link">
                                                <i class="fa fa-chevron-circle-up"></i>
                                            </a>
                                        </li>
                                    </ul>
                                    <div class="x_title">
                                        <h1><strong>異常歷程(單位:時間)</strong></h1>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div id="datatable_error_time_wrapper" class="dataTables_wrapper form-inline dt-bootstrap no-footer">
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <table id="datatable_error_time" class="table table-striped table-bordered dataTable no-footer" style="margin: 0px 0px -10px 0px;" role="grid" aria-describedby="datatable_error_time_info">
                                                        <thead>
                                                            <tr style="background-color:dimgray;color: white;" role="row">
                                                                <th class="sorting" tabindex="0" aria-controls="datatable_error_time" rowspan="1" colspan="1" aria-label="設備名稱: activate to sort column ascending" style="width: 122.004px;">設備名稱</th>
                                                                <th class="sorting" tabindex="0" aria-controls="datatable_error_time" rowspan="1" colspan="1" aria-label="異常訊息: activate to sort column ascending" style="width: 261.004px;">異常訊息</th>
                                                                <th class="sorting" tabindex="0" aria-controls="datatable_error_time" rowspan="1" colspan="1" aria-label="起始時間: activate to sort column ascending" style="width: 211.004px;">起始時間</th>
                                                                <th class="sorting_desc" tabindex="0" aria-controls="datatable_error_time" rowspan="1" colspan="1" aria-sort="descending" aria-label="持續時間: activate to sort column ascending" style="width: 107.004px;">持續時間</th>
                                                                <th class="sorting" tabindex="0" aria-controls="datatable_error_time" rowspan="1" colspan="1" aria-label="作業人員: activate to sort column ascending" style="width: 107px;">作業人員</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>

                                                            <tr role="row" class="odd">
                                                                <td>J-KAFO137+</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:37</td>
                                                                <td class="sorting_1">5:53:54</td>
                                                                <td>dek_user1</td>
                                                            </tr>
                                                            <tr role="row" class="even">
                                                                <td>B-瑞峰本體 MV-20</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:37</td>
                                                                <td class="sorting_1">5:53:54</td>
                                                                <td></td>
                                                            </tr>
                                                            <tr role="row" class="odd">
                                                                <td>I-KAFO168</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:37</td>
                                                                <td class="sorting_1">5:53:53</td>
                                                                <td>Michael</td>
                                                            </tr>
                                                            <tr role="row" class="even">
                                                                <td>A-KAFO BMC2616</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:37</td>
                                                                <td class="sorting_1">5:53:53</td>
                                                                <td></td>
                                                            </tr>
                                                            <tr role="row" class="odd">
                                                                <td>H-KAFO B3116</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:37</td>
                                                                <td class="sorting_1">5:53:53</td>
                                                                <td>Michael</td>
                                                            </tr>
                                                            <tr role="row" class="even">
                                                                <td>F-KAFO137</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:37</td>
                                                                <td class="sorting_1">5:53:50</td>
                                                                <td>Michael</td>
                                                            </tr>
                                                            <tr role="row" class="odd">
                                                                <td>H-KAFO B3116</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:39</td>
                                                                <td class="sorting_1">5:53:48</td>
                                                                <td>Michael</td>
                                                            </tr>
                                                            <tr role="row" class="even">
                                                                <td>H-KAFO B3116</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:45</td>
                                                                <td class="sorting_1">5:53:45</td>
                                                                <td></td>
                                                            </tr>
                                                            <tr role="row" class="odd">
                                                                <td>D-瑞峰EV-10</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:46</td>
                                                                <td class="sorting_1">5:53:44</td>
                                                                <td></td>
                                                            </tr>
                                                            <tr role="row" class="even">
                                                                <td>C-瑞峰臥式</td>
                                                                <td>EX-1000-EMG ALARM</td>
                                                                <td>2019/09/19 09:19:48</td>
                                                                <td class="sorting_1">5:53:42</td>
                                                                <td>Kevin</td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-----以下檢索精靈相關--->
                <!-- set Modal -->
                <div class="backdrop"></div>
                <div class="fab child" data-subitem="1" data-toggle="modal" data-target="#exampleModal">
                    <span><i class="fa fa-line-chart"></i></span>
                </div>
                <div class="fab" id="masterfab">
                    <span><i class="fa fa-list-ul"></i></span>
                </div>
                <!--/set Modal-->
                <!-- Modal -->
                <div id="exampleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h4 class="modal-title" id="myModalLabel2"><i class="fa fa-file-text"></i><b>資料檢索精靈</b></h4>
                            </div>
                            <div class="modal-body" style="padding-top:0px;padding-bottom:0px">
                                <div id="testmodal2" style="padding: 0px 20px;">
                                    <div class="form-group" style="margin-bottom:0px">
                                        <h4>
                                <i class="fa fa-caret-down"><b>設備篩選</b></i> <i id="cbx_remind"></i>
                            </h4>
                                        <div class="row" style="padding-bottom:15px">
                                            <div class="btn-group btn-group-justified">
                                                <!--局部更新-->
                                                <script type="text/javascript">
                                                    //<![CDATA[
                                                    Sys.WebForms.PageRequestManager._initialize('ctl00$ContentPlaceHolder1$ScriptManager1', 'form1', ['tctl00$ContentPlaceHolder1$UpdatePanel1', 'ContentPlaceHolder1_UpdatePanel1'], [], [], 90, 'ctl00');
                                                    //]]>
                                                </script>

                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <div id="ContentPlaceHolder1_UpdatePanel1">

                                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                                        <div class="col-md-2 col-sm-2 col-xs-2" style="font-family:Arial">
                                                            <label>類型:</label>
                                                        </div>
                                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                                            <select name="ctl00$ContentPlaceHolder1$DropDownList_MachType" onchange="javascript:setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder1$DropDownList_MachType\',\'\')', 0)" id="ContentPlaceHolder1_DropDownList_MachType">
                                                                <option selected="selected" value="--Select--">--Select--</option>
                                                                <option value="車床一">車床一</option>
                                                                <option value="洗床一">洗床一</option>
                                                                <option value="測試">測試</option>

                                                            </select>
                                                        </div>
                                                        <div class="col-md-2 col-sm-2 col-xs-2" style="font-family:Arial">
                                                            <label>群組:</label>
                                                        </div>
                                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                                            <select name="ctl00$ContentPlaceHolder1$DropDownList_MachGroup" id="ContentPlaceHolder1_DropDownList_MachGroup">

                                                            </select>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                        <h4>
                                <i class="fa fa-caret-down"><b>計算區間快選</b></i> <i id="cbx_remind"></i>
                            </h4>
                                        <div class="row">
                                            <div class="col-xs-12 col-sm-9 col-md-12 col-lg-12" style="padding-bottom:15px">
                                                <div class="btn-group btn-group-justified">
                                                    <a id="ContentPlaceHolder1_LinkButton_day" class="btn btn-default" href="javascript:__doPostBack('ctl00$ContentPlaceHolder1$LinkButton_day','')">日</a>
                                                    <a id="ContentPlaceHolder1_LinkButton_week" class="btn btn-default " href="javascript:__doPostBack('ctl00$ContentPlaceHolder1$LinkButton_week','')">週</a>
                                                    <a id="ContentPlaceHolder1_LinkButton_month" class="btn btn-default " href="javascript:__doPostBack('ctl00$ContentPlaceHolder1$LinkButton_month','')">月</a>
                                                    <a id="ContentPlaceHolder1_LinkButton_season" class="btn btn-default " href="javascript:__doPostBack('ctl00$ContentPlaceHolder1$LinkButton_season','')">季</a>
                                                    <a id="ContentPlaceHolder1_LinkButton_firsthalf" class="btn btn-default " href="javascript:__doPostBack('ctl00$ContentPlaceHolder1$LinkButton_firsthalf','')">上半年</a>
                                                    <a id="ContentPlaceHolder1_LinkButton_lasthalf" class="btn btn-default " href="javascript:__doPostBack('ctl00$ContentPlaceHolder1$LinkButton_lasthalf','')">下半年</a>
                                                    <a id="ContentPlaceHolder1_LinkButton_year" class="btn btn-default " href="javascript:__doPostBack('ctl00$ContentPlaceHolder1$LinkButton_year','')">全年</a>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <a id="ContentPlaceHolder1_LinkButton_ok" class="btn btn-primary antosubmit2" href="javascript:__doPostBack('ctl00$ContentPlaceHolder1$LinkButton_ok','')" style="position: absolute; right: 0;">執行運算</a>
                                        </div>
                                        <br>
                                        <br>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-----以上檢索精靈相關----->
                <!-- /page content -->
                <!-- jQuery -->
                <script src="../../assets/vendors/jquery/dist/jquery.min.js"></script>
                <!-- Bootstrap -->
                <script src="../../assets/vendors/bootstrap/dist/js/bootstrap.min.js"></script>
                <!-- FastClick -->
                <script src="../../assets/vendors/fastclick/lib/fastclick.js"></script>
                <!-- NProgress -->
                <script src="../../assets/vendors/nprogress/nprogress.js"></script>
                <!-- bootstrap-progressbar -->
                <script src="../../assets/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js"></script>
                <!-- iCheck -->
                <script src="../../assets/vendors/iCheck/icheck.min.js"></script>
                <!-- bootstrap-daterangepicker -->
                <script src="../../assets/vendors/moment/min/moment.min.js"></script>
                <script src="../../assets/vendors/bootstrap-daterangepicker/daterangepicker.js"></script>
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
                <script src="../../assets/vendors/jszip/dist/jszip.min.js"></script>
                <script src="../../assets/vendors/pdfmake/build/pdfmake.min.js"></script>
                <script src="../../assets/vendors/pdfmake/build/vfs_fonts.js"></script>
                <script>
                    //===========================================表格===========================================
                    $('#datatable_error_count').dataTable({
                        "aLengthMenu": [
                            [3, 10, 25, 50, 100, -1],
                            [3, 10, 25, 50, 100, "All"]
                        ],
                        "pageLength": 10,
                        "order": [
                            [3, "desc"]
                        ]
                    });
                    $('#datatable_error_time').dataTable({
                        "aLengthMenu": [
                            [3, 10, 25, 50, 100, -1],
                            [3, 10, 25, 50, 100, "All"]
                        ],
                        "pageLength": 10,
                        "order": [
                            [3, "desc"]
                        ]
                    });
                    //===========================================圓餅圖=============================================
                    var _ch = new CanvasJS.Chart('mach_list', {
                        exportEnabled: true,
                        animationEnabled: true,
                        title: {
                            text: " "
                        },
                        legend: {
                            cursor: "pointer"
                        },
                        data: [{
                            type: "pie",
                            toolTipContent: "{name}: <strong>{y}%</strong>",
                            indexLabel: "{name} - {y}%",
                            indexLabelFontSize: 25,
                            dataPoints: [{
                                y: 52,
                                name: 'EMG ALARM'
                            }, {
                                y: 31,
                                name: '巨集説明格式錯誤'
                            }, {
                                y: 17,
                                name: '文件捜尋不到'
                            }, ]
                        }]
                    });
                    _ch.render()

                    var ch = new CanvasJS.Chart('mach_listTime', {
                        exportEnabled: true,
                        animationEnabled: true,
                        title: {
                            text: " "
                        },
                        legend: {
                            cursor: "pointer"
                        },
                        data: [{
                            type: "pie",
                            toolTipContent: "{name}: <strong>{y}%</strong>",
                            indexLabel: "{name} - {y}%",
                                                        indexLabelFontSize: 25,

                            dataPoints: [{
                                y: 92,
                                name: 'EMG ALARM'
                            }, {
                                y: 8,
                                name: '巨集説明格式錯誤'
                            }, {
                                y: 0,
                                name: '文件捜尋不到'
                            }, ]
                        }]
                    });
                    ch.render()
                        //===========================================時間搜尋===========================================     
                    $(function() {
                        $('#ContentPlaceHolder1_txt_time_str,#ContentPlaceHolder1_txt_time_end').daterangepicker({
                            timePicker: true,
                            singleDatePicker: true,
                            autoUpdateInput: false,
                            singleClasses: "picker_3",
                            locale: {
                                cancelLabel: 'Clear',
                                daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
                                monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
                            }
                        });
                        $('#ContentPlaceHolder1_txt_time_str,#ContentPlaceHolder1_txt_time_end').on('apply.daterangepicker', function(ev, picker) {
                            $(this).val(picker.startDate.format('YYYYMMDDhhmm,A')); //時間格式
                        });
                        $('#ContentPlaceHolder1_txt_time_str,#ContentPlaceHolder1_txt_time_end').on('cancel.daterangepicker', function(ev, picker) {
                            $(this).val('');
                        });
                    });
                    //============================================================================================
                </script>

                <!-- /page content -->
                <!-- footer content -->
                <footer>
                    <!--<div class="pull-right">
            德科智能科技 - 德大ERP可視化 by <a href="https://colorlib.com"> dek Vis</a>
          </div>-->
                    <div class="clearfix"></div>
                </footer>
                <!-- /footer content -->
            </div>
        </div>
        <!-- Logout Modal-->
        <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title" id="myModalLabel2">確定要登出 ?</h4>
                    </div>
                    <div class="modal-body">，如果您準備好結束瀏覽系統，請選擇下方 '登出系統'。</div>
                    <div class="modal-footer">
                        <button class="btn btn-secondary" type="button" data-dismiss="modal">取消操作</button>
                        <input type="submit" name="ctl00$Button_logout" value="登出系統" id="Button_logout" class="btn btn-primary">
                    </div>
                </div>
            </div>
        </div>
        <!-- /Logout Modal-->
    </form><span style="position: absolute; left: 0px; top: -20000px; padding: 0px; margin: 0px; border: none; white-space: pre; line-height: normal; font-family: Impact, Charcoal, sans-serif; font-size: 28px; font-weight: normal; display: none;">Mpgyi</span>
    <script>
        $(document).ready(function() {
            $(window).load(function() { //load函数
                $("#loading").hide();
                /*ShowTime();
                open_notice();*/
            });
        });

        function open_notice() {

            $.ajax({
                type: 'POST',
                dataType: 'xml',
                url: "../../webservice/permission.asmx/set_notice_for_adm",
                data: 'key=',
                success: function(xml) {
                    $(xml).find("ROOT").each(function(i) {
                        //alert();
                        var _class = "label label-danger pull-right";
                        var _count = $(this).attr("system_msg").valueOf();
                        if (_count > 0) {
                            document.getElementById("notice_parents").className = _class;
                            document.getElementById("notice_parents").innerText = _count;
                            document.getElementById("notice_child").className = _class;
                            document.getElementById("notice_child").innerText = _count;
                        }
                    });
                },
                error: function(data, errorThrown) {
                    //alert("Fail");
                }
            });
        }

        function footerbuttons(code) {
            switch (code) {
                case "Settings":
                    window.location = '/pages/sys_control/rights_mag.aspx';
                    break;
            }
        }

        function get_now() {
            var NowDate = new Date();
            var yyyy = NowDate.getFullYear();
            var mm = NowDate.getMonth() + 1;
            var dd = NowDate.getDate();
            var hh = NowDate.getHours();
            var mmn = NowDate.getMinutes();
            var ss = NowDate.getSeconds();
            if (mm < 10) {
                mm = '0' + mm;
            }
            if (dd < 10) {
                dd = '0' + dd;
            }
            if (hh < 10) {
                hh = '0' + hh;
            }
            if (mmn < 10) {
                mmn = '0' + mmn;
            }
            if (ss < 10) {
                ss = '0' + ss;
            }

            now = yyyy + "/" + mm + "/" + dd + " " + hh + ":" + mmn + ":" + ss;
            return now;
        }

        function ShowTime() {
            document.getElementById('showbox').innerHTML = get_now();
            setTimeout('ShowTime()', 1000);
        }
        $("#btn_check").click(function() {
            document.getElementById('Button_logout').click();
        });
    </script>

</body>

</html>