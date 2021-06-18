
var ControllerName = ApplicationName + '/OFFICERS/';
var data_row;
var data_row1;
var group_id
//var firms = '1402102001';
$(document).ready(function () {
    debugger;
    build_static();

    // build and bind officer grid
    build_OFFICERS_grid();
    bind_OFFICERS_grid();



    get_person_data();
    BuildDropDwon1('FIRMS_CODE', 'FIRM_CODE', 'NAME', ApplicationName + '/GROUPS/firms_ddl', firm_cod);

    // fill DropDowns
    BuildDropDwonM('RANK_CODE', 'RANK_ID', 'RANK', ApplicationName + '/OFFICERS/fillDropDown_ranks', "اختر الرتبة");
    BuildDropDwonM('ArmyDepartments', 'DEPARTMENT_ID', 'NAME', ApplicationName + '/OFFICERS/fillDropDown_army_departments', "اختر السلاح");
    BuildDropDwonM('jobs', 'JOB_TYPE_ID', 'JOB_NAME', ApplicationName + '/OFFICERS/fillDropDown_jobs', "اختر الوظيفة");
    BuildDropDwonM('CategoryID', 'SPECIALIZATION_ID', 'NAME', ApplicationName + '/OFFICERS/fillDropDown_category', "اختر الفئة");
    BuildDropDwonM('BloodID', 'BLOOD_TYPE_ID', 'NAME', ApplicationName + '/OFFICERS/fillDropDown_blood', "اختر فصيلة الدم");
    BuildDropDwonM('BorrowID', 'FIRM_CODE', 'NAME', ApplicationName + '/OFFICERS/fillDropDown_OutForce', "اختر الوحدة التابع لها");

    //$("#jqxCheckBox1").jqxCheckBox({ width: 150, height: 25, checked: false });

   
    seet_fin_year();

    // buttons of grid officers

    document.getElementById('st_OFFICERS_gridDiv&force').style.display = 'inline-block';
    document.getElementById('st_OFFICERS_gridDiv&BorrowIn').style.display = 'inline-block';
    document.getElementById('st_OFFICERS_gridDiv&BorrowOut').style.display = 'inline-block';
    document.getElementById('st_OFFICERS_gridDiv&outforce').style.display = 'inline-block';
    document.getElementById('st_OFFICERS_gridDiv&addoff').style.display = 'inline-block';
    document.getElementById('st_OFFICERS_gridDiv&edit').style.display = 'inline-block';
    document.getElementById('st_OFFICERS_gridDiv&delete').style.display = 'inline-block';
    document.getElementById('st_OFFICERS_gridDiv&addrelivants').style.display = 'inline-block';



    //fill dropdown of borrow_units use option_forces
    $("body").on('click', ".optionOutForce", function () {
        var force_value = $(this).val();
        if (force_value == '2' || force_value == '3' || force_value == '4') {

            document.getElementById('BorrowID').style.display = 'inline-block';

        } else {
            document.getElementById('BorrowID').style.display = 'none';

        }
    });



    document.getElementById('st_OFFICERS_gridDiv&force').onclick = (function () {

        bind_OFFICERS_grid();
    });

    document.getElementById('st_OFFICERS_gridDiv&BorrowIn').onclick = (function () {

        OFFICERsGridBind_BorrowIn();
    });
    document.getElementById('st_OFFICERS_gridDiv&BorrowOut').onclick = (function () {

        OFFICERsGridBind_BorrowOut();
    });

   document.getElementById('st_OFFICERS_gridDiv&outforce').onclick = (function () {

        OFFICERsGridBind_outforce();
    });

   document.getElementById('st_OFFICERS_gridDiv&addrelivants').onclick = (function () {

       var rowindex = $("#st_OFFICERS_gridDiv").jqxGrid("getselectedrowindex");
       var rowdata = $("#st_OFFICERS_gridDiv").jqxGrid("getrowdata", rowindex);
 
       var Personcode = rowdata.PERSON_CODE;
       var url = "../WORKFLOW_APP/Relivants?Person_Code=" + Personcode
       window.open(url, "_blank");
   });

    //****************close Overlay Window*******************************************************************

    $("#x").click(function () {
        $(".Big_overlay").fadeOut("slow");
    });

    document.getElementById('st_OFFICERS_gridDiv&addoff').onclick = (function () {
        resetInputs();
        $(".Big_overlay").fadeIn("slow");
        $("#AddInfoBtn").fadeIn("slow");
        $("#UpdateInfoBtn").css({ display: "none" });

    });
    //********************************************************************************************
    //button update
    document.getElementById('st_OFFICERS_gridDiv&edit').onclick = (function () {
        debugger
     
        resetInputs();
        var rowindex = $("#st_OFFICERS_gridDiv").jqxGrid("getselectedrowindex");
        var rowdata = $("#st_OFFICERS_gridDiv").jqxGrid("getrowdata", rowindex);
        debugger
        // get all data of row
        var PersonCode = rowdata.PERSON_CODE;
        $("#UpdateInfoBtn").attr("data", PersonCode);
        var IdNumber = rowdata.ID_NO;
        var SortNumber = rowdata.SORT_NO;
        var national_id = rowdata.PERSONAL_ID_NO;
        var RANK_CODE = rowdata.RANK_ID;
        var PersonName = rowdata.PERSON_NAME;
        var GraduationName = rowdata.GRADUATION_NAME;
        var DepartmentId = rowdata.DEPARTMENT_ID;
        var SpecializationId = rowdata.SPECIALIZATION_ID;
        var BirthPlace = rowdata.BIRTH_PLACE;
        var JobTypeId = rowdata.JOB_TYPE_ID;
        var ReligionId = rowdata.RELIGION_ID;
        var BloodTypeId = rowdata.BLOOD_TYPE_ID;
        var Phone = rowdata.PHONE1;

        var borrow_code = rowdata.BORROW_FIRM_CODE;
        if (borrow_code == null) {
            $("#BorrowID").val("");
        }
        var borrow_status = rowdata.BORROW_STATUS;
        var out_unForce = rowdata.OUT_UN_FORCE;

        var BirthDate = rowdata.BIRTHDATE;
        if (BirthDate == null) {
            BirthDate == "";
        } else {
            BirthDate = getFormattedDate(BirthDate);
        }
        var HireDate = rowdata.HIRE_DATE;
        if (HireDate == null) {
            HireDate == "";
        } else {
            HireDate = getFormattedDate(HireDate);
        }


        var JoinDate = rowdata.JOIN_DATE;
        if (JoinDate == null) {
            JoinDate == "";
        } else {
            JoinDate = getFormattedDate(JoinDate);
        }


        var CurrentRankDate = rowdata.CURRENT_RANK_DATE;
        if (CurrentRankDate == null) {
            CurrentRankDate == "";
        } else {
            CurrentRankDate = getFormattedDate(CurrentRankDate);
        }


        var LeaveDate = rowdata.LEAVE_DATE;
        if (LeaveDate == null) {
            LeaveDate == "";
        } else {
            LeaveDate = getFormattedDate(LeaveDate);
        }


        var NextRankDate = rowdata.NEXT_RANK_DATE;
        if (NextRankDate == null) {
            NextRankDate == "";
        } else {
            NextRankDate = getFormattedDate(NextRankDate);
        }
        var Address = rowdata.ADDERESS;

        //fill inputs with data to update
        $("#RkmaskryID").val(IdNumber);
        $("#RkmAdmyaID").val(SortNumber); 
        $("#NationalID").val(national_id);
        $("#RANK_CODE").val(RANK_CODE);        
        $("#OffNameID").val(PersonName);
        $("#DofNumID").val(GraduationName);
        $("#ArmyDepartments").val(DepartmentId);      
        $("#CategoryID").val(SpecializationId);      
        $("#GovernmentID").val(BirthPlace);
        $("#jobs").val(JobTypeId);   
        $("#ReligionID").val(ReligionId); 
        $("#BloodID").val(BloodTypeId);
        $("#PhoneNumberID").val(Phone);

        if (out_unForce == 0 && borrow_status ==1 ) {
            $("#OutForceID").val(2);
            document.getElementById('BorrowID').style.display = 'inline-block';
            $("#BorrowID").val(borrow_code);
        } else if (out_unForce == 0 && borrow_status == 2) {
            $("#OutForceID").val(3);
            document.getElementById('BorrowID').style.display = 'inline-block';
            $("#BorrowID").val(borrow_code);
        } else if (out_unForce == 1 && borrow_status == 2) {
            $("#OutForceID").val(4);
            document.getElementById('BorrowID').style.display = 'inline-block';
            $("#BorrowID").val(borrow_code);
        } else {
            $("#OutForceID").val(1);
            document.getElementById('BorrowID').style.display = 'none';
        }

        $("#DateOfBirth").val(BirthDate);
        $("#GraduationDate").val(HireDate);
        $("#CombineDate").val(JoinDate);
        $("#CurrentPromotionDate").val(CurrentRankDate);
        $("#RenewalDate").val(LeaveDate);
        $("#ComingPromotionDate").val(NextRankDate);
        $("#AddressID").val(Address);

        if (PersonCode == "" || IdNumber == "" || national_id == "" || SortNumber == "" || RANK_CODE == "" || PersonName == "" || GraduationName == "" ||
            DepartmentId == "" || SpecializationId == "" || BirthPlace == "" || JobTypeId == "" || ReligionId == "" || BloodTypeId == "" ||
            Phone == "" || borrow_code == "" || borrow_status == "" || BirthDate == "" || HireDate == ""
            || JoinDate == "" || CurrentRankDate == "" || LeaveDate == "" ||
            NextRankDate == "" || Address == "") {

            $(".Big_overlay").fadeIn("slow");
            $("#UpdateInfoBtn").fadeIn("slow");
            $("#AddInfoBtn").css({ display: "none" });

        } else {


            $(".Big_overlay").fadeIn("slow");
            $("#UpdateInfoBtn").fadeIn("slow");
            $("#AddInfoBtn").css({ display: "none" });
        }
    });
    //********************************************************************************************
    // ADD Officesrs
    $("body").on('click', "#AddInfoBtn", function () {
        debugger
        var officer_number = $("#RkmaskryID").val();
        var senior_number = $("#RkmAdmyaID").val();
        var national_id =  $("#NationalID").val();
        var rank_code = $("#RANK_CODE").val();
        var officer_name = $("#OffNameID").val();
        var patch_name = $("#DofNumID").val();
        var army_departments = $("#ArmyDepartments").val();
        var united_code = $("#FIRMS_CODE").val();
        var category_code = $("#CategoryID").val();
        var government_name = $("#GovernmentID").val();
        var job_code = $("#jobs").val();
        var religion_code = $("#ReligionID").val();
        var blood_code = $("#BloodID").val();
        var phone_number = $("#PhoneNumberID").val();

        var force_val = $("#OutForceID").val();
        if (force_val == '2') {
            var out_unForce = '0';
            var borrow_status = '1';
            var borrow_code = $("#BorrowID").val();

        } else if (force_val == '3') {
            out_unForce = '0';
            borrow_status = '2';
            borrow_code = $("#BorrowID").val();

        } else if (force_val == '4') {
            out_unForce = '1';
            borrow_status = '2';
            borrow_code = $("#BorrowID").val();
        } else {
            borrow_status = '0';
            out_unForce = '0';
            borrow_code = $("#FIRMS_CODE").val();
        }

        var dateOfBirth = getInsertionDate("#DateOfBirth");
        var graduation_date = getInsertionDate("#GraduationDate");
        var combine_date = getInsertionDate("#CombineDate");
        var current_promotion_date = getInsertionDate("#CurrentPromotionDate");
        var renewal_date = getInsertionDate("#RenewalDate");
        var coming_promotion_date = getInsertionDate("#ComingPromotionDate");
        var address = $("#AddressID").val();

        // check of insertion of data
        if (officer_number == "" || force_val == 0 || senior_number == "" || national_id == "" || rank_code == "" || officer_name == "" || patch_name == "" || army_departments == "" ||
            category_code == "" || government_name == "" || borrow_code == "" || religion_code == "" || blood_code == "" || phone_number == "" || dateOfBirth == "undefined/undefined/" ||
            graduation_date == "undefined/undefined/" || combine_date == "undefined/undefined/" || current_promotion_date == "undefined/undefined/"
            || renewal_date == "undefined/undefined/" || coming_promotion_date == "undefined/undefined/" || address == "") {

            swal({
                title: "خطأ ",
                text: "إدخل البيانات كاملة",
                type: "error",
                timer: 2500,
                showConfirmButton: true,
                confirmButtonText: "نعم"

            });
            //alert("إدخل البيانات كاملة");

        } else {
            //ajax for insert into person data
            $.ajax({

                url: 'OFFICERS/insertData',
                data: JSON.stringify({
                    officer_number: officer_number,
                    senior_number: senior_number,
                    national_id : national_id ,
                    rank_code: rank_code,
                    officer_name: officer_name,
                    patch_name: patch_name,
                    army_departments: army_departments,
                    united_code: united_code,
                    category_code: category_code,
                    government_name: government_name,
                    job_code: job_code,
                    religion_code: religion_code,
                    blood_code: blood_code,
                    phone_number: phone_number,
                    borrow_code: borrow_code,
                    borrow_status: borrow_status,
                    out_unForce : out_unForce ,
                    dateOfBirth: dateOfBirth,
                    graduation_date: graduation_date,
                    combine_date: combine_date,
                    current_promotion_date: current_promotion_date,
                    renewal_date: renewal_date,
                    coming_promotion_date: coming_promotion_date,
                    address: address

                }),
                cache: false,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST",
                success: function (data) {

                    if (data == 1) {
                        bind_OFFICERS_grid();
                        resetInputs();
                        swal({
                            title: "تم الإضافة ",
                            text: "تم الإضافة بنجاح",
                            type: "success",
                            timer: 5000,
                            showConfirmButton: true,
                            confirmButtonText: "نعم"

                        });
                        
                      //  alert("تم الإضافة بنجاح");

                    } else {
                        swal({
                            title: "خطأ ",
                            text: "لم يتم الإضافة",
                            type: "error",
                            timer: 2500,
                            showConfirmButton: true,
                            confirmButtonText: "نعم"

                        });


                    }

                },
                error: function (response) {
                    alert('not inserted');
                }
            });
        }


    });

    //********************************************************************************************
    // UPDATE Officesrs
    $("body").on('click', "#UpdateInfoBtn", function () {
        debugger
        var PersonCode = $("#UpdateInfoBtn").attr("data");
        var officer_number = $("#RkmaskryID").val();
        var senior_number = $("#RkmAdmyaID").val();
        var national_id = $("#NationalID").val();
        var rank_code = $("#RANK_CODE").val();
        var officer_name = $("#OffNameID").val();
        var patch_name = $("#DofNumID").val();
        var army_departments = $("#ArmyDepartments").val();
        //var unite_code = $("#FIRMS_CODE").val();
        var category_code = $("#CategoryID").val();
        var government_name = $("#GovernmentID").val();
        var job_code = $("#jobs").val();
        var religion_code = $("#ReligionID").val();
        var blood_code = $("#BloodID").val();
        var phone_number = $("#PhoneNumberID").val();

        var force_val = $("#OutForceID").val();
        if (force_val == '2') {
            var out_unForce = '0';
            var borrow_status = '1';
            var borrow_code = $("#BorrowID").val();

        } else if (force_val == '3') {
             out_unForce = '0';
             borrow_status = '2';
             borrow_code = $("#BorrowID").val();

        } else if (force_val == '4') {
             out_unForce = '1';
             borrow_status = '2';
             borrow_code = $("#BorrowID").val();
        } else {
            borrow_status = '0';
            out_unForce = '0';
            borrow_code = $("#FIRMS_CODE").val();
        }

        var dateOfBirth = getInsertionDate("#DateOfBirth");
        var graduation_date = getInsertionDate("#GraduationDate");
        var combine_date = getInsertionDate("#CombineDate");
        var current_promotion_date = getInsertionDate("#CurrentPromotionDate");
        var renewal_date = getInsertionDate("#RenewalDate");
        var coming_promotion_date = getInsertionDate("#ComingPromotionDate");
        var address = $("#AddressID").val();
        debugger
        // check of insertion of data
        if (officer_number == "" || senior_number == "" || national_id == "" || rank_code == "" || officer_name == "" || patch_name == "" || army_departments == "" ||
            category_code == "" || government_name == "" || job_code == "" || religion_code == "" || blood_code == "" || phone_number == "" || dateOfBirth == "undefined/undefined/" ||
            graduation_date == "undefined/undefined/" || combine_date == "undefined/undefined/" || current_promotion_date == "undefined/undefined/"
            || renewal_date == "undefined/undefined/" || coming_promotion_date == "undefined/undefined/" || address == "") {

            swal({
                title: "خطأ ",
                text: "إدخل البيانات كاملة",
                type: "error",
                //timer: 2500 
                showConfirmButton: true,
                confirmButtonText: "نعم"

            });
            //alert("إدخل البيانات كاملة");

        } else {
            //ajax for update into person data
            $.ajax({
                url: 'OFFICERS/updateData',
                data: JSON.stringify({
                    PersonCode: PersonCode,
                    officer_number: officer_number,
                    senior_number: senior_number,
                    national_id : national_id ,
                    rank_code: rank_code,
                    officer_name: officer_name,
                    patch_name: patch_name,
                    army_departments: army_departments,
                    //unite_code: unite_code,
                    category_code: category_code,
                    government_name: government_name,
                    job_code: job_code,
                    religion_code: religion_code,
                    blood_code: blood_code,
                    phone_number: phone_number,
                    borrow_code: borrow_code,
                    borrow_status: borrow_status,
                    out_unForce : out_unForce ,
                    dateOfBirth: dateOfBirth,
                    graduation_date: graduation_date,
                    combine_date: combine_date,
                    current_promotion_date: current_promotion_date,
                    renewal_date: renewal_date,
                    coming_promotion_date: coming_promotion_date,
                    address: address

                }),
                cache: false,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST",
                success: function (data) {
                    if (data == 1) {
                        bind_OFFICERS_grid();

                        swal({
                            title: "تم التعديل ",
                            text: "تم التعديل بنجاح",
                            type: "success",
                            timer: 5000,
                            showConfirmButton: true,
                            confirmButtonText: "نعم"

                        });

                        //alert("تم التعديل بنجاح");

                    } else {
                        swal({
                            title: "خطأ ",
                            text: "لم يتم التعديل",
                            type: "error",
                            timer: 2500,
                            showConfirmButton: true,
                            confirmButtonText: "نعم"

                        });


                    }
                },
                error: function (response) {
                    swal({
                        title: "خطأ ",
                        type: "error",
                        timer: 2500,
                        showConfirmButton: true,
                        confirmButtonText: "نعم"

                    });
                }
            });
        }





    });

    //********************************************************************************************
    // DELETE OFFICERS
    document.getElementById('st_OFFICERS_gridDiv&delete').onclick = (function () {
        var rowindex = $("#st_OFFICERS_gridDiv").jqxGrid("getselectedrowindex");
        var rowdata = $("#st_OFFICERS_gridDiv").jqxGrid("getrowdata", rowindex);
        var PersonCode = rowdata.PERSON_CODE;

        $.ajax({
            url: 'OFFICERS/delete_Off',
            data: JSON.stringify({
                PersonCode: PersonCode
            }),
            cache: false,
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
            success: function (data) {

                if (data == 1) {
                    bind_OFFICERS_grid();

                    swal({
                        title: "تم الحذف ",
                        text: "تم الحذف بنجاح",
                        type: "success",
                        timer: 5000,
                        showConfirmButton: true,
                        confirmButtonText: "نعم"

                    });
                    //alert("تم الحذف بنجاح");

                } else {
                    swal({
                        title: "خطأ ",
                        text: "لم يتم الحذف",
                        type: "error",
                        timer: 2500,
                        showConfirmButton: true,
                        confirmButtonText: "نعم"

                    });


                }
            },
            error: function (response) {
                swal({
                    title: "خطأ ",
                    type: "error",
                    timer: 2500,
                    showConfirmButton: true,
                    confirmButtonText: "نعم"

                });
            }
        });
    });
    //********************************************************************************************

});

