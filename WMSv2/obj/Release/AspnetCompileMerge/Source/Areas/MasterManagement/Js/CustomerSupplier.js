$(document).ready(function () {
    initialized();
    BindDropdownList('Class', '/Helper/Dropdown_ClassCusSup');
    BindDropdownList('SiteCode', '/Helper/Dropdown_Site');

    $('#frm_CusSup').on('submit', function (e) {
        e.preventDefault();


        if ($('#Class').val() != ""
                && $('#Address').val() != ""
                && $('#Name').val() != ""
                && $('#Contact').val() != ""
                && $('#EmailAddress').val() != ""
                ) {
            if ($('#ID').val() == "") {
                AddCusSup();
            }
            else {
                EditCusSup();
            }
        }


    });

    $('#AddCusSup').on('hidden.bs.modal', function (e) {
        initialized();
    })
});


function initialized() {
    //$.ajax({
    //    url: '../CustomerSupplier/GetCusSupList',
    //    type: 'POST',
    //    cache: false,
    //    datatype: "json",
    //    success: function (returnData) {
            $('#CusSupTable').DataTable({
                ajax: {
                    url: '../CustomerSupplier/GetCusSupList',
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
                            Clearall();
                            $('#SiteCode').val($('#Sitechoose').val());
                            $('#AddCusSup').modal('show')
                            
                            $('#AddCusSup').find('.modal-title').text("Add Customer/Supplier");
                        }
                    },
                ],
                destroy: true,
                //data: returnData.list,
                columns: [
                    { title: "SiteCode", data: "SiteCode", visible: false, searchable: false, orderable: false },
                    { title: "CusSupCode", data: "CusSupCode", visible: false, searchable: false, orderable: false },
                    { title: "Name", data: "Name", name: "Name" },
                    { title: "Class", data: "Class", name: "Class" },
                    { title: "Address", data: "Address", name: "Address" },
                    { title: "Contact", data: "Contact", name: "Contact" },
                    { title: "EmailAddress", data: "EmailAddress", name: "EmailAddress" },
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

            $('#CusSupTable tbody').off('click');
            $('#CusSupTable tbody').on('click', '.btnedit', function () {
                var count = $(this).data('count');
                var tabledata = $('#CusSupTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();

                $('#SiteCode').val(data.SiteCode);
                $('#Class').val(data.Class);
                $('#Name').val(data.Name);
                $('#Address').val(data.Address);
                $('#Contact').val(data.Contact);
                $('#EmailAddress').val(data.EmailAddress);
                $('#ID').val(data.CusSupCode);

                $('#AddCusSup').find('.modal-title').text("Edit Customer/Supplier");

                $('#AddCusSup').modal('show')
            });

            $('#CusSupTable tbody').on('click', '.btndel', function () {
                var count = $(this).data('count');
                var tabledata = $('#CusSupTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                DeletePopup(data);
            });

           

        }
//    });
//}

function AddCusSup() {

    var CusSupData = {
        SiteCode: $('#SiteCode').val()
        , Class: $('#Class').val()
        , Name: $('#Name').val()
        , Address: $('#Address').val()
        , Contact: $('#Contact').val()
        , EmailAddress: $('#EmailAddress').val()
    }
    $.ajax({
        url: '../CustomerSupplier/CreateCusSup',
        data: CusSupData,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddCusSup').modal('hide');
                msg("Details was successfully saved.", "success");
            }
            else {
                msg("Details already registered", "warning");
            }

        }
    });
}

function EditCusSup() {
    var CusSupData = {
        SiteCode: $('#SiteCode').val()
        , Class: $('#Class').val()
        , Name: $('#Name').val()
        , Address: $('#Address').val()
        , Contact: $('#Contact').val()
        , EmailAddress: $('#EmailAddress').val()
        , CusSupCode : $('#ID').val()
    }
    $.ajax({
        url: '../CustomerSupplier/EditCusSup',
        data: CusSupData,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddCusSup').modal('hide');
                msg("Details was successfully saved.", "success");
            }
            else {
                msg("Details already registered", "warning");
            }
        }
    });
}

function DeleteData(cussup) {
    var cussupCode = cussup.CusSupCode;
    $.ajax({
        url: '../CustomerSupplier/DeleteCusSup?CusSupCode=' + cussupCode,
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