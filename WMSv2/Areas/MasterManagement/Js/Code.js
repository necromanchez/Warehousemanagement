$(document).ready(function () {
    initialized();
    BindDropdownList('Class', '/Helper/Dropdown_CodeBack');
    $('#frm_code').on('submit', function (e) {
        e.preventDefault();
        

        if ($('#CodeName').val() != ""
                && $('#CodeDescription').val() != ""
                ) {
            if ($('#ID').val() == "") {
                AddCode();
            }
            else {
                EditCode();
            }
        }


    });

    $('#AddCode').on('hidden.bs.modal', function (e) {
        initialized();
    })
});


function initialized() {
            $('#CodeTable').DataTable({
                ajax: {
                    url: '../Code/GetCodeList',
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
                            $('#AddCode').modal('show')
                            Clearall();
                            $('#AddCode').find('.modal-title').text("Add Code");
                        }
                    },
                ],
                destroy: true,
                columns: [
                    { title: "Code", data: "Code", visible: false },
                    { title: "CodeGroup", data: "CodeGroup", name: "CodeGroup" },
                    { title: "CodeName", data: "CodeName", name: "CodeName" },
                    { title: "Code Description", data: "CodeDescription" , name: "CodeDescription" },
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
            $('#CodeTable tbody').off('click');
            $('#CodeTable tbody').on('click', '.btnedit', function () {
                var count = $(this).data('count');
                var tabledata = $('#CodeTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();

                $('#CodeName').val(data.CodeName);
                $('#CodeDescription').val(data.CodeDescription);
                $('#ID').val(data.Code);
    
                $('#AddCode').find('.modal-title').text("Edit Code");

                $('#AddCode').modal('show')
            });
            $('#CodeTable tbody').on('click', '.btndel', function () {
                var count = $(this).data('count');
                var tabledata = $('#CodeTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                DeletePopup(data);
            });

        }

function AddCode() {

    var CodeData = {
        CodeName: $('#CodeName').val()
        , CodeDescription: $('#CodeDescription').val()
    }

    $.ajax({
        url: '../Code/CreateCode',
        data: { code : CodeData, cc:$('#Class').val()},
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddCode').modal('hide');
                msg("Code was successfully saved.", "success");
            }
            else {
                msg("Code already registered", "warning");
            }

        }
    });
}

function EditCode() {
    var CodeData = {
        CodeName: $('#CodeName').val()
       , CodeDescription: $('#CodeDescription').val()
       , Code: $('#ID').val()
    }
    $.ajax({
        url: '../Code/EditCode',
        data: {code: CodeData },
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddCode').modal('hide');
                msg("Code was successfully saved.", "success");
            }
            else {
                msg("Code already registered", "warning");
            }
        }
    });
}

function DeleteData(code) {
    var Code = code.Code;
    $.ajax({
        url: '../Code/DeleteCode?Code=' + Code,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();

                msg("Code was successfully removed.", "success");
            }
            else {
                msg("Failed to remove Code", "warning");

            }
        }
    });
}