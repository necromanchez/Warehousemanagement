$(document).ready(function () {
    initialized();

    $('#frm_site').on('submit', function (e) {
        e.preventDefault();
        if ($('#Sitename').val() != "" &&
           $('#Address').val() != "") {
            if ($('#ID').val() == "") {
                AddSite();
            }
            else {
                EditSite();
            }
        }
    });

    $('#AddSites').on('hidden.bs.modal', function (e) {
        initialized();
    })
});


function initialized() {
    //$.ajax({
    //    url: '../SiteMaster/GetSiteList',
    //    type: 'POST',
    //    cache: false,
    //    datatype: "json",
    //    success: function (returnData) {
            $('#SiteMasterTable').DataTable({
                ajax: {
                    url: '../SiteMaster/GetSiteList',
                    type: "POST",
                    datatype: "json"
                },
                 
                serverSide: "true",
                order:[0,"asc"],
                processing: "true",
                language: {
                    "processing":"processing... please wait"
                },
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: '  Add  ',
                        className: 'btn btn-success btn-flat fa fa-plus',
                        action: function ( e, dt, node, config ) {
                            Clearall();
                            $('#AddSites').modal('show')
                            $('#AddSites').find('.modal-title').text("Add Site");
                        }
                    },
                ],
                destroy: true,
                //data: returnData.list,
                columns: [
                    { title: "SiteCode", data: "SiteCode", visible: false },
                    { title: "Site Name", data: "SiteName", name: "SiteName" },
                    { title: "Address", data: "Address", name: "Address" },
                     {
                         title: "Action",
                         render: function (data, type, full, meta) {
                             data = '<input type="hidden" id="withChange_' + meta.row + '" value="0" class="withChange_' + meta.row + '">'
                                 + '<button type="button" class="btn btn-warning btn-sm btn-flat btnedit_' + meta.row + ' btnedit" data-count="' + meta.row + '">'
                                 + '<i class="fa fa-edit"></i>'
                                  + '</button><button type="button" class="btn btn-sm btn-primary btn-flat editinput btncancel_' + meta.row + ' btncancel " data-count="' + meta.row + '" style="display:none">'
                                 + '<i class="fa fa-minus-circle"></i></button>'
                                 + '</button><button type="button" class="btn btn-sm btn-success btn-flat editinput btnsave_' + meta.row + ' btnsave " data-count="' + meta.row + '" style="display:none">'
                                 + '<i class="fa fa-check"></i></button>'
                                 + '</button><button type="button" class="btn btn-sm btn-danger btn-flat editinput btndel_' + meta.row + ' btndel " data-count="' + meta.row + '">'
                                 + '<i class="fa fa-trash-o"></i></button>'
                             return data;
                         }, searchable: false, orderable: false
                     },
                ],
               
            });
            $('#SiteMasterTable tbody').off('click');
            $('#SiteMasterTable tbody').on('click', '.btnedit', function () {
                var count = $(this).data('count');
                //$(".spnrow_" + count).hide();
                //$(".txtrow_" + count).show();
                //$(".btnedit_" + count).hide();
                //$(".btncancel_" + count).show();
                //$(".btnsave_" + count).show();
                //$("#withChange_" + count).val("1");
                var tabledata = $('#SiteMasterTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                $('#ID').val(data.SiteCode);
                $('#Sitename').val(data.SiteName);
                $('#Address').val(data.Address);
                $('#AddSites').find('.modal-title').text("Edit Site");
                $('#AddSites').modal('show')

            });

            $('#SiteMasterTable tbody').on('click', '.btncancel', function () {
                var count = $(this).data('count');
                $(".spnrow_" + count).show();
                $(".txtrow_" + count).hide();
                $(".btnedit_" + count).show();
                $(".btncancel_" + count).hide();
                $(".btnsave_" + count).hide();
                $("#withChange_" + count).val("0");
                initialized();
            });

            $('#SiteMasterTable tbody').on('click', '.btnsave', function () {
                var count = $(this).data('count');
                $(".spnrow_" + count).show();
                $(".txtrow_" + count).hide();
                $(".btnedit_" + count).show();
                $(".btncancel_" + count).hide();
                $(".btnsave_" + count).hide();
                $("#withChange_" + count).val("0");
                var tabledata = $('#SiteMasterTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                EditSite(data);
            });

            $('#SiteMasterTable tbody').on('click', '.btndel', function () {
                var count = $(this).data('count');
                var tabledata = $('#SiteMasterTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                DeletePopup(data);
            });

          
        }

function AddSite() {
    var SiteData = {
        SiteName: $('#Sitename').val()
        , Address: $('#Address').val()

    }
    $.ajax({
        url: '../SiteMaster/CreateSite',
        data: SiteData,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddSites').modal('hide');
                msg("Sites was successfully saved.", "success");
            }
            else {
                msg("Sites already registered", "warning");
            }

        }
    });
}

//function EditSite(Site) {
    //Site.SiteName = $("#row" + Site.SiteCode + "1").val();
    //Site.Address = $("#row" + Site.SiteCode + "2").val();

function EditSite() {
    var SiteData = {
        SiteName: $('#Sitename').val()
        , Address: $('#Address').val()
        , SiteCode:$('#ID').val()
    }
    $.ajax({
        url: '../SiteMaster/EditSite',
        data: SiteData,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddSites').modal('hide');
                msg("Sites was successfully saved.", "success");
            }
            else {
                msg("Sites already registered", "warning");

            }
        }
    });
}

function DeleteData(Site) {
    var SiteCode = Site.SiteCode;
    $.ajax({
        url: '../SiteMaster/DeleteSite?SiteCode=' + SiteCode,
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