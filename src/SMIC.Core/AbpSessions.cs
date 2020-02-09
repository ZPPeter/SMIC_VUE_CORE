using Abp.Authorization.Users;
using Abp.Runtime.Session;
using SMIC.Authorization.Users;
using SMIC.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMIC
{
    public static class AbpSessions
    {
        private static Dictionary<long?, User> dict;

        /// <summary>
        /// 保存登录用户信息
        /// </summary>
        /// <param name="user"></param>
        public static void SaveUserToCache(User user)
        {
            if (dict == null) dict = new Dictionary<long?, User>();

            if (!dict.ContainsKey(user.Id))
            {
                dict.Add(user.Id, user);
            }
        }

        /// <summary>
        /// 获取当前登录用户名
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string GetUserName(this IAbpSession session)
        {
            return dict.ContainsKey(session.UserId)
                ? dict[session.UserId].UserName
                : string.Empty;
        }

        /*
        private static Dictionary<long?, AbpLoginResult<Tenant, User>> dict;

        /// <summary>
        /// 保存登录用户信息
        /// </summary>
        /// <param name="user"></param>
        public static void SaveUserToCache(AbpLoginResult<Tenant, User> user)
        {
            if (dict == null) dict = new Dictionary<long?, AbpLoginResult<Tenant, User>>();

            if (!dict.ContainsKey(user.User.Id))
            {
                dict.Add(user.User.Id, user);
            }
        }

        /// <summary>
        /// 获取当前登录用户名
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string GetUserName(this IAbpSession session)
        {
            return dict.ContainsKey(session.UserId)
                ? dict[session.UserId].User.UserName
                : string.Empty;
        }
        */
    }
}
