var ApplicationName = "/.."
var gridAddUrl = '';
var gridId = 'G_STEP_GRD';
var ControllerName = ApplicationName + '/G_STEPS/';
var data_row;


$(document).ready(function () {

    //  var ControllerName = ApplicationName + '/groups/';

    bld_steps_grd();
    //bnd_step_grd(2);
    BLD_DD("group_id", "../G_STEPS/GET_GRP", 200, 30, "darkblue", 'OFF_ABS_GROUP_ID', 'OFF_ABS_GROUP_NAME', -1);
    //   
    $("#group_id").on('select', function (event) {
        var x = event.args.item.value;
        bnd_step_grd(x);
    });
    $('#G_STEP_GRD').on('rowselect', function (event) {
        var args = event.args;
        var row = args.rowindex;
        var data = $('#G_STEP_GRD').jqxGrid('getrowdata', row);
        data_row = $('#G_STEP_GRD').jqxGrid('getrowid', row);
        // AG_VILLAGES_Refresh('AG_VILLAGES');


    });
});

function bld_steps_grd() {
    theme = "darkblue";
    headerText = " خطوات المجموعات";
    gridAddUrl = ControllerName + 'Create';
    $("#G_STEP_GRD").jqxGrid({
        width: '75%',
        autoheight: true,
        pageable: true,
        theme: "darkblue",
        sortable: true,
        rtl: true,
        showaggregates: true,
        showstatusbar: true,
        statusbarheight: 50,
        filterable: true,
        showfilterrow: true,
        showtoolbar: true,
        selectionmode: 'singlerow',
        //source: dataAdapter,
        rendertoolbar: toolbarfn,
        columns: [
            { text: 'كود الالمرحلة', dataField: 'OFF_ABS_STEPS_ID', width: '10%', cellsalign: 'center', align: 'center', hidden: true },
            { text: 'كود المجموعه', dataField: 'OFF_ABS_GROUP_ID', width: '10%', cellsalign: 'center', align: 'center', hidden: true },
            { text: 'اسم المجموعه', dataField: 'OFF_ABS_GROUP_NAME', width: '25%', cellsalign: 'center', align: 'center' },
            { text: 'اسم المرحلة', dataField: 'OFF_ABS_STEPS_NAME', width: '25%', cellsalign: 'center', align: 'center' },
            { text: ' الوظيفة', dataField: 'ARH_ROLE_NAME', width: '20%', cellsalign: 'center', align: 'center' },
            { text: ' ترتيب المرحلة ', dataField: 'ORDER_ID', width: '20%', cellsalign: 'center', align: 'center' },
            {
                text: '', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                    return "<img  style='margin-left: 5px;cursor:pointer' height='20' width='20' src='../images/edit.png' onclick='open_edit(" + row + ", G_STEP_GRD)'/>";
                }
            },
            {
                text: '', columntype: 'button', cellsalign: 'center', filterable: false, width: '5%', align: 'center', cellsrenderer: function (row, datafield, value) {
                    return "<img   style='margin-left: 5px;cursor:pointer' height='17' width='17' src='../images/delete.png' onclick='open_confirm(" + row + ", G_STEP_GRD)'/>";
                }
            }

        ]

    });
    $("#G_STEP_GRD").jqxGrid({ enabletooltips: true });
}
function bnd_step_grd(GR) {

    var source = {
        datatype: "json",
        datafields: [
        { name: 'OFF_ABS_GROUP_ID' },
            { name: 'OFF_ABS_STEPS_ID' },
             { name: 'OFF_ABS_STEPS_NAME' },
              { name: 'OFF_ABS_GROUP_NAME' },
               { name: 'ARH_ROLE_NAME' },
            { name: 'ORDER_ID' }

        ],
        async: false,

        url: 'G_STEPS/GET_STEPS',
        data: { GR: GR }

    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#G_STEP_GRD").jqxGrid({ source: dataAdapter });
}

