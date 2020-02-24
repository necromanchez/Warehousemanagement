$(document).ready(function () {
    initialized();
    $('#frm_user').on('submit', function (e) {
        e.preventDefault();
        if ($('#UserID').val() != ""
                && $('#Password').val() != ""
                && $('#LastName').val() != ""
                && $('#MiddleName').val() != ""
                && $('#FirstName').val() != ""
                ) {
            if ($('#ID').val() == "") {
                AddUser();
            }
            else {
                EditUser();
            }
        }
            
        
    });

    $('#AddUser').on('hidden.bs.modal', function (e) {
        initialized();
    })
});


function initialized() {
          
   
           var table =  $('#UserMasterTable').DataTable({
                ajax: {
                    url: '../UserMaster/GetUserList',
                    type: "POST",
                    datatype: "json"
                },
                orderCellsTop: true,
                fixedHeader: true,
                serverSide: "true",
                order: [],
                sort: [],
                ordering: false,
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
                            $('#AddUser').modal('show')
                            $('#UserID').prop('disabled', false);
                            Clearall();
                            $('#AddUser').find('.modal-title').text("Add User");
                        }
                    },
                ],
                destroy: true,
                columns: [
                    { title: "UserID", data: "UserID", name: "UserID" },
                    { title: "First Name", data: "FirstName", name: "FirstName" },
                    { title: "Middle Name", data: "MiddleName", name: "MiddleName" },
                    { title: "Last Name", data: "LastName", name: "LastName" },
                    { title: "Approved", data: "Approved" ,visible:false},
                    { title: "Locked", data: "Locked", visible: false },
                    { title: "LoginAttempts", data: "LoginAttempts",visible:false},
                    { title: "SuperUser", data: "SuperUser", visible: false },
                    { title: "Reversal", data: "Reversal", visible: false },
                    { title: "IsManager", data: "IsManager", visible: false },
                    { title: "LastDateLoggedIn", data: "LastDateLoggedIn", visible: false },
                     {
                         title: "Action",
                         render: function (data, type, full, meta) {
                             data = '<input type="hidden" id="withChange_' + meta.row + '" value="0" class="withChange_' + meta.row + '">'
                                 + '<button type="button" class="btn btn-warning btn-sm btn-flat btnedit_' + meta.row + ' btnedit" data-count="' + meta.row + '">'
                                 + '<i class="fa fa-edit"></i>'
                                 + '</button><button type="button" class="btn btn-sm btn-danger btn-flat editinput btndel_' + meta.row + ' btndel " data-count="' + meta.row + '">'
                                 + '<i class="fa fa-trash-o"></i></button>'
                             return data;
                         }, searchable: false, orderable: false
                     },
                ],
         
            });
             
            $('#UserMasterTable tbody').off('click');
            $('#UserMasterTable tbody').on('click', '.btnedit', function () {
                var count = $(this).data('count');
                var tabledata = $('#UserMasterTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                $('#UserID').prop('disabled','disabled');
                $('#ID').val(data.UserID);
                $('#UserID').val(data.UserID);
                $('#LastName').val(data.LastName);
                $('#MiddleName').val(data.MiddleName);
                $('#FirstName').val(data.FirstName);
                $('#Approved').val(data.Approved);
                $('#Locked').val(data.Locked); 
                $('#SuperUser').val(data.SuperUser);
                $('#Reversal').val(data.Reversal); 
                $('#IsManager').val(data.IsManager);
                $('.bootstrap-switch').removeClass("bootstrap-switch-on");
                $('.bootstrap-switch').removeClass("bootstrap-switch-off");
                if (data.Approved)
                {
                    $(".bootstrap-switch-id-Approved").addClass("bootstrap-switch-on");
                }
                else {
                    $(".bootstrap-switch-id-Approved").addClass("bootstrap-switch-off");
                }
                if (data.Locked) {
                    $(".bootstrap-switch-id-Locked").addClass("bootstrap-switch-on");
                }
                else {
                    $(".bootstrap-switch-id-Locked").addClass("bootstrap-switch-off");
                }
                if (data.SuperUser) {
                    $(".bootstrap-switch-id-SuperUser").addClass("bootstrap-switch-on");
                }
                else {
                    $(".bootstrap-switch-id-SuperUser").addClass("bootstrap-switch-off");
                }
                if (data.Reversal) {
                    $(".bootstrap-switch-id-Reversal").addClass("bootstrap-switch-on");
                }
                else {
                    $(".bootstrap-switch-id-Reversal").addClass("bootstrap-switch-off");
                }
                if (data.IsManager) {
                    $(".bootstrap-switch-id-IsManager").addClass("bootstrap-switch-on");
                }
                else {
                    $(".bootstrap-switch-id-IsManager").addClass("bootstrap-switch-off");
                }
                $('#AddUser').find('.modal-title').text("Edit User");
                $('#AddUser').modal('show')
            });

            $('#UserMasterTable tbody').on('click', '.btndel', function () {
                var count = $(this).data('count');
                var tabledata = $('#UserMasterTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                DeletePopup(data);
            }); 

           
        }

function AddUser() {
 
    var UserData = {
        UserID: $('#UserID').val()
        , Password: $('#Password').val()
        , LastName: $('#LastName').val()
        , MiddleName: $('#MiddleName').val()
        , FirstName: $('#FirstName').val()
        , Approved: ($('#Approved').is(":checked"))?true:false
        , Locked: ($('#Locked').is(":checked")) ? true : false
        , SuperUser: ($('#SuperUser').is(":checked"))?true:false
        , Reversal: ($('#Reversal').is(":checked"))?true:false
        , IsManager: ($('#IsManager').is(":checked"))?true:false

    }
    $.ajax({
        url: '../UserMaster/CreateUser',
        data: UserData,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddUser').modal('hide');
                msg("User was successfully saved.", "success");
            }
            else {
                msg("User already registered", "warning");
            }

        }
    });
}

function EditUser() {
 
    var UserData = {
        UserID: $('#UserID').val()
        , Password: $('#Password').val()
        , LastName: $('#LastName').val()
        , MiddleName: $('#MiddleName').val()
        , FirstName: $('#FirstName').val()
        , Approved: ($('.bootstrap-switch-id-Approved').hasClass("bootstrap-switch-on")) ? true : false
        , Locked: ($('.bootstrap-switch-id-Locked').hasClass("bootstrap-switch-on")) ? true : false
        , SuperUser:($('.bootstrap-switch-id-SuperUser').hasClass("bootstrap-switch-on")) ? true : false
        , Reversal: ($('.bootstrap-switch-id-Reversal').hasClass("bootstrap-switch-on")) ? true : false
        , IsManager: ($('.bootstrap-switch-id-IsManager').hasClass("bootstrap-switch-on")) ? true : false
        
    }
    $.ajax({
        url: '../UserMaster/EditUser',
        data: UserData,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddUser').modal('hide');
                msg("User was successfully saved.", "success");
            }
            else {
                msg("User already registered", "warning");

            }
        }
    });
}

function DeleteData(user) {
    var UserID = user.UserID;
    $.ajax({
        url: '../UserMaster/DeleteUser?UserID=' + UserID,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddUser').modal('hide');
                msg("Sites was successfully removed.", "success");
            }
            else {
                msg("Failed to remove Sites", "warning");

            }
        }
    });
}