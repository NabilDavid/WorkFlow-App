var ApplicationName = "/../WORKFLOW_APP";
var gridId = 'st_holidays_gridDiv';
var ControllerName = ApplicationName + '/Holidays/';

$(document).ready(function () {
    $('#page_name').text(' الاجازات الرسميه');
    get_person_data();
    bld_holiday_grd();
    bind_holiday_grid();
    //alert(firm_cod);
    BuildDropDwon_grid("Searchyear", "H_YEAR", "H_YEAR", "Holidays/getYear");
    BuildDropDwon_firm("firm", "FIRM_CODE", "NAME", "Holidays/getfirm");

  
    $('.jqx-combobox-input-summer').keyup(function (event) {
       
        var yyyy = "";
        yyyy = $('.jqx-combobox-input-summer').val();
            if (yyyy != "") {
                bind_holiday_grid_Search(yyyy);    
                
            }

            else {
                bind_holiday_grid();
            }

          
            
    });


    $('#Searchyear').on('select', function (event) {
        
        var yyyy = $('#Searchyear').val();
        bind_holiday_grid_Search(yyyy);
       
      
    });
    
    
   
});


//build the grid of holidays
function bld_holiday_grd() {

    theme = "darkblue";

    disp = "inline-flex, none, inline-flex";
    headerText = " الإجازات الرسمية ";
    gridAddUrl = '/../WORKFLOW_APP/Holidays/Create';
    h = "350";
    w = "700";
  
    $("#st_holidays_gridDiv").jqxGrid({
        width: '80%',
        height: 500,
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
            { text: 'الاجازة', dataField: 'H_DESC', cellsalign: 'center', align: 'center', width: '40%' },
            { text: 'من', dataField: 'H_FROM_DATE', cellsalign: 'center', align: 'center', width: '15%' },//
            { text: 'الي', dataField: 'H_TO_DATE', cellsalign: 'center', align: 'center', width: '15%' },//
            { text: 'السنه', dataField: 'H_YEAR', cellsalign: 'center', align: 'center', width: '10%' },
            { text: 'كود الوحده', dataField: 'FIRM_CODE', cellsalign: 'center', align: 'center', width: '23%', hidden:true },
           
            {
                text: '', columntype: 'button', cellsalign: 'center', filterable: false, width: '10%', align: 'center', cellsrenderer: function (row, datafield, value) {
                    return "<img  style='margin-left: 5px;cursor:pointer' height='20' width='20' src='../WORKFLOW_APP/images/edit.png' onclick='open_edit(" + row + ", st_holidays_gridDiv)'/>";
                }
            },
            {
                text: '', columntype: 'button', cellsalign: 'center', filterable: false, width: '10%', align: 'center', cellsrenderer: function (row, datafield, value) {
                    return "<img   style='margin-left: 5px;cursor:pointer' height='17' width='17' src='../WORKFLOW_APP/images/delete.png' onclick='open_confirm(" + row + ", st_holidays_gridDiv)'/>";
                }
            }

        ]
       
    });
   


}
// bind data to the grid
function bind_holiday_grid() {
  
    $('#st_holidays_gridDiv').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
         { name: 'H_DESC' },
         { name: 'H_FROM_DATE' },
           { name: 'H_TO_DATE' } ,
        { name: 'H_YEAR' },
    { name: 'FIRM_CODE' }
        ],
        async: false,     

        url: 'Holidays/getHoliday',
        data: { FIRM: firm_cod }
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_holidays_gridDiv").jqxGrid({ source: dataAdapter });


}
// bind data to search of the grid




