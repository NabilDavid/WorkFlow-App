var ControllerName = ApplicationName + '/Talab_Clinic_Sh/';
//var firms = '1402102001';
var firms;
var person_rank_cat;
var RANK_CAT_ID;
var PERSON_CODE;
var period_id;
var fin_year;
var ABSENCE_TYPE_ID = 11;
var from_date;
var FromTime;
var ToTime;
var FullDateFrom;
var FullDateTo;

$(document).ready(function () {
    $('#page_name').text('طلبات عياده');

    get_person_data();
    seet_fin_year();
    firms = firm_cod;

    var a = Date.now();
    // var persid = "29205251203156";
    $('#TalabData').jqxDateTimeInput({ animationType: 'fade', rtl: true, dropDownHorizontalAlignment: 'right', theme: "darkblue" });
    $('#TalabData ').jqxDateTimeInput('setMinDate', new Date(a));
    //$('#TalabTime').jqxDateTimeInput({ animationType: 'fade', rtl: true, theme: "darkblue", formatString: 'hh:mm' });
    $("#TalabTimeFrom").jqxDateTimeInput({ formatString: 'HH:mm' , textAlign: "center" ,showCalendarButton: false, theme: "darkblue" });
    $("#TalabTimeTo").jqxDateTimeInput({formatString: 'HH:mm',textAlign: "center" , showCalendarButton: false, theme: "darkblue" });

     from_date = $('#TalabData').val();
     FromTime = $("#TalabTimeFrom").val();
     ToTime = $("#TalabTimeTo").val();

     FullDateFrom = from_date + " " + FromTime
     FullDateTo = from_date + " " + ToTime
    //alert(FullDateTo);

    build_grid_clinic();
    bindClinicGrid($('#TalabData').val(), firms);

    $('#TalabData').on('change', function (event) {

        from_date = $('#TalabData').val();
        bindClinicGrid(from_date, firms);// keyboard, mouse or null depending on how the date was selected.
        bnd_ROLE_grid(firms, fin_year, period_id, PERSON_CODE, RANK_CAT_ID, person_rank_cat, ABSENCE_TYPE_ID);

    });

    build_ROLE_grid();

});

