
var ControllerName = ApplicationName + '/OFF_TAMMAM_AFRAD/';
var data_row;
var data_row1;
var group_id
//var firms = '1402102001';
$(document).ready(function () {

    build_static();
    build_TAMAMM_grid();
    get_person_data();
    BuildDropDwon1('FIRMS_CODE', 'FIRM_CODE', 'NAME', ApplicationName + '/GROUPS/firms_ddl', firm_cod);
    get_notif();
    seet_fin_year();
    bnd_TAMAMM_grid(firm_cod, $('#DT_DATE').val())
    document.getElementById('st_TAMAMM_gridDiv&add').style.display = 'inline-block';

    $('#REP').on('click', function (event) {
        //   window.open('../REPORTS/REP_TAMMAM/REPORT_PAGE.aspx?FIRM_CODE=' + firm_cod, '_blank');

        $('#popUpDiv').show(1000);
        $('#overlayDiv').show(1000);
        off_rep();
        $('#jqxTabs').jqxTabs({ selectedItem: 0 });
    });

    $('#st_TAMAMM_gridDiv').on('rowselect', function (event) {

        var data = $('#st_TAMAMM_gridDiv').jqxGrid('getrowdata', event.args.rowindex);
        off_name = data.PERSON_NAME;
        off_Id = data.PERSON_CODE;
        //$("#dialog-edit2").dialog('close');
        $('#ddl_name_txt').val(off_name);
        data_row1 = { PERSON_CODE: data.PERSON_CODE, FIN_YEAR: data.FIN_YEAR, TRAINING_PERIOD_ID: data.TRAINING_PERIOD_ID, FIRM_CODE: data.FIRM_CODE, FROM_DATE: data.FROM_DATE, ABSENCE_TYPE_ID: data.ABSENCE_TYPE_ID, RANK_CAT_ID: data.RANK_CAT_ID, PERSON_CAT_ID: data.PERSON_CAT_ID };

    });

    $('#DT_DATE').on('change', function (event) {
        //  mission = -1;
        $('#jqxTabs').jqxTabs({ selectedItem: 0 });
        $('#st_TAMAMM_gridDiv').jqxGrid('clear');
        bnd_TAMAMM_grid(firm_cod, $('#DT_DATE').val())

    });

    $('#btn_edit').on('click', function (event) {

        build_Edit_time_grid();
        bnd_Edit_time_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), $("#user_name").val());
        //bnd_ROLE_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $("#user_name").val(), mission);
    });


    $("#st_edit_tam").on('cellendedit', function (event) {

        var column = args.datafield;
        var row = args.rowindex;
        //debugger;
        var rowdata = $('#st_edit_tam').jqxGrid('getrowdata', row);
        var value = args.value;
        var oldvalue = args.oldvalue;
        rowdata[column] = value;
        // var cntl = event.args.owner.editcell.editor;
        // var pric = rowdata["LPRICE"] != "" ? rowdata["LPRICE"] : 0;
        // var p1 = rowdata["P1"] != "" ? rowdata["P1"] : 0;
        // var p2 = rowdata["P2"] != "" ? rowdata["P2"] : 0;
        // var p3 = rowdata["P3"] != "" ? rowdata["P3"] : 0;

        if (column == "TIME" || column == "EXIT_DATE" || column == "IS_LINE") {
            if (column == "TIME") {
                var rowdata = $('#st_edit_tam').jqxGrid('getrowdata', row);

                var data = $('#st_edit_tam').jqxGrid('getrowdata', event.args.rowindex);


                data = {
                    FIRM_CODE: firm_cod, FIN_YEAR: $("#TXT_FIN_YEAR").val(),
                    PERSON_CODE: data.PERSON_CODE, FROM_DATE: $('#DT_DATE').val(), per: $("#user_name").val(),
                    EXIT_DATE: data.EXIT_DATE, ENTRY_DATE: value, TIM: value,

                    Command: "add_officer_fun_TIM"
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
                            bnd_Edit_time_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), $("#user_name").val());
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
                            bnd_Edit_time_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), $("#user_name").val());
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
            else if (column == "EXIT_DATE") {
                // var b = ["", edit_c[1], "", ""]



                //   $("#BOS_HF").val(b);




                var rowdata = $('#st_edit_tam').jqxGrid('getrowdata', row);

                var data = $('#st_edit_tam').jqxGrid('getrowdata', event.args.rowindex);


                data = {
                    FIRM_CODE: firm_cod, FIN_YEAR: $("#TXT_FIN_YEAR").val(),
                    PERSON_CODE: data.PERSON_CODE, FROM_DATE: $('#DT_DATE').val(), per: $("#user_name").val(),
                    EXIT_DATE: value, ENTRY_DATE: data.ENTRY_DATE, TIM: value,

                    Command: "add_officer_fun_TIM"
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
                            bnd_Edit_time_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), $("#user_name").val());
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
                            bnd_Edit_time_grid(firm_cod, $("#TXT_FIN_YEAR").val(), period_id, $('#DT_DATE').val(), $("#user_name").val());
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
                text: "  خطأ",
                type: "error",
                timer: 2200
            });
        }

    });


});

