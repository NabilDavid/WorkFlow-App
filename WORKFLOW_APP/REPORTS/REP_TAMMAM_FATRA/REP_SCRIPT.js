$(document).ready(function () {
    $('#from').jqxDateTimeInput({ animationType: 'fade', width: '80%', height: 30, rtl: true, dropDownHorizontalAlignment: 'right', theme: "darkblue" });
    $('#to').jqxDateTimeInput({ animationType: 'fade', width: '80%', height: 30, rtl: true, dropDownHorizontalAlignment: 'right', theme: "darkblue" });
    //$('#rep_click').jqxInput({ width: '95%', height: 30, rtl: true, theme: 'darkblue' });
  //  BuildDropDwon_static('TRANS_REASON', 'saf_trans_reason_id', 'saf_trans_reason_name', 'F_SARF_PAG.aspx/trans_reason_ddl');



    $('#rep_click').on('click', function (event) {

        confirm_PARM();
    });
});

function BuildDropDwon_static(id, value, name, url) {


    var mydata
    $.ajax({

        url: url,
        data: JSON.stringify({
        }),
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        record: 'Table',
        dataType: "json",
        type: "POST",
        success: function (data) {
            mydata = data.d;
        },
        error: function (response) {
            alert("grid")
        }
    });
    var source =
          {
              localdata: mydata,
              datatype: "json",
              datafields:
                 [
                  { name: value },
                     { name: name }
                 ],
          };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $('#' + id).jqxDropDownList({ source: dataAdapter });

    $('#' + id).jqxDropDownList({
        width: '95%',
        height: '30px',
        source: dataAdapter,
        source: dataAdapter,
        //  autoComplete: true,
        // searchMode: "containsignorecase",
        theme: "darkblue",
        dropDownHeight: 100,
        rtl: true,
        selectedIndex: 0,
        placeHolder: "    ",
        dropDownHorizontalAlignment: 'right',
        displayMember: name,
        valueMember: value


    });

    //$('#PARENT_FORM_DopDwon').jqxDropDownList('insertAt', { label: ' ---بدون الصفحة الام------', value: "" }, "");

}

function confirm_PARM() {
    
    Date_From = ($('#from').val());
    Date_TO = ($('#to').val());
    drop_vl = $('#TRANS_REASON').val();
    $.ajax({
        url: "REPORT_PAGE.aspx/SET_variables",
        data: JSON.stringify({ Date_From: Date_From, Date_TO: Date_TO }),
        cache: false,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",

        success: function (data) {
            $('#view_report').click();
        },
        error: function (response) {

        }
    });

}