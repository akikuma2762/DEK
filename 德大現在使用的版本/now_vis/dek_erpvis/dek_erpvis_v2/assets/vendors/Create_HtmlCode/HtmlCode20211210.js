/*--------------------------------------------------------------------------------------以下JS代碼--------------------------------------------------------------------------------------------------------*/
//產生表格(含有印出按鈕的那種)
function set_Table(DataTableName) {
    $(DataTableName).DataTable({
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
        scrollCollapse: true,
        dom: "<'top-list'<'pull-left'f>'row'<'pull-middle'B>'row'<'pull-right'l>>" +
            "<rt>" +
            "<'bottom-list'<'pull-left'i>row'<'pull-right'p>>",

        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });

}

//DATATABLE儲存當下分頁資料
function stateSave_Table(DataTableName) {
    $(DataTableName).DataTable({
        destroy: true,
        stateSave: true,
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
        scrollCollapse: true,
        dom: "<'top-list'<'pull-left'f>'row'<'pull-middle'B>'row'<'pull-right'l>>" +
            "<rt>" +
            "<'bottom-list'<'pull-left'i>row'<'pull-right'p>>",

        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });

}
//20220804 適用有總計之頁面,不排序,防止套件亂排序
function set_Table_defaulf_sort(DataTableName) {
    $(DataTableName).DataTable({
        destroy: true,
        order: false,
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
        scrollCollapse: true,
        dom: "<'top-list'<'pull-left'f>'row'<'pull-middle'B>'row'<'pull-right'l>>" +
            "<rt>" +
            "<'bottom-list'<'pull-left'i>row'<'pull-right'p>>",

        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });

}

//產生order total表格
function set_TotalTable(DataTableName) {
    $(DataTableName).DataTable({
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
        searching: false, paging: false, info: false,
        colReorder: true,
        scrollCollapse: true,
        dom: "",
        buttons: [
        ]

    });
}
$(document).ready(function () {
    jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
});


//圖片的顏色配置
CanvasJS.addColorSet("greenShades", [
    "#4656cc",
    "#3d1b41",
    "#c3cdf5",
    "#e43849",
    "#ea601b",
    "#991f42",
    "#f5b025",
    "#84ac52",
    "#5db0c3",
    "#1ea1d1",
    "#18478e",
    "#003c55"]);


//圖片的顏色配置
CanvasJS.addColorSet("colorlist", [
    "#5b59ac",
    "#ff4d4d"]);


//產生直方圖
function set_column(canvas, title, subtitle, xtext, ytext, name, chartdata) {
    var chart = new CanvasJS.Chart(canvas, {
        animationEnabled: true,
        colorSet: "greenShades",
        theme: "light1",
        title: {
            text: title,
            fontFamily: 'NotoSans',
            fontWeight: 'bolder',
            textAlign: "'center",
        },
        subtitles: [{
            text: subtitle,
            fontFamily: 'NotoSans',
            fontWeight: 'bolder',
            textAlign: "'center",
        }],
        axisX: {
            title: xtext,
            labelFontFamily: 'NotoSans',
            fontWeight: 'bolder',
            textAlign: "'center",

            intervalType: "year"
        },
        axisY: [{
            title: ytext,
            labelFontFamily: 'NotoSans',
            fontWeight: 'bolder',
            textAlign: "'center",
            lineThickness: 1,
            lineColor: "#d0d0d0",
            gridColor: "transparent",
        }],
        legend: {
            fontSize: 15,
            cursor: "pointer",
        },
        toolTip: {
            shared: true,
            content: toolTipContent
        },
        data: [{
            type: 'column',
            indexLabelPlacement: "outside", indexLabelBackgroundColor: "white",
            name: name,
            dataPoints: chartdata
        }]
    });
    chart.render();
}

//產生圓餅圖
function set_pie(canvas, title, subtitle, piedata) {
    var chartpie = new CanvasJS.Chart(canvas,
        {
            colorSet: "colorlist",
            animationEnabled: true,
            title: {
                text: title,
                fontFamily: "NotoSans",
                fontWeight: "bolder",
            },
            subtitles: [{
                text: subtitle,
                fontFamily: 'NotoSans',
                fontWeight: 'bolder',
                textAlign: "'center",
            }],
            legend: {
                fontSize: 15,
                cursor: "pointer",
            },
            data: [{
                showInLegend: true,
                type: "pie",
                startAngle: 240,
                indexLabelFontSize: 20,
                toolTipContent: "<b>{name}</b> - #percent%",
                indexLabel: "{name} - #percent%",
                yValueFormatString: "##0''",
                indexLabelFontSize: 18,
                dataPoints: piedata
            }]
        });
    chartpie.render();
}

