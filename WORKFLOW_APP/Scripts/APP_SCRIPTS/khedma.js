var ApplicationName = "/../WORKFLOW_APP"
var gridAddUrl = '';
var gridId = 'KHDMA_GRD';
var ControllerName = ApplicationName + '/KHADAMAT/';
var tp;
var firms;

$(document).ready(function () {
    get_person_data();
    $('#TY_DD').jqxInput({ width: '80%', height: 38, theme: 'darkblue', disabled: true });
    $('#TP_DD').jqxInput({ width: '100%', height: 38, theme: 'darkblue', disabled: true });
    BLD_DD("FIRM_DD", "../WORKFLOW_APP/Vacation/GET_UNT", 300, 30, "darkblue", 'FIRM_CODE', 'NAME', 0);
    BLD_DD("KHDMA_DD", "../WORKFLOW_APP/KHADAMAT/GET_KHDMA", '100%', 30, "darkblue", 'ID', 'NAME', -1);
    Get_fin_year();
    bld_MON_DD();
    var m = new Date().getMonth() + 1;
    $("#MON_DD").jqxDropDownList('selectItem', m);
    bld_khdma_grd();
    BLD_ROL_GRD('99%', 220);
    //bnd_khdma_grd($("#FIRM_DD").val(), $("#KHDMA_DD").val(), $("#MON_DD").val(), $("#TY_DD").val(), tp);
    $('#KHDMA_DD').on('select', function (event) {
        var item = args.item;
        var value = item.value;
        bnd_khdma_grd($("#FIRM_DD").val(), value, $("#MON_DD").val(), $("#TY_DD").val(), tp);
    });
    firms = firm;
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // show report of khedma
    $("body").on("click", "#KhedmaOff_Btn", function () {
        
        var FIRM = $('#FIRM_DD').val();
        var Typ_Khedma = $('#KHDMA_DD').val();
        var MON = $('#MON_DD').val();
        var monthplus = parseInt(MON) + 1;
        var Typ_Year = $('#TY_DD').val();
        var PROID = tp;

        var url = "REPORTS/Rep_Khedma_Off/Report_Khedma_officer.aspx?FIRM=" + FIRM + "&TYP=" + Typ_Khedma + "&MON=" + MON + "&monthplus=" + monthplus + "&TY=" + Typ_Year + "&PROID=" + PROID;
        window.open(url, "_blank");
    });
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $('#KHDMA_GRD').on('rowselect', function (event) {
        var args = event.args;
        var row = args.rowindex;
        var data = $('#KHDMA_GRD').jqxGrid('getrowdata', row);
        person_c = data.PERSON_CODE + ',' + data.RANK_ID + ',' + data.RANK_CAT_ID + ',' + data.PERSON_CAT_ID + ',' + data.RANK + ',' + data.PERSON_NAME + ',' + data.ADDRESS + ',' + data.PERSONAL_ID_NO;
        var data1 = {
            firm_code: data.FIRM_CODE, person_id: data.PERSON_CODE, ABS_TYP: $('#KHDMA_DD').val(), FDT: data.FROM_DATE, TDT: data.TO_DATE
        };
        BND_ROL_GRD(data1);
    });
    $("#VAC_ROLE").on('cellendedit', function (event) {
        var column = args.datafield;
        var row = args.rowindex;
        var value = args.value;
        var oldvalue = args.oldvalue;
        var rowdata = $('#KHDMA_GRD').jqxGrid('getrowdata', $('#KHDMA_GRD').jqxGrid('selectedrowindex'));
        var data = $('#VAC_ROLE').jqxGrid('getrowdata', event.args.rowindex);
        if (value != oldvalue && value != 'إختر') {

            if (column == "PERSON_NAME") {
                var b = ["", edit_c[1], "", ""];

                data1 = {
                    FIRMS_ABSENCES_PERSONS_DET_ID: data.ID,
                    OFF_ABS_STEPS_ID: data.OFF_ABS_STEPS_ID,
                    OFF_ABS_GROUP_ID: data.OFF_ABS_GROUP_ID,
                    PERSON_CODE: data.PERSON_CODE,
                    per: $("#user_name").val(),
                    PERSON_DATE_OWEN: b[1],
                    step_name: data.OFF_ABS_STEPS_NAME,
                    ABSENCE_TYPE_ID: data.TYPE_ID,
                    FIRM_CODE: data.FIRM_CODE,
                    FROM_DATE: rowdata.FROM_DATE,
                    TO_DATE: rowdata.TO_DATE,
                    FIN_YEAR: $('#TY_DD').val(),
                    TRAINING_PERIOD_ID: tp,
                    RANK_CAT_ID: rowdata.RANK_CAT_ID,
                    PERSON_CAT_ID: rowdata.PERSON_CAT_ID,
                    Command: "add_officer_fun"
                }

                $.ajax({
                    url: ControllerName + data1.Command,
                    type: 'POST',
                    data: data1,
                    dataType: 'json',
                    success: function (data) {
                        if (data.status) {
                            var data1 = {
                                firm_code: rowdata.FIRM_CODE, person_id: rowdata.PERSON_CODE, ABS_TYP: $('#KHDMA_DD').val(), FDT: rowdata.FROM_DATE, TDT: rowdata.TO_DATE
                            };
                            BND_ROL_GRD(data1);
                            swal({
                                title: "تم الاضافة",
                                text: "تم  الاضافة بنجاح",
                                type: "success",
                                timer: 2200
                            });
                            $("#dialog-edit").dialog('close');
                            // alert("saad");
                        }
                        else {
                            swal({
                                title: "خطأ",
                                text: "  " + data.message + "",
                                type: "error",
                                timer: 2200
                            });
                            $("#dialog-edit").dialog('close');
                        }
                    },
                    error: function () {
                        swal({
                            title: "خطأ",
                            text: "  " + data.message + "",
                            type: "error",
                            timer: 2200
                        });
                        // $('#msg').html('<div class="failed">Error! Please try again.</div>');
                        alert(data.message);
                    }
                });
            }




        }
        else {
            //$('#VAC_ROLE').jqxGrid('endupdate');
            //BND_ROL_GRD(firm, person_c.split(',')[7], rowdata.PERSON_VACATIONS_SEQ);
            var txt = (value == 'إختر') ? "يجب إختيار ضابط" : "الضابط موجود بالفعل";
            swal({
                title: "خطأ ",
                text: txt,
                type: "error",
                timer: 2200
            });
            if (value == 'إختر') {
                setTimeout(function () {
                    $("#VAC_ROLE").jqxGrid('setcellvalue', row, "PERSON_NAME", "/");
                }, 500);
            }
            setTimeout(function () {
                BND_ROL_GRD(firm, person_c.split(',')[7], rowdata.PERSON_VACATIONS_SEQ);
            }, 500);

        }

    });

    $("#off_grd").on('cellendedit', function (event) {
        var column = args.datafield;
        var row = args.rowindex;
        var value = args.value;
        var oldvalue = args.oldvalue;
        var data = $('#off_grd').jqxGrid('getrowdata', row);
        if (value != oldvalue && value != '') {
            if (column == "DT") {
                $("#off_grd").jqxGrid('setcellvalue', row, "DT1", data.DT);
            }
        }
    });


});


