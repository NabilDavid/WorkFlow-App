

var ApplicationName = "/../WORKFLOW_APP";
var gridId = 'st_getUsers_gridDiv';
var ControllerName = ApplicationName + '/Users/';

var PERSON_ID = 0;
var passWord = "";
var userName = "";
var userDesc = "";
var Status = "";
var flag = 0;
var firm = "";



$(document).ready(function () {

    $('#page_name').text('المستخدمين');
    BuildDropDwon_firm("firm", "FIRM_CODE", "NAME", "Users/getfirm");
    
    build_userGroup_grid();
    bind_group_grid();

   
    

        
}); // the end of ready function

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
           data: {}
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

}
function build_userGroup_grid() {
    theme = "darkblue";

    disp = "inline-flex, none, inline-flex";
    headerText = " المستخدمين ";
    gridAddUrl = '/../WORKFLOW_APP/Users/Create';
    h = "400";
    w = "700";

    $("#st_getUsers_gridDiv").jqxGrid({
        width: '90%',
        height: 400,
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
              { text: ' رتبه', dataField: 'RANK', width: '15%', cellsalign: 'center', align: 'center' },
              { text: 'اسم الضابط', dataField: 'PERSON_NAME', width: '30%', cellsalign: 'center', align: 'center' },
             { text: 'اسم المستخدم', dataField: 'USER_NAME', width: '30%', cellsalign: 'center', align: 'center' },
              { text: 'كلمه السر', dataField: 'USER_PASSWORD', width: '0%', cellsalign: 'center', align: 'center', hidden: 'true' },
               { text: ' كود المستحدم', dataField: 'PERSON_CODE', width: '0%', cellsalign: 'center', align: 'center', hidden: 'true' },
                { text: ' رقم الرتبه', dataField: 'RANK_ID', width: '0%', cellsalign: 'center', align: 'center', hidden: 'true' },
              { text: 'الملاحظات', dataField: 'USER_DESC', width: '0%', cellsalign: 'center', align: 'center', hidden: 'true' },
               { text: 'الحاله', dataField: 'STATUS', width: '0%', cellsalign: 'center', align: 'center', hidden: 'true' },
              { text: ' الفاعليه', dataField: 'ACTIVATION', width: '15%', cellsalign: 'center', align: 'center' },
              { text: ' رقم المستخدم', dataField: 'PERSON_ID', width: '0%', cellsalign: 'center', align: 'center', hidden: 'true' },
               {
                   text: '', columntype: 'button', cellsalign: 'center', filterable: false, width: '10%', align: 'center', cellsrenderer: function (row, datafield, value) {
                       return "<img  style='margin-left: 5px;cursor:pointer' height='20' width='20' src='../WORKFLOW_APP/images/edit.png' onclick='open_edit(" + row + ", st_getUsers_gridDiv)'/>";
                   }
               }



        ]


    });



}
function openDlg(url, title, w, h) {

    $.ajax({
        url: url,
        success: function (data) {

            $("#dialog-edit").dialog({
                title: title,
                autoOpen: false,
                resizable: true,
                height: h,
                width: w,

                //postion: ['center','2'],
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

    
    debugger

    var details = $('#' + gridId.id).jqxGrid('getrowdata', $('#' + gridId.id).jqxGrid('selectedrowindex'));
    if (gridId.id == "st_getUsers_gridDiv") {
        var url = '/../WORKFLOW_APP/Users/Edit';

        var data = { PERSON_ID: details.PERSON_ID, USER_NAME: details.USER_NAME, USER_PASSWORD: details.USER_PASSWORD, USER_DESC: details.USER_DESC, STATUS: details.STATUS, PERSON_CODE: details.PERSON_CODE, RANK_ID: details.RANK_ID };

       
    }

    openDialog_parm_user(url, data, "تعديل");

}
// send data to control by url 
function openDialog_parm_user(url, data, title) {

    $.ajax({
        url: url,
        data: data,
        success: function (data) {
            $("#dialog-edit").dialog({
                title: title,
                autoOpen: false,
                resizable: true,
                height: '400',
                width: '700',
                //postion: ['center', '1000'],
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




















































function bind_group_grid() {

    $('#st_getUsers_gridDiv').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
            
         { name: 'RANK' },
         { name: 'PERSON_CODE' },
         { name: 'RANK_ID' },
         { name: 'PERSON_NAME' },
         { name: 'USER_NAME' },
         { name: 'USER_PASSWORD' },
          { name: 'USER_DESC' },
          { name: 'STATUS' },
       { name: 'ACTIVATION' },
        { name: 'PERSON_ID' }
    ],
        async: false,

        url: 'Users/getUsers',
        // data: { GR: GR }
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_getUsers_gridDiv").jqxGrid({ source: dataAdapter });
}

function BuildDropDwonlist(id, value, name, url) {

    var source =
       {
           datatype: "json",
           datafields:
               [
                { name: value },
                   { name: name }
               ],
           async: false,
           url: url

       };
    var dataAdapter = new $.jqx.dataAdapter(source, { contentType: 'application/json; charset=utf-8' });


    $('#' + id).jqxComboBox({
        width: '100%',
        height: '30',
        source: dataAdapter,
        autoComplete: true,
        searchMode: "containsignorecase",
        theme: "darkblue",
        rtl: true,
        checkboxes: true,
        dropDownHeight: 150,
        // selectedIndex: 0,
        //multiple:true,
        placeHolder: "اختر الصلاحيه",
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value


    });
    //$('#PARENT_FORM_DopDwon').jqxDropDownList('insertAt', { label: ' ---بدون الصفحة الام------', value: "" }, "");

}

function bldstat(div)



{
   
    
    var txt = { "label": "فعال", "value": 1 };
    var val = { "label": "غير فعال", "value": 0 };
   
    var data = new Array(txt, val);
    var source =
    {
        localdata: data,
        datatype: "array"
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $('#' + div).jqxDropDownList({
        source: dataAdapter,
        displayMember: "label",
        valueMember: "value",
        width: '100%',
        height: '30',
        placeHolder: " اختر الفاعليه    ",
        selectedIndex: 0,
        theme: "darkblue",
        rtl: true
    });
}
var toolbarfn12 = function (toolbar) {
    var dis_itm = disp.split(',');
    var container = $("<div style='overflow: hidden; position: relative; margin: 5px;'></div>");
    var addButton = $("<div id='" + gridId + "&add'  onclick=openDlg('" + gridAddUrl + "','إضافة'," + w + "," + h + ") style='float: right;cursor:pointer; margin-left: 5px; display:" + dis_itm[0] + ";' ><img style='position: relative; margin-top: 2px;width:20px;' src='../WORKFLOW_APP/images/addrec.png'><span  style='margin-left: 4px; position: relative; top: 0px;'>إضافة</span></div>");


    var reloadButton = $("<div id='" + gridId + "&relod' style='float: right; margin-left: 5px;display:" + dis_itm[1] + ";'><img style='position: relative; margin-top: 2px; display:block;' src='../WORKFLOW_APP/images/refresh.png'><span style='margin-left: 4px; position: relative; top: 0px;'>تحميل</span></div>");
   // var printButton = $("<div id='" + gridId + "&print' onclick='print_rep(" + gridId + ");' style='float: right; margin-left: 5px;display:" + dis_itm[2] + ";'><img style='position: relative; margin-top: 2px; display:block;width:20px;' src='../WORKFLOW_APP/images/printer.png'><span style='margin-left: 4px; position: relative; top: 0px;'>طباعة</span></div>");
    var lbl = $("<div style='float: right; margin-left: 5px; '><span style='margin-left: 4px;cursor:pointer; position: relative; top: -2px; margin-right: 44px;font-size: 21px;'>" + headerText + "</span></div>");
    container.append(addButton);

    container.append(reloadButton);
  // container.append(printButton);
    container.append(lbl);
    toolbar.append(container);
    addButton.jqxButton({ theme: theme, width: 60, height: 20 });
    //printButton.jqxButton({ theme: theme, width: 60, height: 20 });


    reloadButton.jqxButton({ theme: theme, width: 70, height: 20 });
    reloadButton.click(function (event) {
        var id = event.currentTarget.id.split("&")[0];
        eval(id + "_Refresh('" + id + "')");
    });
};





