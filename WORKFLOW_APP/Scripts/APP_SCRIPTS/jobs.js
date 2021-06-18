var JOB_TYPE_ID = 0;
var ApplicationName = "/../WORKFLOW_APP";
var gridId = 'st_jobs_gridDiv';
var gridId2 = 'st_person_gridDiv';

var ControllerName = ApplicationName + '/Holidays/';

$(document).ready(function () {
    $('#page_name').text(' الوظــائف');
    //get_person_data();
    BuildDropDwon_firm("firm", "FIRM_CODE", "NAME", "Jobs/getfirm");
   
    bld_Jobs_grd();
    bind_Jobs_grid();
   
    $("#st_jobs_gridDiv").on("rowselect", function (event) {


        var data = $('#st_jobs_gridDiv').jqxGrid('getrowdata', event.args.rowindex);
        JOB_TYPE_ID = event.args.row.JOB_TYPE_ID;
        
        buildState();
        var state = $('#bldStateGrid').val();
        if (state == 1) {
            bld_person_grd();
            bind_person_grid2(state);
        }
    });

    $('#bldStateGrid').on('select', function (event) {
        
        var state = $('#bldStateGrid').val();
       
        if (state == 0) {

            bind_person_grid2(state);
        }
        else if (state == 1) {

            bind_person_grid2(state);
        }
        else {
            bind_person_grid();
        }
    });

  
});




function BuildDropDwon_firm(id, value, name, url) {

    var source =
       {
           datatype: "json",
           datafields:
               [
                { name: value },

                   { name: name }
               ],
           async: false,
           url: url,
           data: { }
       };
    var dataAdapter = new $.jqx.dataAdapter(source, { contentType: 'application/json; charset=utf-8' });


    $('#' + id).jqxComboBox({
        width: '65%',
        height: '38px',
        source: dataAdapter,
        autoComplete: true,
        searchMode: "containsignorecase",
        theme: "darkblue",
        rtl: true,
        selectedIndex: 0,
        placeHolder: ":اختار ",
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value


    });
    //$('#PARENT_FORM_DopDwon').jqxDropDownList('insertAt', { label: ' ---بدون الصفحة الام------', value: "" }, "");

}


//build the grid of jobs
function bld_Jobs_grd() {
    theme = "darkblue";

    disp = "inline-flex, none, inline-flex";
    headerText = " الوظــائف ";
    gridAddUrl = '/../WORKFLOW_APP/jobs/Create';
    h = "350";
    w = "700";

    $("#st_jobs_gridDiv").jqxGrid({
        width: '80%',
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
        rendertoolbar: toolbarfn12,
        columns: [
             { text: 'رقم الوظيفه', dataField: 'JOB_TYPE_ID', cellsalign: 'center', align: 'center', width: '0%', hidden: 'true' },
             { text: ' الفاعليه', dataField: 'ACTIVATION', cellsalign: 'center', align: 'center', width: '0%', hidden: 'true' },
            { text: 'الوظيفه', dataField: 'JOB_NAME', cellsalign: 'center', align: 'center', width: '40%' },
            { text: 'الاسم المختصر', dataField: 'SHORT_NAME', cellsalign: 'center', align: 'center', width: '30%' },//
            { text: 'الفاعليه', dataField: 'APP_FLAG', cellsalign: 'center', align: 'center', width: '15%' },//
            {
                text: '', columntype: 'button', cellsalign: 'center', filterable: false, width: '15%', align: 'center', cellsrenderer: function (row, datafield, value) {
                    return "<img  style='margin-left: 5px;cursor:pointer' height='20' width='20' src='../WORKFLOW_APP/images/edit.png' onclick='open_edit(" + row + ", st_jobs_gridDiv)'/>";
                }
            }

        ]

    });



}
// bind data to the grid
function bind_Jobs_grid() {
    
    $('#st_jobs_gridDiv').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
         { name: 'JOB_TYPE_ID' },
         { name: 'JOB_NAME' },
         { name: 'SHORT_NAME' },
         { name: 'APP_FLAG' },
         { name: 'ACTIVATION' },
        ],
        async: false,

        url: 'Jobs/getJobs',
        data: {  }
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_jobs_gridDiv").jqxGrid({ source: dataAdapter });


}




