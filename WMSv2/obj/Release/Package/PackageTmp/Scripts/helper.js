$(document).ready(function () {
    BindDropdownListSt('Sitechoose', '/Helper/Dropdown_Site');
   


    $(".call").keyup(function () {
        $('#InboundPlanQty').val();
        var sum = 0;

        var Type = "";
        switch ($(this).attr("id")) {
            case "pqc_q":
                Type = "Case";
                $.ajax({
                    url: '../ItemInbound/GetDetails?ItemCode=' + $('#ItemCode').val() + "&Type=" + Type,
                    type: 'POST',
                    cache: false,
                    datatype: "json",
                    success: function (returnData) {
                        if (returnData.unit != null) {
                            $('#QtyPerCase').val(returnData.unit.QtyperCase * $('#pqc_q').val());
                            $('#kgCase').val((returnData.unit.gwkg == null) ? "0 KG" : returnData.unit.gwkg * $('#pqc_q').val() + " KG");
                            $('#ngCase').val((returnData.unit.nwkg == null) ? "0 KG" : returnData.unit.nwkg * $('#pqc_q').val() + " KG");
                        }

                        $(".cal").each(function () {
                            var val = $.trim($(this).val());
                            if (val) {
                                val = parseFloat(val.replace(/^\$/, ""));
                                sum += !isNaN(val) ? val : 0;
                            }
                        });
                        $('#InboundPlanQty').val(sum);
                    }
                });
                break;
            case "qpic_q":
                Type = "INNERCase";
                $.ajax({
                    url: '../ItemInbound/GetDetails?ItemCode=' + $('#ItemCode').val() + "&Type=" + Type,
                    type: 'POST',
                    cache: false,
                    datatype: "json",
                    success: function (returnData) {
                        if (returnData.unit != null) {
                            $('#QtyPerInnerCase').val(returnData.unit.QtyperCase * $('#qpic_q').val());
                            $('#kgInner').val((returnData.unit.gwkg == null) ? "0 KG" : returnData.unit.gwkg * $('#qpic_q').val() + " KG");
                            $('#ngInner').val((returnData.unit.nwkg == null) ? "0 KG" : returnData.unit.nwkg * $('#qpic_q').val() + " KG");
                        }

                        $(".cal").each(function () {
                            var val = $.trim($(this).val());
                            if (val) {
                                val = parseFloat(val.replace(/^\$/, ""));
                                sum += !isNaN(val) ? val : 0;
                            }
                        });
                        $('#InboundPlanQty').val(sum);
                    }
                });
                break;
            case "QtyPerUnit":
                Type = "PerUnit";
                $.ajax({
                    url: '../ItemInbound/GetDetails?ItemCode=' + $('#ItemCode').val() + "&Type=" + Type,
                    type: 'POST',
                    cache: false,
                    datatype: "json",
                    success: function (returnData) {
                        if (returnData.unit == null) {
                            $('#kgpi').val("0 KG");
                        }
                        else if (returnData.unit.gwkg == null) {
                            $('#kgpi').val("0 KG");
                        }
                        else {
                            $('#kgpi').val(returnData.unit.gwkg + " KG");
                        }
                        if (returnData.unit == null) {
                            $('#ngpi').val("0 KG")
                        }
                        else if (returnData.unit.nwkg == null) {
                            $('#ngpi').val("0 KG")
                        }
                        else {
                            $('#ngpi').val(returnData.unit.nwkg + " KG")
                        }

                        $(".cal").each(function () {
                            var val = $.trim($(this).val());
                            if (val) {
                                val = parseFloat(val.replace(/^\$/, ""));
                                sum += !isNaN(val) ? val : 0;
                            }
                        });
                        $('#InboundPlanQty').val(sum);
                    }
                });
                break;

        }


    });
});

function msg(message, status) {
    type = ["", "info", "danger", "success", "warning", "rose", "primary"],
    color = Math.floor(6 * Math.random() + 1),
    icon = (status == "success") ? 'done' : "warning";
    $.notify({
        icon: icon,
        message: message
    }, {
        type: status,
        timer: 3e3,
        placement: {
            from: 'top',
            align: 'right'
        }
    })
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function Clearall() {
    $('.clear').val("");
    $('.selectpicker').selectpicker('refresh');
    $('.form-group').removeClass('has-danger');
    $('.form-check').removeClass('has-danger');
    $('.form-group').removeClass('has-success');
    $('.form-check').removeClass('has-success');
    $('.form-group').removeClass('is-filled');
    $('.form-check').removeClass('is-filled');
    $('.selectpicker').selectpicker('refresh');
    $('.bootstrap-switch').removeClass("bootstrap-switch-on");
    $('.bootstrap-switch').removeClass("bootstrap-switch-off");
    $('.bootstrap-switch').addClass("bootstrap-switch-on");
}


BindDropdownList = function (id, url) {
    var select = $('#' + id);

    $.ajax({
        async: true,
        type: "POST",
        cache: false,
        url: url,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (returndata) {
            select.empty();
            select.append($('<option></option>').val(0).html("- SELECT -"));
            if (returndata.ok) {
                $.each(returndata.data, function (index, itemData) {
                    select.append($('<option></option>').val(itemData.value).html(itemData.text));
                });
            }
            $('.selectpicker').selectpicker('refresh');
        }
    });
}

BindDropdownListSt = function (id, url) {
    var select = $('#' + id);

    $.ajax({
        async: true,
        type: "POST",
        cache: false,
        url: url,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (returndata) {
         
            select.empty();
            if (returndata.ok) {
                $.each(returndata.data, function (index, itemData) {
                    select.append($('<option></option>').val(itemData.value).html(itemData.text));
                });
            }
            $('.selectpicker').selectpicker('refresh');
            //$("#Sitechoose").val($("#Sitechoose option:first").val());
            $('select[name="Sitechoose"]').find('option[value="' + $('#globalSite').val() + '"]').attr("selected", true);
            $('#Sitechoose').change(function () {
                $.ajax({
                    url: '/Login/SetSite?Sitecode=' + $('#Sitechoose').val(),
                    type: 'POST',
                    cache: false,
                    datatype: "json",
                    success: function (returnData) {
                        location.reload();
                        initialized();
                    }
                });
            })
        }
    });
}

function DeletePopup(data,functionCall){
    swal({
        title: 'Are you sure?',
        text: 'You will not be able to recover this data!',
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, keep it',
        confirmButtonClass: "btn btn-success",
        cancelButtonClass: "btn btn-danger",
        buttonsStyling: false
    }).then(function () {
        DeleteData(data);
        swal({
            title: 'Deleted!',
            text: 'Your imaginary file has been deleted.',
            type: 'success',
            confirmButtonClass: "btn btn-success",
            buttonsStyling: false
        }).catch(swal.noop);
    }, function (dismiss) {
        // dismiss can be 'overlay', 'cancel', 'close', 'esc', 'timer'
        if (dismiss === 'cancel') {
            swal({
                title: 'Cancelled',
                text: 'Your imaginary file is safe :)',
                type: 'error',
                confirmButtonClass: "btn btn-info",
                buttonsStyling: false
            }).catch(swal.noop);
        }
    }).catch(swal.noop);
}
