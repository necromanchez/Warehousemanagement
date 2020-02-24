$(document).ready(function () {
    var str = location.href.toLowerCase();
    
   
    $("#themenu li a").each(function () {
        if (str.indexOf(this.href.toLowerCase()) > -1) {
            $("li.active").removeClass("active");
            $(this).parent().addClass("active");
            $(this).parent("li").addClass("active");
            $(this).parent().parent().find('ul').addClass("active");
        }

        if (str.includes("mastermanagement"))
        {
            $('#ManagementList').parent("li").addClass("active");
            $('#ManagementList').addClass("collapse show");
        }
        else if(str.includes("inbound")) {
            $('#InboundMaster').parent("li").addClass("active");
            $('#InboundMaster').addClass("collapse show");
        }
        else if (str.includes("stockmanagement")) {
            $('#Stockmanagement').parent("li").addClass("active");
            $('#Stockmanagement').addClass("collapse show");
        }
        else if (str.includes("outbound")) {
            $('#Outbound').parent("li").addClass("active");
            $('#Outbound').addClass("collapse show");
        }

    });


})