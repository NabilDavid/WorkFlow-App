
var ApplicationName = "/../WORKFLOW_APP"
var gridAddUrl = '';
var name = '';
var add = '';
var pid = '';
var person_c = '';
var firm = '';
var disp = "none,none";
var w = '1000';
var h = '400';
var flag = 0;
var toolbarfn = function (toolbar) {
    var container = $("<div style='overflow: hidden; position: relative; margin: 5px;'></div>");
    var addButton = $("<div id='" + gridId + "&add'  onclick=openDialog('" + gridAddUrl + "','إضافة') style='float: right;cursor:pointer; margin-left: 5px; display:none;' ><img style='position: relative; margin-top: 2px' src='images/add.png'><span  style='margin-left: 4px; position: relative; top: 0px;'>إضافة</span></div>");

    // buttons of grid officers
    var forceButton = $("<div id='" + gridId + "&force' style='float: right;cursor:pointer; margin-left: 5px; display:none;' ><span  style='margin-left: 4px; position: relative; top: 0px;'>قوة الوحدة</span></div>");
    var BorrowIn_Button = $("<div id='" + gridId + "&BorrowIn' style='float: right;cursor:pointer; margin-left: 5px; display:none;' ><span  style='margin-left: 4px; position: relative; top: 0px;'>ملحقين عليها</span></div>");
    var BorrowOut_Button = $("<div id='" + gridId + "&BorrowOut' style='float: right;cursor:pointer; margin-left: 5px; display:none;' ><span  style='margin-left: 4px; position: relative; top: 0px;'> ملحقين بالخارج</span></div>");
    var outforceButton = $("<div id='" + gridId + "&outforce' style='float: right;cursor:pointer; margin-left: 5px; display:none;' ><span  style='margin-left: 4px; position: relative; top: 0px;'>خارج القوة</span></div>");
   
    var addButtonoff = $("<div id='" + gridId + "&addoff' style='float: right;cursor:pointer; margin-left: 5px; display:none;' ><img style='position: relative; margin-top: 2px' src='images/add.png'><span  style='margin-left: 4px; position: relative; top: 0px;'>إضافة</span></div>");
    var editButton = $("<div id='" + gridId + "&edit'  style='float: right;cursor:pointer; margin-left: 5px; display:none;' ><img style='position: relative; margin-top: 2px' src='images/EDIT.png'><span  style='margin-left: 4px; position: relative; top: 0px;'>تعديل</span></div>");
    var deleteButton = $("<div id='" + gridId + "&delete'  style='float: right;cursor:pointer; margin-left: 5px; display:none;' ><img style='position: relative; margin-top: 2px' src='images/delete.png'><span  style='margin-left: 4px; position: relative; top: 0px;'>حذف</span></div>");
    var addRelivantsButton = $("<div id='" + gridId + "&addrelivants' style='float: right;cursor:pointer; margin-left: 5px; display:none;' ><span  style='margin-left: 4px; position: relative; top: 0px;'>إضافة أقارب</span></div>");


    var reloadButton = $("<div id='" + gridId + "&relod' style='float: right; margin-left: 5px;display:none;'><img style='position: relative; margin-top: 2px; display:block;' src='../images/refresh.png'><span style='margin-left: 4px; position: relative; top: 0px;'>تحميل</span></div>");
    var lbl = $("<div style='float: right; margin-left: 5px; '><span style='margin-left: 4px;cursor:pointer; position: relative; top: -2px; margin-right: 44px;font-size: 21px;'>" + headerText + "</span></div>");
    container.append(addButton);

    // buttons of grid officers 
    container.append(forceButton);
    container.append(BorrowIn_Button);
    container.append(BorrowOut_Button);
    container.append(outforceButton);
    container.append(addButtonoff);
    container.append(editButton);
    container.append(deleteButton);
    container.append(addRelivantsButton);


    container.append(reloadButton);
    container.append(lbl);
    toolbar.append(container);
    addButton.jqxButton({ theme: theme, width: 60, height: 20 });


    //buttons of grid officers
    forceButton.jqxButton({ theme: theme });
    BorrowIn_Button.jqxButton({ theme: theme});
    BorrowOut_Button.jqxButton({ theme: theme });
    outforceButton.jqxButton({ theme: theme });
    addButtonoff.jqxButton({ theme: theme });
    editButton.jqxButton({ theme: theme });
    deleteButton.jqxButton({ theme: theme });
    addRelivantsButton.jqxButton({ theme: theme });




    reloadButton.jqxButton({ theme: theme, width: 70, height: 20 });
    reloadButton.click(function (event) {
        var id = event.currentTarget.id.split("&")[0];
        eval(id + "_Refresh('" + id + "')");
    });
};

