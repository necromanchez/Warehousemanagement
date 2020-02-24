$(document).ready(function () {
    initialized();

});

function initialized() {

    $('#OutboundTable').DataTable({
        ajax: {
            url: '../OutboundSummary/GetOutboundList',
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
        ],

    }).columns.adjust();
}