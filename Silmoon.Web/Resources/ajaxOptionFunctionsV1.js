
// .Web.Mvc.MvcHelper.GetAjaxOptionJavascriptFunctionsV1() generate javascript
function _ajax_on_complete_v1(senderId, e, onCompleted, onError, flag) {
    e.Flag = flag;
    if (e.readyState == 4 && e.status == 200) {
        if (senderId != "") $("#" + senderId).html(__ajax_Request_ButtonText);
        e.Data = JSON.parse(e.responseText);
        if (typeof (onCompleted) == "function") onCompleted(senderId, e);
    }
    else {
        //if (senderId != "") $("#" + senderId).html(__ajax_Request_ButtonText);
        if (senderId != "") $("#" + senderId).html("Error");
        if (typeof (onError) == "function") onError(senderId, e, flag);
    }
}

function _ajax_on_begin_v1(senderId, onBegin, flag) {
    __ajax_Request_ButtonText = "";
    if (senderId != "") {
        __ajax_Request_ButtonText = $("#" + senderId)[0].innerText;
    }
    if (senderId != "") $("#" + senderId).html("请求中...");
    if (typeof (onBegin) == "function") onBegin(senderId, onBegin, flag);
}
// .Web.Mvc.MvcHelper.GetAjaxOptionJavascriptFunctionsV1() end javascript