// build and bind grid of ClinicOrder
function build_grid_clinic() {

    var cellsrenderer = function (row, columnfield, value, defualthtml, columnproperites) {
        return '<div style="margin :4px; font-size:16px;float:right;text-align: center ;min-width:100%">' + value + '</div>';
    }


    var cellsrendererReport = function (row, columnfield, value, defualthtml, columnproperites) {
        //return '<div style="width:100%;text-align:center;"><button id="showReport" class="btn btn-primary form-control">عـرض الطلب</button></div>';
        return '<div style="width:100%;text-align:center;"><i onclick="show_Report_Sh()" class="fa fa-file-pdf-o" style="padding:6px;font-size:30px;cursor:pointer;color:#4be92ce3;"></i></div>';
    }

    var cellsrendererdel = function (row, columnfield, value, defualthtml, columnproperites) {
        return '<div style="width:100%;text-align:center;"><i onclick="DEL_clinic_order()" class="fa fa-trash" style="padding:6px;font-size:30px;cursor:pointer;color:red;"></i></div>';
    }

    $("#TalabClinicGrid").jqxGrid(
    {
        width: '80%',
        showfilterrow: true,
        height: 200,
        filterable: true,
        rtl: true,
        //pageable: true,
        selectedrowindex: 0,
        //pageSize: 100,
        theme: "darkblue",
        rowsheight: 40,
        filterrowheight: 40,
        columnsresize: true,
        columns: [
    { text: 'الرتبــة', dataField: 'RANK', cellsalign: 'center', align: 'center', width: '15%' },
    { text: 'الإســـــــــم', dataField: 'PERSON_NAME', cellsalign: 'center', align: 'center', width: '35%' },
    { text: 'PERSON_CODE', dataField: 'PERSON_CODE', cellsalign: 'center', align: 'center', width: '50%', hidden: true },
    { text: 'RANK_CAT_ID', dataField: 'RANK_CAT_ID', cellsalign: 'center', align: 'center', width: '50%', hidden: true },
    { text: 'PERSON_CAT_ID', dataField: 'PERSON_CAT_ID', cellsalign: 'center', align: 'center', width: '50%', hidden: true },


    { text: 'من تاريخ', dataField: 'FROM_DATE', cellsalign: 'center', align: 'center', width: '15%', filtertype: 'range' },
    { text: 'إلى تاريخ', dataField: 'TO_DATE', cellsalign: 'center', align: 'center', width: '15%',  filtertype: 'range' },

    { text: 'عرض الطلب', datafield: 'ShowReport', disabled: true, width: '10%', cellsrenderer: cellsrendererReport, cellsalign: 'center', align: 'center' },
    { text: 'حذف', datafield: 'Delete', disabled: true, width: '10%', cellsrenderer: cellsrendererdel, cellsalign: 'center', align: 'center' }
        ]
    });

    $("#TalabClinicGrid").on("rowselect", function (event) {
        
        var args = event.args;
        var row = args.rowindex;
        var data = $('#TalabClinicGrid').jqxGrid('getrowdata', row);

        PERSON_CODE = data.PERSON_CODE;
        RANK_CAT_ID = data.RANK_CAT_ID;
        person_rank_cat = data.PERSON_CAT_ID;
        FullDateFrom = data.FROM_DATE;
        FullDateTo = data.TO_DATE;

        bnd_ROLE_grid(firms, fin_year, period_id, PERSON_CODE, RANK_CAT_ID, person_rank_cat,FullDateFrom, FullDateTo , ABSENCE_TYPE_ID);
        get_person_data();

    });
    //$('#grid_services').on('rowclick', function (event) {

    //});
}
function bindClinicGrid(fromdate1, FirmCode1) {

    $('#TalabClinicGrid').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
                      { name: 'RANK_ID' },
                         { name: 'RANK' },
                         { name: 'PERSON_NAME' },
                         { name: 'FIRM_CODE' },
                         { name: 'FROM_DATE' },
                         { name: 'TO_DATE' },
                         { name: 'ABSENCE_NOTES' },
                         { name: 'ID_NO' },
                         { name: 'PERSON_CODE' },
                         { name: 'COMMANDER_FLAG' },
                         { name: 'FIN_YEAR' },
                         { name: 'TRAINING_PERIOD_ID' },
                         { name: 'RANK_CAT_ID' },
                         { name: 'REQUEST_DATE' },
                         { name: 'ABSENCE_STATUS' },
                         { name: 'ABSENCE_TYPE_ID' },
                         { name: 'CATEGORY_ID' },
                         { name: 'SORT_NO' },
                         { name: 'PERSON_CAT_ID' },
                         { name: 'CURRENT_RANK_DATE' },
                         { name: 'FORCE_DELETE_DATE' },
                         { name: 'ESCAPE_ORDER_NO' },
                         { name: 'RETURN_ORDER_NO' },
                         { name: 'DAY_STATUS' }

        ],
        async: false,

        url: 'Talab_Clinic_Sh/bind_data_clinic',
        data: {
            fromdate: fromdate1,
            FirmCode: FirmCode1
        }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#TalabClinicGrid").jqxGrid({ source: dataAdapter });
}

