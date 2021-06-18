var pers_cod;
var firm_cod;
//let socket = new WebSocket("ws://localhost:9001/methodName");

//socket.onopen = function(e) {
//    socket.send("");
//};

//socket.onmessage = function(event) {
//    alert(`${event.data}`);

//};



//socket.onerror = function(error) {
//    alert(`[error] ${error.message}`);
//};
//get id from url
function getQS(fld, url) {
    var h = url ? url : window.location.href;
    var reg = new RegExp('[?&]' + fld + '([^&#]*)', 'i');
    var str = reg.exec(h);
    return str ? str[1].substr(1, str[1].length) : null;
}
//get id from web services
function get_person_data() {

    var x = 1;
    // var pers_cod;
    //pers_cod = getQS('nn', window.location.href);

    bind_person_id();
    get_firm(pers_cod);
    set_ddl(pers_cod, firm_cod);

    if (pers_cod == "") {

        $.ajax({
            url: "http://192.223.2.200:81/command_ad/php/ldap.php",
            data: { out: 1 },
            cache: false,
            async: false,
            //contentType: "application/json; charset=utf-8",

            type: "POST",
            success: function (reslult) {

                var FormList = JSON.parse(reslult);
                pers_cod = FormList[0].employeeid;
                get_firm(pers_cod);
                //  firm_cod = FormList[0].HIA_UNIT_NO;
                // firm_cod = '1402102001'//'1400000000'//'1402102001'
                set_ddl(pers_cod, 1, 1, firm_cod);
                $('#firm_cod').val(firm_cod)
                //debugger;
                // alert(FormList[0].employeeid);
            },
            error: function (response) {


                //var xx = response.responseText;
                alert("ادخل رقم المستخدم");
            }
        });
    }
    else {



    }
    //debugger

}



function get_notif() {

    tasks_arr2_2 = [];
    tasks_arr2_3 = [];
    tasks_arr2_1 = [];
    $.ajax({
        url: 'HOME_app/get_notif_fun',
        //cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        //data: JSON.stringify({
        //    GOVERNORATE_ID: $("#GOVERNORATE_ID").val(), ARMY_ID: $("#ARMY_ID").val(), UNIT_ID: $("#DEP_ID").val()
        //     , SUBJECT_ID: $("#SUBJECT_ID").val(), COMP_ID: $("#COMP_ID").val(), SPONSER_TYPE: $("#SPONSER_TYPE").val(), STATUS_ID: $('#STATUS_ID').val()
        //}),
        success: function (datarecord) {

            $('#main_notif').empty();
            $('#main_notif').append(datarecord.s);


        },
        error: function (response) {

        }
    });
}

function get_firm(pers_cod) {


    $.ajax({
        url: 'HOME_app/get_firm_fun',
        //cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        data: JSON.stringify({
            per: pers_cod
            //    GOVERNORATE_ID: $("#GOVERNORATE_ID").val(), ARMY_ID: $("#ARMY_ID").val(), UNIT_ID: $("#DEP_ID").val()
            //     , SUBJECT_ID: $("#SUBJECT_ID").val(), COMP_ID: $("#COMP_ID").val(), SPONSER_TYPE: $("#SPONSER_TYPE").val(), STATUS_ID: $('#STATUS_ID').val()
        }),
        success: function (datarecord) {
            firm_cod = datarecord.firm;



        },
        error: function (response) {

        }
    });
}



function bind_person_id() {

    $.ajax({
        url: "HOME_app/person_id",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (reslult) {

            pers_cod = reslult.pid;
            return pers_cod;
        },
        error: function (response) {
        }
    }
    )
}
//////get_data
