using Abp.Authorization.Users;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SMIC.Models.TokenAuth
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public long UserId { get; set; }

        public string SurName { get; set; }

        public string[] Roles { get; set; }

        public DateTime? LastReadNoticeTime { get; set; }
    }
}
