
// Silmoon.Web.Mvc.MvcHelper.GetAjaxOptionJavascriptFunctions() generate javascript
function _ajax_on_complete(senderId, e, onSuccess, onFailed, onError, onSuccessNeedRefreshPage) {
    if (e.readyState == 4 && e.status == 200) {
        if (senderId != "") $("#" + senderId).html(__ajax_Request_ButtonText);
        e.Data = JSON.parse(e.responseText);
        if (e.Data.Success) {
            //if (senderId != "") $("#" + senderId).html('成功');
            if (onSuccess != null) onSuccess(senderId, e);
            if (onSuccessNeedRefreshPage) {
                setTimeout(function () { location.reload() }, 1000);
                if (senderId != "") $("#" + senderId).html('刷新...');
            }
        } else {
            //if (senderId != "") $("#" + senderId).html('失败')
            if (onFailed != null) onFailed(senderId, e);
        }
    }
    else {
        //if (senderId != "") $("#" + senderId).html('错误')
        if (onError != null) onError(senderId, e);
    }
}

function _ajax_on_begin(senderId, onBegin) {
    __ajax_Request_ButtonText = "";
    if (senderId != "") {
        __ajax_Request_ButtonText = $("#" + senderId)[0].innerText;
    }
    if (senderId != "") $("#" + senderId).html("请求中...");
    if (onBegin != null) onBegin(senderId, onBegin);
}
// Silmoon.Web.Mvc.MvcHelper.GetAjaxOptionJavascriptFunctions() end javascript
