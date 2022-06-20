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
        scrollX: true,
        scrollY: '50vh',
        scrollCollapse: true,
        dom: "<'row'<'pull-left'f>'row'<'col-sm-3'>'row'<'col-sm-3'B>'row'<'pull-right'l>>" +
            "<rt>" +
            "<'row'<'pull-left'i>'row'<'col-sm-4'>row'<'col-sm-3'>'row'<'pull-right'p>>",

        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });

}