// show report of clinicOrder
function show_Report_Sh() {

    debugger
    var rowindex = $("#TalabClinicGrid").jqxGrid("getselectedrowindex");
    var rowdata = $("#TalabClinicGrid").jqxGrid("getrowdata", rowindex);

    var rank_off = rowdata.RANK;
    var person_name = rowdata.PERSON_NAME;
    var PERSON_CO = rowdata.PERSON_CODE ;

    FullDateFrom = rowdata.FROM_DATE;
    var splt = FullDateFrom.split(" ")
    var from_date = splt[1];
    var from_Time = splt[0];


    FullDateTo = rowdata.TO_DATE;
    var splt1 = FullDateTo.split(" ")
    var To_Time = splt1[0];

    var url = "REPORTS/Rep_Clinic_Sh/Report_Clinic_Sh.aspx?rank_off=" + rank_off + "&person_name=" + person_name + "&from_date=" + from_date + "&from_Time=" + from_Time + "&To_Time=" + To_Time + "&PERSON_CODE=" + PERSON_CO;
    window.open(url, "_blank");

    //$.ajax({
    //    url: "../REPORTS/Rep_Clinic/Report_Clinic.aspx/Set_Parameter",
    //    data:{
    //    "rank_off1": rank_off ,
    //    "person_name1": person_name,
    //    "from_date1": from_date,
    //    "from_Time1": from_Time,
    //     "To_Time1": To_Time },
    //    contentType: "charset=utf-8",

    //    type: "POST",
    //    success: function (data) {

    //    },
    //    error: function (response) {

    //    }
    //});


}
//********************************************************************************
//Build and Bind Grid of Popub
function bld_off_all_grd() {
    theme = "darkblue";
    headerText = "ضباط المجموعات";
    gridAddUrl = "Talab_Clinic_Sh/" + 'Create_Officers';
    $("#off_grid").jqxGrid({
        width: 500,
        height: 300,
        // pageable: true,
        theme: "darkblue",
        sortable: true,
        rtl: true,
        showaggregates: true,
        //  showstatusbar: true,
        //  statusbarheight: 50,
        filterable: true,
        showfilterrow: true,
        showtoolbar: true,
        // selectionmode: 'singlerow',
        //source: dataAdapter,
        rendertoolbar: toolbarfn,
        columns: [
            { text: 'كود المرحلة', dataField: 'PERSON_CODE', width: '10%', cellsalign: 'center', align: 'center', hidden: true },
            { text: 'كود الم', dataField: 'PERSONAL_ID_NO', width: '10%', cellsalign: 'center', align: 'center', hidden: true },
            { text: 'كود ', dataField: 'RANK_CAT_ID', width: '10%', cellsalign: 'center', align: 'center', hidden: true },
            { text: 'كو ', dataField: 'PERSON_CAT_ID', width: '10%', cellsalign: 'center', align: 'center', hidden: true },



            { text: 'كود المجموعه', dataField: 'OFF_ABS_GROUP_ID', width: '10%', cellsalign: 'center', align: 'center', hidden: true },
            //{ text: 'اسم المجموعه', dataField: 'OFF_ABS_GROUP_NAME', width: '25%', cellsalign: 'center', align: 'center' },
            { text: 'الرتبة ', dataField: 'RANK', width: '45%', cellsalign: 'center', align: 'center' },
            { text: ' الاسم', dataField: 'PERSON_NAME', width: '55%', cellsalign: 'center', align: 'center' },
           // { text: ' ترتيب المرحلة ', dataField: 'ORDER_ID', width: '15%', cellsalign: 'center', align: 'center' },
            //{
            //    text: 'تعديل', columntype: 'button', cellsalign: 'center', filterable: false, width: '10%', align: 'center', cellsrenderer: function (row, datafield, value) {
            //        return "<img  style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/edit.png' onclick='open_edit(" + row + ", st_group__steps_gridDiv)'/>";
            //    }
            //},
            //{
            //    text: 'حذف', columntype: 'button', cellsalign: 'center', filterable: false, width: '10%', align: 'center', cellsrenderer: function (row, datafield, value) {
            //        return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15vh' width='25vw' src='images/delete.png' onclick='open_confirm(" + row + ", st_group__steps_gridDiv)'/>";
            //    }
            //}

        ]

    });
    $("#off_grid").jqxGrid({ enabletooltips: true });
}
function bnd_off_all_grd(GR, abs) {

    $('#off_grid').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
                { name: 'PERSON_CODE' },
                { name: 'PERSONAL_ID_NO' },          
                { name: 'RANK_ID' },
                { name: 'RANK_CAT_ID' },
                { name: 'PERSON_NAME' },
                { name: 'RANK' },
                { name: 'PERSON_CAT_ID' },
                { name: 'ADDERESS' },
                { name: 'FIRM_CODE' }
        ],
        async: false,

        url: 'Talab_Clinic_Sh/GET_off_all',
        data: { firms: GR, rank_cat: abs }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#off_grid").jqxGrid({ source: dataAdapter });
}

