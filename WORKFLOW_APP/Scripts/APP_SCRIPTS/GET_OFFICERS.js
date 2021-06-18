$(document).ready(function () {


});

function bld_off_all_grd() {
    
    theme = "darkblue";
    headerText = " ";
    gridAddUrl = ControllerName + 'Create_Off';
    $("#off_grid").jqxGrid({
        width: '98%',
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
                { name: 'RANK_ID' },
                { name: 'RANK_CAT_ID' },
                { name: 'PERSON_NAME' },
                { name: 'RANK' },
                { name: 'PERSON_CAT_ID' },
                { name: 'ADDERESS' },
                { name: 'FIRM_CODE' }
            ],
        async: false,

        url: 'GROUPS/GET_off_all_SAF',
        data: { firms: GR, rank_cat: abs }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#off_grid").jqxGrid({ source: dataAdapter });
}
function build_ROLE_grid() {
    
    gridId = 'off_ROLE';

    theme = "darkblue";
    headerText = " الخطوات";
    gridAddUrl = ControllerName + 'Create';
    $("#off_ROLE").jqxGrid({
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
        editable: true,
        //   selectionmode: 'singlerow',
        //source: dataAdapter,
        rendertoolbar: toolbarfn,
        columns: [
                                  { text: 'الترتيب', dataField: 'ORDER_ID', cellsalign: 'center', align: 'center', editable: false, width: '5%' },
                       { text: 'اسم الخطوة', dataField: 'OFF_ABS_STEPS_NAME', cellsalign: 'center', editable: false, align: 'center', width: '45%' },
                       //{ text: 'الرتبة', dataField: 'RANK', cellsalign: 'center', align: 'center', width: '15%' },


                                                 {
                                                     text: 'الاسم ', dataField: 'PERSON_NAME', cellsalign: 'center', align: 'center', editable: true, width: '45%', columntype: 'combobox',
                                                     createeditor: function (row, cellvalue, editor, cellText, width, height) {
                                                         
                                                         var prod = $('#off_ROLE').jqxGrid('getrowdata', row).NM;
                                                         var x = [];
                                                         x[0] = 1;
                                                         var source1 = {
                                                             datatype: "json",
                                                             datafields:
                                                                 [{ name: 'NM' },
                                                                 { name: 'PERSON_CODE' }],
                                                             async: false,
                                                            
                                                             url: '../TALAB_M2M/GET_off_role',
                                                             data: { firm_code: firm_cod, P: "1" }
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
                   { name: 'RANK' }

        ],
        async: false,

        url: 'TALAB_M2M/GET_grid_ROLE',
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

        url: 'TALAB_M2M/GET_grid_ROLE',
        data: { firm_code: firm_code, fin_year: fin_year, P: PERIOD_ID, person_id: person_id, mission: mission }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#off_ROLE").jqxGrid({ source: dataAdapter });
}