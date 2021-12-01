
// .Web.Mvc.MvcHelper.GetAjaxOptionJavascriptFunctionsV2() generate javascript
function _ajax_on_complete_v2(e, onCompleted, onError, flag) {
    if (e.readyState == 4 && e.status == 200) {
        e.Data = JSON.parse(e.responseText);
        if (typeof (onCompleted) == "function") onCompleted(e, flag);
    }
    else {
        if (onError != null) onError(e);
    }
}

function _ajax_on_begin_v2(e, onBegin, flag) {
    if (typeof (onBegin) == "function") onBegin(e);
}
// .Web.Mvc.MvcHelper.GetAjaxOptionJavascriptFunctionsV2() end javascript
