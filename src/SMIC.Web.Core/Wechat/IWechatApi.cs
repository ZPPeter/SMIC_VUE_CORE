﻿using System.Net.Http;
using WebApiClient;
using WebApiClient.Attributes;

namespace SMIC.Wechat
{
    [LogFilter]
    [HttpHost("https://api.weixin.qq.com")]
    public interface IWeChatApi : IHttpApi
    {
        [HttpGet("/sns/jscode2session")]
        [JsonReturn]
        ITask<AuthApiResult> AuthCodeAsync(string appid, string secret, string js_code, string grant_type = "authorization_code");

        //https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET
        [HttpGet("/cgi-bin/token")]
        [JsonReturn]
        ITask<AccessTokenResult> GetAccessTokenAsync(string appid, string secret, string grant_type = "client_credential");

        //https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=ACCESS_TOKEN
        [HttpPost("/wxa/getwxacodeunlimit")]
        ITask<HttpResponseMessage> GetWXCodeUnlimit(string access_token, [JsonContent]QrCodeBRequest request);
    }
}
