var ControllerName = ApplicationName + '/TALAB_M2M_OK/';
var period_id;
var mission;
var mission_det;


$(document).ready(function () {
    $('#jqxTabs').hide();
    build_static();
    build_mission_grid();
    build_mission_member_grid();
    get_person_data();
    BuildDropDwon1('FIRMS_CODE', 'FIRM_CODE', 'NAME', ApplicationName + '/GROUPS/firms_ddl', firm_cod);
    seet_fin_year();
    $('#dt_firm_missions').on('rowselect', function (event) {
        
        var args = event.args;
        var row = args.rowindex;
        $('#jqxTabs').jqxTabs({ selectedItem: 0 });
        var data = $('#dt_firm_missions').jqxGrid('getrowdata', row);
        data_row = $('#dt_firm_missions').jqxGrid('getrowid', row);
      //  build_mission_member_grid();
     
       // document.getElementById('dt_mission_member&add').style.display = 'inline-block';
     //   document.getElementById('steps').style.display = 'inline-block';
        $('#txt_mission_introduction').val(data.TITLE);
        $('#txt_mission_subject').val(data.SUBJECT);
        $('#txt_mission_distribution').val($('#TXT_PERIOD').val());
        mission = data.ID;
        if (data.TYP == "مأمورية") {
            $('#jqxTabs').show();
               bnd_mission_member_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), pers_cod, data.ID);
        }
        else {
            $('#jqxTabs').hide();
        }
        //  build_ROLE_grid();
        // bnd_ROLE_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $("#user_name").val());
    });
    $('#DT_DATE').on('change', function (event) {

        bnd_mission_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), $("#user_name").val());
        $('#dt_mission_member').jqxGrid('clear');
    });



});
function build_static() {
    $('#DT_DATE').jqxDateTimeInput({ animationType: 'fade', width: '80%', height: 38, rtl: true, dropDownHorizontalAlignment: 'right', theme: "darkblue" });
    $('#TXT_FIN_YEAR').jqxInput({ width: '80%', height: 38, theme: 'darkblue', disabled: true });
    $('#TXT_PERIOD').jqxInput({ width: '80%', height: 38, theme: 'darkblue', disabled: true });
    $('#page_name').text('  التصديق علي المأمورية');
    $('#jqxTabs').jqxTabs({ width: '80%', height: 300, rtl: true, theme: 'darkblue' });
    $('#txt_mission_introduction').jqxInput({ width: '85%', height: 70, theme: 'darkblue', disabled: true });
    $('#txt_mission_subject').jqxInput({ width: '85%', height: 70, theme: 'darkblue', disabled: true });
    $('#txt_mission_distribution').jqxInput({ width: '85%', height: 70, theme: 'darkblue', disabled: true });

}
function build_mission_grid() {


    gridId = 'dt_firm_missions';

    theme = "darkblue";
    headerText = "التصديقات";
    gridAddUrl = ControllerName + 'Create';
    $("#dt_firm_missions").jqxGrid({
        width: '80%',
        height: 230,
        theme: "darkblue",
        sortable: true,
        rtl: true,
        showaggregates: true,
        filterable: true,
        showfilterrow: true,
        showtoolbar: true,
        rendertoolbar: toolbarfn,
        columns: [
                       { text: 'نوع التصديق', dataField: 'TYP', cellsalign: 'center', align: 'center', width: '5%' },
                       { text: 'ملخص', dataField: 'TITLE', cellsalign: 'center', align: 'center', width: '10%' },
                       { text: 'مستوي التصديق', dataField: 'STP', cellsalign: 'center', align: 'center', width: '20%' },
                       { text: 'مقدم الطلب', dataField: 'OFF', cellsalign: 'center', align: 'center', width: '20%' },
                       { text: 'من', dataField: 'FROM_DATE', cellsalign: 'center', align: 'center', width: '15%' },
                       { text: 'الي', dataField: 'TO_DATE_DT', cellsalign: 'center', align: 'center', width: '15%' },
                       {
                           text: 'تصدق', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                               
                               var dt = $('#dt_firm_missions').jqxGrid('getrowdata', row);
                               var x = dt.TYP == "أجازة" ? 2 : dt.TYP == "مأمورية" ? 3 : 1;
                               return "<img  style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/edit.png' onclick='open_edit(" + row + ",dt_firm_missions, " + x + ", 1)'/>";
                            }
                       },
                       {
                           text: 'رفض', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                               var dt = $('#dt_firm_missions').jqxGrid('getrowdata', row);
                               var x = dt.TYP == "أجازة" ? 2 : "مأمورية" ? 3 : 1;
                               return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/delete.png' onclick='open_edit(" + row + ",dt_firm_missions, " + x + ", 0)'/>";
                          }
                       },
                       {
                           text: 'طباعة', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                               return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/printer.png' onclick='open_confirm(" + row + ",dt_firm_missions)'/>";
                           }
                       }

        ]

    });
    $("#dt_firm_missions").jqxGrid({ enabletooltips: true });
}
function bnd_mission_grid(firm_code, fin_year, PERIOD_ID, date, person_id) {

    $('#dt_firm_missions').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
               { name: 'FIN_YEAR' },
               { name: 'TRAINING_PERIOD_ID' },
               { name: 'FIRM_CODE' },
               { name: 'ID' },
               { name: 'TYP' },
               { name: 'TITLE' },      
               { name: 'SUBJECT' },
               { name: 'FROM_DATE' },
               { name: 'TO_DATE_DT' },
               { name: 'OFF_ABS_STEPS_ID' },
               { name: 'OFF_ABS_GROUP_ID' },
               { name: 'PERSON_CODE' },
               { name: 'SEQ' },
               { name: 'COM' },
               { name: 'STP' },
               { name: 'OFF' }

        ],
        async: false,

        url: 'TALAB_M2M_OK/GET_grid_m2m',
        data: { firm_code: firm_code, fin_year: fin_year, P: PERIOD_ID, date: date, person_id: person_id }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#dt_firm_missions").jqxGrid({ source: dataAdapter });
}
function build_mission_member_grid() {


    gridId = 'dt_mission_member';

    theme = "darkblue";
    headerText = " ضباط المأمورية";
    gridAddUrl = 'TALAB_M2M/Create_Off';
    $("#dt_mission_member").jqxGrid({
        width: '99%',
        height: 230,
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
        //   selectionmode: 'singlerow',
        //source: dataAdapter,
        rendertoolbar: toolbarfn,
        columns: [
                                   { text: 'الرتبــة', dataField: 'RANK', cellsalign: 'center', align: 'center', width: '15%' },
                       { text: 'الإســـــــــم', dataField: 'MALK_NAME', cellsalign: 'center', align: 'center', width: '75%' },
                       //{ text: 'التكليف', dataField: 'PERSON_MISSION', cellsalign: 'center', align: 'center', width: '10%' },
                               {

                                   text: 'تصدق', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                                       return "<img  style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/edit.png' onclick='open_edit(" + row + ",dt_mission_member,1, 1)'/>";
                                   }
                               },
                  {
                      text: 'رفض', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                          return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/delete.png' onclick='open_edit(" + row + ",dt_mission_member,1,0)'/>";
                      }
                  }

        ]

    });
    $("#dt_mission_member").jqxGrid({ enabletooltips: true });
}
function bnd_mission_member_grid(firm_code, fin_year, PERIOD_ID, date, person_id, MISSION_ID) {

    $('#dt_mission_member').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
                        { name: 'MALK' },
                   { name: 'TASDEK' },
                        { name: 'FIN_YEAR' },
               { name: 'TRAINING_PERIOD_ID' },
                { name: 'FIRM_CODE' },
                   { name: 'RANK' },
                   { name: 'MALK_NAME' },
                   { name: 'TASDEK_NAME' },
                   { name: 'INTRODUCTION' },
                   { name: 'MISSION_ID' },
                   { name: 'SUBJECT' },
                          { name: 'FROM_DATE' },
                   { name: 'TO_DATE' }


        ],
        async: false,

        url: 'TALAB_M2M_OK/GET_grid_m2m_mem',
        data: { firm_code: firm_code, fin_year: fin_year, P: PERIOD_ID, date: date, person_id: person_id, MISSION_ID: MISSION_ID }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#dt_mission_member").jqxGrid({ source: dataAdapter });
}
function seet_fin_year() {
    $.ajax({
        url: "TALAB_M2M/GET_fin_year",
        dataType: "json",
        success: function (reslult) {
            var FormList = reslult;
            if (FormList != "") {
                var dt = FormList;
                $("#TXT_FIN_YEAR").val(FormList[0].FIN_YEAR);
                $("#TXT_PERIOD").val(FormList[0].TRAINING_PERIOD);
                period_id = FormList[0].TRAINING_PERIOD_ID;
                bnd_mission_grid(firm_cod, FormList[0].FIN_YEAR, period_id, $('#DT_DATE').val(), pers_cod);
            }
        },
        error: function (response) {

        }
    });
}
function open_edit(row, gridId, flag, d) {
    
     var details = $('#' + gridId.id).jqxGrid('getrowdata', row);
   // var details = dt;
    if (flag == 2) {
        var url = ControllerName + 'vac_ok';
        var data = { PERSON_VACATIONS_DET_ID: details.SEQ, PERSON_VACATIONS_SEQ: details.ID, PERSON_DATE_ID: details.PERSON_CODE, PERSON_DATE_OWEN: details.COM, DECTION: d };
    }
    else if (flag == 3) {

        var url = ControllerName + 'Edit';
        var data = {
            FIN_YEAR: details.FIN_YEAR, TRAINING_PERIOD_ID: details.TRAINING_PERIOD_ID, MISSION_ID: details.ID, FIRM_CODE: details.FIRM_CODE, PERSON_CODE: details.PERSON_CODE,
            FIRM_MISSIONS_DET_ID: details.SEQ, PERSON_DATE_OWEN: details.COM, DECTION: d
        };
    }
    else {
        var url = ControllerName + 'abs_ok';
        var data = {
            FIN_YEAR: details.FIN_YEAR, TRAINING_PERIOD_ID: details.TRAINING_PERIOD_ID, ABSENCE_TYPE_ID: details.ID, FIRM_CODE: details.FIRM_CODE,
            FIRMS_ABSENCES_PERSONS_DET_ID: details.SEQ, PERSON_CODE: details.PERSON_CODE, FROM_DATE: details.FROM_DATE, TO_DATE: details.TO_DATE, PERSON_DATE_OWEN: details.COM, DECTION: d
        };
    }
    edit_fun(url, data, flag);
}
function edit_fun(url, data, flag) {
    
    $.ajax({
        url: url,
        type: 'POST',
        data: data,
        dataType: 'json',
        success: function (Data) {

            if (Data.status) {
                swal({
                    title: Data.title,
                    text: Data.message,
                    type: Data.type,
                    timer: 2200
                });
                if (gridId == "dt_firm_missions") {
                    
                    var xxx = $("#DT_DATE").val();
                    //eval(gridId + "_Refresh(" + firm_cod + ",'" + $("#TXT_FIN_YEAR").val() + "'," + period_id + ",'" + xxx + "'," + $("#user_name").val() + "," + mission + ")");
                    bnd_mission_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), $("#user_name").val());
                  //  $("#dialog-confirm").dialog('close');
                }
                if (gridId == "dt_mission_member") {

                    var xxx = $("#DT_DATE").val();
                    eval(gridId + "_Refresh(" + firm_cod + ",'" + $("#TXT_FIN_YEAR").val() + "'," + period_id + ",'" + xxx + "'," + $("#user_name").val() + "," + mission + ")");
                    bnd_mission_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), $("#user_name").val());
                    $("#dialog-confirm").dialog('close');
                }
                if (gridId == "off_ROLE") {

                    //   var xxx = $("#DT_DATE").val();
                    eval(gridId + "_Refresh(" + firm_cod + ",'" + $("#TXT_FIN_YEAR").val() + "'," + period_id + "," + $("#user_name").val() + ")");
                 //   $("#dialog-confirm").dialog('close');
                }
                //    $('#OFF_ABS_GROUP_NAME').val('');
                //  AG_SECTORS_Refresh('AG_SECTORS');
                //alert(data.message);
                //  $("#dialog-edit").dialog('close');
                // alert("saad");
            }
            else {
                bnd_mission_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), $("#user_name").val());
                //swal({
                //    title: "خطا",
                //    text: "  " + Data.message + "",
                //    type: "error",
                //    timer: 2200
                //});

              //  $("#dialog-confirm").dialog('close');
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
            $("#dialog-confirm").dialog("close");
        }
    });
}
function open_confirm(row, gridId, flag, d) {

    var details = $('#' + gridId.id).jqxGrid('getrowdata', row);
    if (gridId.id == "dt_firm_missions" && flag == 2) {
        var url = ControllerName + 'vac_ok';
        var data = { PERSON_VACATIONS_DET_ID: details.SEQ, PERSON_VACATIONS_SEQ: details.ID, PERSON_DATE_ID: details.PERSON_CODE, PERSON_DATE_OWEN: details.COM, DECTION: d };
        open_del(url, data, gridId.id);

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
function dt_mission_member_Refresh(firm_code, fin_year, PERIOD_ID, date, person_id, MISSION_ID) {
    
    $('#dt_mission_member').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
                        { name: 'PERSON_MISSION_DATE' },
                   { name: 'PERSON_MISSION' },
                   { name: 'PERSON_CAT_ID' },
                   { name: 'RANK_CAT_ID' },
                   { name: 'FIRM_CODE' },
                   { name: 'PERSON_CODE' },
                   { name: 'PERSON_NAME' },
                   { name: 'RANK_ID' },
                          { name: 'RANK' },
                   { name: 'MISSION_ID' },
                   { name: 'TRAINING_PERIOD_ID' },
                   { name: 'FIN_YEAR' }

        ],
        async: false,

        url: 'TALAB_M2M_OK/GET_grid_m2m_mem',
        data: { firm_code: firm_code, fin_year: fin_year, P: PERIOD_ID, date: date, person_id: person_id, MISSION_ID: MISSION_ID }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#dt_mission_member").jqxGrid({ source: dataAdapter });
}
function dt_firm_missions_Refresh(firm_code, fin_year, PERIOD_ID, date, person_id) {
    
    $('#dt_firm_missions').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
         { name: 'ROWNUM' },
               { name: 'IS_DONE' },

               { name: 'IS_PLANNED' },
               { name: 'MISSION_TYPE' },
               { name: 'MISSION_TYPE_NAME' },
               { name: 'TO_DATE' },
               { name: 'FROM_DATE' },
               { name: 'DISTRIBUTION' },
               { name: 'PROJECT_ID' },
               { name: 'FIRM_CODE' },
               { name: 'FIN_YEAR' },
               { name: 'TRAINING_PERIOD_ID' },
               { name: 'FIRM_NAME' },
               { name: 'MISSION_ID' },
               { name: 'MISSION_FIRM_CODE' },
               { name: 'INTRODUCTION' },
               { name: 'FINAL' },
               { name: 'SUBJECT' }

        ],
        async: false,

        url: 'TALAB_M2M_OK/GET_grid_m2m',
        data: { firm_code: firm_code, fin_year: fin_year, P: PERIOD_ID, date: date, person_id: person_id }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#dt_firm_missions").jqxGrid({ source: dataAdapter });
}
function set_ddl(id1, frm) {

    var pid = id1;
    //getQS('nn', window.location.href);

    $.ajax({
        url: "groups/GET_JOP",
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
                //person_rank_cat = dt[3];
                //$("#PERSON_CODE").val(dt[0]);

                //$("#RANK_CAT_ID").val(dt[5]);
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
function all_ok() {
    $.ajax({
        url: "TALAB_M2M_OK/all_ok",
        type: 'POST',
        //data: data,
        dataType: 'json',
        success: function (Data) {
            
            swal({
                title: "تم",
                text: "تم التصديق على جميع الطلبات",
                type: Data.type,
                timer: 2200
            });
            bnd_mission_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), $("#user_name").val());
        },
        error: function (err) {
            swal({
                title: "خطا ",
                text: "خطأ  ف الحذف",
                type: "error",
                timer: 2200
            });
        }
    });
}