//產生雙色直方圖
function set_stackedColumn(canvas, title, subtitle, xtext, ytext, name1, date1, name2, data2, x_text, y_text) {
    var chart = new CanvasJS.Chart(canvas, {
        colorSet: "greenShades",
        animationEnabled: true,
        theme: "light2",
        title: {
            text: title,
            fontFamily: "NotoSans",
            fontWeight: "bolder",
        },
        subtitles: [{
            text: subtitle,
            fontFamily: "NotoSans",
            fontWeight: 'bolder',
            textAlign: "'center",
        }],
        axisX: {
            title: xtext,
            intervalType: "year",
            labelAngle: -180,
        },
        axisY: {
            title: ytext,
            lineThickness: 1,
            lineColor: "#d0d0d0",
            gridColor: "transparent",
        },
        legend: {
            fontSize: 15,
            cursor: "pointer",
        },
        toolTip: {
            shared: true,
            content: toolTipContent
        },
        data: [{
            type: 'stackedColumn',
            indexLabelPlacement: "outside", indexLabelBackgroundColor: "white",
            showInLegend: true,
            color: "#ff4d4d",

            name: name1,
            dataPoints: date1
        }, {
            type: 'stackedColumn',
            indexLabelPlacement: "outside", indexLabelBackgroundColor: "white",
            showInLegend: true,
            color: "#5b59ac",

            name: name2,
            dataPoints: data2
        }
        ]
    });
    chart.render();
    if (x_text !== '' && y_text !== '')
        show_qty(chart, canvas, x_text, y_text);
}

//物料領用統計表明細用
function set_manystackColumn(canvas, title, subtitle, datapoint) {
    var chartcol = new CanvasJS.Chart(canvas, {
        colorSet: "greenShades",

        animationEnabled: true,

        title: {
            text: title,
            fontFamily: "NotoSans",
            fontWeight: "bolder",
        },
        subtitles: [{
            text: subtitle,
            fontFamily: 'NotoSans',
            fontWeight: 'bolder',
            textAlign: "'center",
        }],
        axisX: {
            title: '月份',
            intervalType: "year"
        },
        axisY: {
            title: '數量',
            lineThickness: 1,
            lineColor: "#d0d0d0",
            gridColor: "transparent",
        },
        legend: {
            fontSize: 15,
            cursor: "pointer",
        },
        toolTip: {
            shared: true,
            content: toolTipContent
        },
        data: datapoint
    });
    chartcol.render();
}

//顯示總和的ToolTip
function toolTipContent(e) {
    var str = "";
    var total = 0;
    var str2, str3;
    for (var i = 0; i < e.entries.length; i++) {
        var str1 = "<span style= 'color:" + e.entries[i].dataSeries.color + "'> " + e.entries[i].dataSeries.name + "</span>: <strong>" + e.entries[i].dataPoint.y + "</strong><br/>";
        total = e.entries[i].dataPoint.y + total;
        str = str.concat(str1);
    }
    str2 = "<span style = 'color:DodgerBlue;'><strong>" + (e.entries[0].dataPoint.label) + "</strong></span><br/>";
    str3 = "<span style = 'color:Tomato'>Total:</span><strong>" + total + "</strong><br/>";
    return (str2.concat(str)).concat(str3);
}


//防止切換Tab時產生跑版
function loadpage(cookiename, datatablename) {
    $(document).ready(function () {
        if (cookiename !== '' && datatablename !== '') {
            return_preaction(cookiename, datatablename)
        }
        $('#example').DataTable({
            responsive: true
        });
        $('#exampleInTab').DataTable({
            responsive: true
        });
        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            $($.fn.dataTable.tables(true)).DataTable()
                .columns.adjust()
                .responsive.recalc();
        });
    });
}

//返回上一頁時，跑至上一步之頁籤
function return_preaction(name, tablename) {
    jQuery.fn.dataTable.Api.register('page.jumpToData()', function (data, column) {
        var pos = this.column(column, { order: 'current' }).data().indexOf(data);

        if (pos >= 0) {
            var page = Math.floor(pos / this.page.info().length);
            this.page(page).draw(false);
        }
        return this;
    });

    var y = get_value(name);
    var table = $(tablename).DataTable();
    table.page.jumpToData(y, 0);
    delete_value(name);
}

//從COOKIE取得值
function get_value(name) {

    var test = name + '=';
    var x = document.cookie.split(';');
    var y = '';
    for (var i = 0; i < x.length; i++) {
        var item = x[i].trim();
        if (item.indexOf(test) == 0) {
            y = item.substring(test.length, item.length);
        }
    }
    return y;
}

