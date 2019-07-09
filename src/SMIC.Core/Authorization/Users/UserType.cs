using System;
using System.Collections.Generic;
using System.Text;

namespace SMIC.Authorization.Users
{
    public enum UserType : short
    {
        /// <summary>
        /// 后台用户
        /// </summary>
        Backend = 0,
        /// <summary>
        /// 前台会员
        /// </summary>
        Frontend = 1
    }
}
