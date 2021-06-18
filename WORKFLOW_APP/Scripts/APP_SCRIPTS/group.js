
var groupID = 0;
var activation = "";
var flag = 0;
$(document).ready(function () {
    $('#page_name').text(' المجموعات');
    BuildDropDwon_firm("firm", "FIRM_CODE", "NAME", "groupUsers/getfirm");
    build_userGroup_grid();
    bind_group_grid();
    bldstat("activation");

    $("#st_groupp_gridDiv").on("rowselect", function (event) {


        groupID = event.args.row.GROUP_ID;
        groupName = event.args.row.GROUP_NAME;
        $("#groupName").val(groupName);

        activation = event.args.row.DEVELOPERS;
        $("#activation").val(activation);
        

    });


    $("#save").on('click', function (e) {
       
        e.preventDefault();

        var groupName = $("#groupName").val();
        var activation = $("#activation").val();


        if ($("#groupName").val() == "" || $("#activation").val() == "")
        {
            swal({
                title: "خطأ",
                text: "أدخل أسم المجموعة او الحاله",
                type: "error"
            });
        }
        else{
            $.ajax({

                url: "groupUsers/getGroupToSearchFoundation",
                type: 'POST',
                dataType: 'json',
                success: function (result) {
                    debugger
                    flag = 0;
                    for (var i = 0 ; i < result.length; i++) {
                        if (result[i].GROUP_NAME == groupName) {

                            flag = 1;
                            break;
                        }

                    }

                    if (flag == 1) {

                        swal({
                            title: "",
                            text: " هذا المجموعه موجوده من قبل  ",
                            type: "error"
                        });
                    }

                    else {

                        $.ajax({
                            url: "groupUsers/insertGroup",
                            type: 'POST',
                            data: { groupName: groupName, activation: activation },
                            dataType: 'json',
                            success: function (result) {
                                if (result == 1) {

                                    bind_group_grid();
                                    swal({
                                        title: "",
                                        text: "تم إضافة البيانات بنجاح",
                                        type: "success"

                                    });
                                    $("#groupName").val("");

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

       

    });
    $("#edit").on("click", function () {
      
        var groupName = $("#groupName").val();
        var activation = $("#activation").val();
       

        if ($("#groupName").val() == "" || $("#activation").val()=="") {
        
            swal({
                title: "خطأ",
                text: "أدخل أسم المجموعة او الحاله",
                type: "error"
            });
        }
        else{
            $.ajax({
                url: "groupUsers/updateGroup",
                type: 'POST',
                data: { groupName: groupName, activation: activation, groupId: groupID },
                dataType: 'json',
                success: function (result) {
                    if (result == 1) {
                        bind_group_grid();
                        swal({
                            title: "",
                            text: "تم التعديل البيانات بنجاح",
                            type: "success"

                        });
                    } else {
                        modal.style.display = "none";
                        swal({
                            title: "خطأ",
                            text: "لم يتم تعديل  البيانات",
                            type: "error"
                        });
                    }
                },
                error: function () {
                    modal.style.display = "none";
                    swal({
                        title: "خطأ",
                        text: "خطأ في إرسال البيانات الى الخادم",
                        type: "error"
                    });
                }
            });



        } 
        $("#groupName").val("");
    });
    $("#delete").on('click', function (e) {


        e.preventDefault();
        if ($("#groupName").val() == "" || $("#activation").val() == "") {

            swal({
                title: "خطأ",
                text: "أدخل أسم المجموعة او الحاله",
                type: "error"
            });
        }
        else {
            $.ajax({

                url: "groupUsers/changeActivation",
                type: 'POST',
                data: { groupID: groupID },
                dataType: 'json',
                success: function (result) {
                    if (result == 1) {

                        bind_group_grid();
                        $("#activation").val(0);
                        swal({
                            title: "",
                            text: "تم تغيير الفاعليه بنجاح",
                            type: "success"

                        });

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
        }
        $("#groupName").val("");
    }) // end of exit acivation button


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
           data: {  }
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
    
   
  
    gridId = 'st_groupp_gridDiv';

    theme = "darkblue";
    headerText = " المجموعات";
  
    $("#st_groupp_gridDiv").jqxGrid({
        width: '70%',
        height: '300px',
        theme: "darkblue",
        selectionmode:"singlerow",
        sortable: true,
        rtl: true,
        showaggregates: true,
        filterable: true,
        showfilterrow: true,
        showtoolbar: true,
        columns: [
            { text: 'كود المجموعه', dataField: 'GROUP_ID', width: '0%', cellsalign: 'center', align: 'center', hidden: 'true' },
            { text: 'الفاعليه', dataField: 'DEVELOPERS', width: '0%', cellsalign: 'center', align: 'center', hidden: 'true' },
            { text: 'اسم المجموعه', dataField: 'GROUP_NAME', width: '50%', cellsalign: 'center', align: 'center' },
          { text: ' الحالة ', dataField: 'ACTIVATION', width: '50%', cellsalign: 'center', align: 'center' }
                              
          
        ]

    });
    $("#st_groupp_gridDiv").jqxGrid({ enabletooltips: true });
}

function bind_group_grid() {

    $('#st_groupp_gridDiv').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
         { name: 'GROUP_ID' },
         { name: 'DEVELOPERS' },
         { name: 'GROUP_NAME' },
         { name: 'ACTIVATION' }
        ],
        async: false,

        url: 'groupUsers/getgroupGrid',
       // data: { GR: GR }
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_groupp_gridDiv").jqxGrid({ source: dataAdapter });
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
        width: '80%',
        height: '30',
        placeHolder: " اختر الفاعليه    ",
        selectedIndex: 0,
        theme: "darkblue",
        rtl: true
    });
}