//******************************************************** out of ready *******************************************

//resest data
function resetInputs() {
    var zero = '0';
    $("#RkmaskryID").val("");
    $("#RkmAdmyaID").val("");
    $("#NationalID").val("");
    $("#RANK_CODE").val("");
    $("#OffNameID").val("");
    $("#DofNumID").val("");
    $("#ArmyDepartments").val("");
    $("#CategoryID").val("");
    $("#GovernmentID").val("");
    $("#jobs").val("");
    $("#OutForceID").val(zero);
    $("#BorrowID").val("");
    document.getElementById('BorrowID').style.display = 'none';
    $("#ReligionID").val(zero);
    $("#BloodID").val("");
    $("#PhoneNumberID").val("");
    $("#DateOfBirth").val("");
    $("#GraduationDate").val("");
    $("#CombineDate").val("");
    $("#CurrentPromotionDate").val("");
    $("#RenewalDate").val("");
    $("#ComingPromotionDate").val("");
    $("#AddressID").val("");



}

//format date to insert
function getInsertionDate(DateId) {
    var Date = $(DateId).val();
    var splitDate = Date.split("-");
    var year = splitDate[0];
    var month = splitDate[1];
    var day = splitDate[2];
    Date = day + '/' + month + '/' + year;
    return Date;
}
//format date to update
function getFormattedDate(DateId) {
    debugger
    var splitDate = DateId.split("/");
    var day = splitDate[0];
    var month = splitDate[1];
    var year = splitDate[2];
    DateId = year + '-' + month + '-' + day;
    return DateId;
}
//*************************************************************************