//********************************************************************************
// build and bind grid roles
function build_ROLE_grid() {

    gridId = 'off_ROLE';

    theme = "darkblue";
    headerText = " الخطوات";
    gridAddUrl = "Talab_Clinic_Sh/" + 'Create';
    $("#off_ROLE").jqxGrid({
        width: '80%',
        height: 200,
        //pageable: true,
        theme: "darkblue",
        sortable: true,
        rtl: true,
        showaggregates: true,
        // showstatusbar: true,    
        // statusbarheight: 50,
        filterable: true,
        showfilterrow: true,
        showtoolbar: true,
        editable: true,
        //   selectionmode: 'singlerow',
        //source: dataAdapter,
        rendertoolbar: toolbarfn,
        columns: [
                                  { text: 'الترتيب', dataField: 'ORDER_ID', cellsalign: 'center', align: 'center', editable: false, width: '5%' },
                       { text: 'اسم الخطوة', dataField: 'OFF_ABS_STEPS_NAME', cellsalign: 'center', editable: false, align: 'center', width: '45%' },
                       //{ text: 'الرتبة', dataField: 'RANK', cellsalign: 'center', align: 'center', width: '15%' },


                             {
                                 text: 'الاسم ', dataField: 'PERSON_NAME', cellsalign: 'center', align: 'center', width: '50%', columntype: 'combobox',
                                 createeditor: function (row, cellvalue, editor, cellText, width, height) {
                                     debugger
                                     var ROL = $('#off_ROLE').jqxGrid('getrowdata', row).ROL;

                                     var source1 = {
                                         datatype: "json",
                                         datafields:
                                             [{ name: 'NM' },
                                             { name: 'PERSON_CODE' }],
                                         async: false,

                                         url: 'Talab_Clinic_Sh/GET_off_role',
                                         data: { firm: firms, rol: ROL }
                                     };
                                     var dataAdapter1 = new $.jqx.dataAdapter(source1, { contentType: 'application/json; charset=utf-8' });
                                     editor.jqxDropDownList({
                                         source: dataAdapter1,
                                         displayMember: "NM",
                                         valueMember: "PERSON_CODE",
                                         width: '30%', height: 28,
                                         placeHolder: "إختر"
                                     });
                                 },
                                 geteditorvalue: function (row, cellvalue, editor, cell) {

                                     var rowdata = $('#off_ROLE').jqxGrid('getrowdata', row);
                                     edit_c = [];
                                     edit_c[0] = editor.text();
                                     edit_c[1] = editor.val()
                                     rowdata["PERSON_NAME"] = editor.text();
                                     return editor.text();
                                 }
                             }

        ]

    });
    $("#off_ROLE").jqxGrid({ enabletooltips: true });


}
function bnd_ROLE_grid(firms, fin_year, period_id, PERSON_CODE, RANK_CAT_ID, person_rank_cat,FullDateFrom , FullDateTo , ABSENCE_TYPE_ID) {

    //var FROM_DATE = $('#TalabData').val();
    

    $('#off_ROLE').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
                   { name: 'OFF_ABS_STEPS_ID' },
                   { name: 'OFF_ABS_GROUP_ID' },
                   { name: 'OFF_ABS_STEPS_NAME' },
                   { name: 'ROL' },
                   { name: 'FIRMS_ABSENCES_PERSONS_DET_ID' },
                   { name: 'ORDER_ID' },
                   { name: 'PERSON_DATA_ID' },
                   { name: 'PERSON_NAME' },
                   { name: 'RANK' }

        ],
        async: false,

        url: 'Talab_Clinic_Sh/retrieveSteps',

        data: {
            PERSON_CODE: PERSON_CODE,
            fin_year: fin_year,
            period_id: period_id,   
            firms: firms,
            ABSENCE_TYPE_ID: ABSENCE_TYPE_ID,
            RANK_CAT_ID: RANK_CAT_ID,
            person_rank_cat: person_rank_cat,
            FullDateFrom: FullDateFrom,
            FullDateTo: FullDateTo


        }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#off_ROLE").jqxGrid({ source: dataAdapter });
}
//********************************************************************************

