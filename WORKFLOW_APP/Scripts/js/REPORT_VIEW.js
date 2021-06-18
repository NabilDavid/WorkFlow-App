var amana_approval_id = "";
$(document).ready(function () {

    $("body").on("click", "#add_copy_no", function () {
        window_dispaly();
        $("#window_content").html("<div class='row' dir='rtl'><div class='col-md-6 mar'><input type='text' class='form-control' id='copy-no' placeholder=' ادخل رقم  الصورة'/></div><div class='col-md-6 mar'><input data='' type='button' class='btn btn-info form-control' id='ADD_COPY_BTN' value=' حفظ '/></div></div>'");
    })

    $("body").on("click", "#add_attach_no", function () {
        window_dispaly();
        $("#window_content").html("<div class='row' dir='rtl'><div class='col-md-6 mar'><input type='number' class='form-control' id='attach-no' placeholder='    ادخل عدد المرفقات'/></div><div class='col-md-6 mar'><input data='' type='button' class='btn btn-info form-control' id='ADD_ATTACH_BTN' value='حفظ  '/></div></div>'");
    })

    $("body").on("click", "#add_subject", function () {
         
        $("#window_content").html("<div class='row' dir='rtl'><div class='col-md-4 mar'><input type='text' class='form-control' id='new-subject' placeholder=' اضف موضوع جديد  '/></div><div class='col-md-4 mar'><select id='branch_id' class='form-control' size='1'><option class='option_id' value='0'>أخترالفرع المختص   </option></select></div><div class='col-md-4 mar'><input data='' type='button' class='btn btn-info form-control' id='ADD_SUBJECT_BTN' value='إضافة  '/></div><div class='col-md-4 mar'><input data='' type='button' class='btn btn-info form-control' id='UPDATE_SUBJECT_BTN' value='تعديل  ' /></div></div><div class='col-md-12'><div class='SUBJECT_GRID'></div></div><div class='col-md-4 mar'><input data='' type='button' class='btn btn-info form-control' id='SUBJECT_BTN' value=' اختر '/></div></div>'");
        fill_branch_dropdown();
        build_subject_grid();
        bind_subject_grid();
        window_dispaly();
        
    })

    $("body").on("click", "#add_department", function () {
       
            $("#window_content").html("<div class='row' dir='rtl'><div class='col-md-6 mar'><select id='dep_id' class='form-control' size='1'><option class='option_id' value='0'>اختر الجهات المختصة </option></select></div><div class='col-md-6 mar'><input data='' type='button' class='btn btn-info form-control' id='ADD_DEPARTMENT_BTN' value='إضافة جهه '/></div></div><div class='col-md-12'><div class='DEPARTMENT_GRID'></div></div><div class='col-md-6 mar'><input data='' type='button' class='btn btn-info form-control' id='APPROVAL_DEPARTMENT_BTN' value=' اختر هذه الجهات '/></div></div>'");
            fill_department_dropdown();
            build_department_grid();
            bind_department_grid();
            window_dispaly();
        
    })

    $("body").on("click", "#add_copy_dep", function () {
       
            $("#window_content").html("<div class='row' dir='rtl'><div class='col-md-6 mar'><select id='dep_id' class='form-control' size='1'><option class='option_id' value='0'>اختر الجهات المختصة </option></select></div><div class='col-md-6 mar'><input data='' type='button' class='btn btn-info form-control' id='ADD_COPY_DEPARTMENT_BTN' value='إضافة جهه '/></div></div><div class='col-md-12'><div class='DEPARTMENT_GRID'></div></div><div class='col-md-6 mar'><input data='' type='button' class='btn btn-info form-control' id='APPROVAL_COPY_DEPARTMENT_BTN' value=' اختر هذه الجهات '/></div></div>'");
            fill_department_dropdown();
            build_department_grid();
            bind_copy_department_grid();
            window_dispaly();
        
    })

    $("body").on("click", "#add_approval_text", function () {
        
        $("#window_content").html("<div class='row' dir='rtl'><div class='col-md-4 mar'><input type='text' class='form-control' id='approval-text' placeholder=' اضف  عنوان تصديق جديد'/></div><div class='col-md-3 mar'><input type='text' class='form-control' id='qed-no' placeholder='  رقم القيد '/></div><div class='col-md-3 mar'><input type='date' class='form-control' id='q-date' placeholder='    ادخل التاريخ'/></div><div class='col-md-4 mar'><input type='text' class='form-control' id='5etm-no' placeholder='  رقم  ختم الوثائق'/></div><div class='col-md-3 mar'><input type='text' class='form-control' id='post-no' placeholder=' اضف رقم البريد العسكرى'/></div><div class='col-md-3 mar'><input type='text' class='form-control' id='app-no' placeholder='  رقم التصديق '/></div><div class='col-md-4 mar'><input data='' type='button' class='btn btn-info form-control' id='ADD_APPROVAL_ADDRESS_BTN' value='إضافة تصديق '/></div><div class='col-md-3 mar'><input data='' type='button' class='btn btn-info form-control' id='UPDATE_APPROVAL_ADDRESS_BTN' value='تعديل التصديق '/></div></div><div class='col-md-12'><div class='APPROVAL_GRID'></div></div><div class='col-md-6 mar'><input data='' type='button' class='btn btn-info form-control' id='APPROVAL_ADDRESS_BTN' value=' اختر التصديق '/></div></div>'");
        build_approval_grid();
        bind_approval_grid();
        window_dispaly();
    })

    $("body").on("click", "#SUBJECT_BTN", function () {

        var rowindex = $('.SUBJECT_GRID').jqxGrid('getselectedrowindex');
        var data = $('.SUBJECT_GRID').jqxGrid('getrowdata', rowindex);
        var subject = data.SUBJECT_NAME;
        $("#subject").empty();
        $("#subject").append(subject);
        $("#add_subject").removeClass("fa-pencil").addClass("fa-refresh").css({"color":"red"});
        $("#windowInsert").fadeOut();

         
    });

    $("body").on("click", "#ADD_SUBJECT_BTN", function () {

        var new_subject = $("#new-subject").val();
        var branch_id = $("#branch_id").val();
       

        if (branch_id == 0) {
            alert("اختر  اسم الفرع  ")
        } else {
            if (new_subject == "" || branch_id == "") {

                alert("ادخل البيانات بشكل صحيح ");
            } else {

                $.ajax({
                    url: 'REPORT_VIEW.aspx/insertNewSubject',
                    data: JSON.stringify({
                        branch_id: branch_id,
                        new_subject: new_subject

                    }),
                    cache: false,
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    success: function (data) {

                        alert("تمت الاضافه")
                        bind_subject_grid();
                    },
                    error: function (response) {
                        alert("لم تتم الاضافة ");
                    }
                });
            }
        }

    });

    $("body").on("click","#ADD_APPROVAL_ADDRESS_BTN", function () {
        debugger;
        var new_app = $("#approval-text").val();
        var qed_no = $("#qed-no").val();
        var q_date = $("#q-date").val();
        var khetm_no = $("#5etm-no").val();
        var app_no = $("#app-no").val();
        var post_no = $("#post-no").val();


         
            if (new_app == "" || qed_no == "" || q_date == "" || khetm_no == "" || app_no == "" || post_no == "") {

                alert("ادخل البيانات بشكل صحيح ");
            } else {

                $.ajax({
                    url: 'REPORT_VIEW.aspx/insertNewApproval',
                    data: JSON.stringify({
                        new_app: new_app,
                        qed_no: qed_no,
                        q_date: q_date,
                        khetm_no: khetm_no,
                        app_no: app_no,
                        post_no: post_no
                    }),
                    cache: false,
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    success: function (data) {

                        alert("تمت الاضافه");
                        bind_approval_grid();
                    },
                    error: function (response) {
                        alert("لم تتم الاضافة ");
                    }
                });            
        }
    });

    $("body").on("click", "#APPROVAL_ADDRESS_BTN", function () {

        var rowindex = $('.APPROVAL_GRID').jqxGrid('getselectedrowindex');
        var data = $('.APPROVAL_GRID').jqxGrid('getrowdata', rowindex);
        var approval_text = data.APPROVAL_TEXT;
        amana_approval_id = data.AMANA_APROVAL_ID;
        var qed_no = data.QED_NO;
        var approval_no = data.APPROVAL_NO ;
        var branch_id =  data.BRANCH_ID;
        var qed_date = data.QED_DATE;
        var post_no = data.POST_NO;
        var stamp_doc_no = data.STAMP_DOC_NO;

        $("#approval_address").empty();
        $("#approval_address").append(approval_text);
        build_department_label();
        build_copy_department_label();
        build_approval_header_label();
        build_approval_text_label();
        $("#add_department").removeClass("fa-pencil").addClass("fa-refresh").css({ "color": "red" });
        $("#add_approval_text").removeClass("fa-pencil").addClass("fa-refresh").css({ "color": "red" });
        $("#add_copy_dep").addClass("fa fa-refresh ").css({ "color": "red" });
        $("#add_department").addClass("fa fa-refresh").css({ "color": "red" });

        $("#date").empty();
        $("#date").append(qed_date);

        $("#qed_no").empty();
        $("#qed_no").append(qed_no);

        $("#5etm_no").empty();
        $("#5etm_no").append(stamp_doc_no);

        $("#post_no").empty();
        $("#post_no").append(post_no);

        $("#approval_no").empty();
        $("#approval_no").append(branch_id + "/" + approval_no);

        $("#windowInsert").fadeOut();


    });

    $("body").on("click", "#ADD_DEPARTMENT_BTN", function () {
        debugger;
        var dep_id = $("#dep_id").val();
        
        if (dep_id == 0) {
            alert("اختر  الجهه المختصة   ")
        } else {

            $.ajax({
                url: 'REPORT_VIEW.aspx/insertNewDep',
                data: JSON.stringify({
                    dep_id: dep_id,
                    amana_approval_id: amana_approval_id
                }),
                cache: false,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST",
                success: function (data) {

                    alert("تمت الاضافه");
                    bind_department_grid();
                },
                error: function (response) {
                    alert("لم تتم الاضافة ");
                }
            });
        }
    });

    $("body").on("click", "#APPROVAL_DEPARTMENT_BTN", function () {
        build_department_label();
        build_copy_department_label();
        $("#windowInsert").fadeOut();

        
    });

    $("body").on("click", "#ADD_COPY_DEPARTMENT_BTN", function () {
        debugger;
        var dep_id = $("#dep_id").val();

        if (dep_id == 0) {
            alert("اختر  الجهه المختصة   ")
        } else {

            $.ajax({
                url: 'REPORT_VIEW.aspx/insertCopyDep',
                data: JSON.stringify({
                    dep_id: dep_id,
                    amana_approval_id: amana_approval_id
                }),
                cache: false,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST",
                success: function (data) {

                    alert("تمت الاضافه");
                    bind_copy_department_grid();
                },
                error: function (response) {
                    alert("لم تتم الاضافة ");
                }
            });
        }
    });

    $("body").on("click", "#APPROVAL_COPY_DEPARTMENT_BTN", function () {
        build_department_label();
        build_copy_department_label();
        $("#windowInsert").fadeOut();


    });

    $("body").on("click", "#ADD_COPY_BTN", function () {

        var copy = $("#copy-no").val();
        if (copy == 0) {
            alert("   ادخل رقم الصورة !!  ")
        } else {


            $("#copy_no").empty();
            $("#copy_no").append(copy);
            $("#add_copy_no").removeClass("fa-pencil").addClass("fa-refresh").css({ "color": "red" });
            $("#windowInsert").fadeOut();
        }
    });

    $("body").on("click", "#ADD_ATTACH_BTN", function () {
        
        var attach = $("#attach-no").val();
        if (attach == 0) {
            alert("   ادخل رقم القيد !!  ")
        } else {


            $("#attach_no").empty();
            $("#attach_no").append(attach);
            $("#add_attach_no").removeClass("fa-pencil").addClass("fa-refresh").css({ "color": "red" });
            $("#windowInsert").fadeOut();
        }
    });
   

});

