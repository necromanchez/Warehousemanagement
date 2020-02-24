$(document).ready(function () {
    initialized();
    demo.initDateTimePicker();
    BindDropdownList('CodeOwner', '/Helper/Dropdown_ClassOwner');
    BindDropdownList('CodeSupplier', '/Helper/Dropdown_ClassSupplier');
    BindDropdownList('CodeClassInbound', '/Helper/Dropdown_CodeClassInbound');
    BindDropdownList('SiteCode', '/Helper/Dropdown_Site');

    $('#frm_Inbound').on('submit', function (e) {
        e.preventDefault();
        if ($('#ID').val() == "") {
            AddInboundPlan($(this));
        }
        else {
            EditInboundPlan($(this));
        }
        
    });

    $('#AddInboundPlan').on('hidden.bs.modal', function (e) {
        initialized();
    })

    $('[data-toggle="tooltip"]').tooltip()

 
});


function initialized() {
            $('#InboundPlanTable').DataTable({
                ajax: {
                    url: '../InboundPlan/GetInboundPlanList',
                    type: "POST",
                    datatype: "json"
                },
                autoWidth:false,
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
                            console.log($('#siteycode').val());
                            $('#SiteCode').val($('#siteycode').val());
                            $('#AddInboundPlan').modal('show')
                            Clearall();
                            $('#AddInboundPlan').find('.modal-title').text("Add Inbound Plan");
                           
                        }
                    },
                ],
                destroy: true,
                columns: [
                    { title: "ID", data: "ID", visible: false },
                    { title: "CodeSupplier", data: "CodeSupplier", visible: false },
                    { title: "CodeClassInbound", data: "CodeClassInbound", visible: false },
                    { title: "PlanCode", data: "InboundPlanCode", name: "InboundPlanCode" },
                    {
                        title: "PlanDate", data: function (x) {
                            return moment(x.InboundPlanDate).format("MM/DD/YYYY")
                        }, name: "InboundPlanDate"
                    },
                    { title: "Supplier", data: "CodeSupplier", name: "CodeSupplier" },
                    { title: "Slip No", data: "SlipNo", name: "SlipNo" },
                    {
                        title: "Slip Date", data: function (x) {
                            return moment(x.SlipDate).format("MM/DD/YYYY")
                        }
                    , name: "SlipDate" },
                    { title: "Class", data: "CodeClassInbound", name: "CodeClassInbound" },
                    { title: "Status", data: "InboundStatus", name: "InboundStatus" },
                    { title: "PrintStatus", data: "PrintStatus", name: "PrintStatus" },
                    {
                         title: "Action",
                         render: function (data, type, full, meta) {
                             data = '<input type="hidden" id="withChange_' + meta.row + '" value="0" class="withChange_' + meta.row + '">'
                                 + '<button type="button" class="btn btn-warning btn-sm btn-flat btnedit_' + meta.row + ' btnedit" data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Modify">'
                                 + '<i class="fa fa-edit"></i>'
                                 + '</button><button type="button" class="btn btn-sm btn-danger btn-flat editinput btndel_' + meta.row + ' btndel " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Delete">'
                                 + '<i class="fa fa-trash-o"></i></button>'
                                 + '</button><button type="button" class="btn btn-sm btn btn-primary btn-flat editinput btnitem_' + meta.row + ' btnitem " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Add Items">'
                                 + '<i class="fa fa-dropbox"></i></button>'
                                 + '</button><button type="button" class="btn btn-sm btn btn-success btn-flat setInbound btncomplete_' + meta.row + ' btncomplete " data-count="' + meta.row + '" data-toggle="tooltip" data-placement="top" title="Complete">'
                                 + '<i class="fa fa-mail-forward"></i></button>'
                             return data;
                         }, searchable: false, orderable: false
                    },
                ],

            }).columns.adjust();

            $('#InboundPlanTable tbody').off('click');
            $('#InboundPlanTable tbody').on('click', '.btnedit', function () {
                var count = $(this).data('count');
                var tabledata = $('#InboundPlanTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();

                $('#SiteCode').val(data.SiteCode);
                $('#CodeOwner').val(data.CodeOwner);
                $('#InboundPlanDate').val(moment(data.InboundPlanDate).format("MM/DD/YYYY"));
                $('#CodeSupplier').val(data.CodeSupCode);
                $('#SlipClass').val(data.SlipClass);
                $('#SlipDate').val(moment(data.SlipDate).format("MM/DD/YYYY"));
                $('#SiteCode').val(data.SiteCode);
                $('#SlipNo').val(data.SlipNo);
                $('#CodeClassInbound').val(data.CodeClassInboundCode);
                $('#Remarks').val(data.Remarks);
              
                $('#ID').val(data.ID);
    
                $('#AddInboundPlan').find('.modal-title').text("Edit Inbound");
             
                $('#AddInboundPlan').modal('show')
            });

            $('#InboundPlanTable tbody').on('click', '.btndel', function () {
                var count = $(this).data('count');
                var tabledata = $('#InboundPlanTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                DeletePopup(data);
            });

            $('#InboundPlanTable tbody').on('click', '.btnitem', function () {
                var count = $(this).data('count');
                var tabledata = $('#InboundPlanTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                $('#InboundPlanCode').val(data.InboundPlanCode);
                $("#InboundItemModal").modal("show");
                initialized_inboundItem();
                initialized_inbounded();
                $("#frm_SettoInbound").hide();
            });

            $('#InboundPlanTable tbody').on('click', '.btncomplete', function () {
                var count = $(this).data('count');
                var tabledata = $('#InboundPlanTable').DataTable();
                var data = tabledata.row($(this).parents('tr')).data();
                $.ajax({
                    url: '../InboundPlan/VerifyInboundItems?InboundPlanCode=' + data.InboundPlanCode,
                    type: 'POST',
                    cache: false,
                    datatype: "json",
                    success: function (returnData) {
                        if (returnData.ok)
                        {
                       
                            CompleteINPlan(data.InboundPlanCode,"Complete");
                        }
                        else {
                            if (confirm("Continue Partial Inbound?")) {
                            CompleteINPlan(data.InboundPlanCode, "Partial");
                            }
                            else {
                                initialized();
                            }
                        }
                    }
                });
             
            });
          
}

function AddInboundPlan(data) {
    var datanow = data.serialize();
    datanow += '&SiteCode=' + $('#globalSite').val();
    $.ajax({
        url: '../InboundPlan/CreateInboundPlan',
        data: datanow,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddInboundPlan').modal('hide');
                msg("Inbound Plan was successfully saved.", "success");

                //CREATE Inventory
                $.ajax({
                    url: '/StockInventory/CreateStockInventory',
                    data: returnData.InboundPlan,
                    type: 'POST',
                    cache: false,
                    datatype: "json",
                    success: function (returnData) {
                        if (returnData.result == "success") {


                        }
                        else {

                        }

                    }
                });
            }
            else {
                msg("Inbound Plan already registered", "warning");
            }

        }
    });
}

function EditInboundPlan(data) {
    
    $.ajax({
        url: '../InboundPlan/EditInboundPlan',
        data: data.serialize(),
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                $('#AddInboundPlan').modal('hide');
                msg("Inbound Plan was successfully saved.", "success");
            }
            else {
                msg("Inbound Plan already registered", "warning");
            }
        }
    });
}

function DeleteData(code) {
    var Code = code.InboundPlanCode;
    $.ajax({
        url: '/InboundPlan/DeleteInboundPlan?InboundPlanCode=' + Code,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();

                msg("Inbound Plan was successfully removed.", "success");
            }
            else {
                msg("Failed to remove Inbound Plan", "warning");

            }
        }
    });
}

function CompleteINPlan(InboundPlanCode, Result) {
    $.ajax({
        url: '../InboundPlan/ToCompleteInbound?InboundPlanCode=' + InboundPlanCode +"&Result="+Result,
        type: 'POST',
        cache: false,
        datatype: "json",
        success: function (returnData) {
            if (returnData.result == "success") {
                initialized();
                Clearall();
                msg("Inbound Plan Completed", "success");
            }
            else {
                msg("Cannot Complete this Inbound Plan", "warning");
            }

        }
    });
}