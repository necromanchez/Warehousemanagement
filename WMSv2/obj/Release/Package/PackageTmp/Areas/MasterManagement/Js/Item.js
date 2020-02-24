$(document).ready(function () {
    initialized();
    BindDropdownList('unit', '/Helper/Dropdown_Units');
    BindDropdownList('Measurement', '/Helper/Dropdown_Units');
    BindDropdownList('CusSupCode', '/Helper/Dropdown_ClassOwner');
    BindDropdownList('CargoClass', '/Helper/Dropdown_CargoClass');
    $('#frm_Item').on('submit', function (e) {
        e.preventDefault();

        if ($('#ItemName').val() != ""
                && $('#Description').val() != ""
                && $('#CargoClass').val() != ""
                && $('#Measurement').val() != ""
                ) {
            if ($('#ID').val() == "") {
                AddItem();
            }
            else {
                EditItem();
            }
        }
    });

    $('#AddItem').on('hidden.bs.modal', function (e) {
        initialized();
    })
    $('#ConversionModal').on('hidden.bs.modal', function (e) {
        initialized();
    })

    $('#ItemSupModal').on('hidden.bs.modal', function (e) {
        initialized();
    })

    $('#btnsavecon').on('click', function (e) {
        var conData = {
            FromUnitType: $('#fromUnit').val()
          , FromUnit: $('#frUnit').val()
          , ToUnitType: $('#TUnit').val()
          , ToUnit: $('#ToUnit').val()
          , ItemCode: $('#itemcodes').val()
        }
            $.ajax({
                url: '../ItemMaster/CreateItemcon',
                data: conData,
                type: 'POST',
                cache: false,
                datatype: "json",
                success: function (returnData) {
                    if (returnData.result == "success") {
                        
                        Clearall();
                        reloadconv();
                        msg("Details was successfully saved.", "success");
                    }
                    else {
                        msg("Details already registered", "warning");
                    }

                }
            });
    });

    $('#btnsavesup').on('click', function (e) {
        var supData = {
            CusSupCode: $('#supplier').val()
            , Name: $('#supplier').text().trim()
            , ItemCode: $('#itemcodes').val()
        }
        $.ajax({
            url: '../ItemMaster/CreateItemsup',
            data: supData,
            type: 'POST',
            cache: false,
            datatype: "json",
            success: function (returnData) {
                if (returnData.result == "success") {

                    Clearall();
                    reloadsup();
                    msg("Details was successfully saved.", "success");
                }
                else {
                    msg("Details already registered", "warning");
                }

            }
        });
    });


    
    
});