function window_dispaly() {

    $("#windowInsert").fadeIn();
    $("#close_window_btn").on('click', function () {
        $("#windowInsert").fadeOut();
    }); 
    
}

/*-------subject-------------------*/
function build_subject_grid() {
   
    var cellsrenderer = function (row, columnfield, value, defualthtml, columnproperites) {
        return '<div style="margin :3px; font-size:16px;float:right;text-align: center;min-width:100%">' + value + '</div>';
    }
    var cellsrendererdelete = function (row, columnfield, value, defualthtml, columnproperites) {
        return '<div onClick="delete_subject()" class="fa fa-trash" style="color:red ;margin :3px; font-size:20px;float:right;text-align: center;min-width:100%">' + value + '</div>';
    }
    var cellsrendererupdate = function (row, columnfield, value, defualthtml, columnproperites) {
        return '<div onClick="update_subject()" class="fa fa-edit" style=" color:green ;margin :3px; font-size:20px;float:right;text-align: center;min-width:100%">' + value + '</div>';
    }

    $(".SUBJECT_GRID").jqxGrid(
    {
        sortable: true,
        width: "100%",
        height: "200px",
        rtl: true,
        columns: [
            { text: 'الرقم', datafield: 'SUBJECT_ID', width: "10%", align: "center", cellsalign: "center", cellsrenderer: cellsrenderer },
            { text: 'اسم الموضوع', datafield: 'SUBJECT_NAME', width: "60%" ,align: "center", cellsalign: "center", cellsrenderer: cellsrenderer },
            { text: ' رقم الفرع ', datafield: 'BRANCH_ID', width: "10%", align: "center", cellsalign: "center", cellsrenderer: cellsrenderer },
             { text: ' تعديل ', width: "10%", align: "center", align: "center", cellsrenderer: cellsrendererupdate },
             { text: ' حذف ', width: "10%", align: "center",  align: "center",cellsrenderer: cellsrendererdelete }
        ]
    });

  
}
function bind_subject_grid() {
    var data_subject;
    $.ajax({

        url: 'REPORT_VIEW.aspx/GetSubject',
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (data) {

            data_subject = data.d;

        },
        error: function (response) {
            alert("error")
        }

    });
    var source =
      {
          datatype: "json",
          datafields: [
              { name: 'SUBJECT_ID' },
              { name: 'SUBJECT_NAME' },
              { name: 'BRANCH_ID' }

          ],
          localdata: data_subject

      };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $(".SUBJECT_GRID").jqxGrid({ source: dataAdapter });
}
function fill_branch_dropdown() {

    $.ajax({

        url: 'REPORT_VIEW.aspx/GETBRANCH',
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (result) {
            debugger;
            var branch = JSON.parse(result.d);
            for (var i = 0; i < branch.Table.length; i++) {
                $("#branch_id").append("<option class='option_id' value='" + branch.Table[i].BRANCH_ID + "'>" + branch.Table[i].BRANCH_NAME + "</option>");

            }

        },
        error: function (response) {
            alert("error")
        }

    });
}
function delete_subject() {

    var delId = $('.SUBJECT_GRID').jqxGrid('getselectedrowindexes');
    var deldata = $('.SUBJECT_GRID').jqxGrid('getrowdata', delId);
    var subject_code = deldata.SUBJECT_ID;


    $.ajax({

        url: 'REPORT_VIEW.aspx/deleteSubject',
        data: JSON.stringify({
            subject_code: subject_code

        }),

        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (data) {

            alert("تم حذف الموضوع")
            bind_subject_grid();
        },
        error: function (response) {
            alert("لم يتم الحذف")
        }

    });

}
function update_subject() {

    var delId = $('.SUBJECT_GRID').jqxGrid('getselectedrowindexes');
    var deldata = $('.SUBJECT_GRID').jqxGrid('getrowdata', delId);
    var subject_id = deldata.SUBJECT_ID;
    var subject_name = deldata.SUBJECT_NAME;
    var branch_id = deldata.BRANCH_ID;
    $("#new-subject").val(subject_name);
    $("#branch_id").val(branch_id);
    $("#ADD_SUBJECT_BTN").hide();
    $("body").on("click", "#UPDATE_SUBJECT_BTN", function () {
        //var subject_id;
        var new_subject = $("#new-subject").val();
        var branch_id = $("#branch_id").val();


        if (branch_id == 0) {
            alert("اختر  اسم الفرع  ")
        } else {
            if (new_subject == "" || branch_id == "") {

                alert("ادخل البيانات بشكل صحيح ");
            } else {

                $.ajax({
                    url: 'REPORT_VIEW.aspx/updateSubject',
                    data: JSON.stringify({
                        subject_id: subject_id,
                        branch_id: branch_id,
                        new_subject: new_subject

                    }),
                    cache: false,
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    success: function (data) {

                        alert("تم تعديل الموضوع")
                        bind_subject_grid();
                        $("#new-subject").val("");
                        $("#branch_id").val("");
                        $("#ADD_SUBJECT_BTN").show();
                        $("#UPDATE_SUBJECT_BTN").hide();
                    },
                    error: function (response) {
                        alert("لم  يتم التعديل ");
                    }
                });
            }
        }

    });

}