function display_khdma() {
    bnd_khdma_grd($("#FIRM_DD").val(), $("#KHDMA_DD").val(), $("#MON_DD").val(), $("#TY_DD").val(), tp);
}

function bld_MON_DD() {
    $('#MON_DD').jqxDropDownList({
        source: [{ value: "1", label: "يناير" }, { value: "2", label: "فبراير" }, { value: "3", label: "مارس" }, { value: "4", label: "إبريل" }, { value: "5", label: "مايو" }, { value: "6", label: "يونيو" }, { value: "7", label: "يوليو" }, { value: "8", label: "أغسطس" }, { value: "9", label: "سسبتمبر" }, { value: "10", label: "أكتوبر" }, { value: "11", label: "نوفمبر" }, { value: "12", label: "ديسمبر" }],
        displayMember: "label",
        valueMember: "value",
        width: '100%',
        height: '30',
        selectedIndex: 0,
        theme: 'darkblue',
        rtl: true
    });
}

function Get_fin_year() {
    $.ajax({
        url: "../WORKFLOW_APP/TALAB_M2M/GET_fin_year",
        dataType: "json",
        success: function (reslult) {
            var FormList = reslult;
            if (FormList != "") {
                var dt = FormList;
                $("#TY_DD").val(FormList[0].FIN_YEAR);
                $("#TP_DD").val(FormList[0].TRAINING_PERIOD);
                tp = FormList[0].TRAINING_PERIOD_ID;
                //bnd_mission_grid(firm_cod, FormList[0].FIN_YEAR, period_id, $('#DT_DATE').val(), pers_cod);
            }
        },
        error: function (response) {

        }
    });
}