var toolbarfn1 = function (toolbar) {
    var dis_itm = disp.split(',');
    var container = $("<div style='overflow: hidden; position: relative; margin: 5px;'></div>");
    var addButton = $("<div id='" + gridId + "&add'  onclick=openDlg('" + gridAddUrl + "','إضافة'," + w + "," + h + ") style='float: right;cursor:pointer; margin-left: 5px; display:" + dis_itm[0] + ";' ><img style='position: relative; margin-top: 2px;width:20px;' src='../WORKFLOW_APP/images/addrec.png'><span  style='margin-left: 4px; position: relative; top: 0px;'>إضافة</span></div>");


    var reloadButton = $("<div id='" + gridId + "&relod' style='float: right; margin-left: 5px;display:" + dis_itm[1] + ";'><img style='position: relative; margin-top: 2px; display:block;' src='../WORKFLOW_APP/images/refresh.png'><span style='margin-left: 4px; position: relative; top: 0px;'>تحميل</span></div>");
    var printButton = $("<div id='" + gridId + "&print' onclick='print_rep(" + gridId + ");' style='float: right; margin-left: 5px;display:" + dis_itm[2] + ";'><img style='position: relative; margin-top: 2px; display:block;width:20px;' src='../WORKFLOW_APP/images/printer.png'><span style='margin-left: 4px; position: relative; top: 0px;'>طباعة</span></div>");
    var lbl = $("<div style='float: right; margin-left: 5px; '><span style='margin-left: 4px;cursor:pointer; position: relative; top: -2px; margin-right: 44px;font-size: 21px;'>" + headerText + "</span></div>");
    container.append(addButton);

    container.append(reloadButton);
    container.append(printButton);
    container.append(lbl);
    toolbar.append(container);
    addButton.jqxButton({ theme: theme, width: 60, height: 20 });
    printButton.jqxButton({ theme: theme, width: 60, height: 20 });


    reloadButton.jqxButton({ theme: theme, width: 70, height: 20 });
    reloadButton.click(function (event) {
        var id = event.currentTarget.id.split("&")[0];
        eval(id + "_Refresh('" + id + "')");
    });
};