/*-------approval-------------------*/
function build_approval_grid() {

    var cellsrenderer = function (row, columnfield, value, defualthtml, columnproperites) {
        return '<div style="margin :3px; font-size:16px;float:right;text-align: center;min-width:100%">' + value + '</div>';
    }
    var cellsrendererdelete = function (row, columnfield, value, defualthtml, columnproperites) {
        return '<div onClick="delete_approval()" class="fa fa-trash" style="color:red ;margin :3px; font-size:20px;float:right;text-align: center;min-width:100%">' + value + '</div>';
    }
    var cellsrendererupdate = function (row, columnfield, value, defualthtml, columnproperites) {
        return '<div onClick="update_approval()" class="fa fa-edit" style=" color:green ;margin :3px; font-size:20px;float:right;text-align: center;min-width:100%">' + value + '</div>';
    }

    $(".APPROVAL_GRID").jqxGrid(
    {
        sortable: true,
        width: "100%",
        height: "300px",
        rtl: true,
        columns: [
            { text: 'الرقم', datafield: 'AMANA_APROVAL_ID', width: "5%", align: "center", cellsalign: "center", cellsrenderer: cellsrenderer },
            { text: ' التصديق', datafield: 'APPROVAL_TEXT', width: "40%", align: "center", cellsalign: "center", cellsrenderer: cellsrenderer },
            { text: ' رقم القيد ', datafield: 'QED_NO', width: "10%", align: "center", cellsalign: "center", cellsrenderer: cellsrenderer },
            { text: '  التاريخ ', datafield: 'QED_DATE', width: "10%", align: "center", cellsalign: "center", cellsrenderer: cellsrenderer },
            { text: ' رقم ختم الوثائق ', datafield: 'STAMP_DOC_NO', width: "10%", align: "center", cellsalign: "center", cellsrenderer: cellsrenderer },
            { text: ' رقم البريد العسكرى ', datafield: 'POST_NO', width: "5%", align: "center", cellsalign: "center", cellsrenderer: cellsrenderer },
            { text: ' رقم التصديق ', datafield: 'APPROVAL_NO', width: "10%", align: "center", cellsalign: "center", cellsrenderer: cellsrenderer },
             { text: ' تعديل ', width: "5%", align: "center", align: "center", cellsrenderer: cellsrendererupdate },
             { text: ' حذف ', width: "5%", align: "center", align: "center", cellsrenderer: cellsrendererdelete }
        ]
    });


}
function bind_approval_grid() {
    var data_approval;
    $.ajax({

        url: 'REPORT_VIEW.aspx/GetApproval',
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (data) {

            data_approval = data.d;

        },
        error: function (response) {
            alert("error")
        }

    });
    var source =
      {
          datatype: "json",
          datafields: [
              
              { name: 'AMANA_APROVAL_ID' },
              { name: 'APPROVAL_TEXT' },
              { name: 'QED_NO' },
              { name: 'QED_DATE' },
              { name: 'STAMP_DOC_NO' },
              { name: 'POST_NO' },
              { name: 'APPROVAL_NO' },
              { name: 'BRANCH_ID' }

          ],
          localdata: data_approval

      };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $(".APPROVAL_GRID").jqxGrid({ source: dataAdapter });
}
function delete_approval() {

    var delId = $('.APPROVAL_GRID').jqxGrid('getselectedrowindexes');
    var deldata = $('.APPROVAL_GRID').jqxGrid('getrowdata', delId);
    var approval_id = deldata.AMANA_APROVAL_ID;


    $.ajax({

        url: 'REPORT_VIEW.aspx/deleteApproval',
        data: JSON.stringify({
            approval_id: approval_id

        }),

        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (data) {

            alert("تم حذف التصديق")
            bind_approval_grid();
        },
        error: function (response) {
            alert("لم يتم الحذف")
        }

    });

}
function update_approval() {

    var delId = $('.APPROVAL_GRID').jqxGrid('getselectedrowindexes');
    var deldata = $('.APPROVAL_GRID').jqxGrid('getrowdata', delId);
    var approval_id = deldata.AMANA_APROVAL_ID;
    var approval_text = deldata.APPROVAL_TEXT;
    var qed_date = deldata.QED_DATE;
    qed_date= qed_date.split('/');

    qed_date= qed_date[2] + '-' + qed_date[1] + '-' + qed_date[0];
    var post_no = deldata.POST_NO;
    var stamp_doc_no = deldata.STAMP_DOC_NO;
    var approval_no = deldata.APPROVAL_NO;
    var qed_no = deldata.QED_NO;

    $("#approval-text").val(approval_text);
    $("#qed-no").val(qed_no);
     $("#q-date").val(qed_date);
     $("#5etm-no").val(stamp_doc_no);
     $("#app-no").val(approval_no);
     $("#post-no").val(post_no);


     $("#ADD_APPROVAL_ADDRESS_BTN").hide();

     $("body").on("click", "#UPDATE_APPROVAL_ADDRESS_BTN", function () {
         debugger;
         var new_app = $("#approval-text").val();
         var qed_no = $("#qed-no").val();
         var q_date = $("#q-date").val();
         var khetm_no = $("#5etm-no").val();
         var app_no = $("#app-no").val();
         var post_no = $("#post-no").val();


       
         
         if (new_app == "" || qed_no == "" || q_date == "" || khetm_no == "" || app_no == "" || post_no == "") {

                alert("ادخل البيانات بشكل صحيح ");
            } else {

                $.ajax({
                    url: 'REPORT_VIEW.aspx/updateApproval',
                    data: JSON.stringify({
                        approval_id:approval_id,
                        new_app: new_app,
                        qed_no: qed_no,
                        q_date: q_date,
                        khetm_no: khetm_no,
                        app_no:app_no,
                        post_no: post_no
                    }),
                    cache: false,
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    success: function (data) {

                        alert("تم تعديل الموضوع")
                        bind_approval_grid();

                         $("#approval-text").val("");
                         $("#qed-no").val("");
                         $("#q-date").val("");
                         $("#5etm-no").val("");
                         $("#app-no").val("");
                         $("#post-no").val("");


                         $("#ADD_APPROVAL_ADDRESS_BTN").show();
                        $("#UPDATE_APPROVAL_ADDRESS_BTN").hide();
                    },
                    error: function (response) {
                        alert("لم  يتم التعديل ");
                    }
                });
            }
        

    });

}

