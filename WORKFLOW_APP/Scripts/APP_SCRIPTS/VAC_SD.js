var ApplicationName = "/../WORKFLOW_APP"
var gridAddUrl = '';
var gridId = 'VAC_GRD';
var ControllerName = ApplicationName + '/SD_VAC/';
var data_row;
var vacation;
var firms;

$(document).ready(function () {

    get_person_data();
    var d = new Date(new Date().getFullYear(), 0, 1);
    var d2 = new Date(new Date(new Date().getFullYear()+1, 0, 1) - 1);
    $('#FROM_DATE').jqxDateTimeInput({ animationType: 'fade', width: '120', height: 30, rtl: true, dropDownHorizontalAlignment: 'right', theme: "darkblue", value: d });
    $('#TO_DATE').jqxDateTimeInput({ animationType: 'fade', width: '120', height: 30, rtl: true, dropDownHorizontalAlignment: 'right', theme: "darkblue", value: d2 });
    BLD_DD("TXT_FIRM_CODE", "../WORKFLOW_APP/Vacation/GET_UNT", 300, 30, "darkblue", 'FIRM_CODE', 'NAME', 0);
    bld_vac_grd_sd();
    bnd_vac_grd_sd();
    BLD_V_TOT();
    BLD_ROL_GRD();
    $('#VAC_GRD').on('rowselect', function (event) {
        var args = event.args;
        var row = args.rowindex;
        var data = $('#VAC_GRD').jqxGrid('getrowdata', row);
        vacation = data.SEQ;
        BND_V_TOT(data.PERSON_CODE);
        person_c = data.PERSON_CODE + ',' + data.RANK_ID + ',' + data.RANK_CAT_ID + ',' + data.PERSON_CAT_ID + ',' + data.RANK + ',' + data.PERSON_NAME + ',' + data.ADDRESS + ',' + data.PERSONAL_ID_NO;
        BND_ROL_GRD(data.FIRM_CODE, data.PERSONAL_ID_NO, data.SEQ);
    });
    $("#VAC_ROLE").on('cellendedit', function (event) {
        var column = args.datafield;
        var row = args.rowindex;
        var value = args.value;
        var oldvalue = args.oldvalue;
        var rowdata = $('#VAC_ROLE').jqxGrid('getrowdata', row);
        var data = $('#VAC_ROLE').jqxGrid('getrowdata', event.args.rowindex);
        if (value != oldvalue && value != 'إختر') {

            if (column == "PERSON_NAME") {
                var b = ["", edit_c[1], "", ""];


                data = {
                    PERSON_VACATIONS_SEQ: vacation,
                    OFF_ABS_STEPS_ID: data.OFF_ABS_STEPS_ID,
                    OFF_ABS_GROUP_ID: data.OFF_ABS_GROUP_ID,
                    PERSON_DATE_ID: data.PERSON_DATE_ID,
                    per: $("#user_name").val(),
                    PERSON_DATE_OWEN: b[1],
                    step_name: data.OFF_ABS_STEPS_NAME,
                    PERSON_VACATIONS_DET_ID: data.PERSON_VACATIONS_DET_ID,

                    Command: "add_officer_fun"
                }

                $.ajax({
                    url: ControllerName + data.Command,
                    type: 'POST',
                    data: data,
                    dataType: 'json',
                    success: function (data) {
                        if (data.status) {
                            BND_ROL_GRD(firm, person_c.split(',')[7], rowdata.PERSON_VACATIONS_SEQ);
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

    $("#off_ROLE").on("cellclick", function (event) {

        var column = event.args.column;
        var row = event.args.rowindex;
        var columnindex = event.args.columnindex;
        var rowdata = $('#off_ROLE').jqxGrid('getrowdata', row);
    });
    $("#TO_DATE").on("change", function (event) {
        var jsDate = event.args.date;
        bnd_vac_grd_sd();
    });
    $("#FROM_DATE").on("change", function (event) {
        var jsDate = event.args.date;
        bnd_vac_grd_sd();
    });
    firms = firm;
    
});

function get_off(data) {
    $('#dialog-edit').dialog('close');
    person_c = data.PERSON_CODE + ',' + data.RANK_ID + ',' + data.RANK_CAT_ID + ',' + data.PERSON_CAT_ID + ',' + data.ADDERESS;
    setTimeout(openDialog_parm, 1000, '../WORKFLOW_APP/SD_VAC/Create_VAC', { pers: data.PERSON_CODE, firm: data.FIRM_CODE }, 'إضافة');      //, function (D) { setTimeout(function () { $('#off_detail').text(data.PERSON_NAME); }, 3000); }, function (e) { }
    setTimeout(function (D) { $('#off_detail').text(data.RANK + " / " + data.PERSON_NAME); }, 1500);
    

};

function open_edit_sd(row, gridId) {
    var details = $('#' + gridId.id).jqxGrid('getrowdata', $('#' + gridId.id).jqxGrid('selectedrowindex'));
    if (gridId.id == "VAC_GRD") {
        var url = '../WORKFLOW_APP/sd_vac/Edit'; // /' + details.SEQ + '/' + details.FIRM_CODE
        var data = { id: details.SEQ, firm: details.FIRM_CODE, pers: details.PERSON_CODE };
    } 

    openDialog_parm(url, data, "تعديل");
}

function open_confirm_sd(row, gridId, f) {

    var details = $('#' + gridId.id).jqxGrid('getrowdata', $('#' + gridId.id).jqxGrid('selectedrowindex'));
    if (gridId.id == "VAC_GRD" && f == 1) {
        var url = '../WORKFLOW_APP/sd_vac/Delete';
        var data = { id: details.SEQ, firm: details.FIRM_CODE, pers: details.PERSON_CODE };
        open_del(url, data, gridId, "BND_ROL_GRD('" + details.FIRM_CODE + "'," + details.PERSONAL_ID_NO + "," + details.SEQ + ")");

    }
    else if (gridId.id == "VAC_GRD" && f == 2) {
        var url = '../WORKFLOW_APP/sd_vac/del_vac';
        var data = { id: details.SEQ, firm: details.FIRM_CODE, pers: details.PERSON_CODE };
        open_del(url, data, gridId, "BND_ROL_GRD('" + details.FIRM_CODE + "'," + details.PERSONAL_ID_NO + "," + details.SEQ + ")");
    }

    else if (gridId.id == "st_group__person_gridDiv") {
        var url = ControllerName + 'Delete_OFF';
        var data = { id: details.OFF_ABS_GROUP_OFF_ID, id1: details.OFF_ABS_GROUP_ID };
        openConfirmDialog_parm(url, data, gridId.id, details.OFF_ABS_GROUP_ID);
    }

}

function getPtr(row) {
    var d = $('#VAC_GRD').jqxGrid('getrowdata', row);
    return d.PTR;
}

function bld_vac_grd_sd() {
    theme = "darkblue";
    disp = "inline-flex, none, inline-flex";
    headerText = "الأجازات";
    gridAddUrl = ControllerName + 'Create';
    w = '1000';
    h = '400';
    $("#VAC_GRD").jqxGrid({
        width: '100%',
        height: 300,
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
            { text: 'رتبــــة', dataField: 'R_NAME', cellsalign: 'center', align: 'center', width: '10%' },
            { text: 'إســـــم', dataField: 'PERSON_NAME', cellsalign: 'center', align: 'center', width: '15%' },
            { text: 'نوع الاجازة', dataField: 'VACATION_TYPE_ID_NAME', cellsalign: 'center', align: 'center', width: '10%' },
            { text: 'تاريخ تقديم الطلب', dataField: 'REQUEST_DATE', cellsalign: 'center', align: 'center', width: '10%' },
            { text: 'من', dataField: 'FROM_DATE', cellsalign: 'center', align: 'center', width: '10%', columngroup: 'agaza_mokdma', filtercondition: 'CONTAINS' },//
            { text: 'الي', dataField: 'TO_DATE', cellsalign: 'center', align: 'center', width: '10%', columngroup: 'agaza_mokdma', filtercondition: 'CONTAINS' },
            { text: 'من', dataField: 'ACTUAL_START', cellsalign: 'center', align: 'center', width: '10%', columngroup: 'agaza_mosdk', filtercondition: 'CONTAINS' },//
            { text: 'الي', dataField: 'ACTUAL_END', cellsalign: 'center', align: 'center', width: '10%', columngroup: 'agaza_mosdk', filtercondition: 'CONTAINS' },
            { text: 'قرار القائد', dataField: 'COMANDER_DECESION', cellsalign: 'center', align: 'center', filtertype: 'bool', width: '7%', columntype: 'checkbox' },
            {
                text: '', columntype: 'button', cellsalign: 'center', filterable: false, width: '4%', align: 'center', cellsrenderer: function (row, datafield, value) {
                    return "<img  style='margin-left: 5px;cursor:pointer' height='20' width='20' src='../WORKFLOW_APP/images/edit.png' onclick='open_edit_sd(" + row + ", VAC_GRD)'/>";
                }
            },
            {
                text: '', columntype: 'button', cellsalign: 'center', filterable: false, width: '4%', align: 'center', cellsrenderer: function (row, datafield, value) {
                    return "<img   style='margin-left: 5px;cursor:pointer' height='17' width='17' src='../WORKFLOW_APP/images/delete.png' onclick='open_confirm_sd(" + row + ", VAC_GRD, getPtr(" + row + "))'/>";
                }
            }

        ]
        ,
    columngroups: [
    { text: 'الاجازة المقدمة', align: 'center', name: 'agaza_mokdma' },
    { text: 'الاجازة المصدق عليها', align: 'center', name: 'agaza_mosdk' }
    ]

    });
    $("#VAC_GRD").jqxGrid({ enabletooltips: true });
    $('#VAC_GRD').jqxGrid('sortby', 'FROM_DATE', 'desc');
}

function bnd_vac_grd_sd() {
    var d1 = $('#FROM_DATE').val();
    var d2 = $('#TO_DATE').val();
    $("#VAC_GRD").jqxGrid('clearselection');
    $("#VAC_GRD").jqxGrid('clear');
    var source = {
        datatype: "json",
        datafield : [
              { name: 'SEQ' },
              { name: 'FIRM_CODE' },
              { name: 'PERSON_CODE' },
              { name: 'RANK_ID' },
              { name: 'RANK_CAT_ID' },
              { name: 'PERSON_CAT_ID' },
              { name: 'VACATION_TYPE_ID' },
              { name: 'REQUEST_DATE', type: 'text' },
              { name: 'FROM_DATE', type: 'text' },
              { name: 'TO_DATE', type: 'text' },
              { name: 'OTHER_PERSON_CODE' },
              { name: 'OTHER_PERSON_NAME' },
              { name: 'OTHER_PER_DECS' },
              { name: 'RAN_RANK_ID' },
              { name: 'R_NAME' },
              { name: 'RAN_RANK_CAT_ID' },
              { name: 'RAN_PERSON_CAT_ID' },
              { name: 'SUPERVISOR_CODE' },
              { name: 'PERSON_NAME' },
              { name: 'SUPERVISOR_NOTES' },
              { name: 'SUPERVISOR_DECESION' },
              { name: 'PERSONAL_ID_NO' },
              { name: 'PLANNING_CODE' },
              { name: 'PLANNING_NOTES' },
              { name: 'PLANNING_DECESION' },
              { name: 'VICE_COMMAND_CODE' },
              { name: 'VICE_COMMAND_NOTES' },
              { name: 'VICE_COMMAND_DECESION' },
              { name: 'COMANDER_CODE' },
              { name: 'COMANDER_NOTES' },
              { name: 'COMANDER_DECESION' },
              { name: 'ACTUAL_START' },
              { name: 'ACTUAL_END' },
              { name: 'APPROVED_BY' },
              { name: 'APPROVAL_NO' },
              { name: 'APPROVAL_DATE' },
              { name: 'ADDRESS' },
              { name: 'EXCHANGE_FOR_DATE' },
              { name: 'FLAG_PLAN' },
              { name: 'ADRS' },
              { name: 'VACATION_TYPE_ID_NAME' },
              { name: 'PTR' }

        ],
        async: false,
        url: '../WORKFLOW_APP/SD_VAC/get_vac',
        data: {
            DFRM: d1,
            DTO: d2,
            ID: pid,
            FIRM: firm
        }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#VAC_GRD").jqxGrid({ source: dataAdapter });
}

function VAC_GRD_Refresh() {
    if (window.location.href.contains("SD")) {
        bnd_vac_grd_sd();
    }
    else {
        bnd_vac_grd();
    }
}

function BLD_V_TOT() {
    $("#V_TOT_GRD").jqxGrid('clear');
    disp = "none, none, none";
    headerText = "الأجازات السابقة";
    $("#V_TOT_GRD").jqxGrid(
    {
        filterable: true,
        width: '100%',
        rtl: true,
        showfilterrow: true,
        showaggregates: false,
        showtoolbar: true,
        everpresentrowposition: "top",
        pageSize: 100,
        pageable: false,
        columnsResize: true,
        sortable: true,
        theme: 'darkblue',
        height: '300px',
        rowsheight: 35,
        filterrowheight: 35,
        rendertoolbar: toolbarfn1,
        columns: [
             { text: 'نوع الاجازة', dataField: 'NAME', cellsalign: 'center', align: 'center', width: '30%' },
              { text: 'النصف الأول', dataField: 'CNT1', cellsalign: 'center', align: 'center', width: '20%' },
                { text: 'المتبقي', dataField: 'CNT3', cellsalign: 'center', align: 'center', width: '15%' },
              { text: 'النصف الثانى', dataField: 'CNT2', cellsalign: 'center', align: 'center', width: '20%' },
                { text: 'المتبقي', dataField: 'CNT4', cellsalign: 'center', align: 'center', width: '15%' }

        ]
    });
};

function BND_V_TOT(PER) {
    var FRM = $('#TXT_FIRM_CODE').val();
    var source = {
        datatype: "json",
        datafield: [
                 { name: 'CNT1' },
                 { name: 'CNT2' },
                 { name: 'NAME' },
                 { name: 'CNT3' },
                 { name: 'CNT4' }

        ],
        async: false,
        url: '../WORKFLOW_APP/SD_VAC/get_vac_his',
        data: {
            PERS: PER,
            FIRM: FRM,
            //VFRM: $('#FROM_DATE').val(),
            //VTO: $('#TO_DATE').val()
        }
    }
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#V_TOT_GRD").jqxGrid({ source: dataAdapter });
};

function BLD_OFF_GRD() {
    
    theme = "darkblue";
    headerText = "ضباط";
    gridAddUrl = '../WORKFLOW_APP/GROUPS/Create_Off';
    $("#off_grid").jqxGrid({
        width: 500,
        height: 300,
        theme: "darkblue",
        sortable: true,
        rtl: true,
        showaggregates: true,
        filterable: true,
        showfilterrow: true,
        showtoolbar: true,
        rendertoolbar: toolbarfn,
        columns: [
            { text: 'كود المرحلة', dataField: 'PERSON_CODE', width: '10%', cellsalign: 'center', align: 'center', hidden: true },
            { text: 'كود المجموعه', dataField: 'OFF_ABS_GROUP_ID', width: '10%', cellsalign: 'center', align: 'center', hidden: true },
            { text: 'الرتبة ', dataField: 'RANK', width: '45%', cellsalign: 'center', align: 'center' },
            { text: ' الاسم', dataField: 'PERSON_NAME', width: '55%', cellsalign: 'center', align: 'center' }
        ]

    });
    $("#off_grid").jqxGrid({ enabletooltips: true });
};

function bld_off_all_grd() {
    BLD_OFF_GRD();
};

function bnd_off_all_grd(firms, value) {
    BND_OFF_GRD(firms, value);
};

function BND_OFF_GRD(GR, abs) {
        $('#off_grid').jqxGrid('clearselection');
        var source = {
            datatype: "json",
            datafields: [
                { name: 'PERSON_CODE' },
                { name: 'RANK_ID' },
                { name: 'RANK_CAT_ID' },
                { name: 'PERSON_NAME' },
                { name: 'RANK' },
                { name: 'PERSON_CAT_ID' },
                { name: 'ADDERESS' },
                { name: 'FIRM_CODE' }
            ],
            async: false,

            url: '../WORKFLOW_APP/GROUPS/GET_off_all',
            data: { firms: GR, rank_cat: abs }

        };

        var dataAdapter = new $.jqx.dataAdapter(source);
        $("#off_grid").jqxGrid({ source: dataAdapter });
};

function BLD_ROL_GRD() {

    gridId = 'VAC_ROLE';

    theme = "darkblue";
    headerText = " الخطوات";
    gridAddUrl = ControllerName + 'Create';
    $("#VAC_ROLE").jqxGrid({
        width: '100%',
        height: 300,
        theme: "darkblue",
        sortable: true,
        rtl: true,
        showaggregates: true,
        filterable: true,
        showfilterrow: true,
        showtoolbar: true,
        editable: true,
        rendertoolbar: toolbarfn,
        columns: [
                    { text: 'الترتيب', dataField: 'ORDER_ID', cellsalign: 'center', align: 'center', width: '10%' },
                    { text: 'اسم الخطوة', dataField: 'OFF_ABS_STEPS_NAME', cellsalign: 'center', align: 'center', width: '20%' },
                    { text: 'الوظيفة', dataField: 'JOB', cellsalign: 'center', align: 'center', width: '20%' },
                    { text: 'الرتبة', dataField: 'RANK', cellsalign: 'center', align: 'center', width: '15%' },
                    {
                        text: 'الاسم ', dataField: 'PERSON_NAME', cellsalign: 'center', align: 'center', width: '35%', columntype: 'combobox',
                        createeditor: function (row, cellvalue, editor, cellText, width, height) {
                            var ROL = $('#VAC_ROLE').jqxGrid('getrowdata', row).ROL;
                            var x = [];
                            x[0] = 1;
                            var source1 = {
                                datatype: "json",
                                datafields:
                                    [{ name: 'NM' },
                                    { name: 'PERSON_CODE' }],
                                async: false,

                                url: '../WORKFLOW_APP/SD_VAC/GET_ROL_OFF',
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

                            var rowdata = $('#VAC_ROLE').jqxGrid('getrowdata', row);
                            edit_c = [];
                            edit_c[0] = editor.text();
                            edit_c[1] = editor.val()
                            rowdata["PERSON_NAME"] = editor.text();
                            return editor.text();
                        }
                    }

        ]

    });
    $("#VAC_ROLE").jqxGrid({ enabletooltips: true });
}

function BND_ROL_GRD(firm_code,  person_id, vac) {

    $('#VAC_ROLE').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
                        { name: 'OFF_ABS_STEPS_ID' },
                   { name: 'OFF_ABS_GROUP_ID' },
                   { name: 'OFF_ABS_STEPS_NAME' },
                   { name: 'PERSON_VACATIONS_DET_ID' },
                   { name: 'ORDER_ID' },
                   { name: 'PERSON_DATE_ID' },
                   { name: 'PERSON_NAME' },
                   { name: 'PERSON_VACATIONS_SEQ' },
                   { name: 'RANK' },
                   { name: 'JOB' },
                   { name: 'ROL' }

        ],
        async: false,

        url: '../WORKFLOW_APP/SD_VAC/GET_V_ROLE',
        data: { firm_code: firm_code, person_id: person_id, VAC_ID: vac }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#VAC_ROLE").jqxGrid({ source: dataAdapter });
}

function print_rep(gridId) {
    var details = $('#' + gridId.id).jqxGrid('getrowdata', $('#' + gridId.id).jqxGrid('selectedrowindex'));
    var url = "REPORTS/Talab_Agaza/TalabAgaza.aspx?SEQ=" + details.SEQ + "&PERSON_CODE=" + details.PERSON_CODE + "&firm=" + details.FIRM_CODE+"&from_date=" + details.FROM_DATE;
    window.open(url, "_blank");
}



