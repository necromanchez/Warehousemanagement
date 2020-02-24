$(document).ready(function () {
    initialized();
    BindDropdownList('StockClassCode', '/Helper/Dropdown_ClassStock');
    BindDropdownList('ItemCode', '/Helper/Dropdown_Items');

    $(".actualpickssss").keyup(function () {
        alert("asd");
    });

    $('#OutboundItemModal').on('hidden.bs.modal', function (e) {
        initialized();
    })

    $('#btnsavecon').on('click', function () {
        //$.ajax({
        //    url: '/Outbound/Picked/SetPicked/',
        //    type: 'POST',
        //    data: { PickID: PickID, ActualpickQty: $('#InboundPlanQty').val(), OutboundPlanNo: selectedOutBoundPlanCode },
        //    cache: false,
        //    datatype: "json",
        //    success: function (returnData) {
        //        msg("Actual Picked Saved", "success");
        //        initialized_outboundItem();
        //    }
        //});

        $.ajax({
            url: '/Outbound/Picked/SetPicked/',
            type: 'POST',
            data: { PickID: selectedpickID, ActualpickQty: $('#InboundPlanQty').val(), OutboundPlanNo: selectedOutBoundPlanCode },
            cache: false,
            datatype: "json",
            success: function (returnData) {
                msg("Actual Picked Saved", "success");
                initialized_outboundItem();
            }
        });

    })
});
var s;
var selectedpickID;
var selectedOutBoundPlanCode;
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
        $('#OutboundItemModal').find('.modal-title').text("Picked Items");
        
        $("#btnsavecon").html('Save');
        $("#OutboundItemModal").modal("show");
        //disabled to picked
        $("#ItemCode").prop('disabled', true);
        $("#StockClassCode").prop('disabled', true);
        $("#PoNo").prop('disabled', true);
        $("#LotNo").prop('disabled', true);
        $("#ExpirationDate").prop('disabled', true);
        
        initialized_outboundItem();
    });

    $('#PickedTable tbody').on('dblclick', 'tr', function () {
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
    
    $('#OutboundItemTable').DataTable({
        ajax: {
            url: '../Picked/GetOutboundPicked_ItemList?OutboundPlanCode=' + $('#OutboundPlanCode').val(),
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
            { title: "OutBoundPlanCode", data: "OutBoundPlanCode", visible: false },
            { title: "ItemCode", data: "ItemCode", visible: false },
            { title: "PickID", data: "PickID", visible: false },
            { title: "Item", data: "ItemName" },
            { title: "CodeName", data: "CodeName" },
            { title: "Po No", data: "PoNo" },
            { title: "Lot No", data: "LotNo" },
            { title: "Case", data: "QtyPerCase" },
            { title: "Inner Case", data: "QtyPerInnerCase" },
            { title: "Per Unit", data: "QtyPerUnit" },
            { title: "Plan Qty", data: "SUMQty" },
            //{
            //    title: "Actual Qty", data: function (x) {

            //        return data = "<input type='number' class='form-control clear actualpickssss' id='ActualQty"+x.PickID+"' value='"+x.ActualQty+"'>"
            //    }
            //},
            { title: "Picked Qty", data: "ActualQty" },
            {
                        title: "Action",
                        render: function (data, type, full, meta) {
                            data = '<input type="hidden" id="withChange_' + meta.row + '" value="0" class="withChange_' + meta.row + '">'
                                + '<button type="button" class="btn btn-success btn-sm btn-flat btnedit_' + meta.row + ' btnedit" data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Pick">'
                                + '<i class="fa fa-save"></i>'
                            return data;
                        }, searchable: false, orderable: false
            },
           
        ],

    }).columns.adjust();

    $('#OutboundItemTable tbody').one('click', '.btnedit', function () {
        var count = $(this).data('count');
        var tabledata = $('#OutboundItemTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        selectedpickID = data.PickID;
        selectedOutBoundPlanCode = data.OutBoundPlanCode;
        $.ajax({
            url: '../OutboundItem/GetOutboundItem?ID=' + data.ID,
            type: 'POST',
            cache: false,
            datatype: "json",
            success: function (returnData) {
                Clearall();
                $('#ItemCode').val(data.ItemCode);
                $('#theQty').text("Picked Qty");
                $('#StockClassCode').val(returnData.item.StockClassCode);
                $('#ExpirationDate').val(moment(returnData.item.ExpirationDate).format("MM/DD/YYYY"));
                $('#LotNo').val(returnData.item.LotNo);
                $('#PoNo').val(returnData.item.PoNo);

                $('#pqc_q').val('');
                $('#qpic_q').val('');
                if(returnData.item.length == 0)
                {
                    $('#pqc_q').attr('readonly', true);
                    $('#qpic_q').attr('readonly', true);
                }
                else {
                    $('#pqc_q').attr('readonly', true);
                    $('#qpic_q').attr('readonly', true);
                    if (returnData.item.QtyPerCase != null) {
                        $('#pqc_q').attr('readonly', false);
                    }
                    if (returnData.item.QtyPerInnerCase != null) {
                        $('#qpic_q').attr('readonly', false);
                    }

                }
                
            }
        });
        //alert($('#ActualQty' + data.PickID).val());
        //$.ajax({
        //    url: '../Picked/SetPicked/',
        //    type: 'POST',
        //    data: { PickID: data.PickID, ActualpickQty: $('#ActualQty' + data.PickID).val(), OutboundPlanNo: data.OutBoundPlanCode },
        //    cache: false,
        //    datatype: "json",
        //    success: function (returnData) {
        //        msg("Actual Picked Saved", "success");
        //        initialized_outboundItem();
        //    }
        //});
    });
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
        url: '../Picked/PickedOutboundPlan/',
        type: 'POST',
        data:{OutboundPlanNoList:Outplanno},
        cache: false,
        datatype: "json",
        success: function (returnData) {
            initialized_outboundItem();
            initialized();
        }
    });
}