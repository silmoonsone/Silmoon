
// Silmoon.Web.Mvc.MvcHelper.GetAjaxOptionJavascriptFunctionsV2() generate javascript
function _ajax_on_complete_v2(e, onCompleted, onError) {
    if (e.readyState == 4) {
        e.Data = JSON.parse(e.responseText);
        if (onCompleted != null) onCompleted(e);
    }
    else {
        if (onError != null) onError(e);
    }
}

function _ajax_on_begin_v2(e, onBegin) {
    if (onBegin != null) onBegin(e);
}
// Silmoon.Web.Mvc.MvcHelper.GetAjaxOptionJavascriptFunctionsV2() end javascript
