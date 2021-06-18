var ControllerName = ApplicationName + '/TALAB_M2M_SH/';
var period_id;
var mission;
var mission_det;
var mission_per_cod;
var fin_year;
var data_row1;
var fc;

$(document).ready(function () {
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
        var data = $('#dt_firm_missions').jqxGrid('getrowdata', event.args.rowindex);
       // data_row1 = { PERSON_CODE: data.PERSON_CODE, FIN_YEAR: data.FIN_YEAR, TRAINING_PERIOD_ID: data.TRAINING_PERIOD_ID, FIRM_CODE: data.FIRM_CODE, FROM_DATE: data.FROM_DATE, ABSENCE_TYPE_ID: data.ABSENCE_TYPE_ID, RANK_CAT_ID: data.RANK_CAT_ID, PERSON_CAT_ID: data.PERSON_CAT_ID };
        data_row1 = { FIN_YEAR: data.FIN_YEAR, TRAINING_PERIOD_ID: data.TRAINING_PERIOD_ID, MISSION_ID: data.MISSION_ID, FIRM_CODE: data.FIRM_CODE };
      
    //    var data = $('#dt_firm_missions').jqxGrid('getrowdata', row);
        mission = data.MISSION_ID;
        data_row = $('#dt_firm_missions').jqxGrid('getrowid', row);
        bnd_mission_member_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), $("#user_name").val(), data.MISSION_ID);
        document.getElementById('dt_mission_member&add').style.display = 'inline-block';
       document.getElementById('steps').style.display = 'none';
        $('#txt_mission_introduction').val(data.INTRODUCTION);
        $('#txt_mission_subject').val(data.SUBJECT);
        $('#txt_mission_distribution').val($('#TXT_PERIOD').val());
        mission = data.MISSION_ID;
        fc = data.FIRM_CODE;
        //  build_ROLE_grid();
        // bnd_ROLE_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $("#user_name").val());
      
    });


    $('#dt_mission_member').on('rowselect', function (event) {
        
        var args = event.args;
        var row = args.rowindex;
        $('#jqxTabs').jqxTabs({ selectedItem: 0 });
        var data = $('#dt_mission_member').jqxGrid('getrowdata', row);
        data_row = $('#dt_mission_member').jqxGrid('getrowid', row);
      //  bnd_mission_member_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), $("#user_name").val(), data.MISSION_ID);
       // document.getElementById('dt_mission_member&add').style.display = 'inline-block';
        document.getElementById('steps').style.display = 'inline-block';

        mission_per_cod = data.PERSON_CODE;
      //    build_ROLE_grid();
        //  bnd_ROLE_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, mission_per_cod);
     
    }); 
    $('#steps').on('click', function (event) {
        
        build_ROLE_grid();
        bnd_ROLE_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, mission_per_cod, mission);
    });

    $('#DT_DATE').on('change', function (event) {


        bnd_mission_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), $("#user_name").val());
        $('#dt_mission_member').jqxGrid('clear');
    });

    $("#off_ROLE").on('cellendedit', function (event) {
        var column = args.datafield;
        var row = args.rowindex;


        var value = args.value;
        var oldvalue = args.oldvalue;
        if (value != oldvalue) {

            if (column == "PERSON_NAME") {
                var b = ["", edit_c[1], "", ""]



                //   $("#BOS_HF").val(b);




                var rowdata = $('#off_ROLE').jqxGrid('getrowdata', row);

                var data = $('#off_ROLE').jqxGrid('getrowdata', event.args.rowindex);


                data = {
                    TRAINING_PERIOD_ID: period_id, FIRM_CODE: firm_cod, FIN_YEAR: $("#TXT_FIN_YEAR").val(), MISSION_ID: mission,
                    OFF_ABS_STEPS_ID: data.OFF_ABS_STEPS_ID, OFF_ABS_GROUP_ID: data.OFF_ABS_GROUP_ID, MISSION_ID: mission, PERSON_CODE: data.PERSON_DATA_ID, per: $("#user_name").val(),
                    PERSON_DATE_OWEN: b[1], step_name: data.OFF_ABS_STEPS_NAME, FIRM_MISSIONS_DET_ID: data.FIRM_MISSIONS_DET_ID,

                    Command: "add_officer_fun"
                }

                $.ajax({
                    url: ControllerName + data.Command,
                    type: 'POST',
                    data: data,
                    dataType: 'json',
                    success: function (data) {



                        if (data.status) {
                            swal({
                                title: "تم التعديل",
                                text: "تم  التعديل بنجاح",
                                type: "success",
                                timer: 2200
                            });
                            bnd_ROLE_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, mission_per_cod);
                            //    $('#OFF_ABS_GROUP_NAME').val('');
                            //  AG_SECTORS_Refresh('AG_SECTORS');
                            //alert(data.message);
                            //  $("#dialog-edit").dialog('close');
                            // alert("saad");
                        }
                        else {
                            swal({
                                title: "خطا",
                                text: "  " + data.message + "",
                                type: "error",
                                timer: 2200
                            });
                            bnd_ROLE_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, mission_per_cod);
                            $("#dialog-edit").dialog('close');
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
        else {
            swal({
                title: "خطأ ",
                text: "الضابط موجود بالفعل",
                type: "error",
                timer: 2200
            });
        }

    });

    $("#off_ROLE").on("cellclick", function (event) {

        var column = event.args.column;
        var row = event.args.rowindex;
        var columnindex = event.args.columnindex;
        var rowdata = $('#off_ROLE').jqxGrid('getrowdata', row);
        mission_det = rowdata.FIRM_MISSIONS_DET_ID;
        // $('#LGNA_DATE').val(rowdata.COMMITTEE_DATE);
        // build_lgna_plan(rowdata.FIRM_CODE, rowdata.COMMITTEE_TYPE, rowdata.COMMITTEE_DATE, rowdata.COMMITTEE_MONTH, rowdata.FIN_YEAR);
    });

});

function build_static() {
    $('#DT_DATE').jqxDateTimeInput({ animationType: 'fade', width: '80%', height: 38, rtl: true, dropDownHorizontalAlignment: 'right', theme: "darkblue" });
    $('#TXT_FIN_YEAR').jqxInput({ width: '80%', height: 38, theme: 'darkblue', disabled: true });
    $('#TXT_PERIOD').jqxInput({ width: '80%', height: 38, theme: 'darkblue', disabled: true });
    $('#page_name').text(' المأموريــات');
    $('#jqxTabs').jqxTabs({ width: '80%', height: 300, rtl: true, theme: 'darkblue' });
    $('#txt_mission_introduction').jqxInput({ width: '85%', height: 70, theme: 'darkblue', disabled: true });
    $('#txt_mission_subject').jqxInput({ width: '85%', height: 70, theme: 'darkblue', disabled: true });
    $('#txt_mission_distribution').jqxInput({ width: '85%', height: 70, theme: 'darkblue', disabled: true });
}
function build_mission_grid() {


    gridId = 'dt_firm_missions';

    theme = "darkblue";
    headerText = " المأموريات";
    gridAddUrl = ControllerName + 'Create';
    $("#dt_firm_missions").jqxGrid({
        width: '80%',
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
                                  { text: 'م', dataField: 'ROWNUM', cellsalign: 'center', align: 'center', width: '5%' },
                       { text: 'الجهة', dataField: 'FIRM_NAME', cellsalign: 'center', align: 'center', width: '30%' },
                       { text: 'من', dataField: 'FROM_DATE', cellsalign: 'center', align: 'center', width: '15%' },
                       { text: 'الي', dataField: 'TO_DATE', cellsalign: 'center', align: 'center', width: '15%' },//
                       { text: 'نوع المأمورية', dataField: 'TYP_NAME', cellsalign: 'center', align: 'center', width: '20%' },//
                                                     {

                                                         text: 'طباعه', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                                                             return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/printer.png' onclick='off_rep();'/>";
                                                         }
                                                     },

                      // { text: 'تمت', columntype: 'checkbox', dataField: 'IS_DONE', cellsalign: 'center', align: 'center', width: '5%' },
                               {

                                   text: 'تعديل', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                                       return "<img  style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/edit.png' onclick='open_edit(" + row + ",dt_firm_missions)'/>";
                                   }
                               },

                  {
                      text: 'حذف', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                          return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/delete.png' onclick='open_confirm(" + row + ",dt_firm_missions)'/>";
                      }
                  }

        ]

    });
    $("#dt_firm_missions").jqxGrid({ enabletooltips: true });
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
                { name: 'TYP_NAME' },
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

        url: 'TALAB_M2M_SH/GET_grid_m2m',
        data: { firm_code: firm_code, fin_year: fin_year, P: PERIOD_ID, date: date, person_id: person_id }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#dt_firm_missions").jqxGrid({ source: dataAdapter });
}
function build_mission_member_grid() {

    gridId = 'dt_mission_member';

    theme = "darkblue";
    headerText = " ضباط المأموريات";
    gridAddUrl = 'TALAB_M2M_SH/Create_Off';
    $("#dt_mission_member").jqxGrid({
        width: '99%',
        height: 215,
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
                                   { text: 'الرتبــة', dataField: 'RANK', cellsalign: 'center', align: 'center', width: '10%' },
                       { text: 'الإســـــــــم', dataField: 'PERSON_NAME', cellsalign: 'center', align: 'center', width: '85%' },

                               //{

                               //    text: 'تعديل', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                               //        return "<img  style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/edit.png' onclick='open_edit(" + row + ",dt_mission_member)'/>";
                               //    }
                               //},
                               //                                                        {
                               //                                                            text: 'قطع', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {

                               //                                                                return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/ok.png' onclick='open_edit(" + row + ",dt_firm_missions)'/>";
                               //                                                            }
                               //                                                        },
                  {
                      text: 'حذف', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                          return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/delete.png' onclick='open_confirm(" + row + ",dt_mission_member)'/>";
                      }
                  }

        ]

    });
    $("#dt_mission_member").jqxGrid({ enabletooltips: true });
}
function seet_fin_year() {


    //getQS('nn', window.location.href);

    $.ajax({
        url: "TALAB_M2M/GET_fin_year",
        // data: { ID: pid, FIRM: frm },


        dataType: "json",

        success: function (reslult) {

            //var FormList = JSON.parse(reslult);
            var FormList = reslult;
            //pers_cod = FormList[0].employeeid;
            if (FormList != "") {

                var dt = FormList;
                

                $("#TXT_FIN_YEAR").val(FormList[0].FIN_YEAR);
                $("#TXT_PERIOD").val(FormList[0].TRAINING_PERIOD);
                period_id = FormList[0].TRAINING_PERIOD_ID;
                fin_year = FormList[0].FIN_YEAR;
                bnd_mission_grid(firm_cod, FormList[0].FIN_YEAR, period_id, $('#DT_DATE').val(), pers_cod);
                document.getElementById('dt_firm_missions&add').style.display = 'inline-block';
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
function bnd_mission_grid(firm_code, fin_year, PERIOD_ID, date, person_id) {

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
                { name: 'TYP_NAME' },
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

        url: 'TALAB_M2M_SH/GET_grid_m2m',
        data: { firm_code: firm_code, fin_year: fin_year, P: PERIOD_ID, date: date, person_id: person_id }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#dt_firm_missions").jqxGrid({ source: dataAdapter });
}
function bnd_mission_member_grid(firm_code, fin_year, PERIOD_ID, date, person_id, MISSION_ID) {

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

        url: 'TALAB_M2M_SH/GET_grid_m2m_mem',
        data: { firm_code: firm_code, fin_year: fin_year, P: PERIOD_ID, date: date, person_id: person_id, MISSION_ID: MISSION_ID }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#dt_mission_member").jqxGrid({ source: dataAdapter });
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

        url: 'TALAB_M2M/GET_grid_m2m_mem',
        data: { firm_code: firm_code, fin_year: fin_year, P: PERIOD_ID, date: date, person_id: person_id, MISSION_ID: MISSION_ID }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#dt_mission_member").jqxGrid({ source: dataAdapter });
}
function build_gehat_grid() {

    
    gridId = 'gehat_grid';

    theme = "darkblue";
    headerText = " الجهات";
    gridAddUrl = ControllerName + 'Create';
    $("#gehat_grid").jqxGrid({
        width: 450,
        height: 300,
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

                       { text: 'الجهة', dataField: 'NAME', cellsalign: 'center', align: 'center', width: '100%' }


        ]

    });
    $("#gehat_grid").jqxGrid({ enabletooltips: true });
}
function bnd_gehat_grid() {

    $('#gehat_grid').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
             { name: 'FIRM_CODE' },
             { name: 'PARENT_FIRM_CODE' },
             { name: 'NAME' }

        ],
        async: false,

        url: 'TALAB_M2M/GET_grid_gehat',
        data: {}

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#gehat_grid").jqxGrid({ source: dataAdapter });
}
function build_ROLE_grid() {

    gridId = 'off_ROLE';

    theme = "darkblue";
    headerText = " الخطوات";
    gridAddUrl = ControllerName + 'Create';
    $("#off_ROLE").jqxGrid({
        width: '99%',
        height: 215,
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
                                text: 'الاسم ', dataField: 'PERSON_NAME', cellsalign: 'center', align: 'center', width: '45%', columntype: 'combobox',
                                createeditor: function (row, cellvalue, editor, cellText, width, height) {
                                    var ROL = $('#off_ROLE').jqxGrid('getrowdata', row).ROL;
                                    
                                    var x = [];
                                    x[0] = 1;
                                    var source1 = {
                                        datatype: "json",
                                        datafields:
                                            [{ name: 'NM' },
                                            { name: 'PERSON_CODE' }],
                                        async: false,

                                        url: 'SD_VAC/GET_ROL_OFF',
                                        data: { firm: firm_cod, rol: ROL }
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
                            },
                               //{

                               //    text: 'تعديل', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                               //        return "<img  style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/edit.png' onclick='open_edit(" + row + ",dt_firm_missions)'/>";
                               //    }
                               //},
                  {
                      text: 'حذف', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                          return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/delete.png' onclick='open_confirm(" + row + ",off_ROLE)'/>";
                      }
                  }

        ]

    });
    $("#off_ROLE").jqxGrid({ enabletooltips: true });
}
function bnd_ROLE_grid(firm_code, fin_year, PERIOD_ID, person_id) {

    $('#off_ROLE').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
                        { name: 'OFF_ABS_STEPS_ID' },
                   { name: 'OFF_ABS_GROUP_ID' },
                   { name: 'OFF_ABS_STEPS_NAME' },
                           { name: 'FIRM_MISSIONS_DET_ID' },

                   { name: 'ORDER_ID' },
                   { name: 'PERSON_DATA_ID' },
                   { name: 'PERSON_NAME' },
                       { name: 'ROL' },
                   { name: 'RANK' }

        ],
        async: false,

        url: 'TALAB_M2M_SH/GET_grid_ROLE',
        data: { firm_code: firm_code, fin_year: fin_year, P: PERIOD_ID, person_id: person_id, mission: mission }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#off_ROLE").jqxGrid({ source: dataAdapter });
}
function off_ROLE_Refresh(firm_code, fin_year, PERIOD_ID, person_id) {

    $('#off_ROLE').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
                        { name: 'OFF_ABS_STEPS_ID' },
                   { name: 'OFF_ABS_GROUP_ID' },
                   { name: 'OFF_ABS_STEPS_NAME' },
                           { name: 'FIRM_MISSIONS_DET_ID' },
                   { name: 'ORDER_ID' },
                   { name: 'PERSON_DATA_ID' },
                   { name: 'PERSON_NAME' },
                   { name: 'RANK' }

        ],
        async: false,

        url: 'TALAB_M2M_SH/GET_grid_ROLE',
        data: { firm_code: firm_code, fin_year: fin_year, P: PERIOD_ID, person_id: person_id, mission: mission }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#off_ROLE").jqxGrid({ source: dataAdapter });
}
function open_edit(row, gridId) {
    
    var details = $('#' + gridId.id).jqxGrid('getrowdata', row);
    if (gridId.id == "dt_firm_missions") {
        var url = ControllerName + 'Edit/' + details.MISSION_ID;
        var data = { FIN_YEAR: details.FIN_YEAR, TRAINING_PERIOD_ID: details.TRAINING_PERIOD_ID, MISSION_ID: details.MISSION_ID, FIRM_CODE: details.FIRM_CODE };

    } else if (gridId.id == "st_group__steps_gridDiv") {
        var url = ControllerName + 'Edit_Steps/' + details.OFF_ABS_STEPS_ID;
        var data = { id: details.OFF_ABS_STEPS_ID, id1: details.OFF_ABS_GROUP_ID };
    }

    openDialog_parm(url, data_row1, "تعديل");
}
function open_edit1(row, gridId) {

    var details = $('#' + gridId.id).jqxGrid('getrowdata', row);
    if (gridId.id == "dt_firm_missions") {
        var url = ControllerName + 'Edit_STATUS/' + details.MISSION_ID;
        var data = { FIN_YEAR: details.FIN_YEAR, TRAINING_PERIOD_ID: details.TRAINING_PERIOD_ID, MISSION_ID: details.MISSION_ID, FIRM_CODE: details.FIRM_CODE };

    } else if (gridId.id == "st_group__steps_gridDiv") {
        var url = ControllerName + 'Edit_Steps/' + details.OFF_ABS_STEPS_ID;
        var data = { id: details.OFF_ABS_STEPS_ID, id1: details.OFF_ABS_GROUP_ID };
    }

    openDialog_parm(url, data_row1, "  قطع المأمورية");
}
function open_confirm(row, gridId) {

    var details = $('#' + gridId.id).jqxGrid('getrowdata', row);
    
    if (gridId.id == "dt_firm_missions") {
        var url = ControllerName + 'Delete';
        var data = { TRAINING_PERIOD_ID: period_id, FIRM_CODE: firm_cod, FIN_YEAR: $("#TXT_FIN_YEAR").val(), MISSION_ID: mission, PERSON_CODE: details.PERSON_CODE, per: $("#user_name").val() };
        openConfirmDialog_m2m(url, data, gridId.id, $('#ABSCENCE_CATEGORY_ID').val());

    }
    else if (gridId.id == "dt_mission_member") {
        var url = ControllerName + 'Delete_OFF';
        var data = { TRAINING_PERIOD_ID: period_id, FIRM_CODE: firm_cod, FIN_YEAR: $("#TXT_FIN_YEAR").val(), MISSION_ID: mission, PERSON_CODE: details.PERSON_CODE, per: $("#user_name").val() };
        openConfirmDialog_m2m(url, data, gridId.id, details.OFF_ABS_GROUP_ID);
    }

    else if (gridId.id == "off_ROLE") {
        var url = ControllerName + 'Delete_OFF_det';
        var data = { TRAINING_PERIOD_ID: period_id, FIRM_CODE: firm_cod, FIN_YEAR: $("#TXT_FIN_YEAR").val(), MISSION_ID: mission, PERSON_CODE: mission_per_cod, per: $("#user_name").val(), mission_dett: details.FIRM_MISSIONS_DET_ID };
        openConfirmDialog_m2m(url, data, gridId.id, details.OFF_ABS_GROUP_ID);
    }

}
function openConfirmDialog_m2m(url, data1, gridId, id) {
    $("#dialog-confirm").dialog({
        autoOpen: false,
        resizable: false,
        height: 'auto',
        width: 'auto',
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        buttons: {
            "اوافق": function () {
                $.ajax({
                    url: url,
                    type: 'POST',
                    data: data1,
                    dataType: 'json',
                    success: function (data) {

                        
                        if (data.status) {
                            swal({
                                title: "تم الحذف",
                                text: "تم  الحذف بنجاح",
                                type: "success",
                                timer: 2200
                            });
                            if (gridId == "dt_firm_missions") {

                                var xxx = $("#DT_DATE").val();
                                eval(gridId + "_Refresh(" + firm_cod + ",'" + $("#TXT_FIN_YEAR").val() + "'," + period_id + ",'" + xxx + "'," + $("#user_name").val() + "," + mission + ")");
                                eval("dt_mission_member_Refresh(" + firm_cod + ",'" + $("#TXT_FIN_YEAR").val() + "'," + period_id + ",'" + xxx + "'," + $("#user_name").val() + "," + mission + ")");

                                $('#dt_mission_member').jqxGrid('clear');
                                $('#jqxTabs').jqxTabs({ selectedItem: 0 });

                                $("#dialog-confirm").dialog('close');
                            }
                            if (gridId == "dt_mission_member") {

                                var xxx = $("#DT_DATE").val();
                                eval(gridId + "_Refresh(" + firm_cod + ",'" + $("#TXT_FIN_YEAR").val() + "'," + period_id + ",'" + xxx + "'," + $("#user_name").val() + "," + mission + ")");
                                $("#dialog-confirm").dialog('close');
                            }
                            if (gridId == "off_ROLE") {

                                //   var xxx = $("#DT_DATE").val();
                                eval(gridId + "_Refresh(" + firm_cod + ",'" + $("#TXT_FIN_YEAR").val() + "'," + period_id + "," + mission_per_cod + ")");
                                $("#dialog-confirm").dialog('close');
                            }
                            //    $('#OFF_ABS_GROUP_NAME').val('');
                            //  AG_SECTORS_Refresh('AG_SECTORS');
                            //alert(data.message);
                            //  $("#dialog-edit").dialog('close');
                            // alert("saad");
                        }
                        else {
                            swal({
                                title: "خطا",
                                text: "  " + data.message + "",
                                type: "error",
                                timer: 2200
                            });

                            $("#dialog-confirm").dialog('close');
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

            },
            "لا اوافق": function () {
                $(this).dialog("close");

            }
        }
    });
    $("#dialog-confirm").dialog('open');
    return false;
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

function off_rep() {
    debugger;
    $('#popUpDiv').show(1000);
    $('#overlayDiv').show(1000);
    var xxx = "../WORKFLOW_APP/REPORTS/Rep_gwab_mam/TalabM2morya.aspx?MISSION=" + mission + "& FIRM_CODE = " + fc;
    document.getElementById("show_rep").src = "../WORKFLOW_APP/REPORTS/Rep_gwab_mam/TalabM2morya.aspx?MISSION=" + mission + "&FIRM_CODE=" + fc;
    $('#show_rep').toggle('show');
}