// select off from dropdown
$("body").on("cellendedit", "#off_ROLE", function (event) {
    debugger
    var column = args.datafield;
    var row = args.rowindex;

    var value = args.value;
    var oldvalue = args.oldvalue;

    if (value != oldvalue) {

        if (column == "PERSON_NAME") {
            var b = ["", edit_c[1], "", ""]
            if (b[1] == "") {
                swal({
                    title: "خطا",
                    text: "لابد من اختيار اسم شخص",
                    type: "error",
                    timer: 2200
                });
            } else {
               // var rowdata = $('#off_ROLE').jqxGrid('getrowdata', row);

                var data = $('#off_ROLE').jqxGrid('getrowdata', event.args.rowindex);
               // var FROM_DATE = $('#TalabData').val();

                var rowindex = $("#TalabClinicGrid").jqxGrid("getselectedrowindex");
                var rowdata = $("#TalabClinicGrid").jqxGrid("getrowdata", rowindex);
                PERSON_CODE = rowdata.PERSON_CODE;
                RANK_CAT_ID = rowdata.RANK_CAT_ID;
                person_rank_cat = rowdata.PERSON_CAT_ID;
                FullDateFrom = rowdata.FROM_DATE;
                FullDateTo = rowdata.TO_DATE;

                data = {
                    FIRMS_ABSENCES_PERSONS_DET_ID: data.FIRMS_ABSENCES_PERSONS_DET_ID,
                    PERSON_CODE: PERSON_CODE,
                    FIN_YEAR: fin_year,
                    TRAINING_PERIOD_ID: period_id,
                    PERSON_DATE_OWEN: b[1],
                    FIRM_CODE: firms,
                    ABSENCE_TYPE_ID: ABSENCE_TYPE_ID,
                    RANK_CAT_ID: RANK_CAT_ID,
                    PERSON_CAT_ID: person_rank_cat,
                    FullDateFrom: FullDateFrom,
                    FullDateTo : FullDateTo,

                    Command: "Update_details"
                }

                $.ajax({
                    url: "Talab_Clinic_Sh/" +data.Command,
                    type: 'POST',
                    data: data,
                    dataType: 'json',
                    success: function (data) {

                        if (data == 1) {
                            swal({
                                title: "تم التعديل",
                                text: "تم  التعديل بنجاح",
                                type: "success",
                                timer: 2200
                            });
                            //var args = event.args;
                            //var row = args.rowindex;
                            //var data = $('#TalabClinicGrid').jqxGrid('getrowdata', row);

                            //PERSON_CODE = data.PERSON_CODE;
                            //RANK_CAT_ID = data.RANK_CAT_ID;
                            //person_rank_cat = data.PERSON_CAT_ID;
                       
                            bnd_ROLE_grid(firms, fin_year, period_id, PERSON_CODE, RANK_CAT_ID, person_rank_cat,FullDateFrom,FullDateTo , ABSENCE_TYPE_ID);
                        }
                        else {
                            swal({
                                title: "خطا",
                                text: "حدث خطأ",
                                type: "error",
                                timer: 2200
                            });
                            bnd_ROLE_grid(firms, fin_year, period_id, PERSON_CODE, RANK_CAT_ID, person_rank_cat, FullDateFrom, FullDateTo, ABSENCE_TYPE_ID);
                            $("#dialog-edit").dialog('close');
                            get_person_data();

                        }
                    },
                    error: function () {
                        //swal({
                        //    title: "خطأ",
                        //    text: "  " + data.message + "",
                        //    type: "error",
                        //    timer: 2200
                        //});
                        // $('#msg').html('<div class="failed">Error! Please try again.</div>');
                        // alert(data.message);
                    }
                });
            }
        }


    }
    else {
        swal({
            title: "خطأ ",
            text: "الطلب موجود بالفعل",
            type: "error",
            timer: 2200
        });
    }

});

//********************************************************************************