//build the grid of person
function bld_person_grd() {
    theme = "darkblue";

    disp = "inline-flex, none, inline-flex";
    headerText = " الضبـــاط ";
    gridAddUrl = '/../WORKFLOW_APP/Jobs/Create2';
    h = "350";
    w = "700";

    $("#st_person_gridDiv").jqxGrid({
        width: '80%',
        height: 240,
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
        rendertoolbar: toolbarfn123,
        columns: [
            { text: 'رقم الضابط', dataField: 'SEQ', cellsalign: 'center', align: 'center', width: '0%', hidden: 'true' },
            { text: 'كود الضابط', dataField: 'PERSON_CODE', cellsalign: 'center', align: 'center', width: '0%', hidden: 'true' },
            { text: 'رتبه', dataField: 'RANK', cellsalign: 'center', align: 'center', width: '10%' },
            { text: 'الضابط', dataField: 'PERSON_NAME', cellsalign: 'center', align: 'center', width: '40%' },//
            { text: 'من ', dataField: 'FROM_DATE', cellsalign: 'center', align: 'center', width: '15%' },//
            { text: 'الي', dataField: 'TO_DATE', cellsalign: 'center', align: 'center', width: '15%' },
         

            {
                text: '', columntype: 'button', cellsalign: 'center', filterable: false, width: '10%', align: 'center', cellsrenderer: function (row, datafield, value) {
                    return "<img  style='margin-left: 5px;cursor:pointer' height='20' width='20' src='../WORKFLOW_APP/images/edit.png' onclick='open_edit2(" + row + ", st_person_gridDiv)'/>";
                }
            },
            {
                text: '', columntype: 'button', cellsalign: 'center', filterable: false, width: '10%', align: 'center', cellsrenderer: function (row, datafield, value) {
                    return "<img   style='margin-left: 5px;cursor:pointer' height='17' width='17' src='../WORKFLOW_APP/images/delete.png' onclick='openConfirm (" + row + ", st_person_gridDiv)'/>";
                }
            }

        ]

    });



}
// bind data to the grid
function bind_person_grid() {
    
    $('#st_person_gridDiv').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
         { name: 'SEQ' },
         { name: 'PERSON_CODE' },
         { name: 'RANK' },
         { name: 'PERSON_NAME' },
         { name: 'FROM_DATE' },
           { name: 'TO_DATE' },
        ],
        async: false,

        url: 'Jobs/getperson',
        data: { JOB_TYPE_ID: JOB_TYPE_ID }
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_person_gridDiv").jqxGrid({ source: dataAdapter });


}
function bind_person_grid2(state) {

    $('#st_person_gridDiv').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
         { name: 'SEQ' },
         { name: 'PERSON_CODE' },
         { name: 'RANK' },
         { name: 'PERSON_NAME' },
         { name: 'FROM_DATE' },
           { name: 'TO_DATE' },
        ],
        async: false,

        url: 'Jobs/getperson2',
        data: { JOB_TYPE_ID: JOB_TYPE_ID , state : state }
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_person_gridDiv").jqxGrid({ source: dataAdapter });


}



