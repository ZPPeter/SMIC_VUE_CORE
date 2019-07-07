﻿using System.Threading.Tasks;
using Abp.Runtime.Caching;

namespace SMIC.Wechat.AccessToken
{
    public class WechatAccessTokenAccessor : IWechatAccessTokenAccessor
    {
        private readonly ICacheManager _cacheManager;
        public WechatAccessTokenAccessor(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        public async Task<WechatAccessToken> GetToken()
        {
            return new WechatAccessToken
            {
                Token = await _cacheManager.GetCache<string, string>(WechatConsts.WechatCacheName)
                .GetOrDefaultAsync(WechatConsts.AccessTokenCacheKey)
            };
        }
    }
}
