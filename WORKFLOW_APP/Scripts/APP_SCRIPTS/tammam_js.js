
var ss = "";
var nn = "";
$(document).ready(function () {
    $(document).attr('title', 'التمام اليومي للوحدات');
    $('#page_name').text('  التمام اليومي للوحدات');
    $.fn.extend({
        animateCss: function (animationName) {
            var animationEnd = 'webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend';
            this.addClass('animated ' + animationName).one(animationEnd, function () {
                $(this).removeClass('animated ' + animationName);
            });
            return this;
        }
    });

    var theme = "darkblue";
    //var firm_cod = '1400000000';
    
    $("#fromDateCal").jqxDateTimeInput({ width: '150px', height: '25px', theme: theme, formatString: 'dd/MM/yyyy HH:mm' });
  //  $('#fromDateCal').jqxDateTimeInput('val', "" + (new Date().getFullYear()).toString() + "/" + (Number(new Date().getMonth()) + 1).toString() + "/" + new Date().getDate() + "");
    //$('#fromDateCal').jqxDateTimeInput('setDate', new Date(
    //    (new Date().getFullYear()),
    //    (Number(new Date().getMonth())),
    //    (new Date().getDate()),
    //    (new Date().getHours()),
    //    (new Date().getMinutes()))
    //);





//    $('#fromDateCal').jqxDateTimeInput('setDate', new Date(
//    (new Date().getFullYear()),
//    (9),
//    (1),
//    (new Date().getHours()),
//    (new Date().getMinutes()))
//);
  //  $("#toDateCal").jqxDateTimeInput({ width: '150px', height: '25px', theme: theme });
  
    $("#prev").on("click", function () {
        debugger
        var firm_cod = '1400000000';
        var fromDateObj = $('#fromDateCal').jqxDateTimeInput('getDate');
        var fromDateDay = fromDateObj.getDate();
        var fromDateMonth = Number(fromDateObj.getMonth()) + 1;
        var fromDateYear = fromDateObj.getFullYear();

        var n = parseInt($('#next1').val());
        var t = parseInt($('#prev11').val());
        var x = n - 5;
        var y = n - 1;
        $('#next1').val(y);
        $('#prev11').val(x);
        var datee = $('#fromDateCal').val();
        Bindall_tammam_Chart(theme, datee, firm_cod, x, y);
        //$('#deptsTreeDiv').treeview('collapseAll');
        //$('#deptsTreeDiv').treeview('Clear');

    });

    ShowGraphs(theme);



    $("#next").on("click", function () {

        
        var firm_cod = '1400000000';
        var fromDateObj = $('#fromDateCal').jqxDateTimeInput('getDate');
        var fromDateDay = fromDateObj.getDate();
        var fromDateMonth = Number(fromDateObj.getMonth()) + 1;
        var fromDateYear = fromDateObj.getFullYear();

        var n = parseInt($('#next1').val());
        var t = parseInt($('#prev11').val());
        var x = t + 1;
        var y = t + 6;
        var datee = $('#fromDateCal').val();
        $('#next1').val(y);
        $('#prev11').val(x);
        Bindall_tammam_Chart(theme, datee, firm_cod, x, y);
        //$('#deptsTreeDiv').treeview('collapseAll');
        //$('#deptsTreeDiv').treeview('Clear');

    });
   
});

function ShowGraphs(theme) {
    var firm_cod = '1400000000';
    var fromDateObj = $('#fromDateCal').jqxDateTimeInput('getDate');
    var fromDateDay = fromDateObj.getDate();
    var fromDateMonth = Number(fromDateObj.getMonth()) + 1;
    var fromDateYear = fromDateObj.getFullYear();
    var datee = $('#fromDateCal').val();
    $('#next1').val(1);
    $('#prev11').val(5);
    Bindall_tammam_Chart(theme, datee, firm_cod, 1, 5);
    Bindall_detail_tammam_Chart(theme, datee, firm_cod);
  
    Binddetail_detailChart(theme, datee, firm_cod,-1);
   // BindAccedentsCategoriesChart(theme, fromDateDay, fromDateMonth, fromDateYear, toDateDay, toDateMonth, toDateYear);
  //  BindAccedentsTypesChart(theme, fromDateDay, fromDateMonth, fromDateYear, toDateDay, toDateMonth, toDateYear);
   // BindperiodicAccedentsChart(theme, fromDateDay, fromDateMonth, fromDateYear, toDateDay, toDateMonth, toDateYear);
}