// render toolbar of grids
var toolbarfn12 = function (toolbar) {
    var dis_itm = disp.split(',');
    var container = $("<div style='overflow: hidden; position: relative; margin: 5px;'></div>");


    var addButton = $("<div id='" + gridId + "&add'  onclick=openDlg22('" + gridAddUrl + "','إضافة'," + w + "," + h + ") style='float: right;cursor:pointer; margin-left: 5px; display:" + dis_itm[0] + ";' ><img style='position: relative; margin-top: 2px;width:20px;' src='../WORKFLOW_APP/images/addrec.png'><span  style='margin-left: 4px; position: relative; top: 0px;'>إضافة</span></div>");


    var reloadButton = $("<div id='" + gridId + "&relod' style='float: right; margin-left: 5px;display:" + dis_itm[1] + ";'><img style='position: relative; margin-top: 2px; display:block;' src='../WORKFLOW_APP/images/refresh.png'><span style='margin-left: 4px; position: relative; top: 0px;'>تحميل</span></div>");
    //var printButton = $("<div id='" + gridId + "&print' onclick='print_rep(" + gridId + ");' style='float: right; margin-left: 5px;display:" + dis_itm[2] + ";'><img style='position: relative; margin-top: 2px; display:block;width:20px;' src='../WORKFLOW_APP/images/printer.png'><span style='margin-left: 4px; position: relative; top: 0px;'>طباعة</span></div>");
    var lbl = $("<div style='float: right; margin-left: 5px; '><span style='margin-left: 4px;cursor:pointer; position: relative; top: -2px; margin-right: 44px;font-size: 21px;'>" + headerText + "</span></div>");
    container.append(addButton);

    container.append(reloadButton);
   // container.append(printButton);
    container.append(lbl);
    toolbar.append(container);
    addButton.jqxButton({ theme: theme, width: 60, height: 20 });
  //  printButton.jqxButton({ theme: theme, width: 60, height: 20 });


    reloadButton.jqxButton({ theme: theme, width: 70, height: 20 });
    reloadButton.click(function (event) {
        var id = event.currentTarget.id.split("&")[0];
        eval(id + "_Refresh('" + id + "')");
    });
};
var toolbarfn123 = function (toolbar) {
    var dis_itm = disp.split(',');
    var container = $("<div style='overflow: hidden; position: relative; margin: 5px;'></div>");


    var addButton = $("<div id='" + gridId2 + "&add'  onclick=openDlg33('" + gridAddUrl + "','إضافة'," + w + "," + h + ") style='float: right;cursor:pointer; margin-left: 5px; display:" + dis_itm[0] + ";' ><img style='position: relative; margin-top: 2px;width:20px;' src='../WORKFLOW_APP/images/addrec.png'><span  style='margin-left: 4px; position: relative; top: 0px;'>إضافة</span></div>");


    var reloadButton = $("<div id='" + gridId2 + "&relod' style='float: right; margin-left: 5px;display:" + dis_itm[1] + ";'><img style='position: relative; margin-top: 2px; display:block;' src='../WORKFLOW_APP/images/refresh.png'><span style='margin-left: 4px; position: relative; top: 0px;'>تحميل</span></div>");
    //var printButton = $("<div id='" + gridId2 + "&print' onclick='print_rep(" + gridId2 + ");' style='float: right; margin-left: 5px;display:" + dis_itm[2] + ";'><img style='position: relative; margin-top: 2px; display:block;width:20px;' src='../WORKFLOW_APP/images/printer.png'><span style='margin-left: 4px; position: relative; top: 0px;'>طباعة</span></div>");
    var lbl = $("<div style='float: right; margin-left: 5px; '><span style='margin-left: 4px;cursor:pointer; position: relative; top: -2px; margin-right: 44px;font-size: 21px;'>" + headerText + "</span></div>");
    container.append(addButton);

    container.append(reloadButton);
   // container.append(printButton);
    container.append(lbl);
    toolbar.append(container);
    addButton.jqxButton({ theme: theme, width: 60, height: 20 });
   // printButton.jqxButton({ theme: theme, width: 60, height: 20 });


    reloadButton.jqxButton({ theme: theme, width: 70, height: 20 });
    reloadButton.click(function (event) {
        var id = event.currentTarget.id.split("&")[0];
        eval(id + "_Refresh('" + id + "')");
    });
};

function openConfirm(row, gridId) {
    debugger
    var details = $('#' + gridId.id).jqxGrid('getrowdata', row);
    
    if (gridId.id == "st_person_gridDiv") {
        var url = 'Jobs/Delete';
        var data = { PERSON_CODE: details.PERSON_CODE, SEQ: details.SEQ };
        open_delete(url, data, gridId.id);
    }
    }

function open_delete(url, data, gridId) {
    swal({
        title: "هل تريد الحذف  ",
        text: "لا يمكن الرجوع فى عمليه الحذف ",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "نعم",
        cancelButtonText: "لا",
        closeOnConfirm: false
    },
            function () {
                $.ajax({
                    url: url,
                    type: 'POST',
                    data: data,
                    dataType: 'json',
                    success: function (d) {

                        if(d == 1)
                            
                        {
                            swal({
                                title: "",
                                text: "تم  المسح  بنجاح",
                                type: "success"

                            });
                            bind_person_grid();
                            $('#bldStateGrid').val(2);
                        }
                        

                       
                        eval(gridId.id + "_Refresh()");



                    },
                    error: function (err) {
                        // $('#msg').html('<div class="failed">Error! Please try again.</div>');
                        swal({
                            title: "خطا ",
                            text: "خطأ  ف الحذف",
                            type: "error",
                            timer: 2200
                        });
                        //  alert("Error!.");
                    }
                });
            });

}


// filter the detail grid 
function buildState() {

    

   var txt = { "label": "السابق", "value": 0 };
   var val = { "label": "الحالي", "value": 1 };
   var val2 = { "label": "الكل", "value": 2 };
  
    var data = new Array(txt, val,val2);
    var source =
    {
        localdata: data,
        datatype: "array"
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#bldStateGrid").jqxDropDownList({
        source: dataAdapter,
        displayMember: "label",
        valueMember: "value",
        width: '55%',
        height: '30',
       // placeHolder: " التصنيف  ",
        selectedIndex: 1,
        theme: "darkblue",
        rtl: true
    });
}