function bld_KHDMA() {
    $('#MON_DD').jqxDropDownList({
        source: [{ value: "1", label: "يناير" }, { value: "2", label: "فبراير" }, { value: "3", label: "مارس" }, { value: "4", label: "إبريل" }, { value: "5", label: "مايو" }, { value: "6", label: "يونيو" }, { value: "7", label: "يوليو" }, { value: "8", label: "أغسطس" }, { value: "9", label: "سسبتمبر" }, { value: "10", label: "أكتوبر" }, { value: "11", label: "نوفمبر" }, { value: "12", label: "ديسمبر" }],
        displayMember: "label",
        valueMember: "value",
        width: '100%',
        height: '30',
        selectedIndex: 0,
        theme: 'darkblue',
        rtl: true
    });
}

function open_confirm(row, gridId) {

    var D = $('#' + gridId.id).jqxGrid('getrowdata', row);
    if (gridId.id == "KHDMA_GRD") {
        var url = ControllerName + 'Delete';
        var data = { id: D.SEQ, firm: D.FIRM_CODE, pers: D.PERSON_CODE };
        open_del(url, data, gridId.id, "BND_ROL_GRD('" + details.FIRM_CODE + "'," + details.PERSONAL_ID_NO + "," + details.SEQ + ")");

    }
    else if (gridId.id == "st_group__steps_gridDiv") {
        var url = ControllerName + 'Delete_Steps';
        var data = { id: details.OFF_ABS_STEPS_ID, id1: details.OFF_ABS_GROUP_ID };
        openConfirmDialog_parm(url, data, gridId.id, details.OFF_ABS_GROUP_ID);
    }

    else if (gridId.id == "st_group__person_gridDiv") {
        var url = ControllerName + 'Delete_OFF';
        var data = { id: details.OFF_ABS_GROUP_OFF_ID, id1: details.OFF_ABS_GROUP_ID };
        openConfirmDialog_parm(url, data, gridId.id, details.OFF_ABS_GROUP_ID);
    }

}

function bld_khdma_grd() {
    theme = "darkblue";
    disp = "inline-flex, none, inline-flex";
    headerText = "الخدمات";
    gridAddUrl = ControllerName + 'Create';
    w = "0";
    h = 500;
    $("#KHDMA_GRD").jqxGrid(
        {
            width: '99%',
            height: '300',
            pageable: true,
            theme: "darkblue",
            sortable: true,
            rtl: true,
            showaggregates: true,
            showstatusbar: false,
            statusbarheight: 50,
            filterable: true,
            showfilterrow: true,
            showtoolbar: true,
            everpresentrowposition: "top",
            pageSize: 100,
            rowsheight: 35,
            filterrowheight: 35,
            selectionmode: 'singlerow',
            rendertoolbar: toolbarfn1,
            columns: [
                  { text: 'الرتبــة', dataField: 'RANK', cellsalign: 'center', align: 'center', width: '10%' },
                  { text: 'الإســـــــــم', dataField: 'PERSON_NAME', cellsalign: 'center', align: 'center', width: '45%' },
                  { text: 'من', dataField: 'FROM_DATE', cellsalign: 'center', align: 'center', width: '10%' },
                  { text: 'الى', dataField: 'TO_DATE', cellsalign: 'center', align: 'center', width: '10%' },//
                  { text: 'اليوم', dataField: 'DY', cellsalign: 'center', align: 'center', width: '10%' },
                  { text: 'معاملة اليوم', dataField: 'DAY_STAT', cellsalign: 'center', align: 'center', width: '10%' },
                  {
                      text: '', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                          return "<img   style='margin-left: 5px;cursor:pointer' height='17' width='17' src='../WORKFLOW_APP/images/delete.png' onclick='open_confirm(" + row + ", KHDMA_GRD)'/>";
                      }
                  }
            ]
        });
    $('#KHDMA_GRD').jqxGrid('clearselection');
}