function initialized() {
    //$.ajax({
    //    url: '../ItemMaster/GetItemList',
    //    type: 'POST',
    //    cache: false,
    //    datatype: "json",
    //    success: function (returnData) {
        $('#ItemTable').DataTable({
            ajax: {
                    url: '../ItemMaster/GetItemList',
                    type: "POST",
                    datatype: "json"
                },
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
                        className: 'btn btn-success btn-flat fa fa-plus',
                        action: function (e, dt, node, config) {
                            $('#AddItem').modal('show')
                            Clearall();
                            $('#AddItem').find('.modal-title').text("Add Item");
                            
                        }
                    },
                ],
                destroy: true,
                //data: returnData.list,
                columns: [
                    { title: "ItemCode", data: "ItemCode", visible: false },
                    { title: "Item Name", data: "ItemName", name: "ItemName" },
                    { title: "Description", data: "Description", name: "Description" },
                    { title: "Owner", data: "CusSupCode", name: "CusSupCode" },
                    { title: "Cargo Class", data: "CargoClass", name: "CargoClass" },
                    { title: "Base Measurement", data: "BaseMeasurement", name: "BaseMeasurement" },
                    {
                        title: "Action",
                        render: function (data, type, full, meta) {
                            data = '<input type="hidden" id="withChange_' + meta.row + '" value="0" class="withChange_' + meta.row + '">'
                                + '<button type="button" class="btn btn-warning btn-sm btn-flat btnedit_' + meta.row + ' btnedit" data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Modify">'
                                + '<i class="fa fa-edit"></i>'
                                + '</button><button type="button" class="btn btn-sm btn-danger btn-flat editinput btndel_' + meta.row + ' btndel " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Delete">'
                                + '<i class="fa fa-trash-o"></i></button>'
                                + '</button><button type="button" class="btn btn-sm btn-info btn-flat editinput btncon_' + meta.row + ' btncon " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Conversion">'
                                + '<i class="fa fa-dashboard"></i></button>'
                                + '</button><button type="button" class="btn btn-sm btn btn-secondary btn-flat editinput btnsup_' + meta.row + ' btnsup " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Supplier">'
                                + '<i class="fa fa-bus"></i></button>'
                                + '</button><button type="button" class="btn btn-sm btn btn-primary btn-flat editinput btncase_' + meta.row + ' btncase " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Unit">'
                                + '<i class="fa fa-dropbox"></i></button>'
                            return data;
                        }, searchable: false, orderable: false
                    },
                ],

            });

            $('#ItemTable tbody').off('click');
            $('#ItemTable tbody').on('click', '.btnedit', function () {
                var count = $(this).data('count');
                var tabledata = $('#ItemTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();

                $('#ItemName').val(data.ItemName);
                $('#Description').val(data.Description);
                $('#CusSupCode').val(data.CusSupCode);
                $('#CargoClass').val(data.CargoClass);
                $('#Measurement').val(data.BaseMeasurement);
                $('#ID').val(data.ItemCode);

                $('#AddItem').find('.modal-title').text("Edit Item");

                $('#AddItem').modal('show')
            });

            $('#ItemTable tbody').on('click', '.btndel', function () {
                var count = $(this).data('count');
                var tabledata = $('#ItemTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                DeletePopup(data);
            });

            $('#ItemTable tbody').on('click', '.btncon', function () {
                var count = $(this).data('count');
                var tabledata = $('#ItemTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                $('#itemcodes').val(data.ItemCode);
                reloadconv();
            });

            $('#ItemTable tbody').on('click', '.btnsup', function () {
                var count = $(this).data('count');
                var tabledata = $('#ItemTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                $('#itemcodes').val(data.ItemCode);
                reloadsup();
            });

            $('#ItemTable tbody').on('click', '.btncase', function () {
                var count = $(this).data('count');
                var tabledata = $('#ItemTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                $('#itemcodes').val(data.ItemCode);
                theItemCode = data.ItemCode;
                getcase(data);
            });

        }
//    });
//}
var theItemCode = "";
function AddItem() {
    var ItemData = {
        ItemName: $('#ItemName').val()
        , Description: $('#Description').val()
        , CusSupCode: $('#CusSupCode').val()
        , CargoClass: $('#CargoClass').val()
        , BaseMeasurement: $('#Measurement').val()
        , ItemCode: $('#ID').val()
    }
    $.ajax({
        url: '../ItemMaster/CreateItem',
        data: ItemData,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddItem').modal('hide');
                msg("Details was successfully saved.", "success");
            }
            else {
                msg("Details already registered", "warning");
            }

        }
    });
}

function EditItem() {
    var ItemData = {
        ItemName: $('#ItemName').val()
      , Description: $('#Description').val()
      , CusSupCode: $('#CusSupCode').val()
      , CargoClass: $('#CargoClass').val()
      , BaseMeasurement: $('#Measurement').val()
      , ItemCode : $('#ID').val()
    }
    $.ajax({
        url: '../ItemMaster/EditItem',
        data: ItemData,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddItem').modal('hide');
                msg("Details was successfully saved.", "success");
            }
            else {
                msg("Details already registered", "warning");
            }
        }
    });
}

function DeleteData(item) {
    var itemcode = item.ItemCode;
    $.ajax({
        url: '../ItemMaster/DeleteItem?ItemCode=' + itemcode,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();

                msg("Details was successfully removed.", "success");
            }
            else {
                msg("Failed to remove Details", "warning");

            }
        }
    });
}

function reloadconv() {
    $.ajax({
        url: '../ItemMaster/GetItemconList?ItemCode='+ $('#itemcodes').val(),
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            $('#ItemconTable').DataTable({
                data: returnData.list,
                destroy: true,
                columns: [
                    { title: "From", data: "FromUnitType", name: "FromUnitType" },
                    { title: "Unit", data: "FromUnit", name: "FromUnit" },
                    { title: "To", data: "ToUnitType", name: "ToUnitType" },
                    { title: "Unit", data: "ToUnit", name: "ToUnit" },
                ],

            });

            $('#ConversionModal').find('.modal-title').text("Conversion - " + returnData.itemName);
            $('#ConversionModal').modal('show')
         
           
        }
    });
}

function reloadsup() {
    $.ajax({
        url: '../ItemMaster/GetItemsupList?ItemCode=' + $('#itemcodes').val(),
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            $('#ItemsupTable').DataTable({
                data: returnData.list,
                destroy: true,
                columns: [
                    { title: "ItemCode", data: "ItemCode", visible:false},
                    { title: "Name", data: "Name" },
                ],

            });

            $('#ItemSupModal').find('.modal-title').text("Supplier - " + returnData.itemName);
            $('#ItemSupModal').modal('show')


        }
    });
}

