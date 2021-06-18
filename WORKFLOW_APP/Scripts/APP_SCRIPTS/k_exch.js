var ApplicationName = "/../WORKFLOW_APP"
var gridAddUrl = '';
var gridId = 'KHDMA_GRD';
var ControllerName = ApplicationName + '/KHDMA_CHANG/';
var tp;
var firms;

$(document).ready(function () {
    get_person_data();
    BLD_DD("FIRM_DD", "../WORKFLOW_APP/Vacation/GET_UNT", 300, 30, "darkblue", 'FIRM_CODE', 'NAME', 0);
    BLD_DD("MON_DD", "../WORKFLOW_APP/KHDMA_CHANG/GET_MON", '100%', 30, "darkblue", 'ID', 'NAME', 0);
    BLD_DD("KHDMA_DD", "../WORKFLOW_APP/KHADAMAT/GET_KHDMA", '100%', 30, "darkblue", 'ID', 'NAME', -1);
    //setTimeout(BLD1_DD, 2500, "K_DT1", "../KHDMA_CHANG/GET_O_K1", '100%', 30, "darkblue", 'DT', 'DT', -1,
    //    { FIRM: $('#FIRM_DD').val(), TYP: $('#KHDMA_DD').val(), MON: $('#MON_DD').val(), PERS: person_c.split(',')[0], TY: $('#ty').val(), PY: $('#tp').val() });
    
    Get_fin_year();
    bld_khdma_grd();
    set_dd_p(pers_cod, firm, "$('#OFF1_RNK').val(person_c.split(',')[4]); $('#OFF1_NM').val(person_c.split(',')[5]);$('#off1_data').val(person_c.split(',')[0]);");
    
    //bnd_khdma_grd($("#FIRM_DD").val(), $("#KHDMA_DD").val(), $("#MON_DD").val(), $("#TY_DD").val(), tp);
    $('#KHDMA_DD').on('select', function (event) {
        var item = args.item;
        var value = item.value;
        BLD1_DD('K_DT1', '../WORKFLOW_APP/KHDMA_CHANG/GET_O_K1', '100%', 30, 'darkblue', 'DT', 'DT', -1, { FIRM: $('#FIRM_DD').val(), TYP: value, MON: $('#MON_DD').val(), PERS: person_c.split(',')[0], TY: $('#ty').val(), PY: $('#tp').val() });
        BLD1_DD('K_DT2', '../WORKFLOW_APP/KHDMA_CHANG/GET_O_K1', '100%', 30, 'darkblue', 'DT', 'DT', -1, { FIRM: $('#FIRM_DD').val(), TYP: value, MON: $('#MON_DD').val(), PERS: $('#off2_data').val(), TY: $('#ty').val(), PY: $('#tp').val() });
        bnd_khdma_grd($('#FIRM_DD').val(), $('#KHDMA_DD').val(), $('#MON_DD').text(), $('#ty').val(), $('#tp').val());
    });
    $('#MON_DD').on('select', function (event) {
        var item = args.item;
        var value = item.value;
        BLD1_DD('K_DT1', '../WORKFLOW_APP/KHDMA_CHANG/GET_O_K1', '100%', 30, 'darkblue', 'DT', 'DT', -1, { FIRM: $('#FIRM_DD').val(), TYP: $('#KHDMA_DD').val(), MON: value, PERS: person_c.split(',')[0], TY: $('#ty').val(), PY: $('#tp').val() });
        BLD1_DD('K_DT1', '../WORKFLOW_APP/KHDMA_CHANG/GET_O_K1', '100%', 30, 'darkblue', 'DT', 'DT', -1, { FIRM: $('#FIRM_DD').val(), TYP: $('#KHDMA_DD').val(), MON: value, PERS: $('#off2_data').val(), TY: $('#ty').val(), PY: $('#tp').val() });
        bnd_khdma_grd($('#FIRM_DD').val(), $('#KHDMA_DD').val(), $('#MON_DD').text(), $('#ty').val(), $('#tp').val());
    });

    firms = firm;

    
    
});

function get_off(data) {
    $('#OFF2_RNK').val(data.RANK);
    $('#OFF2_NM').val(data.PERSON_NAME);
    $('#off2_data').val(data.PERSON_CODE);
    BLD1_DD('K_DT2', '../WORKFLOW_APP/KHDMA_CHANG/GET_O_K1', '100%', 30, 'darkblue', 'DT', 'DT', -1, { FIRM: $('#FIRM_DD').val(), TYP: $('#KHDMA_DD').val(), MON: $('#MON_DD').val(), PERS: data.PERSON_CODE, TY: $('#ty').val(), PY: $('#tp').val() });

    $("#dialog-edit").dialog('close');
}
// show report of khedma
//function print_rep() {
//    debugger
//    var FIRM = $('#FIRM_DD').val();
//    var Typ_Khedma = $('#KHDMA_DD').val();
//    var MonthYear = $('#MON_DD').val();
//    //var monthplus = parseInt(MON) + 1;
//    var Typ_Year = $('#ty').val();
//    var PROID = $('#tp').val();