function build_static() {
    debugger
    // $("#DT_DATE").jqxDateTimeInput({ formatString: "T", showTimeButton: true, showCalendarButton: false, width: '200px', height: '25px', theme: 'redmond' });
    //$('#DT_DATE').jqxDateTimeInput({ animationType: 'fade', width: '80%', height: 38, rtl: true, dropDownHorizontalAlignment: 'right', theme: "darkblue" });
    $('#TXT_FIN_YEAR').jqxInput({ width: '80%', height: 38, theme: 'darkblue', disabled: true });
    $('#TXT_PERIOD').jqxInput({ width: '80%', height: 38, theme: 'darkblue', disabled: true });
    //$('#page_name').text('  تمام الضــباط');
  //  $('#jqxTabs').jqxTabs({ width: '80%', height: 500, rtl: true, theme: 'darkblue' });
    // $('#jqxTabs').jqxTabs({ width: '80%', height: 300, rtl: true, theme: 'darkblue' });
    // $('#txt_mission_introduction').jqxInput({ width: '85%', height: 70, theme: 'darkblue', disabled: true });
    // $('#txt_mission_subject').jqxInput({ width: '85%', height: 70, theme: 'darkblue', disabled: true });
    //  $('#txt_mission_distribution').jqxInput({ width: '85%', height: 70, theme: 'darkblue', disabled: true });

}

