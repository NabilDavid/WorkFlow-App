

var function_ID = 0;
var functionName = "";
var part = "";
var windowName = "";
var myActivation = "";
var flag = 0;




$(document).ready(function () {

    
    $('#page_name').text(' الصفحات');
    BuildDropDwon_firm("firm", "FIRM_CODE", "NAME", "Pages/getfirm");
    BuildDropDwonlist_part("part", "SYSTEM_DEPT_ID", "SYSTEM_DEPT", "Pages/getParts");
    build_PageGroup_grid();
    bind_page_grid();
    bldstat("myActivation");


    $("#st_getPage_gridDiv").on("rowselect", function (event) {

       
       var data = $('#st_getPage_gridDiv').jqxGrid('getrowdata', event.args.rowindex);
        function_ID = event.args.row.FUNCTION_ID;

       functionName = event.args.row.FUNCTION_ANAME
        $("#functionName").val(functionName);

         windowName = event.args.row.WINDOW_NAME
        $("#windowName").val(windowName);

         myActivation = event.args.row.IS_AVAILABLE;
         $("#myActivation").val(myActivation);

         
         if (event.args.row.SYSTEM_DEPT_ID != null) {
             part = event.args.row.SYSTEM_DEPT_ID
             $("#part").val(part);
         }
         else {
             part = "";
             $("#part").val(part);
         }

        $("#myGroup").jqxComboBox('uncheckAll');
        $.ajax({

            url: "Pages/gePagesGroups",
            type: 'POST',
            data: { function_ID: function_ID },
            dataType: 'json',
            success: function (data) {

                if (data != "") {

                    for (var i = 0; i < data.length; i++) {
                        $('#myGroup').jqxComboBox('checkItem', $('#myGroup').jqxComboBox('getItemByValue', data[i].GROUP_ID))

                    }
                }
            },
            error: function (e) {

                alert(error.responseText);
            }
        });


    });
    BuildDropDwonlist("myGroup", "GROUP_ID", "GROUP_NAME", "Pages/getGroups");
    
  

    // save page and his group
    $("#save").on('click', function (e) {
        e.preventDefault();
   
       

        // get data page to send to the server
         functionName = $("#functionName").val();
         windowName = $("#windowName").val();
         myActivation = $("#myActivation").val();
         part = $("#part").val();

        var groups = "";
        // get checked items in drop down to send server
        var items = $(".group").jqxComboBox('getCheckedItems');

        for (var i = 0; i < items.length ; i++) {
            if (i == (items.length - 1)) {
                groups += items[i].value;
            }
            else {
                groups += items[i].value + ",";
            }

        }



        if (functionName == "" || windowName == "" || myActivation == "" || part == "") {
            swal({
                title: "",
                text: "  من فضلك ادخل  الاسم و اللينك والفاعليه والفرع ",
                type: "error"
            });
        }
        else {
            
            $.ajax({

                url: "Pages/getPageToSearchFoundation",
                type: 'POST',
                dataType: 'json',
                success: function (result) {
                
                    flag = 0;
                    for (var i = 0 ; i < result.length; i++) {
                        if (result[i].FUNCTION_ANAME == functionName) {

                            flag = 1;
                            break;
                        }

                    }

                    if (flag == 1) {

                        swal({
                            title: "",
                            text: " هذه الصفحه موجوده من قبل  ",
                            type: "error"
                        });
                    }

                    else {

                        $.ajax({

                            url: "Pages/insertPage",
                            type: 'POST',
                            data: { functionName: functionName, myActivation: myActivation, windowName: windowName,part:part,groups: groups },
                            dataType: 'json',
                            success: function (result) {
                                if (result == 1) {

                                    bind_page_grid();

                                    swal({
                                        title: "",
                                        text: "تم إضافة البيانات بنجاح",
                                        type: "success"

                                    });
                                    $("#functionName").val("");
                                    $("#windowName").val("");
                                    $("#myActivation").val("");
                                    $("#part").val("");
                                    $("#myGroup").jqxComboBox('uncheckAll');
                                } else {
                                    swal({
                                        title: "خطأ",
                                        text: "لم يتم إدخال البيانات",
                                        type: "error"
                                    });
                                }
                            },
                            error: function () {
                                swal({
                                    title: "خطأ",
                                    text: "خطأ في إرسال البيانات الى الخادم",
                                    type: "error"
                                });
                            }
                        });

                    }

                },
                error: function () {

                }
            }); // end of  ajax
                    

                

        }



       

    }); //end of save button
    // exit acitvation for this page
    $("#delete").on('click', function (e) {
        e.preventDefault();

        // get data page to send to the server


        $.ajax({

            url: "Pages/changeActivation",
            type: 'POST',
            data: { function_ID: function_ID },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                   
                    bind_page_grid();
                    $("#myActivation").val(0);
                    swal({
                        title: "",
                        text: "تم تغيير الفاعليه بنجاح",
                        type: "success"

                    });
                    $("#functionName").val("");
                    $("#windowName").val("");
                    $("#myActivation").val("");
                    $("#myGroup").jqxComboBox('uncheckAll');

                }
                else {

                    swal({
                        title: "خطأ",
                        text: "لم يتم تغيير الفاعليه",
                        type: "error"
                    });

                }
            },
            error: function () {
                swal({
                    title: "خطأ",
                    text: "خطأ في إرسال البيانات الى الخادم",
                    type: "error"
                });
            }
        }); // end of  ajax

    }) // end of exit acivation button

    // edit page and his group
    $("#edit").on('click', function (e) {
        e.preventDefault();

        // get data page to send to the server
        var newFunctionName = $("#functionName").val();
        var newWindowName = $("#windowName").val();
        var newMyActivation = $("#myActivation").val();
        var myNewPart = $("#part").val();

        var newGroups = "";
        // get checked items in drop down to send server
        var items = $(".group").jqxComboBox('getCheckedItems');

        for (var i = 0; i < items.length ; i++) {
            if (i == (items.length - 1)) {
                newGroups += items[i].value;
            }
            else {
                newGroups += items[i].value + ",";
            }

        }
        


        if (newFunctionName == "" || newWindowName == "" || newMyActivation == "" || myNewPart == "") {
            swal({
                title: "",
                text: "  من فضلك ادخل  الاسم و اللينك والفاعليه والفرع ",
                type: "error"
            });
        }

        else {
          
                $.ajax({
                    url: "Pages/updatePageAndGroup",
                    type: 'POST',
                    data: {
                        function_ID: function_ID, functionName: functionName, windowName: windowName, myActivation: myActivation, myNewPart:myNewPart,
                        newFunctionName: newFunctionName, newWindowName: newWindowName, newMyActivation: newMyActivation, newGroups: newGroups
                    },
                    dataType: 'json',
                    success: function (result) {
                        
                        if (result == 1) {
                            bind_page_grid();
                            swal({
                                title: "",
                                text: "تم تعديل البيانات بنجاح",
                                type: "success"
                            });
                            $("#functionName").val("");
                            $("#windowName").val("");
                            $("#myActivation").val("");
                            $("#myGroup").jqxComboBox('uncheckAll');
                            $("#part").val("");
                        } else {
                            swal({
                                title: "خطأ",
                                text: "لم يتم تعديل البيانات",
                                type: "error"
                            });
                        }
                    },
                    error: function () {
                        swal({
                            title: "خطأ",
                            text: "خطأ في إرسال البيانات الى الخادم",
                            type: "error"
                        });
                    }
                }); // end of  ajax      

        




        }


       

    });
    





});
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
function build_PageGroup_grid() {

  

    gridId = 'st_getUsers_gridDiv';

    theme = "darkblue";
    headerText = " الشاشات";

    $("#st_getPage_gridDiv").jqxGrid({
        width: '100%',
        height: '300px',
        theme: "darkblue",
        sortable: true,
        rtl: true,
        showaggregates: true,
        filterable: true,
        showfilterrow: true,
        showtoolbar: true,
        columns: [
            { text: 'اسم الصفحه', dataField: 'FUNCTION_ANAME', width: '50%', cellsalign: 'center', align: 'center' },
            
            { text: ' الفاعليه', dataField: 'MYACTIVATION', width: '50%', cellsalign: 'center', align: 'center' },

            { text: ' رقم الصفحه', dataField: 'FUNCTION_ID', width: '0%', cellsalign: 'center', align: 'center', hidden: 'true' },
            { text: ' رقم الصفحه', dataField: 'IS_AVAILABLE', width: '0%', cellsalign: 'center', align: 'center', hidden: 'true' },
            { text: '  الفرع', dataField: 'SYSTEM_DEPT_ID', width: '0%', cellsalign: 'center', align: 'center', hidden: 'true' },
            { text: ' مسار الصفحه', dataField: 'WINDOW_NAME', width: '0%', cellsalign: 'center', align: 'center', hidden: 'true' }


        ]

    });
    $("#st_getPage_gridDiv").jqxGrid({ enabletooltips: true });
}
function bind_page_grid() {

    $('#st_getPage_gridDiv').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
         { name: 'FUNCTION_ANAME' },
         { name: 'SYSTEM_DEPT_ID' },
         { name: 'MYACTIVATION' },
          { name: 'FUNCTION_ID' },
           { name: 'IS_AVAILABLE' },
           { name: 'WINDOW_NAME' }
       
        ],
        async: false,

        url: 'Pages/getPages',
        // data: { GR: GR }
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_getPage_gridDiv").jqxGrid({ source: dataAdapter });
}
function bldstat(div) {


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
function BuildDropDwonlist_part(id, value, name, url) {

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
       // checkboxes: true,
        dropDownHeight: 150,
        // selectedIndex: 0,
        //multiple:true,
        placeHolder: "اختر الفرع",
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value


    });
    //$('#PARENT_FORM_DopDwon').jqxDropDownList('insertAt', { label: ' ---بدون الصفحة الام------', value: "" }, "");

}
