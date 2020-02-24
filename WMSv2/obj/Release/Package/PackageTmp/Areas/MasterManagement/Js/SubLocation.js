$(document).ready(function () {
    initialized();
    $('#frm_sublocation').on('submit', function (e) {
        e.preventDefault();
        

        if ($('#LocationName').val() != ""
                && $('#LocationName').val() != ""
                && $('#Capacity').val() != ""
                ) {
            if ($('#ID').val() == "") {
                AddSubLocation();
            }
            else {
                EditLocation();
            }
        }


    });

    $('#AddSubLocation').on('hidden.bs.modal', function (e) {
        initialized();
    })
});
var LocationCode = getParameterByName("LocationCode");


function initialized() {
    $.ajax({
        url: '/MasterManagement/LocationSub/GetLocationList?LocationCode=' + LocationCode,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            $('#link7').find('.card-title').text("Sub-Location - " + returnData.LocationName);
            $('#LocationTableSub').DataTable({
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: '  Add  ',
                        className: 'btn btn-success btn-flat nc-icon nc-bank',
                        action: function (e, dt, node, config) {
                            $('#AddSubLocation').modal('show')
                            Clearall();
                            $('#LocationName').val(returnData.LocationName);
                            $('#AddSubLocation').find('.modal-title').text("Add Sub-Location");
                        }
                    },
                ],
                destroy: true,
                data: returnData.list,
                columns: [
                    { title: "LocationSubCode", data: "LocationSubCode", visible:false },
                    { title: "Location", data: "LocationSubName" },
                    { title: "Capacity", data: "Capacity" },
                    {
                         title: "Action",
                         render: function (data, type, full, meta) {
                             data = '<input type="hidden" id="withChange_' + meta.row + '" value="0" class="withChange_' + meta.row + '">'
                                 + '<button type="button" class="btn btn-warning btn-sm btn-flat btnedit_' + meta.row + ' btnedit" data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Modify">'
                                 + '<i class="fa fa-edit"></i>'
                                 + '</button><button type="button" class="btn btn-sm btn-danger btn-flat editinput btndel_' + meta.row + ' btndel " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Delete">'
                                 + '<i class="fa fa-trash-o"></i></button>'
                               
                             return data;
                         }, searchable: false, orderable: false
                    },
                ],

            });

            $('#LocationTableSub tbody').one('click', '.btnedit', function () {
                var count = $(this).data('count');
                var tabledata = $('#LocationTableSub').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();

                $('#LocationName').val(returnData.LocationName);
                $('#LocationSubName').val(data.LocationSubName);
                $('#Capacity').val(data.Capacity);
                $('#ID').val(data.LocationSubCode);
                $('#AddSubLocation').find('.modal-title').text("Edit Sub-Location");

                $('#AddSubLocation').modal('show')
            });

            $('#LocationTableSub tbody').one('click', '.btndel', function () {
                var count = $(this).data('count');
                var tabledata = $('#LocationTableSub').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                DeleteSubLocation(data);
            });


        }
    });
}

function AddSubLocation() {

    var SubLocationData = {
        SiteCode: $('#SiteCode').val()
        , LocationCode: LocationCode
        , LocationSubName: $('#LocationSubName').val()
        , Capacity: $('#Capacity').val()
    }
    $.ajax({
        url: '../LocationSub/CreateSubLocation',
        data: SubLocationData,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddSubLocation').modal('hide');
                msg("Location was successfully saved.", "success");
            }
            else {
                msg("Location already registered", "warning");
            }

        }
    });
}

function EditLocation() {
    var SubLocationData = {
        SiteCode: $('#SiteCode').val()
       , LocationCode: LocationCode
       , LocationSubName: $('#LocationSubName').val()
       , Capacity: $('#Capacity').val()
       , LocationSubCode : $('#ID').val()
    }
    $.ajax({
        url: '../LocationSub/EditSubLocation',
        data: SubLocationData,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddSubLocation').modal('hide');
                msg("Location was successfully saved.", "success");
            }
            else {
                msg("Location already registered", "warning");
            }
        }
    });
}

function DeleteSubLocation(location) {
    var LocationSubCode = location.LocationSubCode;
    $.ajax({
        url: '../LocationSub/DeleteSubLocation?SubLocationCode=' + LocationSubCode,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();

                msg("Sites was successfully removed.", "success");
            }
            else {
                msg("Failed to remove Sites", "warning");

            }
        }
    });
}