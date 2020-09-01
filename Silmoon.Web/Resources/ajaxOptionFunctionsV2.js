
// Silmoon.Web.Mvc.MvcHelper.GetAjaxOptionJavascriptFunctionsV2() generate javascript
function _ajax_on_complete_v2(e, onSuccess, onError) {
    if (e.readyState == 4) {
        e.Data = JSON.parse(e.responseText);
        if (e.Data.Success) {
            if (onSuccess != null) onSuccess(e);
        } else {
            if (onFailed != null) onFailed(e);
        }
    }
    else {
        if (onError != null) onError(e);
    }
}

function _ajax_on_begin_v2(e, onBegin) {
    if (onBegin != null) onBegin(onBegin);
}
// Silmoon.Web.Mvc.MvcHelper.GetAjaxOptionJavascriptFunctionsV2() end javascript
