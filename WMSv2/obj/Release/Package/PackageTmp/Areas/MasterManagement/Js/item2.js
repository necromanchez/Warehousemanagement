$(document).ready(function () {
    BindDropdownList('fromUnit', '/Helper/Dropdown_Units');
    BindDropdownList('TUnit', '/Helper/Dropdown_Units');

    
    $('#ItemCaseModal').on('hidden.bs.modal', function (e) {
        initialized();
       
    })

    $('#btnsaveunit').on('click', function (e) {
    
        AddItemUnit(thetype);
    });


  

    //$('#tabs').on('shown.bs.tab', function (event) {
    //    var x = $(event.target).text();         // active tab
    //    var y = $(event.relatedTarget).text();  // previous tab
    //    Clearall();
    //    thetype = x;
    //        switch (x) {
    //            case "Case":
                    
    //                $("#qtyrow").show();
    //                $('#qtypercase').attr("placeholder", "Qty Per Case");
    //                break
    //            case "Inner Case":
    //                $("#qtyrow").show();
    //                $('#qtypercase').attr("placeholder", "Qty Per InnerCase");
    //                break
    //            default:
    //                $("#qtyrow").hide();
    //                break;
    //        }
    //});
   

});

var thetype = "Case";
function AddItemUnit() {
    var ItemUnitData = {
        Type: thetype
        , ItemCode: theItemCode
        , QtyperCase: $('#qtypercase').val()
        , Unit: $('#unit').val()
        , gwkg: $('#gwkg').val()
        , nwkg: $('#nwkg').val()
        , slcm: $('#slcm').val()
        , swcm: $('#swcm').val()
        , shcm: $('#shcm').val()
    }

    var ItemUnitDataINNER = {
        Type: "INNERCase"
        , ItemCode: theItemCode
        , QtyperCase: $('#qtypercaseINNERCase').val()
        , Unit: $('#unitINNERCase').val()
        , gwkg: $('#gwkgINNERCase').val()
        , nwkg: $('#nwkgINNERCase').val()
        , slcm: $('#slcmINNERCase').val()
        , swcm: $('#swcmINNERCase').val()
        , shcm: $('#shcmINNERCase').val()
    }

    var ItemUnitDataPerUnit = {
        Type: "PerUnit"
       , ItemCode: theItemCode
       , QtyperCase: $('#qtypercasePerUnit').val()
       , Unit: $('#unitPerUnit').val()
       , gwkg: $('#gwkgPerUnit').val()
       , nwkg: $('#nwkgPerUnit').val()
       , slcm: $('#slcmPerUnit').val()
       , swcm: $('#swcmPerUnit').val()
       , shcm: $('#shcmPerUnit').val()
    }

    var ItemCategory = {
         ItemCode: theItemCode
        , Category: $('#Category').val()
        , Maker: $('#Maker').val()
        , MOQ: $('#MOQ').val()
        , SPQ: $('#SPQ').val()
        , LTpocoverage: $('#LTpocoverage').val()
        , SPpocoverage: $('#SPpocoverage').val()
        , Prodfamily: $('#Prodfamily').val()
        , PlanDT: $('#PlanDT').val()
        , serialno: ($('#serialno').is(":checked")) ? true : false
        , imei1: ($('#imei1').is(":checked")) ? true : false
        , imei2: ($('#imei2').is(":checked")) ? true : false
        , stpoint: ($('#stpoint').is(":checked")) ? true : false
        , ltpoint: ($('#ltpoint').is(":checked")) ? true : false
        , remarks: $('#remarks').val()
    }
    $.ajax({
        url: '../ItemMaster/CreateItemUnit',
        data: { itemunit: ItemUnitData, stockqty: $('#stockqty').val(), InnerCase: ItemUnitDataINNER, PerUnit: ItemUnitDataPerUnit },
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                $.ajax({
                    url: '../ItemMaster/CreateItemCategory',
                    data: ItemCategory,
                    type: 'POST',
                    cache: false,
                    datatype: "json",
                    success: function (returnData) {
                        initialized();
                        Clearall();
                        $('#ItemCaseModal').modal('hide');
                    }
                });
                //msg("Details was successfully saved.", "success");
            }
            else {
                msg("Details already registered", "warning");
            }

        }
    });
}