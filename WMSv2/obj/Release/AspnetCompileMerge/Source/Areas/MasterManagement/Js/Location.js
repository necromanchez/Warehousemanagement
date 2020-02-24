$(document).ready(function () {
    initialized();
    $('#frm_location').on('submit', function (e) {
        e.preventDefault();
        

        if ($('#LocationName').val() != ""
                && $('#LocationDesc').val() != ""
                && $('#CapacityUnity').val() != ""
                && $('#Capacity').val() != ""
                ) {
            if ($('#ID').val() == "") {
                AddLocation();
            }
            else {
                EditLocation();
            }
        }


    });

    $('#AddLocation').on('hidden.bs.modal', function (e) {
        initialized();
    })
});


function initialized() {
    //$.ajax({
    //    url: '../LocationMaster/GetLocationList',
    //    type: 'POST',
    //    cache: false,
    //    datatype: "json",
    //    success: function (returnData) {
            $('#LocationTable').DataTable({
                ajax: {
                    url: '../LocationMaster/GetLocationList',
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
                            $('#AddLocation').modal('show')
                            Clearall();
                            $('#AddLocation').find('.modal-title').text("Add Location");
                        }
                    },
                ],
                destroy: true,
                //data: returnData.list,
                columns: [
                    { title: "LocationCode", data: "LocationCode", visible:false },
                    { title: "Location", data: "LocationName", name: "LocationName" },
                    { title: "Description", data: "LocationDesc", name: "LocationDesc" },
                    { title: "Capacity Unit", data: "CapacityUnity", name: "CapacityUnity" },
                    { title: "Capacity", data: "Capacity", name: "Capacity" },
                    {
                         title: "Action",
                         render: function (data, type, full, meta) {
                             data = '<input type="hidden" id="withChange_' + meta.row + '" value="0" class="withChange_' + meta.row + '">'
                                 + '<button type="button" class="btn btn-warning btn-sm btn-flat btnedit_' + meta.row + ' btnedit" data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Modify">'
                                 + '<i class="fa fa-edit"></i>'
                                 + '</button><button type="button" class="btn btn-sm btn-danger btn-flat editinput btndel_' + meta.row + ' btndel " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Delete">'
                                 + '<i class="fa fa-trash-o"></i></button>'
                                 + '</button><button type="button" class="btn btn-sm btn-info btn-flat editinput btnview_' + meta.row + ' btnview " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="View">'
                                 + '<i class="fa fa-desktop"></i></button>'
                             return data;
                         }, searchable: false, orderable: false
                    },
                ],

            });
            $('#LocationTable tbody').off('click')
            $('#LocationTable tbody').on('click', '.btnedit', function () {
                var count = $(this).data('count');
                var tabledata = $('#LocationTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();

                $('#LocationName').val(data.LocationName);
                $('#LocationDesc').val(data.LocationDesc);
                $('#Capacity').val(data.Capacity);
                $('#CapacityUnity').val(data.CapacityUnity);
                $('#ID').val(data.LocationCode);
    
                $('#AddLocation').find('.modal-title').text("Edit Location");

                $('#AddLocation').modal('show')
            });

            $('#LocationTable tbody').on('click', '.btndel', function () {
                var count = $(this).data('count');
                var tabledata = $('#LocationTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                DeletePopup(data);
            });

            $('#LocationTable tbody').on('click', '.btnview', function () {
                var count = $(this).data('count');
                var tabledata = $('#LocationTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                window.location = "/MasterManagement/LocationSub?LocationCode=" + data.LocationCode;
            });

        }
//    });
//}

function AddLocation() {

    var LocationData = {
        SiteCode: $('#SiteCode').val()
        , LocationName: $('#LocationName').val()
        , LocationDesc: $('#LocationDesc').val()
        , CapacityUnity: $('#CapacityUnity').val()
        , Capacity: $('#Capacity').val()
    }
    $.ajax({
        url: '../LocationMaster/CreateLocation',
        data: LocationData,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddLocation').modal('hide');
                msg("Location was successfully saved.", "success");
            }
            else {
                msg("Location already registered", "warning");
            }

        }
    });
}

function EditLocation() {
    var LocationData = {
        SiteCode: $('#SiteCode').val()
       , LocationName: $('#LocationName').val()
       , LocationDesc: $('#LocationDesc').val()
       , CapacityUnity: $('#CapacityUnity').val()
       , Capacity: $('#Capacity').val()
       , LocationCode: $('#ID').val()
    }
    $.ajax({
        url: '../LocationMaster/EditLocation',
        data: LocationData,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddLocation').modal('hide');
                msg("Location was successfully saved.", "success");
            }
            else {
                msg("Location already registered", "warning");
            }
        }
    });
}

function DeleteData(location) {
    var LocationCode = location.LocationCode;
    $.ajax({
        url: '../LocationMaster/DeleteLocation?LocationCode=' + LocationCode,
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