function getcase(item) {
    $('#ItemCaseModal').find('.modal-title').text("Categories - " + item.ItemName);
    Clearall();

    $.ajax({
        url: '../ItemMaster/GetUnits?ItemCode=' + item.ItemCode,
        type: 'GET',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            //$('[href=#' + returnData.itemunitlist[0].Type + ']').tab('show');
            $(".nav-link").removeClass("active");
            if (returnData.itemunitlist.length > 0) {
                switch(returnData.itemunitlist[0].Type)
                {
                    case "Case":
                        $("#ctab").addClass("active");
                        break;
                    case "Inner Case":
                        $("#icctab").addClass("active");
                        break;
                    case "Per Pieces":
                        $("#iptab").addClass("active");
                        break;
                }
                //CASE
                $('#stockqty').val(returnData.itemunitlist[0].MaintainingQty)
                $('#qtypercase').val(returnData.itemunitlist[0].QtyperCase)
                $('#unit').val(returnData.itemunitlist[0].Unit)
                $('#gwkg').val(returnData.itemunitlist[0].gwkg)
                $('#nwkg').val(returnData.itemunitlist[0].nwkg)
                $('#slcm').val(returnData.itemunitlist[0].slcm)
                $('#swcm').val(returnData.itemunitlist[0].swcm)
                $('#shcm').val(returnData.itemunitlist[0].shcm)
                //INNER Case
            
                $('#qtypercaseINNERCase').val(returnData.itemunitlist[1].QtyperCase)
                $('#unitINNERCase').val(returnData.itemunitlist[1].Unit)
                $('#gwkgINNERCase').val(returnData.itemunitlist[1].gwkg)
                $('#nwkgINNERCase').val(returnData.itemunitlist[1].nwkg)
                $('#slcmINNERCase').val(returnData.itemunitlist[1].slcm)
                $('#swcmINNERCase').val(returnData.itemunitlist[1].swcm)
                $('#shcmINNERCase').val(returnData.itemunitlist[1].shcm)

                //Per unit
             
                $('#qtypercasePerUnit').val(returnData.itemunitlist[0].QtyperCase)
                $('#unitPerUnit').val(returnData.itemunitlist[0].Unit)
                $('#gwkgPerUnit').val(returnData.itemunitlist[0].gwkg)
                $('#nwkgPerUnit').val(returnData.itemunitlist[0].nwkg)
                $('#slcmPerUnit').val(returnData.itemunitlist[0].slcm)
                $('#swcmPerUnit').val(returnData.itemunitlist[0].swcm)
                $('#shcmPerUnit').val(returnData.itemunitlist[0].shcm)

                $('#Category').val(returnData.itemcategory.Category)
                $('#Maker').val(returnData.itemcategory.Maker)
                $('#MOQ').val(returnData.itemcategory.MOQ)
                $('#SPQ').val(returnData.itemcategory.SPQ)
                $('#LTpocoverage').val(returnData.itemcategory.LTpocoverage)
                $('#SPpocoverage').val(returnData.itemcategory.SPpocoverage)
                $('#Prodfamily').val(returnData.itemcategory.Prodfamily)
                $('#PlanDT').val(returnData.itemcategory.PlanDT)
                $('#btnsaveunit').text("Update");

                $('.bootstrap-switch').removeClass("bootstrap-switch-on");
                $('.bootstrap-switch').removeClass("bootstrap-switch-off");
                if (!returnData.itemcategory.serialno) {

                    $(".bootstrap-switch-id-serialno").addClass("bootstrap-switch-off");
                }
                else {
                    $(".bootstrap-switch-id-serialno").removeClass("bootstrap-switch-on");
                }
                if (!returnData.itemcategory.imei1) {

                    $(".bootstrap-switch-id-imei1").addClass("bootstrap-switch-off");
                }
                else {
                    $(".bootstrap-switch-id-imei1").removeClass("bootstrap-switch-on");
                }
                if (!returnData.itemcategory.imei2) {

                    $(".bootstrap-switch-id-imei2").addClass("bootstrap-switch-off");
                }
                else {
                    $(".bootstrap-switch-id-imei2").removeClass("bootstrap-switch-on");
                }
                if (!returnData.itemcategory.stpoint) {

                    $(".bootstrap-switch-id-stpoint").addClass("bootstrap-switch-off");
                }
                else {
                    $(".bootstrap-switch-id-stpoint").removeClass("bootstrap-switch-on");
                }
                if (!returnData.itemcategory.ltpoint) {

                    $(".bootstrap-switch-id-ltpoint").addClass("bootstrap-switch-off");
                }
                else {
                    $(".bootstrap-switch-id-ltpoint").removeClass("bootstrap-switch-on");
                }
            }
            else {
                $('#btnsaveunit').text("Save");
                $('.nav-tabs [href="#Case]"').tab('show')
            }
        
  
    
            $('#ItemCaseModal').modal('show')
            
        }
    });

   
}