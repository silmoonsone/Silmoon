
function _ajax_on_complete(senderId, e, onSuccess, onFailed, onError, onSuccessNeedRefreshPage) {
    if (e.readyState == 4) {
        var obj = JSON.parse(e.responseText);
        if (obj.Success) {
            $("#" + senderId).html('成功');
            if (onSuccess != null) onSuccess(senderId, e);
            if (onSuccessNeedRefreshPage) {
                setTimeout(function () { location.reload() }, 1000);
                $("#" + senderId).html('成功...刷新数据');
            }
        } else {
            $("#" + senderId).html('失败')
            if (onFailed != null) onFailed(senderId, e);
        }
    }
    else {
        $("#" + senderId).html('错误')
        if (onError != null) onError(senderId, e);
    }
}

function _ajax_on_begin(senderId) {
    $("#" + senderId).html("请求中...");
}