// add jop type 

function openDlg22(url, title, w, h) {

    $.ajax({
        url: url,
        success: function (data) {

            $("#dialog-edit").dialog({
                title: title,
                autoOpen: false,
                resizable: false,
                height: h,
                width: w,
              
                //postion: ['center','1000'],
                //left: '100px',
                show: { effect: "explode", duration: 0 },
                hide: {
                    effect: "explode", duration: 0},
                modal: true,
                draggable: true,
                open: function (event, ui) {
                    $("#dialog-edit").html(data);

                },
                close: function (event, ui) {
                    $("#dialog-edit").html('');
                    $('#overlayDiv').hide();
                }
            });

            $("#dialog-edit").dialog('open');
            return false;
        },
        error: function (err) {

            alert(err.responseText);

        }
    });


}
function openDlg33(url, title, w, h) {

    $.ajax({
        url: url,
        success: function (data) {

            $("#dialog-edit").dialog({
                title: title,
                autoOpen: false,
                resizable: false,
                height: h,
                width: w,

                //postion: ['center','1000'],
                //left: '100px',
                show: { effect: "explode", duration: 0 },
                hide: {
                    effect: "explode", duration: 0
                },
                modal: true,
                draggable: true,
                open: function (event, ui) {
                    $("#dialog-edit").html(data);

                },
                close: function (event, ui) {
                    $("#dialog-edit").html('');
                    $('#overlayDiv').hide();
                }
            });

            $("#dialog-edit").dialog('open');
            return false;
        },
        error: function (err) {

            alert(err.responseText);

        }
    });


}


function open_edit(row, gridId) {


    

    var details = $('#' + gridId.id).jqxGrid('getrowdata', $('#' + gridId.id).jqxGrid('selectedrowindex'));
    if (gridId.id == "st_jobs_gridDiv") {
        var url = '/../WORKFLOW_APP/Jobs/Edit';
        var data = { JOB_TYPE_ID: details.JOB_TYPE_ID, JOB_NAME: details.JOB_NAME, SHORT_NAME: details.SHORT_NAME, ACTIVATION: details.ACTIVATION };
    } 

    openDialog_parm_job(url, data, "تعديل");

}
function openDialog_parm_job(url, data, title) {

    $.ajax({
        url: url,
        data: data,
        success: function (data) {
            $("#dialog-edit").dialog({
                title: title,
                autoOpen: false,
                resizable: true,
                height: '350',
                width: '700',
                show: { effect: "explode", duration: 0 },
                hide: {
                    effect: "explode", duration: 0
                },
                modal: true,
                draggable: true,
                open: function (event, ui) {
                    $("#dialog-edit").html(data);
                },
                close: function (event, ui) {
                    $("#dialog-edit").html('');
                    $("#dialog-edit").dialog('close');
                }
            });

            $("#dialog-edit").dialog('open');
            return false;
        },
        error: function (err) {

            alert(err.responseText);

        }
    });


}


function open_edit2(row, gridId) {

    


    var details = $('#' + gridId.id).jqxGrid('getrowdata', $('#' + gridId.id).jqxGrid('selectedrowindex'));
    if (gridId.id == "st_person_gridDiv") {
        var url = '/../WORKFLOW_APP/Jobs/Edit2';
        var data = { PERSON_CODE: details.PERSON_CODE, SEQ: details.SEQ};
    }

    openDialog_parm_person(url, data, "تعديل");

}
function openDialog_parm_person(url, data, title) {

    $.ajax({
        url: url,
        data: data,
        success: function (data) {
            $("#dialog-edit").dialog({
                title: title,
                autoOpen: false,
                resizable: true,
                height: '350',
                width: '700',
                show: { effect: "explode", duration: 0 },
                hide: {
                    effect: "explode", duration: 0
                },
                modal: true,
                draggable: true,
                open: function (event, ui) {
                    $("#dialog-edit").html(data);
                },
                close: function (event, ui) {
                    $("#dialog-edit").html('');
                    $("#dialog-edit").dialog('close');
                }
            });

            $("#dialog-edit").dialog('open');
            return false;
        },
        error: function (err) {

            alert(err.responseText);

        }
    });


}


