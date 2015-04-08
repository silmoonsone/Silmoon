using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Types
{
    /// <summary>
    /// 表示一个通用的状态代码
    /// </summary>
    public enum StateCode
    {
        /// <summary>
        /// 出于系统保护的目的
        /// </summary>
        SYSTEM_PROTECT = -15,
        /// <summary>
        /// 系统级别的操作限制
        /// </summary>
        SYSTEM_LIMIT = -15,
        /// <summary>
        /// 服务器错误
        /// </summary>
        SERVER_ERROR = -13,
        /// <summary>
        /// 服务器失败
        /// </summary>
        SERVER_FAIL = -12,
        /// <summary>
        /// 失败
        /// </summary>
        FAIL = -12,
        /// <summary>
        /// 数量限制
        /// </summary>
        QUOIT_LIMIT = -11,
        /// <summary>
        /// 已经存在
        /// </summary>
        EXISTED = -10,
        /// <summary>
        /// 用户不存在
        /// </summary>
        USER_NOT_EXIST = -9,
        /// <summary>
        /// 不存在
        /// </summary>
        NOT_EXIST = -8,
        /// <summary>
        /// 用户限制
        /// </summary>
        USER_LIMIT = -7,
        /// <summary>
        /// 冲突
        /// </summary>
        CONFLICT = -6,
        /// <summary>
        /// 软件限制
        /// </summary>
        SOFT_LIMIT = -5,
        /// <summary>
        /// 参数错误
        /// </summary>
        PARAM_ERROR = -4,
        /// <summary>
        /// 错误
        /// </summary>
        ERROR = -3,
        /// <summary>
        /// 权限拒绝
        /// </summary>
        PERMISSION_REJECT = -2,
        /// <summary>
        /// 未登录
        /// </summary>
        NOT_LOGIN = -1,
        /// <summary>
        /// 无状态
        /// </summary>
        None = 0,
        /// <summary>
        /// 成功
        /// </summary>
        SUCCESS = 1,
        /// <summary>
        /// 多操作成功
        /// </summary>
        MULTI_SUCCESS = 2,
    }
}
