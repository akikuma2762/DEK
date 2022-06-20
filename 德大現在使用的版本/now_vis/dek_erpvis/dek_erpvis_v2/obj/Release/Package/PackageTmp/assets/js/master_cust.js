// datapicker 
// 引入此javascrtip即可
$(function () {
    $('#ContentPlaceHolder1_txt_time_str,#ContentPlaceHolder1_txt_time_end').daterangepicker({
        singleDatePicker: true,
        autoUpdateInput: false,
        singleClasses: "picker_3",
        locale: {
            cancelLabel: 'Clear',
            daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
            monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
        }
    });
    $('#ContentPlaceHolder1_txt_time_str,#ContentPlaceHolder1_txt_time_end').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('YYYYMMDD'));
    });
    $('#ContentPlaceHolder1_txt_time_str,#ContentPlaceHolder1_txt_time_end').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
    });
});