function st_TAMAMM_gridDiv_Refresh1(firm_cod, date) {
    debugger
    $('#st_TAMAMM_gridDiv').jqxGrid('clearselection');
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
      { name: 'PERSON_CAT_ID' },
      { name: 'ABSENCE_STATUS' },
      { name: 'ABSENCE_TYPE_ID' },
      { name: 'CATEGORY_ID' },
      { name: 'SORT_NO' },
      { name: 'CURRENT_RANK_DATE' },
   { name: 'ABSENCE_NAME' }

        ],
        async: false,

        url: 'OFF_TAMMAM_AFRAD/GET_grid_m2m_mem1',
        data: { firm: firm_cod, date: date }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_TAMAMM_gridDiv").jqxGrid({ source: dataAdapter });
}

function build_static() {
    // $("#DT_DATE").jqxDateTimeInput({ formatString: "T", showTimeButton: true, showCalendarButton: false, width: '200px', height: '25px', theme: 'redmond' });
    $('#DT_DATE').jqxDateTimeInput({ animationType: 'fade', width: '80%', height: 38, rtl: true, dropDownHorizontalAlignment: 'right', theme: "darkblue" });
    $('#TXT_FIN_YEAR').jqxInput({ width: '80%', height: 38, theme: 'darkblue', disabled: true });
    $('#TXT_PERIOD').jqxInput({ width: '80%', height: 38, theme: 'darkblue', disabled: true });
    $('#page_name').text('  تمام الأفــراد');
    $('#jqxTabs').jqxTabs({ width: '80%', height: 500, rtl: true, theme: 'darkblue' });
    // $('#jqxTabs').jqxTabs({ width: '80%', height: 300, rtl: true, theme: 'darkblue' });
    // $('#txt_mission_introduction').jqxInput({ width: '85%', height: 70, theme: 'darkblue', disabled: true });
    // $('#txt_mission_subject').jqxInput({ width: '85%', height: 70, theme: 'darkblue', disabled: true });
    //  $('#txt_mission_distribution').jqxInput({ width: '85%', height: 70, theme: 'darkblue', disabled: true });

}
function build_TAMAMM_grid() {


    gridId = 'st_TAMAMM_gridDiv';

    theme = "darkblue";
    headerText = "تمام الأفــراد";
    gridAddUrl = ControllerName + 'Create';
    $("#st_TAMAMM_gridDiv").jqxGrid({
        width: '99%',
        height: 450,
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
                                          { text: 'كود الرتبة', dataField: 'RANK_ID', cellsalign: 'center', align: 'center', width: '10%', hidden: true },
                               { text: 'الرتبة', dataField: 'RANK', cellsalign: 'center', align: 'center', width: '10%' },//
                               { text: 'الاسم', dataField: 'PERSON_NAME', cellsalign: 'center', align: 'center', width: '20%' },//
                               { text: 'التمام', dataField: 'ABSENCE_NAME', cellsalign: 'center', align: 'center', width: '10%' },//
                               { text: 'كود الوحدة', dataField: 'FIRM_CODE', cellsalign: 'center', align: 'center', width: 90, hidden: true },
                               { text: 'من', dataField: 'FROM_DATE', cellsalign: 'center', align: 'center', width: '15%' },//
                               { text: 'الي', dataField: 'ACT_DATE', cellsalign: 'center', align: 'center', width: '15%' },//
                               { text: 'الجهة', dataField: 'ABSENCE_NOTES', cellsalign: 'center', align: 'center', width: '15%' },//
                               { text: 'الرقم العسكري', dataField: 'ID_NO', cellsalign: 'center', align: 'center', width: 250, hidden: true },
                               { text: 'الوظيفة', dataField: 'PERSON_CODE', cellsalign: 'center', align: 'center', width: 250, hidden: true },
                               { text: 'تاريخ الميلاد', dataField: 'FIN_YEAR', cellsalign: 'center', align: 'center', width: 110, hidden: true },
                               { text: 'المحافظة', dataField: 'TRAINING_PERIOD_ID', cellsalign: 'center', align: 'center', width: 150, hidden: true },
                               { text: 'فصيلة الدم', dataField: 'RANK_CAT_ID', cellsalign: 'center', align: 'center', width: 150, hidden: true },
                               { text: 'فصيلة الدم', dataField: 'PERSON_CAT_ID', cellsalign: 'center', align: 'center', width: 150, hidden: true },
                               { text: 'الديانة', dataField: 'ABSENCE_STATUS', cellsalign: 'center', align: 'center', width: 112, hidden: true },
                               {

                                   text: 'تعديل', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {

                                       return " <img  style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/edit.png' onclick='open_edit(" + row + ",st_TAMAMM_gridDiv)'/>";
                                   }
                               },
                                         {
                                             text: 'قطع', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {

                                                 return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/ok.png' onclick='open_edit1(" + row + ",st_TAMAMM_gridDiv)'/>";
                                             }
                                         },
                  {
                      text: 'حذف', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                          return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/delete.png' onclick='open_confirm(" + row + ",st_TAMAMM_gridDiv)'/>";
                      }
                  }

        ]

    });
    $("#st_TAMAMM_gridDiv").jqxGrid({ enabletooltips: true });
}
function bnd_TAMAMM_grid(firm_cod, date) {

    $('#st_TAMAMM_gridDiv').jqxGrid('clearselection');
    $('#st_TAMAMM_gridDiv').jqxGrid('clear');
    var source = {
        datatype: "json",
        datafields: [
      { name: 'RANK_ID' },
      { name: 'RANK' },
      { name: 'PERSON_NAME' },
      { name: 'FIRM_CODE' },
      { name: 'FROM_DATE' },
      { name: 'TO_DATE' },
        { name: 'ACT_DATE' },
      { name: 'ABSENCE_NOTES' },
      { name: 'ID_NO' },
      { name: 'PERSON_CODE' },
      { name: 'COMMANDER_FLAG' },
      { name: 'FIN_YEAR' },
      { name: 'TRAINING_PERIOD_ID' },
      { name: 'RANK_CAT_ID' },
      { name: 'PERSON_CAT_ID' },
      { name: 'ABSENCE_STATUS' },
      { name: 'ABSENCE_TYPE_ID' },
      { name: 'CATEGORY_ID' },
      { name: 'SORT_NO' },
      { name: 'CURRENT_RANK_DATE' },
   { name: 'ABSENCE_NAME' }

        ],
        async: false,

        url: 'OFF_TAMMAM_AFRAD/GET_grid_m2m_mem',
        data: { firm: firm_cod, date: date }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_TAMAMM_gridDiv").jqxGrid({ source: dataAdapter });
}

function bnd_TAMAMM_grid_AFRAD(firm_cod, date) {

    $('#st_TAMAMM_gridDiv').jqxGrid('clearselection');
    $('#st_TAMAMM_gridDiv').jqxGrid('clear');
    var source = {
        datatype: "json",
        datafields: [
      { name: 'RANK_ID' },
      { name: 'RANK' },
      { name: 'PERSON_NAME' },
      { name: 'FIRM_CODE' },
      { name: 'FROM_DATE' },
      { name: 'TO_DATE' },
        { name: 'ACT_DATE' },
      { name: 'ABSENCE_NOTES' },
      { name: 'ID_NO' },
      { name: 'PERSON_CODE' },
      { name: 'COMMANDER_FLAG' },
      { name: 'FIN_YEAR' },
      { name: 'TRAINING_PERIOD_ID' },
      { name: 'RANK_CAT_ID' },
      { name: 'PERSON_CAT_ID' },
      { name: 'ABSENCE_STATUS' },
      { name: 'ABSENCE_TYPE_ID' },
      { name: 'CATEGORY_ID' },
      { name: 'SORT_NO' },
      { name: 'CURRENT_RANK_DATE' },
   { name: 'ABSENCE_NAME' }

        ],
        async: false,

        url: 'OFF_TAMMAM_AFRAD/GET_grid_m2m_mem',
        data: { firm: firm_cod, date: date }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_TAMAMM_gridDiv").jqxGrid({ source: dataAdapter });
}

function build_Edit_time_grid() {


    gridId = 'st_edit_tam';

    theme = "darkblue";
    headerText = "ادخال التوقيتات ";
    gridAddUrl = ControllerName + 'Create';
    $("#st_edit_tam").jqxGrid({
        width: '99%',
        height: 450,
        //pageable: true,
        theme: "darkblue",
        sortable: true,
        rtl: true,
        editable: true,
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
                                          { text: 'كود الرتبة', dataField: 'RANK_ID', cellsalign: 'center', align: 'center', width: '10%', hidden: true },
                               { text: 'الرتبة', dataField: 'RANK', cellsalign: 'center', editable: false, align: 'center', width: '15%' },//
                               { text: 'الاسم', dataField: 'PERSON_NAME', cellsalign: 'center', editable: false, align: 'center', width: '55%' },//
                               { text: 'سعت الدخول', dataField: 'TIME', cellsalign: 'center', editable: true, align: 'center', width: '15%' },//
                               { text: 'كود الوحدة', dataField: 'FIRM_CODE', cellsalign: 'center', align: 'center', editable: false, width: 90, hidden: true },
                               { text: 'سعت الخروج', dataField: 'EXIT_DATE', cellsalign: 'center', editable: true, align: 'center', width: '15%' },//
                              // { text: 'ال', dataField: 'TO_DATE', cellsalign: 'center', align: 'center', width: '10%' },//

                  //             {

                  //                 text: 'تعديل', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {

                  //                     return " <img  style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/edit.png' onclick='open_edit(" + row + ",st_edit_tam)'/>";
                  //                 }
                  //             },
                  //{
                  //    text: 'حذف', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                  //        return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/delete.png' onclick='open_confirm(" + row + ",st_edit_tam)'/>";
                  //    }
                  //}

        ]

    });
    $("#st_edit_tam").jqxGrid({ enabletooltips: true });
}
function bnd_Edit_time_grid(firm_code, fin_year, PERIOD_ID, date, person_id) {

    $('#st_edit_tam').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
                        { name: 'RANK' },
                   { name: 'PERSON_NAME' },
                   { name: 'PERSON_CODE' },
                   { name: 'FROM_DATE' },
                   { name: 'FIRM_CODE' },
                   { name: 'ENTRY_DATE' },
                   { name: 'TIME' },
                   { name: 'ENTRY_DATE1' },
                          { name: 'EXIT_DATE' },
                           { name: 'EXIT_DATE1' }


        ],
        async: false,

        url: 'OFF_TAMMAM_AFRAD/GET_grid_OFF_TIM',
        data: { firm_code: firm_code, fin_year: fin_year, P: PERIOD_ID, date: date, person_id: person_id }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_edit_tam ").jqxGrid({ source: dataAdapter });
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
function open_edit(row, gridId) {
    
    var details = $('#' + gridId.id).jqxGrid('getrowdata', row);
    if (gridId.id == "st_TAMAMM_gridDiv") {

        var url = ControllerName + 'Edit/' + details.PERSON_CODE;
        //  var data = { PERSON_CODE: details.PERSON_CODE, FIN_YEAR: details.FIN_YEAR, TRAINING_PERIOD_ID: details.TRAINING_PERIOD_ID, FIRM_CODE: details.FIRM_CODE, FROM_DATE: details.FROM_DATE, ABSENCE_TYPE_ID: details.ABSENCE_TYPE_ID, RANK_CAT_ID: details.RANK_CAT_ID, PERSON_CAT_ID: details.PERSON_CAT_ID };

    } else if (gridId.id == "st_group__steps_gridDiv") {
        var url = ControllerName + 'Edit_Steps/' + details.OFF_ABS_STEPS_ID;
        var data = { id: details.OFF_ABS_STEPS_ID, id1: details.OFF_ABS_GROUP_ID };
    }

    openDialog_parm(url, data_row1, "تعديل");
}
function open_confirm(row, gridId) {
    
    var details = $('#' + gridId.id).jqxGrid('getrowdata', row);
    if (gridId.id == "st_TAMAMM_gridDiv") {
        var url = ControllerName + 'Delete';
        //  var data = { PERSON_CODE: details.PERSON_CODE, FIN_YEAR: details.FIN_YEAR, TRAINING_PERIOD_ID: details.TRAINING_PERIOD_ID, FIRM_CODE: details.FIRM_CODE, FROM_DATE: details.FROM_DATE, TO_DATE: details.TO_DATE, ABSENCE_TYPE_ID: details.ABSENCE_TYPE_ID, RANK_CAT_ID: details.RANK_CAT_ID, PERSON_CAT_ID: details.PERSON_CAT_ID };
        openConfirmDialog_m2m(url, data_row1, gridId.id);

    }
    else if (gridId.id == "dt_mission_member") {
        var url = ControllerName + 'Delete_OFF';
        var data = { TRAINING_PERIOD_ID: period_id, FIRM_CODE: firm_cod, FIN_YEAR: $("#TXT_FIN_YEAR").val(), MISSION_ID: mission, PERSON_CODE: details.PERSON_CODE, per: $("#user_name").val() };
        openConfirmDialog_m2m(url, data, gridId.id, details.OFF_ABS_GROUP_ID);
    }

    else if (gridId.id == "off_ROLE") {
        var url = ControllerName + 'Delete_OFF_det';
        var data = { TRAINING_PERIOD_ID: period_id, FIRM_CODE: firm_cod, FIN_YEAR: $("#TXT_FIN_YEAR").val(), MISSION_ID: mission, PERSON_CODE: details.PERSON_CODE, per: $("#user_name").val(), mission_dett: mission_det };
        openConfirmDialog_m2m(url, data, gridId.id, details.OFF_ABS_GROUP_ID);
    }

}
function openConfirmDialog_m2m(url, data1, gridId) {
    
    $("#dialog-confirm").dialog({
        autoOpen: false,
        resizable: false,
        height: 'auto',
        width: 'auto',
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        buttons: {
            "أوافق": function () {
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
                            if (gridId == "st_TAMAMM_gridDiv") {

                                var xxx = $("#DT_DATE").val();
                                bnd_TAMAMM_grid_AFRAD(firm_cod, xxx)
                               // eval(gridId + "_Refresh1(" + firm_cod + ",'" + xxx + "')");
                                //  eval("dt_mission_member_Refresh(" + firm_cod + ",'" + $("#TXT_FIN_YEAR").val() + "'," + period_id + ",'" + xxx + "'," + $("#user_name").val() + "," + mission + ")");
                                $("#dialog-confirm").dialog('close');
                                
                            }


                           
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
            "لا أوافق": function () {
                $(this).dialog("close");

            }
        }
    });
    $("#dialog-confirm").dialog('open');
    return false;
}
function build_gehat_grid() {


    gridId = 'gehat_grid';

    theme = "darkblue";
    headerText = " الجهات";
    gridAddUrl = ControllerName + 'Create';
    $("#gehat_grid").jqxGrid({
        width: '98%',
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
function open_edit1(row, gridId) {
    debugger
    var details = $('#' + gridId.id).jqxGrid('getrowdata', row);
    if (gridId.id == "st_TAMAMM_gridDiv") {
        var url = ControllerName + 'Edit_CUT/' + details.PERSON_CODE;
        // var data = { PERSON_CODE: details.PERSON_CODE, FIN_YEAR: details.FIN_YEAR, TRAINING_PERIOD_ID: details.TRAINING_PERIOD_ID, FIRM_CODE: details.FIRM_CODE, FROM_DATE: details.FROM_DATE, ABSENCE_TYPE_ID: details.ABSENCE_TYPE_ID, RANK_CAT_ID: details.RANK_CAT_ID, PERSON_CAT_ID: details.PERSON_CAT_ID };

    } else if (gridId.id == "st_group__steps_gridDiv") {
        var url = ControllerName + 'Edit_Steps/' + details.OFF_ABS_STEPS_ID;
        var data = { id: details.OFF_ABS_STEPS_ID, id1: details.OFF_ABS_GROUP_ID };
    }

    openDialog_parm(url, data_row1, "  قطع التمام");
}

function off_rep() {
    if ($('#DT_DATE').val() == '') { alert('إدخل التاريخ ') } else
    {
        document.getElementById("show_rep").src = "../WORKFLOW_APP/REPORTS/REP_TAMMAM/REPORT_PAGE.aspx?FIRM_CODE=" + firm_cod;
        $('#show_rep').toggle('show');
    }
}
