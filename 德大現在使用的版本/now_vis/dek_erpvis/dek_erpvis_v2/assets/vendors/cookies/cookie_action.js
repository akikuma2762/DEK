//function doCookieSetup(name, value) {
//    var expires = new Date();
//    //有效時間保存 2 天 2*24*60*60*1000
//    expires.setTime(expires.getTime() + 172800000);
//    document.cookie = name + "=" + escape(value) + ";expires=" + expires.toGMTString()
//}

//function getCookie(name) {
//    var arg = escape(name) + "=";
//    var nameLen = arg.length;
//    var cookieLen = document.cookie.length;
//    var i = 0;
//    while (i & lt; cookieLen) {
//        var j = i + nameLen;
//        if (document.cookie.substring(i, j) == arg) return getCookieValueByIndex(j);
//        i = document.cookie.indexOf(" ", i) + 1;
//        if (i == 0) break;
//    }
//    return null;
//}

//function getCookieValueByIndex(startIndex) {
//    var endIndex = document.cookie.indexOf(";", startIndex);
//    if (endIndex == -1) endIndex = document.cookie.length;
//    return unescape(document.cookie.substring(startIndex, endIndex));
//}


//function delCookie(name) {
//    var exp = new Date();
//    exp.setTime(exp.getTime() - 1);
//    var cval = getCookie(name);
//    document.cookie = escape(name) + "=" + cval + "; expires=" + exp.toGMTString();
//}


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
function delete_value(name) {
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}

function return_preaction(name,tablename) {
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


