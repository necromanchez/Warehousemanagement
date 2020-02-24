$(document).ready(function () {
    initialized();

});

function initialized() {

    $('#StockInventoryTable').DataTable({
        ajax: {
            url: '../StockInventory/GetStockInventoryList',
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
            { title: "Class", data: "StockClass" },
            { title: "Item Name", data: "ItemName" },
            { title: "Quantity", data: "OverAllQty" },
           
        ],

    }).columns.adjust();
}