//addOrder
$("body").on("click", "#AddOrder", function () {
  
    //get_person_data();
   
    from_date = $('#TalabData').val();
    FromTime = $("#TalabTimeFrom").val();
    ToTime = $("#TalabTimeTo").val();

    // get hours_from(Int)
    var spl = FromTime.split(":");
    var hours_f = parseInt(spl[0]);
    //var hours_f = 24 - parseInt(spl[0]);
    //var minute_f = parseInt(spl[1]);

    // get hours_To(Int)
    var spl2 = ToTime.split(":");
    var hours_t = parseInt(spl2[0]);
    //var minute_t = parseInt(spl2[1]);
    //var hours_t = 24 - parseInt(spl2[0]);

    FullDateFrom = from_date + " " + FromTime
    FullDateTo = from_date + " " + ToTime
    
    //if ((hours_f >= hours_t && (hours_f - hours_t) <= 1) || (hours_f >= hours_t || (hours_f - hours_t) <= 1) || minute_f > minute_t)
    if (hours_f >= hours_t) {
        swal({
            title: "خطأ ",
            text: "التوقيت غير صحيح",
            type: "error",

            timer: 2500

        });
    }else{
        $.ajax({
            url: "Talab_Clinic_Sh/add_rec",
            data: JSON.stringify({
                PERSON_CODE: PERSON_CODE,
                firms: firms,
                fin_year: fin_year,
                period_id: period_id,
                RANK_CAT_ID: RANK_CAT_ID,
                person_rank_cat: person_rank_cat,
                ABSENCE_TYPE_ID: ABSENCE_TYPE_ID,
                FullDateFrom: FullDateFrom,
                FullDateTo: FullDateTo
            }),
            cache: false,
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
            success: function (data) {

                if (data == 1) {
                    swal({
                        title: "تم الإضافة",
                        text: "تم  إضافة الطلب بنجاح",
                        type: "success",
                        timer: 2500

                    });
              
                    bindClinicGrid($('#TalabData').val(), firms);
                    //location.reload();
                }else if((data == 2)) {
                    swal({
                        title: "خطأ ",
                        text: "الضابط لديه تمام فى نفس التوقيت",
                        type: "error",

                        timer: 2500

                    });

                } else {

                    swal({
                        title: "خطأ فى التاريخ",
                        text: "التاريخ الذي ادخلتة اقل من تاريخ اليوم",
                        type: "error",

                        timer: 2500

                    });
                }
            },
            error: function (response) {
                swal({
                    title: "خطاء",
                    text: "الطلب موجود بالفعل",
                    type: "error",
                    timer: 1500

                });
            }
        });

}



});
//***************************************************************************************

//delete clinic order 
function DEL_clinic_order() {
   
    swal({
        title: " هل تريد الحذف ؟",
        text: "لا يمكن الرجوع فى عملية الحذف",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "نعم",
        cancelButtonText: "لا"
    }, function () {
        var rowindex = $("#TalabClinicGrid").jqxGrid("getselectedrowindex");
        var rowdata = $("#TalabClinicGrid").jqxGrid("getrowdata", rowindex);

       // var FROM_DATE = $('#TalabData').val();
        var PERSON_CODE = rowdata.PERSON_CODE;
        var FIN_YEAR = rowdata.FIN_YEAR;
        var TRAINING_PERIOD_ID = rowdata.TRAINING_PERIOD_ID;
        var FIRM_CODE = rowdata.FIRM_CODE;
        var ABSENCE_TYPE_ID = rowdata.ABSENCE_TYPE_ID;
        var RANK_CAT_ID = rowdata.RANK_CAT_ID;
        var PERSON_CAT_ID = rowdata.PERSON_CAT_ID;
        var FullDateFrom = rowdata.FROM_DATE;
        var FullDateTo = rowdata.TO_DATE;
        // delete detail
        DEL_clinic_order_Det();

        $.ajax({
            url: "Talab_Clinic_Sh/Dele_Clinic_Order",
            data: JSON.stringify({
                PERSON_CODE: PERSON_CODE,
                FIN_YEAR: FIN_YEAR,
                TRAINING_PERIOD_ID: TRAINING_PERIOD_ID,
                FIRM_CODE: FIRM_CODE,
                ABSENCE_TYPE_ID: ABSENCE_TYPE_ID,
                RANK_CAT_ID: RANK_CAT_ID,
                PERSON_CAT_ID: PERSON_CAT_ID,
                FullDateFrom: FullDateFrom,
                FullDateTo: FullDateTo


            }),
            cache: false,
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
            success: function (data) {
                if (data == 1) {
                    swal({
                        title: "تم الحذف",
                        text: "تم  حذف الطلب",
                        type: "success",
                        timer: 2500

                    });
                    bindClinicGrid($('#TalabData').val(), firms);
                }
            },
            error: function (response) {
                swal({
                    title: "لم يتم ",
                    text: "لم يتم حذف الطلب",
                    type: "error",
                    timer: 2500

                });
            }
        });
    });

}

