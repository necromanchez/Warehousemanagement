$(document).ready(function () {
    initialized();

    $('#OutboundItemModal_View').on('hidden.bs.modal', function (e) {
        initialized();
        initialized();
    })
  
});
var s;
function initialized() {

    s = $('#PickedTable').DataTable({
        ajax: {
            url: '../Picked/GetPickedList',
            type: "POST",
            datatype: "json"
        },
        autoWidth: false,
        serverSide: "true",
        order: [0, "asc"],
        processing: "true",
        language: {
            "processing": "processing... please wait"
        },
        dom: 'Bfrtip',
        buttons: [
            {
                text: '  PICKED  ',
                className: 'btn btn-success btn-flat fa fa-plus',
                action: function (e, dt, node, config) {
                    SetPicked();
                }
            },
        ],
        destroy: true,
        columns: [
            { title: "ID", data: "ID", visible: false },
            { title: "OutboundPlanNo", data: "OutboundPlanNo" },
            { title: "Owner", data: "CodeOwner" },
            { title: "Owner2", data: "CodeOwner2", visible: false },
            {
                title: "PlanDate", data: function (x) {
                    return moment(x.OutboundPlanDate).format("MM/DD/YYYY")
                }
            },
            {
                title: "Delivery Date", data: function (x) {
                    return moment(x.DeliveryPlanDate).format("MM/DD/YYYY")
                }
            },
            {
                title: "Shipout Date", data: function (x) {
                    return moment(x.ShipoutDate).format("MM/DD/YYYY")
                }
            },
            { title: "ShipTo", data: "ShipToCode" },
            { title: "ShipToCode2", data: "ShipToCode2", visible: false },
            { title: "Slip No", data: "SlipNo" },
            { title: "SlipClass", data: "SlipClass", visible: false },
            {
                title: "Slip Date", data: function (x) {
                    return moment(x.SlipDate).format("MM/DD/YYYY")
                }
            },
            { title: "Outbound Class", data: "OutboundClassCode" },
            { title: "OutboundClassCode2", data: "OutboundClassCode2", visible: false },
            { title: "Status", data: "OutboundStatus" },
            { title: "UrgentFlagCode", data: "UrgentFlagCode", visible: false },
            { title: "TransportClassCode", data: "TransportClassCode", visible: false },
            { title: "DEClass", data: "DEClass", visible: false },
            { title: "ShipperCode", data: "ShipperCode", visible: false },
            { title: "BuyerCode", data: "BuyerCode", visible: false },
            { title: "ConsigneeCode", data: "ConsigneeCode", visible: false },
            { title: "SlipRemarks", data: "SlipRemarks", visible: false },
            {
                 title: "Action",
                 render: function (data, type, full, meta) {
                     data = '<input type="hidden" id="withChange_' + meta.row + '" value="0" class="withChange_' + meta.row + '">'
                   
                         + '</button><button type="button" class="btn btn-sm btn btn-primary btn-flat editinput btnitem_' + meta.row + ' btnitem " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="View Items">'
                         + '<i class="fa fa-dropbox"></i></button>'
                         + '</button><button type="button" class="btn btn-sm btn btn-warning btn-flat editinput btncancel_' + meta.row + ' btncancel " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Cancel">'
                         + '<i class="fa fa-mail-reply"></i></button>'


                     return data;
                 }, searchable: false, orderable: false
             },
        ],

    }).columns.adjust();

    $('#PickedTable tbody').one('click', '.btnitem', function () {
        var count = $(this).data('count');
        var tabledata = $('#PickedTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        $('#OutboundPlanCode').val(data.OutboundPlanNo);
        $("#OutboundItemModal_View").modal("show");
        initialized_outboundItem();
    });

    $('#PickedTable tbody').on('click', 'tr', function () {
        var tabledata = $('#PickedTable').DataTable();
        if ($(this).hasClass("row_selected")) {
            $(this).removeClass('row_selected');
        }
        else {
            $(this).addClass('row_selected');
        }
    });
 

    
}

function initialized_outboundItem() {
    $('#OutboundItemTable_view').DataTable({
        ajax: {
            url: '../OutboundItem/GetOutboundItemList?OutboundPlanCode=' + $('#OutboundPlanCode').val(),
            type: "POST",
            datatype: "json"
        },
        autoWidth: false,
        serverSide: "true",
        order: [0, "asc"],
        processing: "true",
        language: {
            "processing": "processing... please wait"
        },
        destroy: true,
        columns: [
            { title: "ID", data: "ID", visible: false },
            { title: "ItemCode", data: "ItemCode", visible: false },
            { title: "Item", data: "ItemName" },
            { title: "CodeName", data: "CodeName" },
            { title: "Po No", data: "PoNo" },
            { title: "Lot No", data: "LotNo" },
            { title: "Case", data: "QtyPerCase" },
            { title: "Inner Case", data: "QtyPerInnerCase" },
            { title: "Per Unit", data: "QtyPerUnit" },
            { title: "Plan Qty", data: "SUMQty" },
            {
                title: "Actual Qty", data: function () {

                    return data = "<input type='number' class='form-control clear' id='ActualQty'>"
                }},
           
        ],

    }).columns.adjust();

}

function SetPicked() {
   
    var rows = $('.row_selected');
    var table = s;
    var Outplanno = [];
    var rowData = table.rows(rows).data();
    rowData.each(function (x) {
        //alert(x.OutboundPlanNo);
        Outplanno.push(x.OutboundPlanNo);
    });
    $.ajax({
        url: '../Picked/SetPicked/OutboundPlanNoList',
        type: 'POST',
        data:{OutboundPlanNoList:Outplanno},
        cache: false,
        datatype: "json",
        success: function (returnData) {

        }
    });
}