// buildand bind officers grid
function build_OFFICERS_grid() {


    gridId = 'st_OFFICERS_gridDiv';

    theme = "darkblue";
    headerText = " بيانات الضباط";
    gridAddUrl = ControllerName + 'Create';
    $("#st_OFFICERS_gridDiv").jqxGrid({
        width: '80%',
        height: 450,
        //pageable: true,
        theme: "darkblue",
        sortable: true,
        rtl: true,
        //showaggregates: true,
        // showstatusbar: true,    
        // statusbarheight: 50,
        filterable: true,
        showfilterrow: true,
        showtoolbar: true,
        //   selectionmode: 'singlerow',
        //source: dataAdapter,
        rendertoolbar: toolbarfn,
        columns: [
                      { text: 'الرقم العسكري', dataField: 'ID_NO', cellsalign: 'center', align: 'center', width: 110 },
                      { text: 'رقم الاقدمية', dataField: 'SORT_NO', cellsalign: 'center', align: 'center', width: 90 },
                      { text: 'رقم قومى', dataField: 'PERSONAL_ID_NO', cellsalign: 'center', align: 'center', width: 120 },

                      { text: 'الرتبة', dataField: 'RANK', cellsalign: 'center', align: 'center', width: 90 },
                      { text: 'الاسم', dataField: 'PERSON_NAME', cellsalign: 'center', align: 'center', width: 300 },
                      { text: 'رقم الدفعة', dataField: 'GRADUATION_NAME', cellsalign: 'center', align: 'center', width: 100 },
                      { text: 'السلاح', dataField: 'DEPARTMENT_NAME', cellsalign: 'center', align: 'center', width: 110 },
                      { text: 'الوحدة', dataField: 'FIRM_NAME', cellsalign: 'center', align: 'center', width: 250 },
                      { text: 'الوظيفة', dataField: 'JOB_NAME', cellsalign: 'center', align: 'center', width: 250 },
                      { text: 'فئة ', dataField: 'SPECIALIZATION_ID', cellsalign: 'center', align: 'center', width: 100, hidden: true },
                      { text: 'فئة التخصص', dataField: 'SPECIALIZATIONS_NAME', cellsalign: 'center', align: 'center', width: 100 },
                      { text: 'تاريخ الميلاد', dataField: 'BIRTHDATE', cellsalign: 'center', align: 'center', width: 110 },
                      { text: 'المحافظة', dataField: 'BIRTH_PLACE', cellsalign: 'center', align: 'center', width: 150 },
                      { text: 'فصيلة الدم', dataField: 'BLOOD_TYPE_NAME', cellsalign: 'center', align: 'center', width: 150 },
                      { text: 'الديانة', dataField: 'RELIGION_NAME', cellsalign: 'center', align: 'center', width: 112 },

                      { text: 'PERSON_CODE', dataField: 'PERSON_CODE', cellsalign: 'center', align: 'center', width: 500 , hidden : true },
                      { text: 'OUT_UN_FORCE', dataField: 'OUT_UN_FORCE', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'BORROW_STATUS', dataField: 'BORROW_STATUS', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'BORROW_FIRM_CODE', dataField: 'BORROW_FIRM_CODE', cellsalign: 'center', align: 'center', width: 200, hidden: true },


                      { text: 'تاريخ التخرج', dataField: 'HIRE_DATE', cellsalign: 'center', align: 'center', width: 110 },
                      { text: 'تاريخ الضم ع القوة', dataField: 'JOIN_DATE', cellsalign: 'center', align: 'center', width: 130 },
                      { text: 'تاريخ الترقي للرتبة الحالية', dataField: 'CURRENT_RANK_DATE', cellsalign: 'center', align: 'center', width: 200 },
                      { text: 'تاريخ التجديد', dataField: 'LEAVE_DATE', cellsalign: 'center', align: 'center', width: 110 },
                      { text: 'تاريخ الترقي للرتبة القادمة', dataField: 'NEXT_RANK_DATE', cellsalign: 'center', align: 'center', width: 200 },
                      { text: 'اخر امر نقل', dataField: 'TRANSFER_NO', cellsalign: 'center', align: 'center', width: 120 },
                      { text: 'تاريخ ضم النظم', dataField: 'NOZOM_DATE', cellsalign: 'center', align: 'center', width: 150 },
                      { text: 'الجنس', dataField: 'SEX', cellsalign: 'center', align: 'center', width: 100, hidden: true },
                      { text: 'عدد مرات الزواج', dataField: 'MARRIGE_CONT', cellsalign: 'center', align: 'center', width: 120 },
                      { text: 'COMMUNITY_NO', dataField: 'COMMUNITY_NO', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'اركان حرب', dataField: 'ARKAN_HARB', cellsalign: 'center', align: 'center', width: 100 },
                      { text: 'عدد البنات', dataField: 'DAUGHTERS_COUNT', cellsalign: 'center', align: 'center', width: 100 },
                      { text: 'عدد الابناء', dataField: 'SONS_COUNT', cellsalign: 'center', align: 'center', width: 100 },
                      { text: 'رقم الهاتف 2', dataField: 'PHONE_2', cellsalign: 'center', align: 'center', width: 200 },
                      { text: 'رقم الهاتف 1', dataField: 'PHONE1', cellsalign: 'center', align: 'center', width: 200 },
                      { text: 'العنوان', dataField: 'ADDERESS', cellsalign: 'center', align: 'center', width: 400 },
                      { text: 'PERSON_CAT_ID', dataField: 'PERSON_CAT_ID', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'RANK_CAT_ID', dataField: 'RANK_CAT_ID', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'SOCIAL_STATE_ID', dataField: 'SOCIAL_STATE_ID', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'كود الوحدة', dataField: 'FIRM_CODE', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'PARENT_STATUS_ID', dataField: 'PARENT_STATUS_ID', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'ID_TYPE_ID', dataField: 'ID_TYPE_ID', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'AGE_CATEGORY_ID', dataField: 'AGE_CATEGORY_ID', cellsalign: 'center', align: 'center', width: 500, hidden: true },
                      { text: 'كود فصيلة الدم', dataField: 'BLOOD_TYPE_ID', cellsalign: 'center', align: 'center', width: 500, hidden: true },
                      { text: 'كود الديانة', dataField: 'RELIGION_ID', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'SECTOR_ID', dataField: 'SECTOR_ID', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'GOVERNERATE_ID', dataField: 'GOVERNERATE_ID', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'كود الوظيفة', dataField: 'JOB_TYPE_ID', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'DEPARTMENT_ID', dataField: 'DEPARTMENT_ID', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'كود الرتبة', dataField: 'RANK_ID', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'CATEGORY_ID', dataField: 'CATEGORY_ID', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'مقاس الحذاء', dataField: 'SHOE_SIZE', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'مقاس البدلة', dataField: 'SUIT_SIZE', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'مقاس الافارول', dataField: 'OVERALL_SIZE', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'مقاس القناع', dataField: 'MASK_SIZE', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'RANK_RENEW_DATE', dataField: 'RANK_RENEW_DATE', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'FIELD_SERVICE_ABILITY', dataField: 'FIELD_SERVICE_ABILITY', cellsalign: 'center', align: 'center', width: 500, hidden: true },
                      { text: 'FEED_REPLACE', dataField: 'FEED_REPLACE', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'P_ACCEDENT', dataField: 'P_ACCEDENT', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'P_RANKING', dataField: 'P_RANKING', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'P_COURSES', dataField: 'P_COURSES', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'P_TRAVEL', dataField: 'P_TRAVEL', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'P_LANGUAGE', dataField: 'P_LANGUAGE', cellsalign: 'center', align: 'center', width: 500, hidden: true },
                      { text: 'P_PUNISHMENTS', dataField: 'P_PUNISHMENTS', cellsalign: 'center', align: 'center', width: 500, hidden: true },
                      { text: 'P_BATTELES', dataField: 'P_BATTELES', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'P_JOBS', dataField: 'P_JOBS', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'T_JOBS', dataField: 'T_JOBS', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'P_HOSPITALS', dataField: 'P_HOSPITALS', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'P_INVESTGATIONS', dataField: 'P_INVESTGATIONS', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'PERSON_MEDALS', dataField: 'PERSON_MEDALS', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'PERSON_STUDIES', dataField: 'PERSON_STUDIES', cellsalign: 'center', align: 'center', width: 500, hidden: true },
                      { text: 'PERSONS_RANKING', dataField: 'PERSONS_RANKING', cellsalign: 'center', align: 'center', width: 500, hidden: true },
                      { text: 'PERSON_COMPLAINS', dataField: 'PERSON_COMPLAINS', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'امتياز', dataField: 'PERSON_EXCELANT', columngroup: 'grade', cellsalign: 'center', align: 'center', width: 70 },
                      { text: 'جيد ج', dataField: 'PERSON_V_G', columngroup: 'grade', cellsalign: 'center', align: 'center', width: 70 },
                      { text: 'جيد', dataField: 'PERSON_G', columngroup: 'grade', cellsalign: 'center', align: 'center', width: 70 },
                      { text: 'مقبول', dataField: 'PERSON_ACC', columngroup: 'grade', cellsalign: 'center', align: 'center', width: 70 },
                      { text: 'ضعيف', dataField: 'PERSON_WEAK', columngroup: 'grade', cellsalign: 'center', align: 'center', width: 70 },
                      { text: 'lgh', dataField: 'lgh', cellsalign: 'center', align: 'center', width: 200, hidden: true },
                      { text: 'wh', dataField: 'wh', cellsalign: 'center', align: 'center', width: 200, hidden: true }
        ]


    });
    $("#st_OFFICERS_gridDiv").jqxGrid({ enabletooltips: true });
}
function bind_OFFICERS_grid() {

    $('#st_OFFICERS_gridDiv').jqxGrid('clearselection');
    $('#st_OFFICERS_gridDiv').jqxGrid('clear');
    var source = {
        datatype: "json",
        datafields: [
        { name: 'OUT_UN_FORCE' },
          { name: 'JOB_NAME' },
          { name: 'HIRE_DATE' },
          { name: 'CURRENT_RANK_DATE' },
          { name: 'NEXT_RANK_DATE' },
          { name: 'LEAVE_DATE' },
          { name: 'JOIN_DATE' },
          { name: 'SEX' },
          { name: 'MARRIGE_CONT' },
          { name: 'COMMUNITY_NO' },
          { name: 'ARKAN_HARB' },
          { name: 'DAUGHTERS_COUNT' },
          { name: 'SONS_COUNT' },
          { name: 'BIRTH_PLACE' },
          { name: 'SORT_NO' },
          { name: 'ID_NO' },
          { name: 'PHONE_2' },
          { name: 'PHONE1' },
          { name: 'BIRTHDATE' },
          { name: 'ADDERESS' },
          { name: 'PERSON_NAME' },
          {name : 'PERSONAL_ID_NO'},
          { name: "BORROW_STATUS" },
          { name: "BORROW_FIRM_CODE" },
          { name: 'PERSON_CAT_ID' },
          { name: 'RANK_CAT_ID' },
          { name: 'SOCIAL_STATE_ID' },
          { name: 'FIRM_CODE' },
          { name: 'FIRM_NAME' },
          { name: 'PARENT_STATUS_ID' },
          { name: 'ID_TYPE_ID' },
          { name: 'AGE_CATEGORY_ID' },
          { name: 'BLOOD_TYPE_ID' },
          { name: 'RELIGION_ID' },
          { name: 'SECTOR_ID' },
          { name: 'GOVERNERATE_ID' },
          { name: 'JOB_TYPE_ID' },
          { name: 'DEPARTMENT_ID' },
          { name: 'DEPARTMENT_NAME' },
          { name: 'RANK_ID' },
          { name: 'RANK' },
          { name: 'PERSON_CODE' },
          { name: 'GRADUATION_NAME' },
          { name: 'CATEGORY_ID' },
          { name: 'SHOE_SIZE' },
          { name: 'SUIT_SIZE' },
          { name: 'OVERALL_SIZE' },
          { name: 'MASK_SIZE' },
          { name: 'RANK_RENEW_DATE' },
          { name: 'TRANSFER_NO' },
          { name: 'FIELD_SERVICE_ABILITY' },
          { name: 'FEED_REPLACE' },
          { name: 'NOZOM_DATE' },
          { name: 'P_ACCEDENT' },
          { name: 'P_RANKING' },
          { name: 'P_COURSES' },
          { name: 'P_TRAVEL' },
          { name: 'P_LANGUAGE' },
          { name: 'P_PUNISHMENTS' },
          { name: 'P_BATTELES' },
          { name: 'P_JOBS' },
          { name: 'T_JOBS' },
          { name: 'P_HOSPITALS' },
          { name: 'P_INVESTGATIONS' },
          { name: 'PERSON_MEDALS' },
          { name: 'PERSON_STUDIES' },
          { name: 'PERSONS_RANKING' },
          { name: 'PERSON_COMPLAINS' },
          { name: 'PERSON_EXCELANT' },
          { name: 'PERSON_V_G' },
          { name: 'PERSON_G' },
          { name: 'PERSON_ACC' },
          { name: 'PERSON_WEAK' },
          { name: 'lgh' },
          { name: 'wh' },
          { name: 'SPECIALIZATION_ID' },
          { name: 'SPECIALIZATIONS_NAME' },
          { name: 'BLOOD_TYPE_NAME' },
          { name: 'RELIGION_NAME' }
        ],

        async: false,

        url: 'OFFICERS/bind_data'

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_OFFICERS_gridDiv").jqxGrid({ source: dataAdapter });
}

// buildand bind officers grid outForce
function OFFICERsGridBind_outforce() {


    $('#st_OFFICERS_gridDiv').jqxGrid('clearselection');
    $('#st_OFFICERS_gridDiv').jqxGrid('clear');
    var source = {
        datatype: "json",
        datafields: [
        { name: 'OUT_UN_FORCE' },
          { name: 'JOB_NAME' },
          { name: 'HIRE_DATE' },
          { name: 'CURRENT_RANK_DATE' },
          { name: 'NEXT_RANK_DATE' },
          { name: 'LEAVE_DATE' },
          { name: 'JOIN_DATE' },
          { name: 'SEX' },
          { name: 'MARRIGE_CONT' },
          { name: 'COMMUNITY_NO' },
          { name: 'ARKAN_HARB' },
          { name: 'DAUGHTERS_COUNT' },
          { name: 'SONS_COUNT' },
          { name: 'BIRTH_PLACE' },
          { name: 'SORT_NO' },
          { name: 'ID_NO' },
          { name: 'PHONE_2' },
          { name: 'PHONE1' },
          { name: 'BIRTHDATE' },
          { name: 'ADDERESS' },
          { name: 'PERSON_NAME' },
          {name : 'PERSONAL_ID_NO'},
          { name: "BORROW_STATUS" },
          { name: "BORROW_FIRM_CODE" },
          { name: 'PERSON_CAT_ID' },
          { name: 'RANK_CAT_ID' },
          { name: 'SOCIAL_STATE_ID' },
          { name: 'FIRM_CODE' },
          { name: 'FIRM_NAME' },
          { name: 'PARENT_STATUS_ID' },
          { name: 'ID_TYPE_ID' },
          { name: 'AGE_CATEGORY_ID' },
          { name: 'BLOOD_TYPE_ID' },
          { name: 'RELIGION_ID' },
          { name: 'SECTOR_ID' },
          { name: 'GOVERNERATE_ID' },
          { name: 'JOB_TYPE_ID' },
          { name: 'DEPARTMENT_ID' },
          { name: 'DEPARTMENT_NAME' },
          { name: 'RANK_ID' },
          { name: 'RANK' },
          { name: 'PERSON_CODE' },
          { name: 'GRADUATION_NAME' },
          { name: 'CATEGORY_ID' },
          { name: 'SHOE_SIZE' },
          { name: 'SUIT_SIZE' },
          { name: 'OVERALL_SIZE' },
          { name: 'MASK_SIZE' },
          { name: 'RANK_RENEW_DATE' },
          { name: 'TRANSFER_NO' },
          { name: 'FIELD_SERVICE_ABILITY' },
          { name: 'FEED_REPLACE' },
          { name: 'NOZOM_DATE' },
          { name: 'P_ACCEDENT' },
          { name: 'P_RANKING' },
          { name: 'P_COURSES' },
          { name: 'P_TRAVEL' },
          { name: 'P_LANGUAGE' },
          { name: 'P_PUNISHMENTS' },
          { name: 'P_BATTELES' },
          { name: 'P_JOBS' },
          { name: 'T_JOBS' },
          { name: 'P_HOSPITALS' },
          { name: 'P_INVESTGATIONS' },
          { name: 'PERSON_MEDALS' },
          { name: 'PERSON_STUDIES' },
          { name: 'PERSONS_RANKING' },
          { name: 'PERSON_COMPLAINS' },
          { name: 'PERSON_EXCELANT' },
          { name: 'PERSON_V_G' },
          { name: 'PERSON_G' },
          { name: 'PERSON_ACC' },
          { name: 'PERSON_WEAK' },
          { name: 'lgh' },
          { name: 'wh' },
          { name: 'SPECIALIZATION_ID' },
          { name: 'SPECIALIZATIONS_NAME' },
          { name: 'BLOOD_TYPE_NAME' },
          { name: 'RELIGION_NAME' }
        ],

        async: false,

        url: 'OFFICERS/bind_data_outforce'

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_OFFICERS_gridDiv").jqxGrid({ source: dataAdapter });

}
// buildand bind officers grid borrowIn
function OFFICERsGridBind_BorrowIn() {


    $('#st_OFFICERS_gridDiv').jqxGrid('clearselection');
    $('#st_OFFICERS_gridDiv').jqxGrid('clear');
    var source = {
        datatype: "json",
        datafields: [
        { name: 'OUT_UN_FORCE' },
          { name: 'JOB_NAME' },
          { name: 'HIRE_DATE' },
          { name: 'CURRENT_RANK_DATE' },
          { name: 'NEXT_RANK_DATE' },
          { name: 'LEAVE_DATE' },
          { name: 'JOIN_DATE' },
          { name: 'SEX' },
          { name: 'MARRIGE_CONT' },
          { name: 'COMMUNITY_NO' },
          { name: 'ARKAN_HARB' },
          { name: 'DAUGHTERS_COUNT' },
          { name: 'SONS_COUNT' },
          { name: 'BIRTH_PLACE' },
          { name: 'SORT_NO' },
          { name: 'ID_NO' },
          { name: 'PHONE_2' },
          { name: 'PHONE1' },
          { name: 'BIRTHDATE' },
          { name: 'ADDERESS' },
          { name: 'PERSON_NAME' },
          { name: 'PERSONAL_ID_NO' },
          { name: "BORROW_STATUS" },
          { name: "BORROW_FIRM_CODE" },
          { name: 'PERSON_CAT_ID' },
          { name: 'RANK_CAT_ID' },
          { name: 'SOCIAL_STATE_ID' },
          { name: 'FIRM_CODE' },
          { name: 'FIRM_NAME' },
          { name: 'PARENT_STATUS_ID' },
          { name: 'ID_TYPE_ID' },
          { name: 'AGE_CATEGORY_ID' },
          { name: 'BLOOD_TYPE_ID' },
          { name: 'RELIGION_ID' },
          { name: 'SECTOR_ID' },
          { name: 'GOVERNERATE_ID' },
          { name: 'JOB_TYPE_ID' },
          { name: 'DEPARTMENT_ID' },
          { name: 'DEPARTMENT_NAME' },
          { name: 'RANK_ID' },
          { name: 'RANK' },
          { name: 'PERSON_CODE' },
          { name: 'GRADUATION_NAME' },
          { name: 'CATEGORY_ID' },
          { name: 'SHOE_SIZE' },
          { name: 'SUIT_SIZE' },
          { name: 'OVERALL_SIZE' },
          { name: 'MASK_SIZE' },
          { name: 'RANK_RENEW_DATE' },
          { name: 'TRANSFER_NO' },
          { name: 'FIELD_SERVICE_ABILITY' },
          { name: 'FEED_REPLACE' },
          { name: 'NOZOM_DATE' },
          { name: 'P_ACCEDENT' },
          { name: 'P_RANKING' },
          { name: 'P_COURSES' },
          { name: 'P_TRAVEL' },
          { name: 'P_LANGUAGE' },
          { name: 'P_PUNISHMENTS' },
          { name: 'P_BATTELES' },
          { name: 'P_JOBS' },
          { name: 'T_JOBS' },
          { name: 'P_HOSPITALS' },
          { name: 'P_INVESTGATIONS' },
          { name: 'PERSON_MEDALS' },
          { name: 'PERSON_STUDIES' },
          { name: 'PERSONS_RANKING' },
          { name: 'PERSON_COMPLAINS' },
          { name: 'PERSON_EXCELANT' },
          { name: 'PERSON_V_G' },
          { name: 'PERSON_G' },
          { name: 'PERSON_ACC' },
          { name: 'PERSON_WEAK' },
          { name: 'lgh' },
          { name: 'wh' },
          { name: 'SPECIALIZATION_ID' },
          { name: 'SPECIALIZATIONS_NAME' },
          { name: 'BLOOD_TYPE_NAME' },
          { name: 'RELIGION_NAME' }
        ],

        async: false,

        url: 'OFFICERS/bind_data_BorrowIn'

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_OFFICERS_gridDiv").jqxGrid({ source: dataAdapter });

}
// buildand bind officers grid borrowOut
function OFFICERsGridBind_BorrowOut() {


    $('#st_OFFICERS_gridDiv').jqxGrid('clearselection');
    $('#st_OFFICERS_gridDiv').jqxGrid('clear');
    var source = {
        datatype: "json",
        datafields: [
        { name: 'OUT_UN_FORCE' },
          { name: 'JOB_NAME' },
          { name: 'HIRE_DATE' },
          { name: 'CURRENT_RANK_DATE' },
          { name: 'NEXT_RANK_DATE' },
          { name: 'LEAVE_DATE' },
          { name: 'JOIN_DATE' },
          { name: 'SEX' },
          { name: 'MARRIGE_CONT' },
          { name: 'COMMUNITY_NO' },
          { name: 'ARKAN_HARB' },
          { name: 'DAUGHTERS_COUNT' },
          { name: 'SONS_COUNT' },
          { name: 'BIRTH_PLACE' },
          { name: 'SORT_NO' },
          { name: 'ID_NO' },
          { name: 'PHONE_2' },
          { name: 'PHONE1' },
          { name: 'BIRTHDATE' },
          { name: 'ADDERESS' },
          { name: 'PERSON_NAME' },
          { name: 'PERSONAL_ID_NO' },
          { name: "BORROW_STATUS" },
          { name: "BORROW_FIRM_CODE" },
          { name: 'PERSON_CAT_ID' },
          { name: 'RANK_CAT_ID' },
          { name: 'SOCIAL_STATE_ID' },
          { name: 'FIRM_CODE' },
          { name: 'FIRM_NAME' },
          { name: 'PARENT_STATUS_ID' },
          { name: 'ID_TYPE_ID' },
          { name: 'AGE_CATEGORY_ID' },
          { name: 'BLOOD_TYPE_ID' },
          { name: 'RELIGION_ID' },
          { name: 'SECTOR_ID' },
          { name: 'GOVERNERATE_ID' },
          { name: 'JOB_TYPE_ID' },
          { name: 'DEPARTMENT_ID' },
          { name: 'DEPARTMENT_NAME' },
          { name: 'RANK_ID' },
          { name: 'RANK' },
          { name: 'PERSON_CODE' },
          { name: 'GRADUATION_NAME' },
          { name: 'CATEGORY_ID' },
          { name: 'SHOE_SIZE' },
          { name: 'SUIT_SIZE' },
          { name: 'OVERALL_SIZE' },
          { name: 'MASK_SIZE' },
          { name: 'RANK_RENEW_DATE' },
          { name: 'TRANSFER_NO' },
          { name: 'FIELD_SERVICE_ABILITY' },
          { name: 'FEED_REPLACE' },
          { name: 'NOZOM_DATE' },
          { name: 'P_ACCEDENT' },
          { name: 'P_RANKING' },
          { name: 'P_COURSES' },
          { name: 'P_TRAVEL' },
          { name: 'P_LANGUAGE' },
          { name: 'P_PUNISHMENTS' },
          { name: 'P_BATTELES' },
          { name: 'P_JOBS' },
          { name: 'T_JOBS' },
          { name: 'P_HOSPITALS' },
          { name: 'P_INVESTGATIONS' },
          { name: 'PERSON_MEDALS' },
          { name: 'PERSON_STUDIES' },
          { name: 'PERSONS_RANKING' },
          { name: 'PERSON_COMPLAINS' },
          { name: 'PERSON_EXCELANT' },
          { name: 'PERSON_V_G' },
          { name: 'PERSON_G' },
          { name: 'PERSON_ACC' },
          { name: 'PERSON_WEAK' },
          { name: 'lgh' },
          { name: 'wh' },
          { name: 'SPECIALIZATION_ID' },
          { name: 'SPECIALIZATIONS_NAME' },
          { name: 'BLOOD_TYPE_NAME' },
          { name: 'RELIGION_NAME' }
        ],

        async: false,

        url: 'OFFICERS/bind_data_BorrowOut'

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_OFFICERS_gridDiv").jqxGrid({ source: dataAdapter });

}



function seet_fin_year() {


    //getQS('nn', window.location.href);

    $.ajax({
        url: "OFFICERS/GET_fin_year",
        // data: { ID: pid, FIRM: frm },


        dataType: "json",

        success: function (reslult) {

            //var FormList = JSON.parse(reslult);
            var FormList = reslult;
            //pers_cod = FormList[0].employeeid;
            if (FormList != "") {

                var dt = FormList;

                $("#TXT_FIN_YEAR").val(FormList[0].FIN_YEAR);
                $("#TXT_PERIOD").val(FormList[0].TRAINING_PERIOD);
                period_id = FormList[0].TRAINING_PERIOD_ID;
                fin_year = FormList[0].FIN_YEAR;
                //bnd_mission_grid(firm_cod, FormList[0].FIN_YEAR, period_id, $('#DT_DATE').val(), pers_cod);
                //document.getElementById('dt_firm_missions&add').style.display = 'inline-block';
                // $("#user_name").show();
                //  $("#user_name").val(pid);







                //var data = dt[7].split(',');
                //for (var i = 0; i < data.length; i++) {

                //    if (data[i] == 'pln') {
                //        //  $("#TXT_PLANNING_DECISION").jqxDropDownList({ disabled: false });
                //    }

                //    else if (data[i] == 'cmd') {

                //        //$("#TXT_COMANDER_DECESION").jqxDropDownList({ disabled: false });
                //    }
                //}
            }

        },
        error: function (response) {

        }
    });
}
function set_ddl(id1, frm) {

    var pid = id1;
    //getQS('nn', window.location.href);

    $.ajax({
        url: "groups/GET_JOP",
        data: { ID: pid, FIRM: frm },


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