//delete clinic order details 
function DEL_clinic_order_Det() {

    var rowindex = $("#TalabClinicGrid").jqxGrid("getselectedrowindex");
    var rowdata = $("#TalabClinicGrid").jqxGrid("getrowdata", rowindex);

   // var FROM_DATE = $('#TalabData').val();

    var PERSON_CODE = rowdata.PERSON_CODE;
    var FIN_YEAR = rowdata.FIN_YEAR;
    var TRAINING_PERIOD_ID = rowdata.TRAINING_PERIOD_ID;
    var FIRM_CODE = rowdata.FIRM_CODE;
    var ABSENCE_TYPE_ID = rowdata.ABSENCE_TYPE_ID;
    var RANK_CAT_ID = rowdata.RANK_CAT_ID;
    var PERSON_CAT_ID = rowdata.PERSON_CAT_ID;
    var FullDateFrom = rowdata.FROM_DATE;
    var FullDateTo = rowdata.TO_DATE;


    $.ajax({
        url: "Talab_Clinic_Sh/Dele_Clinic_Order_Det",
        data: JSON.stringify({
            PERSON_CODE: PERSON_CODE,
            FIN_YEAR: FIN_YEAR,
            TRAINING_PERIOD_ID: TRAINING_PERIOD_ID,
            FIRM_CODE: FIRM_CODE,
            ABSENCE_TYPE_ID: ABSENCE_TYPE_ID,
            RANK_CAT_ID: RANK_CAT_ID,
            PERSON_CAT_ID: PERSON_CAT_ID,
            FullDateFrom: FullDateFrom,
            FullDateTo: FullDateTo


        }),
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (data) {
            if (data == 1) {
                swal({
                    title: "تم الحذف",
                    text: "تم  حذف الطلب",
                    type: "success",
                    timer: 2500

                });
                bnd_ROLE_grid(firms, fin_year, period_id, PERSON_CODE, RANK_CAT_ID, person_rank_cat, FullDateFrom, FullDateTo, ABSENCE_TYPE_ID);
                // bindClinicGrid($('#TalabData').val(), firms, pers_cod);
            }
        },
        error: function (response) {
            swal({
                title: "لم يتم ",
                text: "لم يتم حذف الطلب",
                type: "error",
                timer: 2500

            });
        }
    });



}

//**********************************************************

