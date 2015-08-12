var callAjax = true;
//$.currentMenuId;
$(document).ready(function () {
    if (successMsg) {
        ShowInfoMessage(successMsg);
    }
    BindKWindow();
    BindNumericTextBox();
    $(".content").find(":text, textarea, select").filter(":visible:enabled").first().focus();
    BindWarehouseAutoComplete();
    BindVendorAutoComplete();
    BindCustomerAutoComplete();
    $("input:text").focus(function () { $(this).select(); });
    if ($("#actMenId").val() != 0)
    {
      
        $("#" + $("#actMenId").val()).click();
    }

    $(".datepicker").kendoDatePicker();
});
$(document).on("click", ".btcancel", function (e) {
    parent.history.back();
    return false;
});

$(document).on("click", ".btnClear", function () {
    $('input[type=text]').val('');
    $('input[type=text], textarea').val('');
});

$(document).on("click", ".btnclose", function (e) {
    $('*[data-role="window"]').data("kendoWindow").close();
});

function BindWarehouseAutoComplete()
{
    $(".warehouseauto").kendoComboBox({
        dataTextField: "Name",
        dataValueField: "WarehouseId",
        dataSource: {
            type: "json",
            transport: {
                read: warauto
            }
        },
        change: function (e) {
            var di = this.dataItem();
            if (!di) {
                this.value('');
            }
        },
        filter: "contains",
        suggest: true,
    });
}

function BindCustomerAutoComplete() {
    $(".customerauto").kendoComboBox({
        dataTextField: "Name",
        dataValueField: "CustomerId",
        dataSource: {
            type: "json",
            transport: {
                read: customerAuto
            }
        },
        change: function (e) {
            var di = this.dataItem();
            if (!di) {
                this.value('');
            }
        },
        filter: "contains",
        template: " #= CustomerCode # - #=Name #",
        suggest: true,
    });
}


function BindVendorAutoComplete() {
    $(".vendorauto").kendoComboBox({
        dataTextField: "Name",
        dataValueField: "VendorId",
        dataSource: {
            type: "json",
            transport: {
                read: vendorAuto
            }
        },
        change : function (e) {
            var di = this.dataItem();
            if (!di) {
                this.value('');
            }
        },
        filter: "contains",
        template: " #= VendorCode # - #=Name #",
        suggest: true,
    });
}

function BindNumericTextBox()
{
    $('.percentage').each(function (i, obj) {
        if ($(this).val() == undefined || $(this).val() == '') {
            $(this).val(0);
        }
    });
    $('.numeric').each(function (i, obj) {
        if ($(this).val() == undefined || $(this).val() == '' || $(this).val() == 0) {
            $(this).val(1);
        }
    });

    $('.currency').each(function (i, obj) {
        if ($(this).val() == undefined || $(this).val() == '') {
            $(this).val(0);
        }
    });
    $('.decimalNumeric').each(function (i, obj) {
        if ($(this).val() == undefined || $(this).val() == '') {
            $(this).val(0);
        }
    });


    $(".percentage").kendoNumericTextBox({
        format: "p0",
        min: 0,
        max: 0.1,
        step: 0.01
    });

    $(".numeric").kendoNumericTextBox({
        format: "{0:n0}",
        step: 1,
        min: 0,
    });

    $(".currency").kendoNumericTextBox({
        format: "c",
        step: 5,
        min: 0,
    });

    $(".decimalNumeric").kendoNumericTextBox({
        format: "{0:n2}",
        step: 5,
        min: 0,
    });

}

function BindKWindow()
{
    $(".kwindow").kendoWindow({
        title: $(this).attr('data-title'),
        visible: false,
        height: "60%",
        width: "60%",
        modal: true,
        actions: [
            "Close"
        ]
    });
}
function OpenModalWindow(windowId,winTitle) {
    var window = $('#' + windowId);
    window.data("kendoWindow").title(winTitle).open().center();
}
window.onload = function () {
    $("body").removeClass("loadstate");
};

$(".btcancel").on("click", function ()
{
    window.location = dashboardUrl;
});

$( document ).ajaxStart(function() {
});

$(document ).ajaxError(function() {
    //alert('there is an error while processing your request');
    //window.location = loginURL;
});


function CloseModalWindow(windowId) {
    var window = $('#' + windowId);
    window.data("kendoWindow").close();
}

function CallAjaxMethod(url, ajaxtype, data, callback)
{
        $.ajax({
            type: ajaxtype,
            url: url,
            data: data,
            success: callback
        });
}
function ShowInfoMessage(msg) {
    $("#spanmsg").text(msg);
    $("#sucessmsg").show();
    $("#sucessmsg").animate({ top: "0" }, 2000);
    $("#sucessmsg").fadeOut(15000);
}


function ShowErrorMessage(msg) {
    $("#spanermsg").text(msg);
    $("#errormsg").show();  
    $("#errormsg").animate({ top: "0" }, 2000);
    $("#errormsg").fadeOut(15000);

}

function LoadPartialViewForm(targetdivdivid, formId, url, callbackfunc)
{
    $.ajax({
        url: url,
        dataType: 'html',
        cache: false,
        async:false,
        success: function (data) {
            $('#' + targetdivdivid).html(data);
            $('#' + formId).removeData('validator');
            $('#' + formId).removeData('unobtrusiveValidation');
            $('#' + formId).each(function () { $.data($(this)[0], 'validator', false); }); //enable to display the error messages
            $.validator.unobtrusive.parse('#' + formId);
            $('#' + formId).data("validator").settings.ignore = ":hidden:not(.k-numerictextbox input)"
            if (callbackfunc)
                return callbackfunc();
        }
    });
}
function SubmitAjaxForm(formId, url, callBack) {
    $.ajax({
        url: url,
        type: 'post',
        cache: false,
        data: $('#' + formId).serialize(),
        success: function (data) {
            return callBack(data);
        },
    });
}