function bind_holiday_grid_Search(year) {

    $('#st_holidays_gridDiv').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
         { name: 'H_DESC' },
         { name: 'H_FROM_DATE' },
           { name: 'H_TO_DATE' },
        { name: 'H_YEAR' },
    { name: 'FIRM_CODE' }
        ],
        async: false,

        url: 'Holidays/getHolidaySearch',
        data: { FIRM: firm_cod , year:year }
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_holidays_gridDiv").jqxGrid({ source: dataAdapter });


}
// build dropdown  to get  the firm 
function BuildDropDwon_firm(id, value, name, url) {
    debugger
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
           data: { FIRM: firm_cod }
       };
    var dataAdapter = new $.jqx.dataAdapter(source, { contentType: 'application/json; charset=utf-8' });


    $('#' + id).jqxComboBox({
        width: '75%',
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
function BuildDropDwon_grid(id, value, name, url) {

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
           data: { FIRM: firm_cod }
       };
    var dataAdapter = new $.jqx.dataAdapter(source, { contentType: 'application/json; charset=utf-8' });


    $('#' + id).jqxComboBox({
        width: '75%',
        height: '38px',
      
        source: dataAdapter,
        autoComplete: true,
        searchMode: "containsignorecase",
        theme: "summer",
        rtl: true,
        selectedIndex: -1,
        placeHolder: ":اختار    ",
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value


    });
 //   $('#Searchyear').jqxDropDownList('insertAt', { label: ' ---بدون الصفحة الام------', value: "" }, "");

}
//  send data  and url  to open dialog 
function open_edit(row, gridId) {

  

    
    var details = $('#' + gridId.id).jqxGrid('getrowdata', $('#' + gridId.id).jqxGrid('selectedrowindex'));
    if (gridId.id == "st_holidays_gridDiv") {
        var url = '/../WORKFLOW_APP/Holidays/Edit'; 
        var data = { FIRM_CODE: details.FIRM_CODE, H_YEAR: details.H_YEAR, H_FROM_DATE: details.H_FROM_DATE, H_TO_DATE: details.H_TO_DATE, H_DESC: details.H_DESC };
    }
    
    openDialog_parm_vac_holiday(url, data, "تعديل");
   
}
// send data to control by url 
function openDialog_parm_vac_holiday(url, data, title) {
    
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
// send data  and url  to open confirm 
function open_confirm(row, gridId) {
    debugger
    var details = $('#' + gridId.id).jqxGrid('getrowdata', $('#' + gridId.id).jqxGrid('selectedrowindex'));
    if (gridId.id == "st_holidays_gridDiv") {
        var url = '../WORKFLOW_APP/Holidays/Delete';
        var data = { FIRM_CODE: details.FIRM_CODE, H_YEAR: details.H_YEAR, H_FROM_DATE: details.H_FROM_DATE, H_TO_DATE: details.H_TO_DATE };
        open_delete(url, data, gridId, "BND_ROL_GRD('" + details.FIRM_CODE + "'," + details.H_YEAR + "," + details.H_FROM_DATE + "," + details.H_TO_DATE + ")");
       
    }
    
}
//  send data and url to delete it
function open_delete(url, data, gridId, fn) {
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
                        
                        swal({
                            title: d.title,
                            text: d.message,
                            type: d.type,
                            timer: 2200
                        });
                        bind_holiday_grid();
                        eval(fn);
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
var toolbarfn12 = function (toolbar) {
    var dis_itm = disp.split(',');
    var container = $("<div style='overflow: hidden; position: relative; margin: 5px;'></div>");


    var addButton = $("<div id='" + gridId + "&add'  onclick=openDlg22('" + gridAddUrl + "','إضافة'," + w + "," + h + ") style='float: right;cursor:pointer; margin-left: 5px; display:" + dis_itm[0] + ";' ><img style='position: relative; margin-top: 2px;width:20px;' src='../WORKFLOW_APP/images/addrec.png'><span  style='margin-left: 4px; position: relative; top: 0px;'>إضافة</span></div>");


   var reloadButton = $("<div id='" + gridId + "&relod' style='float: right; margin-left: 5px;display:" + dis_itm[1] + ";'><img style='position: relative; margin-top: 2px; display:block;' src='../WORKFLOW_APP/images/refresh.png'><span style='margin-left: 4px; position: relative; top: 0px;'>تحميل</span></div>");
  //  var printButton = $("<div id='" + gridId + "&print' onclick='print_rep(" + gridId + ");' style='float: right; margin-left: 5px;display:" + dis_itm[2] + ";'><img style='position: relative; margin-top: 2px; display:block;width:20px;' src='../WORKFLOW_APP/images/printer.png'><span style='margin-left: 4px; position: relative; top: 0px;'>طباعة</span></div>");
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
function openDlg22(url, title, w, h) {
    
    $.ajax({
        url: url,
        success: function (data) {

            $("#dialog-edit").dialog({
                title: title,
                autoOpen: false,
                //resizable: true,
                height: h,
                width: w,
                //postion: ['center','1000'],
                //left: '100px',
                show: { effect: "explode", duration: 0 },
                hide: {
                    effect: "explode", duration:0
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