//set persCode 
function set_ddl(id1, frm) {

    var pid = id1;
    //getQS('nn', window.location.href);

    $.ajax({
        url: "Talab_Clinic_Sh/GET_JOP",
        data: { ID: pid, FIRM: frm },


        dataType: "json",

        success: function (reslult) {

            var FormList = reslult;
            if (FormList != "") {

                var dt = FormList.split('/');

                //$("#TXT_PLANNING_DECISION").jqxDropDownList({ disabled: true });
                //$("#TXT_COMANDER_DECESION").jqxDropDownList({ disabled: true });
                $("#user_name").text(dt[1] + ' / ' + dt[2]);
                $("#user_name").show();
                $("#user_name").val(pid);
                //per_idd = dt[0];
                //rank_id_cat = dt[5];
                person_rank_cat = dt[3];
                RANK_CAT_ID = dt[5];

                PERSON_CODE = dt[0];

                //$("#PERSON_CAT_ID").val(dt[3]);
                if (dt[9] == "1") {
                    $('#mang_desc').show();

                    //document.getElementById('DATE').style.display = 'none';
                    //document.getElementById('btn_enter_officers').style.display = 'none';
                    //document.getElementById('DATE_mgr').style.display = 'block';
                    //document.getElementById('DATE_plan').style.display = 'none';
                    //document.getElementById('vac_req').style.display = 'none';


                }
                else if (dt[9] == "2") {
                    $('#vcm_desc').show();

                }


                else {
                    $('#mang_desc').hide();
                    $('btn_enter_officers').show();

                }
                if (dt[8] != "0") {

                    //$('#off_desc').hide();
                    //document.getElementById('btn_choose_off').style.display = 'none';
                    //document.getElementById('btn_tw2etat').style.display = 'none';
                    //document.getElementById('btn_refresh').style.display = 'none';
                    //document.getElementById('Div1').style.display = 'none';
                    //document.getElementById('btn_show_rep').style.display = 'inline-block';
                    //document.getElementById('FROM_DATE').style.display = 'none';
                    //document.getElementById('dt_tamam_dobat').style.display = 'none';
                    //document.getElementById('lab').style.display = 'none';
                    // $('btn_enter_officers').hide();
                }
                else {

                    //$('#off_desc').show();
                    ////  $('btn_enter_officers')
                    //document.getElementById('btn_choose_off').style.display = 'inline-block';
                    //document.getElementById('btn_tw2etat').style.display = 'inline-block';
                    //document.getElementById('btn_refresh').style.display = 'inline-block';
                    //document.getElementById('Div1').style.display = 'inline-block';
                    //document.getElementById('btn_show_rep').style.display = 'inline-block';
                    //document.getElementById('FROM_DATE').style.display = 'none';
                    //document.getElementById('dt_tamam_dobat').style.display = 'none';
                    //document.getElementById('lab').style.display = 'none';


                }

                var data = dt[7].split(',');
                for (var i = 0; i < data.length; i++) {

                    if (data[i] == 'pln') {
                        //  $("#TXT_PLANNING_DECISION").jqxDropDownList({ disabled: false });
                    }

                    else if (data[i] == 'cmd') {

                        //$("#TXT_COMANDER_DECESION").jqxDropDownList({ disabled: false });
                    }
                }
            }

        },
        error: function (response) {

        }
    });
}
//**********************************************************

//get fin year
function seet_fin_year() {


    //getQS('nn', window.location.href);

    $.ajax({
        url: "Talab_Clinic_Sh/GET_fin_year",
        // data: { ID: pid, FIRM: frm },


        dataType: "json",

        success: function (reslult) {

            //var FormList = JSON.parse(reslult);
            var FormList = reslult;
            //pers_cod = FormList[0].employeeid;
            if (FormList != "") {

                var dt = FormList;

                period_id = FormList[0].TRAINING_PERIOD_ID;
                fin_year = FormList[0].FIN_YEAR;


                //$("#TXT_FIN_YEAR").val(FormList[0].FIN_YEAR);
                //$("#TXT_PERIOD").val(FormList[0].TRAINING_PERIOD);

                //bnd_mission_grid(firm_cod, FormList[0].FIN_YEAR, period_id, $('#DT_DATE').val(), pers_cod);
                //document.getElementById('dt_firm_missions&add').style.display = 'inline-block';
                // $("#user_name").show();
                //  $("#user_name").val(pid);

                //var data = dt[7].split(',');
                //for (var i = 0; i < data.length; i++) {

                //    if (data[i] == 'pln') {
                //        //  $("#TXT_PLANNING_DECISION").jqxDropDownList({ disabled: false });
                //    }

                //    else if (data[i] == 'cmd') {

                //        //$("#TXT_COMANDER_DECESION").jqxDropDownList({ disabled: false });
                //    }
                //}
            }

        },
        error: function (response) {

        }
    });
}


//getcurrentdate
function getcurrentdate() {
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();
    var year = d.getFullYear();
    if (month < 10)
        month = '0' + month;
    if (day < 10)
        day = '0' + day;
    var fulldate = day + "/" + month + "/" + year;
    return fulldate;
}