function openDialog(url, title) {

    $.ajax({
        url: url,
        success: function (data) {

            $("#dialog-edit").dialog({
                title: title,
                autoOpen: false,
                //resizable: true,
                height: 'auto',
                width: 'auto',
                postion: ['left', '2000'],
                left: '500px',
                show: { effect: "blind", duration: 500 },
                hide: {
                    effect: "explode", duration: 1000
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

function openDialog2(url, title) {

    $.ajax({
        url: url,
        success: function (data) {

            $("#dialog-edit2").dialog({
                title: title,
                autoOpen: false,
                resizable: true,
                height: 'auto',
                width: 'auto',
                // top:1000,
                show: { effect: "blind", duration: 500 },
                hide: {
                    effect: "explode", duration: 1000
                },
                modal: true,
                draggable: true,
                open: function (event, ui) {
                    $("#dialog-edit2").html(data);

                },
                close: function (event, ui) {
                    $("#dialog-edit2").html('');
                    $('#overlayDiv').hide();
                }
            });

            $("#dialog-edit2").dialog('open');
            return false;
        },
        error: function (err) {

            alert(err.responseText);

        }
    });


}

function openDlg(url, title, w, h) {
    if (w == "0") {
        w = "auto";
    }
    if (h == "0") {
        h = "auto";
    }
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
                show: { effect: "blind", duration: 500 },
                hide: {
                    effect: "explode", duration: 1000
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

function openDialog_parm(url, data, title) {

    $.ajax({
        url: url,
        data: data,
        success: function (data) {
            $("#dialog-edit").dialog({
                title: title,
                autoOpen: false,
                resizable: true,
                height: 'auto',
                width: 'auto',
                show: { effect: "blind", duration: 0 },
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

function openConfirmDialog(url, data, gridId) {
    $("#dialog-confirm").dialog({
        autoOpen: false,
        resizable: false,
        height: 'auto',
        width: 'auto',
        top: 100,
        right: 2000,
        show: { effect: "blind", duration: 500 },
        hide: {
            effect: "explode", duration: 1000
        },
        modal: true,
        draggable: true,
        buttons: {
            "أوافق": function () {
                $.ajax({
                    url: url,
                    type: 'POST',
                    data: data,
                    dataType: 'json',
                    success: function (data) {
                        swal({
                            title: "تم الحذف",
                            text: "تم  الحذف  بنجاح",
                            type: "success",
                            timer: 2200
                        });
                        eval(gridId + "_Refresh()");
                        $("#dialog-confirm").dialog('close');



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

            },
            "لا أوافق": function () {
                $(this).dialog("close");

            }
        }
    });
    $("#dialog-confirm").dialog('open');
    return false;
}

function open_del(url, data, gridId, fn) {
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
                        flag = 1;
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

function openConfirmDialog_parm(url, data, gridId, id) {
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
                    data: data,
                    dataType: 'json',
                    success: function (data) {
                        swal({
                            title: "تم ",
                            text: "تمت  العملية بنجاح",
                            type: "success",
                            timer: 2200
                        });



                        eval(gridId + "_Refresh(" + id + ")");
                        $("#dialog-confirm").dialog('close');

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

function BuildDropDwon(id, value, name, url) {

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
        width: '75%',
        height: '38px',
        source: dataAdapter,
        autoComplete: true,
        searchMode: "containsignorecase",
        theme: "darkblue",
        rtl: true,
        placeHolder: ":اختار    ",
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value


    });
    //$('#PARENT_FORM_DopDwon').jqxDropDownList('insertAt', { label: ' ---بدون الصفحة الام------', value: "" }, "");

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
        width: '75%',
        height: '38px',
        source: dataAdapter,
        autoComplete: true,
        searchMode: "containsignorecase",
        theme: "darkblue",
        rtl: true,
        selectedIndex: 0,

        placeHolder: ":اختار    ",
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value


    });
    //$('#PARENT_FORM_DopDwon').jqxDropDownList('insertAt', { label: ' ---بدون الصفحة الام------', value: "" }, "");

}

function BuildDropDwon1(id, value, name, url, parm) {
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
           data: { firm_code: parm },

       };
    var dataAdapter = new $.jqx.dataAdapter(source, { contentType: 'application/json; charset=utf-8' });

    $('#' + id).jqxComboBox({
        width: '100%',
        height: '38px',
        source: dataAdapter,
        autoComplete: true,
        searchMode: "containsignorecase",
        theme: "darkblue",
        rtl: true,
        selectedIndex: 0,
        placeHolder: ":اختار    ",
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value


    });
    //$('#PARENT_FORM_DopDwon').jqxDropDownList('insertAt', { label: ' ---بدون الصفحة الام------', value: "" }, "");

}

function BuildDropDwonM(id, value, name, url, placeholderType) {
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
        width: '99%',
        height: '35px',
        source: dataAdapter,
        autoComplete: true,
        searchMode: "containsignorecase",
        theme: "darkblue",
        rtl: true,
        // selectedIndex: 0,
        placeHolder: placeholderType,
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value


    });
   
}

function BuildDropDwon_cert_spec(id, value, name, placeholderType) {
    $('#' + id).jqxComboBox({
        width: '99%',
        height: '35px',
        autoComplete: true,
        searchMode: "containsignorecase",
        theme: "darkblue",
        rtl: true,
        // selectedIndex: 0,
        placeHolder: placeholderType,
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value

    });
}

function BindDropDwon_cert(id, value, name, url,  educationLevelId) {
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
           url: url ,
           data: { parm: educationLevelId }
       };
    var dataAdapter = new $.jqx.dataAdapter(source, { contentType: 'application/json; charset=utf-8' });
    $('#' + id).jqxComboBox({ source: dataAdapter });

}

function BindDropDwon_spec(id, value, name, url, educationLevelId , certificationID) {
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
           data: {
               parm: educationLevelId,
               parm1: certificationID

           }
       };
    var dataAdapter = new $.jqx.dataAdapter(source, { contentType: 'application/json; charset=utf-8' });
    $('#' + id).jqxComboBox({ source: dataAdapter });

}

// this functios for users security page
function BuildDropDwonlist_rank(id, value, name, url) {
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
        width: '75%',
        height: '38px',
        source: dataAdapter,
        autoComplete: true,
        searchMode: "containsignorecase",
        theme: "darkblue",
        rtl: true,
        selectedIndex: -1,
        //checkboxes:true,
        placeHolder: "الرتبه :",
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value


    });
    //$('#PARENT_FORM_DopDwon').jqxDropDownList('insertAt', { label: ' ---بدون الصفحة الام------', value: "" }, "");

}
function BuildDropDwonlist_officer(id, value, name, url, parm) {

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
           data: { firm_code: parm },
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
        selectedIndex: -1,
        placeHolder: " اختر الضابط :",
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value


    });
    //$('#PARENT_FORM_DopDwon').jqxDropDownList('insertAt', { label: ' ---بدون الصفحة الام------', value: "" }, "");

}
function BuildDropDwonlistcheckBoxes(id, value, name, url) {
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
        width: '75%',
        height: '38px',
        source: dataAdapter,
        autoComplete: true,
        searchMode: "containsignorecase",
        theme: "darkblue",
        rtl: true,
        selectedIndex: 0,
        checkboxes: true,
        placeHolder: ":اختار    ",
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value


    });
    //$('#PARENT_FORM_DopDwon').jqxDropDownList('insertAt', { label: ' ---بدون الصفحة الام------', value: "" }, "");

}
function BuildDropDwon_2_param(id, value, name, url, parm1, pram2) {
    $('#' + id).jqxComboBox('clear');

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
           data: { firm_code: parm1, rank_id: pram2 },
       };
    var dataAdapter = new $.jqx.dataAdapter(source, { contentType: 'application/json; charset=utf-8' });
    $('#' + id).jqxComboBox({
        source: dataAdapter,
    });



}
function BuildDropDwon_2_param1(id, value, name, url, parm1, pram2) {

    $('#' + id).jqxComboBox({
        width: '75%',
        height: '38px',

        autoComplete: true,
        searchMode: "containsignorecase",
        theme: "darkblue",
        rtl: true,
        selectedIndex: -1,
        placeHolder: ":اختار    ",
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value


    });


}


//--------------------------------------



function builddropdownlist1(query, record, comboname, theme, wdth, h, onsuc) {

    var datafields = new Array();

    var recarr = record.split(',');

    for (i = 0; i < recarr.length; i++) {

        datafields.push({ name: recarr[i] });
    }
    PageMethods.bind_data(query, onsuccess, onfail);
    var dataAdapter;
    function onsuccess(res) {
        source = {
            datatype: "xml",
            datafields: datafields,
            async: false,
            record: 'Table',
        };
        source.localdata = res
        dataAdapter = new $.jqx.dataAdapter(source, { contentType: 'application/json; charset=utf-8' });
        $('#' + comboname).jqxDropDownList({

            rtl: true,
            selectedIndex: -1, source: dataAdapter,
            displayMember: recarr[1], valueMember: recarr[0],
            theme: theme,
            height: h, filterable: true, width: wdth, placeHolder: 'اختر من القائمة'
        });
        onsuc();
    }

    function onfail() {
        alert("server down");
    }
}

function BuildTree(id, value, name, parent, url) {
    var source =
{
    datatype: "json",
    datafields: [
        { name: value },
        { name: name },
       { name: parent }
    ],

    id: value,
    url: url,
    async: false
};

    var dataAdapter = new $.jqx.dataAdapter(source);

    dataAdapter.dataBind();
    var records = dataAdapter.getRecordsHierarchy(value, parent, 'items', [{ name: name, map: 'label' }, { name: value, map: 'value' }]);

    $('#' + id).jqxTree({ source: records, rtl: true, height: '300px', width: '100%', theme: '' });
}

function SearchTree(gridId, TbId) {
    $("#" + TbId).keyup(function () {
        var searchedValue = $("#" + TbId).val().toLowerCase();
        var items = $('#' + gridId).jqxTree("getItems");
        for (var i = 0; i < items.length; i++) {
            $("#" + items[i].id + " div:eq(0)").css("border", "none");
        }
        $('#' + gridId).jqxTree('collapseAll');
        if (searchedValue != "") {
            var selectedItems = new Array();
            for (var i = 0; i < items.length; i++) {
                if (items[i].label.toLowerCase().indexOf(searchedValue) != -1) {
                    $('#' + gridId).jqxTree('expandItem', items[i].parentElement);
                    $("#" + items[i].id + " div:eq(0)").css("border", "2px solid Green");
                }

            };

        }
    });
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
    $(div).jqxDropDownList({
        source: dataAdapter,
        displayMember: "label",
        valueMember: "value",
        width: '75%',
        height: '30',
        placeHolder: ":اختار    ",
        selectedIndex: 0,
        theme: theme,
        rtl: true
    });
}

function BLD_DD(DIV, URL, w, h, th, val, dis, sel) {
    source = {
        datatype: "json",
        datafields:
            [
            { name: dis },
            { name: val }]
        ,
        url: URL
    };


    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#" + DIV).jqxDropDownList({
        selectedIndex: sel,
        source: dataAdapter,
        animationType: 'slide',
        dropDownHorizontalAlignment: 'right',
        rtl: true,
        displayMember: dis,
        valueMember: val,
        theme: th,      //'ui-myredmond'
        placeHolder: "إختر ",
        //dropDownWidth: 200,
        width: w,
        height: h
    });
}

function BLD1_DD(DIV, URL, w, h, th, val, dis, sel, par) {
    source = {
        datatype: "json",
        datafields:
            [
            { name: dis },
            { name: val }]
        ,
        url: URL,
        data: par
    };


    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#" + DIV).jqxDropDownList({
        selectedIndex: sel,
        source: dataAdapter,
        animationType: 'slide',
        dropDownHorizontalAlignment: 'right',
        rtl: true,
        displayMember: dis,
        valueMember: val,
        theme: th,      //'ui-myredmond'
        placeHolder: "إختر ",
        //dropDownWidth: 200,
        width: w,
        height: h
    });
}

function set_ddl(id1, frm) {

    pid = id1;
    firm = frm;
    $.ajax({
        url: "../WORKFLOW_APP/groups/GET_JOP",
        data: { ID: pid, FIRM: frm },


        dataType: "json",

        success: function (reslult) {

            var FormList = reslult;
            if (FormList != "") {

                var dt = FormList.split('/');
                person_c = dt[0] + ',' + dt[4] + ',' + dt[5] + ',' + dt[3] + ',' + dt[1] + ',' + dt[2] + ',' + dt[6] + ',' + pid;
                localStorage.setItem("person_c", person_c);
                $("#user_name").text(dt[1] + ' / ' + dt[2]);
                $('h2').text($("#user_name").text());
                $("#user_name").show();
                $("#user_name").val(pid);
                if (dt[9] == "1") {
                    $('#mang_desc').show();
                }
                else if (dt[9] == "2") {
                    $('#vcm_desc').show();

                }


                else {
                    $('#mang_desc').hide();
                    $('btn_enter_officers').show();

                }
                if (dt[8] != "0") {

                }
                else {

                }
                add = dt[6];
                var data = dt[7].split(',');
                for (var i = 0; i < data.length; i++) {

                    if (data[i] == 'pln') {

                    }

                    else if (data[i] == 'cmd') {

                    }
                }
            }

        },
        error: function (response) {

        }
    });
}

function set_dd_p(id1, frm, fn) {

    pid = id1;
    firm = frm;
    $.ajax({
        url: "../WORKFLOW_APP/groups/GET_JOP",
        data: { ID: pid, FIRM: frm },


        dataType: "json",

        success: function (reslult) {

            var FormList = reslult;
            if (FormList != "") {

                var dt = FormList.split('/');
                person_c = dt[0] + ',' + dt[4] + ',' + dt[5] + ',' + dt[3] + ',' + dt[1] + ',' + dt[2] + ',' + dt[6] + ',' + pid;
                localStorage.setItem("person_c", person_c);
                $("#user_name").text(dt[1] + ' / ' + dt[2]);
                $('h2').text($("#user_name").text());
                $("#user_name").show();
                $("#user_name").val(pid);
                if (dt[9] == "1") {
                    $('#mang_desc').show();
                }
                else if (dt[9] == "2") {
                    $('#vcm_desc').show();

                }


                else {
                    $('#mang_desc').hide();
                    $('btn_enter_officers').show();

                }
                if (dt[8] != "0") {

                }
                else {

                }
                add = dt[6];
                var data = dt[7].split(',');
                for (var i = 0; i < data.length; i++) {

                    if (data[i] == 'pln') {

                    }

                    else if (data[i] == 'cmd') {

                    }
                }
            }
            eval(fn);

        },
        error: function (response) {

        }
    });
}

function BLD_OFF_GRD() {

    theme = "darkblue";
    headerText = "ضباط";
    gridAddUrl = '../GROUPS/Create_Off';
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

        url: '../GROUPS/GET_off_all',
        data: { firms: GR, rank_cat: abs }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#off_grid").jqxGrid({ source: dataAdapter });
};

function bld_off_all_grd() {
    BLD_OFF_GRD();
};

function bnd_off_all_grd(firms, value) {
    BND_OFF_GRD(firms, value);
};

function BLD_ROL_GRD(w, h) {

    gridId = 'VAC_ROLE';

    theme = "darkblue";
    headerText = " الخطوات";
    gridAddUrl = ControllerName + 'Create';
    $("#VAC_ROLE").jqxGrid({
        width: w,
        height: h,
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

function BND_ROL_GRD(data) {

    $('#VAC_ROLE').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
                        { name: 'OFF_ABS_STEPS_ID' },
                   { name: 'OFF_ABS_GROUP_ID' },
                   { name: 'OFF_ABS_STEPS_NAME' },
                   { name: 'ID' },
                   { name: 'ORDER_ID' },
                   { name: 'PERSON_CODE' },
                   { name: 'PERSON_NAME' },
                   { name: 'TYPE_ID' },
                   { name: 'RANK' },
                   { name: 'JOB' },
                   { name: 'ROL' }

        ],
        async: false,

        url: ControllerName + 'GET_V_ROLE',
        data: data

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#VAC_ROLE").jqxGrid({ source: dataAdapter });
}

