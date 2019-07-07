using System.Threading.Tasks;
using SMIC.Wechat;
using Castle.Core.Logging;
using WebApiClient;

namespace SMIC.Authentication.External.Wechat
{
    public class WechatAuthProviderApi : ExternalAuthProviderApiBase
    {
        public ILogger Logger { get; set; }

        public WechatAuthProviderApi()
        {
            Logger = NullLogger.Instance;
        }

        public override async Task<ExternalAuthUserInfo> GetUserInfo(string accessCode)
        {
            var client = HttpApiClient.Create<IWeChatApi>();

            var authResult = await client.AuthCodeAsync(ProviderInfo.ClientId, ProviderInfo.ClientSecret, accessCode);

            if (authResult.errcode == 0)
            {
                return new WechatAuthUserInfo
                {
                    EmailAddress = $"{authResult.openid}@AbpProjectName.com",
                    Name = $"{authResult.openid}",
                    Provider = ProviderInfo.Name,
                    ProviderKey = authResult.openid,
                    Surname = authResult.openid,
                    SessionKey = authResult.session_key,
                    OpenId = authResult.openid
                };
            }
            else
            {
                Logger.Error($"{GetType().FullName}:{authResult.errcode},{authResult.errmsg}");
                throw new AbpProjectNameBusinessException(ErrorCode.WechatAuthByCodeFailed);
            }
        }
    }
}
