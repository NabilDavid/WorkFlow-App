var ControllerName = ApplicationName + '/Relivants/';
var personcode;
$(document).ready(function () {
   

    var url_string = window.location.href;
    var url = new URL(url_string);
    personcode = url.searchParams.get("Person_Code");
   
    build_Relivants_grid();

    //ajax to get officer name
    $.ajax({

        url: 'Relivants/getOffName',
        data: JSON.stringify({
            personcode: personcode
        }),
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (data) {
            alert(data.d);
     
        },
        error: function (response) {
            alert("error");
        }
    });



});
//****************************************************OUT_READY**************************************************
// buildand bind Relivants grid
function build_Relivants_grid() {


    gridId = 'RelivantsGrid';

    theme = "darkblue";
    headerText = " أقارب الضابط";
    gridAddUrl = ControllerName + 'Create';
    $("#RelivantsGrid").jqxGrid({
        width: '99%',
        height: 450,
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

                
        ]


    });
    $("#RelivantsGrid").jqxGrid({ enabletooltips: true });
}
function bind_OFFICERS_grid() {

    $('#RelivantsGrid').jqxGrid('clearselection');
    $('#RelivantsGrid').jqxGrid('clear');
    var source = {
        datatype: "json",
        datafields: [
        { name: 'OUT_UN_FORCE' },
          { name: 'JOB_NAME' },
          { name: 'HIRE_DATE' },
          { name: 'CURRENT_RANK_DATE' },

         
           ],

        async: false,

        url: 'Relivants/bind_data'

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#RelivantsGrid").jqxGrid({ source: dataAdapter });
}