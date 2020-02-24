$(document).ready(function () {
    initialized();

});

function initialized() {

    $('#StockDetailedInventoryTable').DataTable({
        ajax: {
            url: '../StockDetailInventory/GetStockDetailedInventoryList',
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
            { title: "InboundPlanCode", data: "InboundPlanCode" },
            { title: "StockCode", data: "StockCode" },
            { title: "InboundNoResult", data: "InboundNoResult" },
            { title: "PO No", data: "PONo" },
           
             {
                 title: "ExpiredDate", data: function (x) {
                     return moment(x.ExpiredDate).format("MM/DD/YYYY")
                 }
             },
             {
                 title: "InboundDate", data: function (x) {
                     return moment(x.InboundDate).format("MM/DD/YYYY")
                 }
             },
            { title: "Location", data: "Location" },
            { title: "SubLocation", data: "SubLocation" },
            { title: "Actual Stock", data: "ActualStockQty" },
            { title: "Allocated", data: "AllocatedQty" },
            { title: "Picked", data: "PickedQty" },
           
             {
                 title: "Slip Date", data: function (x) {
                     return moment(x.SlipDate).format("MM/DD/YYYY")
                 }
             },
            { title: "Slip No", data: "SlipNo" },
             {
                 title: "Last Inbound Date", data: function (x) {
                     return moment(x.LastInboundDate).format("MM/DD/YYYY")
                 }
             },
               {
                   title: "Last Out boundDate", data: function (x) {
                       x.LastOutboundDate = (x.LastOutboundDate != null) ? moment(x.LastOutboundDate).format("MM/DD/YYYY") : ''
                       return x.LastOutboundDate
                 }
             },
           

        ],

    }).columns.adjust();
}