/*-------department-------------------*/
function build_department_grid() {

    var cellsrenderer = function (row, columnfield, value, defualthtml, columnproperites) {
        return '<div style="margin :3px; font-size:16px;float:right;text-align: center;min-width:100%">' + value + '</div>';
    }
    var cellsrendererdelete = function (row, columnfield, value, defualthtml, columnproperites) {
        return '<div onClick="delete_department()" class="fa fa-trash" style="color:red ;margin :3px; font-size:20px;float:right;text-align: center;min-width:100%">' + value + '</div>';
    }
   

    $(".DEPARTMENT_GRID").jqxGrid(
    {
        sortable: true,
        width: "100%",
        height: "300px",
        rtl: true,
        columns: [
            { text: 'الرقم', datafield: 'ROWNUM', width: "10%", align: "center", cellsalign: "center", cellsrenderer: cellsrenderer },
            { text: ' اسم الجهه', datafield: 'DEP_NAME', width: "80%", align: "center", cellsalign: "center", cellsrenderer: cellsrenderer },           
             { text: ' حذف ', width: "10%", align: "center", align: "center", cellsrenderer: cellsrendererdelete }
        ]
    });


}
function build_department_label() {
    var data_department;
    $.ajax({

        url: 'REPORT_VIEW.aspx/GetDepartment',
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (data) {

            data_department = JSON.parse(data.d);
            $("#departments").empty();
            for (var i = 0; i < data_department.Table.length; i++) {
                $("#departments").append("السيد رئيس " + data_department.Table[i].DEP_NAME);
                $("#departments").append("</br>");
            }
            

        },
        error: function (response) {
            alert("error")
        }

    });
}
function bind_department_grid() {
    var data_department;
    $.ajax({

        url: 'REPORT_VIEW.aspx/GetDepartment',
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (data) {

            data_department = data.d;

        },
        error: function (response) {
            alert("error")
        }

    });
    var source =
      {
          datatype: "json",
          datafields: [

              { name: 'ROWNUM' },
              {name:'DEP_ID'},
              { name: 'DEP_NAME' }
             

          ],
          localdata: data_department

      };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $(".DEPARTMENT_GRID").jqxGrid({ source: dataAdapter });
}
function fill_department_dropdown() {

    $.ajax({

        url: 'REPORT_VIEW.aspx/GET_DEPARTMENT',
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (result) {
            
            var department = JSON.parse(result.d);
            for (var i = 0; i < department.Table.length; i++) {
                $("#dep_id").append("<option class='option_id' value='" + department.Table[i].DEP_ID + "'>" + department.Table[i].DEP_NAME + "</option>");
               
            }

        },
        error: function (response) {
            alert("error")
        }

    });
}
function delete_department() {
    debugger;
    var delId = $('.DEPARTMENT_GRID').jqxGrid('getselectedrowindexes');
    var deldata = $('.DEPARTMENT_GRID').jqxGrid('getrowdata', delId);
    var dep_id = deldata.DEP_ID;
    

    $.ajax({

        url: 'REPORT_VIEW.aspx/deleteDep',
        data: JSON.stringify({
            dep_id: dep_id

        }),

        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (data) {

            alert("تم حذف الجهه")
            bind_department_grid();
        },
        error: function (response) {
            alert("لم يتم الحذف")
        }

    });

}
function build_copy_department_label() {
    var data_department;
    $.ajax({

        url: 'REPORT_VIEW.aspx/GetCopyDepartment',
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (data) {

            data_department = JSON.parse(data.d);
            $("#copy_dep").empty();
            for (var i = 0; i < data_department.Table.length; i++) {
                $("#copy_dep").append("السيد رئيس " + data_department.Table[i].DEP_NAME);
                $("#copy_dep").append("</br>");
            }


        },
        error: function (response) {
            alert("error")
        }

    });
}
function bind_copy_department_grid() {
    var data_department;
    $.ajax({

        url: 'REPORT_VIEW.aspx/GetCopyDepartment',
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (data) {

            data_department = data.d;

        },
        error: function (response) {
            alert("error")
        }

    });
    var source =
      {
          datatype: "json",
          datafields: [

              { name: 'ROWNUM' },
              { name: 'DEP_ID' },
              { name: 'DEP_NAME' }


          ],
          localdata: data_department

      };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $(".DEPARTMENT_GRID").jqxGrid({ source: dataAdapter });
}