function bnd_khdma_grd(FIRM, TYP, MON, TY, PROID) {

    var source = {
        datatype: "json",
        datafield : [
              { name: 'RANK_ID' },
              { name: 'RANK' },
              { name: 'PERSON_NAME' },
              { name: 'FIRM_CODE' },
              { name: 'FROM_DATE' },
              { name: 'DY' },
              { name: 'DAY_STAT' },
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
        url: ControllerName + 'get_khd_grd',
        data: {
            FIRM: FIRM,
            TYP: TYP,
            MON: MON,
            TY: TY,
            PRIOD: PROID
        }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#KHDMA_GRD").jqxGrid({ source: dataAdapter });

}

function KHDMA_GRD_Refresh() {
    bnd_khdma_grd($("#FIRM_DD").val(), $("#KHDMA_DD").val(), $("#MON_DD").val(), $("#TY_DD").val(), tp);
}

function setdt() {

}

function bld_offgrd() {
    theme = "darkblue";
    disp = "none, none, none";
    headerText = "تعيين الخدمات";
    gridAddUrl = ControllerName + 'Create';
    $("#off_grd").jqxGrid(
        {
            width: 1300,
            height: '99%',
            pageable: true,
            theme: "darkblue",
            sortable: true,
            editable: true,
            rtl: true,
            showaggregates: true,
            showstatusbar: false,
            statusbarheight: 50,
            filterable: true,
            showfilterrow: true,
            showtoolbar: true,
            everpresentrowposition: "top",
            pageSize: 100,
            rowsheight: 35,
            filterrowheight: 35,
            selectionmode: 'singlerow',
            //rendertoolbar: toolbarfn1,
            columns: [
                  { text: 'رقم تحقيق الشخصية', dataField: 'ID_NO', cellsalign: 'center', align: 'center', width: '15%', editable: false },
                  { text: 'الرتبــة', dataField: 'RANK', cellsalign: 'center', align: 'center', width: '10%', editable: false },
                  { text: 'الإســـــــــم', dataField: 'PERSON_NAME', cellsalign: 'center', align: 'center', width: '40%', editable: false },
                  {
                      text: 'من', dataField: 'DT', cellsalign: 'center', align: 'center', width: '15%', columntype: 'datetimeinput', editable: true, cellsformat: 'dd/MM/yyyy HH:mm',
                      validation: function (cell, value) {
                          if (value == "")
                              return true;

                          var year = value.getFullYear();
                          if (value < Date.now()) {
                              return { result: false, message: "لا يمكن تعديل خدمة منتهية" };
                          }
                          var dt = value;
                          $("#off_grd").jqxGrid('setcellvalue', cell.row, "DT1", new Date(dt.getFullYear(),dt.getMonth(),dt.getDate()+1,dt.getHours(),dt.getMinutes(),0));
                          return true;
                      }
                  },
                  {
                      text: 'الى', dataField: 'DT1', cellsalign: 'center', align: 'center', width: '15%', columntype: 'datetimeinput', editable: true, cellsformat: 'dd/MM/yyyy HH:mm',
                      validation: function (cell, value) {
                          if (value == "")
                              return true;

                          var year = value.getFullYear();
                          if (value < Date.now()) {
                              return { result: false, message: "لا يمكن تعديل خدمة منتهية" };
                          }
                          return true;
                      }
                  },
                  {
                      text: '', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                          return "<img   style='margin: -3px 0 0 -3px;cursor:pointer' height='25' width='25' src='../WORKFLOW_APP/images/ok.png' onclick='add_khdma(" + row + ");'/>";
                      }
                  }
            ]
        });
    $('#off_grd').jqxGrid('clearselection');
}

function bnd_offgrd(FIRM, CAT) {

    var source = {
        datatype: "json",
        datafield: [
              { name: 'RANK_ID' },
              { name: 'RANK' },
              { name: 'PERSON_CODE' },
              { name: 'PERSON_NAME' },
              { name: 'ID_NO' },
              { name: 'FIRM_CODE' },
              { name: 'NAME' },
              { name: 'CATEGORY_ID' },
              { name: 'DISPLAY_ORDER' },
              { name: 'PERSON_CAT_ID' },
              { name: 'RANK_CAT_ID' },
              { name: 'DT' },
              { name: 'DT1' }
        ],
        async: false,
        url: ControllerName + 'get_khd_off',
        data: {
            FIRM: FIRM,
            CAT: CAT
        }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#off_grd").jqxGrid({ source: dataAdapter });

}

function add_khdma(row) {
    var details = $('#off_grd').jqxGrid('getrowdata', $('#off_grd').jqxGrid('selectedrowindex'));
    var x = details.DT;
    var x1 = details.DT1;
    var dt = x.getDate() + "/" + (x.getMonth() + 1) + "/" + x.getFullYear();
    var dt2 = x1 != null ? x1.getDate() + "/" + (x1.getMonth() + 1) + "/" + x1.getFullYear() + " " + x1.getHours() + ':' + x1.getMinutes() + ':' + x1.getSeconds() :
        (x.getDate() + 1) + "/" + (x.getMonth() + 1) + "/" + x.getFullYear() + " " + x.getHours() + ':' + x.getMinutes() + ':' + x.getSeconds();
    dt = dt + " " + x.getHours() + ':' + x.getMinutes() + ':' + x.getSeconds();
    var url = ControllerName + 'addrec';
    var data = {
        PERSON_CODE: details.PERSON_CODE, FIN_YEAR: $('#TY_DD').val(), TRAINING_PERIOD_ID: tp, FIRM_CODE: details.FIRM_CODE,
        FROM_DATE: dt, ABSENCE_TYPE_ID: $('#KHDMA_DD').val(), RANK_CAT_ID: details.RANK_CAT_ID,
        PERSON_CAT_ID: details.PERSON_CAT_ID, TO_DATE: dt2
    };
    $.ajax({
        url: url,
        type: 'POST',
        data: data,
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                swal({
                    title: data.title,
                    text: data.message,
                    type: data.type,
                    timer: 2200
                });
                bnd_khdma_grd($("#FIRM_DD").val(), $("#KHDMA_DD").val(), $("#MON_DD").val(), $("#TY_DD").val(), tp);
                //$("#dialog-confirm").dialog('close');
            }
            else {
                swal({
                    title: "خطا",
                    text: "  " + data.message + "",
                    type: "error",
                    timer: 2200
                });

            }


        },
        error: function (err) {
            // $('#msg').html('<div class="failed">Error! Please try again.</div>');

            swal({
                title: "خطا ",
                text: "خطأ  ف الحذف",
                type: "error",
                timer: 2200
            });
        }
    });
    
}

function open_confirm(row, gridId) {
    var details = $('#' + gridId.id).jqxGrid('getrowdata', $('#' + gridId.id).jqxGrid('selectedrowindex'));
    var dt = details.FROM_DATE;
    var dt2 = details.TO_DATE;
    var url = ControllerName + 'delrec';
    var data = {
        PERSON_CODE: details.PERSON_CODE, FIN_YEAR: $('#TY_DD').val(), TRAINING_PERIOD_ID: tp, FIRM_CODE: details.FIRM_CODE,
        FROM_DATE: dt, ABSENCE_TYPE_ID: $('#KHDMA_DD').val(), RANK_CAT_ID: details.RANK_CAT_ID,
        PERSON_CAT_ID: details.PERSON_CAT_ID, TO_DATE: dt2
    };
    open_del(url, data, gridId);


}