//    var url = "REPORTS/Rep_Khedma_Change/Report_Khedma_Change.aspx?FIRM=" + FIRM + "&TYP=" + Typ_Khedma + "&MonthYear=" + MonthYear;
//    window.open(url, "_blank");
//}
function exc_exchng() {
    data = {
        FIRM: $('#FIRM_DD').val(), TYP: $('#KHDMA_DD').val(), PC1: person_c.split(',')[0], PC2: $('#off2_data').val(), F_DT: $('#K_DT1').val(), T_DT: $('#K_DT2').val(),
        Command: "add_exc"
    }
    pid = person_c.split(',')[7];
    firm = $('#TXT_FIRM_CODE').val();
    $.ajax({
        url: ControllerName + data.Command,
        type: 'POST',
        data: data,
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                $("#dialog-edit").dialog('close');
                swal({
                    title: "تم " + data.message,
                    text: "تم " + data.message + " بنجاح ",
                    type: "success",
                    timer: 2200
                });
                bnd_khdma_grd($('#FIRM_DD').val(), $('#KHDMA_DD').val(), $('#MON_DD').text(), $('#ty').val(), $('#tp').val());
            }
            else {
                swal({
                    title: "خطأ",
                    text: "  " + data.message + "",
                    type: "error",
                    timer: 2200
                });
            }
        },
        error: function () {
            swal({
                title: "خطأ",
                text: "  " + data.message + "",
                type: "error",
                timer: 2200
            });
        }
    });
}

function display_khdma() {
    bnd_khdma_grd($("#FIRM_DD").val(), $("#KHDMA_DD").val(), $("#MON_DD").val(), $("#TY_DD").val(), tp);
}

function Get_fin_year() {
    $.ajax({
        url: "../WORKFLOW_APP/TALAB_M2M/GET_fin_year",
        dataType: "json",
        success: function (reslult) {
            var FormList = reslult;
            if (FormList != "") {
                var dt = FormList;
                $("#ty").val(FormList[0].FIN_YEAR);
                $("#tp").val(FormList[0].TRAINING_PERIOD_ID);
            }
        },
        error: function (response) {

        }
    });
}

function open_confirm(row, gridId) {
    
    var D = $('#' + gridId.id).jqxGrid('getrowdata', row);
    if (gridId.id == "KHDMA_CH_GRD") {
        var url = ControllerName + 'Delete';
        var data = { FIRM_CODE: D.FIRM_CODE, FROM_PERSON_CODE: D.FROM_PERSON_CODE, TO_PERSON_CODE: D.TO_PERSON_CODE, ABSENCE_TYPE_ID: D.ABSENCE_TYPE_ID, FROM_DATE: D.FROM_DATE };
        open_del(url, data, gridId.id, "bnd_khdma_grd($('#FIRM_DD').val(), $('#KHDMA_DD').val(), $('#MON_DD').text(), $('#ty').val(), $('#tp').val())");

    }

}

function bld_khdma_grd() {

    var from_personcode;
    var to_personcode;
    var fromdate;

    theme = "darkblue";
    disp = "none, none, inline-flex";
    headerText = "الخدمات";
    gridAddUrl = ControllerName + 'Create';
    $("#KHDMA_CH_GRD").jqxGrid(
        {
            width: 1300,
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

                        { text: 'تاريخ الطلب', dataField: 'EXCHANGE_DATE', cellsalign: 'center', align: 'center', width: '15%', cellsformat: 'yyyy/MM/dd' },
                        { text: 'من ضابط', dataField: 'FOFF', cellsalign: 'center', align: 'center', width: '20%' },//
                        { text: 'الى ضابط', dataField: 'TOFF', cellsalign: 'center', align: 'center', width: '20%' },
                        { text: 'من يوم', dataField: 'FROM_DATE', cellsalign: 'center', align: 'center', width: '12%', cellsformat: 'yyyy/MM/dd' },
                        { text: 'الى يوم', dataField: 'TO_DATE', cellsalign: 'center', align: 'center', width: '12%', cellsformat: 'yyyy/MM/dd' },
                        { text: 'تصدق', dataField: 'IS_APPROVED', cellsalign: 'center', columntype: 'checkbox', align: 'center', width: '6%' },
                         {
                             text: 'المبادلة', datafield: 'Edit', align: 'center', cellsalign: 'center', columntype: 'button', disabled: true, width: '10%', cellsrenderer: function () {
                                 return "المبادلة";
                             }, buttonclick: function (row) {
                                 var FIRM = $('#FIRM_DD').val();
                                 var Typ_Khedma = $('#KHDMA_DD').val();
                                 var MonthYear = $('#MON_DD').text()
                                

                                 var url = "REPORTS/Rep_Khedma_Change/Report_Khedma_Change.aspx?FIRM=" + FIRM + "&TYP=" + Typ_Khedma + "&MonthYear=" + MonthYear + "&From_PersonCode=" + from_personcode + "&To_PersonCode=" + to_personcode + "&FromDate=" + fromdate;
                                 window.open(url, "_blank");
                             }
                         },
                        {
                            text: '', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                                return "<img   style='margin-left: 5px;cursor:pointer' height='17' width='17' src='../WORKFLOW_APP/images/delete.png' onclick='open_confirm(" + row + ", KHDMA_CH_GRD)'/>";
                          }
                        }
            ]
        });
    $('#KHDMA_CH_GRD').jqxGrid('clearselection');

    $("#KHDMA_CH_GRD").on("rowselect", function (event) {
        
        var args = event.args;
        var row = args.rowindex;
        var data = $('#KHDMA_CH_GRD').jqxGrid('getrowdata', row);

        from_personcode = data.FROM_PERSON_CODE;
        to_personcode = data.TO_PERSON_CODE;
        fromdate = data.FROM_DATE;
        var spl = fromdate.split(" ");
        fromdate = spl[1];
        //alert(fromdate);

    });


}

