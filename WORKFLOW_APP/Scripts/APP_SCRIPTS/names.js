
var PERSONAL_ID_NO ="";
$(document).ready(function () {
  
    build_officerNames_grid();
    get_person_data();
    bind_officerNames_grid();

    $("#st_officerNames_gridDiv").on("rowselect", function (event) {
        
        PERSONAL_ID_NO = event.args.row.PERSONAL_ID_NO;
       window.open('../WORKFLOW_APP/TALAB_M2M_OK?nn=' + PERSONAL_ID_NO, '_self');
    
    });

}); 



function build_officerNames_grid() {

    gridId = 'st_officerNames_gridDiv';

    theme = "darkblue";
    headerText = " اسماء الضباط";

    $("#st_officerNames_gridDiv").jqxGrid({
        width: '60%',
        height: '300px',
        theme: "darkblue",
        sortable: true,
        rtl: true,
        
        showaggregates: true,
        filterable: true,
        showfilterrow: true,
        showtoolbar: true,
        columns: [
            { text: ' الرتبــة', dataField: 'RANK', width: '50%', cellsalign: 'center', align: 'center' },
            { text: ' الإســـــــــم', dataField: 'PERSON_NAME', width: '50%', cellsalign: 'center', align: 'center' },
         { text: ' الرقم القومي', dataField: 'PERSONAL_ID_NO', width: '100%', cellsalign: 'center', align: 'center' , hidden: 'true' }

          

        ]

    });
    $("#st_officerNames_gridDiv").jqxGrid({ enabletooltips: true });
}

function bind_officerNames_grid() {
   
    $('#st_officerNames_gridDiv').jqxGrid('clearselection');
    var source = {
        datatype: "json",
        datafields: [
         { name: 'PERSON_NAME' },
         { name: 'PERSONAL_ID_NO' },
           { name: 'RANK' }


         
        ],
        async: false,
        //  url: 'Users/getUsers',

        url: 'Names/getOfficers',
        data: { FIRM: firm_cod }
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#st_officerNames_gridDiv").jqxGrid({ source: dataAdapter });


}
function get_person_data() {
    debugger
    var x = 1;
  
    pers_cod = getQS('nn', window.location.href);

    if (pers_cod == null) {
        $.ajax({
            url: "http://192.223.1.148/command_edara/php/ldap.php",
            data: { out: 1 },
            cache: false,
            async: false,
          
            type: "POST",
            success: function (reslult) {

                var FormList = JSON.parse(reslult);
                pers_cod = FormList[0].employeeid;
                firm_cod = FormList[0].HIA_UNIT_NO;
           
                set_ddl(pers_cod, 1, 1, firm_cod);
                $('#firm_cod').val(firm_cod)
            
            },
            error: function (response) {


               
                alert("ادخل رقم المستخدم");
            }
        });
    }
    else {
        pers_cod = getQS('nn', window.location.href);
        set_ddl(pers_cod, '1402102001');
        firm_cod = '1402102001';
    }
   

}