//刪除該COOKIE的值
function delete_value(name) {
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
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

//列印圖片按鈕
function print_img(button_name) {
    document.getElementById(button_name).addEventListener("click", function () {
        chart.exportChart({ format: "png" });
        parent.location.reload();
    });
}

//依據X軸內容改變Y軸內容
function dropdownlist_change(x, y, value) {
    $('#' + y).empty();

    var ddlist = document.getElementById(x);
    //取得當前X的值
    var now_value = ddlist.value;
    var ddl = document.getElementById(y);
    for (i = 0; i < ddlist.options.length; i++) {
        if (ddlist.options[i].value !== now_value) {

            var option = document.createElement("OPTION");
            option.innerHTML = ddlist.options[i].value;
            option.value = ddlist.options[i].value;
            ddl.options.add(option);
        }
    }
    if (value !== '') {
        selectElement(y, value);
    }
}
function selectElement(id, valueToSelect) {
    let element = document.getElementById(id);
    element.value = valueToSelect;
}

//不指定顯示比數->全show
function checkstatus(checkid, textboxid) {
    var checkBox = document.getElementById(checkid);
    var text = document.getElementById(textboxid);
    if (checkBox.checked === true) {
        text.disabled = true;
    } else {
        text.disabled = false;
    }
}

//20220901檢查bootstrap modal的input是否為空
function check_Modal_Input(id) {
    var bool = false;
    $("#"+id +" input").each(function (index, val) {
        //console.log($(this).val(), $(this).parent().children()[0].innerText);
        var value = $(this).val();
        if (value == "" || value == null) {
            var Text = $(this).parent().children()[0].innerText;
            Text = Text.replace(":", "")
            alert(Text + "不得為空!!!");
            bool = true;
            return false;
        }
    })
    return bool;
}
//20220901 清空modal 中已輸入資料
function clearModal(Product) {
    $('body').on('hidden.bs.modal', ".modal", function () {
        $(this).find('input').val("");
        $(this).find('span').text("");
        var select = $(this).find('select');
        $(select).each(function (index, val) {
            $(this).get(0).selectedIndex = 0;
            //清空後回填目前搜索產線
            $(this).each(function () {
                if(Product!=null)
                    $(this).val(Product);
                console.log($(this).val(), $(this).text());
            })
        });
    });
}
//20220901讀取cookkie
function readCookie(name) {
    var obj = {};
    var cookieInfo = "";
    var cookie_name = name + "=";
    var arry = document.cookie.split(';');
    for (var i = 0; i < arry.length; i++) {
        var cookie_value = arry[i];
        while (cookie_value.charAt(0) == ' ') cookie_value = cookie_value.substring(1, cookie_value.length);
        if (cookie_value.indexOf(cookie_name) == 0) {
            cookieInfo= cookie_value.substring(cookie_name.length, cookie_value.length);
            var arry = cookieInfo.split("&");
            for (var i = 0; i < arry.length; i++) {
                var arry2 = arry[i].split("=");
                obj[arry2[0]] = arry2[1];
            }
            return obj;
        } 
    }
    return null;
}
/*--------------------------------------------------------------------------------------以下產生HTML碼----------------------------------------------------------------------------------------------------*/
//產生表格的程式碼(無控制項版本)
function create_tablecode(div_table, tabletitle, datatableid, th, tr) {
    var divname = document.getElementById(div_table);
    divname.innerHTML =
        '<div class="x_panel Div_Shadow">' +
        '<div class="x_content ">' +
        '<div class="x_panel">' +
        '<div id="_FormTitle" class="x_title" style="text-align: center">' +
        '<h1 class="text-center _mdTitle" style="width: 100%">' +
        '<b>' +
        tabletitle +
        '</b>' +
        '</h1>' +
        '<h3 class="text-center _xsTitle" style="width: 100%">' +
        '<b>' +
        tabletitle +
        '</b>' +
        '</h3>' +
        '<div class="clearfix">' +
        '</div>' +
        '</div>' +
        '<table id="' + datatableid + '" class="table  table-ts table-bordered nowrap" border="1" cellspacing="0" width="100%">' +
        '<thead>' +
        '<tr id="tr_row">' +
        th +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        tr +
        '</tbody>' +
        '</table>' +
        '</div>' +
        '</div>' +
        '</div>';
}
function create_TotalTableCode(div_table, tabletitle, datatableid, th, tr) {
    var divname = document.getElementById(div_table);
    divname.innerHTML =
        '<div class="row">' +
        '<div class="col-md-12 col-sm-12 col-xs-12">' +
        '<div class="x_panel Div_Shadow">' +
        '<div class="x_content">' +
        '<div class="x_panel">' +
        '<table id="' + datatableid + '" class="table  table-ts table-bordered nowrap" border="1" cellspacing="0" width="100%">' +
        '<thead>' +
        '<tr>' +
        th +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        tr +
        '</tbody>' +
        '</table>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>';
}
//產生表格的程式碼(無需陰影)
function create_tablecode_noshdrow(div_table, tabletitle, datatableid, th, tr) {
    var divname = document.getElementById(div_table);
    divname.innerHTML =
        '<div class="col-md-12 col-sm-12 col-xs-12"  id="table_information">' +
        '<div class="x_panel ">' +
        '<div class="x_content">' +
        '<div class="x_panel">' +
        '<div id="_FormTitle" class="x_title" style="text-align: center">' +
        '<h1 class="text-center _mdTitle" style="width: 100%">' +
        '<b>' +
        tabletitle +
        '</b>' +
        '</h1>' +
        '<h3 class="text-center _xsTitle" style="width: 100%">' +
        '<b>' +
        tabletitle +
        '</b>' +
        '</h3>' +
        '<div class="clearfix">' +
        '</div>' +
        '</div>' +
        '<table id="' + datatableid + '" class="table  table-ts table-bordered nowrap" border="1" cellspacing="0" width="100%">' +
        '<thead>' +
        '<tr id="tr_row">' +
        th +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        tr +
        '</tbody>' +
        '</table>' +
        '</div>' +
        '</div>' +
        '</div>' +

        '</div>';
}
//產生表格的程式碼(有控制項版本,無需陰影)
function create_code_noshdrow(div_table, tabletitle, datatableid, th, tr) {
    var divname = document.getElementById(div_table);
    divname.innerHTML =
        '<div class="col-md-9 col-sm-12 col-xs-12"  id="table_information">' +

        '<div class="x_content">' +
        '<div class="x_panel">' +
        '<div id="_FormTitle" class="x_title" style="text-align: center">' +
        '<h1 class="text-center _mdTitle" style="width: 100%">' +
        '<b>' +
        tabletitle +
        '</b>' +
        '</h1>' +
        '<h3 class="text-center _xsTitle" style="width: 100%">' +
        '<b>' +
        tabletitle +
        '</b>' +
        '</h3>' +
        '<div class="clearfix">' +
        '</div>' +
        '</div>' +
        '<table id="' + datatableid + '" class="table  table-ts table-bordered nowrap" border="1" cellspacing="0" width="100%">' +
        '<thead>' +
        '<tr id="tr_row">' +
        th +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        tr +
        '</tbody>' +
        '</table>' +
        '</div>' +
        '</div>' +


        '</div>';
}
function create_tablecode_noshdrow_havesubtitle(div_table, tabletitle, datatableid, th, tr, subtitle) {
    var divname = document.getElementById(div_table);
    divname.innerHTML =
        '<div class="col-md-12 col-sm-12 col-xs-12"  id="table_information">' +
        '<div class="x_panel ">' +
        '<div class="x_content">' +
        '<div class="x_panel">' +
        '<div id="_FormTitle" class="x_title" style="text-align: center">' +
        '<h1 class="text-center _mdTitle" style="width: 100%">' +
        '<b>' +
        tabletitle +
        '</b>' +
        '</h1>' +
        '<h3 class="text-center _xsTitle" style="width: 100%">' +
        '<b>' +
        tabletitle +
        '</b>' +
        '</h3>' +
        subtitle +
        '<div class="clearfix">' +
        '</div>' +
        '</div>' +
        '<table id="' + datatableid + '" class="table  table-ts table-bordered nowrap" border="1" cellspacing="0" width="100%">' +
        '<thead>' +
        '<tr id="tr_row">' +
        th +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        tr +
        '</tbody>' +
        '</table>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>';
}
//產生表格的程式碼(有控制項版本)
function create_tablehtmlcode(div_table, tabletitle, datatableid, th, tr) {
    var divname = document.getElementById(div_table);
    divname.innerHTML =
        '<div class="col-md-9 col-sm-12 col-xs-12  padding-left-right-4">' +
        '<div class="x_panel">' +
        '<div class="x_title">' +
        '<h1 class="text-center _mdTitle" style="width: 100%">' +
        '<b>' +
        tabletitle +
        '</b>' +
        '</h1>' +
        '<h3 class="text-center _xsTitle" style="width: 100%">' +
        '<b>' +
        tabletitle +
        '</b>' +
        '</h3>' +
        '<div class="clearfix">' +
        '</div>' +
        '</div>' +
        '<table id="' + datatableid + '" class="table table-ts table-bordered nowrap" cellspacing="0" width="100%">' +
        '<thead>' +
        '<tr id="tr_row">' +
        th +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        tr.replaceAll('~', '\n').replaceAll("^", "'") +
        '</tbody>'
    '</table>' +
        '</div>' +
        '</div>';
}
//產生圖片的程式碼(只有單一一張圖片的)
function create_imgcode(div_img, downid, canvasid) {
    var divname = document.getElementById(div_img);
    divname.innerHTML =
        '<div class="col-md-12 col-sm-12 col-xs-12">' +
        '<div class="dashboard_graph x_panel" >' +
        '<div class="x_content">' +
        '<div class="_safe">' +
        '<button style="display: none" type="button" id="' + downid + '" title="另存成圖片">' +
        '<img src="../../assets/images/download.jpg" style="width: 36.39px; height: 36.39px;">' +
        '</button>' +
        '</div>' +
        '<div class="col-md-12 col-sm-12 col-xs-12 ">' +
        '<div id="' + canvasid + '">' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>';
}
//產生圖片的程式碼(同時需要多張的情況)
function create_imghtmlcode(div_img, downid, canvasid, md_value, class_id) {
    var divname = document.getElementById(div_img);
    divname.innerHTML =
        '<div class="col-md-' + md_value + ' col-sm-12 col-xs-12">' +
        '<div class="x_panel ' + class_id + '">' +
        '<div class="x_content">' +
        '<p class="text-muted font-13 m-b-30">' +
        '</p>' +
        '<div class="col-md-12 col-sm-12 col-xs-12">' +
        '<div class="col-md-12 col-sm-12 col-xs-12">' +
        '<div style="text-align: right; width: 100%; padding: 0;">' +
        '<button style="display: none" type="button" id="' + downid + '" title="另存成圖片">' +
        '<img src="../../assets/images/download.jpg" style="width: 36.39px; height: 36.39px;">' +
        '</button>' +
        '</div>' +
        '<div class="col-md-12 col-sm-12 col-xs-12">' +
        '<div id="' + canvasid + '">' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>';
}
/*--------------------------------------------------------------------------------------以下為異常回復用----------------------------------------------------------------------------------------------------*/
//呈現影音檔，若該影音檔長>寬 → 長800px,寬450px  反之 長450px,寬800px
function show_image(url, type) {
    //計算圖片大小的
    var img_url = url;

    // 创建对象
    var img = new Image();

    // 改变图片的src
    img.src = img_url

    var img_width;
    var img_height;

    // 判断是否有缓存
    if (img.complete) {
        // 打印
        img_width = img.width;
        img_height = img.height;
    } else {
        // 加载完成执行
        img.onload = function () {
            // 打印
            img_width = img.width;
            img_height = img.height;
        }
    }
    //影片
    if (type === 'video') {
        if ($(window).width() < 768) {
            document.getElementById('lbltipAddedComment').innerHTML = ' <video  width="100%" height="auto" src="' + url + '" controls="" href=\"javascript: void()\" ></video>';
        } else {
            if (img_width > img_height) {
                document.getElementById('lbltipAddedComment').innerHTML = ' <video  width="800px" height="450px" src="' + url + '" controls="" href=\"javascript: void()\" ></video>';
            }
            else {
                document.getElementById('lbltipAddedComment').innerHTML = ' <video  width="450px" height="800px" src="' + url + '" controls="" href=\"javascript: void()\" ></video>';
            }
        }
    }//圖片
    else {
        if ($(window).width() < 768) {
            document.getElementById('lbltipAddedComment').innerHTML = ' <img src="' + url + '" alt="..." width="100%" height="auto" >';
        } else {
            if (img_width > img_height) {
                document.getElementById('lbltipAddedComment').innerHTML = ' <img src="' + url + '" alt="..." width="800px" height="450px" >';
            }
            else {
                document.getElementById('lbltipAddedComment').innerHTML = ' <img src="' + url + '" alt="..." width="450px" height="800px" >';
            }
        }
    }
}
//下拉選單改變
function Change_Content() {
    if (document.getElementById("ContentPlaceHolder1_DropDownList_Status").value != "結案") {
        document.getElementById("ContentPlaceHolder1_Case_Close").style.display = "none";
    }
    else {
        document.getElementById("ContentPlaceHolder1_Case_Close").style.display = "block";
    }
}
//建立Checkbox,文字用影音檔取代
function create_item(file, ul_id, type) {

    var ul = document.getElementById(ul_id);
    while (ul.firstChild) ul.removeChild(ul.firstChild);

    var list = file.split("^");
    for (var i = 0; i < list.length - 1; i++) {

        var ul = document.getElementById(ul_id); //ul
        var li = document.createElement('li');//li
        var checkbox = document.createElement('input');
        checkbox.type = "checkbox";
        checkbox.value = list[i];
        checkbox.name = type;
        checkbox.checked = true;
        li.appendChild(checkbox);

        //影片檔用
        if (list[i].split('.')[1] === 'mp4') {
            var video = document.createElement('video');
            video.src = list[i];
            video.setAttribute("controls", "controls");
            video.height = "200";
            video.width = "200";
            li.appendChild(video)
        }//圖片檔用->excel OR word OR PDF OR PPT 無法顯示
        else {
            var img = document.createElement('img');
            img = new Image(200, 200);
            img.src = list[i];
            li.appendChild(img)
        }
        ul.appendChild(li);
    }
}
//把檔案連結印到TextBox內，以便後端存入資料庫
function get_checkboxvalue(checkbox_name, txt_name) {

    var file = [];
    $.each($("input[name='" + checkbox_name + "']:checked"), function () {
        file.push($(this).val());
    });
    $(txt_name).val('' + file.join('^') + '');
}
/*--------------------------------------------------------------------------------------以下為生產推移圖用----------------------------------------------------------------------------------------------------*/
//產生生產推移圖跟生產領料圖的JS
function set_image(LineName, divcolumn, title, expectedvalue, truevalue, printbutton1, divbar, printbutton2) {
    var chart = new CanvasJS.Chart(divcolumn, {
        // exportEnabled: true,
        animationEnabled: true,
        theme: "light1",
        title: {
            display: true,
            text: LineName + '生產領料圖',
            fontFamily: 'NotoSans',
            fontWeight: 'bolder',
            textAlign: "'center",
        }, subtitles: [{
            text: title,
            fontFamily: 'NotoSans',
            fontWeight: 'bolder',
            textAlign: "'center",
            fontSize: 18,
        }],
        axisX: {
            title: '日期',
            interval: 1
        },
        axisY: {
            title: '數量',
            lineThickness: 1,
            lineColor: "#d0d0d0",
            gridColor: "transparent",
        }, axisY2: {
            title: "",
            tickLength: 1,
            lineThickness: 0,
            margin: 0,
            valueFormatString: " ",
        },
        toolTip: {
            shared: true,
            contentFormatter: function (e) {//debug
                var str = "<span>" + e.entries[0].dataPoint.label + "</span><br/>";
                temp = "";
                for (var i = 0; i < e.entries.length; i++) {
                    if (e.entries[i].dataSeries.name != "") {
                        var temp = "<span style='color:" + e.entries[i].dataSeries.color + "'>" + e.entries[i].dataSeries.name + ": </span>" + "<strong>" + e.entries[i].dataPoint.y + "</strong> <br/>";
                        str = str.concat(temp);
                    }
                }
                return (str);
            }
        },
        legend: {
            fontSize: 15,
            cursor: "pointer",
            itemclick: toggleDataSeries,
        },
        data: [{
            type: "column",
            name: "應生產",
            showInLegend: true,
            dataPoints: expectedvalue

        }, {
            type: "column",
            // axisYType: "secondary",
            name: "實生產",
            showInLegend: true,
            dataPoints: truevalue
        },
        ]
    });

    document.getElementById(printbutton1).addEventListener("click", function () {
        chart.exportChart({ format: "png" });
        parent.location.reload();
    });
    // 2019/06/13，更換圖表類型
    function ChangeChartType(Type) {
        for (var i = 0; i < chart.options.data.length; i++) {
            chart.options.data[i].type = Type;
        }
        chart.render();
    }

    //2019/05/20，控制圖例開關
    function toggleDataSeries(e) {
        if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
            e.dataSeries.visible = false;
        }
        else {
            e.dataSeries.visible = true;
        }
        chart.render();
    }


    //生產推移圖

    var chart_bar = new CanvasJS.Chart(divbar, {
        // exportEnabled: true,
        animationEnabled: true,
        theme: "light1",
        title: {
            display: true,
            text: LineName + '生產推移圖',
            fontFamily: 'NotoSans',
            fontWeight: 'bolder',
            textAlign: "'center",
        }, subtitles: [{
            text: title,
            fontFamily: 'NotoSans',
            fontWeight: 'bolder',
            textAlign: "'center",
            fontSize: 18,
        }],
        axisX: {
            title: '日期',
            interval: 1
        },
        axisY: {
            title: '數量',
            lineThickness: 1,
            lineColor: "#d0d0d0",
            gridColor: "transparent",
        }, legend: {
            fontSize: 15,
            cursor: "pointer",
            itemclick: function (e) {
                //console.log("legend click: " + e.dataPointIndex);
                //console.log(e);
                if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                    e.dataSeries.visible = false;
                } else {
                    e.dataSeries.visible = true;
                }

                e.chart.render();
            }
        },
        toolTip: {
            shared: true,
            contentFormatter: function (e) {//debug
                var str = "<span>" + e.entries[0].dataPoint.label + "</span><br/>";
                temp = "";
                //tooptip顯示
                for (var i = 0; i < e.entries.length; i++) {
                    var temp = "<span style='color:" + e.entries[i].dataSeries.color + "'>" + e.entries[i].dataSeries.name + ": </span>"
                    temp += "<strong>" + e.entries[i].dataPoint.y + "</strong> <br/>";
                    str = str.concat(temp);
                    temp = e.entries[i].dataPoint.y;
                }


                //2019/06/05 10:33 pm....... john-------
                var tar = 0, tar = 0, ans = 0, e_length = e.entries.length;
                now = (e.entries[e.entries.length - 1].dataPoint.y);//決定第一條線要跟哪條線相減
                tar = (e.entries[e.entries.length - e.entries.length].dataPoint.y);
                if (e_length == 1) {//如果等於你選的那條線

                    var max = chart_bar.data[chart_bar.options.data.length - 1].dataPoints.length - 1;
                    var color = chart_bar.data[chart_bar.options.data.length - 1].color;
                    var name = chart_bar.data[chart_bar.options.data.length - 1].name;
                    now = chart_bar.data[chart_bar.options.data.length - 1].dataPoints[max].y;
                    str += "<strong style='color:" + color + ";'>" + name + " : </strong><strong>" + now + "</strong> <br/>";
                }

                ans = (now) - (tar);
                str += "<strong style='color:tomato;'>差值 : </strong><strong>" + ans + "</strong> <br/>";
                return (str);
            }
        },

        data: []
    });
    document.getElementById(printbutton2).addEventListener("click", function () {
        chart_bar.exportChart({ format: "png" });
        parent.location.reload();
    });
    function createPareto_1() {//debug
        var dps = [];
        var yValue = 0;
        for (var i = 0; i < chart.data[0].dataPoints.length; i++) {
            yValue += chart.data[0].dataPoints[i].y;

            if (i == (chart.data[0].dataPoints.length - 1)) {
                dps.push({
                    lineColor: "blue",
                    label: chart.data[0].dataPoints[i].label,
                    y: yValue,
                    //indexLabel: ""//"生產目標:" + yValue
                });

            } else {
                dps.push({
                    lineColor: "blue",
                    label: chart.data[0].dataPoints[i].label,
                    y: yValue
                });
            }
        }
        chart_bar.addTo("data", {
            type: "line",
            name: "累積生產目標",
            markerSize: 0,
            indexLabelFontSize: 20,
            showInLegend: true,
            dataPoints: dps
        });
        count_0 = yValue;
    }


    function createPareto_2() {//debug
        var dps = [];
        var yValue = 0;
        var _markerType = "";
        var _markerColor = "";

        for (var i = 0; i < chart.data[1].dataPoints.length; i++) {
            yValue += chart.data[1].dataPoints[i].y; //實際產量_累加



            if (yValue < chart_bar.data[0].dataPoints[i].y) {//應生產
                _markerType = "cross";
                _markerColor = "tomato";

            } else {
                _markerType = "triangle";
                _markerColor = "#6B8E23";
            }

            if (i == (chart.data[0].dataPoints.length - 1)) {
                dps.push({
                    label: chart.data[1].dataPoints[i].label,
                    y: yValue,
                    //indexLabel: "累積生產量:" + yValue,
                    markerType: _markerType,
                    markerColor: _markerColor
                });
            }
            else {
                dps.push({
                    label: chart.data[1].dataPoints[i].label,
                    y: yValue,
                    markerType: _markerType,
                    markerColor: _markerColor
                });
            }
        }
        chart_bar.addTo("data", {
            type: "line",
            name: "實際生產量",
            markerSize: 10,
            indexLabelFontSize: 20,
            showInLegend: true,
            dataPoints: dps
        });
        count_1 = yValue;
    }

    /* */
    /**/
    chart.render();
    var max = chart.axisY[0].get("maximum");
    chart.options.axisY2.maximum = max;
    chart.render();

    createPareto_1();
    createPareto_2();
    chart_bar.render();
}
/*--------------------------------------------------------------------------------------以下顯示燈號用----------------------------------------------------------------------------------------------------*/
function show_light(div_name) {
    var divname = document.getElementById(div_name);
    divname.innerHTML =
        '<div class="col-md-12 col-sm-12 col-xs-12">' +
        '<div class="col-md-6 col-sm-6 col-xs-6">' +
        '<div class="col-md-3 col-sm-3 col-xs-3">' +
        '<img src="../../assets/images/MOLDING.PNG" alt="..." width="30px" height="30px">' +
        '</div>' +
        '<div class="col-md-9 col-sm-9 col-xs-9">' +
        '<span>' +
        '段取中' +
        '</span> ' +
        '</div>' +
        '</div>' +
        '<div class="col-md-6 col-sm-6 col-xs-6">' +
        '<div class="col-md-3 col-sm-3 col-xs-3">' +
        '<img src="../../assets/images/MOLDED.PNG" alt="..." width="30px" height="30px">' +
        '</div>' +
        '<div class="col-md-9 col-sm-9 col-xs-9">' +
        '<span>' +
        '段取完成' +
        '</span>' +
        '</div>' +
        '</div>' +
        '<br>' +
        '<hr />' +
        '<div class="col-md-6 col-sm-6 col-xs-6">' +
        '<div class="col-md-3 col-sm-3 col-xs-3">' +
        '<img src="../../assets/images/RUN.PNG" alt="..." width="30px" height="30px">' +
        '</div>' +
        '<div class="col-md-9 col-sm-9 col-xs-9">' +
        '<span>' +
        '加工中' +
        '</span>' +
        '</div>' +
        '</div>' +
        '<div class="col-md-6 col-sm-6 col-xs-6">' +
        '<div class="col-md-3 col-sm-3 col-xs-3">' +
        '<img src="../../assets/images/FINISH.PNG" alt="..." width="30px" height="30px">' +
        '</div>' +
        '<div class="col-md-9 col-sm-9 col-xs-9">' +
        '<span>' +
        '加工完成' +
        '</span>' +
        '</div>' +
        '</div>' +
        '<br>' +
        '<hr />' +
        '<div class="col-md-6 col-sm-6 col-xs-6">' +
        '<div class="col-md-3 col-sm-3 col-xs-3">' +
        '<img src="../../assets/images/ERROR.PNG" alt="..." width="30px" height="30px">' +
        '</div>' +
        '<div class="col-md-9 col-sm-9 col-xs-9">' +
        '<span>' +
        '異常' +
        '</span>' +
        '</div>' +
        '</div>' +
        '<div class="col-md-6 col-sm-6 col-xs-6">' +
        '<div class="col-md-3 col-sm-3 col-xs-3">' +
        '<img src="../../assets/images/READY.PNG" alt="..." width="30px" height="30px">' +
        '</div>' +
        '<div class="col-md-9 col-sm-9 col-xs-9">' +
        '<span>' +
        '閒置' +
        '</span>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<br>' +
        '<hr />' +
        '<br>' +
        '<br>' +
        '<br>' +
        '<br>' +
        '<br>';
}
/*--------------------------------------------------------------------------------------以下為workhourList用----------------------------------------------------------------------------------------------------*/
function set_machinelist(div_name, allmach, nowmach, textboxid, event_name) {

    var divname = document.getElementById(div_name);
    var mach_list = allmach.split(',');
    for (i = 0; i < mach_list.length - 1; i++) {
        if (mach_list[i] === nowmach) {
            divname.innerHTML += '<label class="btn btn-default active" onclick="' + event_name + '(\'' + mach_list[i] + '\')" style="margin-bottom:10px">' +
                '<input type="radio" name="' + mach_list[i] + '" id="' + mach_list[i] + '" class="radio" value="' + mach_list[i] + '">' +
                mach_list[i] +
                '</label>';
            $(textboxid).val('' + nowmach + '');
        }
        else {
            divname.innerHTML += '<label class="btn btn-default" onclick="' + event_name + '(\'' + mach_list[i] + '\')" style="margin-bottom:10px">' +
                '<input type="radio" name="' + mach_list[i] + '" id="' + mach_list[i] + '" class="radio" value="' + mach_list[i] + '">' +
                mach_list[i] +
                '</label>';
        }
    }


}
function set_information(div_name, information) {
    var divname = document.getElementById(div_name);
    var data = information.split(',');
    divname.innerHTML += '<div class="col-md-12 col-sm-12 col-xs-12">';
    for (i = 0; i < data.length; i++) {
        divname.innerHTML += '<div class="col-md-12 col-sm-12 col-xs-12">' +
            '<div class="col-md-5 col-sm-4 col-xs-4">' +
            data[i] + ':' +
            '</div>' +
            '<div class="col-md-7 col-sm-8 col-xs-8">' +
            data[i + 1] +
            '</div>' +
            '<div class="col-md-12 col-sm-12 col-xs-12">' +
            ' <hr />' +
            '</div>' +
            '</div>' +
            '<br>';
        i++;
    }
    divname.innerHTML += '</div>'
}
/*--------------------------------------------------------------------------------------以下權限控管用----------------------------------------------------------------------------------------------------*/
function set_dropdownlist(x, y, value) {
    $('#' + y).empty();

    var ddlist = document.getElementById(x);
    //取得當前X的值
    var now_value = ddlist.value;
    var ddl = document.getElementById(y);
    for (i = 0; i < ddlist.options.length; i++) {
        if (ddlist.options[i].value !== value) {
            var option = document.createElement("OPTION");
            option.innerHTML = ddlist.options[i].text;
            option.value = ddlist.options[i].value;
            ddl.options.add(option);
        }
    }
}
/*--------------------------------------------------------------------------------------以下訂單用---------------------------------------------------------------------------------------------------------*/
//訂單用，計算總合高度
function show_qty(chart, div_name, x_text, y_text) {

    var indexLabels = [];
    addIndexLabels(chart);

    function addIndexLabels(chart) {
        for (var i = 0; i < chart.data[0].dataPoints.length; i++) {
            indexLabels.push(document.createElement("p"));
            indexLabels[i].setAttribute("id", "indexlabel");
            indexLabels[i].innerHTML = chart.data[1].dataPoints[i].y + chart.data[0].dataPoints[i].y;
            document.getElementById(div_name).appendChild(indexLabels[i]);
        }
        positionIndexLabels(chart);
    }

    function positionIndexLabels(chart) {
        var x_num = 0;
        var y_num = 0;
        var x_value = document.getElementById(x_text).value;
        var y_value = document.getElementById(y_text).value;

        var max_height = 0;

        //要先抓到目前最高的數值
        for (var i = 0; i < indexLabels.length; i++) {
            var now_value = chart.data[0].dataPoints[i].y + chart.data[1].dataPoints[i].y;

            if (now_value > max_height)
                max_height = now_value;

        }
        var value_length = max_height.toString().length;
        var maxbytes = 1;
        for (i = 0; i < value_length - 1; i++)
            maxbytes *= 10;

        if (x_value === 'PLINE_NO' && y_value === 'QUANTITY') {
            x_num = 0;
            if (maxbytes >= 100)
                y_num = maxbytes * 0.5;
            else
                y_num = maxbytes * 0.15;
        }
        else if (x_value === 'CUST_NO' && y_value === 'QUANTITY') {
            x_num = 0;
            if (maxbytes >= 100)
                y_num = maxbytes * 0.15;
            else
                y_num = maxbytes * 0.15;
        }

        //麻煩的地方
        else if (x_value === 'PLINE_NO' && y_value === 'AMOUNT') {
            x_num = -0.08;
            if (maxbytes >= 100)
                y_num = maxbytes * 0.2;
            else
                y_num = maxbytes * 0.15;
        }

        else if (x_value === 'CUST_NO' && y_value === 'AMOUNT') {
            x_num = -0.18;
            if (maxbytes >= 100)
                y_num = maxbytes * 0.2;
            else
                y_num = maxbytes * 0.15;
        }



        for (var i = 0; i < indexLabels.length; i++) {
            indexLabels[i].style.top = chart.axisY[0].convertValueToPixel(chart.data[0].dataPoints[i].y + chart.data[1].dataPoints[i].y + (y_num)) - (indexLabels[i].clientHeight / 2) + "px";

            indexLabels[i].style.left = chart.axisX[0].convertValueToPixel(chart.data[0].dataPoints[i].x + (x_num)) + "px";
        }
    }

    window.onresize = function (event) {
        positionIndexLabels(chart);
    }
}
/*--------------------------------------------------------------------------------------以下訂單用---------------------------------------------------------------------------------------------------------*/
//全選用
function all_check(checkid) {
    $('#' + checkid + ' input').change(function () {
        var check = $(this).context.checked;
        var val = $(this).val();
        if (val === "" && check === true) {
            seletedAllItem(true);
        } else if (val === "" && check === false) {
            seletedAllItem(false);
        } else if (val !== "" && check === false) {
            if ($(this)[0].id.split("_")[3] != 0) {
                var x = document.getElementById(checkid + "_0");
                x.checked = false;
            }
        }
    });
    function seletedAllItem(seleted) {
        $('#' + checkid + ' input').each(function () {
            $(this).context.checked = seleted;
        });
    }

}