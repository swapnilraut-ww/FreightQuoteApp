/// <reference path="Common.js" />
$(document).ready(function () {
    BindVendorList("divVendorList");
});

$(document).on("click", ".chkALL", function (e) {
    if (this.checked) {
        $('.chkslctItem').each(function () {
            this.checked = true;
        });
    } else {
        $('.chkslctItem').each(function () {
            this.checked = false;
        });

    }
});

$(document).on("click", ".btnEdit", function (e) {
    window.location = vendor.editVendor + '?id=' + $(this).attr("id");
});

$(document).on("click", ".remove", function (e)
{
    var url = vendor.removeVendor + "?Id=" + $(this).attr("id");
    alertify.confirm("are you sure to remove this ?", function (e) {
        if (e) {
            CallAjaxMethod(url, 'Get', "", AfterRemoveVendor);
        } 
    });
});

function AfterRemoveVendor(data)
{
    alertify.success("vendor removed successfully");
    BindVendorList("divVendorList");

}
function BindVendorList(divId)
{
    $("#" + divId).kendoGrid({
        dataSource: {
            type: "json",
            transport: {
                read: vendor.vendorList
            },
            pageSize: 10
        },
        height: 450,
        sortable: true,
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        columns: [
             {
                 field: "LocationBarcode",
                 headerTemplate: "<input type='checkbox' class='chkALL' />",
                 template: '<input type="checkbox" id="chk#=VendorId#"  #= IsActive ? "title =Confirmed" : ""#  #= IsActive ? "checked =checked" : "" #   />',
                 width: 80
             },
            {
                field: "Name",
                title: "Name",
            },
            {
                field: "Address",
                title: "Address",
            },
            {
                field: "PhoneNumber",
                title: "Phone Number",
            },
             {
                 field: "Email",
                 title: "Email",
             }
        ]
    });

}