function build_approval_header_label() {
    var data_header;
    $.ajax({

        url: 'REPORT_VIEW.aspx/GetHeader',
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (data) {

            data_header = JSON.parse(data.d);
            $("#approval_header").empty();
            $("#approval_header").append(data_header.Table[0].APPROVAL_ITEM_TEXT);
        },
        error: function (response) {
            alert("error")
        }

});

}
function build_approval_text_label() {
    var data_text;
    $.ajax({

        url: 'REPORT_VIEW.aspx/GetText',
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (data) {
            var id =0;
            data_text = JSON.parse(data.d);
            $("#approval_text").empty();
            for (var i = 0; i < data_text.Table.length ; i++) {
                
                if (id == data_text.Table[i].APPROVAL_ITEM_ID) {
                    $("#approval_text").append("<ul><li>" + data_text.Table[i].ASSIGNMENT_TEXT + "</li></ul>");
                }
                else {
                    if (data_text.Table[i].ASSIGNMENT_TEXT != null) 
                        $("#approval_text").append(" <li class='approv_text'>" + data_text.Table[i].APPROVAL_ITEM_TEXT + "<ul><li>" + data_text.Table[i].ASSIGNMENT_TEXT + "</li></ol></li>");
                    else $("#approval_text").append(" <li class='approv_text'>" + data_text.Table[i].APPROVAL_ITEM_TEXT);
                }

                id = data_text.Table[i].APPROVAL_ITEM_ID;
            }
            
            
        },
        error: function (response) {
            alert("error")
        }

    });

}