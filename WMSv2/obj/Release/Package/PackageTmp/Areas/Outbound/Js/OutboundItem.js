$(document).ready(function () {


    BindDropdownList('StockClassCode', '/Helper/Dropdown_ClassStock');
    BindDropdownList('StockClassCode2', '/Helper/Dropdown_ClassStock');
    BindDropdownList('ItemCode', '/Helper/Dropdown_Items');
    BindDropdownList('Location', '/Helper/Dropdown_Location');

    $('#Location').change(function () {
        BindDropdownList('SubLocation', '/Helper/Dropdown_SubLocation?LocationCode=' + $('#Location').val());
    })

    $('#btnsavecon').on('click', function (e) {

        e.preventDefault();
       
        AddOutboundItem($(this));
        


    });
    $('#OutboundItemModal').on('hidden.bs.modal', function (e) {
        initialized();
    })

    $("#ItemCode").change(function () {
        Clearall();
        $.ajax({
            url: '../ItemInbound/GetDetails?ItemCode=' + $('#ItemCode').val() + "&Type=",
            type: 'POST',
            cache: false,
            datatype: "json",
            success: function (returnData) {
                $('#pqc_q').val('');
                $('#qpic_q').val('');
                if (returnData.list.length == 0) {
                    $('#pqc_q').attr('readonly', true);
                    $('#qpic_q').attr('readonly', true);
                }
                else {
                    $('#pqc_q').attr('readonly', true);
                    $('#qpic_q').attr('readonly', true);
                    if (returnData.list[0].QtyperCase != null) {
                        $('#pqc_q').attr('readonly', false);
                    }
                    if (returnData.list[1].QtyperCase != null) {
                        $('#qpic_q').attr('readonly', false);
                    }

                }
            }
        });
    });

  

    $('#btnbackadd').on('click', function () {
        $('#additems').show();
        $('#frm_SettoInbound').hide();
        $('#OutboundItemModal').find('.modal-title').text("Inbound Item");
        initialized_inboundItem();
        initialized_inbounded();
        Clearall();
        $("#QtyPerUnit").prop("readonly", false);
        settoinbound = false;
    })
});


var settoinbound = false;
function initialized_outboundItem() {
    $('#OutboundItemTable').DataTable({
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
            { title: "ItemCode", data: "ItemCode",visible: false  },
            { title: "Item", data: "ItemName" },
            { title: "CodeName", data: "CodeName" },
            { title: "Po No", data: "PoNo" },
            { title: "Lot No", data: "LotNo" },
            { title: "Case", data: "QtyPerCase" },
            { title: "Inner Case", data: "QtyPerInnerCase" },
            { title: "Per Unit", data: "QtyPerUnit" },
            { title: "Plan Qty", data: "SUMQty" },
            //{ title: "Actual Received", data: "ActualReceived" },
            {
                title: "Action",
                render: function (data, type, full, meta) {

                    data = '<input type="hidden" id="withChange_' + meta.row + '" value="0" class="withChange_' + meta.row + '">'
                   + '</button><button type="button" class="btn btn-sm btn-info btn-flat editinput btninbound_' + meta.row + ' btninbound " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="View">'
                   + '<i class="fa fa-archive"></i></button>'

                   + '</button><button type="button" class="btn btn-sm btn-danger btn-flat editinput btndel_' + meta.row + ' btndel " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Delete">'
                   + '<i class="fa fa-trash-o"></i></button>'

                    return data;
                }, searchable: false, orderable: false
            },
        ],

    }).columns.adjust();

    //$('#InboundItemTable tbody').one('click', '.btndel', function () {
    //    var count = $(this).data('count');
    //    var tabledata = $('#InboundItemTable').DataTable();
    //    var data = tabledata.row($(this).parents('tr')).data();
    //    DeleteInboundItem(data);
    //});

    $('#OutboundItemTable tbody').one('click', '.btninbound', function () {
        var tabledata = $('#OutboundItemTable').DataTable();
        var data = tabledata.row($(this).parents('tr')).data();

        $.ajax({
            url: '../OutboundItem/GetOutboundItem?ID=' + data.ID,
            type: 'POST',
            cache: false,
            datatype: "json",
            success: function (returnData) {
                Clearall();
                $('#ItemCode').val(data.ItemCode);
                $('#pqc_q').val(data.QtyPerCase);
                $("#pqc_q").prop("readonly", true);
                $('#pqc_q').keyup();
                $('#qpic_q').val(data.QtyPerInnerCase);
                $("#qpic_q").prop("readonly", true);
                $('#qpic_q').keyup();
                $('#QtyPerUnit').val(data.QtyPerUnit);
                $("#QtyPerUnit").prop("readonly", true);
                $('#QtyPerUnit').keyup();
                $('#InboundDate').val(moment(returnData.item.PlanDate).format("MM/DD/YYYY"));
                settoinbound = true;
                $('#StockClassCode').val(returnData.item.StockClassCode);
                $('#ExpirationDate').val(moment(returnData.item.ExpirationDate).format("MM/DD/YYYY"));
                $('#LotNo').val(returnData.item.LotNo);
                $('#PoNo').val(returnData.item.PoNo);
                
                
                //$('#OutboundItemModal').find('.modal-title').text("Set Inbound - " + data.ItemName);
               
                initialized_outboundItem();
            }
        });

    });



}


function AddOutboundItem(data) {

    data = $('#frm_OBitems').serialize()
    $.ajax({
        url: '../OutboundItem/CreateOutboundItem',
        data: data,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized_outboundItem();
                Clearall();
                msg("Outbound Item was successfully saved.", "success");
            }
            else {
                msg("Outbound Item already registered", "warning");
            }
        }
    });
}

