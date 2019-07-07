using Abp.Events.Bus;

namespace SMIC.Authentication.External.Wechat.Events
{
    public class WechatLoginSuccessEventData : EventData
    {
        public string SessionKey { get; set; }
        public long UserId { get; set; }
    }
}
