$(document).ready(function () {
    initialized();
    demo.initDateTimePicker();
    BindDropdownList('CodeOwner', '/Helper/Dropdown_ClassOwner');
    BindDropdownList('SiteCode', '/Helper/Dropdown_Site');
    BindDropdownList('OutboundClassCode', '/Helper/Dropdown_OutboundClass');
    BindDropdownList('TransportClassCode', '/Helper/Dropdown_OutboundClassTransport');
    BindDropdownList('ShipToCode', '/Helper/Dropdown_ShipToCode');
    BindDropdownList('ShipperCode', '/Helper/Dropdown_Shipper');
    BindDropdownList('BuyerCode', '/Helper/Dropdown_Buyer');
    BindDropdownList('ConsigneeCode', '/Helper/Dropdown_ConsigneeCode');

    
    
    
    $('#frm_outbound').on('submit', function (e) {
        e.preventDefault();
        if ($('#ID').val() == "") {
            AddoutboundPlan($(this));
        }
        else {
            EditoutboundPlan($(this));
        }
        
    });

    $('#AddInboundPlan').on('hidden.bs.modal', function (e) {
        initialized();
    })
});

function initialized() {

    $('#OutboundPlanTable').DataTable({
        ajax: {
            url: '../OutboundPlan/GetOutboundPlanList',
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
                text: '  Add  ',
                className: 'btn btn-success btn-flat nc-icon nc-bank',
                action: function (e, dt, node, config) {
                    $('#SiteCode').val($('#globalSite').val());
                    $('#AddOutboundPlan').find('.modal-title').text("Add Outbound Plan");
                    $('#AddOutboundPlan').modal('show')
                  
                }
            },
        ],
        destroy: true,
        columns: [
            { title: "ID", data: "ID", visible: false },
            { title: "OutboundPlanNo", data: "OutboundPlanNo" },
            { title: "Owner", data: "CodeOwner" },
            { title: "Owner2", data: "CodeOwner2", visible:false},
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
            { title: "SlipClass", data: "SlipClass",visible:false },
            {
                title: "Slip Date", data: function (x) {
                    return moment(x.SlipDate).format("MM/DD/YYYY")
                }
            },
            { title: "Outbound Class", data: "OutboundClassCode" },
            { title: "OutboundClassCode2", data: "OutboundClassCode2", visible: false },
            { title: "Status", data: "OutboundStatus" },
            { title: "UrgentFlagCode", data: "UrgentFlagCode", visible:false },
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
                        + '<button type="button" class="btn btn-warning btn-sm btn-flat btnedit_' + meta.row + ' btnedit" data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Modify">'
                        + '<i class="fa fa-edit"></i>'
                        + '</button><button type="button" class="btn btn-sm btn-danger btn-flat editinput btndel_' + meta.row + ' btndel " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Delete">'
                        + '<i class="fa fa-trash-o"></i></button>'
                         + '</br>'

                        + '</button><button type="button" class="btn btn-sm btn btn-primary btn-flat editinput btnitem_' + meta.row + ' btnitem " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Add Items">'
                        + '<i class="fa fa-dropbox"></i></button>'
                        + '</button><button type="button" class="btn btn-sm btn btn-success btn-flat setallocation btnAllocate_' + meta.row + ' btnAllocate " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Allocate">'
                        + '<i class="fa fa-mail-forward"></i></button>'
                       
                   
                    return data;
                }, searchable: false, orderable: false
            },
        ],

    }).columns.adjust();

    $('#OutboundPlanTable tbody').one('click', '.btnedit', function () {
        var count = $(this).data('count');
        var tabledata = $('#OutboundPlanTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        $('#CodeOwner').val(data.CodeOwner2);
        $('#SiteCode').val($('#globalSite').val());
        $('#OutboundPlanDate').val(moment(data.OutboundPlanDate).format("MM/DD/YYYY"));
        $('#UrgentFlagCode').val(data.UrgentFlagCode);
        $('#SlipDate').val(moment(data.SlipDate).format("MM/DD/YYYY"));
        $('#SlipClass').val(data.SlipClass);
        $('#SlipNo').val(data.SlipNo);
        $('#OutboundClassCode').val(data.OutboundClassCode2);
        $('#TransportClassCode').val(data.TransportClassCode);
        $('#DEClass').val(data.DEClass);
        $('#ShipToCode').val(data.ShipToCode2);
        $('#ShipperCode').val(data.ShipperCode);
        $('#BuyerCode').val(data.BuyerCode);
        $('#ConsigneeCode').val(data.ConsigneeCode);
        $('#DeliveryPlanDate').val(moment(data.DeliveryPlanDate).format("MM/DD/YYYY"));
        $('#ShipoutDate').val(moment(data.ShipoutDate).format("MM/DD/YYYY"));
        $('#SlipRemarks').val(data.SlipRemarks);
        $('#ID').val(data.ID);
        $('#AddOutboundPlan').find('.modal-title').text("Edit Outbound");

        $('#AddOutboundPlan').modal('show')
    });

    $('#OutboundPlanTable tbody').one('click', '.btndel', function () {
        var count = $(this).data('count');
        var tabledata = $('#OutboundPlanTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        DeleteOutbound(data);
    });

    $('#OutboundPlanTable tbody').one('click', '.btnitem', function () {
        var count = $(this).data('count');
        var tabledata = $('#OutboundPlanTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        $('#OutboundPlanCode').val(data.OutboundPlanNo);
        $("#OutboundItemModal").modal("show");
        initialized_outboundItem();
    });

    $('#OutboundPlanTable tbody').one('click', '.btnAllocate', function () {
        var count = $(this).data('count');
        var tabledata = $('#OutboundPlanTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();
        $.ajax({
            url: '../OutboundPlan/Allocate_Outbound?OutboundPlanNo=' + data.OutboundPlanNo,
            type: 'POST',
            cache: false,
            datatype: "json",
            success: function (returnData) {
                initialized();
            }
        });
    });
}


function AddoutboundPlan(data) {
    var datanow = data.serialize();
    datanow += '&SiteCode=' + $('#globalSite').val();
    $.ajax({
        url: '../OutboundPlan/CreateOutboundPlan',
        data: datanow,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddOutboundPlan').modal('hide');
                msg("Outbound Plan was successfully saved.", "success");
            }
            else {
                msg("Outbound Plan already registered", "warning");
            }

        }
    });
}

function EditoutboundPlan(data) {
    var datanow = data.serialize();
    datanow += '&SiteCode=' + $('#globalSite').val();
    $.ajax({
        url: '../OutboundPlan/EditOutboundPlan',
        data: datanow,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddOutboundPlan').modal('hide');
                msg("Outbound Plan was successfully saved.", "success");
            }
            else {
                msg("Outbound Plan already registered", "warning");
            }
        }
    });
}

function DeleteOutbound(code) {
    var Code = code.OutboundPlanNo;
    $.ajax({
        url: '../OutboundPlan/DeleteOutoundPlan?OutboundPlanNo=' + Code,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();

                msg("Inbound Plan was successfully removed.", "success");
            }
            else {
                msg("Failed to remove Inbound Plan", "warning");

            }
        }
    });
}