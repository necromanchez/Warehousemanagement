$(document).ready(function () {
    initialized();
    
});

function initialized() {

    $('#InboundResultTable').DataTable({
        ajax: {
            url: '../InboundResult/GetInboundResultList',
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
            { title: "CodeSupplier", data: "CodeSupplier", visible: false },
            { title: "CodeClassInbound", data: "CodeClassInbound", visible: false },
            { title: "PlanCode", data: "InboundPlanCode", name: "InboundPlanCode" },
            {
                title: "PlanDate", data: function (x) {
                    return moment(x.InboundPlanDate).format("MM/DD/YYYY")
                }
            , name: "InboundPlanDate"
            },
            { title: "Supplier", data: "CodeSupplier", name: "CodeSupplier" },
            { title: "Slip No", data: "SlipNo", name: "SlipNo" },
            {
                title: "Slip Date", data: function (x) {
                    return moment(x.SlipDate).format("MM/DD/YYYY")
                }
            , name: "SlipDate" },
            { title: "Class", data: "CodeClassInbound", name: "CodeClassInbound" },
            { title: "Status", data: "InboundStatus", name: "InboundStatus" },
            { title: "PrintStatus", data: "PrintStatus", name: "PrintStatus" },
          
        ],

    }).columns.adjust();
}