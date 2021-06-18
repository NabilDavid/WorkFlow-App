
var ControllerName = ApplicationName + '/groups/';
var data_row;
var group_id
var firms = firms;
$(document).ready(function () {
   
  //  var ControllerName = ApplicationName + '/groups/';
    //setTimeout(function () {
    //    var firm_cod = '1402102001';
    //    builddropdownlist1("SELECT FIRM_CODE, NAME FROM FIRMS WHERE FIRM_CODE = '" + firm_cod + "'", "FIRM_CODE,NAME", "FIRMS_CODE", "ui-myredmond", 300, 40, function () { $('#FIRMS_CODE').val(firm_cod); });
    //   // builddropdownlist1("SELECT ABSENCE_TYPE_ID, NAME FROM ABSENCE_TYPES where ABSCENCE_CATEGORY_ID = 1 AND  FOR_OFFICERS = 1 ORDER BY 1", "ABSENCE_TYPE_ID,NAME", "khdma_typ_dd", "ui-myredmond", 200, 40, function () { $('#khdma_typ_dd').val('16'); });
    //   // builddropdownlist1("SELECT DISTINCT(TO_CHAR (FROM_DATE, 'yyyy/MM')) AS DT,TO_CHAR (FROM_DATE, 'yyyy') AS YR,TO_CHAR (FROM_DATE, 'MM') AS MN FROM FIRMS_ABSENCES_PERSONS EX ORDER BY YR DESC,MN DESC", "DT,DT", "MN_YR_DD", "ui-myredmond", 200, 40, function () { $("#MN_YR_DD").jqxDropDownList({ selectedIndex: 0 }); });
    //  //  builddropdownlist1("SELECT ABSENCE_TYPE_ID, NAME FROM ABSENCE_TYPES where ABSCENCE_CATEGORY_ID = 1 AND  FOR_OFFICERS = 1 ORDER BY 1", "ABSENCE_TYPE_ID,NAME", "khdma_typ_dd1", "ui-myredmond", 200, 40, function () { });

    //    //build_grid_exch($('#TXT_FIRM_CODE').val(), $('#khdma_typ_dd').val(), $('#MN_YR_DD').val());

    //}, 1000);



    build_dept_grid();
    bld_steps_grd();
    bld_off_grd();
    get_person_data();
  //  bnd_step_grd(2)
  //  bind_group_grid('st_group_gridDiv');
    BuildDropDwon1('FIRMS_CODE', 'FIRM_CODE', 'NAME', ApplicationName + '/GROUPS/firms_ddl', firm_cod);
    BuildDropDwon('ABSCENCE_CATEGORY_ID', 'ABSCENCE_CATEGORY_ID', 'ABSCENCE_CATEGORY', ApplicationName + '/GROUPS/absc_cat');
  
    $('#page_name').text(' الصلاحيات');

    $('#st_group_gridDiv').on('rowselect', function (event) {
        var args = event.args;
        var row = args.rowindex;
        
        var data = $('#st_group_gridDiv').jqxGrid('getrowdata', row); 
        data_row = $('#st_group_gridDiv').jqxGrid('getrowid', row);
    //    $('#group_id').val(xx);
        // AG_VILLAGES_Refresh('AG_VILLAGES');
        bnd_step_grd(data.OFF_ABS_GROUP_ID)
        bnd_off_grd(data.OFF_ABS_GROUP_ID)
        $('#FIRMS_CODE').val(data.FIRM_CODE);
        group_id = data.OFF_ABS_GROUP_ID
        document.getElementById('st_group__person_gridDiv&add').style.display = 'inline-block';
        document.getElementById('st_group__steps_gridDiv&add').style.display = 'inline-block';

      
       // $('#OFF_ABS_GROUP_ID_DIV').attr('disabled', true)

    });



    $('#ABSCENCE_CATEGORY_ID').on('select', function (event) {
        var item = args.item;
        // get item's label and value.
        var label = item.label;
        var value = item.value;
        bind_group_grid(value)
        bnd_step_grd(0);
        bnd_off_grd(0)
        document.getElementById('st_group_gridDiv&add').style.display = 'inline-block';
    
        //document.getElementById('st_group_gridDiv_add').style.display = 'inline-block';
    });


    $('#off_grid').on('rowdoubleclick', function (event) {
        var data = $('#off_grid').jqxGrid('getrowdata', event.args.rowindex);
        
        data = {
            OFF_ABS_GROUP_ID: group_id, OFF_SKELETON_OFFICERS_ID: data.PERSON_CODE, ABS_CAT_ID: $('#ABSCENCE_CATEGORY_ID').val(), PERSON_DATA_ID: data.PERSON_CODE,

            Command: "Create_OFF"
        }

        $.ajax({
            url: ControllerName + data.Command,
            type: 'POST',
            data: data,
            dataType: 'json',
            success: function (data) {
                //  st_group_gridDiv_Refresh();
                bnd_step_grd(group_id);
                bnd_off_grd(group_id)
                if (data.status) {
                    swal({
                        title: "تم الاضافة",
                        text: "تم  الاضافة بنجاح",
                        type: "success",
                        timer: 2200
                    });

                    //    $('#OFF_ABS_GROUP_NAME').val('');
                    //  AG_SECTORS_Refresh('AG_SECTORS');
                    //alert(data.message);
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




    });

});

$(window).on('load', function () {
    $(".Loader").fadeOut("slow");;
});


function get_off(data) {
    data = {
        OFF_ABS_GROUP_ID: group_id, OFF_SKELETON_OFFICERS_ID: data.PERSON_CODE, ABS_CAT_ID: $('#ABSCENCE_CATEGORY_ID').val(), PERSON_DATA_ID: data.PERSON_CODE,

        Command: "Create_OFF"
    }

    $.ajax({
        url: ControllerName + data.Command,
        type: 'POST',
        data: data,
        dataType: 'json',
        success: function (data) {
            //  st_group_gridDiv_Refresh();
            bnd_step_grd(group_id);
            bnd_off_grd(group_id)
            if (data.status) {
                swal({
                    title: "تم الاضافة",
                    text: "تم  الاضافة بنجاح",
                    type: "success",
                    timer: 2200
                });

                //    $('#OFF_ABS_GROUP_NAME').val('');
                //  AG_SECTORS_Refresh('AG_SECTORS');
                //alert(data.message);
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
function st_group_gridDiv_Refresh(GR) {

    var source = {
        datatype: "json",
        datafields: [
        { name: 'OFF_ABS_GROUP_ID' },
            { name: 'NAME' },
             { name: 'FIRM_CODE' },
            { name: 'ABSCENCE_CATEGORY_ID' },
             { name: 'DEF_NAME' },
              { name: 'OFF_ABS_GROUP_NAME' },
               { name: 'ABSCENCE_CATEGORY' },
            { name: 'PN' }

        ],
        async: false,
        
        url: 'GROUPS/getgroupGrid',
        data: { GR: GR }
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_group_gridDiv").jqxGrid({ source: dataAdapter });
}
function st_group__steps_gridDiv_Refresh(GR) {

    var source = {
        datatype: "json",
        datafields: [
        { name: 'OFF_ABS_GROUP_ID' },
            { name: 'OFF_ABS_STEPS_ID' },
             { name: 'OFF_ABS_STEPS_NAME' },
              { name: 'OFF_ABS_GROUP_NAME' },
               { name: 'ARH_ROLE_NAME' },
            { name: 'ORDER_ID' }

        ],
        async: false,

        url: 'GROUPS/GET_STEPS',
        data: { GR: GR }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_group__steps_gridDiv").jqxGrid({ source: dataAdapter });
}
function st_group__person_gridDiv_Refresh(GR) {

    $('#st_group__person_gridDiv').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
        { name: 'OFF_ABS_GROUP_OFF_ID' },
            { name: 'OFF_ABS_GROUP_ID' },
             { name: 'OFF_SKELETON_OFFICERS_ID' },
              { name: 'PERSON_NAME' },

                { name: 'RANK' },
            { name: 'PERSON_DATA_ID' }

        ],
        async: false,

        url: 'GROUPS/GET_off',
        data: { GR: GR }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_group__person_gridDiv").jqxGrid({ source: dataAdapter });
}
function bind_group_grid(GR) {
    $('#st_group_gridDiv').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
        { name: 'OFF_ABS_GROUP_ID' },
        { name: 'NAME' },
            { name: 'FIRM_CODE' },
            { name: 'ABSCENCE_CATEGORY_ID' },
            { name: 'DEF_NAME' },
            { name: 'OFF_ABS_GROUP_NAME' },
             { name: 'ABSCENCE_CATEGORY' },
            { name: 'PN' }

        ],
        async: false,
    
        url: 'GROUPS/getgroupGrid',
        data: { GR: GR }
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_group_gridDiv").jqxGrid({ source: dataAdapter });
}
function build_dept_grid() {
    

    gridId = 'st_group_gridDiv';

    theme = "darkblue";
    headerText = " المجموعات";
    gridAddUrl = ControllerName + 'Create';
    $("#st_group_gridDiv").jqxGrid({
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
            { text: 'كود المجموعه', dataField: 'OFF_ABS_GROUP_ID', width: '10%', cellsalign: 'center', align: 'center' },
            { text: 'اسم المجموعه', dataField: 'OFF_ABS_GROUP_NAME', width: '25%', cellsalign: 'center', align: 'center' },
            { text: ' نوع التمام', dataField: 'ABSCENCE_CATEGORY', width: '25%', cellsalign: 'center', align: 'center' },
               { text: ' إسم الوحدة ', dataField: 'NAME', width: '20%', cellsalign: 'center', align: 'center' },
                  { text: ' الحالة ', dataField: 'DEF_NAME', width: '10%', cellsalign: 'center', align: 'center' },
                               {

                                   text: 'تعديل', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                                       return " <img  style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/edit.png' onclick='open_edit(" + row + ",st_group_gridDiv)'/>";
                                   }
                               },
                  {
                      text: 'حذف', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                          return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/delete.png' onclick='open_confirm(" + row + ",st_group_gridDiv)'/>";
                      }
                  }

        ]

    });
    $("#st_group_gridDiv").jqxGrid({ enabletooltips: true });
}
function bld_steps_grd() {
   
    gridId = 'st_group__steps_gridDiv';
    theme = "darkblue";
    headerText = " خطوات المجموعات";
    gridAddUrl = ControllerName + 'Create_Steps';
    $("#st_group__steps_gridDiv").jqxGrid({
        width: '98%',
        height: 230,
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
            { text: 'كود المرحلة', dataField: 'OFF_ABS_STEPS_ID', width: '10%', cellsalign: 'center', align: 'center', hidden: true },
            { text: 'كود المجموعه', dataField: 'OFF_ABS_GROUP_ID', width: '10%', cellsalign: 'center', align: 'center', hidden: true },
            //{ text: 'اسم المجموعه', dataField: 'OFF_ABS_GROUP_NAME', width: '25%', cellsalign: 'center', align: 'center' },
            { text: 'اسم المرحلة', dataField: 'OFF_ABS_STEPS_NAME', width: '35%', cellsalign: 'center', align: 'center' },
            { text: ' الوظيفة', dataField: 'ARH_ROLE_NAME', width: '25%', cellsalign: 'center', align: 'center' },
            { text: ' ترتيب المرحلة ', dataField: 'ORDER_ID', width: '20%', cellsalign: 'center', align: 'center' },
            {
                text: 'تعديل', columntype: 'button', cellsalign: 'center', filterable: false, width: '10%', align: 'center', cellsrenderer: function (row, datafield, value) {
                    return "<img  style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/edit.png' onclick='open_edit(" + row + ", st_group__steps_gridDiv)'/>";
                }
            },
            {
                text: 'حذف', columntype: 'button', cellsalign: 'center', filterable: false, width: '10%', align: 'center', cellsrenderer: function (row, datafield, value) {
                    return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15vh' width='25vw' src='images/delete.png' onclick='open_confirm(" + row + ", st_group__steps_gridDiv)'/>";
                }
            }

        ]

    });
    $("#st_group__steps_gridDiv").jqxGrid({ enabletooltips: true });
}
function bnd_step_grd(GR) {

    $('#st_group__steps_gridDiv').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
        { name: 'OFF_ABS_GROUP_ID' },
            { name: 'OFF_ABS_STEPS_ID' },
             { name: 'OFF_ABS_STEPS_NAME' },
              { name: 'OFF_ABS_GROUP_NAME' },
               { name: 'ARH_ROLE_NAME' },
            { name: 'ORDER_ID' }

        ],
        async: false,

        url: 'GROUPS/GET_STEPS',
        data: { GR: GR }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_group__steps_gridDiv").jqxGrid({ source: dataAdapter });
}
function bld_off_grd() {
    theme = "darkblue";
    gridId = 'st_group__person_gridDiv';
    headerText = "ضباط المجموعات";
    gridAddUrl = ControllerName + 'Create_Off';
    $("#st_group__person_gridDiv").jqxGrid({
        width: '74%',
        height: 230,
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
            { text: 'كود المرحلة', dataField: 'OFF_ABS_STEPS_ID', width: '10%', cellsalign: 'center', align: 'center', hidden: true },
            { text: 'كود المجموعه', dataField: 'OFF_ABS_GROUP_ID', width: '10%', cellsalign: 'center', align: 'center', hidden: true },
            //{ text: 'اسم المجموعه', dataField: 'OFF_ABS_GROUP_NAME', width: '25%', cellsalign: 'center', align: 'center' },
            { text: 'الرتبة ', dataField: 'RANK', width: '35%', cellsalign: 'center', align: 'center' },
            { text: ' الاسم', dataField: 'PERSON_NAME', width: '55%', cellsalign: 'center', align: 'center' },
           // { text: ' ترتيب المرحلة ', dataField: 'ORDER_ID', width: '15%', cellsalign: 'center', align: 'center' },
            //{
            //    text: 'تعديل', columntype: 'button', cellsalign: 'center', filterable: false, width: '10%', align: 'center', cellsrenderer: function (row, datafield, value) {
            //        return "<img  style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15' width='25' src='images/edit.png' onclick='open_edit(" + row + ", st_group__steps_gridDiv)'/>";
            //    }
            //},
            {
                text: 'حذف', columntype: 'button', cellsalign: 'center', filterable: false, width: '10%', align: 'center', cellsrenderer: function (row, datafield, value) {
                    return "<img   style='margin-left: 5px;margin-top:-10px; cursor:pointer;' height='15vh' width='25vw' src='images/delete.png' onclick='open_confirm(" + row + ", st_group__person_gridDiv)'/>";
                }
            }

        ]

    });
    $("#st_group__steps_gridDiv").jqxGrid({ enabletooltips: true });
}
function bnd_off_grd(GR) {

    $('#st_group__person_gridDiv').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
        { name: 'OFF_ABS_GROUP_OFF_ID' },
            { name: 'OFF_ABS_GROUP_ID' },
             { name: 'OFF_SKELETON_OFFICERS_ID' },
              { name: 'PERSON_NAME' },
              
                { name: 'RANK' },
            { name: 'PERSON_DATA_ID' }

        ],
        async: false,

        url: 'GROUPS/GET_off',
        data: { GR: GR }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_group__person_gridDiv").jqxGrid({ source: dataAdapter });
}
function bld_off_all_grd() {
    theme = "darkblue";
    headerText = "ضباط المجموعات";
    gridAddUrl = ControllerName + 'Create_Off';
    $("#off_grid").jqxGrid({
        width: 500,
        height:300,
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
            { name: 'PERSON_CAT_ID' }

        ],
        async: false,

        url: 'GROUPS/GET_off_all',
        data: { firms: GR, rank_cat: abs }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#off_grid").jqxGrid({ source: dataAdapter });
}
function open_edit(row, gridId) {
    
    var details = $('#' + gridId.id).jqxGrid('getrowdata', row);
    if (gridId.id == "st_group_gridDiv") {
        var url = ControllerName + 'Edit/' + details.OFF_ABS_GROUP_ID;

    } else if (gridId.id == "st_group__steps_gridDiv") {
        var url = ControllerName + 'Edit_Steps/' + details.OFF_ABS_STEPS_ID;
        var data = { id: details.OFF_ABS_STEPS_ID, id1: details.OFF_ABS_GROUP_ID };
    }

    openDialog_parm(url,data, "تعديل");
}
function open_confirm(row, gridId) {
    
    var details = $('#' + gridId.id).jqxGrid('getrowdata', row);
    if (gridId.id == "st_group_gridDiv") {
        var url = ControllerName + 'Delete';
        var data = { id: details.OFF_ABS_GROUP_ID };
        openConfirmDialog_parm(url, data, gridId.id, $('#ABSCENCE_CATEGORY_ID').val());

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
function set_ddl(id1,  frm) {
    
    var pid = id1;
    //getQS('nn', window.location.href);

    $.ajax({
        url: "groups/GET_JOP",
        data: { ID: pid,  FIRM: frm },
       
      
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