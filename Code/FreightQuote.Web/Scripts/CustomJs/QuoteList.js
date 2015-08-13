/// <reference path="Common.js" />
var venderList;
var statusList = [
        { StatusId: "Open", Name: "Open" },
        { StatusId: "Send", Name: "Send" },
        { StatusId: "Received", Name: "Received" },
        { StatusId: "Shipped", Name: "Shipped" },
        { StatusId: "Completed", Name: "Completed" }
];
$(document).ready(function () {
    $.ajax({
        type: "get",
        url: "/Quote/GetVendors",
        success: function (data) {
            venderList = data;
            BindQuoteList("divQuoteList");
        }
    });

    $(document).on("click", ".btnEdit", function (e) {
        var url = quote.editQuote + '?id=' + $(this).attr("id");
        window.location = url;
    });

    $(document).on("click", ".removeMsg", function (e) {
        var url = quote.removeQuote + "?Id=" + $(this).attr("id");
        alertify.confirm("are you sure to remove this ?", function (e) {
            if (e) {
                CallAjaxMethod(url, 'Get', "", AfterRemoveQuote);
            }
        });
    });
});

function venderName(venderId) {
    for (var i = 0; i < venderList.length; i++) {
        if (venderList[i].VenderId == venderId) {
            return venderList[i].Name;
        }
    }

    return "";
}

function statusName(statusId) {
    for (var i = 0; i < statusList.length; i++) {
        if (statusList[i].StatusId == statusId) {
            return statusList[i].Name;
        }
    }

    return "";
}



function AfterRemoveQuote(data) {
    alertify.success("Quote removed successfully");
    BindQuoteList("divQuoteList");
}

function categoryDropDownEditor(container, options) {
    $('<input required data-text-field="Name" data-value-field="VenderId" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDropDownList({
            autoBind: false,
            dataSource: {
                type: "odata",
                transport: {
                    read: "/Quote/GetVendors"
                }
            }
        });
}
var _dateToString = function (date) {
    return kendo.toString(kendo.parseDate(date), "G");
};

function BindQuoteList(divId) {
    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: quote.quoteList,
                dataType: "json"
            },
            update: {
                url: quote.updateQuote,
                dataType: "json"
            },
            destroy: {
                url: quote.removeQuote,
                dataType: "json"
            },
            parameterMap: function (data, operation) {
                if (operation === "update" || operation === "create") {
                    data.ShipDate = _dateToString(data.ShipDate);
                    data.CreationDate = _dateToString(data.CreationDate);
                }
                return data;
            }
        },
        pageSize: 10,
        schema: {
            model: {
                id: "QuoteId",
                fields: {
                    ReferenceNo: { editable: false },
                    PickupLocation: { editable: true, validation: { required: true } },
                    DeliveryLocation: { editable: true, validation: { required: true } },
                    ShipDate: { editable: true, validation: { required: true }, type: "date", format: "{0:MM/dd/yyyy}" },
                    CreationDate: { editable: false, type: "date", format: "{0:MM/dd/yyyy}" },
                    Description: { editable: true, validation: { required: true } },
                    Comments: { editable: true },
                    VenderName: { editable: true, validation: { required: true } },
                    VenderId: { editable: true, validation: { required: true } },
                    Status: { editable: true, validation: { required: true } },
                }
            }
        }
    });

    $("#" + divId).kendoGrid({
        dataSource: dataSource,
        height: 450,
        sortable: true,
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        editable: "inline",
        columns: [
            {
                field: "ReferenceNo",
                title: "Ref #",
                width: 60,
            },
            {
                field: "PickupLocation",
                title: "Pickup Location",
                width: 100,
            },
            {
                field: "DeliveryLocation",
                title: "Delivery Location",
                width: 120,
            },
             {
                 field: "ShipDate",
                 title: "Ship Date",
                 width: 100,
                 type: "date",
                 format: "{0:MM/dd/yyyy}"
             },
             {
                 field: "CreationDate",
                 title: "Creation Date",
                 type: "date",
                 width: 90,
                 format: "{0:MM/dd/yyyy}"
             },
            {
                field: "Description",
                title: "Description",
                width: 100,
            },
            {
                field: "Comments",
                title: "Comments",
                width: 80,
            },
            {
                field: "VenderName",
                title: "Vender",
                width: 90,
                sortable: false,
                template: "#= venderName(VenderId) #", // the template shows the name corresponding to the VenderId field
                editor: function (container) { // use a dropdownlist as an editor
                    var input = $('<input id="VenderId" required data-text-field="Name" data-value-field="VenderId" name="VenderId">');
                    // append to the editor container 
                    input.appendTo(container);

                    // initialize a dropdownlist
                    input.kendoDropDownList({
                        dataTextField: "Name",
                        dataValueField: "VenderId",
                        dataSource: venderList,
                        optionLabel : "--Select--"
                    }).appendTo(container);
                }
            },
            {
                field: "Status",
                title: "Status",
                width: 100,
                template: "#= statusName(Status) #", // the template shows the name corresponding to the status field
                editor: function (container) { // use a dropdownlist as an editor
                    var input = $('<input id="Status" required data-text-field="Name" data-value-field="StatusId" name="Status">');
                    // append to the editor container 
                    input.appendTo(container);

                    // initialize a dropdownlist
                    input.kendoDropDownList({
                        dataTextField: "Name",
                        dataValueField: "StatusId",
                        dataSource: statusList,
                        optionLabel: "--Select--"
                    }).appendTo(container);
                }
            },
            { command: [{ name: "edit", text: "", width: "10px" }, { name: "destroy", text: "", width: "10px" }], title: "&nbsp;", width: "80px" }]
        //{
        //    field: "QuoteId",
        //    title: "Edit",
        //    width: 50,
        //    template: '<span id="#=QuoteId#" class="editMsg" ><img id="#=QuoteId#" class="btnEdit" src="/Content/Images/edit.jpg" style="cursor: pointer;" /></span>',
        //    sortable: false
        //},
        //{
        //    field: "QuoteId",
        //    title: "Delete",
        //    width: 60,
        //    template: '<span id="#=QuoteId#"  class="removeMsg" ><img id="#=QuoteId#" class="removeMsg" src="/Content/Images/deleteicon.png" style="cursor: pointer;" /></span>',
        //    sortable: false
        //}        
    });

}