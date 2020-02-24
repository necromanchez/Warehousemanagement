$(document).ready(function () {
    initialized();
  
    $('#btnsave').on("click", function () {
        EditItemPrice();
    })
    $('#AddPrice').on('hidden.bs.modal', function (e) {
        initialized();
    })
});


function initialized() {
    //$.ajax({
    //    url: '../ItemPriceMaster/GetItemPriceList',
    //    type: 'POST',
    //    cache: false,
    //    datatype: "json",
    //    success: function (returnData) {
            $('#PriceMasterTable').DataTable({
                ajax: {
                    url: '../ItemPriceMaster/GetItemPriceList',
                    type: "POST",
                    datatype: "json"
                },
                serverSide: "true",
                order: [0, "asc"],
                processing: "true",
                language: {
                    "processing": "processing... please wait"
                },
                destroy: true,
                //data: returnData.list,
                columns: [
                    { title: "ItemCode", data: "ItemCode", visible: false },
                    { title: "ItemName", data: "ItemName", name: "ItemName" },
                    { title: "Currency", data: "Currency", name: "Currency" },
                    { title: "Unit Price", data: "UnitPrice", name: "UnitPrice" },
                    {
                        title: "Action",
                        render: function (data, type, full, meta) {
                            data = '<input type="hidden" id="withChange_' + meta.row + '" value="0" class="withChange_' + meta.row + '">'
                                + '<button type="button" class="btn btn-warning btn-sm btn-flat btnedit_' + meta.row + ' btnedit" data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Update">'
                                + '<i class="fa fa-edit"></i>'
                            return data;
                        }, searchable: false, orderable: false
                    },
                ],

            });

            $('#PriceMasterTable tbody').one('click', '.btnedit', function () {
                var count = $(this).data('count');
                var tabledata = $('#PriceMasterTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();

                $('#ItemCode').val(data.ItemCode);
                $('#ItemName').val(data.ItemName);
                $('#Currency').val(data.Currency);
                $('#Unit').val(data.UnitPrice);

                $('#AddPrice').find('.modal-title').text("Update Price");

                $('#AddPrice').modal('show')
            });

          

          

        }
//    });
//}



function EditItemPrice() {
    var ItemPriceData = {
        ItemCode: $('#ItemCode').val()
       , Currency: $('#Currency').val()
       , UnitPrice: $('#Unit').val()
    }
    $.ajax({
        url: '../ItemPriceMaster/EditItemPrice',
        data: ItemPriceData,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddPrice').modal('hide');
                msg("Price was successfully saved.", "success");
            }
            else {
                msg("Price already registered", "warning");
            }
        }
    });
}