function Bindall_tammam_Chart(theme, datee, firm_cod, from, to1) {
   
    var firm;
   var dataArr = new Array();
   var tasks_arr2_2 = new Array();
   var tasks_arr2_3 = new Array();
   var x = "التمــام اليــومي للوحـدات";
  var y= $('#title_name').val(x)
    $.ajax({
        url: 'TAMAM_UNITS/getall_tammamDataFN',
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        data: JSON.stringify({
            datee:datee,
            firms: firm_cod,
            from: from,
            to: to1
        }),
        success: function (datarecord) {
            debugger
            var data = datarecord
          //  nn = data[0].NAME
            for (var i = 0 ; i < data.length ; i++) {
                dataArr[i] = {
                    name: data[i].NAME,
                    y: parseFloat(data[i].TOTAL),
                    FIRM_CODE: data[i].FIRM_CODE,
                    firm_namee: data[i].FIRM_NAME,

                };
         
                tasks_arr2_2[i] = {
                    name: data[i].NAME,
                    y: parseFloat(data[i].IIN),
                    FIRM_CODE: data[i].FIRM_CODE,
                    firm: data[i].FIRM_CODE,
                };

                tasks_arr2_3[i] = {
                    name: data[i].NAME,
                    y: parseFloat(data[i].OUT),
                    FIRM_CODE: data[i].FIRM_CODE,
                };
            
            }
          
        },
        error: function (response) {
        }
    });

    Highcharts.chart('all_tammam', {
        chart: {
            type: 'column',
           
        },
        title: {
            text: x //'التمــام اليــومي للـوحدات '
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            opposite: true,
            title: {
                text: '  '
            }
        },
        
        legend: {
            enabled: false
        },
        plotOptions: {
            series: {
                cursor: 'pointer',

                point: {
                    events: {
                        click: function () {
                             
                         
                            var fromDateObj = $('#fromDateCal').jqxDateTimeInput('getDate');
                            var fromDateDay = fromDateObj.getDate();
                            var fromDateMonth = Number(fromDateObj.getMonth()) + 1;
                            var fromDateYear = fromDateObj.getFullYear(); 
                            $('#unit_cod').val(this.options.FIRM_CODE);
                            $('#unit_name').val(this.options.name);
                            var datee = $('#fromDateCal').val();
                            //  Bindall_tammam_Chart(theme, fromDateDay, fromDateMonth, fromDateYear, firm_cod);
                            nn = this.options.name;
                            firm_cod = this.options.FIRM_CODE;
                            Bindall_detail_tammam_Chart(theme, datee, this.options.FIRM_CODE);
                            Binddetail_detailChart(theme, datee, firm_cod, -1);
                        }
                    }
                },
                borderWidth: 0,
                dataLabels: {
                    enabled: true
                }
            }
        },
        event: {
            click: function (e) {
                
                alert(e.yAxis[0].value);
               
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:17px">{series.name}</span><br>',
            pointFormat: '<span style="color:{point}">{point.name}</span>: <b>{point.y}</b> <br/>'
        },
        series: [{
            name: 'القوة',
            data: dataArr
        },
   {
       name: 'الموجود',
       data: tasks_arr2_2
   },
   {
       name: 'الخارج',
       data: tasks_arr2_3
   }
        ],
        //series: [{
        //    name: ' الجهة ',
        //    colorByPoint: true,
        //    data: dataArr
        //}]
    });
   
}
function Bindall_detail_tammam_Chart(theme, datee, firm_cod) {
  
    var dataArr = new Array();
    //var tasks_arr2_2 = new Array();
   // var tasks_arr2_3 = new Array();
    $.ajax({
        url: 'TAMAM_UNITS/geta_detail_tammamDataFN_f',
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        data: JSON.stringify({
            datee:datee,
            firms: firm_cod
        }),
        success: function (datarecord) {
            
            var data = datarecord
          
            for (var i = 0 ; i < data.length ; i++) {
                dataArr[i] = {
                    
                    name: data[i].ABSCENCE_CATEGORY,
                    y: parseFloat(data[i].TAMMAM),
                    FIRM_CODE: data[i].FIRM_CODE,
                    FIRM_NAME: data[i].FIRM_NAME,
                   
                    ABSCENCE_CATEGORY_ID: data[i].ABSCENCE_CATEGORY_ID
                };

                //tasks_arr2_2[i] = {
                //    name: data.Table[i].NAME,
                //    y: parseFloat(data.Table[i].IIN),
                //    FIRM_CODE: data.Table[i].FIRM_CODE,
                //};
                //tasks_arr2_3[i] = {
                //    name: data.Table[i].NAME,
                //    y: parseFloat(data.Table[i].OUT),
                //    FIRM_CODE: data.Table[i].FIRM_CODE,
                //};
            }
        },
        error: function (response) {
        }
    });

    Highcharts.chart('all_detail_tammam', {
        chart: {
            type: 'column'
        },
        title: {
            text: " تمام   " + nn
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            opposite: true,
            title: {
                text: ''
            }
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            series: {
                cursor: 'pointer',
                point: {
                    events: {
                        click: function () {
                            
                            
                            // bindUnitAccedents(theme, this.options.FIRM_CODE, this.options.name);
                            ss = this.options.name;
                            Binddetail_detailChart(theme, datee, firm_cod, this.options.ABSCENCE_CATEGORY_ID);
                        
                          //  bindUnits_tammam(theme, $('#unit_cod').val(), $('#unit_name').val());
                        }
                    }
                },
                borderWidth: 0,
                dataLabels: {
                    enabled: true
                }
            }
        },
        event: {
            click: function (e) {
                alert(e.yAxis[0].value);
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:16px">{series.name}</span><br>',
            pointFormat: '<span style="color:{point}">{point.name}</span>: <b>{point.y}</b> <br/>'
        },
        series: [{
            type: 'pie',
            name: 'النوع',
            data: dataArr
        },

        ],
        //series: [{
        //    name: ' الجهة ',
        //    colorByPoint: true,
        //    data: dataArr
        //}]
    });
}

function Binddetail_detailChart(theme, datee, firm_cod, abs_cat) {
    var nan = "";
    var dataArr = new Array();
    var ABS_NAME = '';
    var ABSENCE_TYPE_ID = '';
    $.ajax({
        url: 'TAMAM_UNITS/getall__detail_tammamDataFN',
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        data: JSON.stringify({
            datee:datee,
            firms: firm_cod,
            abs_cat: abs_cat
        }),
        success: function (datarecord) {
            
            var data = datarecord
           //  nan = data[0].NAME;
            for (var i = 0 ; i < data.length ; i++) {
                
                dataArr[i] = {
                    name: data[i].NAME,
                    y: parseFloat(data[i].TAMMAM),
                    FIRM_CODE: data[i].FIRM_CODE,
                   // ABS_NAME: data.Table[i].,
                    ABSENCE_TYPE_ID: data[i].ABSENCE_TYPE_ID,
                   
                };
            }
        },
        error: function (response) {
        }
    });

    Highcharts.chart('all_detail_detail', {
        chart: {
            type: 'column'
            //styledMode: true
        },
        title: {
            text: '  ' + ss
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            opposite: true,
            title: {
                text: '  '
            }
        },

        legend: {
            enabled: false
        },
        tooltip: {
            ///headerFormat: '<span style="font-size:16px">{series.name}</span><br>',
            pointFormat: '<span style="color:{point}">{point.name}</span>: <b>{point.y}</b> <br/>'
        },
        series: [{
            //type: 'pie',
            cursor: 'pointer',
            point: {
                events: {
                    click: function () {
                        //Binddetail_detailChart(theme, fromDateDay, fromDateMonth, fromDateYear, this.options.FIRM_CODE, this.options.ABSCECE_CAT_ID);
                        bld_steps_grd(); 
                        bnd_step_grd(theme, this.options.ABSENCE_TYPE_ID, firm_cod, this.options.name);
                     //  bind_detail_tammam(theme, this.options.ABSENCE_TYPE_ID, this.options.FIRM_CODE, this.options.name);

                    }
                }
            },
            //allowPointSelect: true,
            name: 'نوع التمام',
            data: dataArr,
            //showInLegend: true
        }]
    });
}

function bindUnits_tammam(theme, FIRM_CODE, UNIT_NAME) {
    
    $('#popUpHeaderDiv').text("تمام  : " + UNIT_NAME);
    var fromDateObj = $('#fromDateCal').jqxDateTimeInput('getDate');
   // var toDateObj = $('#toDateCal').jqxDateTimeInput('getDate');

    var fromDateDay = fromDateObj.getDate();
    var fromDateMonth = Number(fromDateObj.getMonth()) + 1;
    var fromDateYear = fromDateObj.getFullYear();

    $("#innerTableDiv").jqxGrid('clear');

    source = {
        datatype: "xml",
        datafields: [
         { name: 'FIRM_CODE' },
         { name: 'NAME' },
         { name: 'E3ARDA' },
         { name: 'MTA2R' },
         { name: 'BDL_RA7A' },
         { name: 'SANYWA' },
         { name: 'MARDYA' },
         { name: 'MST' },
         { name: 'MA2MORA_D' },
         { name: 'MA2MORA_7' },
         { name: 'FERA2A' },
         { name: 'IIN' },
             { name: 'OUT' },
         { name: 'TOTAL' },
        ],
        async: false,
       // record: 'Table',
        url: 'TAMAM_UNITS/bindUnit_tammamFN',
        data: {
            FIRM_CODE: FIRM_CODE,
            fromDateDay: fromDateDay,
            fromDateMonth: fromDateMonth,
            fromDateYear: fromDateYear

        },
    };
    var dataAdapter = new $.jqx.dataAdapter(source, { contentType: 'application/json; charset=utf-8' });

    $("#innerTableDiv").jqxGrid({
        width: '1250px',
        height: '45vh',
        filterable: true,
        rtl: true,
        autoheight: true,
        pageable: true,
        theme: theme,
        pagesize: 15,
        showfilterrow: true,
        columnsheight: 50,
        source: dataAdapter,
        rowsheight: 40,
        columns:
        [
                                       { text: ' اسم الوحدة ', dataField: 'NAME', cellsalign: 'center', align: 'center', width: '18%' },
                            { text: 'عارضة', dataField: 'E3ARDA', cellsalign: 'center', align: 'center', width: '7%' },
                            { text: 'متأخر', dataField: 'MTA2R', cellsalign: 'center', align: 'center', width: '5%' },
                             { text: ' بدل راحة', dataField: 'BDL_RA7A', cellsalign: 'center', align: 'center', width: '7%' },
                            { text: ' سنوية  ', dataField: 'SANYWA', cellsalign: 'center', align: 'center', width: '5%' },
                              { text: 'مرضية', dataField: 'MARDYA', cellsalign: 'center', align: 'center', width: '5%' },
                            { text: ' مست  ', dataField: 'MST', cellsalign: 'center', align: 'center', width: '5%' },
                              { text: ' مأ. داخلية', dataField: 'MA2MORA_D', cellsalign: 'center', align: 'center', width: '7%' },
                            { text: 'مأ. خارجية ', dataField: 'MA2MORA_7', cellsalign: 'center', align: 'center', width: '9%' },
                              { text: ' فرقة  ', dataField: 'FERA2A', cellsalign: 'center', align: 'center', width: '5%' },
                              { text: ' موجود', dataField: 'IIN', cellsalign: 'center', align: 'center', width: '5%' },
                               { text: ' خارج', dataField: 'OUT', cellsalign: 'center', align: 'center', width: '5%' },
                            { text: ' الأجمالي  ', dataField: 'TOTAL', cellsalign: 'center', align: 'center', width: '7%' },

                           {
                               text: 'التمام', datafield: 'Edit', align: 'center', cellsalign: 'center', columntype: 'button', disabled: true, width: '10%', cellsrenderer: function () {
                                   return "التمام";
                               }, buttonclick: function (row) {
                                   var d = $("#innerTableDiv").jqxGrid('getrowdata', row);
                                   window.open("../REPORTS_TAMAM_YWMY/tamam_ywmy_form.aspx?FIRM_CODE=" + d.FIRM_CODE);
                                   
                               }
                           }
        ]
    });

    //enhance grid style
    $('.jqx-grid-cell').css('font-size', '15px');
    $('.jqx-grid-column-header').css('font-size', '20px');
    $('.jqx-grid-column-header').css('font-weight', 'bold');

    setPager('innerTableDiv');
    $('#overlayDiv').show();
    $('#popUpDiv').show();
}

function bind_detail_tammam(theme, ABSENCE_TYPE_ID, FIRM_CODE, CAT_NAME) {
    
    $('#popUpHeaderDiv').text("تصنيف التمام : " + CAT_NAME);
    var fromDateObj = $('#fromDateCal').jqxDateTimeInput('getDate');
  //  var toDateObj = $('#toDateCal').jqxDateTimeInput('getDate');

    var fromDateDay = fromDateObj.getDate();
    var fromDateMonth = Number(fromDateObj.getMonth()) + 1;
    var fromDateYear = fromDateObj.getFullYear();
    var datee2 = $('#fromDateCal').val();
    source = {
        datatype: "json",
        datafields: [
            { name: 'PERSON_CODE' },
            { name: 'PERSON_NAME' },
            { name: 'ABS_NAME' },
            { name: 'ABSENCE_TYPE_ID' },
            { name: 'FIRM_NAME' },       
             { name: 'TO_DATE' },
              { name: 'FROM_DATE' },
               { name: 'SHORT_NAME' }
        ],
        async: false,
       // record: 'Table',
        url: 'TAMAM_UNITS/bindUnit_detail_tammamFN',
        data: {
            ABSCECE_CAT_ID: ABSENCE_TYPE_ID,
            FIRM_CODE:FIRM_CODE,
            datee:datee2

        },
    };
    var dataAdapter = new $.jqx.dataAdapter(source, { contentType: 'application/json; charset=utf-8' });
    
    $("#innerTableDiv").jqxGrid({
        width: '1250px',
        height: '65vh',
        filterable: true,
        rtl: true,
        autoheight: true,
        pageable: true,
        theme: theme,
        pagesize: 15,
        showfilterrow: true,
        columnsheight: 50,
        source: dataAdapter,
        showtoolbar: true,
        rowsheight: 40,
        columns:
        [
            { text: 'الجهة', dataField: 'FIRM_NAME', width: '20%', cellsalign: 'center', align: 'center' },
            { text: 'الرتبة', dataField: 'SHORT_NAME', width: '15%', cellsalign: 'center', align: 'center' },
            { text: 'الاسم', dataField: 'PERSON_NAME', width: '25%', cellsalign: 'center', align: 'center' },
            { text: 'من ', dataField: 'FROM_DATE', width: '20%', cellsalign: 'center', align: 'center' },
            { text: 'الي', dataField: 'TO_DATE', width: '20%', cellsalign: 'center', align: 'center' }
           
        ]
    });

    $('.jqx-grid-cell').css('font-size', '15px');
    $('.jqx-grid-column-header').css('font-size', '20px');
    $('.jqx-grid-column-header').css('font-weight', 'bold');
    
   // setPager('innerTableDiv');
    $('#overlayDiv').show();
    $('#popUpDiv').show();
}


function bld_steps_grd() {
    
   // $('#popUpHeaderDiv').text("تصنيف التمام : " + CAT_NAME);
    gridId = 'innerTableDiv';
    theme = "darkblue";
    headerText = "تمام الخوارج";
    //gridAddUrl = ControllerName + 'Create_Steps';
    $("#innerTableDiv").jqxGrid({
        width: '98%',
        height: 400,
        // pageable: true,
        theme: "darkblue",
        sortable: true,
        rtl: true,
        showaggregates: true,
          showstatusbar: true,
        //  statusbarheight: 50,
        filterable: true,
        showfilterrow: true,
        showtoolbar: true,
        // selectionmode: 'singlerow',
        //source: dataAdapter,
       // rendertoolbar: toolbarfn,
        columns: [
                       { text: 'الجهة', dataField: 'FIRM_NAME', width: '20%', cellsalign: 'center', align: 'center' },
            { text: 'الرتبة', dataField: 'SHORT_NAME', width: '15%', cellsalign: 'center', align: 'center' },
            { text: 'الاسم', dataField: 'PERSON_NAME', width: '25%', cellsalign: 'center', align: 'center' },
            { text: 'من ', dataField: 'FROM_DATE', width: '20%', cellsalign: 'center', align: 'center' },
            { text: 'الي', dataField: 'TO_DATE', width: '20%', cellsalign: 'center', align: 'center' }

        ]

    });
    $("#innerTableDiv").jqxGrid({ enabletooltips: true });
    $('#overlayDiv').show();
    $('#popUpDiv').show();
}
function bnd_step_grd(theme, ABSENCE_TYPE_ID, FIRM_CODE, CAT_NAME) {
    
    $("#innerTableDiv").jqxGrid('clear');
    $('#innerTableDiv').jqxGrid('clearselection');
    $('#popUpHeaderDiv').text("تصنيف التمام : " + CAT_NAME);
    var fromDateObj = $('#fromDateCal').jqxDateTimeInput('getDate');
    //  var toDateObj = $('#toDateCal').jqxDateTimeInput('getDate');

    var fromDateDay = fromDateObj.getDate();
    var fromDateMonth = Number(fromDateObj.getMonth()) + 1;
    var fromDateYear = fromDateObj.getFullYear();
    var datee2 = $('#fromDateCal').val();
    var source = {
        datatype: "json",
        datafields: [
            { name: 'PERSON_CODE' },
            { name: 'PERSON_NAME' },
            { name: 'ABS_NAME' },
            { name: 'ABSENCE_TYPE_ID' },
            { name: 'FIRM_NAME' },
             { name: 'TO_DATE' },
              { name: 'FROM_DATE' },
               { name: 'SHORT_NAME' }

        ],
        async: false,

        url: 'TAMAM_UNITS/bindUnit_detail_tammamFN',
        data: {
            ABSCECE_CAT_ID: ABSENCE_TYPE_ID,
            FIRM_CODE: FIRM_CODE,
           datee:datee2
        }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#innerTableDiv").jqxGrid({ source: dataAdapter });
    $('#overlayDiv').show();
    $('#popUpDiv').show();
}

function bindTypeAccedents(theme, ACCEDENT_TYPE_ID, Type_NAME) {
    $('#popUpHeaderDiv').text("الحوادث من نوع : " + Type_NAME);
    var fromDateObj = $('#fromDateCal').jqxDateTimeInput('getDate');
    var toDateObj = $('#toDateCal').jqxDateTimeInput('getDate');

    var fromDateDay = fromDateObj.getDate();
    var fromDateMonth = Number(fromDateObj.getMonth()) + 1;
    var fromDateYear = fromDateObj.getFullYear();

    var toDateDay = toDateObj.getDate();
    var toDateMonth = Number(toDateObj.getMonth()) + 1;
    var toDateYear = toDateObj.getFullYear();

    source = {
        datatype: "xml",
        datafields: [
            { name: 'CATNAME' },
            { name: 'ACCEDENT_DATE' },
            { name: 'FIN_YEAR' },
            { name: 'PLACE' },
            { name: 'FIRM_NAME' },
            { name: 'TRAINING_PERIOD' }
        ],
        async: false,
        record: 'Table',
        url: 'AccedentsDashBoard.aspx/bindTypeAccedentsFN',
        data: {
            ACCEDENT_TYPE_ID: ACCEDENT_TYPE_ID,
            fromDateDay: fromDateDay,
            fromDateMonth: fromDateMonth,
            fromDateYear: fromDateYear,
            toDateDay: toDateDay,
            toDateMonth: toDateMonth,
            toDateYear: toDateYear
        },
    };
    var dataAdapter = new $.jqx.dataAdapter(source, { contentType: 'application/json; charset=utf-8' });

    $("#innerTableDiv").jqxGrid({
        width: '1200px',
        height: '45vh',
        filterable: true,
        rtl: true,
        autoheight: true,
        pageable: true,
        theme: theme,
        pagesize: 5,
        showfilterrow: true,
        columnsheight: 50,
        source: dataAdapter,
        rowsheight: 40,
        columns:
        [
            { text: 'الجهة', dataField: 'FIRM_NAME', width: '20%', cellsalign: 'center', align: 'center' },
            { text: 'التصنيف', dataField: 'CATNAME', width: '20%', cellsalign: 'center', align: 'center' },
            { text: 'التاريخ', dataField: 'ACCEDENT_DATE', width: '15%', cellsalign: 'center', align: 'center' },
            { text: 'العام التدريبى', dataField: 'FIN_YEAR', width: '15%', cellsalign: 'center', align: 'center' },
            { text: 'المكان', dataField: 'PLACE', width: '15%', cellsalign: 'center', align: 'center' },
            { text: 'الفترة التدريبية', dataField: 'TRAINING_PERIOD', width: '15%', cellsalign: 'center', align: 'center' }
        ]
    });

    $('.jqx-grid-cell').css('font-size', '15px');
    $('.jqx-grid-column-header').css('font-size', '20px');
    $('.jqx-grid-column-header').css('font-weight', 'bold');

    setPager('innerTableDiv');
    $('#overlayDiv').show();
    $('#popUpDiv').show();
}

function bindPeriodicAccedents(theme, PERIOD_NAME, ACCEDENTYEAR, ACCEDENTMONTH) {
    $('#popUpHeaderDiv').text("الحوادث خلال شهر : " + PERIOD_NAME);

    source = {
        datatype: "xml",
        datafields: [
            { name: 'TYPENAME' },
            { name: 'CATNAME' },
            { name: 'ACCEDENT_DATE' },
            { name: 'FIN_YEAR' },
            { name: 'PLACE' },
            { name: 'FIRM_NAME' },
            { name: 'TRAINING_PERIOD' }
        ],
        async: false,
        record: 'Table',
        url: 'AccedentsDashBoard.aspx/bindPeriodicAccedentsFN',
        data: {
            ACCEDENTYEAR: ACCEDENTYEAR,
            ACCEDENTMONTH: ACCEDENTMONTH
        },
    };
    var dataAdapter = new $.jqx.dataAdapter(source, { contentType: 'application/json; charset=utf-8' });

    $("#innerTableDiv").jqxGrid({
        width: '1010px',
        height: '45vh',
        filterable: true,
        rtl: true,
        autoheight: true,
        pageable: true,
        theme: theme,
        pagesize: 5,
        showfilterrow: true,
        columnsheight: 50,
        source: dataAdapter,
        rowsheight: 40,
        columns:
        [
            { text: 'الجهة', dataField: 'FIRM_NAME', width: '20%', cellsalign: 'center', align: 'center' },
            { text: 'النوع', dataField: 'TYPENAME', width: '15%', cellsalign: 'center', align: 'center' },
            { text: 'التصنيف', dataField: 'CATNAME', width: '20%', cellsalign: 'center', align: 'center' },
            { text: 'التاريخ', dataField: 'ACCEDENT_DATE', width: '10%', cellsalign: 'center', align: 'center' },
            { text: 'العام التدريبى', dataField: 'FIN_YEAR', width: '10%', cellsalign: 'center', align: 'center' },
            { text: 'المكان', dataField: 'PLACE', width: '13%', cellsalign: 'center', align: 'center' },
            { text: 'الفترة التدريبية', dataField: 'TRAINING_PERIOD', width: '12%', cellsalign: 'center', align: 'center' }
        ]
    });

    $('.jqx-grid-cell').css('font-size', '15px');
    $('.jqx-grid-column-header').css('font-size', '20px');
    $('.jqx-grid-column-header').css('font-weight', 'bold');
    setPager('innerTableDiv');
    $('#overlayDiv').show();
    $('#popUpDiv').show();
}

function setPager(grdId) {
    var x = "#pager" + grdId;
    $(x).children().children().toArray()[4].innerHTML = " عدد البيانات ";
    $(x).children().children().toArray()[4].style = "font-family:Arial; font-size:16px; font-weight:Bold;float:right;";
    $(x).children().children().toArray()[6].innerHTML = "عرض الصفحة";
    $(x).children().children().toArray()[6].style = "font-family:Arial; font-size:16px; font-weight:Bold;float:right;";
    $(x).children().children().toArray()[2].style = "float: right;font-family: msdcbi;text-align: center;width: 7%;font-size: 16px;font-weight:Bold;";
    $(x).children().children().toArray()[3].style = "float: right;font-family: msdcbi;text-align: center;width: 3%;font-size: 16px;";
    $(x).children().children().children()[4].style = "float: right;font-family: msdcbi;text-align: center;width: 3%;font-size: 16px;";
    var xx = $(x).children().children().toArray()[2].innerHTML.split(' ');
    xx[1] = "من";
    xx[0] = xx[0].split('-').reverse().join('-');
    var txt = xx[0] + " " + xx[1] + " " + xx[2];
    $(x).children().children().toArray()[2].innerHTML = txt;
    $(x).children().children().toArray()[2].style.direction = "rtl";
}