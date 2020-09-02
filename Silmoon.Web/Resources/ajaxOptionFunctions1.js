
// Silmoon.Web.Mvc.MvcHelper.GetAjaxOptionJavascriptFunctionsV1() generate javascript
function _ajax_on_complete_v1(senderId, e, onCompleted, onError) {
    if (e.readyState == 4) {
        if (senderId != "") $("#" + senderId).html(__ajax_Request_ButtonText);
        e.Data = JSON.parse(e.responseText);
        if (onCompleted != null) onCompleted(senderId, e);
    }
    else {
        if (onError != null) onError(senderId, e);
    }
}

function _ajax_on_begin_v1(senderId, onBegin) {
    __ajax_Request_ButtonText = "";
    if (senderId != "") {
        __ajax_Request_ButtonText = $("#" + senderId)[0].innerText;
    }
    if (senderId != "") $("#" + senderId).html("请求中...");
    if (onBegin != null) onBegin(senderId, onBegin);
}
// Silmoon.Web.Mvc.MvcHelper.GetAjaxOptionJavascriptFunctionsV1() end javascript