function bnd_khdma_grd(FIRM, TYP, MON, TY, PROID) {

    var source = {
        datatype: "json",
        datafield : [
                        { name: 'FIRM_CODE' },
                        { name: 'FROM_PERSON_CODE' },
                        { name: 'TO_PERSON_CODE' },
                        { name: 'ABSENCE_TYPE_ID' },
                        { name: 'FROM_DATE' },
                        { name: 'FROM_RANK_ID' },
                        { name: 'FROM_RANK_CAT_ID' },
                        { name: 'FROM_PERSON_CAT_ID' },
                        { name: 'TO_RANK_ID' },
                        { name: 'TO_RANK_CAT_ID' },
                        { name: 'TO_PERSON_CAT_ID' },
                        { name: 'EXCHANGE_DATE' },
                        { name: 'TO_DATE' },
                        { name: 'OPENION1' },
                        { name: 'SEC_COMMAND_OPENION' },
                        { name: 'COMMAND_DECISION' },
                        { name: 'IS_APPROVED' },
                        { name: 'APPROVAL_NO' },
                        { name: 'APPROVAL_DATE' },
                        { name: 'RANK1' },
                        { name: 'NAME1' },
                        { name: 'RANK2' },
                        { name: 'NAME2' },
                        { name: 'OTHER_PER_DECS' },
                        { name: 'PLANNING_DECESION' },
                        { name: 'PLANNING_NOTES' },
                        { name: 'VICE_COMMAND_DECESION' },
                        { name: 'VICE_COMMAND_NOTES' },
                        { name: 'FOFF' },
                        { name: 'TOFF' }
        ],
        async: false,
        url: ControllerName + 'get_khd_grd',
        data: {
            FIRM: FIRM,
            TYP: TYP,
            MON: MON,
            PERS: localStorage.getItem("person_c").split(',')[0]
        }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#KHDMA_CH_GRD").jqxGrid({ source: dataAdapter });

}

function KHDMA_CH_GRD_Refresh() {
    bnd_khdma_grd($('#FIRM_DD').val(), $('#KHDMA_DD').val(), $('#MON_DD').text(), $('#ty').val(), $('#tp').val());
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
                      text: 'من', dataField: 'DT', cellsalign: 'center', align: 'center', width: '15%', columntype: 'datetimeinput', editable: true, cellsformat: 'dd/MM/yyyy',
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
                      text: 'الى', dataField: 'DT1', cellsalign: 'center', align: 'center', width: '15%', columntype: 'datetimeinput', editable: true, cellsformat: 'dd/MM/yyyy',
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
    var dt2 = x1 != null ? x1.getDate() + "/" + (x1.getMonth() + 1) + "/" + x1.getFullYear() : dt;
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
    var d = $('#' + gridId.id).jqxGrid('getrowdata', $('#' + gridId.id).jqxGrid('selectedrowindex'));
    var dt = d.FROM_DATE;
    var dt2 = d.TO_DATE;
    var url = ControllerName + 'del_exc';
    data = {
        FIRM: $('#FIRM_DD').val(), TYP: d.ABSENCE_TYPE_ID, PC1: d.FROM_PERSON_CODE, PC2: d.TO_PERSON_CODE, F_DT: d.FROM_DATE, T_DT: d.TO_DATE
    }
    open_del(url, data, gridId.id);


}

